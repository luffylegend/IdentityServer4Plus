// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;
using Xunit;
using Entities = IdentityServer4.EntityFramework.Entities;
using Models = IdentityServer4.Models;

namespace EntityFramework.Storage.UnitTests.Mappers;

public class ApiResourceMappersTests
{
    [Fact]
    public void Can_Map()
    {
        var model = new Models.ApiResource();
        var mappedEntity = model.ToEntity();
        var mappedModel = mappedEntity.ToModel();

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }

    [Fact]
    public void Properties_Map()
    {
        var model = new Models.ApiResource()
        {
            Description = "description",
            DisplayName = "displayname",
            Name = "foo",
            Scopes = { "foo1", "foo2" },
            Enabled = false
        };


        var mappedEntity = model.ToEntity();

        mappedEntity.Scopes.Count.Should().Be(2);
        var foo1 = mappedEntity.Scopes.FirstOrDefault(x => x.Scope == "foo1");
        foo1.Should().NotBeNull();
        var foo2 = mappedEntity.Scopes.FirstOrDefault(x => x.Scope == "foo2");
        foo2.Should().NotBeNull();


        var mappedModel = mappedEntity.ToModel();

        mappedModel.Description.Should().Be("description");
        mappedModel.DisplayName.Should().Be("displayname");
        mappedModel.Enabled.Should().BeFalse();
        mappedModel.Name.Should().Be("foo");
    }

    [Fact]
    public void Missing_values_should_use_defaults()
    {
        var entity = new Entities.ApiResource
        {
            Secrets = new System.Collections.Generic.List<Entities.ApiResourceSecret>
        {
            new Entities.ApiResourceSecret
            {
            }
        }
        };

        var def = new Models.ApiResource
        {
            ApiSecrets = { new Models.Secret("foo") }
        };

        var model = entity.ToModel();
        model.ApiSecrets.First().Type.Should().Be(def.ApiSecrets.First().Type);
    }

    [Fact]
    public void Mapping_model_to_entity_maps_all_properties()
    {
        var excludedProperties = new string[]
        {
        "Id",
        "Updated",
        "LastAccessed",
        "NonEditable"
        };

        MapperTestHelpers
            .AllPropertiesAreMapped<Models.ApiResource, Entities.ApiResource>(
                source => source.AllowedAccessTokenSigningAlgorithms.Add("RS256"),
                source => source.ToEntity(),
                excludedProperties,
                out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }

    [Fact]
    public void Mapping_entity_to_model_maps_all_properties()
    {
        MapperTestHelpers
            .AllPropertiesAreMapped<Entities.ApiResource, Models.ApiResource>(source => source.ToModel(), out var unmappedMembers)
            .Should()
            .BeTrue($"{string.Join(',', unmappedMembers)} should be mapped");
    }
}
