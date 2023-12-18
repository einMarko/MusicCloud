using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Infrastructure.Repositories;
using MusicCloud.Application.models;
using MusicCloud.Webapp.Dto;
using MusicCloud.Webapp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicCloud.Webapp.Pages.Artists
{
    public class IndexModel : PageModel
    {
        private readonly ArtistRepository _artists;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
        public IReadOnlyList<ArtistRepository.ArtistWithAlbumAndSongCount> Artists { get; private set; } = new List<ArtistRepository.ArtistWithAlbumAndSongCount>();

        public IndexModel(ArtistRepository artists, IMapper mapper, AuthService authService)
        {
            _artists = artists;
            _mapper = mapper;
            _authService = authService;
        }
        [BindProperty]
        public ArtistDto NewArtist { get; set; } = default!;
        [TempData]
        public string? Message { get; set; }
        public IActionResult OnPostNewArtist(Guid guid)
        {
            if(!_authService.IsAdmin) {
                Message = "You are not authorized to add an Artist";
                return Page(); 
            }
            if (!ModelState.IsValid) { return Page(); }
            var artist = _mapper.Map<Artist>(NewArtist);
            var (success, message) = _artists.Insert(artist);
            if (!success)
            {
                ModelState.AddModelError("", message);
                return Page();
            }
            return RedirectToPage();
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Artists = _artists.GetArtistsWithAlbumAndSongCount();
        }

        public bool CanEditArtist(Guid artistGuid) =>
            (_authService.IsAdmin ||
            Artists.FirstOrDefault(ar => ar.Guid == artistGuid)?.Manager?.Username == _authService.Username)
            && _authService.IsAuthenticated;

        public bool CanAddArtist() =>
            _authService.IsAdmin;
    }
}
