using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEvento _evento;

        public EventoController(IEvento evento)
        {
            _evento = evento;
        }

        [HttpGet("{id:guid}")]
        public async Task <IActionResult> BuscarPorId(Guid id)
        {
            var eventoBuscado = await _evento.BuscarPorId(id);

            if (eventoBuscado == null)
            {
                return NotFound("Usuario não encontrado"); 
            }

            return Ok(eventoBuscado);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var evento = await _evento.Listar();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar( [FromBody] EventoDTO dto)
        {
            var eventoBuscado = new Evento
            {
                NomeEvento = dto.NomeEvento,
                Descricao = dto.Descricao,
                DataEvento = dto.DataEvento,
                ImagemUrl = dto.ImagemUrl,
                IdTipoEvento = dto.IdTipoEvento,
                IdInstituicao = dto.IdInstituicao
            };

            try
            {
                await _evento.Cadastrar(eventoBuscado);
                return StatusCode(201, eventoBuscado);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] EventoDTO dto)
        {
            var eventoBuscado = new Evento
            {
                NomeEvento = dto.NomeEvento,
                Descricao = dto.Descricao,
                DataEvento = dto.DataEvento,
                ImagemUrl = dto.ImagemUrl,
                IdTipoEvento = dto.IdTipoEvento,
                IdInstituicao = dto.IdInstituicao
            };

            await _evento.Atualizar(id, eventoBuscado);

            return Ok(eventoBuscado);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _evento.Deletar(id);
            return NoContent();
        }
     }
}
