namespace ArtfulAdventures.Web.Controllers
{
    using System;
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
        public async Task<ActionResult> Upload(PictureAddFormModel model)
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
            var fileName = file.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            //Resize the image
            Bitmap newImage = ResizeImage(file);
            SaveFileLocal(filePath, newImage);

            //Get the URL of the file to be uploaded
            var url = Path.Combine(ftpServerUrl, fileName);

            //Upload the file to the FTP server
            await UploadToFtpServer.UploadFile(fileName, filePath);

            return url;
        }

        private static void SaveFileLocal(string filePath, Bitmap newImage)
        {
            using (var stream = new MemoryStream())
            {
                newImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                var imageBytes = stream.ToArray();

                //Save the file to the local folder
                using (var str = new FileStream(
        filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                {
                    str.Write(imageBytes, 0, imageBytes.Length);
                }
            }
        }

        private static Bitmap ResizeImage(IFormFile file)
        {
            Image image = Image.FromStream(file.OpenReadStream(), true, true);
            var newWidth = image.Width;
            var newHeight = image.Height;
            if (image.Width > 200 && image.Height > 200)
            {
                newWidth = (int)(image.Width * 0.5);
                newHeight = (int)(image.Height * 0.5);
            }

            var newImage = new Bitmap(image, new Size(newWidth, newHeight));
            return newImage;
        }
    }
}
