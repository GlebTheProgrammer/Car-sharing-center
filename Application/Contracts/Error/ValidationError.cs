using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Error
{
    public sealed class ValidationError
    {
        [property: JsonPropertyName("type")] public string Type { get; init; } = string.Empty;
        [property: JsonPropertyName("title")] public string Title { get; init; } = string.Empty;
        [property: JsonPropertyName("status")] public int Status { get; init; }
        [property: JsonPropertyName("traceId")] public string TraceId { get; init; } = string.Empty;
        [property: JsonPropertyName("errors")] public Dictionary<string, List<string>> Errors { get; init; } = new Dictionary<string, List<string>>();
    }
}
