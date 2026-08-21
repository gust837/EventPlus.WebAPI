using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        private readonly ICloudinary _cloudinary;

        public EventoController(IEvento evento, IConfiguration c)
        {
            _evento = evento;

            var acc = new Account
            (
                c["Cloudinary:CloudName"],
                c["Cloudinary:ApiKey"],
                c["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(acc);
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
        public async Task<IActionResult> Cadastrar( [FromForm] EventoDTO dto)
        {
            try
            {
                string imgUrl = null;

                if (dto.ImagemUrl != null && dto.ImagemUrl.Length > 0)
                {
                    await using var stream = dto.ImagemUrl.OpenReadStream();
                    var upload = new ImageUploadParams
                    {
                        File = new FileDescription(dto.ImagemUrl.FileName, stream),
                        Folder = "EventPlus/Eventos"
                    };

                    var uploadResuts = await _cloudinary.UploadAsync(upload);

                    imgUrl = uploadResuts.SecureUrl.ToString();
                }

                var eventoBuscado = new Evento
                {
                    NomeEvento = dto.NomeEvento,
                    Descricao = dto.Descricao,
                    DataEvento = dto.DataEvento,
                    ImagemUrl = imgUrl,
                    IdTipoEvento = dto.IdTipoEvento,
                    IdInstituicao = dto.IdInstituicao
                };



                await _evento.Cadastrar(eventoBuscado);
                return StatusCode(201, eventoBuscado);
            }

            catch (Exception ex)
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
