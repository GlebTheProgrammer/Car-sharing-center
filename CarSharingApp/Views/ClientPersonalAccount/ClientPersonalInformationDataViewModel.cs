using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.OrderData;
using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Views.UserPersonalAccount
{
    public class ClientPersonalInformationDataViewModel
    {
        public ClientAccountViewModel ClientAccountViewModel { get; set; }

        public int VehiclesAdded { get; set; }
        public int ActiveVehicles { get; set; }
        public int ActiveOrdersCount { get; set; }

        public List<OrderInUserAccountViewModel> ActveOrders { get; set; }
        public List<VehicleAccountViewModel> ClientVehicles { get; set; }
    }
}
