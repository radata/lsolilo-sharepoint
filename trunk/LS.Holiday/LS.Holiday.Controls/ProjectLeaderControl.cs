using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using LS.Holiday.Core;

namespace LS.Holiday.Controls
{
    public class ProjectLeaderControl : SPFieldUser
    {
        #region Properties
        public override string DefaultValue
        {
            get
            {
                SPWeb web = SPContext.Current.Web;
                SPUser projectLeader = UserHelper.GetUserProjectLeader(web.CurrentUser, SPServiceContext.Current, SPContext.Current);

                string defaultValue = string.Format("{0};#{1}", projectLeader.ID.ToString(), projectLeader.Name);
                if (this.SelectionGroup > 0)
                {
                    SPGroup group = web.Groups[this.SelectionGroup];
                    if ((group != null) && (group.ContainsCurrentUser))
                        return defaultValue;
                }
                else
                    return defaultValue;

                return string.Empty;
            }
            set
            {
                base.DefaultValue = value;
            }
        }
        #endregion

        #region Constructors
        public ProjectLeaderControl(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            Initialize();
        }

        public ProjectLeaderControl(SPFieldCollection fields, string fieldName, string displayName)
            : base(fields, fieldName, displayName)
        {
            Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.SelectionMode = SPFieldUserSelectionMode.PeopleOnly;
            this.AllowMultipleValues = false;
        }
        #endregion
    }
}
