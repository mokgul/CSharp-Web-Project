namespace ArtfulAdventures.Web.Controllers
{
    using System.Security.Claims;

    using ArtfulAdventures.Data;
    using ArtfulAdventures.Web.ViewModels.Picture;
    using ArtfulAdventures.Web.ViewModels.Skill;
    using ArtfulAdventures.Web.ViewModels.UserProfile;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class UserProfileController : Controller
    {
        private readonly ArtfulAdventuresDbContext _data;

        public UserProfileController(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            string userId = GetUserId();
            var user = await _data.Users.Include(m => m.ApplicationUsersPictures).Include(s => s.ApplicationUsersSkills).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            ICollection<SkillViewModel> skills = user!.ApplicationUsersSkills.Select(sa => new SkillViewModel()
            {
                Id = sa.SkillId,
                Name = _data.Skills.FirstOrDefault(s => s.Id == sa.SkillId)!.Type,
            }).ToList();

            ICollection<PictureVisualizeViewModel> pictures = user!.ApplicationUsersPictures.Select(p => new PictureVisualizeViewModel()
            {
                PictureUrl = Path.GetFileName(_data.Pictures.FirstOrDefault(i => i.Id == p.PictureId)!.Url),
            }).ToList();

            var model = new ProfileViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                ProfilePictureUrl = user.Url,
                Name = user.Name,
                Bio = user.Bio,
                About = user.About,
                CityName = user.CityName,
                Skills = skills,
                Pictures = pictures,
            };
            return View(model);
        }

        private string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
