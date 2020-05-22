using FMS.DataManagers.Models;
using Prism.Mvvm;

namespace OrderAccounting.Common.Infrastructure.Models
{
    public class TransportModel : BindableBase
    {
        #region Properties

        public int LaborDetailVehicleId { get; set; }

        public bool HasAggregate { get; set; }

        private int? _vehicleId;
        public int? VehicleId
        {
            get { return _vehicleId; }
            set
            {
                if (_vehicleId != value)
                {
                    _vehicleId = value;
                    RaisePropertyChanged("VehicleId");
                }
            }
        }

        private int? _driverId;
        public int? DriverId
        {
            get { return _driverId; }
            set
            {
                if (_driverId != value)
                {
                    _driverId = value;
                    RaisePropertyChanged("DriverId");
                }
            }
        }

        private int? _vehicleTypeId;
        public int? VehicleTypeId
        {
            get { return _vehicleTypeId; }
            set
            {
                if (_vehicleTypeId != value)
                {
                    _vehicleTypeId = value;
                    RaisePropertyChanged("VehicleTypeId");
                }
            }
        }

        private int? _unitId;
        public int? UnitId
        {
            get { return _unitId; }
            set
            {
                if (_unitId != value)
                {
                    _unitId = value;
                    RaisePropertyChanged("UnitId");
                }
            }
        }

        private int? _unitTypeId;
        public int? UnitTypeId
        {
            get { return _unitTypeId; }
            set
            {
                if (_unitTypeId != value)
                {
                    _unitTypeId = value;
                    RaisePropertyChanged("UnitTypeId");
                }
            }
        }

        private string _driver;
        public string Driver
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

        private string _displayVehicle;
        public string DisplayVehicle
        {
            get { return _displayVehicle; }
            set
            {
                if (_displayVehicle != value)
                {
                    _displayVehicle = value;
                    RaisePropertyChanged("DisplayVehicle");
                }
            }
        }

        private string _displayUnit;
        public string DisplayUnit
        {
            get { return _displayUnit; }
            set
            {
                if (_displayUnit != value)
                {
                    _displayUnit = value;
                    RaisePropertyChanged("DisplayUnit");
                }
            }
        }

        private decimal _speed;
        public decimal Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    RaisePropertyChanged("Speed");
                }
            }
        }

        private decimal? _width;
        public decimal? Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }

        #endregion

        #region Methods

        internal static TransportModel ConvertToModel(VehicleModel vehicle, UnitModel unit, decimal speed, decimal? width, EmployeeModel driver)
        {
            var result = new TransportModel();

            if (vehicle != null)
            {
                result.VehicleId = vehicle.Id;
                result.VehicleTypeId = vehicle.TypeId;
                result.DisplayVehicle = vehicle.DisplayName;
            }

            result.Speed = speed;

            if (driver != null)
            {
                result.DriverId = driver.Id;

                //var employee = directoryManager.EmployeesCollection.FirstOrDefault(x => x.Id == result.DriverId);
                //if (employee != null)
                //{
                result.Driver = driver.FullName;
                //}
            }

            if (unit != null)
            {
                result.HasAggregate = true;
                result.UnitId = unit.Id;
                result.UnitTypeId = unit.TypeId;
                result.DisplayUnit = unit.DisplayName;
                result.Width = width;
            }

            return result;
        }

        #endregion

    }
}
