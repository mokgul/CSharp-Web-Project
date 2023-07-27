namespace ArtfulAdventures.Web.Controllers
{
    using System.Xml.Linq;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.Configuration;
    using ArtfulAdventures.Web.ViewModels;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using ArtfulAdventures.Web.ViewModels.Picture;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class FollowingController : Controller
    {
        private readonly IFollowingService _followingService;

        public FollowingController(IFollowingService service)
        {
            _followingService = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> All(string sort, int page)
        {
            try
            {
                string username = User!.Identity!.Name!;
                ExploreViewModel model = await _followingService.GetExploreViewModelAsync(sort, page, username);

                ViewBag.Sort = sort;
                ViewBag.CurrentPage = page;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("All", "Following");
            }
        }

    }
}
