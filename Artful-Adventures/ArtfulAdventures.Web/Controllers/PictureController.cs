namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using ArtfulAdventures.Web.ViewModels.Picture;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class PictureController : Controller
    {
        private readonly ArtfulAdventuresDbContext _data;

        public PictureController(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Upload()
        {

            var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
            {
                Id = h.Id,
                Name = h.Type
            }).ToListAsync();

            PictureAddFormModel model = new PictureAddFormModel()
            {
                HashTags = hashtags
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(PictureAddFormModel model)
        {
            var form = await Request.ReadFormAsync();
            var file = form.Files.First();

            var id = file.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", id + ".png");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string userId = GetUserId();
            //INCLUDE IS A MUST IF I WANT TO USE THE NAVIGATION PROPERTIES
            var user = await _data.Users.Include(m => m.ApplicationUsersPictures).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            try
            {
                if (ModelState.IsValid)
                {
                    Picture picture = new Picture()
                    {
                        Url = path,
                        UserId = user.Id,
                        Owner = user,
                        CreatedOn = DateTime.UtcNow,
                        Likes = 0,
                        Description = model.Description,
                    };
                    picture.PicturesHashTags.Add(new PictureHashTag()
                    {
                        Picture = picture,
                        PictureId = picture.Id,
                        Tag = await _data.HashTags.FirstOrDefaultAsync(h => h.Id == model.HashTagId),
                        TagId = model.HashTagId
                    });
                    picture.ApplicationUsersPictures.Add(new ApplicationUserPicture()
                    {
                        Picture = picture,
                        PictureId = picture.Id,
                        User = user,
                        UserId = user.Id
                    });

                    await _data.Pictures.AddAsync(picture);
                    await _data.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return RedirectToAction("Index", "Home");
        }

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
