// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;
using Xunit;
using Entities = IdentityServer4.EntityFramework.Entities;
using Models = IdentityServer4.Models;

namespace EntityFramework.Storage.UnitTests.Mappers;

public class ScopesMappersTests
{
    [Fact]
    public void CanMapScope()
    {
        var model = new Models.ApiScope();
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
        "Updated",
        "Created",
        "LastAccessed",
        "NonEditable"
        };

        MapperTestHelpers
            .AllPropertiesAreMapped<Models.ApiScope, Entities.ApiScope>(source => source.ToEntity(), excludedProperties, out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }

    [Fact]
    public void Mapping_entity_to_model_maps_all_properties()
    {
        MapperTestHelpers
            .AllPropertiesAreMapped<Entities.ApiScope, Models.ApiScope>(source => source.ToModel(), out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }

    [Fact]
    public void Properties_Map()
    {
        var model = new IdentityServer4.Models.ApiScope()
        {
            Description = "description",
            DisplayName = "displayname",
            Name = "foo",
            UserClaims = { "c1", "c2" },
            Properties = {
            { "x", "xx" },
            { "y", "yy" },
        },
            Enabled = false
        };


        var mappedEntity = model.ToEntity();
        mappedEntity.Description.Should().Be("description");
        mappedEntity.DisplayName.Should().Be("displayname");
        mappedEntity.Name.Should().Be("foo");

        mappedEntity.UserClaims.Count.Should().Be(2);
        mappedEntity.UserClaims.Select(x => x.Type).Should().BeEquivalentTo(new[] { "c1", "c2" });
        mappedEntity.Properties.Count.Should().Be(2);
        mappedEntity.Properties.Should().Contain(x => x.Key == "x" && x.Value == "xx");
        mappedEntity.Properties.Should().Contain(x => x.Key == "y" && x.Value == "yy");


        var mappedModel = mappedEntity.ToModel();

        mappedModel.Description.Should().Be("description");
        mappedModel.DisplayName.Should().Be("displayname");
        mappedModel.Enabled.Should().BeFalse();
        mappedModel.Name.Should().Be("foo");
        mappedModel.UserClaims.Count.Should().Be(2);
        mappedModel.UserClaims.Should().BeEquivalentTo(new[] { "c1", "c2" });
        mappedModel.Properties.Count.Should().Be(2);
        mappedModel.Properties["x"].Should().Be("xx");
        mappedModel.Properties["y"].Should().Be("yy");
    }
}
