using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.infrastructure;
using Xunit;

namespace MusicCloud.Test
{
    [Collection("Sequential")]
    public class CloudContextTests : DatabaseTest
    {
        private CloudContext GetDatabase(bool deleteDb = false)
        {
            var db = new CloudContext(new DbContextOptionsBuilder()
                .UseSqlite("DataSource=CloudView.db")
                .Options);
            if (deleteDb)
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            return db;
        }

        /*[Fact]
        public void CreateDatabaseSuccessTest()
        {
            using var db = GetDatabase(deleteDb: true);
        }*/

        [Fact]
        public void testSeeds()
        {
            using var db = GetDatabase(deleteDb: true);
            db.Seed();

            Assert.True(db.Artists.ToList().Count > 0);
            Assert.True(db.Albums.ToList().Count > 0);
            Assert.True(db.Genres.ToList().Count > 0);
            Assert.True(db.Songs.ToList().Count > 0);
        }
    }
}
