using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuario
    {
        private readonly EventContext _context;

        public TipoUsuarioRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, TipoUsuario tipoUsuario)
        {
            var tituloLimpo = tipoUsuario.Titulo.Trim();
            var jaExiste = await _context.TipoUsuario.AnyAsync(t =>
                t.IdTipoUsuario != id && t.Titulo.ToLower() == tituloLimpo.ToLower());

            if (jaExiste)
            {
                throw new InvalidOperationException("Já existe um tipo de usuário cadastrado com este título.");
            }

            var tipoUsuarioBuscado = await _context.TipoUsuario.FindAsync(id);

            if (tipoUsuarioBuscado != null)
            {
                tipoUsuarioBuscado.Titulo = tituloLimpo;
                _context.TipoUsuario.Update(tipoUsuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TipoUsuario?> BuscarPorId(Guid id)
        {
            return await _context.TipoUsuario.FirstOrDefaultAsync(t => t.IdTipoUsuario == id);
        }

        public async Task Cadastrar(TipoUsuario tipoUsuario)
        {
            var tituloLimpo = tipoUsuario.Titulo.Trim();
            var jaExiste = await _context.TipoUsuario.AnyAsync(t =>
                t.Titulo.ToLower() == tituloLimpo.ToLower());

            if (jaExiste)
            {
                throw new InvalidOperationException("Já existe um tipo de usuário cadastrado com este título.");
            }

            tipoUsuario.Titulo = tituloLimpo;
            await _context.TipoUsuario.AddAsync(tipoUsuario);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var tipoUsuarioBuscado = await _context.TipoUsuario
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.IdTipoUsuario == id);

            if (tipoUsuarioBuscado != null)
            {
                foreach (var usuario in tipoUsuarioBuscado.Usuario)
                {
                    usuario.IdTipoUsuario = null;
                }

                _context.TipoUsuario.Remove(tipoUsuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TipoUsuario>> Listar()
        {
            return await _context.TipoUsuario.AsNoTracking().ToListAsync();
        }
    }
}
