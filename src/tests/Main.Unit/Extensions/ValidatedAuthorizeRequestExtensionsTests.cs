// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Validation;
using System;
using Xunit;

namespace UnitTests.Extensions;

public class ValidatedAuthorizeRequestExtensionsTests
{
    [Fact]
    public void GetAcrValues_should_return_snapshot_of_values()
    {
        var request = new ValidatedAuthorizeRequest()
        {
            Raw = new System.Collections.Specialized.NameValueCollection()
        };
        request.AuthenticationContextReferenceClasses.Add("a");
        request.AuthenticationContextReferenceClasses.Add("b");
        request.AuthenticationContextReferenceClasses.Add("c");

        var acrs = request.GetAcrValues();
        foreach (var acr in acrs)
        {
            request.RemoveAcrValue(acr);
        }
    }

    [Fact]
    [Obsolete]
    public void ToOptimizedFullDictionary_should_return_dictionary_with_array_for_repeated_keys_when_request_objects_are_used()
    {
        var request = new ValidatedAuthorizeRequest()
        {
            Raw = new System.Collections.Specialized.NameValueCollection
        {
            { OidcConstants.AuthorizeRequest.Request, "Request object here" },
            { OidcConstants.AuthorizeRequest.Resource, "Resource1" },
            { OidcConstants.AuthorizeRequest.Resource, "Resource2" },
        }
        };

        var res = request.ToOptimizedFullDictionary();

        Assert.Equal(2, res[OidcConstants.AuthorizeRequest.Resource].Length);
    }
}
