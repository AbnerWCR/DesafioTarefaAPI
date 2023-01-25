using Flunt.Notifications;
using Flunt.Validations;
using Newtonsoft.Json;
using TarefaAPI.Enums;
using TarefaAPI.Models;

namespace TarefaAPI.ViewModels
{
    public class CriarTarefaViewModel : Notifiable<Notification>
    {
        [JsonProperty(PropertyName = "titulo")]
        public string Titulo { get; set; }

        [JsonProperty(PropertyName = "descricao")]
        public string Descricao { get; set; }

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

            return new Tarefa(null, Titulo, Descricao, DateTime.Now, Status);
        }
    }
}