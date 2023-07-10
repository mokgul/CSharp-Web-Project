﻿// <auto-generated />
using System;
using ArtfulAdventures.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArtfulAdventures.Data.Migrations
{
    [DbContext(typeof(ArtfulAdventuresDbContext))]
    [Migration("20230710124119_user-constants-updated")]
    partial class userconstantsupdated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ApplicationUserApplicationUser", b =>
                {
                    b.Property<Guid>("FollowersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowingId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FollowersId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("ApplicationUserApplicationUser");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("About")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CityName")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Url")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.ApplicationUserPicture", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PictureId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "PictureId");

                    b.HasIndex("PictureId");

                    b.ToTable("ApplicationUsersPictures");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.ApplicationUserSkill", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("ApplicationUsersSkills");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Challenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Participants")
                        .HasColumnType("int");

                    b.Property<string>("Requirements")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("Id");

                    b.ToTable("Challenges");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("BlogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PictureId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("PictureId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.HashTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HashTags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Abstract"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Anatomy"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Animals_Wildlife"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Anime_Manga"
                        },
                        new
                        {
                            Id = 5,
                            Type = "Automotive"
                        },
                        new
                        {
                            Id = 6,
                            Type = "Architectural_Design"
                        },
                        new
                        {
                            Id = 7,
                            Type = "Art"
                        },
                        new
                        {
                            Id = 8,
                            Type = "Artist"
                        },
                        new
                        {
                            Id = 9,
                            Type = "Artwork"
                        },
                        new
                        {
                            Id = 10,
                            Type = "Character_Design"
                        },
                        new
                        {
                            Id = 11,
                            Type = "Comic"
                        },
                        new
                        {
                            Id = 12,
                            Type = "Creative"
                        },
                        new
                        {
                            Id = 13,
                            Type = "Drawing"
                        },
                        new
                        {
                            Id = 14,
                            Type = "Digital_Аrt"
                        },
                        new
                        {
                            Id = 15,
                            Type = "Digital_Painting"
                        },
                        new
                        {
                            Id = 16,
                            Type = "Digital_Drawing"
                        },
                        new
                        {
                            Id = 17,
                            Type = "Digital_Illustration"
                        },
                        new
                        {
                            Id = 18,
                            Type = "Digital_Portrait"
                        },
                        new
                        {
                            Id = 19,
                            Type = "Digital_Sketching"
                        },
                        new
                        {
                            Id = 20,
                            Type = "Environmental_Design"
                        },
                        new
                        {
                            Id = 21,
                            Type = "Fan_Art"
                        },
                        new
                        {
                            Id = 22,
                            Type = "Fantasy"
                        },
                        new
                        {
                            Id = 23,
                            Type = "Game_Art"
                        },
                        new
                        {
                            Id = 24,
                            Type = "Graffiti"
                        },
                        new
                        {
                            Id = 25,
                            Type = "Illustration"
                        },
                        new
                        {
                            Id = 26,
                            Type = "Mechanical_Design"
                        },
                        new
                        {
                            Id = 27,
                            Type = "Painting"
                        },
                        new
                        {
                            Id = 28,
                            Type = "Pixel_Art"
                        },
                        new
                        {
                            Id = 29,
                            Type = "Portrait"
                        },
                        new
                        {
                            Id = 30,
                            Type = "Props"
                        },
                        new
                        {
                            Id = 31,
                            Type = "Realism"
                        },
                        new
                        {
                            Id = 32,
                            Type = "Science_Fiction"
                        },
                        new
                        {
                            Id = 33,
                            Type = "Sketch"
                        },
                        new
                        {
                            Id = 34,
                            Type = "Street_Art"
                        },
                        new
                        {
                            Id = 35,
                            Type = "Vector_Art"
                        },
                        new
                        {
                            Id = 36,
                            Type = "Water_Color"
                        },
                        new
                        {
                            Id = 37,
                            Type = "Weapons"
                        },
                        new
                        {
                            Id = 38,
                            Type = "Unreal_Engine"
                        });
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ChallengeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("UserId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.PictureHashTag", b =>
                {
                    b.Property<Guid>("PictureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("PictureId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PicturesHashTags");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "_3D_Modeling"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Abstract"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Acrylic_Painting"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Anatomy_Knowledge"
                        },
                        new
                        {
                            Id = 5,
                            Type = "Animal_Anatomy_Knowledge"
                        },
                        new
                        {
                            Id = 6,
                            Type = "Architecture_Design"
                        },
                        new
                        {
                            Id = 7,
                            Type = "Branding_Design"
                        },
                        new
                        {
                            Id = 8,
                            Type = "Brushwork_Techniques"
                        },
                        new
                        {
                            Id = 9,
                            Type = "Cartooning_Skills"
                        },
                        new
                        {
                            Id = 10,
                            Type = "Character_Design"
                        },
                        new
                        {
                            Id = 11,
                            Type = "Charcoal_Drawing"
                        },
                        new
                        {
                            Id = 12,
                            Type = "Color_Theory_And_Mixing"
                        },
                        new
                        {
                            Id = 13,
                            Type = "Comic_Book_Illustration"
                        },
                        new
                        {
                            Id = 14,
                            Type = "Composition"
                        },
                        new
                        {
                            Id = 15,
                            Type = "Concept_Art"
                        },
                        new
                        {
                            Id = 16,
                            Type = "Creature_Design"
                        },
                        new
                        {
                            Id = 17,
                            Type = "Digital_Painting"
                        },
                        new
                        {
                            Id = 18,
                            Type = "Digital_Sketching"
                        },
                        new
                        {
                            Id = 19,
                            Type = "Digital_Sculpting"
                        },
                        new
                        {
                            Id = 20,
                            Type = "Drawing_From_Life"
                        },
                        new
                        {
                            Id = 21,
                            Type = "Environment_Design"
                        },
                        new
                        {
                            Id = 22,
                            Type = "Game_Design"
                        },
                        new
                        {
                            Id = 23,
                            Type = "Graphic_Design"
                        },
                        new
                        {
                            Id = 24,
                            Type = "Illustration"
                        },
                        new
                        {
                            Id = 25,
                            Type = "Ink_Drawing"
                        },
                        new
                        {
                            Id = 26,
                            Type = "Landscape_Painting"
                        },
                        new
                        {
                            Id = 27,
                            Type = "Layout_Design"
                        },
                        new
                        {
                            Id = 28,
                            Type = "Light_And_Shadow"
                        },
                        new
                        {
                            Id = 29,
                            Type = "Logo_Design"
                        },
                        new
                        {
                            Id = 30,
                            Type = "Oil_Painting"
                        },
                        new
                        {
                            Id = 31,
                            Type = "Pastel_Drawing"
                        },
                        new
                        {
                            Id = 32,
                            Type = "Perspective_Drawing"
                        },
                        new
                        {
                            Id = 33,
                            Type = "Photoshop"
                        },
                        new
                        {
                            Id = 34,
                            Type = "Portrait_Painting"
                        },
                        new
                        {
                            Id = 35,
                            Type = "Prop_Design"
                        },
                        new
                        {
                            Id = 36,
                            Type = "Proportions_And_Measurements"
                        },
                        new
                        {
                            Id = 37,
                            Type = "Quick_Sketch"
                        },
                        new
                        {
                            Id = 38,
                            Type = "Realistic"
                        },
                        new
                        {
                            Id = 39,
                            Type = "Shading_Techniques"
                        },
                        new
                        {
                            Id = 40,
                            Type = "Tradiotiona_lArt"
                        },
                        new
                        {
                            Id = 41,
                            Type = "Vehicle_Design"
                        },
                        new
                        {
                            Id = 42,
                            Type = "Visual_Effects"
                        },
                        new
                        {
                            Id = 43,
                            Type = "Watercolor_Painting"
                        },
                        new
                        {
                            Id = 44,
                            Type = "Weapon_Design"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ApplicationUserApplicationUser", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("FollowersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.ApplicationUserPicture", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.Picture", "Picture")
                        .WithMany("ApplicationUsersPictures")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", "User")
                        .WithMany("ApplicationUsersPictures")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Picture");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.ApplicationUserSkill", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.Skill", "Skill")
                        .WithMany("ApplicationUsersSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", "User")
                        .WithMany("ApplicationUsersSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Blog", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", "Author")
                        .WithMany("Blogs")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Comment", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtfulAdventures.Data.Models.Picture", null)
                        .WithMany("Comments")
                        .HasForeignKey("PictureId");

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Picture", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.Challenge", "Challenge")
                        .WithMany("Pictures")
                        .HasForeignKey("ChallengeId");

                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", "Owner")
                        .WithMany("Portfolio")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Challenge");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.PictureHashTag", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.Picture", "Picture")
                        .WithMany("PicturesHashTags")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtfulAdventures.Data.Models.HashTag", "Tag")
                        .WithMany("PicturesHashTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Picture");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("ArtfulAdventures.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("ApplicationUsersPictures");

                    b.Navigation("ApplicationUsersSkills");

                    b.Navigation("Blogs");

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Blog", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Challenge", b =>
                {
                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.HashTag", b =>
                {
                    b.Navigation("PicturesHashTags");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Picture", b =>
                {
                    b.Navigation("ApplicationUsersPictures");

                    b.Navigation("Comments");

                    b.Navigation("PicturesHashTags");
                });

            modelBuilder.Entity("ArtfulAdventures.Data.Models.Skill", b =>
                {
                    b.Navigation("ApplicationUsersSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
