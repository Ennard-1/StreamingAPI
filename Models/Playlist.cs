using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingAPI.Models
{
   public class Playlist {
    public int ID { get; set; }
    public string Nome { get; set; }
     public int UsuarioID { get; set; }
    public Usuario Usuario { get; set; }
     public List<ItemPlaylist> ItemPlaylists { get; set; }
}

}
