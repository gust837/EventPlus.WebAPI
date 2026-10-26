using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentario _comentario;
        private readonly IModeracaoTextoService _moderacaoTexto;

        public ComentarioController(IComentario comentario, IModeracaoTextoService moderacaoTexto)
        {
            _comentario = comentario;
            _moderacaoTexto = moderacaoTexto;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var comentarioBuscado = await _comentario.BuscarPorId(id);

            if (comentarioBuscado == null)
            {
                return NotFound("Comentario não encontrado.");
            }

            return Ok(comentarioBuscado);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var comentario = await _comentario.Listar();

                return Ok(comentario);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // Lista apenas os comentários de um evento (com o nome do autor), do mais recente para o mais antigo.
        [HttpGet("evento/{idEvento:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> ListarPorEvento(Guid idEvento)
        {
            try
            {
                var comentarios = await _comentario.ListarPorEvento(idEvento);
                return Ok(comentarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ComentarioDTO dto)
        {
            // O autor do comentário vem do token JWT (e não do corpo da requisição)
            // para impedir que alguém publique em nome de outro usuário.
            var idUsuarioLogado = User.ObterIdUsuario();

            if (idUsuarioLogado == null)
            {
                return Unauthorized("Usuário não identificado no token.");
            }

            var contemImproprio = await _moderacaoTexto.ContemConteudoImproprio(dto.Descricao);

            if (contemImproprio)
            {
                return BadRequest("O comentário contém conteúdo impróprio e não pode ser publicado");
            }

            var comentarioBuscado = new Comentario
            {
                Descricao = dto.Descricao,
                DataComentario = DateTime.Now,
                IdEvento = dto.IdEvento,
                IdUsuario = idUsuarioLogado,
                // Só chega aqui o que passou pela moderação, então o comentário pode ser exibido.
                Exibe = true
            };

            try
            {
                await _comentario.Cadastrar(comentarioBuscado);
                return StatusCode(201, comentarioBuscado);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ComentarioDTO dto)
        {
            var comentarioBuscado = new Comentario
            {
                Descricao = dto.Descricao,
                IdEvento = dto.IdEvento,
                IdUsuario = dto.IdUsuario
            };

            await _comentario.Atualizar(id, comentarioBuscado);

            return Ok(comentarioBuscado);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _comentario.Deletar(id);
            return NoContent();
        }
    }
}
