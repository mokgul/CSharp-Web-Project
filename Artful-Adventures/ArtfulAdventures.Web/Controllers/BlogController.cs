namespace ArtfulAdventures.Web.Controllers
{
    using System.Drawing;

    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.Configuration;
    using ArtfulAdventures.Web.ViewModels.Blog;
    using static ArtfulAdventures.Common.GeneralApplicationConstants;

    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

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

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _blogService.CreateBlogAsync(model, userId, path);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
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
            var currentUser = GetUserId();
            var model = await _blogService.GetBlogDetailsAsync(id, currentUser);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs(string sort, int page)
        {

            var model = await _blogService.GetAllBlogsAsync(sort, page);

            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.Action = "GetBlogs";

            return View(model);
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
            var userId = GetUserId();
            var model = await _blogService.GetAllBlogsForManageAsync(sort, userId, page);

            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.Action = "ManageBlogs";

            return View("GetBlogs", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditBlog(string id)
        {
            ViewBag.Action = "EditBlog";
            var model = await _blogService.GetBlogToEditAsync(id);

            return View("CreateBlog", model);
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
