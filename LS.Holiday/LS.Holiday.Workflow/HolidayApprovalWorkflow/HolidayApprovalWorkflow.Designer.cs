using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace LS.Holiday.Workflow.HolidayApprovalWorkflow
{
    public sealed partial class HolidayApprovalWorkflow
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            this.createTaskWithPMTaskContentType = new Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType();
            this.onHolidayWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // createTaskWithPMTaskContentType
            // 
            activitybind1.Name = "HolidayApprovalWorkflow";
            activitybind1.Path = "createTaskWithPMTaskContentType_ContentTypeId";
            correlationtoken1.Name = "taskToken";
            correlationtoken1.OwnerActivityName = "HolidayApprovalWorkflow";
            this.createTaskWithPMTaskContentType.CorrelationToken = correlationtoken1;
            this.createTaskWithPMTaskContentType.ListItemId = -1;
            this.createTaskWithPMTaskContentType.Name = "createTaskWithPMTaskContentType";
            this.createTaskWithPMTaskContentType.SpecialPermissions = null;
            activitybind2.Name = "HolidayApprovalWorkflow";
            activitybind2.Path = "createTaskWithPMTaskContentType_TaskId";
            activitybind3.Name = "HolidayApprovalWorkflow";
            activitybind3.Path = "createTaskWithPMTaskContentType_TaskProperties";
            this.createTaskWithPMTaskContentType.MethodInvoking += new System.EventHandler(this.createTaskWithPMTaskContentType_MethodInvoking);
            this.createTaskWithPMTaskContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.createTaskWithPMTaskContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.ContentTypeIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.createTaskWithPMTaskContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            activitybind5.Name = "HolidayApprovalWorkflow";
            activitybind5.Path = "workflowId";
            // 
            // onHolidayWorkflowActivated
            // 
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "HolidayApprovalWorkflow";
            this.onHolidayWorkflowActivated.CorrelationToken = correlationtoken2;
            this.onHolidayWorkflowActivated.EventName = "OnWorkflowActivated";
            this.onHolidayWorkflowActivated.Name = "onHolidayWorkflowActivated";
            activitybind4.Name = "HolidayApprovalWorkflow";
            activitybind4.Path = "workflowProperties";
            this.onHolidayWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onHolidayWorkflowActivated_Invoked);
            this.onHolidayWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.onHolidayWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // HolidayApprovalWorkflow
            // 
            this.Activities.Add(this.onHolidayWorkflowActivated);
            this.Activities.Add(this.createTaskWithPMTaskContentType);
            this.Name = "HolidayApprovalWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType createTaskWithPMTaskContentType;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onHolidayWorkflowActivated;









    }
}
