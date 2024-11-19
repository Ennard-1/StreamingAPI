using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingAPI.Helpers;
using StreamingAPI.Models;
using StreamingAPI.Repositories;

namespace StreamingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PlaylistController : ControllerBase
    {
        private readonly PlaylistRepository _playlistRepository;
        private readonly JwtHelper _jwtHelper;

        public PlaylistController(PlaylistRepository playlistRepository, JwtHelper jwtHelper)
        {
            _playlistRepository = playlistRepository;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> CreatePlaylist(Playlist playlist)
        {

            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();


            playlist.UsuarioID = userId.Value;

            var createdPlaylist = await _playlistRepository.CreatePlaylist(playlist);
            return CreatedAtAction(
                nameof(GetPlaylistById),
                new { id = createdPlaylist.Id },
                createdPlaylist
            );
        }

        [HttpPost("{id}/videos")]
        public async Task<ActionResult> AddVideoToPlaylist(int id, [FromBody] int conteudoId)
        {
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();

            var success = await _playlistRepository.AddVideoToPlaylist(
                id,
                userId.Value,
                conteudoId
            );

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist>> GetPlaylistById(int id)
        {
            var playlist = await _playlistRepository.GetPlaylistById(id);
            if (playlist == null)
                return NotFound();
            return playlist;
        }

        [HttpGet("user-playlists")]
        public async Task<ActionResult<List<Playlist>>> GetUserPlaylists()
        {

            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();


            var playlists = await _playlistRepository.GetPlaylistsByUserId(userId.Value);
            return Ok(playlists);
        }

        [HttpGet("{id}/videos")]
        public async Task<ActionResult<List<Conteudo>>> GetConteudosByPlaylistId(int id)
        {
            var conteudos = await _playlistRepository.GetConteudosByPlaylistId(id);
            if (conteudos == null)
                return NotFound("Playlist não encontrada.");

            return Ok(conteudos);
        }



        [HttpGet]
        public async Task<ActionResult<List<Playlist>>> GetAllPlaylists()
        {
            return await _playlistRepository.GetAllPlaylists();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Playlist>> UpdatePlaylist(int id, Playlist playlist)
        {
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();


            playlist.UsuarioID = userId.Value;

            var updatedPlaylist = await _playlistRepository.UpdatePlaylist(id, playlist);
            if (updatedPlaylist == null)
                return NotFound();
            return updatedPlaylist;
        }

        [HttpDelete("{playlistId}/videos/{conteudoId}")]
        public async Task<ActionResult> DeleteVideoFromPlaylist(int playlistId, int conteudoId)
        {
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();


            var playlist = await _playlistRepository.GetPlaylistById(playlistId);
            if (playlist == null)
                return NotFound("Playlist não encontrada.");

            if (playlist.UsuarioID != userId.Value)
                return Forbid();


            var success = await _playlistRepository.RemoveVideoFromPlaylist(playlistId, conteudoId);
            if (!success)
                return NotFound("Vídeo não encontrado na playlist.");

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlaylist(int id)
        {
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();


            var playlist = await _playlistRepository.GetPlaylistById(id);
            if (playlist == null)
                return NotFound();

            if (playlist.UsuarioID != userId.Value)
                return Forbid();

            var result = await _playlistRepository.DeletePlaylist(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}
