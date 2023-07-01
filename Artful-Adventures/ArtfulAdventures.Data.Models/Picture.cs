namespace ArtfulAdventures.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static ArtfulAdventures.Common.DataModelsValidationConstants.PictureConstants;

public class Picture
{
    public Picture()
    {
        this.Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(UrlMaxLength)]
    public string Url { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Owner))]
    public Guid UserId { get; set; }

    public ApplicationUser Owner { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = DateFormat)]
    public DateTime CreatedOn { get; set; }

    public int Likes { get; set; }

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    [ForeignKey(nameof(Challenge))]
    public int? ChallengeId { get; set; }

    public Challenge? Challenge { get; set; } 
}

