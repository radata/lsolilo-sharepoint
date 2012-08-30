using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LS.Delegations.Lists
{
    public enum DelegationApprovalStatus
    {
        [Description("Draft")]
        Draft,
        [Description("Do Zaakceptowania")]
        ForApproval,
        [Description("Zaakceptowane")]
        Approved
    }
}
