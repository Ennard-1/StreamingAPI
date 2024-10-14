namespace StreamingAPI.Data
{
    public class ItemPlaylist
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int ConteudoId { get; set; }
        public Conteudo Conteudo { get; set; }

        public int Ordem { get; set; }
    }
}
