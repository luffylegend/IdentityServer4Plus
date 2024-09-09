// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Validator for handling client authentication
    /// </summary>
    public interface IClientSecretValidator
    {
        /// <summary>
        /// Tries to authenticate a client based on the incoming request
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<ClientSecretValidationResult> ValidateAsync(HttpContext context);
    }
}