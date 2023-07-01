namespace ArtfulAdventures.Data.Seeding;

using ArtfulAdventures.Data.Models.Enums;
using ArtfulAdventures.Data.Models;

public class SkillsSeed
{
    public readonly ICollection<Skill> Skills = new HashSet<Skill>();
    public SkillsSeed()
    {
        Skills = GenerateHashSkills();
    }

    private ICollection<Skill> GenerateHashSkills()
    {
        int id = 0;
        foreach (var type in Enum.GetValues(typeof(SkillType)))
        {
            id++;
            Skills.Add(new Skill((SkillType)type) { Id = id });
        }
        return Skills;
    }
}

