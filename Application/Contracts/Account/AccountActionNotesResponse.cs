using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Account
{
    public sealed record AccountActionNotesResponse(
        [property: JsonPropertyName("actionNotes")] List<NoteToBeDisplayedInAccount> ActionNotes
    );

    public sealed record NoteToBeDisplayedInAccount(
        [property: JsonPropertyName("actorId")] Guid ActorId,
        [property: JsonPropertyName("note")] string Note,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("actionMadeTime")] DateTime ActionMadeTime,
        [property: JsonPropertyName("actionEntityId")] Guid? ActionEntityId
    );
}
