using ArtfulAdventures.Services.Common;
using ArtfulAdventures.Services.Data;
using ArtfulAdventures.Web.ViewModels.Picture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ArtfulAdventures.Web.Controllers;

using System.Diagnostics;

using ArtfulAdventures.Data;
using ArtfulAdventures.Web.ViewModels.Blog;
using ArtfulAdventures.Web.ViewModels.Home;
using ArtfulAdventures.Web.ViewModels.Search;

using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ArtfulAdventuresDbContext _data;

    public HomeController(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _data.Pictures.Select(p => new PictureVisualizeViewModel()
        {
            Id = p.Id.ToString(),
            PictureUrl = Path.GetFileName(p.Url),
        }).Take(20).ToListAsync();
        model = FilterBrokenUrls.FilterAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AboutUs()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Privacy()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Search(string query, int page = 1)
    {
        int pageSize = 10;
        int skip = (page - 1) * pageSize;
        if(query == null)
        {
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Query = query;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var search = new SearchEngine(_data);
        
        var pictures = await search.SearchPictures(query);
        var blogs = await search.SearchBlogs(query);
        var users = await search.SearchUsers(query);
        var challenges = await search.SearchChallenges(query);

        var picturesModel = pictures.Select(p => new PictureSearchViewModel
        {
            Id = p.Id.ToString(),
            Description = p.Description,
            PictureUrl = Path.GetFileName(p.Url),
            Owner = p.Owner.UserName,
            CreatedOn = p.CreatedOn,
        }).ToList();

        var blogsModel = blogs.Select(b => new BlogSearchViewModel
        {
            Id = b.Id.ToString(),
            Title = b.Title,
            Content = b.Content,
            Author = b.Author.UserName,
            CreatedOn = b.CreatedOn,
        }).ToList();

        var usersModel = users.Select(u => new UserSearchViewModel
        {
            Id = u.Id.ToString(),
            UserName = u.UserName,
            Name = u.Name,
            Uploads = u.Portfolio.Count,
        }).ToList();

        var challengesModel = challenges.Select(c => new ChallengeSearchViewModel
        {
            Id = c.Id,
            Creator = c.Creator,
            CreatedOn = c.CreatedOn,
            Requirements = c.Requirements,
        }).ToList();

        picturesModel = picturesModel.Skip(skip).Take(pageSize).ToList();
        blogsModel = blogsModel.Skip(skip).Take(pageSize).ToList();
        usersModel = usersModel.Skip(skip).Take(pageSize).ToList();
        challengesModel = challengesModel.Skip(skip).Take(pageSize).ToList();

        var model = new SearchViewModel
        {
            Pictures = picturesModel,
            Blogs = blogsModel,
            Users = usersModel,
            Challenges = challengesModel,
            ResultsCount = pictures.Count + blogs.Count + users.Count + challenges.Count,
        };

        stopwatch.Stop();
        model.SearchTime = (int)stopwatch.Elapsed.TotalMilliseconds;

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
