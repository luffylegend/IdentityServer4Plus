// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFramework.IntegrationTests;

/// <summary>
/// Helper methods to initialize DbContextOptions for the specified database provider and context.
/// </summary>
public class DatabaseProviderBuilder
{
    public static DbContextOptions<TDbContext> BuildInMemory<TDbContext, TStoreOptions>(string name,
        TStoreOptions storeOptions)
        where TDbContext : DbContext
        where TStoreOptions : class
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(storeOptions);

        var builder = new DbContextOptionsBuilder<TDbContext>();
        builder.UseInMemoryDatabase(name);
        builder.UseApplicationServiceProvider(serviceCollection.BuildServiceProvider());
        return builder.Options;
    }

    public static DbContextOptions<TDbContext> BuildSqlite<TDbContext, TStoreOptions>(string name,
        TStoreOptions storeOptions)
        where TDbContext : DbContext
        where TStoreOptions : class
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(storeOptions);

        // Open a connection so that the in-memory database is kept alive
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<TDbContext>();
        builder.UseSqlite(connection);
        builder.UseApplicationServiceProvider(serviceCollection.BuildServiceProvider());
        return builder.Options;
    }

    public static DbContextOptions<TDbContext> BuildLocalDb<TDbContext, TStoreOptions>(string name,
        TStoreOptions storeOptions)
        where TDbContext : DbContext
        where TStoreOptions : class
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(storeOptions);

        var builder = new DbContextOptionsBuilder<TDbContext>();
        builder.UseSqlServer(
            $@"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.IdentityServer4Plus.EntityFramework-8.0.0.{name};trusted_connection=yes;");
        builder.UseApplicationServiceProvider(serviceCollection.BuildServiceProvider());
        return builder.Options;
    }

    public static DbContextOptions<T> BuildAppVeyorSqlServer2016<T>(string name) where T : DbContext
    {
        var builder = new DbContextOptionsBuilder<T>();
        builder.UseSqlServer(
            $@"Server=(local)\SQL2016;Database=Test.IdentityServer4Plus.EntityFramework.{name};User ID=sa;Password=Password12!");
        return builder.Options;
    }
}
