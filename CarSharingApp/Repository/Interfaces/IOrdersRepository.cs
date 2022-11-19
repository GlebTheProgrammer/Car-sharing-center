﻿using CarSharingApp.Models.OrderData;

namespace CarSharingApp.Repository.Interfaces
{
    public interface IOrdersRepository
    {
        public int GetNumberOfVehicleOrders(int vehicleId);
        public int GetNumberOfActiveOrdersForAUser(int userId);
        public List<OrderModel> GetActiveOrdersForAUser(int userId);
    }
}