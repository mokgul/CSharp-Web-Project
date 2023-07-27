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
            var model = await _blogService.GetBlogDetailsAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs(string sort, int page)
        {
            
                var model = await _blogService.GetAllBlogsAsync(sort, page);

                ViewBag.Sort = sort;
                ViewBag.CurrentPage = page;

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

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
