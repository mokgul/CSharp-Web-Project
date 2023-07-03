namespace ArtfulAdventures.Data.Configuration;

using ArtfulAdventures.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MappingTablesConfiguration :
    IEntityTypeConfiguration<ApplicationUserSkill>,
    IEntityTypeConfiguration<ApplicationUserPicture>,
    IEntityTypeConfiguration<PictureHashTag>
{
    public void Configure(EntityTypeBuilder<ApplicationUserSkill> builder)
    {
        builder.HasKey(au => new { au.UserId, au.SkillId });
    }
    public void Configure(EntityTypeBuilder<ApplicationUserPicture> builder)
    {
        builder.HasKey(ap => new { ap.UserId, ap.PictureId });
    }

    public void Configure(EntityTypeBuilder<PictureHashTag> builder)
    {
        builder.HasKey(ph => new { ph.PictureId, ph.TagId });
    }

}


