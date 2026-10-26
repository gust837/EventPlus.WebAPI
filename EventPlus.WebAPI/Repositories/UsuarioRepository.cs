using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly EventContext _context;

        public UsuarioRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, Usuario usuario)
        {
            var usuarioBuscado = await _context.Usuario.FindAsync(id);

            if (usuarioBuscado != null)
            {
                usuarioBuscado.Nome = usuario.Nome;
                usuarioBuscado.Email = usuario.Email;
                usuarioBuscado.IdTipoUsuario = usuario.IdTipoUsuario;

                // Só altera a senha se uma nova senha tiver sido preenchida
                if (!string.IsNullOrWhiteSpace(usuario.Senha))
                {
                    usuarioBuscado.Senha = CriptografarUsuario.CriptografarSenha(usuario.Senha);
                }

                _context.Usuario.Update(usuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> BuscarPorEmailESenha(string email, string senha)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            bool senhaValida = CriptografarUsuario.VerificarSenha(senha, usuario.Senha);

            if (!senhaValida)
            {
                return null;
            }

            return usuario;
        }

        public async Task<Usuario?> BuscarPorId(Guid id)
        {
            return await _context.Usuario.Include(e => e.IdTipoUsuarioNavigation).FirstOrDefaultAsync(t => t.IdUsuario == id);
        }

        public async Task Cadastrar(Usuario usuario)
        {
            usuario.Senha = CriptografarUsuario.CriptografarSenha(usuario.Senha);

            await _context.Usuario.AddAsync(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var usuarioBuscado = await _context.Usuario
                .Include(u => u.Comentario)
                .Include(u => u.Presenca)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuarioBuscado != null)
            {
                _context.Comentario.RemoveRange(usuarioBuscado.Comentario);
                _context.Presenca.RemoveRange(usuarioBuscado.Presenca);
                _context.Usuario.Remove(usuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            return await _context.Usuario.Include(e => e.IdTipoUsuarioNavigation).AsNoTracking().ToListAsync();
        }
    }
}
