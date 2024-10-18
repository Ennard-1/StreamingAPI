using Microsoft.EntityFrameworkCore;
using StreamingAPI.Models;

namespace StreamingAPI.Data
{
    public class StreamingDbContext : DbContext
    {
        public StreamingDbContext(DbContextOptions<StreamingDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<VideoPlaylist> VideoPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoPlaylist>()
                .HasKey(vp => new { vp.VideoId, vp.PlaylistId });

            modelBuilder.Entity<VideoPlaylist>()
                .HasOne(vp => vp.Video)
                .WithMany(v => v.VideoPlaylists)
                .HasForeignKey(vp => vp.VideoId);

            modelBuilder.Entity<VideoPlaylist>()
                .HasOne(vp => vp.Playlist)
                .WithMany(p => p.VideoPlaylists)
                .HasForeignKey(vp => vp.PlaylistId);

            modelBuilder.Entity<Video>()
                .HasOne(v => v.Creator)
                .WithMany(u => u.Videos)
                .HasForeignKey(v => v.CreatorId);

            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId);
        }
    }
}
