using Argo.DataAccess.LaborDetail.Model;
using FMS.Core;
using Microsoft.AspNet.SignalR.Client;
using OrderAccounting.Common.SignalRWorker.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OrderAccounting.Common.SignalRWorker.Workers
{
    public class OrderSignalRWorker : ISignalRWorker
    {
        #region Variables

        private bool _wasDisconnected = false;

        private const string ServerURI = "https://labordetailapi.testdomain.local/";

        private List<string> _methodsCollection;

        #endregion

        #region Properties

        public IHubProxy HubProxy { get; set; }
        public HubConnection Connection { get; set; }

        #endregion

        #region Events

        public event AddOrderEventHandler OrderAdded;
        public event AddOrderEventHandler OrderUpdated;
        public event RemoveOrderEventHandler OrderRemoved;

        #endregion

        #region Initialization

        public OrderSignalRWorker()
        {
            _methodsCollection = new List<string>();

            var thread = new Thread(() =>
            {
                ConnectAsync();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        #endregion

        #region Methods

        private async void ConnectAsync()
        {
            var connectionManager = HubConnectionManager.HubConnectionManager.GetHubConnectionManager(ServerURI);
            connectionManager.Closed += ConnectionManager_Closed;

            HubProxy = connectionManager.CreateHubProxy("LaborDetailHub");

            AddMethod<List<LaborDetail.ListItem>>("addLaborDetail", (orders) => OnOrderAdded(orders));

            AddMethod<List<LaborDetail.ListItem>>("updateLaborDetail", (orders) => OnOrderUpdated(orders));

            AddMethod<List<int>>("removeLaborDetail", (ids) => OnOrderRemoved(ids));

            try
            {
                await connectionManager.Initialize();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to connect to server: Start server before connecting clients.");

                if (!_wasDisconnected)
                {
                    ConnectAsync();
                }
            }
        }

        private void AddMethod<T>(string methodName, Action<T> action)
        {
            if (!_methodsCollection.Contains(methodName))
            {
                HubProxy.On<T>(methodName, action);
                _methodsCollection.Add(methodName);
            }
        }

        private void OnOrderRemoved(List<int> orderIds)
        {
            if (OrderRemoved != null)
            {
                OrderRemoved.Invoke(this, new OrderEventArgs(orderIds));
            }
        }

        private void OnOrderAdded(List<LaborDetail.ListItem> orders)
        {
            if (OrderAdded != null)
            {
                OrderAdded.Invoke(this, new OrderEventArgs(orders));
            }
        }

        private void OnOrderUpdated(List<LaborDetail.ListItem> orders)
        {
            if (OrderUpdated != null)
            {
                OrderUpdated.Invoke(this, new OrderEventArgs(orders));
            }
        }

        #endregion

        #region Event Executors

        private void ConnectionManager_Closed()
        {
            _wasDisconnected = true;

            Console.WriteLine("Connection closed");

            ConnectAsync();
        }

        #endregion

    }
}
