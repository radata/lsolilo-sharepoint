using System;
using Microsoft.SharePoint;

namespace LS.Delegations.Generated
{
    public class DelegationsContentTypes
    {
        public static ContentTypeInformation<Fields.AccommodationFields> Accommodation = new ContentTypeInformation<Fields.AccommodationFields>("0x0100808EED08F40D47A6B55319295FFB72E1", "Accommodation", new Fields.AccommodationFields());
        public static ContentTypeInformation<Fields.DelegationFields> Delegation = new ContentTypeInformation<Fields.DelegationFields>("0x0100735281EF6A774E1EBF6A0EFE8FA3BD6D", "Delegation", new Fields.DelegationFields());
        public static ContentTypeInformation<Fields.MonthlyReportFields> MonthlyReport = new ContentTypeInformation<Fields.MonthlyReportFields>("0x010084534150162D41F2957E6D795FB00A5E", "MonthlyReport", new Fields.MonthlyReportFields());
        public static ContentTypeInformation<Fields.PlaceFields> Place = new ContentTypeInformation<Fields.PlaceFields>("0x01006FA93EAAF2CA4638BCEBC9F2B026639C", "Place", new Fields.PlaceFields());
        public static ContentTypeInformation<Fields.TransportFields> Transport = new ContentTypeInformation<Fields.TransportFields>("0x01000D9BA4EE8E3643C5B59C571BEE40F5B4", "Transport", new Fields.TransportFields());

        public class Fields
        {
            public class AccommodationFields
            {
                public FieldInformation Place { get { return DelegationsFields.Place; } }

                public FieldInformation Location { get { return DelegationsFields.Location; } }

                public FieldInformation Days { get { return DelegationsFields.Days; } }

                public FieldInformation Cost { get { return DelegationsFields.Cost; } }

                public FieldInformation Delegation { get { return DelegationsFields.Delegation; } }
            }

            public class DelegationFields
            {
                public FieldInformation Reason { get { return DelegationsFields.Reason; } }

                public FieldInformation Place { get { return DelegationsFields.Place; } }

                public FieldInformation Start { get { return DelegationsFields.Start; } }

                public FieldInformation End { get { return DelegationsFields.End; } }

                public FieldInformation DelegationNote { get { return DelegationsFields.DelegationNote; } }

                public FieldInformation DelegationStatus { get { return DelegationsFields.DelegationStatus; } }

                public FieldInformation Title = new FieldInformation(SPBuiltInFieldId.Title, "Title", string.Empty);
            }

            public class MonthlyReportFields
            {
                public FieldInformation Title = new FieldInformation(SPBuiltInFieldId.Title, "Title", string.Empty);

                public FieldInformation MonthIndicator { get { return DelegationsFields.MonthIndicator; } }

                public FieldInformation MonthlyCost { get { return DelegationsFields.MonthlyCost; } }
            }

            public class PlaceFields
            {
                public FieldInformation City { get { return DelegationsFields.City; } }
            }

            public class TransportFields
            {
                public FieldInformation From { get { return DelegationsFields.From; } }

                public FieldInformation Destination { get { return DelegationsFields.Destination; } }

                public FieldInformation Distance { get { return DelegationsFields.Distance; } }

                public FieldInformation TravelTime { get { return DelegationsFields.TravelTime; } }

                public FieldInformation Cost { get { return DelegationsFields.Cost; } }

                public FieldInformation Transport { get { return DelegationsFields.Transport; } }

                public FieldInformation Delegation { get { return DelegationsFields.Delegation; } }
            }
        }
    }
}
