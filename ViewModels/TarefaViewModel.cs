using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using TarefaAPI.Enums;
using TarefaAPI.Models;

namespace TarefaAPI.ViewModels
{
    public class TarefaViewModel : Notifiable<Notification>
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "titulo")]
        public string Titulo { get; set; }

        [JsonProperty(PropertyName = "descricao")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DateTime Data { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Status Status { get; set; }

        public Tarefa MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Titulo, "Informe o título.")
                .IsLowerThan(Titulo, 50, "O título deve ter no máximo 50 caracteres.")
                .IsGreaterThan(Titulo, 3, "O título deve ter no mínimo 3 caracteres.")

                .IsNotNullOrEmpty(Descricao, "Informe a descrição.")
                .IsLowerThan(Descricao, 100, "A descrição deve ter no máximo 100 caracteres.")
                .IsGreaterThan(Descricao, 3, "A descrição deve ter no mínimo 3 caracteres."));

            return new Tarefa(Id, Titulo, Descricao, Data, Status);
        }

        public TarefaViewModel MapFrom(Tarefa tarefa)
        {
            return new TarefaViewModel
            { 
                Id = tarefa.Id.Value,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Data = tarefa.Data,
                Status = tarefa.Status,
            };
        }

        public IList<TarefaViewModel> MapFromList(IList<Tarefa> tarefas)
        {
            return tarefas.Select(tarefa => new TarefaViewModel
            {
                Id = tarefa.Id.Value,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Data = tarefa.Data,
                Status = tarefa.Status,
            }).ToList();
        }
    }
}
