using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace EventPlus.WebAPI.Controllers
{

    /// <summary>
    /// Controller responsavel pela autenticacao de usuarios via JWT(JSON Web Token).
    /// 
    /// Como funciona o JWT?
    /// 1. o usuario envia e-mail e senha via POST/api/Login.
    /// 2. A API valida as credencias no banco (e-mail e hash BCrypt).
    /// 3. Se valido, a API gera um Token JWT assinado  com a chave secreta.
    /// 4. O cliente use esse token no cabeçalho "Autorization: Bear {token} em todas as requisiçoes seguintes que exigem autenticacao ([Authorize])."
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuario usuario, IConfiguration configuration)
        {
            _usuario = usuario;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            //Buscar o usuario pelo Email e valida a senha BCrypt
            var usuarioBuscado = await _usuario.BuscarPorEmailESenha(dto.Email, dto.Senha);

            //Se as credenciais forem invalidas, retorna 401 Unauthorized
            if (usuarioBuscado == null)
            {
                return Unauthorized("Credenciais invalidas!");
            }

            //Criar a lista de Claims(informaçoes que ficam dentro do token)
            //Claims sao como "afirmaçoes" sobre o usuario que ficam codificadas no token

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioBuscado.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                new Claim("nome", usuarioBuscado.Nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Criar a chave de segurança com base na chave secreta definida
            var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            //Definir algoritimo de assinatura(HMACSHA256 é o padrao)
            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            //"Montar" o token JWT com as informações
            var token = new JwtSecurityToken(
                issuer: "EventPlus.WebAPI", //emite o token
                audience: "EventPlus.WebAPI", //quem tem permissão para consumir
                claims: claims, //informaçoes de identidade 
                expires:DateTime.UtcNow.AddHours(8), //tempo de expiração do token
                signingCredentials: credenciais); //

            //Converter o token para string e retornar para o Client
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new 
            { 
                Token = tokenString,
                Expiracao = token.ValidTo,
                Usuario = new
                {
                    usuarioBuscado.IdUsuario,
                    usuarioBuscado.Nome,
                    usuarioBuscado.Email
                }
            }
           );
        }
    }
}
