using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;

namespace FPS.Controls
{
    [Guid("b6a55c4c-e40f-4b1f-b53f-2273fdce060f")]
    public class RelationField : SPFieldText
    {
        #region Constructors

        public RelationField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public RelationField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        #endregion

        #region Methods

        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return new RelationFieldControl() { FieldName = InternalName };
            }
        }

        #endregion
    }
}
