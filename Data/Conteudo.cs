namespace StreamingAPI.Data
{
    public class Conteudo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        // Chave estrangeira para Criador
        public int CriadorId { get; set; }
        public Criador Criador { get; set; }

        // Relacionamento N:M com Playlist através de ItemPlaylist
        public ICollection<ItemPlaylist> ItensPlaylist { get; set; }
    }
}
