using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class StuffItemViewModel : BaseDataItemViewModel, IOrderItemViewModel
    {
        #region Properties

        private ObservableCollection<ValueModel> _stuffCollection;
        public ObservableCollection<ValueModel> StuffCollection
        {
            get { return _stuffCollection; }
            set
            {
                if (value != null)
                {
                    if (_stuffCollection != value)
                    {
                        _stuffCollection = value;
                        _stuffCollection.CollectionChanged += StuffCollectionChanged;
                    }
                }
            }
        }

        #endregion

        #region Initialization

        public StuffItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            _directoryManager = directoryManager;
        }

        #endregion

        #region Event Executors

        private void StuffCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns validation bool value if base operation, operation and stuff data is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = true;

            //if (StuffCollection == null || StuffCollection.Count == 0)
            //{
            //    return false;
            //}

            //foreach (var stuff in StuffCollection)
            //{
            //    isValid = stuff.Id.HasValue;

            //    if (!isValid) break;
            //}

            return IsValid;
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);
            }
        }

        public override int? GetOperationId()
        {
            ///Empty method
            return null;
        }

        public override int? GetBaseOperationId()
        {
            ///Empty method
            return null;
        }

        public override int? GetPhaseId()
        {
            ///Empty method
            return null;
        }

        public override int? GetOperationTypeId()
        {
            ///Empty method
            return null;
        }

        public override void SetObjectData(bool isTransportation)
        {
            ///Empty method
        }

        #endregion

        #region Command Executors

        protected override void AddCommandExecute(object obj)
        {
            /// TO DO logic for adding new stuff to collection

        }
        protected override bool AddCommandCanExecute(object obj)
        {
            /// TO DO validaion logic for adding new stuff
            return false;
        }

        protected override void RemoveCommandExecute(object obj)
        {
            /// TO DO logic for removing the stuff from collection
        }

        #endregion

    }
}
