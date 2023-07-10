namespace ArtfulAdventures.Web.ViewModels.UserProfile;

using ArtfulAdventures.Web.ViewModels.Picture;
using ArtfulAdventures.Web.ViewModels.Skill;

public class ProfileViewModel : ProfilePartialView
{

    public string Email { get; set; } = null!;

    public string? About { get; set; }

 

    public ICollection<PictureVisualizeViewModel>? Pictures { get; set; }

    public ICollection<SkillViewModel>? Skills { get; set; }

    public string Contact => this.Email;
}

