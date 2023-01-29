using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Application.Interfaces
{
    public interface IActionNotesService
    {
        public Task<List<ActionNote>> GetNotesWithLimitConnectedWithAnEntityAsync(Guid entityId, int skip, int limit);
        public Task<List<ActionNote>> GetCustomerNotesWithLimitConnectedWithAccount(Guid customerId, int skip, int limit);
        public Task<List<ActionNote>> GetCustomerNotesWithLimitConnectedWithVehicles(Guid customerId, int skip, int limit);
        public Task<List<ActionNote>> GetCustomerNotesWithLimitConnectedWithOrders(Guid customerId, int skip, int limit);
    }
}
