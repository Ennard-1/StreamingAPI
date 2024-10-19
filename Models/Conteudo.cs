
namespace StreamingAPI.Models
{
public class Conteudo {
    public int ID { get; set; }
    public string Titulo { get; set; }
    public string Tipo { get; set; }
     public int CriadorID { get; set; }
    public Criador Criador { get; set; }
    public List<ItemPlaylist> ItemPlaylists { get; set; }
}

}