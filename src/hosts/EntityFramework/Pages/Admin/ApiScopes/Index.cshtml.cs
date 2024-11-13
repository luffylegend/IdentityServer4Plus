// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.Admin.ApiScopes;

[SecurityHeaders]
[Authorize]
public class IndexModel : PageModel
{
    private readonly ApiScopeRepository _repository;

    public IndexModel(ApiScopeRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ApiScopeSummaryModel> Scopes { get; private set; } = default!;
    public string? Filter { get; set; }

    public async Task OnGetAsync(string? filter)
    {
        Filter = filter;
        Scopes = await _repository.GetAllAsync(filter);
    }
}
