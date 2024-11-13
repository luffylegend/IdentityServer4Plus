// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Common;

public class MockBackChannelAuthenticationRequestStore : IBackChannelAuthenticationRequestStore
{
    public Dictionary<string, BackChannelAuthenticationRequest> Items { get; set; } = new Dictionary<string, BackChannelAuthenticationRequest>();

    public Task<string> CreateRequestAsync(BackChannelAuthenticationRequest request)
    {
        var key = Guid.NewGuid().ToString();
        request.InternalId = key.Sha256();
        Items.Add(key, request);
        return Task.FromResult(key);
    }

    public Task<BackChannelAuthenticationRequest> GetByAuthenticationRequestIdAsync(string requestId)
    {
        return Task.FromResult(Items[requestId]);
    }

    public Task<BackChannelAuthenticationRequest> GetByInternalIdAsync(string id)
    {
        var item = Items.SingleOrDefault(x => x.Value.InternalId == id);
        return Task.FromResult(item.Value);
    }

    public Task<IEnumerable<BackChannelAuthenticationRequest>> GetLoginsForUserAsync(string subjectId, string clientId = null)
    {
        var items = Items.Where(x => x.Value.Subject.GetSubjectId() == subjectId
                                     && (clientId == null || x.Value.ClientId == clientId)
        );
        return Task.FromResult(items.Select(x => x.Value).AsEnumerable());
    }

    public Task RemoveByInternalIdAsync(string id)
    {
        var item = Items.SingleOrDefault(x => x.Value.InternalId == id);
        if (item.Key != null)
        {
            Items.Remove(item.Key);
        }
        return Task.CompletedTask;
    }

    public Task UpdateByInternalIdAsync(string id, BackChannelAuthenticationRequest request)
    {
        var item = Items.SingleOrDefault(x => x.Value.InternalId == id);
        if (item.Key != null)
        {
            Items.Remove(item.Key);
            Items.Add(item.Key, request);
        }
        return Task.CompletedTask;
    }
}
