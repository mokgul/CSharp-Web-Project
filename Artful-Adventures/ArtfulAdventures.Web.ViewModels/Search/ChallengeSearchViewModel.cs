namespace ArtfulAdventures.Web.ViewModels.Search;

public class ChallengeSearchViewModel
{
    public int Id { get; set; }

    public string Creator { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string Requirements { get; set; } = null!;

}

