using BsCoordsSharp.Extesions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BsCoordsSharp
{
    public class BsMozillaClient : BsClientBase
    {
        private const string Hostname = "http://location.services.mozilla.com";
        private const string Path = "/v1/search?key={0}";
        private const string NetworkType = "gsm";

        private const string TestKey = "test";

        private string _key;

        public BsMozillaClient(string key = null)
        {
            _key = key;
        }

        public async Task<GeoPosition> RequestAsync(int mcc, int mnc, int lac, int cid, string networkType)
        {
            var data = await GetResponseAsync(mcc, mnc, lac, cid, networkType);
            if (data == null) return null;
            return Parse(data);
        }

        protected override async Task<string> GetResponseAsync(int mcc, int mnc, int lac, int cid)
            => await GetResponseAsync(mcc, mnc, lac, cid, NetworkType);

        private async Task<string> GetResponseAsync(int mcc, int mnc, int lac, int cid, string networkType)
        {
            using (var http = new HttpClient { BaseAddress = new Uri(Hostname) })
            {
                var path = string.Format(Path, _key ?? TestKey);
                var requestBody = new RequestBody
                {
                    Cell = new[]
                    {
                        new CellBody
                        {
                            Radio = networkType,
                            Cid = cid,
                            Lac = lac,
                            Mcc = mcc,
                            Mnc = mnc
                        }
                    }
                }.ToJson();
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(path, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        protected override GeoPosition Parse(string data)
        {
            var body = data.FromJson<ResponseBody>();
            if (body.Status?.ToLowerInvariant() != "ok")
                return null;
            return new GeoPosition(body.Lat, body.Lon);
        }

        #region Request
        class RequestBody
        {
            public IEnumerable<CellBody> Cell { get; set; }
        }

        class CellBody
        {
            public string Radio { get; set; }
            public int Cid { get; set; }
            public int Lac { get; set; }
            public int Mcc { get; set; }
            public int Mnc { get; set; }
        }
        #endregion

        #region Response
        class ResponseBody
        {
            public string Status { get; set; }
            public double Lat { get; set; }
            public double Lon { get; set; }
            public double Accuracy { get; set; }
        }
        #endregion
    }
}