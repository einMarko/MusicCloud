using AutoMapper;
using MusicCloud.Application.models;

namespace MusicCloud.Webapp.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<ArtistDto, Artist>()
                .ForMember(
                    art => art.Guid,
                    opt => opt.MapFrom(art => art.Guid == default ? Guid.NewGuid() : art.Guid));
            CreateMap<Artist, ArtistDto>();

            CreateMap<SongDto, Song>()
            .ForMember(
                s => s.Guid,
                opt => opt.MapFrom(s => s.Guid == default ? Guid.NewGuid() : s.Guid))
            .ForMember(
                s => s.Length,
                opt => opt.MapFrom(src => ConvertLengthToTotalSeconds(src.Length)));

            CreateMap<Song, SongDto>();

            CreateMap<AlbumDto, Album>();
            CreateMap<Album, AlbumDto>();
        }

        private static int ConvertLengthToTotalSeconds(string length)
        {
            string[] parts = length.Split(':');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int minutes) &&
                int.TryParse(parts[1], out int seconds))
            {
                return minutes * 60 + seconds;
            }
            return 0;
        }
    }
}
