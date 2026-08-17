using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IEvento
    {
        Task<List<Evento>> Listar();

        Task<List<Evento>> ListarPorInstituicao(Guid idInstituicao);

        Task<List<Evento>> ListarPorInscrito(Guid id);

        Task<List<Evento>> ListarProximoEvento();
        
        Task<Evento?> BuscarPorId(Guid id);

        Task Cadastrar(Evento evento);

        Task Atualizar(Guid id, Evento evento);

        Task Deletar(Guid id);
    }
}
