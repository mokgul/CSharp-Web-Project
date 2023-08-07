﻿namespace ArtfulAdventures.Web.Controllers
{
    using ArtfulAdventures.Web.ViewModels.Challenges;
    using System.Xml.Linq;
    

    using Microsoft.AspNetCore.Mvc;
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.Configuration;
    using static ArtfulAdventures.Common.GeneralApplicationConstants;
    using System.Drawing;
    using System.Security.Claims;

    public class ChallengeController : Controller
    {
        private readonly IChallengeService _challengeService;

        public ChallengeController(IChallengeService service)
        {
            _challengeService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page)
        {
            var model = await _challengeService.GetAllAsync(page);
            ViewBag.CurrentPage = page;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChallengeDetails(int id)
        {
            try
            {
                var model = await _challengeService.GetChallengeDetailsAsync(id);
                return View(model);
            }
            catch (NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("GetAll", "Challenge");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Participate(int id)
        {
            var userId = GetUserId();
            try
            {
                var path = await UploadFile();
                if (String.IsNullOrEmpty(path))
                {
                    TempData["Error"] = "Please select a file to upload.";
                    return RedirectToAction("GetAll", "Challenge");
                }
                await _challengeService.ParticipateAsync(id,userId, path);
            }
            catch (NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ChallengeDetails", new { id = id });
            }
            TempData["Success"] = "Your picture was uploaded successfully!";
            return RedirectToAction("ChallengeDetails", new { id = id });
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

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
