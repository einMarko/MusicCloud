using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Infrastructure.Repositories;
using MusicCloud.Application.models;
using MusicCloud.Webapp.Dto;
using MusicCloud.Webapp.Services;

namespace MusicCloud.Webapp.Pages.Artists
{
    [Authorize]
    public class EditArtistModel : PageModel
    {
        private readonly ArtistRepository _artists;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
        public EditArtistModel(ArtistRepository artists, IMapper mapper, AuthService authService)
        {
            _artists = artists;
            _mapper = mapper;
            _authService = authService;
        }
        [BindProperty]
        public ArtistDto Artist { get; set; } = null!;
        public IActionResult OnPost(Guid guid)
        {
            if (!ModelState.IsValid) { return Page(); }
            var artist = _artists.FindByGuid(guid);
            if (artist is null) { return RedirectToPage("/Artists/Index"); }
            _mapper.Map(Artist, artist);
            var (success, message) = _artists.Update(artist);
            if (!success)
            {
                ModelState.AddModelError("", message);
                return Page();
            }
            return RedirectToPage("/Artists/Index");
        }
        public IActionResult OnGet(Guid guid)
        {
            var artist = _artists.Set
                .Include(ar => ar.Manager)
                .Include(ar => ar.Albums)
                .Include(ar => ar.Songs)
                .FirstOrDefault(ar => ar.Guid == guid);
            if (artist is null) { return RedirectToPage("/Artists/Index"); }
            var username = _authService.Username;
            if(!_authService.HasRole("Admin") && username != artist.Manager.Username)
            {
                return new ForbidResult();
            }
            Artist = _mapper.Map<ArtistDto>(artist);
            return Page();
        }
    }
}
