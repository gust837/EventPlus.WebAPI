using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                return NotFound("Usuário não encontrado!");
            }

            return Ok(UsuarioBuscado);
        }

        /// <summary>
        /// Lista todos os perfis de usuários
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var usuarios = await _usuario.Listar();
                return Ok(usuarios);
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
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Senha) || dto.Senha.Length < 8)
            {
                return BadRequest("A senha é obrigatória e deve ter pelo menos 8 caracteres.");
            }

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
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Atualiza nome, email, tipo e (opcionalmente) a senha do usuário com base no ID
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioDTO dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Senha) && dto.Senha.Length < 8)
            {
                return BadRequest("A nova senha deve ter pelo menos 8 caracteres.");
            }

            var usuarioBuscado = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha ?? string.Empty,
                IdTipoUsuario = dto.IdTipoUsuario
            };

            try
            {
                await _usuario.Atualizar(id, usuarioBuscado);
                return Ok(usuarioBuscado);
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
                await _usuario.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
