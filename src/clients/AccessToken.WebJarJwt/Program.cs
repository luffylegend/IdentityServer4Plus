// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using WebJarJwt;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Host.Main Starting up");

Console.Title = "WebJarJwt (Sample)";

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            theme: AnsiConsoleTheme.Code)
        .MinimumLevel.Information()
        .MinimumLevel.Override("identityserver4", LogEventLevel.Verbose)
        .Enrich.FromLogContext());

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}