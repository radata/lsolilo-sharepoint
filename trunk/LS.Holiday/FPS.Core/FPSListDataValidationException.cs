using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public class FPSListDataValidationException : SPListDataValidationException
    {
        public FPSListDataValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public static FPSListDataValidationException ThrowSPListDataValidationException(SPFailure[] fieldFailures, SPFailure itemFailure)
        {
            var failture = new Failure() { spldvFieldFailures = fieldFailures, spldvItemFailure = itemFailure };

            var serializationInfo = new SerializationInfo(failture.GetType(), new FormatterConverter());
            serializationInfo.AddValue("spldvFieldFailures", fieldFailures);
            serializationInfo.AddValue("spldvItemFailure", 1);
            serializationInfo.AddValue("ClassName", string.Empty);
            serializationInfo.AddValue("Message", string.Empty);
            serializationInfo.AddValue("InnerException", new ArgumentException());
            serializationInfo.AddValue("HelpURL", string.Empty);
            serializationInfo.AddValue("StackTraceString", string.Empty);
            serializationInfo.AddValue("RemoteStackTraceString", string.Empty);
            serializationInfo.AddValue("RemoteStackIndex", 0);
            serializationInfo.AddValue("ExceptionMethod", string.Empty);
            serializationInfo.AddValue("HResult", 1);
            serializationInfo.AddValue("Source", string.Empty);
            serializationInfo.AddValue("nativeErrorMessage", string.Empty);
            serializationInfo.AddValue("nativeStackTrace", string.Empty);

            return new FPSListDataValidationException(serializationInfo, new StreamingContext());
        }

        private class Failure
        {
            internal SPFailure[] spldvFieldFailures;
            internal SPFailure spldvItemFailure;
        }

        //private class FPSFailure : SPFailure
        //{
        //    public FPSFailure(SPListDataValidationException.SPValidationType validationType, SPListDataValidationException.SPReason reason, string name, string displayName, string message)
        //    {

        //    }
        //}
    }
}
