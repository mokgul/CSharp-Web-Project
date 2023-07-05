namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using ArtfulAdventures.Web.ViewModels.Picture;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var model = await _pictureService.GetPictureAddFormModelAsync();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(PictureAddFormModel model)
        {
            string userId = GetUserId();

            try
            {
                if (ModelState.IsValid)
                {
                    var path = await UploadFile();
                    await _pictureService.UploadPictureAsync(model, userId, path);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            
            return RedirectToAction("Index", "Home");
        }

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        private async Task<string> UploadFile()
        {
            var form = await Request.ReadFormAsync();
            var file = form.Files.First();

            var id = file.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", id + ".png");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path;
        }

    }
}
