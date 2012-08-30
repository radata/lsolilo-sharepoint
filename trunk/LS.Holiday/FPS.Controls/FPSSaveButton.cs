using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPS.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    public class FPSSaveButton : SaveButton
    {
        #region Fields

        private const string ValidationIndicator = "AddOrUpdateItem";

        #endregion

        #region Methods

        protected override bool SaveItem()
        {
            try
            {
                return base.SaveItem();
            }
            catch (SPException ex)
            {
                if (ex.ToString().Contains(ValidationIndicator))
                {
                    var validator = ValidatorManager.ParseException(ex.Message);
                    var webPartMainZone = Parent.Parent.Parent.Parent.Controls[0];
                    var listIterator = webPartMainZone.Controls.OfType<FPSListFieldIterator>().FirstOrDefault();
                    var validatorControl = webPartMainZone.Controls.OfType<ItemValidationFailedMessage>().FirstOrDefault();

                    if (validator.FieldName.IsNullOrEmpty() && validatorControl != null)
                    {
                        // ItemValidationFailedMessage has to be replaced - changing control indexes will make form unable to save.
                        var index = webPartMainZone.Controls.IndexOf(validatorControl);
                        webPartMainZone.Controls.RemoveAt(index);
                        webPartMainZone.Controls.AddAt(index, new Label() { CssClass = SPConstants.ValidationClassCSS, Text = validator.Message });
                    }
                    else if (listIterator != null)
                        listIterator.SetFieldErrorMessage(validator.FieldName, validator.Message);
                    else
                        throw;

                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
