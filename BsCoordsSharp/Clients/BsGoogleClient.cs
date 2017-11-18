using System;
using System.Net;
using System.Threading.Tasks;

namespace BsCoordsSharp
{
    /// <summary>
    /// Based on code this article:
    /// https://brainwashinc.wordpress.com/2009/06/26/cell-phone-my-location-google-mobile-maps/
    /// </summary>
    public class BsGoogleClient : IBsClient
    {
        private const string Hostname = "http://www.google.com";
        private const string Path = "/glm/mmap";

        public async Task<GeoPosition> RequestAsync(int mcc, int mnc, int lac, int cid)
        {
            var requestBody = BuildRequestBody(mcc, mnc, lac, cid);
            var data = await GetResponseAsync(requestBody);
            if (data == null) return null;
            return Parse(data);
        }

        private byte[] BuildRequestBody(int mcc, int mnc, int lac, int cid, bool shortcid = false)
        {
            var body = new byte[] {
                0x00, 0x0e,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,

                0x1b,
                0x00, 0x00, 0x00, 0x00, // Offset 0x11
                0x00, 0x00, 0x00, 0x00, // Offset 0x15
                0x00, 0x00, 0x00, 0x00, // Offset 0x19
                0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, // Offset 0x1f
                0x00, 0x00, 0x00, 0x00, // Offset 0x23
                0x00, 0x00, 0x00, 0x00, // Offset 0x27
                0x00, 0x00, 0x00, 0x00, // Offset 0x2b
                0xff, 0xff, 0xff, 0xff,
                0x00, 0x00, 0x00, 0x00
            };

            if (shortcid)
                cid &= 0xFFFF;      // Attempt to resolve the cell using the GSM CID part

            if ((long)cid > 65536) // GSM: 4 hex digits, UTMS: 6 hex digits
                body[0x1c] = 5;
            else
                body[0x1c] = 3;

            body[0x11] = (byte)((mnc >> 24) & 0xFF);
            body[0x12] = (byte)((mnc >> 16) & 0xFF);
            body[0x13] = (byte)((mnc >> 8) & 0xFF);
            body[0x14] = (byte)((mnc >> 0) & 0xFF);

            body[0x15] = (byte)((mcc >> 24) & 0xFF);
            body[0x16] = (byte)((mcc >> 16) & 0xFF);
            body[0x17] = (byte)((mcc >> 8) & 0xFF);
            body[0x18] = (byte)((mcc >> 0) & 0xFF);

            body[0x27] = (byte)((mnc >> 24) & 0xFF);
            body[0x28] = (byte)((mnc >> 16) & 0xFF);
            body[0x29] = (byte)((mnc >> 8) & 0xFF);
            body[0x2a] = (byte)((mnc >> 0) & 0xFF);

            body[0x2b] = (byte)((mcc >> 24) & 0xFF);
            body[0x2c] = (byte)((mcc >> 16) & 0xFF);
            body[0x2d] = (byte)((mcc >> 8) & 0xFF);
            body[0x2e] = (byte)((mcc >> 0) & 0xFF);

            body[0x1f] = (byte)((cid >> 24) & 0xFF);
            body[0x20] = (byte)((cid >> 16) & 0xFF);
            body[0x21] = (byte)((cid >> 8) & 0xFF);
            body[0x22] = (byte)((cid >> 0) & 0xFF);

            body[0x23] = (byte)((lac >> 24) & 0xFF);
            body[0x24] = (byte)((lac >> 16) & 0xFF);
            body[0x25] = (byte)((lac >> 8) & 0xFF);
            body[0x26] = (byte)((lac >> 0) & 0xFF);

            return body;
        }

        private async Task<byte[]> GetResponseAsync(byte[] requestBody)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(Hostname + Path));
            request.Method = "POST";
            request.ContentLength = requestBody.Length;
            request.ContentType = "application/binary";

            using (var outputStream = await request.GetRequestStreamAsync())
                await outputStream.WriteAsync(requestBody, 0, requestBody.Length);

            var response = (HttpWebResponse)request.GetResponse();
            var responseBytes = new byte[response.ContentLength];
            var totalBytesRead = 0;
            if (response.StatusCode != HttpStatusCode.OK) return null;
            while (totalBytesRead < responseBytes.Length)
            {
                totalBytesRead += await response.GetResponseStream()
                    .ReadAsync(responseBytes, totalBytesRead, responseBytes.Length - totalBytesRead);
            }
            return responseBytes;
        }

        private GeoPosition Parse(byte[] data)
        {
            return ((data[3] << 24) | (data[4] << 16) | (data[5] << 8) | (data[6])) == 0
                ? new GeoPosition(
                    ((double)((data[7] << 24) | (data[8] << 16) | (data[9] << 8) | (data[10]))) / 1000000,
                    ((double)((data[11] << 24) | (data[12] << 16) | (data[13] << 8) | (data[14]))) / 1000000)
                : null;

        }
    }
}