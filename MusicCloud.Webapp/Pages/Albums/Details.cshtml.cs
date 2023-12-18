using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MusicCloud.Webapp.Pages.Albums
{
  
    public class AlbumDetailsModel : PageModel
    {
        private readonly AlbumRepository _albums;
        private readonly SongRepository _songs;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
        public AlbumDetailsModel(AlbumRepository albums, SongRepository songs, IMapper mapper, AuthService authService)
        {
            _albums = albums;
            _songs = songs;
            _mapper = mapper;
            _authService = authService;
        }

        [FromRoute]
        public Guid Guid { get; set; }
        public Album Album { get; private set; } = default!;
        [BindProperty]
        public Dictionary<Guid, SongDto> EditSongs { get; set; } = new();

        public IActionResult OnPostEditSong(Guid guid, Guid songGuid, Dictionary<Guid, SongDto> editSongs)
        {
            if (!ModelState.IsValid) { return Page(); }
            var song = _songs.FindByGuid(songGuid);
            if (song is null) { return RedirectToPage(); }

            _mapper.Map(editSongs[songGuid], song);
            var (success, message) = _songs.Update(song);
            if (!success)
            {
                ModelState.AddModelError("", message);
                return Page();
            }
            return RedirectToPage();
        }
        public IActionResult OnGet(Guid guid)
        {
            return Page();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var album = _albums.Set
                .Include(a => a.Songs)
                .ThenInclude(s => s.Genre)
                .Include(a => a.Artist)
                .ThenInclude(ar => ar.Manager)
                .FirstOrDefault(a => a.Guid == Guid);
            if (album is null)
            {
                context.Result = RedirectToPage("/Artists/Index");
                return;
            }
            Album = album;
            EditSongs =
                _songs.Set.Where(s => s.Album.Guid == Guid)
                .ProjectTo<SongDto>(_mapper.ConfigurationProvider)
                .ToDictionary(s => s.Guid, s => s);
        }

        public bool CanEditAlbum(Guid albumGuid) =>
        _authService.IsAdmin ||
            _albums.Set
            .FirstOrDefault(a => a.Guid == albumGuid)?
            .Artist?.Manager?.Username == _authService.Username;
    }
}