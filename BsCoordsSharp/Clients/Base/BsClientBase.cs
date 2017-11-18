using System.Threading.Tasks;

namespace BsCoordsSharp
{
    public abstract class BsClientBase : IBsClient
    {
        public async Task<GeoPosition> RequestAsync(int mcc, int mnc, int lac, int cid)
        {
            var data = await GetResponseAsync(mcc, mnc, lac, cid);
            if (data == null) return null;
            return Parse(data);
        }

        protected abstract Task<string> GetResponseAsync(int mcc, int mnc, int lac, int cid);
        protected abstract GeoPosition Parse(string data);
    }
}