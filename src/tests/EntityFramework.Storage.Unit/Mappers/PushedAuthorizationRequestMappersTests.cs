// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.EntityFramework.Mappers;
using Xunit;
using Entities = IdentityServer4.EntityFramework.Entities;
using Models = IdentityServer4.Models;

namespace EntityFramework.Storage.UnitTests.Mappers;


public class PushedAuthorizationRequestMappersTests
{
    [Fact]
    public void CanMapPushedAuthorizationRequest()
    {
        var model = new Models.PushedAuthorizationRequest();
        var mappedEntity = model.ToEntity();
        var mappedModel = mappedEntity.ToModel();

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }

    [Fact]
    public void Mapping_model_to_entity_maps_all_properties()
    {
        var excludedProperties = new string[]
        {
        "Id",
        };

        MapperTestHelpers
            .AllPropertiesAreMapped<Models.PushedAuthorizationRequest, Entities.PushedAuthorizationRequest>(source => source.ToEntity(), excludedProperties, out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }

    [Fact]
    public void Mapping_entity_to_model_maps_all_properties()
    {
        MapperTestHelpers
            .AllPropertiesAreMapped<Entities.PushedAuthorizationRequest, Models.PushedAuthorizationRequest>(source => source.ToModel(), out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }
}
