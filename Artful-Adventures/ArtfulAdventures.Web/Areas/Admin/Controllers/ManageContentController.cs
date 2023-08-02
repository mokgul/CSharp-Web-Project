namespace ArtfulAdventures.Web.Areas.Admin.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Web.Areas.Admin.Services.Interfaces;
    using ArtfulAdventures.Web.Configuration;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ArtfulAdventures.Common.GeneralApplicationConstants.Roles;

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
    }
}
