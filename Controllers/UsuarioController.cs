using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuario;

        public UsuarioController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var UsuarioBuscado = await _usuario.BuscarPorId(id);

            if (UsuarioBuscado == null)
            {
                return NotFound("Tipo usuario não encontrado!");
            }

            return Ok(UsuarioBuscado);
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
                var tipos = await _usuario.Listar();

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
        /// <param name="Usuario">Perfil do usuario a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
        {
            var usuarioBuscado = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                IdTipoUsuario = dto.IdTipoUsuario
            };

            try
            {
                await _usuario.Cadastrar(usuarioBuscado);
                return StatusCode(201, usuarioBuscado);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

        }

        /// <summary>
        /// Esse metodo atualiza o nome, email e senha do usuario com base no id
        /// </summary>
        /// <param name="id">Id que sera atualizado</param>
        /// <param name="dto">Titulo que sera atualizado</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioDTO dto)
        {
            var usuarioBuscado = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha
            };

            await _usuario.Atualizar(id, usuarioBuscado);

            return Ok(usuarioBuscado);
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
            await _usuario.Deletar(id);
            return NoContent();
        }
    }
}
