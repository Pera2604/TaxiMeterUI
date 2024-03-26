using System.Windows;
using TaxiMeterUI;

namespace TaxiMeterUI
{
    public partial class Location : Window
    {
        public Location()
        {
            InitializeComponent();
        }

        private double pickupLocationLatitude;

        private double pickupLocationLongitude;

        private double destinationLatitude;

        private double destinationLongitude;
        public double PickupLocationLatitude
        {
            get { return pickupLocationLatitude; }
            set { pickupLocationLatitude = value; }
        }
        public double PickupLocationLongitude
        {
            get { return pickupLocationLongitude; }
            set { pickupLocationLongitude = value; }
        }
        public double DestinationLatitude
        {
            get { return destinationLatitude; }
            set { destinationLatitude = value; }
        }
        public double DestinationLongitude
        {
            get { return destinationLongitude; }
            set { destinationLongitude = value; }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            PickupLocationLatitude = InputLatitude(tbPickupLocationLat.Text);
            DestinationLatitude = InputLatitude(tbDestinationLat.Text);

            PickupLocationLongitude = InputLongitude(tbPickupLocationLong.Text);
            DestinationLongitude = InputLongitude(tbDestinationLong.Text);

            if (PickupLocationLatitude != -1 && PickupLocationLongitude != -1 && DestinationLatitude != -1 && DestinationLongitude != -1) 
            {
                Close();
            }
        }

        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371; // Radius of the Earth in kilometers

            // Convert latitude and longitude from degrees to radians
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            // Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance
            double distance = earthRadius * c;

            return distance;
        }

        private static double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        private double InputLatitude(string propertyValue)
        {
            if (double.TryParse(propertyValue, out double result))
            {
                if (result >= -90 && result <= 90)
                {
                    return result;
                }
                else
                {
                    MessageBox.Show("Invalid input. Latitude ranges from -90 to 90 degrees, please try again..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("Please enter valid integers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        private double InputLongitude(string propertyValue)
        {
            if (double.TryParse(propertyValue, out double result))
            {
                if (result >= -180 && result <= 180)
                {
                    return result;
                }
                else
                {
                    MessageBox.Show("Invalid input. Longitude ranges from -180 to 180 degrees, please try again..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("Please enter valid integers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }
    }
}