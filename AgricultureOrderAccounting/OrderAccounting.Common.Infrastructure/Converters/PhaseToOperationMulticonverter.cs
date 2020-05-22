using FMS.DataManagers.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class PhaseToOperationMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                var directoryManager = values[0] as IDirectoryManager;
                var phaseId = values[1] as int?;
                var operTypeId = values[2] as int?;

                var operationTypes = directoryManager.OperationTypes;
                var baseOperationPhases = directoryManager.BaseOperationPhases;
                var baseOperations = directoryManager.BaseOperations;

                var operationType = operationTypes.FirstOrDefault(x => x.Identity == operTypeId);

                if (operationType != null && operationType.Name == "mainproduction" && phaseId.HasValue)
                {
                    var oprations = baseOperationPhases.Where(x => x.OperationCategoryID == phaseId).Select(x => x.BaseOperationID);
                    return baseOperations.Where(z => oprations.Contains(z.Identity)).ToList();
                }
                else
                {
                    return baseOperations;
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
