// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServerHost.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    public ViewModel View { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var localAddresses = new List<string?> { "127.0.0.1", "::1" };
        if (HttpContext.Connection.LocalIpAddress != null)
        {
            localAddresses.Add(HttpContext.Connection.LocalIpAddress.ToString());
        }

        if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            return NotFound();
        }

        View = new ViewModel(await HttpContext.AuthenticateAsync());

        return Page();
    }
}