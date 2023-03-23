using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Application.Services
{
    public sealed class ActionNotesService : IActionNotesService
    {
        private readonly IRepository<ActionNote> _noteRepository;

        public ActionNotesService(IRepository<ActionNote> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<List<ActionNote>> GetCustomerNotesWithLimitConnectedWithAccount(Guid customerId, int skip, int limit)
        {
            var result = await _noteRepository.GetAllAsync(n => (n.ActorId == customerId || n.ActionEntityId == customerId)
                && 10 <= n.Type && n.Type <= 19);

            result.ToList().Sort((x, y) => DateTime.Compare(x.ActionMadeTime, y.ActionMadeTime));

            return result.Reverse().Skip(skip).Take(limit).ToList();
        }

        public async Task<List<ActionNote>> GetCustomerNotesWithLimitConnectedWithOrders(Guid customerId, int skip, int limit)
        {
            var result = await _noteRepository.GetAllAsync(n => (n.ActorId == customerId || n.ActionEntityId == customerId)
                && 30 <= n.Type && n.Type <= 39);

            result.ToList().Sort((x, y) => DateTime.Compare(x.ActionMadeTime, y.ActionMadeTime));

            return result.Reverse().Skip(skip).Take(limit).ToList();
        }

        public async Task<List<ActionNote>> GetCustomerNotesWithLimitConnectedWithVehicles(Guid customerId, int skip, int limit)
        {
            var result = await _noteRepository.GetAllAsync(n => (n.ActorId == customerId || n.ActionEntityId == customerId)
                && 20 <= n.Type && n.Type <= 29);

            result.ToList().Sort((x, y) => DateTime.Compare(x.ActionMadeTime, y.ActionMadeTime));

            return result.Reverse().Skip(skip).Take(limit).ToList();
        }

        public async Task<List<ActionNote>> GetNotesWithLimitConnectedWithAnEntityAsync(Guid entityId, int skip, int limit)
        {
            var result = await _noteRepository.GetAllAsync(n => n.ActorId == entityId || n.ActionEntityId == entityId);

            result.ToList().Sort((x, y) => DateTime.Compare(x.ActionMadeTime, y.ActionMadeTime));

            return result.Reverse().Skip(skip).Take(limit).ToList();
        }
    }
}
