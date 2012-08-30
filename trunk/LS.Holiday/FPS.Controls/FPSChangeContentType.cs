using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using FPS.Core;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    public class FPSChangeContentType : ChangeContentType
    {
        #region Fields

        private const string LabelControlName = "DisplayName";
        private const string ContentTypeDropDownListControlName = "ContentTypeChoice";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name to diplay in NewForm and EditForm.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the allowed content types.
        /// </summary>
        /// <value>
        /// Dictionary key is ContentType name or id. Value is DisplayName; if empty ContentType name will be used.
        /// </value>
        public Dictionary<string, string> AllowedContentTypes { get; set; }

        #endregion

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ControlMode = SPControlMode.Edit;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Label label = this.TemplateContainer.FindControl(LabelControlName) as Label;

            if (label != null)
                label.Text = SPHttpUtility.HtmlEncodeAllowSimpleTextFormatting(DisplayName);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var contentTypeDropDown = this.TemplateContainer.FindControl(ContentTypeDropDownListControlName) as DropDownList;
            if (contentTypeDropDown != null && AllowedContentTypes != null)
            {
                foreach (ListItem item in contentTypeDropDown.Items)
                {
                    var allowedContentType = AllowedContentTypes.FirstOrDefault(pair => item.Value.ToUpper().Contains(pair.Key.ToUpper()) || item.Text.ToUpper().Contains(pair.Key.ToUpper()));
                    item.Enabled = !allowedContentType.Key.IsNullOrEmpty();

                    if (item.Enabled && !allowedContentType.Value.IsNullOrEmpty())
                        item.Text = allowedContentType.Value;
                }
            }
        }

        #endregion
    }
}
