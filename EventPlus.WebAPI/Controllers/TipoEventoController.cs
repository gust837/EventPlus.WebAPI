using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEventoController : ControllerBase
    {
        private readonly ITipoEvento _tipoEvento;

        public TipoEventoController(ITipoEvento tipoEvento)
        {
            _tipoEvento = tipoEvento;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoEventoBuscado = await _tipoEvento.BuscarPorId(id);

            if (tipoEventoBuscado == null)
            {
                return NotFound("Tipo de evento não encontrado!");
            }

            return Ok(tipoEventoBuscado);
        }

        /// <summary>
        /// Lista todos os tipos de evento
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoEvento.Listar();
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo TipoEvento
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
        {
            try
            {
                var tipoEvento = new TipoEvento
                {
                    Titulo = dto.Titulo
                };

                await _tipoEvento.Cadastrar(tipoEvento);
                return StatusCode(201, tipoEvento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Atualiza o título de TipoEvento com base no ID
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoEventoDTO dto)
        {
            try
            {
                var tipoEvento = new TipoEvento
                {
                    Titulo = dto.Titulo
                };

                await _tipoEvento.Atualizar(id, tipoEvento);
                return Ok(tipoEvento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Remove um TipoEvento pelo ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _tipoEvento.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
