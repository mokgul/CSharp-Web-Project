namespace ArtfulAdventures.Web.Controllers
{
    using ArtfulAdventures.Data.Models;
    using ArtfulAdventures.Data;
    using System.Xml.Linq;

    using ArtfulAdventures.Services.Data.Interfaces;
    using ArtfulAdventures.Web.Configuration;
    using ArtfulAdventures.Web.ViewModels;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using FluentFTP;
    using static ArtfulAdventures.Common.GeneralApplicationConstants;

    public class ExploreController : Controller
    {
        private readonly IExploreService _exploreService;

        public ExploreController(IExploreService service)
        {
            _exploreService = service;
        }

        [HttpGet]
        public async Task<IActionResult> All(int page)
        {

            //await DownloadFromFtpServer.DownloadData();
            
            ExploreViewModel model = await _exploreService.GetExploreViewModelAsync(page);

            ViewBag.CurrentPage = page;


            return View(model);

        }

    }
}

