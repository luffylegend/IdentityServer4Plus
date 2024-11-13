// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Configuration.RequestProcessing;
using IdentityServer4.Configuration.Configuration;
using IdentityServer4.Configuration.Models.DynamicClientRegistration;
using IdentityServer4.Configuration.Models;

namespace IdentityServerHost;

internal sealed class CustomClientRegistrationProcessor : DynamicClientRegistrationRequestProcessor
{
    private readonly ICollection<Client> _clients;

    public CustomClientRegistrationProcessor(
        IdentityServerConfigurationOptions options,
        IClientConfigurationStore store,
        ICollection<Client> clients) : base(options, store)
    {
        _clients = clients;
    }

    protected override async Task<IStepResult> AddClientId(DynamicClientRegistrationContext context)
    {
        if (context.Request.Extensions.TryGetValue("client_id", out var clientIdParameter))
        {
            var clientId = clientIdParameter.ToString();
            if (clientId != null)
            {
                if (_clients.Any(c => c.ClientId == clientId))
                {
                    return new DynamicClientRegistrationError(
                        "Duplicate client id",
                        "Attempt to register a client with a client id that has already been registered"
                    );
                }
                else
                {
                    context.Client.ClientId = clientId;
                    return new SuccessfulStep();
                }
            }
        }
        return await base.AddClientId(context);
    }

    protected override async Task<(Secret, string)> GenerateSecret(DynamicClientRegistrationContext context)
    {
        if (context.Request.Extensions.TryGetValue("client_secret", out var secretParam))
        {
            var plainText = secretParam.ToString();
            ArgumentNullException.ThrowIfNull(plainText);
            var secret = new Secret(plainText.Sha256());

            return (secret, plainText);
        }
        return await base.GenerateSecret(context);

    }
}
