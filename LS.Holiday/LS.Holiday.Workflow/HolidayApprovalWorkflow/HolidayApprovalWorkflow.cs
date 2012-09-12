using System;
using System.Workflow.Activities;
using FPS.Evaluation.Core;
using LS.Holiday.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using LS.Holiday.Core.Strings;

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

        private void OnHolidayWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
        {
        }

        private void CreateTaskWithPMTaskContentType_MethodInvoking(object sender, EventArgs e)
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

        private void TaskChangedWhileActivity_Condition(object sender, ConditionalEventArgs e)
        {
            var changed = taskAfterProperties.ExtendedProperties[HolidaysFields.Decision.Guid].ToString();
            e.Result = string.IsNullOrEmpty(changed);
        }

        private void ProcessStatusChangeCodeActivity_ExecuteCode(object sender, EventArgs e)
        {
            bool approved = taskAfterProperties.ExtendedProperties[HolidaysFields.Decision.Guid].ToString() == HolidayDecision.Approve.ToString();

            if (approved)
            {
                workflowProperties.Item[HolidaysFields.Status.Name] = HolidayStatus.Approved.ToString();
                workflowProperties.Item[HolidaysFields.HolidayDescription.Guid] = taskAfterProperties.ExtendedProperties[HolidaysFields.HolidayDescription.Guid];
                workflowProperties.Item.Update();
                SendActionNotification(true);
                SendAccountancyNotification();
                AddInvitationsToCalendars();
            }
            else
            {
                workflowProperties.Item[HolidaysFields.Status.Name] = HolidayStatus.Declined.ToString();
                workflowProperties.Item[HolidaysFields.HolidayDescription.Guid] = taskAfterProperties.ExtendedProperties[HolidaysFields.HolidayDescription.Guid];
                workflowProperties.Item.Update();
                SendActionNotification(false);
            }
        }

        /// <summary>
        /// Sends the action notification.
        /// </summary>
        /// <param name="approved">If set to <c>true</c> send notification about approval, otherwise about rejection.</param>
        private void SendActionNotification(bool approved)
        {
            string subject = Values.LeaderResponseMailHeader;
            string decisionName = approved ? HolidayStatus.Approved.ToString() : HolidayStatus.Declined.ToString();
            string itemLink = EmailHelper.GenerateHolidayLink(workflowProperties.Web, workflowProperties.ItemId, decisionName);
            string content = string.Format(Values.LeaderResponseMailContent, itemLink);
            string serviceMail = Values.HolidayServiceMailAddress;
            SPFieldUserValue employee = new SPFieldUserValue(workflowProperties.Web, workflowProperties.Item[SPBuiltInFieldNames.CreatedBy].ToString());
            EmailHelper.SendEmail(workflowProperties.Web, serviceMail, employee.User.Email, subject, content);
        }

        /// <summary>
        /// Sends the accountancy notification.
        /// </summary>
        private void SendAccountancyNotification()
        {
            string title = workflowProperties.Item[SPBuiltInFieldNames.Title] as string;
            string subject = Values.AccountancyNotificationMailHeader;
            string holidayLink = EmailHelper.GenerateHolidayLink(workflowProperties.Web, workflowProperties.ItemId, title);
            string content = string.Format(Values.AccountancyNotificationMailContent, holidayLink);
            string serviceMail = Values.HolidayServiceMailAddress;
            string accountancyMail = Values.AccountancyMailAddress;
            EmailHelper.SendEmail(workflowProperties.Web, serviceMail, accountancyMail, subject, content);
        }

        /// <summary>
        /// Adds the invitations to public and private calendars.
        /// </summary>
        private void AddInvitationsToCalendars()
        {
            string title = string.Format(Values.CalendarMailHeader, workflowProperties.Item[SPBuiltInFieldNames.Title]);
            SPFieldUserValue employee = new SPFieldUserValue(workflowProperties.Web, workflowProperties.Item[SPBuiltInFieldNames.CreatedBy].ToString());
            string organizer = employee.User.Name;
            string organizerAddress = employee.User.Email;
            string privateCalendar = employee.User.Email;
            string publicCalendar = Values.PublicCalendarMailAddress;
            DateTime? start = workflowProperties.Item[HolidaysFields.StartDate.Name] as DateTime?;
            DateTime? end = workflowProperties.Item[HolidaysFields.EndDate.Name] as DateTime?;
            CalendarHelper.SendCalendarInvitation(workflowProperties.Web, organizer, organizerAddress, publicCalendar, start, end, title);
            CalendarHelper.SendCalendarInvitation(workflowProperties.Web, organizer, organizerAddress, privateCalendar, start, end, title);
        }
    }
}
