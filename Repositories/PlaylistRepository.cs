using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StreamingAPI.Data;
using StreamingAPI.Models;

namespace StreamingAPI.Repositories
{
    public class PlaylistRepository
    {
        private readonly StreamingDbContext _context;

        public PlaylistRepository(StreamingDbContext context)
        {
            _context = context;
        }

        public async Task<Playlist> CreatePlaylist(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task<bool> AddVideoToPlaylist(int playlistId, int usuarioId, int conteudoId)
        {
            var playlist = await _context.Playlists.FirstOrDefaultAsync(p =>
                p.Id == playlistId && p.UsuarioID == usuarioId
            );

            if (playlist == null)
                return false;

            var conteudo = await _context.Conteudos.FindAsync(conteudoId);
            if (conteudo == null)
                return false;

            // Verificar se o vídeo já está na playlist
            bool videoJaExiste = await _context.ItemPlaylists.AnyAsync(ip =>
                ip.PlaylistId == playlistId && ip.ConteudoId == conteudoId
            );

            if (videoJaExiste)
                return false; // Vídeo já está na playlist

            var itemPlaylist = new ItemPlaylist
            {
                PlaylistId = playlistId,
                ConteudoId = conteudoId,
            };

            _context.ItemPlaylists.Add(itemPlaylist);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Playlist> GetPlaylistById(int id)
        {
            // Inclui a tabela intermediária para obter conteúdos da playlist
            return await _context
                .Playlists.Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo) // Inclui o conteúdo relacionado
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Conteudo>> GetConteudosByPlaylistId(int playlistId)
        {
            var playlist = await _context
                .Playlists.Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
                return null;

            // Retornar apenas os conteúdos
            var videos = playlist.Itens.Select(ip => ip.Conteudo).ToList();

            return videos;
        }

        public async Task<List<Playlist>> GetPlaylistsByUserId(int usuarioId)
        {
            // Filtrar playlists pelo ID do usuário
            return await _context
                .Playlists.Include(p => p.Itens) // Inclui os itens da playlist
                .ThenInclude(ip => ip.Conteudo) // Inclui os conteúdos relacionados
                .Where(p => p.UsuarioID == usuarioId) // Apenas playlists do usuário
                .ToListAsync();
        }

        public async Task<List<Playlist>> GetAllPlaylists()
        {
            // Inclui a tabela intermediária para obter conteúdos de todas as playlists
            return await _context
                .Playlists.Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo) // Inclui os conteúdos relacionados
                .ToListAsync();
        }

        public async Task<Playlist> UpdatePlaylist(int id, Playlist playlist)
        {
            var existingPlaylist = await GetPlaylistById(id);
            if (existingPlaylist == null)
                return null;

            existingPlaylist.Nome = playlist.Nome;

            // Para atualizar os conteúdos, você pode precisar gerenciar a coleção ItemPlaylist.
            // Isso poderia incluir a remoção de conteúdos existentes e a adição de novos, se necessário.
            // Implementação da lógica de atualização de conteúdos aqui, se necessário.

            await _context.SaveChangesAsync();
            return existingPlaylist;
        }

        public async Task<bool> DeletePlaylist(int id)
        {
            var playlist = await GetPlaylistById(id);
            if (playlist == null)
                return false;

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
