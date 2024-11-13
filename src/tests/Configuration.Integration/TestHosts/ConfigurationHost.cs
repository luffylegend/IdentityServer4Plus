// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Configuration.EntityFramework;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Services;
using IntegrationTests.TestFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.TestHosts;

public class ConfigurationHost : GenericHost
{
    public ConfigurationHost(
        InMemoryDatabaseRoot databaseRoot,
        string baseAddress = "https://configuration")
            : base(baseAddress)
    {
        OnConfigureServices += (services) => ConfigureServices(services, databaseRoot);
        OnConfigure += Configure;
    }

    private void ConfigureServices(IServiceCollection services, InMemoryDatabaseRoot databaseRoot)
    {
        services.AddRouting();
        services.AddAuthorization();

        services.AddSingleton<ICancellationTokenProvider, MockCancellationTokenProvider>();

        services.AddIdentityServerConfiguration(opt =>
            {

            })
            .AddClientConfigurationStore();
        services.AddSingleton(new ConfigurationStoreOptions());
        services.AddDbContext<ConfigurationDbContext>(opt =>
            opt.UseInMemoryDatabase("configurationDb", databaseRoot));
    }

    private void Configure(WebApplication app)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.MapDynamicClientRegistration("/connect/dcr")
            .AllowAnonymous();
    }
}
