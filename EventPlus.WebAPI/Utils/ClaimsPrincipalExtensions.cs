using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EventPlus.WebAPI.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Lê o Id do usuário logado a partir do token JWT (claim "sub").
        /// O ASP.NET pode mapear o "sub" para ClaimTypes.NameIdentifier, por isso verificamos os dois.
        /// </summary>
        public static Guid? ObterIdUsuario(this ClaimsPrincipal usuario)
        {
            var valor = usuario.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? usuario.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(valor, out var id) ? id : null;
        }
    }
}
