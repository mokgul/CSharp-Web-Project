namespace ArtfulAdventures.Web.ViewModels.Challenges;

    public class ChallengeDetailsViewModel
    {
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Creator { get; set; } = null!;

    public int Participants { get; set; }

    public string Requirements { get; set; } = null!;

    public string Url { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public ICollection<string> Pictures { get; set; } = new HashSet<string>();

    }

