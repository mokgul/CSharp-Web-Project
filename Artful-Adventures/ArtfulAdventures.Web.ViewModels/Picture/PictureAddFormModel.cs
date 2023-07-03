namespace ArtfulAdventures.Web.ViewModels.Picture;

using System.ComponentModel.DataAnnotations;

using ArtfulAdventures.Web.ViewModels.HashTag;

using static ArtfulAdventures.Common.DataModelsValidationConstants.PictureConstants;

public class PictureAddFormModel
{
    [Required]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
    public string Description { get; set; } = null!;

    [Required]
    public int HashTagId { get; set; }

    public IEnumerable<HashTagViewModel> HashTags { get; set; } = new List<HashTagViewModel>();
}

