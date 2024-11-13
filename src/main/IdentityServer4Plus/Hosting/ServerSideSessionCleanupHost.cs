// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper to cleanup expired server side sessions.
/// </summary>
public class ServerSideSessionCleanupHost : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IdentityServerOptions _options;
    private readonly ILogger<ServerSideSessionCleanupHost> _logger;

    private CancellationTokenSource _source;

    /// <summary>
    /// Constructor for ServerSideSessionCleanupHost.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    public ServerSideSessionCleanupHost(IServiceProvider serviceProvider, IdentityServerOptions options, ILogger<ServerSideSessionCleanupHost> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    /// <summary>
    /// Starts the token cleanup polling.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.ServerSideSessions.RemoveExpiredSessions)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call Stop first.");

            _logger.LogDebug("Starting server-side session removal");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternalAsync(_source.Token), cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the token cleanup polling.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_options.ServerSideSessions.RemoveExpiredSessions)
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call Start first.");

            _logger.LogDebug("Stopping server-side session removal");

            _source.Cancel();
            _source = null;
        }

        return Task.CompletedTask;
    }

    private async Task StartInternalAsync(CancellationToken cancellationToken)
    {
        var removalFrequencySeconds = (int) _options.ServerSideSessions.RemoveExpiredSessionsFrequency.TotalSeconds;

        // Start the first run at a random interval.
        var delay = _options.ServerSideSessions.FuzzExpiredSessionRemovalStart
            ? TimeSpan.FromSeconds(Random.Shared.Next(removalFrequencySeconds))
            : _options.ServerSideSessions.RemoveExpiredSessionsFrequency;

        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("CancellationRequested. Exiting.");
                break;
            }

            try
            {
                await Task.Delay(delay, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug("TaskCanceledException. Exiting.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError("Task.Delay exception: {0}. Exiting.", ex.Message);
                break;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("CancellationRequested. Exiting.");
                break;
            }

            await RunAsync(cancellationToken);

            delay = _options.ServerSideSessions.RemoveExpiredSessionsFrequency;
        }
    }

    async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // this is here for testing
        if (!_options.ServerSideSessions.RemoveExpiredSessions) return;

        try
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<ServerSideSessionCleanupHost>>();
                var options = serviceScope.ServiceProvider.GetRequiredService<IdentityServerOptions>();
                var serverSideTicketStore = serviceScope.ServiceProvider.GetRequiredService<IServerSideTicketStore>();
                var sessionCoordinationService = serviceScope.ServiceProvider.GetRequiredService<ISessionCoordinationService>();

                var found = Int32.MaxValue;

                while (found > 0)
                {
                    var sessions = await serverSideTicketStore.GetAndRemoveExpiredSessionsAsync(options.ServerSideSessions.RemoveExpiredSessionsBatchSize, cancellationToken);
                    found = sessions.Count;

                    if (found > 0)
                    {
                        logger.LogDebug("Processing expiration for {count} expired server-side sessions.", found);

                        foreach (var session in sessions)
                        {
                            await sessionCoordinationService.ProcessExpirationAsync(session);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception removing expired sessions");
        }
    }
}
