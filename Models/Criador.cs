
namespace StreamingAPI.Models
{
public class Criador {
    public int ID { get; set; }
    public string Nome { get; set; }
    public List<Conteudo> Conteudos { get; set; } = new();
}


}