namespace ArtfulAdventures.Web.ViewModels.UserProfile;


public class ProfilePartialView
{
    public string Username { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public string? CityName { get; set; }

    public bool isFollowed { get; set; }
}

