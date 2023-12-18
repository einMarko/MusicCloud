using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Infrastructure.Repositories;
using MusicCloud.Webapp.Dto;
using MusicCloud.Webapp.Services;

namespace MusicCloud.Webapp.Pages.Albums
{
    [Authorize]
    public class EditAlbumModel : PageModel
    {
        private readonly AlbumRepository _albums;
        private readonly ArtistRepository _artists;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
        public EditAlbumModel(AlbumRepository albums, ArtistRepository artists, IMapper mapper, AuthService authService)
        {
            _albums = albums;
            _artists = artists;
            _mapper = mapper;
            _authService = authService;
        }
        [BindProperty]
        public AlbumDto Album { get; set; } = null!;
        public IEnumerable<SelectListItem> ArtistSelectList =>
            _artists.Set.OrderBy(a => a.ArtistName).Select(a => new SelectListItem(a.ArtistName, a.Guid.ToString()));
        public IActionResult OnPost(Guid guid)
        {
            if (!ModelState.IsValid) { return Page(); }
            var album = _albums.Set
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefault(a => a.Guid == guid);
            if (album is null) { return RedirectToPage("/Artists/Index"); }
            
            var artist = _artists.FindByGuid(Album.ArtistGuid);
            if (artist is null) { return RedirectToPage("/Artists/Index"); }
            album.Artist = artist;

            album.Songs.ToList().ForEach(a => a.Artist = artist);
            _mapper.Map(Album, album);
            var (success, message) = _albums.Update(album);
            if(!success) { ModelState.AddModelError("", message); }
            return RedirectToPage("/Artists/Index");
        }
        public IActionResult OnGet(Guid guid)
        {
            var albums = _albums.Set
                .Include(a => a.Artist)
                .ThenInclude(ar => ar.Manager)
                .FirstOrDefault(a => a.Guid == guid);
            if (albums is null) { return RedirectToPage("/Artists/Index"); }
            if(!_authService.IsAdmin && _authService.Username != albums.Artist.Manager.Username)
            {
                return new ForbidResult();
            }
            Album = _mapper.Map<AlbumDto>(albums);
            return Page();
        }

        public bool CanEditArtist()
        {
            return _authService.IsAdmin;
        }
    }

}
