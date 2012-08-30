using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPS.Core;
using FPS.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    public class FPSFormField : FormField
    {
        #region Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!HttpContext.Current.Request[SPConstants.AdminModeUrlParameter].IsNullOrEmpty() && Field.ParentList.DoesUserHavePermissions(SPBasePermissions.ManageWeb) && ControlMode == SPControlMode.Edit)
                ControlMode = SPControlMode.Edit;
            else if (Field.ReadOnlyField && ControlMode != SPControlMode.New)
                ControlMode = SPControlMode.Display;

            var value = HttpContext.Current.Request[Field.Title];
            if (!value.IsNullOrEmpty() && Value != null)
            {
                try
                {
                    Value = Convert.ChangeType(value, Value.GetType());

                    if (Field is SPFieldUser)
                    {
                        // Creating user link
                        var displayName = (Field.GetFieldValue(Value as string) as SPFieldUserValue).User.Name;
                        var lookupField = string.Format("{0}{1}{2}", Value, SPConstants.LookupFieldSplitter, displayName);
                        var valueControl = new LiteralControl() { Text = Field.GetFieldValueAsHtml(lookupField) };
                        Controls.Add(valueControl);

                        // This is used to hide PeoplePicker only. Indexes are related with template position.
                        Controls[0].Controls[0].Controls[3].Visible = false;
                    }

                    if (Field is SPFieldLookup)
                    {
                        // Indexes are related with template position. Search would be much slower.
                        var dropDownList = Controls[0].Controls[0] as DropDownList;
                        dropDownList.Items.Cast<ListItem>().ForEach(item => item.Enabled = item.Value == Value as string);
                        dropDownList.Enabled = false;
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log(exception, LogType.Error);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var listFieldIterator = Parent.Parent.Parent.Parent as FPSListFieldIterator;
            if (listFieldIterator != null)
            {
                var errorMessage = listFieldIterator.GetFieldErrorMessage(Field.InternalName);
                if (!errorMessage.IsNullOrEmpty())
                {
                    var errorMessageLabel = new Label() { CssClass = SPConstants.ValidationClassCSS, Text = errorMessage, };
                    var errorMessagePanel = new Panel();
                    errorMessagePanel.Controls.Add(errorMessageLabel);
                    Controls.Add(errorMessagePanel);
                }
            }
        }

        #endregion
    }
}
