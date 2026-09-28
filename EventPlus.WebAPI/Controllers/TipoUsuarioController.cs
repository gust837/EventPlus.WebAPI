using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controller
{
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
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoUsuarioBuscado = await _tipoUsuario.BuscarPorId(id);

            if (tipoUsuarioBuscado == null)
            {
                return NotFound("Tipo usuario não encontrado!");
            }

            return Ok(tipoUsuarioBuscado);
        }


        /// <summary>
        /// Lista todos os perfils de usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoUsuario.Listar();

                return Ok(tipos);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Cadastra um novo perfil de usuario
        /// </summary>
        /// <param name="tipoUsuario">Perfil do usuario a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {
            var tipoUsuario = new TipoUsuario
            {
                Titulo = dto.Titulo
            };

            await _tipoUsuario.Cadastrar(tipoUsuario);

            return StatusCode(201, tipoUsuario);
        }

        /// <summary>
        /// Esse metodo atualiza o titulo usuario com base no id
        /// </summary>
        /// <param name="id">Id que sera atualizado</param>
        /// <param name="dto">Titulo que sera atualizado</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoUsuarioDTO dto)
        {
            var tipoUsuario = new TipoUsuario
            {
                Titulo = dto.Titulo
            };

            await _tipoUsuario.Atualizar(id, tipoUsuario);

            return Ok(tipoUsuario);
        }

        /// <summary>
        /// Remove um perfil de usuario pelo ID
        /// </summary>
        /// <param name="id">Id do perfil a ser removido</param>
        /// 
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _tipoUsuario.Deletar(id);
            return NoContent();
        }
    }
}
