using System.Diagnostics.Eventing.Reader;
using System.Windows;

namespace TaxiMeterUI
{
    public partial class Location : Window
    {
        public int PickupLocationLatitude { get; private set; }
        public int PickupLocationLongitude { get; private set; }
        public int DestinationLatitude { get; private set; }
        public int DestinationLongitude { get; private set; }

        public Location()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbPickupLocationLat.Text, out int first))
            {
                if ((first >= -90 && first <= 90))
                {
                    PickupLocationLatitude = first;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Invalid input. Latitude ranges from -90 to 90 degrees, please try again..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (int.TryParse(tbPickupLocationLong.Text, out int second))
            {
                if (((second >= -180 && second <= 180)))
                {
                    PickupLocationLongitude = second;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Invalid input. Longitude ranges from -180 to 180 degrees, please try again..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (int.TryParse(tbDestinationLat.Text, out int third))
            {
                if ((third >= -90 && third <= 90))
                {
                    DestinationLatitude = third;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Invalid input. Latitude ranges from -90 to 90 degrees, please try again..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if ( int.TryParse(tbDestinationLong.Text, out int fourth))
            {
                if ((fourth >= -180 && fourth <= 180))
                {
                    DestinationLongitude = fourth;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Invalid input. Longitude ranges from -180 to 180 degrees, please try again..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter valid integers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if ( PickupLocationLatitude != null && PickupLocationLongitude != null && DestinationLatitude != null && DestinationLongitude != null)
            {
                Close();
            }
        }
    }
}