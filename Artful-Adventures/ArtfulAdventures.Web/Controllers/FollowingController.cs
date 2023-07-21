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
        public async Task<IActionResult> All(int[] tagsIds, int page)
        {
            string username = User.Identity.Name;
            ExploreViewModel model = await _followingService.GetExploreViewModelAsync(page, username);
            if (tagsIds.Length > 0)
            {
                model = await _followingService.SortByTagAsync(tagsIds, page, username);
            }

            ViewBag.CurrentPage = page;
            return View(model);

        }

    }
}
