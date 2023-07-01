namespace ArtfulAdventures.Web.Controllers;

using ArtfulAdventures.Web.ViewModels.Home;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

public class HomeController : Controller
{

    public HomeController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
