using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiMeterUI
{
    public class TaxiRide
    {
        private const double AverageSpeedPerHour = 30;

        public double CalculateRideDuration(double distance)
        {
            double durationInHours = distance / AverageSpeedPerHour;
            double durationInMinutes = durationInHours * 60;
            return durationInMinutes;
        }

        public double CalculatePrice(double distance, double distanceRate, double duration, double durationRate)
        {
            double totalDistancePrice = distance * distanceRate;
            double totalDurationPrice = duration * durationRate;
            return totalDistancePrice + totalDurationPrice;
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
