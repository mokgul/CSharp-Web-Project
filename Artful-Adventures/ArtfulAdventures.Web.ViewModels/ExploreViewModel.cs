namespace ArtfulAdventures.Web.ViewModels;

using ArtfulAdventures.Web.ViewModels.HashTag;
using ArtfulAdventures.Web.ViewModels.Picture;

using System.Drawing;

public class ExploreViewModel
{
    public ICollection<PictureVisualizeViewModel> PicturesIds { get; set; } = new HashSet<PictureVisualizeViewModel>();
    public List<HashTagViewModel> HashTags { get; set; } = new List<HashTagViewModel>();
}

