using System;
using FPS.Core;

namespace LS.Holiday.Core
{
    public class HolidaysFields
    {
        public static FieldInformation Status = new FieldInformation(new Guid("{0D852C4D-AD57-4008-A4E4-1A6568E0FA15}"), "Status", "$Resources:LS.Holiday.Core.Resources,StatusColumnName");
        public static FieldInformation Decision = new FieldInformation(new Guid("{29439BF4-F1CA-4277-B6F0-A1947F591213}"), "Decision", "$Resources:LS.Holiday.Core.Resources,DecisionColumnName");
        public static FieldInformation StartDate = new FieldInformation(new Guid("{54980C38-C397-46DF-9E3F-4A4B659D67C0}"), "StartDate", "$Resources:LS.Holiday.Core.Resources,StartDateColumnName");
        public static FieldInformation EndDate = new FieldInformation(new Guid("{621F6446-3D87-4D93-A6C3-AEE110947796}"), "EndDate", "$Resources:LS.Holiday.Core.Resources,EndDateColumnName");
        public static FieldInformation ProjectLeader = new FieldInformation(new Guid("{8D6ECDAA-E93E-4079-AD3E-ADAD7FAA25E9}"), "ProjectLeader", "$Resources:LS.Holiday.Core.Resources,ProjectLeaderColumnName");
        public static FieldInformation HolidayDescription = new FieldInformation(new Guid("{6B18058B-6AFE-4800-8482-4A91A01EC7F4}"), "HolidayDescription", "$Resources:LS.Holiday.Core.Resources,DescriptionColumnName");
    }
}
