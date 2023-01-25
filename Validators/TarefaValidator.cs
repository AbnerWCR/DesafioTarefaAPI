using FluentValidation;
using TarefaAPI.Models;

namespace TarefaAPI.Validators
{
    public class TarefaValidator : AbstractValidator<Tarefa>
    {
        public TarefaValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Tarefa não pode ser nula");

            RuleFor(x => x.Titulo)
                .NotNull()
                .WithMessage("Titulo não pode ser nulo")
                
                .MaximumLength(3)
                .WithMessage("Mínimo 3 caracteres.")
                
                .MaximumLength(50)
                .WithMessage("Máximo 50 caracteres.");

            RuleFor(x => x.Descricao)
                .NotNull()
                .WithMessage("Descricao não pode ser nulo")
                
                .MaximumLength(3)
                .WithMessage("Mínimo 3 caracteres.")
                
                .MaximumLength(100)
                .WithMessage("Máximo 50 caracteres.");

            RuleFor(x => x.Data)
                .NotNull()
                .WithMessage("Data não pode ser nulo");

            RuleFor(x => x.Status)
                .NotNull()
                .WithMessage("Status não pode ser nulo");
        }
    }
}