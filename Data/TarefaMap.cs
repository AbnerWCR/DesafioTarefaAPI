using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefaAPI.Models;

namespace TarefaAPI.Data
{
    public class TarefaMap : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.ToTable("TAREFAS");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("integer");
            
            builder.Property(p => p.Titulo)
                .HasColumnName("TITULO")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasColumnName("DESCRICAO")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(p => p.Data)
                .HasColumnName("DATA")
                .HasColumnType("datetime")
                .IsRequired();
            
            builder.Property(p => p.Status)
                .HasColumnName("Status")
                .HasColumnType("smallint")
                .IsRequired();
        }
    }
}