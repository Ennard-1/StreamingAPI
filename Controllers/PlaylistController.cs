using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingAPI.Helpers; // Adicione esta linha
using StreamingAPI.Models;
using StreamingAPI.Repositories;

namespace StreamingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Adicione esta linha para proteger todas as rotas
    public class PlaylistController : ControllerBase
    {
        private readonly PlaylistRepository _playlistRepository;
        private readonly JwtHelper _jwtHelper; // Adicione esta linha

        public PlaylistController(PlaylistRepository playlistRepository, JwtHelper jwtHelper)
        {
            _playlistRepository = playlistRepository;
            _jwtHelper = jwtHelper; // Adicione esta linha
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> CreatePlaylist(Playlist playlist)
        {
            // Obter o ID do usuário do token JWT
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();

            // Definir o CriadorId da playlist
            playlist.UsuarioID = userId.Value; // Defina o CriadorId

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
                return NotFound(); // Ou considere retornar Forbid se o usuário não é o dono da playlist

            return NoContent(); // Retorna 204 No Content se a adição for bem-sucedida
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
            // Obter o ID do usuário do token JWT
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();

            // Buscar as playlists do usuário pelo ID
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

            // Definir o CriadorId da playlist
            playlist.UsuarioID = userId.Value; // Defina o CriadorId

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

            // Verificar se o usuário é o criador da playlist
            var playlist = await _playlistRepository.GetPlaylistById(playlistId);
            if (playlist == null)
                return NotFound("Playlist não encontrada.");

            if (playlist.UsuarioID != userId.Value)
                return Forbid(); // O usuário não é o dono da playlist

            // Remover o vídeo da playlist
            var success = await _playlistRepository.RemoveVideoFromPlaylist(playlistId, conteudoId);
            if (!success)
                return NotFound("Vídeo não encontrado na playlist.");

            return NoContent(); // Retorna 204 No Content se a remoção for bem-sucedida
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlaylist(int id)
        {
            var userId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (userId == null)
                return Unauthorized();

            // Verificar se o usuário é o criador da playlist
            var playlist = await _playlistRepository.GetPlaylistById(id);
            if (playlist == null)
                return NotFound();

            if (playlist.UsuarioID != userId.Value)
                return Forbid(); // O usuário não é o dono da playlist

            var result = await _playlistRepository.DeletePlaylist(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}
