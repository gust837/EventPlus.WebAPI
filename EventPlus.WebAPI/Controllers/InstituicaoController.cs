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
    public class InstituicaoController : ControllerBase
    {
        private readonly IInstituicao _instituicao;

        public InstituicaoController(IInstituicao instituicao)
        {
            _instituicao = instituicao;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var instituicaoBuscada = await _instituicao.BuscarPorId(id);

            if (instituicaoBuscada == null)
            {
                return NotFound("Instituição não encontrada!");
            }

            return Ok(instituicaoBuscada);
        }

        /// <summary>
        /// Lista todas as instituições
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var instituicoes = await _instituicao.Listar();
                return Ok(instituicoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Cadastra uma nova instituição
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] InstituicaoDTO dto)
        {
            try
            {
                var instituicao = new Instituicao
                {
                    NomeFantasia = dto.NomeFantasia,
                    Cnpj = dto.CNPJ,
                    Endereco = dto.Endereco
                };

                await _instituicao.Cadastrar(instituicao);
                return StatusCode(201, instituicao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma instituição com base no ID
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] InstituicaoDTO dto)
        {
            try
            {
                var instituicaoBuscada = new Instituicao
                {
                    NomeFantasia = dto.NomeFantasia,
                    Cnpj = dto.CNPJ,
                    Endereco = dto.Endereco
                };

                await _instituicao.Atualizar(id, instituicaoBuscada);
                return Ok(instituicaoBuscada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Remove uma instituição pelo ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _instituicao.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
