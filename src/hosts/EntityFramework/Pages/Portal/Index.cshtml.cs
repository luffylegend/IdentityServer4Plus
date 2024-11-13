// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.Portal;

public class Index : PageModel
{
    private readonly ClientRepository _repository;
    public IEnumerable<ThirdPartyInitiatedLoginLink> Clients { get; private set; } = default!;

    public Index(ClientRepository repository)
    {
        _repository = repository;
    }

    public async Task OnGetAsync()
    {
        Clients = await _repository.GetClientsWithLoginUris();
    }
}
