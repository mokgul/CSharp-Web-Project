namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Web.ViewModels.Comment;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static ArtfulAdventures.Common.GeneralApplicationConstants.Roles;

    [Authorize]
    public class CommentController : Controller
    {
        private readonly ArtfulAdventuresDbContext _data;
        public CommentController(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }
        [HttpPost]
        public async Task<IActionResult> AddCommentPicture(string content, string pictureId)
        {
            if(string.IsNullOrEmpty(content))
            {
                return RedirectToAction("PictureDetails", "Picture", new { id = pictureId });
            }
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user =  _data.Users.FirstOrDefault(x => x.Id.ToString() == userId);

            var comment = new Comment
            {
                Author = user!.UserName,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                PictureId = Guid.Parse(pictureId),
            };
            await _data.Comments.AddAsync(comment);
            await _data.SaveChangesAsync();
            return RedirectToAction("PictureDetails", "Picture", new { id = pictureId });
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentBlog(string content, string blogId)
        {
            if (string.IsNullOrEmpty(content))
            {
                return RedirectToAction("BlogDetails", "Blog", new { id = blogId });
            }
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = _data.Users.FirstOrDefault(x => x.Id.ToString() == userId);

            var comment = new Comment
            {
                Author = user!.UserName,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                BlogId = Guid.Parse(blogId),
            };
            await _data.Comments.AddAsync(comment);
            await _data.SaveChangesAsync();
            return RedirectToAction("BlogDetails", "Blog", new { id = blogId });
        }

    }
}
