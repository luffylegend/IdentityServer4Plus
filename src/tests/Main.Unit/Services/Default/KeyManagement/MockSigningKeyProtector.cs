// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services.KeyManagement;
using System;

namespace UnitTests.Services.Default.KeyManagement;

class MockSigningKeyProtector : ISigningKeyProtector
{
    public bool ProtectWasCalled { get; set; }

    public SerializedKey Protect(KeyContainer key)
    {
        ProtectWasCalled = true;
        return new SerializedKey
        {
            Id = key.Id,
            Algorithm = key.Algorithm,
            IsX509Certificate = key.HasX509Certificate,
            Created = DateTime.UtcNow,
            Data = KeySerializer.Serialize(key),
        };
    }

    public KeyContainer Unprotect(SerializedKey key)
    {
        return KeySerializer.Deserialize<RsaKeyContainer>(key.Data);
    }
}
