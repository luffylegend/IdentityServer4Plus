// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IntegrationTests.Clients.Setup;

class CustomProfileService : TestUserProfileService
{
    public CustomProfileService(TestUserStore users, ILogger<TestUserProfileService> logger) : base(users, logger)
    { }

    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await base.GetProfileDataAsync(context);

        if (context.Subject.Identity.AuthenticationType == "custom")
        {
            var extraClaim = context.Subject.FindFirst("extra_claim");
            if (extraClaim != null)
            {
                context.IssuedClaims.Add(extraClaim);
            }
        }
    }
}
