using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArtfulAdventures.Data;
namespace ArtfulAdventures.Web.Configuration;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Services.Data;
using ArtfulAdventures.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString =
            builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ArtfulAdventuresDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
            .AddEntityFrameworkStores<ArtfulAdventuresDbContext>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IPictureService, PictureService>();
        builder.Services.AddScoped<IExploreService, ExploreService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IFollowingService, FollowingService>();
        builder.Services.AddScoped<IBlogService, BlogService>();

        var app = builder.Build();

        SyncData.ExecuteAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        
        app.Run();

    }
}
