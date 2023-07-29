namespace ArtfulAdventures.Services.Search
{
    using ArtfulAdventures.Data;
    using ArtfulAdventures.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class SearchEngine
    {
        private readonly ArtfulAdventuresDbContext _data;
        public SearchEngine(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }

        public async Task<ICollection<Blog>> SearchBlogs(string query)
        {
            var queryWords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower());
            var dbBlogs = await _data.Blogs.Include(a => a.Author).ToListAsync();
            var blogs = dbBlogs.Where(b => queryWords.All(q => b.Title.ToLower().Contains(q) || b.Content.ToLower().Contains(q) || b.Author.UserName.ToLower().Contains(q)))
                .ToList();
            return blogs;
        }
        public async Task<ICollection<Picture>> SearchPictures(string query)
        {
            var queryWords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower());
            var dbPictures = await _data.Pictures.Include(u => u.Owner).ToListAsync();
            var pictures = dbPictures
                .Where(p => queryWords.All(q => p.Description.ToLower().Contains(q) || p.Owner.UserName.ToLower().Contains(q)))
                .ToList();
            return pictures;
        }

        public async Task<ICollection<Challenge>> SearchChallenges(string query)
        {
            var queryWords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower());
            var dbChallenges = await _data.Challenges.ToListAsync();
            var challenges = dbChallenges.Where(c => queryWords.All(q => c.Title.ToLower().Contains(q) || c.Requirements.ToLower().Contains(q)|| c.Creator.ToLower().Contains(q)))
                .ToList();
            
            return challenges;
        }

        public async Task<ICollection<ApplicationUser>> SearchUsers(string query)
        {
            var queryWords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower());
            var dbUsers = await _data.Users.Include(p => p.Portfolio).ToListAsync();
            var users = dbUsers.Where(u => queryWords.All(q => u.UserName.ToLower().Contains(q) || (u.Name != null && u.Name.ToLower().Contains(q))))
                .ToList();
            return users;
        }

    }
}
