// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.Mappers;
using FluentAssertions;
using Xunit;
using Models = IdentityServer4.Models;
using Entities = IdentityServer4.EntityFramework.Entities;

namespace EntityFramework.Storage.UnitTests.Mappers;

public class PersistedGrantMappersTests
{
    [Fact]
    public void CanMap()
    {
        var model = new IdentityServer4.Models.PersistedGrant()
        {
            ConsumedTime = new System.DateTime(2020, 02, 03, 4, 5, 6)
        };

        var mappedEntity = model.ToEntity();
        mappedEntity.ConsumedTime.Value.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));

        var mappedModel = mappedEntity.ToModel();
        mappedModel.ConsumedTime.Value.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }

    [Fact]
    public void Mapping_model_to_entity_maps_all_properties()
    {
        var excludedProperties = new string[]
        {
        "Id",
        "Updated",
        "Created",
        "LastAccessed",
        "NonEditable"
        };

        MapperTestHelpers
            .AllPropertiesAreMapped<Models.PersistedGrant, Entities.PersistedGrant>(source => source.ToEntity(), excludedProperties, out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }

    [Fact]
    public void Mapping_entity_to_model_maps_all_properties()
    {
        MapperTestHelpers
            .AllPropertiesAreMapped<Entities.PersistedGrant, Models.PersistedGrant>(source => source.ToModel(), out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }
}
