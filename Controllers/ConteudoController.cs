using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using StreamingAPI.Data;
using StreamingAPI.Helpers;
using StreamingAPI.Models;

namespace StreamingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConteudoController(StreamingDbContext context, JwtHelper jwtHelper)
        : ControllerBase
    {
        private readonly StreamingDbContext _context = context;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conteudo>>> GetConteudos()
        {
            return await _context.Conteudos.Include(c => c.Usuario).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var conteudo = await _context.Conteudos.FindAsync(id);

            if (conteudo == null)
            {
                return NotFound(new { message = "Conteúdo não encontrado." });
            }

            return Ok(conteudo);
        }

        [HttpGet("usuario")]
        public async Task<IActionResult> GetConteudosByUsuario()
        {
            var usuarioId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (usuarioId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var conteudos = await _context
                .Conteudos.Where(c => c.UsuarioID == usuarioId)
                .ToListAsync();

            if (!conteudos.Any())
            {
                return NotFound(new { message = "Nenhum conteúdo encontrado para este usuário." });
            }

            return Ok(conteudos);
        }

        [HttpGet("thumbnails/{thumbnail}")]
        public async Task<IActionResult> GetThumbnail(string thumbnail)
        {
            // Tente encontrar o conteúdo pelo atributo Thumbnail fornecido
            var conteudo = await _context.Conteudos.FirstOrDefaultAsync(c =>
                c.Thumbnail == thumbnail
            );
            if (conteudo == null)
            {
                return NotFound(new { message = "Conteúdo não encontrado." });
            }

            // O atributo Thumbnail já contém o nome do arquivo da thumbnail
            var thumbnailFilePath = Path.Combine("Data/uploads/Thumbnails", conteudo.Thumbnail);

            // Verifique se a thumbnail existe
            if (!System.IO.File.Exists(thumbnailFilePath))
            {
                return NotFound(new { message = "Thumbnail não encontrada." });
            }

            // Retorne a thumbnail como um arquivo
            var fileBytes = await System.IO.File.ReadAllBytesAsync(thumbnailFilePath);
            return File(fileBytes, "image/jpeg"); // Altere o tipo de conteúdo se necessário
        }

        [HttpGet("stream/{fileName}")]
        public IActionResult Stream(string fileName)
        {
            var filePath = Path.Combine("Data/uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );
            var mimeType = "video/mp4";

            var range = Request.Headers.Range.ToString();
            if (string.IsNullOrEmpty(range))
            {
                return File(fileStream, mimeType);
            }

            var rangeHeader = RangeHeaderValue.Parse(range);
            var rangeItem = rangeHeader.Ranges.FirstOrDefault();

            if (rangeItem != null)
            {
                var start = rangeItem.From ?? 0;
                var end = rangeItem.To ?? (fileStream.Length - 1);

                if (start >= fileStream.Length || end >= fileStream.Length || start > end)
                {
                    return StatusCode(StatusCodes.Status416RangeNotSatisfiable);
                }

                fileStream.Seek(start, SeekOrigin.Begin);
                _ = end - start + 1;

                Response.StatusCode = 206;
                Response.Headers.Append(
                    "Content-Range",
                    $"bytes {start}-{end}/{fileStream.Length}"
                );

                return File(fileStream, mimeType);
            }

            return File(fileStream, mimeType);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ConteudoUploadRequest request)
        {
            var usuarioId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (usuarioId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
            {
                return BadRequest(new { message = "Usuário não encontrado." });
            }

            var conteudo = new Conteudo
            {
                Titulo = request.Titulo,
                Tipo = request.Tipo,
                UsuarioID = usuario.Id,
                NomeArquivo = await SaveFile(request.File, "Data/uploads"),
                Thumbnail = await SaveFile(request.Thumbnail, "Data/uploads/Thumbnails"),
            };

            await _context.Conteudos.AddAsync(conteudo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = conteudo.Id }, conteudo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ConteudoUpdateRequest request)
        {
            var usuarioId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (usuarioId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var conteudo = await _context.Conteudos.FindAsync(id);
            if (conteudo == null)
            {
                return NotFound("Conteúdo não encontrado.");
            }

            if (conteudo.UsuarioID != usuarioId)
            {
                return Forbid("Você não tem permissão para atualizar este conteúdo.");
            }

            conteudo.Titulo = request.Titulo ?? conteudo.Titulo;
            conteudo.Tipo = request.Tipo ?? conteudo.Tipo;

            if (request.File != null && request.File.Length > 0)
            {
                DeleteFileIfExists(Path.Combine("Data/uploads", conteudo.NomeArquivo));
                conteudo.NomeArquivo = await SaveFile(request.File, "Data/uploads");
            }

            if (request.Thumbnail != null && request.Thumbnail.Length > 0)
            {
                DeleteFileIfExists(Path.Combine("Data/uploads/Thumbnails", conteudo.Thumbnail));
                conteudo.Thumbnail = await SaveFile(request.Thumbnail, "Data/uploads/Thumbnails");
            }

            _context.Conteudos.Update(conteudo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = _jwtHelper.GetUsuarioIdFromToken(Request);
            if (usuarioId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var conteudo = await _context.Conteudos.FindAsync(id);
            if (conteudo == null)
            {
                return NotFound("Conteúdo não encontrado.");
            }

            if (conteudo.UsuarioID != usuarioId)
            {
                return Forbid("Você não tem permissão para excluir este conteúdo.");
            }

            DeleteFileIfExists(Path.Combine("Data/uploads", conteudo.NomeArquivo));
            DeleteFileIfExists(Path.Combine("Data/uploads/Thumbnails", conteudo.Thumbnail));

            _context.Conteudos.Remove(conteudo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static void DeleteFileIfExists(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private static async Task<string> SaveFile(IFormFile file, string directory)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(directory, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public class ConteudoUploadRequest
        {
            public required string Titulo { get; set; }
            public required string Tipo { get; set; }
            public required IFormFile File { get; set; }
            public required IFormFile Thumbnail { get; set; }
        }

        public class ConteudoUpdateRequest
        {
            public string? Titulo { get; set; }
            public string? Tipo { get; set; }
            public IFormFile? File { get; set; }
            public IFormFile? Thumbnail { get; set; }
        }
    }
}
