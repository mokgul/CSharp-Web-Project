namespace ArtfulAdventures.Web.Components
{
    using ArtfulAdventures.Data;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class OffCanvasProfileViewComponent : ViewComponent
    {
        private readonly ArtfulAdventuresDbContext _data;

        public OffCanvasProfileViewComponent(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = User!.Identity!.Name;
            var user = await _data.Users.FirstOrDefaultAsync(x => x.UserName == currentUser);
            var info = new string[]
            {
                user!.UserName, Path.GetFileName(user.Url)!
            };
            return View(info);
        }
    }
}
