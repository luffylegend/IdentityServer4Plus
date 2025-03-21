// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    /// <summary>
    /// Default token creation service
    /// </summary>
    public class DefaultTokenCreationService : ITokenCreationService
    {
        /// <summary>
        /// The key service
        /// </summary>
        protected readonly IKeyMaterialService Keys;

        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        ///  The clock
        /// </summary>
        protected readonly IClock Clock;

        /// <summary>
        /// The options
        /// </summary>
        protected readonly IdentityServerOptions Options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTokenCreationService"/> class.
        /// </summary>
        /// <param name="clock">The options.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public DefaultTokenCreationService(
            IClock clock,
            IKeyMaterialService keys,
            IdentityServerOptions options,
            ILogger<DefaultTokenCreationService> logger)
        {
            Clock = clock;
            Keys = keys;
            Options = options;
            Logger = logger;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A protected and serialized security token
        /// </returns>
        public virtual async Task<string> CreateTokenAsync(Token token)
        {
            //var header = await CreateHeaderAsync(token);
            var payload = await CreatePayloadAsync(token);
            var headerElements = await CreateHeaderElementsAsync(token);

            //return await CreateJwtAsync(new JwtSecurityToken(header, payload));
            return await CreateJwtAsync(token, payload, headerElements);
        }

        /// <summary>
        /// Creates the JWT payload
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The JWT payload</returns>
        protected virtual Task<string> CreatePayloadAsync(Token token)
        {
            var payload = token.CreateJwtPayload(Clock, Options, Logger);
            return Task.FromResult(JsonSerializer.Serialize(payload));
        }

        /// <summary>
        /// Creates additional JWT header elements
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual Task<Dictionary<string, object>> CreateHeaderElementsAsync(Token token)
        {
            var additionalHeaderElements = new Dictionary<string, object>();

            if (token.Type == IdentityServerConstants.TokenTypes.AccessToken)
            {
                if (Options.AccessTokenJwtType.IsPresent())
                {
                    additionalHeaderElements.Add("typ", Options.AccessTokenJwtType);
                }
            }
            else if (token.Type == IdentityServerConstants.TokenTypes.LogoutToken)
            {
                if (Options.LogoutTokenJwtType.IsPresent())
                {
                    additionalHeaderElements.Add("typ", Options.LogoutTokenJwtType);
                }
            }

            return Task.FromResult(additionalHeaderElements);
        }
        
        /// <summary>
        /// Creates JWT token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="payload"></param>
        /// <param name="headerElements"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual async Task<string> CreateJwtAsync(Token token, string payload,
            Dictionary<string, object> headerElements)
        {
            var credential = await Keys.GetSigningCredentialsAsync(token.AllowedSigningAlgorithms);

            if (credential == null)
            {
                throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
            }

            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            return handler.CreateToken(payload, credential, headerElements);
        }
    }
}