using DAL.Entities;
using System.Data.Entity;

namespace DAL.Context
{
    public class MusicLibraryDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<AlbumReview> AlbumReview { get; set; }

        public DbSet<AlbumSong> AlbumSongs { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Band> Bands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<SongReview> SongReviews { get; set; }

        public DbSet<StorageFile> StorageFiles { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
