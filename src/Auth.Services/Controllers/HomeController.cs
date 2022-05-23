using Auth.Services.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Services.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    [HttpGet("error")]
    public IActionResult HandleError()
    {
        return View("Error");
    }

    [HttpGet("not-found")]
    public IActionResult HandleNotFound()
    {
        return View("NotFound");
    }
}