// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Services.Default;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.UnitTests.ResponseHandling
{
    public class DeviceAuthorizationResponseGeneratorTests
    {
        private readonly List<IdentityResource> _identityResources = new List<IdentityResource> {new IdentityResources.OpenId(), new IdentityResources.Profile()};
        private readonly List<ApiResource> _apiResources = new List<ApiResource> { new ApiResource("resource") { Scopes = {"api1" } } };
        private readonly List<ApiScope> _scopes = new List<ApiScope> { new ApiScope("api1") };

        private readonly FakeUserCodeGenerator _fakeUserCodeGenerator = new FakeUserCodeGenerator();
        private readonly IDeviceFlowCodeService _deviceFlowCodeService = new DefaultDeviceFlowCodeService(new InMemoryDeviceFlowStore(), new StubHandleGenerationService());
        private readonly IdentityServerOptions _options = new IdentityServerOptions();
        private readonly StubClock _clock = new StubClock();
        
        private readonly DeviceAuthorizationResponseGenerator _generator;
        private readonly DeviceAuthorizationRequestValidationResult _testResult;
        private const string TestBaseUrl = "http://localhost:5000/";

        public DeviceAuthorizationResponseGeneratorTests()
        {
            _testResult = new DeviceAuthorizationRequestValidationResult(new ValidatedDeviceAuthorizationRequest
            {
                Client = new Client {ClientId = Guid.NewGuid().ToString()},
                IsOpenIdRequest = true,
                ValidatedResources = new ResourceValidationResult()
            });

            _generator = new DeviceAuthorizationResponseGenerator(
                _options,
                new DefaultUserCodeService(new IUserCodeGenerator[] {new NumericUserCodeGenerator(), _fakeUserCodeGenerator }),
                _deviceFlowCodeService,
                _clock,
                new NullLogger<DeviceAuthorizationResponseGenerator>());
        }

        [Fact]
        public async Task ProcessAsync_when_valiationresult_null_exect_exception()
        {
            Func<Task> act = () => _generator.ProcessAsync(null, TestBaseUrl);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ProcessAsync_when_valiationresult_client_null_exect_exception()
        {
            var validationResult = new DeviceAuthorizationRequestValidationResult(new ValidatedDeviceAuthorizationRequest());
            Func <Task> act = () => _generator.ProcessAsync(validationResult, TestBaseUrl);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ProcessAsync_when_baseurl_null_exect_exception()
        {
            Func<Task> act = () => _generator.ProcessAsync(_testResult, null);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ProcessAsync_when_user_code_collision_expect_retry()
        {
            var creationTime = DateTime.UtcNow;
            _clock.UtcNowFunc = () => creationTime;

            _testResult.ValidatedRequest.Client.UserCodeType = FakeUserCodeGenerator.UserCodeTypeValue;
            await _deviceFlowCodeService.StoreDeviceAuthorizationAsync(FakeUserCodeGenerator.TestCollisionUserCode, new DeviceCode());

            var response = await _generator.ProcessAsync(_testResult, TestBaseUrl);

            response.UserCode.Should().Be(FakeUserCodeGenerator.TestUniqueUserCode);
        }

        [Fact]
        public async Task ProcessAsync_when_user_code_collision_retry_limit_reached_expect_error()
        {
            var creationTime = DateTime.UtcNow;
            _clock.UtcNowFunc = () => creationTime;

            _fakeUserCodeGenerator.RetryLimit = 1;
            _testResult.ValidatedRequest.Client.UserCodeType = FakeUserCodeGenerator.UserCodeTypeValue;
            await _deviceFlowCodeService.StoreDeviceAuthorizationAsync(FakeUserCodeGenerator.TestCollisionUserCode, new DeviceCode());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _generator.ProcessAsync(_testResult, TestBaseUrl));
        }

        [Fact]
        public async Task ProcessAsync_when_generated_expect_user_code_stored()
        {
            var creationTime = DateTime.UtcNow;
            _clock.UtcNowFunc = () => creationTime;

            _testResult.ValidatedRequest.RequestedScopes = new List<string> { "openid", "api1" };
            _testResult.ValidatedRequest.ValidatedResources = new ResourceValidationResult(new Resources(
                _identityResources.Where(x=>x.Name == "openid"), 
                _apiResources.Where(x=>x.Name == "resource"), 
                _scopes.Where(x=>x.Name == "api1")));

            var response = await _generator.ProcessAsync(_testResult, TestBaseUrl);

            response.UserCode.Should().NotBeNullOrWhiteSpace();

            var userCode = await _deviceFlowCodeService.FindByUserCodeAsync(response.UserCode);
            userCode.Should().NotBeNull();
            userCode.ClientId.Should().Be(_testResult.ValidatedRequest.Client.ClientId);
            userCode.Lifetime.Should().Be(_testResult.ValidatedRequest.Client.DeviceCodeLifetime);
            userCode.CreationTime.Should().Be(creationTime);
            userCode.Subject.Should().BeNull();
            userCode.AuthorizedScopes.Should().BeNull();

            userCode.RequestedScopes.Should().Contain(_testResult.ValidatedRequest.RequestedScopes);
        }

        [Fact]
        public async Task ProcessAsync_when_generated_expect_device_code_stored()
        {
            var creationTime = DateTime.UtcNow;
            _clock.UtcNowFunc = () => creationTime;

            var response = await _generator.ProcessAsync(_testResult, TestBaseUrl);

            response.DeviceCode.Should().NotBeNullOrWhiteSpace();
            response.Interval.Should().Be(_options.DeviceFlow.Interval);
            
            var deviceCode = await _deviceFlowCodeService.FindByDeviceCodeAsync(response.DeviceCode);
            deviceCode.Should().NotBeNull();
            deviceCode.ClientId.Should().Be(_testResult.ValidatedRequest.Client.ClientId);
            deviceCode.IsOpenId.Should().Be(_testResult.ValidatedRequest.IsOpenIdRequest);
            deviceCode.Lifetime.Should().Be(_testResult.ValidatedRequest.Client.DeviceCodeLifetime);
            deviceCode.CreationTime.Should().Be(creationTime);
            deviceCode.Subject.Should().BeNull();
            deviceCode.AuthorizedScopes.Should().BeNull();
            
            response.DeviceCodeLifetime.Should().Be(deviceCode.Lifetime);
        }

        [Fact]
        public async Task ProcessAsync_when_DeviceVerificationUrl_is_relative_uri_expect_correct_VerificationUris()
        {
            const string baseUrl = "http://localhost:5000/";
            _options.UserInteraction.DeviceVerificationUrl = "/device";
            _options.UserInteraction.DeviceVerificationUserCodeParameter = "userCode";

            var response = await _generator.ProcessAsync(_testResult, baseUrl);

            response.VerificationUri.Should().Be("http://localhost:5000/device");
            response.VerificationUriComplete.Should().StartWith("http://localhost:5000/device?userCode=");
        }

        [Fact]
        public async Task ProcessAsync_when_DeviceVerificationUrl_is_absolute_uri_expect_correct_VerificationUris()
        {
            const string baseUrl = "http://localhost:5000/";
            _options.UserInteraction.DeviceVerificationUrl = "http://short/device";
            _options.UserInteraction.DeviceVerificationUserCodeParameter = "userCode";

            var response = await _generator.ProcessAsync(_testResult, baseUrl);

            response.VerificationUri.Should().Be("http://short/device");
            response.VerificationUriComplete.Should().StartWith("http://short/device?userCode=");
        }
    }

    internal class FakeUserCodeGenerator : IUserCodeGenerator
    {
        public const string UserCodeTypeValue = "Collider";
        public const string TestUniqueUserCode = "123";
        public const string TestCollisionUserCode = "321";
        private int _tryCount = 0;
        private int _retryLimit = 2;


        public string UserCodeType => UserCodeTypeValue;

        public int RetryLimit
        {
            get => _retryLimit;
            set => _retryLimit = value;
        }

        public Task<string> GenerateAsync()
        {
            if (_tryCount == 0)
            {
                _tryCount++;
                return Task.FromResult(TestCollisionUserCode);
            }

            _tryCount++;
            return Task.FromResult(TestUniqueUserCode);
        }
    }
}