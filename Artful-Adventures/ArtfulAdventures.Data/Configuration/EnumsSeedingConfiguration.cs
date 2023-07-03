namespace ArtfulAdventures.Data.Configuration;

using Microsoft.EntityFrameworkCore;

using ArtfulAdventures.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtfulAdventures.Data.Seeding;

public class EnumsSeedingConfiguration : IEntityTypeConfiguration<HashTag>, IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<HashTag> builder)
    {
        builder.HasData(new HashTagsSeed().HashTags);
    }

    public void Configure(EntityTypeBuilder<Skill> builder)
    {

        builder.HasData(new SkillsSeed().Skills);
    }
}

