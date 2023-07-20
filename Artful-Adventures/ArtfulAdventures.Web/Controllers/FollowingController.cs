namespace ArtfulAdventures.Web.Controllers
{
    using System.Xml.Linq;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Web.Configuration;
    using ArtfulAdventures.Web.ViewModels;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using ArtfulAdventures.Web.ViewModels.Picture;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class FollowingController : Controller
    {
        private readonly ArtfulAdventuresDbContext _data;

        public FollowingController(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> All(int page, string username  )
        {
            
            int pageSize = 20;
            int skip = (page - 1) * pageSize;
            var user = await _data.Users.Include(f => f.Following).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
            {
                Id = h.Id,
                Name = h.Type
            }).ToListAsync();

            var usersFollowed = user.Following.Select(p => p.FollowedId).ToList();
            var pictures = await _data.Pictures.Where(p => usersFollowed.Contains(p.Owner.Id)).Select(p => new PictureVisualizeViewModel()
            {
                Id = p.Id.ToString(),
                PictureUrl = Path.GetFileName(p.Url),
            }).ToListAsync();
            
            //when i move this to the service i need to uncomment the line below
            //pictures = FilterBrokenUrls.FilterAsync(pictures);

            ExploreViewModel model = new ExploreViewModel()
            {
                HashTags = hashtags,
                PicturesIds = pictures.Skip(skip).Take(pageSize).ToList()
            };
            ViewBag.CurrentPage = page;
            return View(model);

        }

    }
}
