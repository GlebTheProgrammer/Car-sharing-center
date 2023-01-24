using CarSharingApp.Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Domain.Entities
{
    public sealed class ActionNote : Entity
    {
        public const string RegisteredNoteStr = "You have successfully registered new account.";
        public const string AddedNewVehicleNoteStr = "You have successfully added new vehicle.";
        public const string DeletedVehicleNoteStr = "You have successfully deleted vehicle.";

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

        public static ActionNote RegisteredNote(Guid actorId) => new(new Guid(), actorId, RegisteredNoteStr, DateTime.Now, null);

        public static ActionNote AddedNewVehicleNote(Guid actorId, string vehicleName, Guid vehicleId) => 
            new(new Guid(), actorId, $"{AddedNewVehicleNoteStr} ({vehicleName})", DateTime.Now, vehicleId);
        public static ActionNote DeletedVehicleNote(Guid actorId, string vehicleName) =>
            new(new Guid(), actorId, $"{AddedNewVehicleNoteStr} ({vehicleName})", DateTime.Now, null);

    }
}
