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
        public Guid createTaskWithPMTaskContentType_TaskId = default(System.Guid);
        public String createTaskWithPMTaskContentType_ContentTypeId = default(System.String);

        public HolidayApprovalWorkflow()
        {
            InitializeComponent();
        }

        private void onHolidayWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
        {
        }

        private void createTaskWithPMTaskContentType_MethodInvoking(object sender, EventArgs e)
        {
            createTaskWithPMTaskContentType_TaskId = Guid.NewGuid();
            createTaskWithPMTaskContentType_ContentTypeId = "0x0108010026E5350277764EBB9850F108E812F91A";

            createTaskWithPMTaskContentType_TaskProperties.Title = "Example title";
            //createTask1_TaskProperties1.AssignedTo = "fp\\fps_pm";
            createTaskWithPMTaskContentType_TaskProperties.ExtendedProperties["HolidayDescription"] = "Some description";
        }

        public SPWorkflowTaskProperties createTaskWithPMTaskContentType_TaskProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
    }
}
