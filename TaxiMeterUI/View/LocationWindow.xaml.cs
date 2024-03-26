using System.Windows;

namespace TaxiMeterUI.View
{
    public partial class LocationWindow : Window
    {
        public LocationWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Location location = new Location();

            location.PickupLocationLatitude = location.InputLatitude(tbPickupLocationLat.Text);
            location.DestinationLatitude = location.InputLatitude(tbDestinationLat.Text);

            location.PickupLocationLongitude = location.InputLongitude(tbPickupLocationLong.Text);
            location.DestinationLongitude = location.InputLongitude(tbDestinationLong.Text);

            if (location.PickupLocationLatitude != -1 && location.PickupLocationLongitude != -1 && location.DestinationLatitude != -1 && location.DestinationLongitude != -1)
            {
                Close();
            }
        }
    }
}
