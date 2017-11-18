using System.Linq;

namespace BsCoordsSharp
{
    public class YandexGeoPosition : GeoPosition
    {
        public GeoPosition Cell { get; }
        public GeoPosition Bs { get; }

        public YandexGeoPosition(double cellLatitude, double cellLongitude, double bsLatitude, double bsLongitude) 
            : base(Avg(cellLatitude, bsLatitude), Avg(cellLongitude, bsLongitude))
        {
            Cell = new GeoPosition(cellLatitude, cellLongitude);
            Bs = new GeoPosition(bsLatitude, bsLongitude);
        }

        private static double Avg(params double[] numbers)
            => numbers.Average();

        public override string ToString()
            => $"{Cell}|{Bs}";
    }
}