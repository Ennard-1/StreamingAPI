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

            bool videoJaExiste = await _context.ItemPlaylists.AnyAsync(ip =>
                ip.PlaylistId == playlistId && ip.ConteudoId == conteudoId
            );

            if (videoJaExiste)
                return false;

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

            return await _context
                .Playlists.Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo)
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


            var videos = playlist.Itens.Select(ip => ip.Conteudo).ToList();

            return videos;
        }

        public async Task<List<Playlist>> GetPlaylistsByUserId(int usuarioId)
        {

            return await _context
                .Playlists.Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo)
                .Where(p => p.UsuarioID == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Playlist>> GetAllPlaylists()
        {
            return await _context
                .Playlists.Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo)
                .ToListAsync();
        }

        public async Task<Playlist> UpdatePlaylist(int id, Playlist playlist)
        {
            var existingPlaylist = await GetPlaylistById(id);
            if (existingPlaylist == null)
                return null;

            existingPlaylist.Nome = playlist.Nome;

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
        public async Task<bool> RemoveVideoFromPlaylist(int playlistId, int conteudoId)
        {
          
            var itemPlaylist = await _context.ItemPlaylists
                .FirstOrDefaultAsync(ip => ip.PlaylistId == playlistId && ip.ConteudoId == conteudoId);

            if (itemPlaylist == null)
                return false; 

            _context.ItemPlaylists.Remove(itemPlaylist);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
