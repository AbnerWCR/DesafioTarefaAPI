using Microsoft.EntityFrameworkCore;
using TarefaAPI.Models;

namespace TarefaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {     

        }

        DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TarefaMap());

            base.OnModelCreating(builder);
        }
    }
}