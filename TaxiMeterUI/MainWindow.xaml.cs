using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Timers;

namespace TaxiMeterUI
{
    public partial class MainWindow : Window
    {
        private TaxiRide taxiRide;
        private CancellationTokenSource cancellationTokenSource;
        private DispatcherTimer timer = new DispatcherTimer();

        public double TotalLiveTaxiPrice { get; set; }
        public double DistanceRate { get; set; }
        public double DurationRate { get; set; }
        public double PickupLocationLat { get; set; }
        public double PickupLocationLong { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationLong { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
        public double Price { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            mainGrid.RowDefinitions[2].Height = new GridLength(0);
            DateAndTime();
            Closing += MainWindow_Closing; // Subscribe to the Closing event

            taxiRide = new TaxiRide();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // stops the timer from continuing to calculate the time even tho the program is closed 
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void DateAndTime()
        {
            //Start a DispatcherTimer to update the TextBlock every second.
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the TextBlock with the current date and time.
            txtDateTime.Text = DateTime.Now.ToString();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // If there's an ongoing calculation, don't start a new one
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                return;

            if (cancellationTokenSource != null)
                cancellationTokenSource.Dispose();

            cancellationTokenSource = new CancellationTokenSource();
            Location location = new Location();

            txtStatus.Text = "Status: Hired";

            if (txtStartTime.Text == "Start time:")
            {
                string startTime = DateTime.Now.ToString();
                txtStartTime.Text = $"Start time: {startTime}";
            }

            Distance = location.CalculateDistance(PickupLocationLat, PickupLocationLong, DestinationLat, DestinationLong);
            txtDistance.Text = $"Distance travelled: {Distance:0.00} km";

            Duration = taxiRide.CalculateRideDuration(Distance);
            txtDuration.Text = $"Duration: {Duration:0.00} minutes";

            DefaultTariffValue();
            Price = taxiRide.CalculatePrice(Distance, DistanceRate, Duration, DurationRate);
            double totalPrice = Price;
            txtTotalPrice.Text = $"Total Price: {totalPrice:0.00} EUR";

            await LiveTaxiFare(TotalLiveTaxiPrice, Distance, cancellationTokenSource.Token);
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            txtStatus.Text = "Status: Processing Payment";

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.RowDefinitions[2].Height = new GridLength(200);
            txtStatus.Text = "Status: For Hire";

            if (txtEndTime.Text == "End time:")
            {
                string endTime = DateTime.Now.ToString();
                txtEndTime.Text = $"End time: {endTime}";
            }
        }

        private void btnTariff_Click(object sender, RoutedEventArgs e)
        {
            if (txtTariff.Text == "Tariff" || txtTariff.Text == "Tariff 3")
            {
                txtTariff.Text = "Tariff 1";
                DistanceRate = 1.09;
                DurationRate = 0.283;
            }
            else if (txtTariff.Text == "Tariff 1")
            {
                txtTariff.Text = "Tariff 2";
                DistanceRate = 1.80;
                DurationRate = 0.40;
            }
            else if (txtTariff.Text == "Tariff 2")
            {
                txtTariff.Text = "Tariff 3";
                DistanceRate = 2.00;
                DurationRate = 0.50;
            }
        }

        private void btnRideInfo_Click(object sender, RoutedEventArgs e)
        {
            Location popup = new Location();
            popup.ShowDialog();

            PickupLocationLat = popup.PickupLocationLatitude;
            PickupLocationLong = popup.PickupLocationLongitude;

            DestinationLat = popup.DestinationLatitude;
            DestinationLong = popup.DestinationLongitude;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.RowDefinitions[2].Height = new GridLength(0);
            ResetTaximeter();
        }

        private void ResetTaximeter()
        {
            txtStartTime.Text = "Start time:";
            txtEndTime.Text = "End time:";
            txtPrice.Text = "Price: 0 EUR";
            cancellationTokenSource = null;
            TotalLiveTaxiPrice = 0;
            PickupLocationLat = 0;
            PickupLocationLong = 0;
            DestinationLat = 0;
            DestinationLong = 0;
            Distance = 0;
            Duration = 0;
            Price = 0;
        }

        private async Task LiveTaxiFare(double startingLiveTaxiPrice, double distance, CancellationToken cancellationToken)
        {
            double liveTaxiPrice = startingLiveTaxiPrice;
            double tenthOfDistance = distance / 10;
            double tenthOfLiveTaxiPrice = tenthOfDistance * DistanceRate;

            while (liveTaxiPrice <= Price)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                liveTaxiPrice += tenthOfLiveTaxiPrice;

                if (liveTaxiPrice > Price)
                {
                    txtPrice.Text = $"Price: {Price:0.00} EUR";
                    break;
                }
                txtPrice.Text = $"Price: {liveTaxiPrice:0.00} EUR";

                await Task.Delay(1000);
            }

            TotalLiveTaxiPrice += liveTaxiPrice;
        }

        private void DefaultTariffValue()
        {
            if (DistanceRate == 0 || DurationRate == 0)
            {
                txtTariff.Text = "Tariff 1";
                DistanceRate = 1.09;
                DurationRate = 0.283;
            }
        }
    }
}