namespace ArtfulAdventures.Web.ViewModels.Picture;

using ArtfulAdventures.Web.ViewModels.Comment;
using ArtfulAdventures.Web.ViewModels.HashTag;

public class PictureDetailsViewModel
{
    public string Id { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Owner { get; set; } = null!;

    public string? OwnerPictureUrl { get; set; }

    public int OwnerPicturesCount { get; set; }

    public int OwnerFollowersCount { get; set; }

    public int OwnerFollowingCount { get; set; }

    public bool isFollowed { get; set; }

    public int Likes { get; set; }

    public string Description { get; set; } = null!;

    public ICollection<HashTagViewModel>? HashTags { get; set; }

    public DateTime? CreatedOn { get; set; }

    public ICollection<CommentViewModel>? Comments { get; set; }
}

