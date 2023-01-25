using Microsoft.EntityFrameworkCore;
using TarefaAPI.Interfaces;
using TarefaAPI.Models;

namespace TarefaAPI.Data
{
    public class BaseRepository<TModel> : IRepository<TModel>
        where TModel : BaseModel
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TModel> CreateAsync(TModel model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<TModel> UpdateAsync(TModel model)
        {
            _context.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await GetByIdAsync(id);

            if (model == null)
                return;

            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TModel>> GetAllAsync()
        {
            return await _context.Set<TModel>()
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            return await _context.Set<TModel>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}