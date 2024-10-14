namespace StreamingAPI.Data
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        // Relacionamento 1:N com Playlist
        public ICollection<Playlist> Playlists { get; set; }
    }
}
