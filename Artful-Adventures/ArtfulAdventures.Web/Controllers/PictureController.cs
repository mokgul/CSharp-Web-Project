namespace ArtfulAdventures.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Security.Claims;

    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.Configuration;
    using ArtfulAdventures.Web.ViewModels.Picture;

    using Microsoft.AspNetCore.Mvc;

    using static ArtfulAdventures.Common.GeneralApplicationConstants;

    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }


        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var model = await _pictureService.GetPictureAddFormModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(PictureAddFormModel model)
        {
            string userId = GetUserId();
            try
            {
                var path = await UploadFile();
                if (String.IsNullOrEmpty(path))
                {
                    ModelState.AddModelError("path", "Please select a file to upload.");
                }
                if (!ModelState.IsValid)
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Path.GetFileName(path)));
                    return View(model);
                }
                await _pictureService.UploadPictureAsync(model, userId, path);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            TempData["Success"] = "Your picture was uploaded successfully!";
            return RedirectToAction("Upload", "Picture");
        }

        [HttpGet]
        public async Task<IActionResult> PictureDetails(string id)
        {
            var picture = await _pictureService.GetPictureDetailsAsync(id);

            return View(picture);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(string pictureId)
        {
            var userId = GetUserId();
            var result = await _pictureService.AddToCollectionAsync(pictureId, userId);
            if (string.IsNullOrEmpty(result))
            {
                TempData["Message"] = "You already have this picture in your collection.";
                return RedirectToAction("PictureDetails", new { id = pictureId });
            }
            TempData["Success"] = result;
            return RedirectToAction("PictureDetails", new { id = pictureId });
        }

        [HttpPost]
        public async Task<IActionResult> LikePicture(string pictureId)
        {
            try
            {
                await _pictureService.LikePictureAsync(pictureId);
            }
            catch (ArgumentException ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("PictureDetails", new { id = pictureId });
            }
            return RedirectToAction("PictureDetails", new { id = pictureId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageGetAllPictures(int page)
        {
            ViewBag.CurrentPage = page;
            var userId = GetUserId();
            var model = await _pictureService.ManageGetAllPicturesAsync(userId, page);
            if (model == null)
            {
                return RedirectToAction("UserProfile", "Profile");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPicture(string id)
        {
            var model = await _pictureService.GetPictureToEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPicture(PictureEditViewModel model)
        {
            try
            {
                await _pictureService.EditPictureAsync(model);
            }
            catch (Exception)
            {
                return RedirectToAction("ManageGetAllPictures", "Picture", new { page = 1 });
            }
            TempData["Success"] = "Your picture was edited successfully!";
            return RedirectToAction("ManageGetAllPictures", "Picture", new { page = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePicture(string id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var userId = GetUserId();
            try
            {
                var pathToDelete = await _pictureService.DeletePictureAsync(id, userId);
                await DeleteFromFtpServer.DeleteFile(pathToDelete);
            }
            catch (Exception)
            {
                return RedirectToAction("ManageGetAllPictures", "Picture", new { page = 1 });
            }
            stopwatch.Stop();
            var time = stopwatch.Elapsed;
            TempData["Success"] = "Your picture was deleted successfully!";
            return RedirectToAction("ManageGetAllPictures", "Picture", new { page = 1 });
        }

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        private async Task<string> UploadFile()
        {
            //Reads the form data from the request body.
            var form = await Request.ReadFormAsync();
            if (form.Files.Count == 0)
            {
                return string.Empty;
            }
            //Gets the first file and saves it to the specified path.
            var file = form.Files.First();
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            //Resize the image
            Bitmap newImage = SaveFileLocal.ResizeImage(file);
            SaveFileLocal.SaveAsync(filePath, newImage);

            //Get the URL of the file to be uploaded
            var url = Path.Combine(ftpServerUrl, fileName);

            //Upload the file to the FTP server
            await UploadToFtpServer.UploadFile(fileName, filePath);

            return url;
        }
    }
}
