using System.Threading.Tasks;

namespace BsCoordsSharp
{
    public interface IBsClient
    {
        Task<GeoPosition> RequestAsync(int mcc, int mnc, int lac, int cid);
    }
}