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
    using Newtonsoft.Json;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using System.Collections.Generic;
    using System.Collections;

    public class ExploreController : Controller
    {
        private readonly IExploreService _exploreService;

        public ExploreController(IExploreService service)
        {
            _exploreService = service;
        }

        [HttpGet]
        public async Task<IActionResult> All(int[] tagsIds, int page)
        {
            ExploreViewModel model = await _exploreService.GetExploreViewModelAsync(page);
            if (tagsIds.Length > 0)
            {
                model = await _exploreService.SortByTagAsync(tagsIds, page);
            }

            ViewBag.CurrentPage = page;

            return View(model);

        }

        //[HttpGet]
        //public async Task<IActionResult> SortByTag(int[] tagsIds, int page)
        //{

        //    ExploreViewModel model = await _exploreService.SortByTagAsync(tagsIds, page);

        //    ViewBag.CurrentPage = page;

        //    return PartialView("_SortByTagPartial", model);

        //}
    }
}

