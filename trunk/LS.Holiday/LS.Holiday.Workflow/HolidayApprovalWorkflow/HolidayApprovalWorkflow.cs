using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace LS.Holiday.Workflow.HolidayApprovalWorkflow
{
    public sealed partial class HolidayApprovalWorkflow : SequentialWorkflowActivity
    {
        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public Guid taskId = default(System.Guid);
        public String contentTypeId = default(System.String);
        public SPWorkflowTaskProperties taskProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties taskAfterProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();

        public HolidayApprovalWorkflow()
        {
            InitializeComponent();
        }

        private void onHolidayWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
        {
        }

        private void createTaskWithPMTaskContentType_MethodInvoking(object sender, EventArgs e)
        {
            taskId = Guid.NewGuid();
            contentTypeId = "0x0108010026E5350277764EBB9850F108E812F91A";
            var title = workflowProperties.Item["Title"];
            var author = workflowProperties.Item["Author"];
            SPFieldUserValue employee = new SPFieldUserValue(workflowProperties.Web, workflowProperties.Item["Author"].ToString());
            var start = workflowProperties.Item["StartDate"];
            var end = workflowProperties.Item["EndDate"];
            SPFieldUserValue manager = new SPFieldUserValue(workflowProperties.Web, workflowProperties.Item["ProjectLeader"].ToString());
            taskProperties.AssignedTo = manager.User.LoginName;
            taskProperties.Title = string.Format("{0} ({1:d} - {2:d})", employee.User.Name, start, end);
        }

        private void taskChangedWhileActivity_Condition(object sender, ConditionalEventArgs e)
        {
            var decisionId = workflowProperties.TaskList.Fields["Decision"].Id;
            var changed = taskAfterProperties.ExtendedProperties[decisionId];
            e.Result = changed == null;
        }

        private void processStatusChangeCodeActivity_ExecuteCode(object sender, EventArgs e)
        {
            var decisionId = workflowProperties.TaskList.Fields["Decision"].Id;
            bool approved = taskAfterProperties.ExtendedProperties[decisionId].ToString() == "Approve";
            if (approved)
            {
                var status = workflowProperties.Item["Status"];
                workflowProperties.Item["Status"] = "Approved";
                workflowProperties.Item.Update();
            }
            else
            {
                var status = workflowProperties.Item["Status"];
                workflowProperties.Item["Status"] = "Declined";
                workflowProperties.Item.Update();
            }
        }
    }
}
