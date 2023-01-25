using TarefaAPI.Enums;
using TarefaAPI.Validators;

namespace TarefaAPI.Models
{
    public record Tarefa(int? Id, string Titulo, string Descricao, DateTime Data, Status Status)
        : BaseModel(Id)
    {
        public override void Validar()
        {
            var validator = new TarefaValidator();
            var validation = validator.Validate(this);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(x => x.ErrorMessage);

                throw new Exception(string.Join("; ", errors));
            }
        }
    }
}