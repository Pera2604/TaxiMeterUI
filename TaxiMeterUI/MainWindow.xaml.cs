using System.Windows;
using System.Windows.Threading;
using TaxiMeterUI.View;

namespace TaxiMeterUI
{
    public partial class MainWindow : Window
    {
        private const double averageSpeedPerHour = 30;

        private DispatcherTimer timer = new DispatcherTimer();

        private CancellationTokenSource cancellationTokenSource;

        private double totalLiveTaxiPrice;

        private double distanceRate;

        private double durationRate;

        private double pickupLocationLat;

        private double pickupLocationLong;

        private double destinationLat;

        private double destinationLong;

        private double distance;

        private double duration;

        private double price;
        public double TotalLiveTaxiPrice
        {
            get { return totalLiveTaxiPrice; }
            set { totalLiveTaxiPrice = value; }
        }
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

        public MainWindow()
        {
            InitializeComponent();
            mainGrid.RowDefinitions[2].Height = new GridLength(0);
            DateAndTime();
            Closing += MainWindow_Closing; // Subscribe to the Closing event
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

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // stops the timer from continuing to calculate the time even tho the program is closed 
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
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

            Duration = CalculateRideDuration();
            txtDuration.Text = $"Duration: {Duration:0.00} minutes";

            DefaultTariffValue();
            Price = CalculatePrice();
            double totalPrice = Price;
            txtTotalPrice.Text = $"Total Price: {totalPrice:0.00} EUR";

            await LiveTaxiFare(totalLiveTaxiPrice, Distance, cancellationTokenSource.Token);
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
            LocationWindow popup = new LocationWindow();
            Location location = new Location();
            popup.ShowDialog();

            PickupLocationLat = location.PickupLocationLatitude;
            PickupLocationLong = location.PickupLocationLongitude;

            DestinationLat = location.DestinationLatitude;
            DestinationLong = location.DestinationLongitude;
        }

        // no worki worki

        //private void LocationCordinates(out double lat1, out double lat2, out double long1, out double long2)
        //{
        //    Location location = new Location();

        //    lat1 = location.PickupLocationLatitude;
        //    long1 = location.PickupLocationLongitude;

        //    lat2 = location.DestinationLatitude;
        //    long2 = location.DestinationLongitude;
        //}

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

        private async Task LiveTaxiFare(double startingLiveTaxiPrice, double distance, CancellationToken cancellationToken)
        {
            double liveTaxiPrice = startingLiveTaxiPrice;
            double tenthOfDistance = distance / 10;
            double tenthOfLiveTaxiPrice = tenthOfDistance * distanceRate;

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
            if( distanceRate == 0 || durationRate == 0 ) 
            {
                txtTariff.Text = "Tariff 1";
                DistanceRate = 1.09;
                DurationRate = 0.283;
            }
        }

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