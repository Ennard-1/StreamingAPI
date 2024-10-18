using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsCreator { get; set; }  // Define se é um criador de conteúdo

        // Relacionamento com vídeos e playlists
        public List<Video> Videos { get; set; } = new();
        public List<Playlist> Playlists { get; set; } = new();
    }
}
