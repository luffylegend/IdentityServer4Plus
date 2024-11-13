// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

namespace IdentityServer4.Stores;

/// <summary>
/// Marker interface to indicate if server side sessions enabled in DI.
/// </summary>
public interface IServerSideSessionsMarker { }

/// <summary>
/// Nop implementation for IServerSideSessionsMarker.
/// </summary>
public class NopIServerSideSessionsMarker : IServerSideSessionsMarker { }
