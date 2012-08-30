using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPS.Core;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    public class FPSSaveAsDraft : SaveButton
    {
        #region Constants

        private const string SaveDraftCommand = "FPSSaveDraft";
        private const string SaveItemCommand = "SaveItem";

        private static readonly string ButtonText = "Save as Draft";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the default rendering template for the button.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that names a rendering template in an .ascx file in the folder C:\program files\common files\microsoft shared\web server extensions\12\template\controltemplates.</returns>
        protected override string DefaultTemplateName
        {
            get
            {
                return "FPSSaveAsDraftButton";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles or re-raises a bubbled event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        /// <returns>
        /// true if the event is handled; otherwise, false (in which case the event is bubbled up to the parent control). The default is false.
        /// </returns>
        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            var commandEventArgs = e as CommandEventArgs;
            if (commandEventArgs.CommandName == SaveDraftCommand)
            {
                var clonedValidatorList = new List<IValidator>(Page.Validators.Cast<IValidator>());

                foreach (IValidator validator in clonedValidatorList)
                    Page.Validators.Remove(validator);

                var returnValue = base.OnBubbleEvent(source, new CommandEventArgs(SaveItemCommand, commandEventArgs.CommandArgument));

                foreach (IValidator validator in clonedValidatorList)
                    Page.Validators.Add(validator);

                return returnValue;
            }

            return false;
        }

        /// <summary>
        /// Saves a list item and checks it in and uploads it when the list is a document library.
        /// </summary>
        /// <returns>
        /// True if the operation is successful; otherwise, false. The default implementation always returns true. See the Remarks section.
        /// </returns>
        protected override bool SaveItem()
        {
            // call event receiver manager to disable event firing while saving - enables to save the state without the workflow moving on
            using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
            {
                return base.SaveItem();
            }
        }

        /// <summary>
        /// Represents the method that handles the <see cref="E:System.Web.UI.Control.PreRender"/> event of the <see cref="T:Microsoft.SharePoint.WebControls.SaveButton"/>.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            Text = ButtonText;
            base.OnPreRender(e);
        }

        #endregion
    }
}
