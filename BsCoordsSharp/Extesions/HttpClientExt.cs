using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BsCoordsSharp.Extesions
{
    public static class HttpClientExt
    {
        public static async Task<string> GetStringAsync(this HttpClient http, string requestUri, Encoding encoding)
        {
            var response = await http.GetAsync(requestUri);
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return Encoding.UTF8.GetString(bytes);
        }
    }
}