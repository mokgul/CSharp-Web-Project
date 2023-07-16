namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Services.Data;
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.ViewModels.Picture;
    using ArtfulAdventures.Web.ViewModels.Skill;
    using ArtfulAdventures.Web.ViewModels.UserProfile;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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
            var model = _profileService.GetProfileViewModelAsync(username);
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

        [HttpPost]
        public async Task<IActionResult> Follow(string username)
        {
            var userVisited = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.UserName == username);
            var userVisitor = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.Id.ToString() == GetUserId());
            if (userVisited!.Id == userVisitor!.Id)
            {
                TempData["Message"] = "You cannot follow yourself.";
                return RedirectToAction("Profile", new { username = username });
            }
            var userFollow = new FollowerFollowing()
            {
                Follower = userVisitor,
                FollowerId = userVisitor!.Id,
                Followed = userVisited,
                FollowedId = userVisited!.Id,
            };
            if (!userVisited!.Followers.Any(f => f.FollowerId == userVisitor!.Id)
                && !userVisitor!.Following.Any(f => f.FollowedId == userVisited!.Id))
            {
                userVisited!.Followers.Add(userFollow);
                userVisitor!.Following.Add(userFollow!);
                await _data.SaveChangesAsync();
            };
            return RedirectToAction("Profile", new { username = username });
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string username)
        {
            var userVisited = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.UserName == username);
            var userVisitor = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.Id.ToString() == GetUserId());


            if (userVisited!.Followers.Any(f => f.FollowerId == userVisitor!.Id)
                && userVisitor!.Following.Any(f => f.FollowedId == userVisited!.Id))
            {
                var userFollow = userVisited!.Followers.FirstOrDefault(f => f.FollowerId == userVisitor!.Id);
                userVisited!.Followers.Remove(userFollow!);
                userVisitor!.Following.Remove(userFollow!);
                await _data.SaveChangesAsync();
            };

            return RedirectToAction("Profile", new { username = username });
        }

        [HttpGet]
        public async Task<IActionResult> Followers(string username)
        {

            var user = await _data.Users.Include(m => m.Followers).Include(m => m.Following).FirstOrDefaultAsync(u => u.UserName == username);
            if (user.Followers.Count == 0)
            {
                TempData["Message"] = "No followers yet.";
                return RedirectToAction("Profile", new { username = username });
            }
            var followers = user!.Followers.Select(f => new ProfilePartialView()
            {
                Username = _data.Users.FirstOrDefault(u => u.Id == f.FollowerId)?.UserName,
                ProfilePictureUrl = f.Follower.Url,
                Name = f.Follower.Name,
                Bio = f.Follower.Bio,
                CityName = f.Follower.CityName,
            }).ToList();
            var model = new FollowViewModel()
            {
                Followers = followers,
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Following(string username)
        {
            var user = await _data.Users.Include(m => m.Followers).Include(m => m.Following).FirstOrDefaultAsync(u => u.UserName == username);
            if (user.Following.Count == 0)
            {
                TempData["Message"] = "No following yet.";
                return RedirectToAction("Profile", new { username = username });
            }
            var following = user!.Following.Select(f => new ProfilePartialView()
            {
                Username = _data.Users.FirstOrDefault(u => u.Id == f.FollowedId)?.UserName,
                ProfilePictureUrl = f.Followed.Url,
                Name = f.Followed.Name,
                Bio = f.Followed.Bio,
                CityName = f.Followed.CityName,
            }).ToList();
            var model = new FollowViewModel()
            {
                Followers = following,
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Portfolio(string username)
        {

            var user = await _data.Users
                .Include(m => m.Followers)
                .Include(s => s.Following)
                .Include(p => p.Portfolio)
                .Include(s => s.ApplicationUsersSkills)
                .FirstOrDefaultAsync(u => u.UserName == username);
            if (user.Portfolio.Count == 0)
            {
                TempData["Message"] = "No portfolio yet.";
                return RedirectToAction("Profile", new { username = username });
            }

            var pictures = user!.Portfolio.Select(p => new PictureVisualizeViewModel()
            {
                Id = p.PictureId.ToString(),
                PictureUrl = Path.GetFileName(_data.Pictures.FirstOrDefault(i => i.Id == p.PictureId)!.Url),
            }).ToList();
            //when i move this to the service i need to uncomment the line below
            //pictures = await FilterBrokenUrls.FilterAsync(pictures);
            var model = new PortfolioViewModel()
            {
                Pictures = pictures,
            };

            return View(model);
        }


    }
}
