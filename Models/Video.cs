using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingAPI.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string VideoPath { get; set; }  // Caminho para o arquivo do vídeo

        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        // Propriedade de navegação para a tabela de junção
        public List<VideoPlaylist> VideoPlaylists { get; set; } = new();
    }
}
