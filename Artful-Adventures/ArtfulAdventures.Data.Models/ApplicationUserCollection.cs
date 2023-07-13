namespace ArtfulAdventures.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ApplicationUserCollection
{
    //Mapping table for Collection property of ApplicationUser
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Picture))]
    public Guid PictureId { get; set; }

    public Picture Picture { get; set; } = null!;
}

