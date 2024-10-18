using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingAPI.Models
{
    public class VideoPlaylist
    {
        public int VideoId { get; set; }

        [ForeignKey("VideoId")]
        public Video Video { get; set; }

        public int PlaylistId { get; set; }

        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; }
    }
}
