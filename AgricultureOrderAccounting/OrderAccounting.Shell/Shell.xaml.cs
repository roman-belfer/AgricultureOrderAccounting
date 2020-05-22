using System.Diagnostics;
using System.Windows;

namespace OrderAccounting.Shell
{
    public partial class Shell : Window
    {
        #region Initialization

        public Shell()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Executors

        private void View_Closed(object sender, System.EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        #endregion

    }
}
