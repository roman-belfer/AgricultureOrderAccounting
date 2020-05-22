using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using FMS.Repository.Helpers;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Models;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels;
using OrderAccounting.Common.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OrderAccounting.Common.Repository.Repositories
{
    public class OrderApiRepository : IOrderRepository
    {
        #region Variables

        private IDirectoryManager _directoryManager;

        private IOrderItemFactory _orderItemFactory;

        const string apiLaborDetail = "https://labordetailapi.testdomain.local/api";

        #endregion

        #region Initialization

        public OrderApiRepository(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
        {
            _directoryManager = directoryManager;
            _orderItemFactory = orderItemFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save collection of IOrderViewModel converting to service data type
        /// Out errorMessage if data id not saved
        /// </summary>
        public IEnumerable<IOrderModel> GetActiveOrders(string accessToken, out string errorMessage)
        {
            errorMessage = null;
            var laborDetailFilter = new LaborDetail.Filter();
            laborDetailFilter.IsActive = true;

            List<IOrderModel> result = new List<IOrderModel>();

            var orders = WebApiWorker.GetResult<List<LaborDetail.ListItem>>(apiLaborDetail + "/LaborDetail/Get", accessToken, out errorMessage, laborDetailFilter);

            if (string.IsNullOrEmpty(errorMessage))
            {
                result = ConvertToWorkModel(orders);
            }

            return result;
        }

        /// <summary>
        /// Load active data from DB converting to List of IWorkOrderModel
        /// Out errorMessage if data not loaded
        /// </summary>
        public IEnumerable<IOrderModel> GetHistoryOrders(int page, IFilterModel filter, string accessToken, out string errorMessage)
        {
            errorMessage = null;

            var laborDetailFilter = filter.ConvertFromModel<LaborDetail.Filter>();
            laborDetailFilter.IsActive = false;
            laborDetailFilter.Page = page;

            List<IOrderModel> result = new List<IOrderModel>();

            var orders = WebApiWorker.GetResult<List<LaborDetail.ListItem>>(apiLaborDetail + "/LaborDetail/Get", accessToken, out errorMessage, laborDetailFilter);

            if (string.IsNullOrEmpty(errorMessage))
            {
                result = ConvertToWorkModel(orders);
            }

            return result;
        }

        /// <summary>
        /// Load history data from DB converting to List of IWorkOrderModel
        /// Out errorMessage if data not loaded
        /// </summary>
        public IEnumerable<IOrderViewModel> GetMasterOrder(int orderId, string accessToken, out string errorMessage)
        {
            errorMessage = string.Empty;

            var result = new List<IOrderViewModel>();

            var order = WebApiWorker.GetResult<LaborDetail.Item>(apiLaborDetail + "/LaborDetail/Get/" + orderId, accessToken, out errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                var labor = new BasicWorkViewModel(_directoryManager, _orderItemFactory);
                labor.ConvertToViewModel(order);
                result.Add(labor);
                foreach (var item in order.SubLaborDetails)
                {
                    var additionalLabor = new AdditionalWorkViewModel(_directoryManager, _orderItemFactory);
                    additionalLabor.ConvertToViewModel(item);
                    result.Add(additionalLabor);
                }

                if (order.ManualOperations.Count > 0 || order.Employees.Count > 0)
                {
                    var manual = new ManualWorkViewModel(_directoryManager, _orderItemFactory);
                    manual.ConvertToViewModel(order);
                    result.Add(manual);
                }

                foreach (var i in result)
                {
                    i.Index = result.IndexOf(i);
                }
            }

            return result;
        }

        /// <summary>
        /// Load detail order data from DB converting to List of IOrderViewModel by orderId
        /// Out errorMessage if data not loaded
        /// </summary>
        public bool SaveOrder(List<IOrderViewModel> orderCollection, string accessToken, out string errorMessage)
        {
            errorMessage = null;

            LaborDetail.Item result = new LaborDetail.Item();
            var mainlabor = orderCollection.FirstOrDefault(x => x is BasicWorkViewModel);
            if (mainlabor != null)
            {
                mainlabor.ConvertToModel(result);
                var sublaborOrderCollection = orderCollection.ToList();
                sublaborOrderCollection.Remove(mainlabor);
                foreach (var item in sublaborOrderCollection)
                {
                    item.ConvertToModel(result);
                }
            }

            if (result.Identity > 0)
            {
                WebApiWorker.PutData(apiLaborDetail + "/LaborDetail/Put", result, null, out errorMessage);
            }
            else
            {
                WebApiWorker.PostData(apiLaborDetail + "/LaborDetail/Post", result, null, out errorMessage);
            }

            return string.IsNullOrEmpty(errorMessage);
        }

        /// <summary>
        /// Change order status by orderId and statusId
        /// </summary>
        public bool ChangeOrderStatus(int LaborDetailID, int StatusID, string accessToken, out string errorMessage)
        {
            WebApiWorker.PostData(apiLaborDetail + "/LaborDetail/ChangeStatus", new { LaborDetailID, StatusID }, null, out errorMessage);

            return string.IsNullOrEmpty(errorMessage);
        }

        /// <summary>
        /// Check is order can be canceled by id if it doesn't have any autodetected works inside
        /// </summary>
        public bool? IsOrderCanBeCanceled(int orderId, string accessToken)
        {
            string errorMessage = "";

            var canBeCanceled = WebApiWorker.GetResult<bool?>(apiLaborDetail + "/LaborDetail/GetCanCancel/" + orderId, accessToken, out errorMessage);

            return canBeCanceled;
        }

        private List<IOrderModel> ConvertToWorkModel(List<LaborDetail.ListItem> orders)
        {
            var result = new List<IOrderModel>();

            foreach (var i in orders)
            {
                result.Add(ConvertToWorkModel(i));
            }

            return result;
        }

        public IOrderModel ConvertToWorkModel(LaborDetail.ListItem order)
        {
            OrderModel item = null;

            if (order != null)
            {
                item = new OrderModel(_directoryManager);

                if (order.ParentID.HasValue)
                {
                    item.MasterNumber = order.ParentID.Value;
                    item.TypeId = 1;
                }
                else
                {
                    item.MasterNumber = order.Identity;
                    item.TypeId = 0;
                }

                item.OperationTypeId = order.OperationTypeID;
                item.BaseOperationId = order.BaseOperationID;
                item.Id = order.Identity;
                item.Number = order.Identity.ToString();
                item.ActualPhaseId = order.ActualPhaseID;
                item.OperationId = order.OperationID;
                item.DateFrom = order.DateFrom;
                item.DateTo = order.DateTo;

                var status = _directoryManager.LaborStatus.FirstOrDefault(x => x.Identity == order.StatusID);
                if (status != null)
                {
                    item.StateName = status.Name;
                }

                item.UnitId = order.UnitID;
                item.VehicleId = order.VehicleID;
            }

            return item;
        }

        #endregion

    }
}
