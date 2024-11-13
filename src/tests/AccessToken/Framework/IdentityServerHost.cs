// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityServer4.AccessToken.Tests;

public class IdentityServerHost : GenericHost
{
    public IdentityServerHost(string baseAddress = "https://identityserver") 
        : base(baseAddress)
    {
        OnConfigureServices += ConfigureServices;
        OnConfigure += Configure;
    }

    public List<Client> Clients { get; set; } = new List<Client>();
    public List<IdentityResource> IdentityResources { get; set; } = new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
    };

    public List<ApiScope> ApiScopes { get; set; } = new();
    public List<ApiResource> ApiResources { get; set; } = new()
    {
        new ApiResource("urn:api1"),
        new ApiResource("urn:api2")
    };

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddAuthorization();

        services.AddLogging(logging => {
            logging.AddFilter("IdentityServer4", LogLevel.Debug);
        });

        services.AddIdentityServer(options=> 
            {
                options.EmitStaticAudienceClaim = true;
            
                // Artificially low durations to force retries
                options.DPoP.ServerClockSkew = TimeSpan.Zero;
                options.DPoP.ProofTokenValidityDuration = TimeSpan.FromSeconds(1);
            })
            .AddInMemoryClients(Clients)
            .AddInMemoryIdentityResources(IdentityResources)
            .AddInMemoryApiResources(ApiResources)
            .AddInMemoryApiScopes(ApiScopes);
    }

    private void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/account/login", context =>
            {
                return Task.CompletedTask;
            });
            
            endpoints.MapGet("/account/logout", async context =>
            {
                // signout as if the user were prompted
                await context.SignOutAsync();

                var logoutId = context.Request.Query["logoutId"];
                var interaction = context.RequestServices.GetRequiredService<IIdentityServerInteractionService>();

                var signOutContext = await interaction.GetLogoutContextAsync(logoutId);
                
                context.Response.Redirect(signOutContext.PostLogoutRedirectUri ?? "/");
            });
        });
    }

    public async Task CreateIdentityServerSessionCookieAsync(string sub, string? sid = null)
    {
        var props = new AuthenticationProperties();
        
        if (!String.IsNullOrWhiteSpace(sid))
        {
            props.Items.Add("session_id", sid);
        }
        
        await IssueSessionCookieAsync(props, new Claim("sub", sub));
    }

    public string CreateIdToken(string sub, string clientId)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _baseAddress,
            Audience = clientId,
            Claims = new Dictionary<string, object>
            {
                { "sub", sub }
            }
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);
    }
}
