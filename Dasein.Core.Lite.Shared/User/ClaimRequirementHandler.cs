using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == c.Type && c.Value == requirement.Value);
        
            if (null == claim)
            {
                throw new UnauthorizedUserException($"User must have role [{requirement.Value}]");
            }

            context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}
