using Argo.DataAccess.LaborDetail.Model;
using FMS.Core;
using FMS.DataManagers.Interfaces;
using FMS.DataManagers.Models;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class TransportItemViewModel : BaseDataItemViewModel, IOrderItemViewModel
    {
        #region Properties

        private ObservableCollection<TransportModel> _transportCollection;
        public ObservableCollection<TransportModel> TransportCollection
        {
            get { return _transportCollection; }
            set
            {
                if (value != null)
                {
                    if (_transportCollection != value)
                    {
                        _transportCollection = value;
                        _transportCollection.CollectionChanged += TransportCollectionChanged;
                    }
                }
            }
        }

        private int? _vehicleUpperTypeId;
        public int? VehicleUpperTypeId
        {
            get { return _vehicleUpperTypeId; }
            set
            {
                if (value != null)
                {
                    if (_vehicleUpperTypeId != value)
                    {
                        _vehicleUpperTypeId = value;
                        RaisePropertyChanged("VehicleUpperTypeId");
                    }
                }
            }
        }

        private int? _unitUpperTypeId;
        public int? UnitUpperTypeId
        {
            get { return _unitUpperTypeId; }
            set
            {
                if (value != null)
                {
                    if (_unitUpperTypeId != value)
                    {
                        _unitUpperTypeId = value;
                        RaisePropertyChanged("UnitUpperTypeId");
                    }
                }
            }
        }

        private decimal? _speed;
        public decimal? Speed
        {
            get { return _speed; }
            set
            {
                if (value != null)
                {
                    if (_speed != value)
                    {
                        _speed = value;
                        RaisePropertyChanged("Speed");
                    }
                }
            }
        }

        private decimal? _width;
        public decimal? Width
        {
            get { return _width; }
            set
            {
                if (value != null)
                {
                    if (_width != value)
                    {
                        _width = value;
                        RaisePropertyChanged("Width");
                    }
                }
            }
        }

        private VehicleModel _vehicle;
        public VehicleModel Vehicle
        {
            get { return _vehicle; }
            set
            {
                if (_vehicle != value)
                {
                    _vehicle = value;
                    RaisePropertyChanged("Vehicle");
                }
            }
        }

        private UnitModel _unit;
        public UnitModel Unit
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    RaisePropertyChanged("Unit");
                }
            }
        }

        private EmployeeModel _driver;
        public EmployeeModel Driver
        {
            get { return _driver; }
            set
            {
                if (_driver != value)
                {
                    _driver = value;
                    RaisePropertyChanged("Driver");
                }
            }
        }

        #endregion

        #region Initialization

        public TransportItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            Title = "Транспортні засоби / Агрегати / Причепи";

            TransportCollection = new ObservableCollection<TransportModel>();
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            VehicleUpperTypeCommand = new RellayCommand(VehicleUpperTypeCommandExecute);
            UnitUpperTypeCommand = new RellayCommand(UnitUpperTypeCommandExecute);
        }

        #endregion

        #region Event Executors

        private void TransportCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataChanged();
        }

        #endregion

        #region Commands

        public RellayCommand VehicleUpperTypeCommand { get; set; }

        public RellayCommand UnitUpperTypeCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns validation bool value if model data is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = false;

            if (TransportCollection == null || TransportCollection.Count == 0)
            {
                return false;
            }

            foreach (var transport in TransportCollection)
            {
                IsValid = transport.VehicleId.HasValue && transport.DriverId.HasValue;

                IsValid &= transport.Speed > 0;

                if (transport.HasAggregate)
                {
                    IsValid &= transport.UnitId.HasValue;

                    IsValid &= transport.Width > 0;
                }

                if (!IsValid) break;
            }

            return IsValid;
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                model.Vehicles = ConvertToList();
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);
                foreach (var vehicle in model.Vehicles)
                {
                    var _Vehicle = _directoryManager.VehiclesCollection.FirstOrDefault(x => x.Id == vehicle.Vehicle.Identity);
                    var _Driver = _directoryManager.EmployeesCollection.FirstOrDefault(x => x.Id == vehicle.Driver.Identity);
                    UnitModel _Unit = null;
                    if (vehicle.Unit != null)
                    {
                        _Unit = _directoryManager.UnitsCollection.FirstOrDefault(x => x.Id == vehicle.Unit.Identity);
                    }
                    var transportModel = TransportModel.ConvertToModel(_Vehicle, _Unit, (vehicle.Speed.HasValue ? vehicle.Speed.Value : 0), vehicle.WorkWidth, _Driver);
                    transportModel.LaborDetailVehicleId = vehicle.Identity;
                    TransportCollection.Add(transportModel);
                }
            }
        }

        private IList<LaborDetailVehicle.Item> ConvertToList()
        {
            var result = new List<LaborDetailVehicle.Item>();

            foreach (var i in TransportCollection)
            {
                var model = new LaborDetailVehicle.Item();

                model.Identity = i.LaborDetailVehicleId;

                model.Vehicle = i.VehicleId.HasValue ? new Argo.DataAccess.All.Models.Vehicle.ListItem()
                {
                    Identity = i.VehicleId.Value
                } : null;

                model.Unit = i.UnitId.HasValue ? new Argo.DataAccess.All.Models.Unit.ListItem()
                {
                    Identity = i.UnitId.Value
                } : null;

                model.Driver = i.DriverId.HasValue ? new Argo.DataAccess.All.Models.Employee.ListItem()
                {
                    Identity = i.DriverId.Value
                } : null;

                model.Speed = i.Speed;
                model.WorkWidth = i.Width;

                result.Add(model);
            }

            return result;
        }

        public override int? GetOperationId()
        {
            return null;
        }

        public override int? GetBaseOperationId()
        {
            return null;
        }

        public override int? GetPhaseId()
        {
            return null;
        }

        public override int? GetOperationTypeId()
        {
            return null;
        }

        public override void SetObjectData(bool isTransportation)
        { }

        #endregion

        #region Command Executors

        protected override void RemoveCommandExecute(object obj)
        {
            if (!IsRemoveCommited())
            {
                return;
            }

            var transport = obj as TransportModel;
            if (TransportCollection != null && TransportCollection.Count > 0 && transport != null)
            {
                TransportCollection.Remove(transport);
            }
        }

        protected override void AddCommandExecute(object obj)
        {
            TransportCollection.Add(TransportModel.ConvertToModel(Vehicle, Unit, Speed.Value, Width, Driver));
        }
        protected override bool AddCommandCanExecute(object obj)
        {
            return Vehicle != null && Driver != null && Speed > 0 && (Unit != null ? Width > 0 : true);
        }

        private void VehicleUpperTypeCommandExecute(object obj)
        {
            var id = obj as int?;

            if (id.HasValue)
            {
                VehicleUpperTypeId = id;
            }
        }

        private void UnitUpperTypeCommandExecute(object obj)
        {
            var id = obj as int?;

            if (id.HasValue)
            {
                UnitUpperTypeId = id;
            }
        }

        #endregion

    }
}
