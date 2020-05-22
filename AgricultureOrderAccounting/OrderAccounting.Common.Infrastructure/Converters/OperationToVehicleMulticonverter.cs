using FMS.DataManagers.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class OperationToVehicleMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var baseOperationId = values[0] as int?;
            var operationId = values[1] as int?;
            var upperTypeId = values[2] as int?;
            var directoryManager = values[3] as IDirectoryManager;

            var vehicleTypes = directoryManager.VehicleTypesInformation;
            var vehicleModels= directoryManager.VehicleModelsInformation;
            var operationCombinations = directoryManager.OperationCombinations;
            var vehicleGroups = directoryManager.VehicleGroupInformation;

            if (baseOperationId.HasValue && upperTypeId.HasValue )
            {
                var selectedOperationCombinations = operationCombinations.Where(y => y.BaseOperationID == baseOperationId && (operationId.HasValue ? y.OperationID == operationId : true)).ToList();

                var modelCombinations = selectedOperationCombinations.Select(x => x.VehicleModelID).ToList();

                var vehicles = vehicleModels.Where(x => modelCombinations.Contains(x.VehModelId)).Select(x => x.VehModelVehTypeId).ToList();

                if (vehicles != null && vehicles.Count > 0)
                {
                    return vehicleTypes.Where(x => x.VehicleTypeUpperTypeId == upperTypeId && vehicles.Contains(x.VehicleTypeId)).ToList(); 
                }
                else
                {
                    var groupsCombinations = selectedOperationCombinations.Select(x => x.VehicleGroupID).ToList();

                    var groups = vehicleGroups.Where(x => groupsCombinations.Contains(x.VGId)).Select(x => x.VGId).ToList();

                    if (groups != null && groups.Count > 0)
                    {
                        var models = directoryManager.VehicleModelsInformation.Where(x => groups.Contains(x.VehModelGroupId.HasValue ? x.VehModelGroupId.Value : 0)).Select(x => x.VehModelId).ToList();

                        if (models != null && models.Count > 0)
                        {
                            return vehicleTypes.Where(x => x.VehicleTypeUpperTypeId == upperTypeId && vehicles.Contains(x.VehicleTypeId)).ToList();
                        }
                    }
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
