﻿namespace ArtfulAdventures.Web.Controllers
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

    public class ExploreController : Controller
    {
        private readonly IExploreService _exploreService;

        public ExploreController(IExploreService service)
        {
            _exploreService = service;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            
            await DownloadFromFtpServer.DownloadData();
            ExploreViewModel model = await  _exploreService.GetExploreViewModelAsync();

            return View(model);

        }
        
    }
}

