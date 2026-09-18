using System;
using System.Collections.Generic;
using System.Text;

namespace VTTBD_CorePlatform.Domain.Common.Enums
{
    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Locked= 3,
        PendingActivation = 4,
        Deleted = 5
    }
}
