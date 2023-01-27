using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Account
{
    public record AccountDataResponse(
        [property: JsonPropertyName("customer")] CustomerToBeDisplayedInAccount Customer,
        [property: JsonPropertyName("actionNotes")] List<NoteToBeDisplayedInAccount> ActionNotes,
        [property: JsonPropertyName("vehicles")] List<VehicleToBeDisplayedInAccount> Vehicles
    );

    // Subentities are below

    public record CustomerToBeDisplayedInAccount(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("username")] string Username,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("country")] string Country,
        [property: JsonPropertyName("city")] string City,
        [property: JsonPropertyName("streetAddress")] string StreetAddress,
        [property: JsonPropertyName("aptSuiteEtc")] string AptSuiteEtc,
        [property: JsonPropertyName("phoneNumber")] string PhoneNumber
    );

    public record NoteToBeDisplayedInAccount(
        [property: JsonPropertyName("actorId")] Guid ActorId,
        [property: JsonPropertyName("note")] string Note,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("actionMadeTime")] DateTime ActionMadeTime,
        [property: JsonPropertyName("actionEntityId")] Guid? ActionEntityId
    );


    public record VehicleToBeDisplayedInAccount(
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("hourlyTariff")] decimal HourlyTariff,
        [property: JsonPropertyName("dailyTariff")] decimal DailyTariff,
        [property: JsonPropertyName("publishedTime")] DateTime PublishedTime,
        [property: JsonPropertyName("timesOrdered")] int TimesOrdered,
        [property: JsonPropertyName("isConfirmedByAdmin")] bool IsConfirmedByAdmin,
        [property: JsonPropertyName("isPublished")] bool IsPublished,
        [property: JsonPropertyName("isOrdered")] bool IsOrdered
    );
}
