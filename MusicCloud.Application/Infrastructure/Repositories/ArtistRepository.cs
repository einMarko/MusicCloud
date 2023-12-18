using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Model;
using MusicCloud.Application.models;

namespace MusicCloud.Application.Infrastructure.Repositories
{
    public class ArtistRepository : Repository<Artist, int>
    {
        public record ArtistWithAlbumAndSongCount(
            Guid Guid,
            string ArtistName,
            string FirstName,
            string LastName,
            DateTime Birthday,
            User? Manager,
            int AlbumsCount,
            int SongsCount
            );

        public ArtistRepository(CloudContext db) : base(db) { }
        public IReadOnlyList<ArtistWithAlbumAndSongCount> GetArtistsWithAlbumAndSongCount()
        {
            return _db.Artists
                .Select(ar => new ArtistWithAlbumAndSongCount(
                ar.Guid,
                ar.ArtistName,
                ar.FirstName,
                ar.LastName,
                ar.Birthday,
                ar.Manager,
                ar.Albums.Count,
                ar.Songs.Count)).ToList();
        }

        public override (bool success, string message) Insert(Artist artist)
        {
            return base.Insert(artist);
        }
    }
}
