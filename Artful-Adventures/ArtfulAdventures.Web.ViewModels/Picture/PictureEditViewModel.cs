namespace ArtfulAdventures.Web.ViewModels.Picture;

using ArtfulAdventures.Web.ViewModels.HashTag;

public class PictureEditViewModel
{
    public string Id { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<HashTagViewModel> HashTags { get; set; } = new List<HashTagViewModel>();
}

