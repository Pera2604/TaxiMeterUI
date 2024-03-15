using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TaxiMeterUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Start a DispatcherTimer to update the TextBlock every second.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the TextBlock with the current date and time.
            txtDateTime.Text = DateTime.Now.ToString();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTariff_Click(object sender, RoutedEventArgs e)
        {
            if (txtTariff.Text == "Tariff" || txtTariff.Text == "Tariff 3")
            {
                txtTariff.Text = "Tariff 1";
            }
            else if ( txtTariff.Text == "Tariff 1")
            {
                txtTariff.Text = "Tariff 2";
            }
            else if ( txtTariff.Text == "Tariff 2")
            {
                txtTariff.Text = "Tariff 3";
            }
        }
    }
}