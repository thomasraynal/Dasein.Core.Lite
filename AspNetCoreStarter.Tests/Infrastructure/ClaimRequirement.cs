﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Infrastructure
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public String Type { get; private set; }
        public String Value { get; private set; }

        public ClaimRequirement(String type, String value)
        {
            Type = type;
            Value = value;
        }
    }
}
