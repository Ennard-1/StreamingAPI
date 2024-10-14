using Microsoft.EntityFrameworkCore;

namespace StreamingAPI.Data
{
    public class StreamingDbContext : DbContext
    {
        public StreamingDbContext(DbContextOptions<StreamingDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Criador> Criadores { get; set; }
        public DbSet<ItemPlaylist> ItensPlaylist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da chave composta de ItemPlaylist (N:M)
            modelBuilder.Entity<ItemPlaylist>()
                .HasKey(ip => new { ip.PlaylistId, ip.ConteudoId });

            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Playlist)
                .WithMany(p => p.ItensPlaylist)
                .HasForeignKey(ip => ip.PlaylistId);

            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Conteudo)
                .WithMany(c => c.ItensPlaylist)
                .HasForeignKey(ip => ip.ConteudoId);
        }
    }
}
