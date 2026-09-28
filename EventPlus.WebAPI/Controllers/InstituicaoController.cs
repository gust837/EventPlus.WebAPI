using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
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
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _instituicao.Listar();

                return Ok(tipos);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Cadastra uma nova instituição 
        /// </summary>
        /// <param name="tipoEvento">Perfil da instituição</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] InstituicaoDTO dto)
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

        /// <summary>
        /// Esse metodo atualiza o nome fantasia de instituicao com base no id
        /// </summary>
        /// <param name="id">Id que sera atualizado</param>
        /// <param name="dto">Titulo que sera atualizado</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] InstituicaoDTO dto)
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

        /// <summary>
        /// Remove uma instituição pelo ID
        /// </summary>
        /// <param name="id">Id da instituição a ser removido</param>
        /// 
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _instituicao.Deletar(id);
            return NoContent();
        }
    }
}
