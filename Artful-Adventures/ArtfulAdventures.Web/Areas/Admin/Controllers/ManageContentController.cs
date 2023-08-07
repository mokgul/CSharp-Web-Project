namespace ArtfulAdventures.Web.Areas.Admin.Controllers
{
    using System.Drawing;
    using System.Security.Claims;

    using ArtfulAdventures.Web.Areas.Admin.Models;
    using ArtfulAdventures.Web.Areas.Admin.Services.Interfaces;
    using ArtfulAdventures.Web.Configuration;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ArtfulAdventures.Common.GeneralApplicationConstants.Roles;
    using static ArtfulAdventures.Common.GeneralApplicationConstants;

    [Authorize(Roles = Administrator)]
    [Area("Admin")]
    public class ManageContentController : Controller
    {
        private readonly IManageContentService _manageContentService;

        public ManageContentController(IManageContentService service)
        {
            _manageContentService = service;
        }

        [HttpPost]
        public async Task<IActionResult> DeletePicture(string pictureId, string user)
        {

            try
            {
                var path = await _manageContentService.DeletePictureAsync(pictureId, user);
                await DeleteFromFtpServer.DeleteFile(path);
                TempData["AdminDelete"] = "Picture deleted successfully!";
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData["AdminDeleteError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlog(string blogId, string user)
        {
            try
            {
                await _manageContentService.DeleteBlogAsync(blogId, user);
                TempData["AdminDelete"] = "Blog deleted successfully!";
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData["AdminDeleteError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommentPicture(string pictureId, string commentId)
        {
            try
            {
                await _manageContentService.DeleteCommentPictureAsync(pictureId, commentId);
                TempData["AdminDeleteComment"] = "Comment deleted successfully!";
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData["AdminDeleteCommentError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommentBlog(string blogId, string commentId)
        {
            try
            {
                await _manageContentService.DeleteCommentBlogAsync(blogId, commentId);
                TempData["AdminDeleteComment"] = "Comment deleted successfully!";
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData["AdminDeleteCommentError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateChallenge() 
        {
            var userId = GetUserId();
            try
            {
                var model = await _manageContentService.CreateChallengeGetFormAsync(userId);
                return View(model);
            }
            catch(ArgumentException ex)
            {
                TempData["AdminDeleteError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateChallenge(ChallengeCreateFormModel model)
        {
            
            try
            {
                var path = await UploadFile();
                if(String.IsNullOrEmpty(path))
                {
                    TempData["AdminCreateError"] = "Please upload a picture!";
                    ModelState.AddModelError(path, "Please upload a picture!");
                }
                ModelState.Remove("ImageUrl");
                if(!ModelState.IsValid)
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Path.GetFileName(path)));
                    return View(model);
                }
                int challegeId = await _manageContentService.CreateChallengeAsync(model, path);
                TempData["AdminCreateChallenge"] = "Challenge created successfully!";
                return RedirectToAction("ChallengeDetails", "Challenge", new { id = challegeId, area="" });
            }
            catch (Exception ex)
            {
                TempData["AdminCreateError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChallenge(int challengeId)
        {
            try
            {
                var path = await _manageContentService.DeleteChallengeAsync(challengeId);
                await DeleteFromFtpServer.DeleteFile(path);
                TempData["AdminDelete"] = "Challenge deleted successfully!";
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData["AdminDeleteError"] = ex.Message;
                return RedirectToAction("Panel", "AdminPanel", new { area = "Admin" });
            }
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
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
