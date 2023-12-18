using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Infrastructure.Repositories;
using MusicCloud.Application.models;
using MusicCloud.Webapp.Dto;
using MusicCloud.Webapp.Services;
using System;
using System.Linq;

namespace MusicCloud.Webapp.Pages.Artists
{
    public class ArtistDetailsModel : PageModel
    {
        private readonly ArtistRepository _artists;
        private readonly AlbumRepository _albums;
        private readonly AuthService _authService;
        public ArtistDetailsModel(ArtistRepository artists, AlbumRepository albums, AuthService authService)
        {
            _artists = artists;
            _albums = albums;
            _authService = authService;
        }
        [BindProperty]
        public Artist Artist { get; set; } = default!;
        public IActionResult OnGet(Guid guid)
        {
            var artist = _artists.Set
                .Include(ar => ar.Albums)
                .Include(ar => ar.Songs)
                .Include(ar => ar.Manager)
                .FirstOrDefault(a => a.Guid == guid);

            if (artist == null)
            {
                return RedirectToPage("/Artists/Index");
            }
            Artist = artist;
            return Page();
        }
        public bool CanEditAlbum(Guid albumGuid) =>
        _authService.IsAdmin ||
            _albums.Set.FirstOrDefault(a => a.Guid == albumGuid)?.Artist.Manager?.Username == _authService.Username;
    }
}
