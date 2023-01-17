using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.ErrorType
{
    public sealed class ValidationError : Error
    {
        [property: JsonPropertyName("errors")] public Dictionary<string, List<string>> Errors { get; init; } = new Dictionary<string, List<string>>();
    }
}
