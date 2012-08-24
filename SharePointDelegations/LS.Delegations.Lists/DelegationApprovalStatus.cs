﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SharePointDelegations
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
