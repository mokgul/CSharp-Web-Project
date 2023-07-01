namespace ArtfulAdventures.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplicationUserSkill
{
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Skill))]
    public int SkillId { get; set; }

    public Skill Skill { get; set; } = null!;
}

