using Microsoft.EntityFrameworkCore;
using StreamingAPI.Models;

namespace StreamingAPI.Data
{
    public class StreamingDbContext : DbContext
    {
        public StreamingDbContext(DbContextOptions<StreamingDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Conteudo> Conteudos { get; set; }
    public DbSet<Criador> Criadores { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
              modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Email deve ser único
         // Playlist -> Usuario
    modelBuilder.Entity<Playlist>()
        .HasOne(p => p.Usuario)
        .WithMany(u => u.Playlists)
        .HasForeignKey(p => p.UsuarioID)
        .OnDelete(DeleteBehavior.Cascade);

    // Conteudo -> Criador
    modelBuilder.Entity<Conteudo>()
        .HasOne(c => c.Criador)
        .WithMany(cr => cr.Conteudos)
        .HasForeignKey(c => c.CriadorID)
        .OnDelete(DeleteBehavior.SetNull);

    // Muitos-para-Muitos entre Playlist e Conteudo via ItemPlaylist
    modelBuilder.Entity<ItemPlaylist>()
        .HasKey(ip => new { ip.PlaylistID, ip.ConteudoID });

    modelBuilder.Entity<ItemPlaylist>()
        .HasOne(ip => ip.Playlist)
        .WithMany(p => p.ItemPlaylists)
        .HasForeignKey(ip => ip.PlaylistID);

    modelBuilder.Entity<ItemPlaylist>()
        .HasOne(ip => ip.Conteudo)
        .WithMany(c => c.ItemPlaylists)
        .HasForeignKey(ip => ip.ConteudoID);
    }
    }
}
