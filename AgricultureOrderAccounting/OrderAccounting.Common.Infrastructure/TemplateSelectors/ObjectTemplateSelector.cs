using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccounting.Common.Infrastructure.TemplateSelectors
{
    public class ObjectTemplateSelector : DataTemplateSelector
    {
        #region Properties

        public DataTemplate SingleObjectTemplate { get; set; }

        public DataTemplate DoubleObjectTemplate { get; set; }

        #endregion

        #region Methods

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var order = item as ObjectItemViewModel;

            if (order != null)
            {
                if (order.IsSingleObject)
                {
                    return SingleObjectTemplate;
                }
                else
                {
                    return DoubleObjectTemplate;
                }
            }

            return null;
        }

        #endregion

    }
}
