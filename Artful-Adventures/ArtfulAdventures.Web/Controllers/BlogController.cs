using ArtfulAdventures.Services.Common;
using Microsoft.AspNetCore.Authorization;

namespace ArtfulAdventures.Web.Controllers
{
    using System.Drawing;
    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.Configuration;
    using ArtfulAdventures.Web.ViewModels.Blog;
    using static ArtfulAdventures.Common.GeneralApplicationConstants;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService service)
        {
            _blogService = service;
        }

        [HttpGet]
        public async Task<IActionResult> CreateBlog()
        {
            ViewBag.Action = "CreateBlog";

            var model = await _blogService.GetBlogViewModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogAddFormModel model)
        {
            var userId = GetUserId();
            try
            {
                var path = await UploadFile();
                if(path == "invalid-file")
                    throw new ArgumentException("Invalid file type. Please upload a valid image.");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _blogService.CreateBlogAsync(model, userId, path);
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateBlog", "Blog");
            }

            TempData["Success"] = "Your blog was published successfully!";

            return RedirectToAction("CreateBlog", "Blog");
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
            var imageValidator = new ValidateFileIsImage();
            if(imageValidator.Validate(file) == false)
                return "invalid-file";
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

        [HttpGet]
        public async Task<IActionResult> BlogDetails(string id)
        {
            try
            {
                var currentUser = GetUserId();
                var model = await _blogService.GetBlogDetailsAsync(id, currentUser);

                return View(model);
            }
            catch (NullReferenceException ex)
            {
                TempData["Error"] = "Blog not found.";
                return RedirectToAction("GetBlogs", new { sort = "", page = 1 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs(string sort, int page)
        {
            try
            {
                var model = await _blogService.GetAllBlogsAsync(sort, page);

                ViewBag.Sort = sort;
                ViewBag.CurrentPage = page;
                ViewBag.Action = "GetBlogs";

                return View(model);
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("GetBlogs", new { sort = "", page = 1 });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Like(string blogId)
        {
            try
            {
                await _blogService.LikeBlogAsync(blogId);
            }
            catch (ArgumentException ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("BlogDetails", new { id = blogId });
            }

            return RedirectToAction("BlogDetails", new { id = blogId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageBlogs(string sort, int page)
        {
            try
            {
                var userId = GetUserId();
                var model = await _blogService.GetAllBlogsForManageAsync(sort, userId, page);

                ViewBag.Sort = sort;
                ViewBag.CurrentPage = page;
                ViewBag.Action = "ManageBlogs";

                return View("GetBlogs", model);
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("ManageBlogs", new { sort = "", page = 1 });
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> EditBlog(string id)
        {
            try
            {
                ViewBag.Action = "EditBlog";
                var model = await _blogService.GetBlogToEditAsync(id);

                return View("CreateBlog", model);
            }
            catch (NullReferenceException ex)
            {
                TempData["NotFound"] = "Blog not found.";
                return RedirectToAction("ManageBlogs", "Blog");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> EditBlog(BlogAddFormModel model)
        {
            string id = model.Id;
            try
            {
                var path = await UploadFile();

                if (!ModelState.IsValid)
                {
                    return RedirectToAction("ManageBlogs", "Blog");
                }

                await _blogService.EditBlogAsync(model, id, path);
            }
            catch (Exception)
            {
                TempData["Error"] = "Something went wrong. Please try again later.";
                return RedirectToAction("ManageBlogs", "Blog");
            }

            TempData["Success"] = "Your blog was edited successfully!";

            return RedirectToAction("ManageBlogs", "Blog");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlog(string id)
        {
            var userId = GetUserId();
            try
            {
                await _blogService.DeleteBlogAsync(id, userId);
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ManageBlogs", "Blog");
            }

            TempData["Success"] = "Your blog was deleted successfully!";

            return RedirectToAction("ManageBlogs", "Blog");
        }

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}