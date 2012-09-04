using System;
using FPS.Core;
using Microsoft.SharePoint;

namespace LS.Holiday.Core
{
    public class HolidayContentTypes
    {
        public static ContentTypeInformation<Fields.HolidayFields> Holiday = new ContentTypeInformation<Fields.HolidayFields>("0x0100119F2245F9444258B7A642E6560B9EE3", "Holiday", new Fields.HolidayFields());
        public static ContentTypeInformation<Fields.ProjectLeaderTaskFields> ProjectLeaderTask = new ContentTypeInformation<Fields.ProjectLeaderTaskFields>("0x0108010026E5350277764EBB9850F108E812F91A", "ProjectLeaderTask", new Fields.ProjectLeaderTaskFields());

        public class Fields
        {
            public class HolidayFields
            {
                public FieldInformation Title = new FieldInformation(SPBuiltInFieldId.Title, "Title", string.Empty);

                public FieldInformation StartDate { get { return HolidaysFields.StartDate; } }

                public FieldInformation EndDate { get { return HolidaysFields.EndDate; } }

                public FieldInformation HolidayDescription { get { return HolidaysFields.HolidayDescription; } }

                public FieldInformation ProjectLeader { get { return HolidaysFields.ProjectLeader; } }

                public FieldInformation Status { get { return HolidaysFields.Status; } }
            }

            public class ProjectLeaderTaskFields
            {
                public FieldInformation Title = new FieldInformation(SPBuiltInFieldId.Title, "Title", string.Empty);

                public FieldInformation Decision { get { return HolidaysFields.Decision; } }

                public FieldInformation Description { get { return HolidaysFields.HolidayDescription; } }
            }
        }
    }
}
