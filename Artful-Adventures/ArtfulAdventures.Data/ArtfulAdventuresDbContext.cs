namespace ArtfulAdventures.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Data.Seeding;

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
        builder.Entity<PictureHashTag>(entity =>
        {
            entity.HasKey(ph => new { ph.PictureId, ph.TagId });
        });

        builder.Entity<ApplicationUserSkill>(entity =>
        {
            entity.HasKey(au => new { au.UserId, au.SkillId });
        });

        builder.Entity<ApplicationUserPicture>(entity =>
        {
            entity.HasKey(ap => new { ap.UserId, ap.PictureId });
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity
                .HasMany(a => a.Portfolio)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder
            .Entity<HashTag>()
            .HasData(new HashTagsSeed().HashTags);

        builder
            .Entity<Skill>()
            .HasData(new SkillsSeed().Skills);

        base.OnModelCreating(builder);
    }
}
