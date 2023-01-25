using Microsoft.EntityFrameworkCore;
using TarefaAPI.Enums;
using TarefaAPI.Interfaces;
using TarefaAPI.Models;

namespace TarefaAPI.Data
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        
        public async Task<IList<Tarefa>> GetByDateAsync(DateTime data)
        {
            return await _context.Set<Tarefa>()
                                    .AsNoTracking()
                                    .Where(t => t.Data == data ||
                                                t.Data.Date == data.Date)
                                    .ToListAsync();
        }

        public async Task<IList<Tarefa>> GetByStatusAsync(Status status)
        {
             return await _context.Set<Tarefa>()
                                    .AsNoTracking()
                                    .Where(t => t.Status == status)
                                    .ToListAsync();
        }

        public async Task<IList<Tarefa>> GetByTitleAsync(string titulo)
        {
            return await _context.Set<Tarefa>()
                                    .AsNoTracking()
                                    .Where(t => t.Titulo == titulo)
                                    .ToListAsync();
        }
    }
}