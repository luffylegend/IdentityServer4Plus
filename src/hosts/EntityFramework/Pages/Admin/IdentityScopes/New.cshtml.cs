// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.Admin.IdentityScopes;

[SecurityHeaders]
[Authorize]
public class NewModel : PageModel
{
    private readonly IdentityScopeRepository _repository;

    public NewModel(IdentityScopeRepository repository)
    {
        _repository = repository;
    }

    [BindProperty]
    public IdentityScopeModel InputModel { get; set; } = default!;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            await _repository.CreateAsync(InputModel);
            return RedirectToPage("/Admin/IdentityScopes/Edit", new { id = InputModel.Name });
        }

        return Page();
    }
}
