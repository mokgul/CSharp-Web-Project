namespace ArtfulAdventures.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Data.Seeding;
using ArtfulAdventures.Data.Configuration;

public class ArtfulAdventuresDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ArtfulAdventuresDbContext(DbContextOptions<ArtfulAdventuresDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; } = null!;

    public DbSet<Challenge> Challenges { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<HashTag> HashTags { get; set; } = null!;

    public DbSet<Picture> Pictures { get; set; } = null!;

    public DbSet<Skill> Skills { get; set; } = null!;

    public DbSet<PictureHashTag> PicturesHashTags { get; set; } = null!;

    public DbSet<ApplicationUserSkill> ApplicationUsersSkills { get; set; } = null!;

    public DbSet<ApplicationUserPicture> ApplicationUsersPictures { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        var mappingConfigurer = new MappingTablesConfiguration();
        var oneToManyConfigurer = new OneToManyConfiguration();
        var enumSeedConfigurer = new EnumsSeedingConfiguration();

        builder.ApplyConfiguration<PictureHashTag>(mappingConfigurer);
        builder.ApplyConfiguration<ApplicationUserSkill>(mappingConfigurer);
        builder.ApplyConfiguration<ApplicationUserPicture>(mappingConfigurer);

        builder.ApplyConfiguration<ApplicationUser>(oneToManyConfigurer);
        
        builder.ApplyConfiguration<HashTag>(enumSeedConfigurer);
        builder.ApplyConfiguration<Skill>(enumSeedConfigurer);

        base.OnModelCreating(builder);
    }
}
