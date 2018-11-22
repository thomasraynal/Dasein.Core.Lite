using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Domain
{
    public enum TradeStatus
    {
        None,
        Created,
        ComplianceCheck,
        Processed,
        Rejected
    }
}
