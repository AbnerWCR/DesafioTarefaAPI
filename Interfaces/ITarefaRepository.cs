using TarefaAPI.Enums;
using TarefaAPI.Models;

namespace TarefaAPI.Interfaces
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
        Task<IList<Tarefa>> GetByTitleAsync(string titulo);
        Task<IList<Tarefa>> GetByDateAsync(DateTime data);
        Task<IList<Tarefa>> GetByStatusAsync(Status status);
    }
}