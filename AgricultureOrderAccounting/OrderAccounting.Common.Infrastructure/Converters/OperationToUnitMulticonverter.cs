using FMS.DataManagers.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class OperationToUnitMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var baseOperationId = values[0] as int?;
            var operationId = values[1] as int?;
            var upperTypeId = values[2] as int?;
            var directoryManager = values[3] as IDirectoryManager;

            var unitInformation = directoryManager.UnitsCollection;
            var operationCombinations = directoryManager.OperationCombinations;
            var vehicleGroups = directoryManager.VehicleGroupInformation;

            if (baseOperationId.HasValue && upperTypeId.HasValue)
            {
                var selectedOperationCombinations = operationCombinations.Where(y => y.BaseOperationID == baseOperationId && (operationId.HasValue ? y.OperationID == operationId : true)).ToList();

                var modelCombinations = selectedOperationCombinations.Select(x => x.UnitModelID).ToList();

                var units = unitInformation.Where(x => modelCombinations.Contains(x.ModelId)).ToList();
                if (units != null && units.Count > 0)
                {
                    return units.Where(x => x.UpperTypeId == upperTypeId).ToList();
                }
                else
                {
                    var groupsCombinations = selectedOperationCombinations.Select(x => x.VehicleGroupID).ToList();

                    var groups = vehicleGroups.Where(x => groupsCombinations.Contains(x.VGId)).Select(x => x.VGId).ToList();

                    if (groups != null && groups.Count > 0)
                    {
                        var models = directoryManager.UnitModelsInformation.Where(x => groups.Contains(x.UnitModelId)).Select(x => x.UnitModelId).ToList();

                        if (models != null && models.Count > 0)
                        {
                            return unitInformation.Where(x => models.Contains(x.ModelId) && x.UpperTypeId == upperTypeId).ToList();
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
