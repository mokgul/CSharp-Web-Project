namespace ArtfulAdventures.Web.ViewModels.UserProfile;

using ArtfulAdventures.Web.ViewModels.Picture;
using ArtfulAdventures.Web.ViewModels.Skill;

public class ProfileViewModel
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public string? CityName { get; set; } 

    public string? About { get; set; }

    public ICollection<PictureVisualizeViewModel>? Pictures { get; set; }

    public ICollection<SkillViewModel>? Skills { get; set; }

    public string Contact => this.Email;
}

