namespace ArtfulAdventures.Web.ViewModels.Challenges;

public class ChallengeVisualizeViewModel
{
    public int Id { get; set; }

    public string? PictureUrl { get; set; }

    public string Title { get; set; } = null!;

    public string Creator { get; set; } = null!;

    public int Participants { get; set; }

    public DateTime CreatedOn { get; set; }

}
