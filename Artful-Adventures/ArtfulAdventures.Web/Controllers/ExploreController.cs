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
        public async Task<IActionResult> All(int page)
        {

            ExploreViewModel model = await _exploreService.GetExploreViewModelAsync(page);

            ViewBag.CurrentPage = page;

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> SortByTag(int page)
        {
            var form = Request.Form;

            List<HashTagViewModel> selectedHashTags = new List<HashTagViewModel>();
            foreach (var item in form)
            {
                if (item.Key.Contains("IsSelected") && item.Value.Contains("true"))
                {
                    HashTagViewModel tag = new HashTagViewModel();
                    string baseKey = item.Key.Substring(0, item.Key.IndexOf(".IsSelected"));
                    tag.Id = int.Parse(form[baseKey + ".Id"]);
                    tag.Name = form[baseKey + ".Name"];
                    tag.IsSelected = true;
                    selectedHashTags.Add(tag);
                }
            }
            var model = new ExploreViewModel()
            {
                HashTags = selectedHashTags
            };
            model = await _exploreService.SortByTagAsync(model, page);

            ViewBag.CurrentPage = page;

            return View(model);
        }
    }
}

