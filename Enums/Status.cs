using System.Text.Json.Serialization;

namespace TarefaAPI.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        [JsonPropertyName("Pendente")]
        Pendente = 1,
    
        [JsonPropertyName("Em andamento")]
        EmAndamento = 2,
    
        [JsonPropertyName("Finalizado")]
        Finalizado = 3
    }
}