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
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            this.onPMTaskChanged = new Microsoft.SharePoint.WorkflowActions.OnTaskChanged();
            this.taskChangedSequenceActivity = new System.Workflow.Activities.SequenceActivity();
            this.deletePMTask = new Microsoft.SharePoint.WorkflowActions.DeleteTask();
            this.processStatusChangeCodeActivity = new System.Workflow.Activities.CodeActivity();
            this.taskChangedWhileActivity = new System.Workflow.Activities.WhileActivity();
            this.createTaskWithPMTaskContentType = new Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType();
            this.onHolidayWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // onPMTaskChanged
            // 
            activitybind1.Name = "HolidayApprovalWorkflow";
            activitybind1.Path = "taskAfterProperties";
            this.onPMTaskChanged.BeforeProperties = null;
            correlationtoken1.Name = "taskToken";
            correlationtoken1.OwnerActivityName = "HolidayApprovalWorkflow";
            this.onPMTaskChanged.CorrelationToken = correlationtoken1;
            this.onPMTaskChanged.Executor = null;
            this.onPMTaskChanged.Name = "onPMTaskChanged";
            activitybind2.Name = "HolidayApprovalWorkflow";
            activitybind2.Path = "taskId";
            this.onPMTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onPMTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // taskChangedSequenceActivity
            // 
            this.taskChangedSequenceActivity.Activities.Add(this.onPMTaskChanged);
            this.taskChangedSequenceActivity.Name = "taskChangedSequenceActivity";
            // 
            // deletePMTask
            // 
            this.deletePMTask.CorrelationToken = correlationtoken1;
            this.deletePMTask.Name = "deletePMTask";
            activitybind3.Name = "HolidayApprovalWorkflow";
            activitybind3.Path = "taskId";
            this.deletePMTask.SetBinding(Microsoft.SharePoint.WorkflowActions.DeleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // processStatusChangeCodeActivity
            // 
            this.processStatusChangeCodeActivity.Name = "processStatusChangeCodeActivity";
            this.processStatusChangeCodeActivity.ExecuteCode += new System.EventHandler(this.processStatusChangeCodeActivity_ExecuteCode);
            // 
            // taskChangedWhileActivity
            // 
            this.taskChangedWhileActivity.Activities.Add(this.taskChangedSequenceActivity);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.taskChangedWhileActivity_Condition);
            this.taskChangedWhileActivity.Condition = codecondition1;
            this.taskChangedWhileActivity.Name = "taskChangedWhileActivity";
            // 
            // createTaskWithPMTaskContentType
            // 
            activitybind4.Name = "HolidayApprovalWorkflow";
            activitybind4.Path = "contentTypeId";
            this.createTaskWithPMTaskContentType.CorrelationToken = correlationtoken1;
            this.createTaskWithPMTaskContentType.ListItemId = -1;
            this.createTaskWithPMTaskContentType.Name = "createTaskWithPMTaskContentType";
            this.createTaskWithPMTaskContentType.SpecialPermissions = null;
            activitybind5.Name = "HolidayApprovalWorkflow";
            activitybind5.Path = "taskId";
            activitybind6.Name = "HolidayApprovalWorkflow";
            activitybind6.Path = "taskProperties";
            this.createTaskWithPMTaskContentType.MethodInvoking += new System.EventHandler(this.createTaskWithPMTaskContentType_MethodInvoking);
            this.createTaskWithPMTaskContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.createTaskWithPMTaskContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.ContentTypeIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.createTaskWithPMTaskContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            activitybind8.Name = "HolidayApprovalWorkflow";
            activitybind8.Path = "workflowId";
            // 
            // onHolidayWorkflowActivated
            // 
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "HolidayApprovalWorkflow";
            this.onHolidayWorkflowActivated.CorrelationToken = correlationtoken2;
            this.onHolidayWorkflowActivated.EventName = "OnWorkflowActivated";
            this.onHolidayWorkflowActivated.Name = "onHolidayWorkflowActivated";
            activitybind7.Name = "HolidayApprovalWorkflow";
            activitybind7.Path = "workflowProperties";
            this.onHolidayWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onHolidayWorkflowActivated_Invoked);
            this.onHolidayWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.onHolidayWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // HolidayApprovalWorkflow
            // 
            this.Activities.Add(this.onHolidayWorkflowActivated);
            this.Activities.Add(this.createTaskWithPMTaskContentType);
            this.Activities.Add(this.taskChangedWhileActivity);
            this.Activities.Add(this.processStatusChangeCodeActivity);
            this.Activities.Add(this.deletePMTask);
            this.Name = "HolidayApprovalWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.OnTaskChanged onPMTaskChanged;

        private SequenceActivity taskChangedSequenceActivity;

        private Microsoft.SharePoint.WorkflowActions.DeleteTask deletePMTask;

        private CodeActivity processStatusChangeCodeActivity;

        private WhileActivity taskChangedWhileActivity;

        private Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType createTaskWithPMTaskContentType;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onHolidayWorkflowActivated;

















    }
}
