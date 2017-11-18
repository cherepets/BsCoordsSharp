using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BsCoordsSharp
{
    public class BsYandexClient : IBsClient
    {
        private const string Hostname = "http://mobile.maps.yandex.net";
        private const string Path = "/cellid_location/?&cellid={3}&operatorid={1}&countrycode={0}&lac={2}";
        
        public async Task<YandexGeoPosition> RequestAsync(int mcc, int mnc, int lac, int cid)
        {
            var data = await GetResponseAsync(mcc, mnc, lac, cid);
            if (data == null) return null;
            return Parse(data);
        }

        protected async Task<string> GetResponseAsync(int mcc, int mnc, int lac, int cid)
        {
            using (var http = new HttpClient { BaseAddress = new Uri(Hostname) })
            {
                var path = string.Format(Path, mcc, mnc, lac, cid);
                try
                {
                    return await http.GetStringAsync(path);
                }
                catch (HttpRequestException exception) when (exception.Message.Contains("404"))
                {
                    return null;
                }
            }
        }

        protected YandexGeoPosition Parse(string data)
        {
            var xdocument = XDocument.Parse(data);
            var xcoordinates = xdocument.Descendants("coordinates").FirstOrDefault();
            if (xcoordinates == null) return null;
            return new YandexGeoPosition(
                (double)xcoordinates.Attribute("latitude"),
                (double)xcoordinates.Attribute("longitude"),
                (double)xcoordinates.Attribute("nlatitude"),
                (double)xcoordinates.Attribute("nlongitude"));
        }

        async Task<GeoPosition> IBsClient.RequestAsync(int mcc, int mnc, int lac, int cid)
            => await RequestAsync(mcc, mnc, lac, cid);
    }
}