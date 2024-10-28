using Microsoft.EntityFrameworkCore;
using StreamingAPI.Models;

namespace StreamingAPI.Data
{
    public class StreamingDbContext : DbContext
    {
        public StreamingDbContext(DbContextOptions<StreamingDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<ItemPlaylist> ItemPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique(); // Email deve ser único

            // Playlist -> Usuario
            modelBuilder.Entity<Playlist>().HasKey(p => p.Id);

            modelBuilder.Entity<Playlist>().Property(p => p.Nome).IsRequired();

            modelBuilder.Entity<Playlist>().Property(p => p.UsuarioID).IsRequired();

            // Conteudo -> Usuario
            modelBuilder
                .Entity<Conteudo>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Conteudos)
                .HasForeignKey(c => c.UsuarioID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPlaylist>().HasKey(ip => new { ip.PlaylistId, ip.ConteudoId });

            modelBuilder
                .Entity<ItemPlaylist>()
                .HasOne(ip => ip.Playlist)
                .WithMany(p => p.Itens)
                .HasForeignKey(ip => ip.PlaylistId);

            modelBuilder
                .Entity<ItemPlaylist>()
                .HasOne(ip => ip.Conteudo)
                .WithMany(c => c.Itens)
                .HasForeignKey(ip => ip.ConteudoId);
        }
    }
}
