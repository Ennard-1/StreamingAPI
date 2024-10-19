using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingAPI.Models
{
    public class Usuario {
    public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }
    public List<Playlist> Playlists { get; set; } = new();
}

}
