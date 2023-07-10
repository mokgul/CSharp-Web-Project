namespace ArtfulAdventures.Web.ViewModels.UserProfile;


public class FollowViewModel
{
    public IEnumerable<ProfilePartialView>? Followers { get; set; } = new HashSet<ProfilePartialView>();
}

