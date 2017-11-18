using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BsCoordsSharp
{
    public class BsOpenCellIdClient : BsClientBase
    {
        private const string Hostname = "http://opencellid.org";
        private const string Path = "/cell/get?key={4}&mnc={1}&mcc={0}&lac={2}&cellid={3}";

        private string _apiKey;

        public BsOpenCellIdClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        protected override async Task<string> GetResponseAsync(int mcc, int mnc, int lac, int cid)
        {
            using (var http = new HttpClient { BaseAddress = new Uri(Hostname) })
            {
                var path = string.Format(Path, mcc, mnc, lac, cid, _apiKey);
                return await http.GetStringAsync(path);
            }
        }

        protected override GeoPosition Parse(string data)
        {
            var xdocument = XDocument.Parse(data);
            var xcell = xdocument.Descendants("cell").FirstOrDefault();
            if (xcell == null) return null;
            return new GeoPosition(
                (double)xcell.Attribute("lat"),
                (double)xcell.Attribute("lon"));
        }
    }
}