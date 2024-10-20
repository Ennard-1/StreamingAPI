using System.ComponentModel.DataAnnotations;

namespace StreamingAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        public ICollection<Conteudo> Conteudos { get; set; } = new List<Conteudo>();

        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
