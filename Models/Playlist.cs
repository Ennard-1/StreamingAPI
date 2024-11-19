using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Playlist
{
    public int Id { get; set; }
    public int UsuarioID { get; set; }

    [Required(ErrorMessage = "O nome da playlist é obrigatório.")]
    public string Nome { get; set; }

   
    [JsonIgnore]
    public virtual ICollection<ItemPlaylist> Itens { get; set; } = new List<ItemPlaylist>();
}
