using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Diagnostics;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class ValidatorManager
    {
        #region Fields

        private static string _fieldMarker = "Field:";
        private static string _fieldSplitter = ";";

        #endregion

        #region Methods

        /// <summary>
        /// Validates the specified properties.
        /// </summary>
        /// <typeparam name="T">Validator implementation.</typeparam>
        /// <param name="properties">The properties.</param>
        /// <returns>Returns true if valid; otherwise false.</returns>
        public static bool Validate<T>(SPItemEventProperties properties) where T : ValidatorBase, new()
        {
            try
            {
                var validator = new T();
                validator.Validate(properties);

                properties.Cancel = !validator.IsValid;
                properties.ErrorMessage = GetExceptionMessage(validator.FieldName, validator.Message);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex, LogType.Error);
            }

            return !properties.Cancel;
        }

        /// <summary>
        /// Parses the exception message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns ValidatorBase.</returns>
        public static ValidatorBase ParseException(string message)
        {
            var validator = new ValidatorBase();
            validator.Message = GetMessageFromExceptionMessage(message);
            validator.FieldName = GetFieldNameFromExceptionMessage(message);
            return validator;
        }

        private static string GetExceptionMessage(string fieldName, string message)
        {
            return fieldName.IsNullOrEmpty() ? message : string.Concat(_fieldMarker, fieldName, _fieldSplitter, message);
        }

        private static string GetFieldNameFromExceptionMessage(string exceptionMessage)
        {
            if (exceptionMessage.IsNullOrEmpty() || !exceptionMessage.StartsWith(_fieldMarker))
                return null;

            return exceptionMessage.Split(_fieldSplitter.ToCharArray()).FirstOrDefault().Replace(_fieldMarker, string.Empty).Trim();
        }

        private static string GetMessageFromExceptionMessage(string exceptionMessage)
        {
            if (exceptionMessage.IsNullOrEmpty() || !exceptionMessage.StartsWith(_fieldMarker))
                return exceptionMessage;

            int splitterIndex = exceptionMessage.IndexOf(_fieldSplitter);
            return splitterIndex >= 0 ? exceptionMessage.Substring(splitterIndex + 1) : exceptionMessage;
        }

        #endregion
    }
}
