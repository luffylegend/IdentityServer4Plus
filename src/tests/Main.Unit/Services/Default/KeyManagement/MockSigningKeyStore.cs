// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Services.Default.KeyManagement;

class MockSigningKeyStore : ISigningKeyStore
{
    public List<SerializedKey> Keys { get; set; } = new List<SerializedKey>();
    public bool LoadKeysAsyncWasCalled { get; set; }
    public bool DeleteWasCalled { get; set; }

    public Task DeleteKeyAsync(string id)
    {
        DeleteWasCalled = true;
        if (Keys != null)
        {
            Keys.Remove(Keys.FirstOrDefault(x => x.Id == id));
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<SerializedKey>> LoadKeysAsync()
    {
        LoadKeysAsyncWasCalled = true;
        return Task.FromResult<IEnumerable<SerializedKey>>(Keys);
    }

    public Task StoreKeyAsync(SerializedKey key)
    {
        if (Keys == null)
        {
            Keys = new List<SerializedKey>();
        }

        Keys.Add(key);
        return Task.CompletedTask;
    }
}
