using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuario _tipoUsuario;

        public TipoUsuarioController(ITipoUsuario tipoUsuario)
        {
            _tipoUsuario = tipoUsuario;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoUsuarioBuscado = await _tipoUsuario.BuscarPorId(id);

            if (tipoUsuarioBuscado == null)
            {
                return NotFound("Tipo de usuário não encontrado!");
            }

            return Ok(tipoUsuarioBuscado);
        }

        /// <summary>
        /// Lista todos os perfis de usuários
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoUsuario.Listar();
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo perfil de usuário
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {
            try
            {
                var tipoUsuario = new TipoUsuario
                {
                    Titulo = dto.Titulo
                };

                await _tipoUsuario.Cadastrar(tipoUsuario);
                return StatusCode(201, tipoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Atualiza o título de TipoUsuario com base no ID
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoUsuarioDTO dto)
        {
            try
            {
                var tipoUsuario = new TipoUsuario
                {
                    Titulo = dto.Titulo
                };

                await _tipoUsuario.Atualizar(id, tipoUsuario);
                return Ok(tipoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Remove um perfil de usuário pelo ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _tipoUsuario.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
