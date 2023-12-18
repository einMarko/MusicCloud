using MusicCloud.Application.infrastructure;
using MusicCloud.Application.models;

namespace MusicCloud.Application.Infrastructure.Repositories
{
    public class SongRepository : Repository<Song, int>
    {
        public SongRepository(CloudContext db) : base(db) { }

    }
}
