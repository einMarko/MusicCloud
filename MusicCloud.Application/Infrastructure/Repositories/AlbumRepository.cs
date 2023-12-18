using MusicCloud.Application.infrastructure;
using MusicCloud.Application.models;

namespace MusicCloud.Application.Infrastructure.Repositories
{
    public class AlbumRepository : Repository<Album, int>
    {
        public AlbumRepository(CloudContext db) : base(db) { }

    }
}
