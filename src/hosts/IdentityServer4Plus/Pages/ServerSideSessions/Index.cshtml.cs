// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.ServerSideSessions;

public class IndexModel : PageModel
{
    private readonly ISessionManagementService? _sessionManagementService;

    public IndexModel(ISessionManagementService? sessionManagementService = null)
    {
        _sessionManagementService = sessionManagementService;
    }

    public QueryResult<UserSession>? UserSessions { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? DisplayNameFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SessionIdFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SubjectIdFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Token { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Prev { get; set; }

    public async Task OnGet()
    {
        if (_sessionManagementService != null)
        {
            UserSessions = await _sessionManagementService.QuerySessionsAsync(new SessionQuery
            {
                ResultsToken = Token,
                RequestPriorResults = Prev == "true",
                DisplayName = DisplayNameFilter,
                SessionId = SessionIdFilter,
                SubjectId = SubjectIdFilter
            });
        }
    }

    [BindProperty]
    public string? SessionId { get; set; }

    public async Task<IActionResult> OnPost()
    {
        ArgumentNullException.ThrowIfNull(_sessionManagementService);

        await _sessionManagementService.RemoveSessionsAsync(new RemoveSessionsContext { 
            SessionId = SessionId,
        });
        return RedirectToPage("/ServerSideSessions/Index", new { Token, DisplayNameFilter, SessionIdFilter, SubjectIdFilter, Prev });
    }
}

