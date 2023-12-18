using MusicCloud.Application.infrastructure;
using MusicCloud.Application.models;

namespace MusicCloud.Application.Infrastructure.Repositories
{
    public class GenreRepository : Repository<Song, int>
    {
        public GenreRepository(CloudContext db) : base(db) { }

    }
}
