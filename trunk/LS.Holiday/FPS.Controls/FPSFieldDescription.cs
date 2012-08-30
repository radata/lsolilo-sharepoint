using System.Security.Permissions;
using System.Web.UI;
using FPS.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    public class FPSFieldDescription : FieldDescription
    {
        #region Fields

        private bool _visible = true;

        #endregion

        #region Properties

        public override bool Visible
        {
            get { return base.Field != null && !base.Field.Description.IsNullOrEmpty() && base.Field.Type != SPFieldType.User && _visible; }
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            set { _visible = value; }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void Render(HtmlTextWriter output)
        {
            if ((base.Field != null) && !string.IsNullOrEmpty(base.Field.Description))
                output.Write(SPHttpUtility.NoEncode(base.Field.Description));
        }

        #endregion
    }
}
