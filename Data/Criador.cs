namespace StreamingAPI.Data
{
    public class Criador
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // Relacionamento 1:N com Conteudo
        public ICollection<Conteudo> Conteudos { get; set; }
    }
}
