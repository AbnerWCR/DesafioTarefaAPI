namespace TarefaAPI.Models
{
    public abstract record BaseModel(int? Id)
    {
        public abstract void Validar();
    }
}