﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace HookaTimes.MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardsController : Controller
{
    private readonly ILogger<DashboardsController> _logger;

    public DashboardsController(ILogger<DashboardsController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("~/Areas/Admin/Views/Pages/Dashboards/Index.cshtml");
    }
}
