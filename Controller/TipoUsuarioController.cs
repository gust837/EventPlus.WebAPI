using EventPlus.WebAPI.Interfaces;
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

    }
}
