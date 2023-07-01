namespace ArtfulAdventures.Data.Models;

using System.ComponentModel.DataAnnotations;

using ArtfulAdventures.Data.Models.Enums;

public class HashTag
{
    public HashTag()
    {
        
    }
    public HashTag(HashTagType type)
    {
        this.Type = type.ToString();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } = null!;
}

