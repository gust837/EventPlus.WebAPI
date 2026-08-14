using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
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
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoEventoBuscado = await _tipoEvento.BuscarPorId(id);

            if (tipoEventoBuscado == null)
            {
                return NotFound("Tipo evento não encontrado!");
            }

            return Ok(tipoEventoBuscado);
        }


        /// <summary>
        /// Lista todos os tipos de evento
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoEvento.Listar();

                return Ok(tipos);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Cadastra um novo perfil de TipoEvento
        /// </summary>
        /// <param name="tipoEvento">Perfil do tipo de evento a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
        {
            var tipoEvento = new TipoEvento
            {
                Titulo = dto.Titulo
            };

            await _tipoEvento.Cadastrar(tipoEvento);

            return StatusCode(201, tipoEvento);
        }

        /// <summary>
        /// Esse metodo atualiza o titulo de tipo evento com base no id
        /// </summary>
        /// <param name="id">Id que sera atualizado</param>
        /// <param name="dto">Titulo que sera atualizado</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoEventoDTO dto)
        {
            var tipoEvento = new TipoEvento
            {
                Titulo = dto.Titulo
            };

            await _tipoEvento.Atualizar(id, tipoEvento);

            return Ok(tipoEvento);
        }

        /// <summary>
        /// Remove um perfil de tipo evento pelo ID
        /// </summary>
        /// <param name="id">Id do perfil a ser removido</param>
        /// 
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _tipoEvento.Deletar(id);
            return NoContent();
        }
    }
}
