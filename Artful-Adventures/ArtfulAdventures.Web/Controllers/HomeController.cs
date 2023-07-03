namespace ArtfulAdventures.Web.Controllers;

using System.Diagnostics;

using ArtfulAdventures.Data;
using ArtfulAdventures.Web.ViewModels.Home;

using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ArtfulAdventuresDbContext _data;

    public HomeController(ArtfulAdventuresDbContext data)
    {
        _data = data;
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
