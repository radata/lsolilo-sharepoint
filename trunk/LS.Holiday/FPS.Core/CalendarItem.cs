using System;
using System.Net.Mail;

namespace FPS.Core
{
    /// <summary>
    /// Class used to create Calendar items.
    /// </summary>
    public class CalendarItem : ICloneable
    {
        #region Fields

        private string _summary;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime? Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value.IsNullOrEmpty() ? string.Empty : value.RemoveHtmlTags(); }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the name of the organizer.
        /// </summary>
        /// <value>
        /// The name of the organizer.
        /// </value>
        public string OrganizerName { get; set; }

        /// <summary>
        /// Gets or sets the organizer mail.
        /// </summary>
        /// <value>
        /// The organizer mail.
        /// </value>
        public string OrganizerMail { get; set; }

        /// <summary>
        /// Gets or sets the attendees.
        /// </summary>
        /// <value>
        /// The attendees.
        /// </value>
        public MailAddressCollection Attendees { get; set; }

        /// <summary>
        /// Gets or sets the recipient of the ToDo message.
        /// </summary>
        /// <value>
        /// Recipient email address.
        /// </value>
        public string Recipient { get; set; }

        /// <summary>
        /// Gets the start in long format.
        /// </summary>
        /// <value>The start in long format.</value>
        public string StartInLongFormat { get { return string.Format("{0} {1}", Start.GetValueOrDefault().ToLocalTime().ToLongDateString(), Start.GetValueOrDefault().ToLocalTime().ToLongTimeString()); } }

        /// <summary>
        /// Gets the end in long format.
        /// </summary>
        /// <value>The end in long format.</value>
        public string EndInLongFormat { get { return string.Format("{0} {1}", End.GetValueOrDefault().ToLocalTime().ToLongDateString(), End.GetValueOrDefault().ToLocalTime().ToLongTimeString()); } }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is cancelled.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the HTML description.
        /// </summary>
        /// <value>
        /// The HTML description.
        /// </value>
        public string HtmlDescription { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItem"/> class.
        /// </summary>
        public CalendarItem()
        {
            Attendees = new MailAddressCollection();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the removed meeting.
        /// </summary>
        public void SetRemovedMeeting()
        {
            IsCancelled = true;
            Title = string.Format("Canceled: {0}", Title);
            Summary = "You have been removed from this meeting.";
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            var itemParameters = new CalendarItem()
            {
                Id = Id,
                Start = Start,
                End = End,
                Title = Title,
                Summary = Summary,
                Location = Location,
                OrganizerMail = OrganizerMail,
                OrganizerName = OrganizerName,
            };

            foreach (var attendee in Attendees)
                itemParameters.Attendees.Add(new MailAddress(attendee.Address));

            return itemParameters;
        }

        #endregion
    }
}
