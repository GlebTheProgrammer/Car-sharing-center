using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.ErrorType
{
    public abstract class Error
    {
        [property: JsonPropertyName("type")] public string Type { get; init; } = string.Empty;
        [property: JsonPropertyName("title")] public string Title { get; init; } = string.Empty;
        [property: JsonPropertyName("status")] public int Status { get; init; }
        [property: JsonPropertyName("traceId")] public string TraceId { get; init; } = string.Empty;
    }
}
