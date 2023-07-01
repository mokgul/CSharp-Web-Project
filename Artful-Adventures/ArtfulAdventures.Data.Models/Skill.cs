namespace ArtfulAdventures.Data.Models;

using ArtfulAdventures.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class Skill
{
    public Skill()
    {

    }
    public Skill(SkillType type)
    {
        this.Type = type.ToString();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } = null!;
}

