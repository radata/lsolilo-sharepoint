using System;
using System.Workflow.Activities;
using FPS.Evaluation.Core;
using LS.Holiday.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

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
            contentTypeId = HolidayContentTypes.ProjectLeaderTask.Id;
            var author = workflowProperties.Item[SPBuiltInFieldNames.CreatedBy];
            SPFieldUserValue employee = new SPFieldUserValue(workflowProperties.Web, workflowProperties.Item[SPBuiltInFieldNames.CreatedBy].ToString());
            var start = workflowProperties.Item[HolidaysFields.StartDate.Name];
            var end = workflowProperties.Item[HolidaysFields.EndDate.Name];
            SPFieldUserValue manager = new SPFieldUserValue(workflowProperties.Web, workflowProperties.Item[HolidaysFields.ProjectLeader.Name].ToString());
            taskProperties.AssignedTo = manager.User.LoginName;
            taskProperties.Title = string.Format("{0} ({1:d} - {2:d})", employee.User.Name, start, end);
        }

        private void taskChangedWhileActivity_Condition(object sender, ConditionalEventArgs e)
        {
            var changed = taskAfterProperties.ExtendedProperties[HolidaysFields.Decision.Guid];
            e.Result = changed == null;
        }

        private void processStatusChangeCodeActivity_ExecuteCode(object sender, EventArgs e)
        {
            bool approved = taskAfterProperties.ExtendedProperties[HolidaysFields.Decision.Guid].ToString() == HolidayDecision.Approve.ToString();

            if (approved)
            {
                workflowProperties.Item[HolidaysFields.Status.Name] = HolidayStatus.Approved.ToString();
                workflowProperties.Item.Update();
            }
            else
            {
                workflowProperties.Item[HolidaysFields.Status.Name] = HolidayStatus.Declined.ToString();
                workflowProperties.Item.Update();
            }
        }
    }
}
