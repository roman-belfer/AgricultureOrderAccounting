using Argo.DataAccess.LaborDetail.Model;
using OrderAccounting.Common.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace OrderAccounting.Common.Repository.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Load active data from DB converting to List of IWorkOrderModel
        /// Out errorMessage if data not loaded
        /// </summary>
        IEnumerable<IOrderModel> GetActiveOrders(string accessToken, out string errorMessage);

        /// <summary>
        /// Load history data from DB converting to List of IWorkOrderModel
        /// Out errorMessage if data not loaded
        /// </summary>
        IEnumerable<IOrderModel> GetHistoryOrders(int pageNumber, IFilterModel filter, string accessToken, out string errorMessage);

        /// <summary>
        /// Load detail order data from DB converting to List of IOrderViewModel by orderId
        /// Out errorMessage if data not loaded
        /// </summary>
        IEnumerable<IOrderViewModel> GetMasterOrder(int orderId, string accessToken, out string errorMessage);

        /// <summary>
        /// Save collection of IOrderViewModel converting to service data type
        /// Out errorMessage if data id not saved
        /// </summary>
        bool SaveOrder(List<IOrderViewModel> _orderCollection, string accessToken, out string errorMessage);

        /// <summary>
        /// Change order status by orderId and statusId
        /// </summary>
        bool ChangeOrderStatus(int orderId, int statusId, string accessToken, out string errorMessage);

        /// <summary>
        /// Check is order can be canceled by id if it doesn't have any autodetected works inside
        /// </summary>
        bool? IsOrderCanBeCanceled(int orderId, string accessToken);

        IOrderModel ConvertToWorkModel(LaborDetail.ListItem order);
    }
}
