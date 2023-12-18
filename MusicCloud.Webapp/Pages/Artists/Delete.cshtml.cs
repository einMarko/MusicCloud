using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.Infrastructure.Repositories;
using MusicCloud.Application.models;
using MusicCloud.Webapp.Services;

namespace MusicCloud.Webapp.Pages.Artists
{
    [Authorize]
    public class DeleteArtistsModel : PageModel
    {
        private readonly ArtistRepository _artists;
        private readonly AuthService _authService;
        public DeleteArtistsModel(ArtistRepository artists, AuthService authService)
        {
            _artists = artists;
            _authService = authService;
        }
        [TempData]
        public string? Message { get; set; }
        public Artist Artist { get; set; } = default!;
        public IActionResult OnPostCancel() { return RedirectToPage("/Artists/Index"); }
        public IActionResult OnPostDelete(Guid guid)
        {
            var artist = _artists.FindByGuid(guid);
            if (artist == null) { return RedirectToPage("/Artists/Index"); }
            var (success, message) = _artists.Delete(artist);
            if (!success) { Message = message; }
            return RedirectToPage("/Artists/Index");
        }
        public IActionResult OnGet(Guid guid)
        {
            var artist = _artists.Set
                .Include(ar => ar.Manager)
                .FirstOrDefault(ar => ar.Guid == guid);
            if (artist == null) { return RedirectToPage("/Artists/Index"); }
            Artist = artist;
            if(!_authService.IsAdmin && _authService.Username != artist.Manager.Username) { return new ForbidResult(); }
            return Page();
        }
    }
}
