using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IPresenca
    {
        Task<List<Presenca>> Listar();

        Task<List<Presenca>> ListarPresenca(Guid idPresenca);

        Task<Presenca?> BuscarPorId(Guid id);

        Task Cadastrar(Presenca presenca);

        Task Deletar(Guid id);

        Task Atualizar(Guid id, Presenca presenca);

    }
}
