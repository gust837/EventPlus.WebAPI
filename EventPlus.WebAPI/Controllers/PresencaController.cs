using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _presenca.Listar();
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // Lista somente as presenças do usuário logado (o Id vem do token JWT).
        [HttpGet("minhas")]
        public async Task<IActionResult> ListarMinhas()
        {
            var idUsuarioLogado = User.ObterIdUsuario();

            if (idUsuarioLogado == null)
            {
                return Unauthorized("Usuário não identificado no token.");
            }

            try
            {
                var presencas = await _presenca.ListarPorUsuario(idUsuarioLogado.Value);
                return Ok(presencas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] PresencaDTO dto)
        {
            // O usuário inscrito vem do token JWT, e não do corpo da requisição,
            // para impedir que alguém inscreva outra pessoa.
            var idUsuarioLogado = User.ObterIdUsuario();

            if (idUsuarioLogado == null)
            {
                return Unauthorized("Usuário não identificado no token.");
            }

            try
            {
                var presencasDoUsuario = await _presenca.ListarPorUsuario(idUsuarioLogado.Value);

                if (presencasDoUsuario.Any(p => p.IdEvento == dto.IdEvento && p.Situacao))
                {
                    return Conflict("Você já está inscrito neste evento.");
                }

                var presenca = new Presenca
                {
                    IdEvento = dto.IdEvento,
                    IdUsuario = idUsuarioLogado,
                    Situacao = dto.Situacao
                };

                await _presenca.Cadastrar(presenca);
                return StatusCode(201, presenca);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] PresencaDTO dto)
        {
            var presencaBuscada = await _presenca.BuscarPorId(id);

            if (presencaBuscada == null)
            {
                return NotFound("Presença não encontrada.");
            }

            var presenca = new Presenca
            {
                IdEvento = dto.IdEvento,
                IdUsuario = dto.IdUsuario,
                // O repositório atualiza apenas a Situacao, então ela precisa ser enviada
                Situacao = dto.Situacao
            };

            await _presenca.Atualizar(id, presenca);
            return Ok(presenca);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var presencaBuscada = await _presenca.BuscarPorId(id);

            if (presencaBuscada == null)
            {
                return NotFound("Presença não encontrada.");
            }

            // Só o próprio usuário pode cancelar a sua inscrição
            if (presencaBuscada.IdUsuario != User.ObterIdUsuario())
            {
                return Forbid();
            }

            await _presenca.Deletar(id);
            return NoContent();
        }
    }
}
