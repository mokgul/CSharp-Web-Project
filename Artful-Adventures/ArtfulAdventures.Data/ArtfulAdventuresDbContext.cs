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

    public DbSet<PicturesHashTags> PicturesHashTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<PicturesHashTags>(entity =>
        {
            entity.HasKey(ph => new { ph.PictureId, ph.TagId });
        });

        builder
            .Entity<HashTag>()
            .HasData(new HashTagsSeed().HashTags);

        base.OnModelCreating(builder);
    }
}
