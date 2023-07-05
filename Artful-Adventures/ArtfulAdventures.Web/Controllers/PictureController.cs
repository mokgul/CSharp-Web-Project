namespace ArtfulAdventures.Web.Controllers
{
    using System.Drawing;
    using System.IO;
    using System.IO.Pipes;
    using System.Net;
    using System.Security.Claims;
    using System.Text;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using ArtfulAdventures.Web.ViewModels.Picture;

    using FluentFTP;
    using FluentFTP.Client.BaseClient;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static ArtfulAdventures.Common.GeneralApplicationConstants;

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
            //Reads the form data from the request body.
            var form = await Request.ReadFormAsync();

            //Gets the first file and saves it to the specified path.
            var file = form.Files.First();
            var fileName = file.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            //Copy the file to the wwwroot/images folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //Get the URL of the file to be uploaded
            var url = Path.Combine(ftpServerUrl, fileName);

            //Upload the file to the FTP server
            UploadToFtpServer(fileName, filePath);

            //Delete the file from the local folder
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return url;
        }

        private static void UploadToFtpServer(string fileName, string filePath)
        {
            FtpClient client = new FtpClient();
            client.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            client.Host = ftpServerUrl;
            client.Port = ftpPort;
            client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);

            client.Connect();
            client.UploadFile(filePath, fileName);
            client.Disconnect();
        }

        private static void OnValidateCertificate(BaseFtpClient control, FtpSslValidationEventArgs e)
        {
            if (e.PolicyErrors != System.Net.Security.SslPolicyErrors.None)
            {
                // invalid cert, do you want to accept it?
                e.Accept = true;
            }
            else
            {
                e.Accept = true;
            }
        }
    }
}
