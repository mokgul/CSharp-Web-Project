namespace ArtfulAdventures.Web.Controllers
{
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class ExploreController : Controller
    {
        private readonly IExploreService _exploreService;

        public ExploreController(IExploreService service)
        {
            _exploreService = service;
        }

        [HttpGet]
        public async Task<IActionResult> All(int[] tagsIds, int page)
        {
            ExploreViewModel model = await _exploreService.GetExploreViewModelAsync(page);
            if (tagsIds.Length > 0)
            {
                model = await _exploreService.SortByTagAsync(tagsIds, page);
            }

            ViewBag.CurrentPage = page;

            return View(model);

        }
    }
}

