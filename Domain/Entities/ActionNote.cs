using CarSharingApp.Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Domain.Entities
{
    public sealed class ActionNote : Entity
    {
        private ActionNote(
            Guid id,
            Guid actorId,
            string note,
            DateTime actionMadeTime,
            Guid? actionEntityId)
            : base(id)
        {
            ActorId = actorId;
            Note = note;
            ActionMadeTime = actionMadeTime;
            ActionEntityId = actionEntityId;
        }

        [Required]
        public Guid ActorId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Note { get; set; }
        [Required]
        public DateTime ActionMadeTime { get; set; }

        public Guid? ActionEntityId { get; set; }

        // Methods to manipulate with existing notes

        public static ActionNote CreateNewNoteWithBlankedActionEntity(ActionNote noteToChange) =>
            new ActionNote(noteToChange.Id, noteToChange.ActorId, noteToChange.Note, noteToChange.ActionMadeTime, null);

        // Note templates which can be used to create database rows containing some useful data 

        // Customer notes section
        public const string RegisteredNoteStr = "You have successfully registered new account.";
        public const string UpdatedCustomerCredentialsNoteStr = "You have successfully updated account credentials.";
        public const string UpdatedCustomerInfoNoteStr = "You have successfully updated account info.";
        public const string UpdatedCustomerPasswordNoteStr = "You have successfully updated account password.";

        public static ActionNote RegisteredNote(Guid actorId) => new(new Guid(), actorId, RegisteredNoteStr, DateTime.Now, null);
        public static ActionNote UpdatedCustomerCredentialsNote(Guid actorId) => new(new Guid(), actorId, UpdatedCustomerCredentialsNoteStr, DateTime.Now, null);
        public static ActionNote UpdatedCustomerInfoNote(Guid actorId) => new(new Guid(), actorId, UpdatedCustomerInfoNoteStr, DateTime.Now, null);
        public static ActionNote UpdatedCustomerPasswordNote(Guid actorId) => new(new Guid(), actorId, UpdatedCustomerPasswordNoteStr, DateTime.Now, null);

        // Vehicles notes section
        public const string AddedNewVehicleNoteStr = "You have successfully added new vehicle.";
        public const string DeletedVehicleNoteStr = "You have successfully deleted vehicle.";
        public const string UpdatedVehicleNoteStr = "You have successfully updated vehicle info.";

        public static ActionNote AddedNewVehicleNote(Guid actorId, string vehicleName, Guid vehicleId) => 
            new(new Guid(), actorId, $"{AddedNewVehicleNoteStr} ({vehicleName})", DateTime.Now, vehicleId);
        public static ActionNote DeletedVehicleNote(Guid actorId, string vehicleName) =>
            new(new Guid(), actorId, $"{AddedNewVehicleNoteStr} ({vehicleName})", DateTime.Now, null);
        public static ActionNote UpdatedVehicleNote(Guid actorId, string vehicleName) =>
            new(new Guid(), actorId, $"{UpdatedVehicleNoteStr} ({vehicleName})", DateTime.Now, null);


    }
}
