namespace ArtfulAdventures.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ArtfulAdventures.Common.GeneralApplicationConstants.Roles;

    [Authorize(Roles = Administrator)]
    [Area("Admin")]
    public class AdminPanelController : Controller
    {
        public IActionResult Panel()
        {
            return View();
        }
    }
}
