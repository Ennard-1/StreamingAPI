using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using StreamingAPI.Data;
using StreamingAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ConteudoController : ControllerBase
{
    private readonly StreamingDbContext _context;

    public ConteudoController(StreamingDbContext context)
    {
        _context = context;
    }

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

    [HttpGet("stream/{fileName}")]
    public IActionResult Stream(string fileName)
    {
        var filePath = Path.Combine("Data/uploads", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var mimeType = "video/mp4";

        var range = Request.Headers["Range"].ToString();
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
            var length = end - start + 1;

            Response.StatusCode = 206;
            Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{fileStream.Length}");

            return File(fileStream, mimeType);
        }

        return File(fileStream, mimeType);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ConteudoUploadRequest request)
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (usuarioIdClaim == null)
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }

        var usuarioId = int.Parse(usuarioIdClaim.Value);

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
        };

        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest(new { message = "Arquivo de vídeo não enviado." });
        }

        if (request.Thumbnail == null || request.Thumbnail.Length == 0)
        {
            return BadRequest(new { message = "Thumbnail não enviada." });
        }

        var videoFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
        var videoFilePath = Path.Combine("Data/uploads", videoFileName);
        using (var stream = new FileStream(videoFilePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }
        conteudo.NomeArquivo = videoFileName;

        var thumbnailFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Thumbnail.FileName)}";
        var thumbnailFilePath = Path.Combine("Data/uploads/Thumbnails", thumbnailFileName);
        using (var stream = new FileStream(thumbnailFilePath, FileMode.Create))
        {
            await request.Thumbnail.CopyToAsync(stream);
        }
        conteudo.Thumbnail = thumbnailFileName;

        await _context.Conteudos.AddAsync(conteudo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = conteudo.Id }, conteudo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] ConteudoUpdateRequest request)
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (usuarioIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var usuarioId = int.Parse(usuarioIdClaim.Value);

        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null)
        {
            return BadRequest("Usuário não encontrado.");
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
            var previousVideoFilePath = Path.Combine("Data/uploads", conteudo.NomeArquivo);
            if (System.IO.File.Exists(previousVideoFilePath))
            {
                System.IO.File.Delete(previousVideoFilePath);
            }

            var videoFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
            var videoFilePath = Path.Combine("Data/uploads", videoFileName);
            using (var stream = new FileStream(videoFilePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }
            conteudo.NomeArquivo = videoFileName;
        }

        if (request.Thumbnail != null && request.Thumbnail.Length > 0)
        {
            var previousThumbnailFilePath = Path.Combine(
                "Data/uploads/Thumbnails",
                conteudo.Thumbnail
            );
            if (System.IO.File.Exists(previousThumbnailFilePath))
            {
                System.IO.File.Delete(previousThumbnailFilePath);
            }

            var thumbnailFileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Thumbnail.FileName)}";
            var thumbnailFilePath = Path.Combine("Data/uploads/Thumbnails", thumbnailFileName);
            using (var stream = new FileStream(thumbnailFilePath, FileMode.Create))
            {
                await request.Thumbnail.CopyToAsync(stream);
            }
            conteudo.Thumbnail = thumbnailFileName;
        }

        _context.Conteudos.Update(conteudo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (usuarioIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var usuarioId = int.Parse(usuarioIdClaim.Value);

        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null)
        {
            return BadRequest("Usuário não encontrado.");
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

        var videoFilePath = Path.Combine("Data/uploads", conteudo.NomeArquivo);
        var thumbnailFilePath = Path.Combine("Data/uploads/Thumbnails", conteudo.Thumbnail);

        if (System.IO.File.Exists(videoFilePath))
        {
            System.IO.File.Delete(videoFilePath);
        }

        if (System.IO.File.Exists(thumbnailFilePath))
        {
            System.IO.File.Delete(thumbnailFilePath);
        }

        _context.Conteudos.Remove(conteudo);
        await _context.SaveChangesAsync();

        return NoContent();
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
