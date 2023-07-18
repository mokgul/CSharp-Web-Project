namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Services.Data.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class UserProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public UserProfileController(IProfileService service)
        {
            _profileService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            var model = await _profileService.GetProfileViewModelAsync(username, GetUserId());
            if (model == null)
            {
                return RedirectToAction("NonExistingProfile");
            }
            return View(model);
        }

        public IActionResult NonExistingProfile()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Portfolio(string username)
        {
            var model = await _profileService.GetPortfolioAsync(username);

            if (model == null)
            {
                TempData["Message"] = "No portfolio yet.";
                return RedirectToAction("Profile", new { username = username });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Favorites(string username)
        {
            var model = await _profileService.GetCollectionAsync(username);

            if (model == null)
            {
                TempData["Message"] = "No favorites yet.";
                return RedirectToAction("Profile", new { username = username });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string username)
        {
            try
            {
                var result = await _profileService.FollowAsync(username, GetUserId());
                if (string.IsNullOrEmpty(result))
                {
                    TempData["Message"] = "You cannot follow yourself.";
                    return RedirectToAction("Profile", new { username = username });
                }
                TempData["Success"] = "You are now following this user.";
                return RedirectToAction("Profile", new { username = username });
            }
            catch
            {
                return RedirectToAction("Profile", new { username = username });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string username)
        {
            try
            {
                await _profileService.UnfollowAsync(username, GetUserId());
                TempData["Success"] = "You are no longer following this user.";
                return RedirectToAction("Profile", new { username = username });
            }
            catch
            {
                TempData["Message"] = "Something went wrong.";
                return RedirectToAction("Profile", new { username = username });
            }

            return RedirectToAction("Profile", new { username = username });
        }

        [HttpGet]
        public async Task<IActionResult> Followers(string username)
        {

            var model = await _profileService.GetFollowersAsync(username);
            if (model == null)
            {
                TempData["Message"] = "No followers yet.";
                return RedirectToAction("Profile", new { username = username });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Following(string username)
        {
            var model = await _profileService.GetFollowingAsync(username);
            if (model == null)
            {
                TempData["Message"] = "No following yet.";
                return RedirectToAction("Profile", new { username = username });
            }
            return View(model);
        }

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
