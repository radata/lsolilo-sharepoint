using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace FPS.Controls
{
    public class PopupUserControl : UserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the page to redirect on submit.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public string PageToRedirectOnSubmit { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is pop UI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Application Page is displayed in Modal Dialog; otherwise, <c>false</c>.
        /// </value>
        protected bool IsPopUI
        {
            get
            {
                return !string.IsNullOrEmpty(base.Request.QueryString["IsDlg"]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK.</param>
        protected void EndOperation(int result = 1)
        {
            if (IsPopUI)
            {
                Page.Response.Clear();
                Page.Response.Write(string.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, {1});</script>", result, string.IsNullOrEmpty(PageToRedirectOnSubmit) ? "null" : string.Format("\"{0}\"", PageToRedirectOnSubmit)));
                Page.Response.End();
            }
            else
            {
                Redirect();
            }
        }

        /// <summary>
        /// Redirects to the URL specified in the PageToRedirectOnOK property.
        /// </summary>
        private void Redirect()
        {
            SPUtility.Redirect(PageToRedirectOnSubmit ?? SPContext.Current.Web.Url, SPRedirectFlags.UseSource, Context);
        }
        
        #endregion
    }
}
