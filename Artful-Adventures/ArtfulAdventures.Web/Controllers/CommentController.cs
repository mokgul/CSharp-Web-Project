namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Web.ViewModels.Comment;

    using Microsoft.AspNetCore.Mvc;

    public class CommentController : Controller
    {
        private readonly ArtfulAdventuresDbContext _data;
        public CommentController(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string content, string pictureId)
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
    }
}
