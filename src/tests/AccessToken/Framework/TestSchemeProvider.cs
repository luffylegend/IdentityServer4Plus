// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityServer4.AccessToken.Tests;

public class TestSchemeProvider : IAuthenticationSchemeProvider
{
    public TestSchemeProvider(string signInSchemeName = "testScheme")
    {
        DefaultSignInScheme = new AuthenticationScheme(signInSchemeName, signInSchemeName, typeof(CookieAuthenticationHandler));
    }

    public AuthenticationScheme? DefaultSignInScheme { get; set; }

    public Task<AuthenticationScheme?> GetDefaultSignInSchemeAsync()
    {
        return Task.FromResult(DefaultSignInScheme);
    }

    #region Not Implemented (No tests have needed these yet)

    public void AddScheme(AuthenticationScheme scheme)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AuthenticationScheme>> GetAllSchemesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationScheme?> GetDefaultChallengeSchemeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationScheme?> GetDefaultForbidSchemeAsync()
    {
        throw new NotImplementedException();
    }


    public Task<AuthenticationScheme?> GetDefaultSignOutSchemeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationScheme?> GetSchemeAsync(string name)
    {
        throw new NotImplementedException();
    }

    public void RemoveScheme(string name)
    {
        throw new NotImplementedException();
    }

    #endregion
}
