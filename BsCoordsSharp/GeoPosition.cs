using System.Globalization;

namespace BsCoordsSharp
{
    public class GeoPosition
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public GeoPosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
            => $"{Latitude.ToString(CultureInfo.InvariantCulture)};{Longitude.ToString(CultureInfo.InvariantCulture)}";
    }
}