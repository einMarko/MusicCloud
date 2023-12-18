using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.Infrastructure.Repositories;

namespace MusicCloud.Webapp.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserRepository _users;
        public IndexModel(UserRepository users)
        {
            _users = users;
        }

        public IEnumerable<Application.Model.User> Users =>
            _users.Set
                .Include(u => u.Artists)
                .OrderBy(u => u.Usertype).ThenBy(u => u.Username);
        public void OnGet()
        {

        }
    }
}
