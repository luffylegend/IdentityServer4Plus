// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServerHost.Configuration;
using IdentityServerHost.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServerHost;

internal static class IdentityServerExtensions
{
    internal static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder builder)
    {
        var identityServer = builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;

                options.EmitScopesAsSpaceDelimitedStringInJwt = true;
                options.Endpoints.EnableJwtRequestUri = true;

                options.ServerSideSessions.UserDisplayNameClaimType = JwtClaimTypes.Name;

                options.UserInteraction.CreateAccountUrl = "/Account/Create";

                options.Endpoints.EnablePushedAuthorizationEndpoint = true;
                options.PushedAuthorization.AllowUnregisteredPushedRedirectUris = true;

                options.KeyManagement.SigningAlgorithms.Add(new SigningAlgorithmOptions
                {
                    Name = "RS256",
                    UseX509Certificate = true
                });
            })
            //.AddServerSideSessions()
            .AddInMemoryClients(Clients.Get().ToList())
            .AddInMemoryIdentityResources(Resources.IdentityResources)
            .AddInMemoryApiScopes(Resources.ApiScopes)
            .AddInMemoryApiResources(Resources.ApiResources)
            //.AddStaticSigningCredential()
            .AddExtensionGrantValidator<Extensions.ExtensionGrantValidator>()
            .AddExtensionGrantValidator<Extensions.NoSubjectExtensionGrantValidator>()
            .AddJwtBearerClientAuthentication()
            .AddAppAuthRedirectUriValidator()
            .AddTestUsers(TestUsers.Users)
            .AddProfileService<HostProfileService>()
            .AddCustomTokenRequestValidator<ParameterizedScopeTokenRequestValidator>()
            .AddScopeParser<ParameterizedScopeParser>()
            .AddMutualTlsSecretValidators()
            .AddInMemoryOidcProviders(
            [
                new IdentityServer4.Models.OidcProvider
            {
                Scheme = "dynamicprovider-idsvr",
                DisplayName = "IdentityServer (via Dynamic Providers)",
                Authority = "https://demo.identityserver4plus.com",
                ClientId = "login",
                ResponseType = "id_token",
                Scope = "openid profile"
            }
            ]);


        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddIdentityServerConfiguration(opt =>
        {
            // opt.DynamicClientRegistration.SecretLifetime = TimeSpan.FromHours(1);
        }).AddInMemoryClientConfigurationStore();

        return builder;
    }

    private static IIdentityServerBuilder AddStaticSigningCredential(this IIdentityServerBuilder builder)
    {
        // create random RS256 key
        //builder.AddDeveloperSigningCredential();


#pragma warning disable SYSLIB0057 // Type or member is obsolete
        // TODO - Use X509CertificateLoader in a future release (when we drop NET8 support)

        // use an RSA-based certificate with RS256
        using var rsaCert = new X509Certificate2("./testkeys/identityserver.test.rsa.p12", "changeit");
        builder.AddSigningCredential(rsaCert, "RS256");

        // ...and PS256
        builder.AddSigningCredential(rsaCert, "PS256");

        // or manually extract ECDSA key from certificate (directly using the certificate is not support by Microsoft right now)
        using var ecCert = new X509Certificate2("./testkeys/identityserver.test.ecdsa.p12", "changeit");
#pragma warning restore SYSLIB0057 // Type or member is obsolete

        var key = new ECDsaSecurityKey(ecCert.GetECDsaPrivateKey())
        {
            KeyId = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)
        };

        return builder.AddSigningCredential(
            key,
            IdentityServerConstants.ECDsaSigningAlgorithm.ES256);
    }
}
