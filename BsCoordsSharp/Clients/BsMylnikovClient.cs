using BsCoordsSharp.Extesions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BsCoordsSharp
{
    public class BsMylnikovClient : BsClientBase
    {
        private const string Hostname = "http://api.mylnikov.org";
        private const string Path = "/geolocation/cell?v=1.1&data=open&key={4}&cellid={3}&mnc={1}&mcc={0}&lac={2}";
        private const string Key = "imGAr7GPI5017U8tjpRQQs8lb17vtZT7";

        protected override async Task<string> GetResponseAsync(int mcc, int mnc, int lac, int cid)
        {
            using (var http = new HttpClient { BaseAddress = new Uri(Hostname) })
            {
                var path = string.Format(Path, mcc, mnc, lac, cid, Key);
                return await http.GetStringAsync(path, Encoding.UTF8);
            }
        }

        protected override GeoPosition Parse(string data)
        {
            var body = data.FromJson<ResponseBody>();
            if (body.Result != 200 || body.Data == null)
                return null;
            return new GeoPosition(body.Data.Lat, body.Data.Lon);
        }

        #region Response
        class ResponseBody
        {
            public int Result { get; set; }
            public DataBody Data { get; set; }
        }

        class DataBody
        {
            public double Lat { get; set; }
            public double Lon { get; set; }
            public double Range { get; set; }
            public long Time { get; set; }
        }
        #endregion
    }
}