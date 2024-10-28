using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StreamingAPI.Models
{
    public class Conteudo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public string NomeArquivo { get; set; }

        [Required]
        public string Thumbnail { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [ForeignKey(nameof(UsuarioID))]
        [JsonIgnore]
        public Usuario Usuario { get; set; }

        [JsonIgnore]
        public virtual ICollection<ItemPlaylist> Itens { get; set; } = new List<ItemPlaylist>();
    }
}
