namespace StreamingAPI.Data
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // Chave estrangeira para Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Relacionamento N:M com Conteudo através de ItemPlaylist
        public ICollection<ItemPlaylist> ItensPlaylist { get; set; }
    }
}
