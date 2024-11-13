// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Models;
using IntegrationTests.TestFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.TestHosts;

public class IdentityServerHost : GenericHost
{

    public IdentityServerHost(InMemoryDatabaseRoot databaseRoot, string baseAddress = "https://identityserver4")
        : base(baseAddress)
    {
        OnConfigureServices += (services) => ConfigureServices(services, databaseRoot);
        OnConfigure += Configure;
    }

    public List<Client> Clients { get; set; } = new List<Client>();
    public List<IdentityResource> IdentityResources { get; set; } = new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
    };
    public List<ApiScope> ApiScopes { get; set; } = new List<ApiScope>();

    private void ConfigureServices(IServiceCollection services, InMemoryDatabaseRoot databaseRoot)
    {
        services.AddRouting();
        services.AddAuthorization();

        services.AddConfigurationDbContext<ConfigurationDbContext>(opt =>
            opt.ConfigureDbContext = builder =>
                builder.UseInMemoryDatabase("configurationDb", databaseRoot));

        services.AddIdentityServer(options =>
        {
            options.EmitStaticAudienceClaim = true;
        })
            .AddClientStore<ClientStore>()
            .AddInMemoryIdentityResources(IdentityResources)
            .AddInMemoryApiScopes(ApiScopes);
    }

    private void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();
    }


    public async Task<Client> GetClientAsync(string clientId)
    {
        var store = AppServices?.GetRequiredService<ClientStore>()
            ?? throw new Exception("Failed to resolve ClientStore in test");
        return await store.FindClientByIdAsync(clientId);
    }
}
