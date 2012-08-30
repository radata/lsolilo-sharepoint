using System;
using System.Collections.Generic;
using System.Web;
using FPS.Core;
using FPS.Core.Enums;
using FPS.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    public class FPSListFieldIterator : ListFieldIterator
    {
        #region Fields

        private const string HiddenFieldsMessageForm = "/_layouts/HiddenFieldsMessage.aspx";
        private int _controlsToRenderCount = 0;
        private Dictionary<string, string> _errorMessages = new Dictionary<string, string>();
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the content type template.
        /// </summary>
        public string ContentTypeTemplateName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the field error message.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="message">The message.</param>
        public void SetFieldErrorMessage(string fieldName, string message)
        {
            if (_errorMessages.ContainsKey(fieldName))
                _errorMessages[fieldName] = message;
            else
                _errorMessages.Add(fieldName, message);
        }

        /// <summary>
        /// Gets the field error message.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Returns field name.</returns>
        public string GetFieldErrorMessage(string fieldName)
        {
            return fieldName.IsNullOrEmpty() || !_errorMessages.ContainsKey(fieldName) ? null : _errorMessages[fieldName];
        }

        protected override bool IsFieldExcluded(SPField field)
        {
            if (!HttpContext.Current.Request[SPConstants.AdminModeUrlParameter].IsNullOrEmpty() && field.ParentList.DoesUserHavePermissions(SPBasePermissions.ManageWeb))
            {
                _controlsToRenderCount++;
                return false;
            }

            var isExcluded = base.IsFieldExcluded(field);

            // This properties are null by default. If they won't be set in content type, field won't be shown in corresponding form
            if ((base.ControlMode == SPControlMode.Display && !field.ShowInDisplayForm.HasValue) ||
                (base.ControlMode == SPControlMode.Edit && !field.ShowInEditForm.HasValue) ||
                (base.ControlMode == SPControlMode.New && !field.ShowInNewForm.HasValue))
                return true;

            if (field.ReadOnlyField)
            {
                // Because it's not posible to override IsFieldExcluded (becouse of many internal fields) I'm checking if field is excluded becouse of Readonly property.
                field.ReadOnlyField = false;
                isExcluded = base.IsFieldExcluded(field) && isExcluded;
                field.ReadOnlyField = true;
            }

            // ContentType selection
            if (field.Type == SPFieldType.Computed && field.InternalName == SPConstants.ContentType)
            {
                var changeContentType = new FPSChangeContentType();
                changeContentType.DisplayName = field.Title;
                var filterItems = field.GetCustomizationValues<string>(CustomPropertyType.Filter) ?? new List<string>();
                var allowedContentTypes = new Dictionary<string, string>();

                foreach (var item in filterItems)
                {
                    var nameValuePair = item.Split('_');
                    if (!allowedContentTypes.ContainsKey(nameValuePair[0]))
                        allowedContentTypes.Add(nameValuePair[0], nameValuePair.Length > 1 ? nameValuePair[1] : null);
                }

                changeContentType.AllowedContentTypes = allowedContentTypes;

                if (!ContentTypeTemplateName.IsNullOrEmpty())
                    changeContentType.TemplateName = ContentTypeTemplateName;

                Controls.Add(changeContentType);
            }

            if (!isExcluded)
                _controlsToRenderCount++;

            return isExcluded;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (_controlsToRenderCount < 1 && !(List is SPDocumentLibrary))
                Context.Server.Transfer(HiddenFieldsMessageForm);
        }

        #endregion
    }
}