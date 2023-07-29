﻿namespace ArtfulAdventures.Data;

using System.Reflection.Emit;

using ArtfulAdventures.Data.Configuration;
using ArtfulAdventures.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

    public DbSet<ApplicationUserPicture> Portfolio { get; set; } = null!;

    public DbSet<ApplicationUserCollection> Collection { get; set; } = null!;

    public DbSet<FollowerFollowing> Follows { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var mappingConfigurer = new MappingTablesConfiguration();
       
        var enumSeedConfigurer = new EnumsSeedingConfiguration();

        var messageConfiguration = new MessageTableConfiguration();

        builder.ApplyConfiguration<PictureHashTag>(mappingConfigurer);
        builder.ApplyConfiguration<ApplicationUserSkill>(mappingConfigurer);
        builder.ApplyConfiguration<ApplicationUserPicture>(mappingConfigurer);
        builder.ApplyConfiguration<ApplicationUserCollection>(mappingConfigurer);
        
        builder.ApplyConfiguration<HashTag>(enumSeedConfigurer);
        builder.ApplyConfiguration<Skill>(enumSeedConfigurer);

        builder.ApplyConfiguration<FollowerFollowing>(new FollowTableConfiguration());

        builder.ApplyConfiguration<Message>(messageConfiguration);

        base.OnModelCreating(builder);
    }
}
