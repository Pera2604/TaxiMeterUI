using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Threading.Tasks;

namespace TaxiMeterUI
{
    // Insta Requests

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainGrid.RowDefinitions[2].Height = new GridLength(0);
            //DateAndTime();
        }
        private CancellationTokenSource cancellationTokenSource;

        private const double averageSpeedPerHour = 30;

        private double distanceRate;

        private double durationRate;

        private double pickupLocationLat;

        private double pickupLocationLong;

        private double destinationLat;

        private double destinationLong;

        private double distance;

        private double duration;

        private double price;

        public double DistanceRate
        {
            get { return distanceRate; }
            set { distanceRate = value; }
        }
        public double DurationRate
        {
            get { return durationRate; }
            set { durationRate = value; }
        }
        public double PickupLocationLat
        {
            get { return pickupLocationLat; }
            set { pickupLocationLat = value; }
        }
        public double PickupLocationLong
        {
            get { return pickupLocationLong; }
            set { pickupLocationLong = value; }
        }
        public double DestinationLat
        {
            get { return destinationLat; }
            set { destinationLat = value; }
        }
        public double DestinationLong
        {
            get { return destinationLong; }
            set { destinationLong = value; }
        }
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public double Price
        {
            get { return price; }
            set {  price = value; }
        }

        //private void DateAndTime()
        //{
        //    //Start a DispatcherTimer to update the TextBlock every second.
        //    DispatcherTimer timer = new DispatcherTimer();
        //    timer.Interval = TimeSpan.FromSeconds(1);
        //    timer.Tick += Timer_Tick;
        //    timer.Start();
        //}

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    // Update the TextBlock with the current date and time.
        //    txtDateTime.Text = DateTime.Now.ToString();
        //} Osrm

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            cancellationTokenSource = new CancellationTokenSource();
            Location location = new Location();

            txtStatus.Text = "Status: Hired";

            string startTime = DateTime.Now.ToString();
            txtStartTime.Text = $"Start time: {startTime}";

            Distance = location.CalculateDistance(PickupLocationLat, PickupLocationLong, DestinationLat, DestinationLong);
            txtDistance.Text = $"Distance travelled: {Distance:0.00} km";

            Duration = CalculateRideDuration();
            txtDuration.Text = $"Duration: {Duration:0.00} minutes";

            DefaultTariffValue();
            Price = CalculatePrice();
            double totalPrice = Price;
            txtTotalPrice.Text = $"Total Price: {totalPrice:0.00} EUR";

            await LiveTaxiFare(Distance, cancellationTokenSource.Token);
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
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

            string endTime = DateTime.Now.ToString();
            txtEndTime.Text = $"End time: {endTime}";
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

        private double CalculateRideDuration()
        {
            double durationInHours = Distance / averageSpeedPerHour;

            double durationInMinutes = durationInHours * 60;

            return durationInMinutes;
        }

        private double CalculatePrice()
        { 
            double totalDistancePrice = Distance * distanceRate;

            double totalDurationPrice = Duration * durationRate;

            return totalDistancePrice + totalDurationPrice;
        }

        // veriable probably better than Property in this case ( value never resets for property just keeps on adding on to it)
        private async Task LiveTaxiFare(double distance, CancellationToken cancellationToken)
        {
            double liveTaxiPrice;
            double liveDistance = 0;
            double tenthOfDistance = distance / 10;

            for (int i = 1; i <= 10; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                liveDistance += tenthOfDistance;
                liveTaxiPrice = liveDistance * distanceRate;

                txtPrice.Text = $"Price: {liveTaxiPrice:0.00} EUR";

                await Task.Delay(1000);
            }
        }

        private void DefaultTariffValue()
        {
            if( distanceRate == 0 || durationRate == 0 ) 
            {
                txtTariff.Text = "Tariff 1";
                DistanceRate = 1.09;
                DurationRate = 0.283;
            }
        }

        // Average Speed Logic Problem - .txt file

        //private double CalculateAverageSpeed()
        //{
        //    DateTime startDateTime = new DateTime();
        //    DateTime endDateTime = new DateTime();

        //    startDateTime = DateTime.ParseExact(txtStartTime.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        //    endDateTime = DateTime.ParseExact(txtEndTime.Text,"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        //    Duration = (endDateTime - startDateTime).TotalSeconds;
        //    txtDuration.Text = $"Duration: {Duration} seconds";

        //    double averageSpeed = Distance / Duration;
        //    txtAverageSpeed.Text = $"Average Speed: {averageSpeed} km/s";

        //    return averageSpeed;
        //}
    }
}