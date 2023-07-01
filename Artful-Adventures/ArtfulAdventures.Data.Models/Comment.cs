namespace ArtfulAdventures.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static ArtfulAdventures.Common.DataModelsValidationConstants.CommentConstants;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(AuthorMaxLength)]
    public string Author { get; set; } = null!;

    [Required]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = DateFormat)]
    public DateTime CreatedOn { get; set; }

    [Required]
    [ForeignKey(nameof(Blog))]
    public Guid BlogId { get; set; }

    public Blog Blog { get; set; } = null!;
}

