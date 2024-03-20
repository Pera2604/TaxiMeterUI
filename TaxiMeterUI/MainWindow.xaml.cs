using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace TaxiMeterUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainGrid.RowDefinitions[2].Height = new GridLength(0);
            //DateAndTime();
        }

        private const double avarageSpeedPerHour = 30;

        private double distanceRate;

        private double durationRate;

        private double pickupLocationLat;

        private double pickupLocationLong;

        private double destinationLat;

        private double destinationLong;

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
        //}

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Location location = new Location();

            txtStatus.Text = "Status: Hired";

            string startTime = DateTime.Now.ToString();
            txtStartTime.Text = $"Start Time: {startTime}";

            double distance = location.CalculateDistance(PickupLocationLat, PickupLocationLong, DestinationLat, DestinationLong);
            txtDistance.Text = $"Distance travelled: {distance:0.00} km";
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.RowDefinitions[2].Height = new GridLength(200);
            txtStatus.Text = "Status: For Hire";

            string endTime = DateTime.Now.ToString();
            txtEndTime.Text = $"End Time: {endTime}";
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

        public double CalculateRideDuration(double distance)
        {
            double durationInHours = distance / avarageSpeedPerHour;

            double durationInMinutes = durationInHours * 60;

            return durationInMinutes;
        }
        //private double CalculatePrice()
        //{


        //    return totalDistanceFare + totalDurationFare;
        //}

        //public double CalculateFare(Location pickupLocation, Location destination)
        //{
        //    double distance = pickupLocation.CalculateHaversineDistance(destination);
        //    Console.WriteLine($"Distance traveled: {distance:0.00} km");

        //    double totalDistanceFare = distance * distanceRate;
        //    Console.WriteLine($"Total Distance Fare: {totalDistanceFare:0.00} EUR \n");

        //    double duration = CalculateRideDuration(distance);
        //    Console.WriteLine($"Duration of the ride in minutes: {duration:0.00}");

        //    double totalDurationFare = duration * durationRate;
        //    Console.WriteLine($"Total Duration Fare: {totalDurationFare:0.00} EUR \n");

        //    return totalDistanceFare + totalDurationFare;
        //}
    }
}