using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentario _comentario;

        public ComentarioController(IComentario comentario)
        {
            _comentario = comentario;
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
        public async Task<IActionResult> Listar()
        {
            try
            {
                var comentario = await _comentario.Listar();

                return Ok(comentario);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ComentarioDTO dto)
        {
            var comentarioBuscado = new Comentario
            {
                Descricao = dto.Descricao,
                DataComentario = dto.DataComentario,
                IdEvento = dto.IdEvento,
                IdUsuario = dto.IdUsuario
            };

            try
            {
                await _comentario.Cadastrar(comentarioBuscado);
                return StatusCode(201, comentarioBuscado);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ComentarioDTO dto)
        {
            var comentarioBuscado = new Comentario
            {
                Descricao = dto.Descricao,
                DataComentario = dto.DataComentario,
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
