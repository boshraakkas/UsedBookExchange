using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UsedBookExchange.Web.Models;

namespace UsedBookExchange.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Handles unexpected application errors.
    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier;

        _logger.LogError(
            "An unexpected error occurred. RequestId: {RequestId}",
            requestId);

        return View(new ErrorViewModel
        {
            RequestId = requestId
        });
    }

    // Handles HTTP status codes such as 400, 403 and 404.
    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult StatusCode(int code)
    {
        Response.StatusCode = code;

        return View(code);
    }
}