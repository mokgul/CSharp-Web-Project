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
        public async Task<IActionResult> All(string sort, int page)
        {
            try
            {
                ExploreViewModel model = await _exploreService.GetExploreViewModelAsync(sort, page);

                ViewBag.Sort = sort;
                ViewBag.CurrentPage = page;

                return View(model);
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("All", "Explore");
            }

        }
    }
}

