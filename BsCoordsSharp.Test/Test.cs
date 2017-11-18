using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BsCoordsSharp.Test
{
    [TestClass]
    public class Test
    {
        private const int Mcc = 250;
        private const int Mnc = 99;
        private const int Lac = 65008;
        private const int Cid = 40507;
        
        private const double Latitude = 55.8;
        private const double Longitude = 37.5;
        private const double Threshold = 1;

        private const string MozillaKey = ""; // Replace with api key
        private const string OpenCellIdKey = ""; // Replace with api key

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task GoogleTest()
            => await TestClient(new BsGoogleClient());

        [TestMethod]
        public async Task MozillaTest()
            => await TestClient(new BsMozillaClient(MozillaKey));

        [TestMethod]
        public async Task MylnikovTest()
            => await TestClient(new BsMylnikovClient());

        [TestMethod]
        public async Task OpenCellIdTest()
            => await TestClient(new BsOpenCellIdClient(OpenCellIdKey));

        [TestMethod]
        public async Task YandexTest()
            => await TestClient(new BsYandexClient());

        private async Task TestClient(IBsClient client)
        {
            var geo = await client.RequestAsync(Mcc, Mnc, Lac, Cid);
            Assert.IsNotNull(geo);
            TestContext.WriteLine($"{client.GetType().Name}: {geo}");
            Assert.IsTrue(Math.Abs(Latitude - geo.Latitude) < Threshold);
            Assert.IsTrue(Math.Abs(Longitude - geo.Longitude) < Threshold);
        }
    }
}