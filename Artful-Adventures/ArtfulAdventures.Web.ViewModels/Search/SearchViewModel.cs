namespace ArtfulAdventures.Web.ViewModels.Search;

public class SearchViewModel
{
    public ICollection<PictureSearchViewModel>? Pictures { get; set; }

    public ICollection<ChallengeSearchViewModel>? Challenges { get; set; } 

    public ICollection<BlogSearchViewModel>? Blogs { get; set; }

    public ICollection<UserSearchViewModel>? Users { get; set; }

    public int ResultsCount { get; set; } 
    public int SearchTime { get; set; }
}

