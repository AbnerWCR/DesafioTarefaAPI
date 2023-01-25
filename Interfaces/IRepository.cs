using TarefaAPI.Enums;
using TarefaAPI.Models;

namespace TarefaAPI.Interfaces
{
    public interface IRepository<TModel> 
        where TModel : BaseModel
    {
        Task<TModel> CreateAsync(TModel model);
        Task<TModel> UpdateAsync(TModel model);
        Task DeleteAsync(int id);
        Task<TModel> GetByIdAsync(int id);
        Task<IList<TModel>> GetAllAsync();
    }
}