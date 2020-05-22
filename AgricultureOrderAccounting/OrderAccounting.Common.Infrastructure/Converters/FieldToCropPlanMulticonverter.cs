using FMS.Services.NetTcpDirectoryServiceReference;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class FieldToCropPlanMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() > 0)
            {
                var cropPlans = values[0] as List<CropPlansExtendedNetTcp>;
                var fieldVersions = values[1] as List<FieldVersionsNetTcp>;
                var field = values[2] as FieldsNetTcp;
                var year = values[3] as int?;

                var cropPlansData = fieldVersions.SelectMany(x => x.CropPlans).ToList();

                if (cropPlans != null && cropPlans.Count > 0 && fieldVersions != null && fieldVersions.Count > 0 && field != null && year.HasValue)
                {
                    var fieldVers = fieldVersions.FirstOrDefault(x => x.FVersFieldId == field.FldId && x.FVersYear == year);
                    if (fieldVers != null)
                    {
                        return fieldVers.CropPlans;
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
