using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly UserRepository _users;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
        public IReadOnlyList<ArtistRepository.ArtistWithAlbumAndSongCount> Artists { get; private set; } = new List<ArtistRepository.ArtistWithAlbumAndSongCount>();

        public IndexModel(ArtistRepository artists, UserRepository users, IMapper mapper, AuthService authService)
        {
            _artists = artists;
            _users = users;
            _mapper = mapper;
            _authService = authService;
        }
        [BindProperty]
        public ArtistDto NewArtist { get; set; } = default!;
        public IEnumerable<SelectListItem> ManagerSelectList =>
            _users.Set.Where(u => u.Usertype == Application.Model.Usertype.Manager)
            .OrderBy(u => u.Username).Select(u => new SelectListItem(u.Username, u.Guid.ToString()));
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
            if(NewArtist.ManagerGuid.HasValue)
            {
                var manager = _users.FindByGuid((Guid)NewArtist.ManagerGuid);
                if(manager.Usertype == Application.Model.Usertype.Admin)
                {
                    Message = "You can not add an Admin as a Manager";
                    return Page();
                }
                artist.Manager = manager;
            }
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
