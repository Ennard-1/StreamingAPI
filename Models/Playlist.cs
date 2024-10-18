using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingAPI.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User Owner { get; set; }

        // Propriedade de navegação para a tabela de junção
        public List<VideoPlaylist> VideoPlaylists { get; set; } = new();
    }
}
