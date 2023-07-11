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
        public async Task<IActionResult> All()
        {
            await DownloadFromFtpServer.DownloadData();
            var user = await _data.Users.Include(f => f.Following).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
            {
                Id = h.Id,
                Name = h.Type
            }).ToListAsync();

            var usersFollowed = user.Following.Select(p => p.FollowedId).ToList();
            var pictures = await _data.Pictures.Where(p => usersFollowed.Contains(p.Owner.Id)).Select(p => new PictureVisualizeViewModel()
            {
                PictureUrl = p.Url,
            }).ToArrayAsync();
            var ftpRemotePaths = new List<string>();
            foreach (var url in pictures)
            {
                var path = Path.GetFileName(url.PictureUrl);
                if (!ftpRemotePaths.Contains(path))
                    ftpRemotePaths.Add(path);
            }
            ExploreViewModel model = new ExploreViewModel()
            {
                HashTags = hashtags,
                PicturesIds = ftpRemotePaths.ToArray().Select(p => new PictureVisualizeViewModel()
                {
                    PictureUrl = p
                }).ToArray(),
            };

            return View(model);

        }
    }
}
