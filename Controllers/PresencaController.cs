using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresencaController : ControllerBase
    {
        private readonly IPresenca _presenca;

        public PresencaController(IPresenca presenca)
        {
            _presenca = presenca;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var presencaBuscada = await _presenca.BuscarPorId(id);

            if (presencaBuscada == null)
            {
                return NotFound("Presença não encontrada.");
            }

            return Ok(presencaBuscada);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _presenca.Listar();
                return Ok(tipos);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] PresencaDTO dto)
        {
            var presenca = new Presenca
            {
                IdEvento = dto.IdEvento,
                IdUsuario = dto.IdUsuario
            };

            await _presenca.Cadastrar(presenca);
            return StatusCode(201, presenca);
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] PresencaDTO dto)
        {
            var presenca = new Presenca
            {
                IdEvento = dto.IdEvento,
                IdUsuario = dto.IdEvento
            };

            await _presenca.Atualizar(id, presenca);
            return StatusCode(201, presenca);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _presenca.Deletar(id);
            return NoContent();
        }
    }
}
