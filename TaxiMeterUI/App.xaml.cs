using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace TaxiMeterUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Exit += App_Exit;
        }


        private void App_Exit(object sender, ExitEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }
    }
}
