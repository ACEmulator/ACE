using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Common;
using ACE.Server.Api;

namespace ACE.Server.Tests
{
    [TestClass]
    public class StatusApiTests
    {
        private static HttpClient _http = new HttpClient();
        private const string BaseUrl = "http://127.0.0.1:5000";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var config = new MasterConfiguration
            {
                Server = new GameConfiguration
                {
                    Api = new ApiSettings { Enabled = true, Host = "127.0.0.1", Port = 5000, RequestsPerMinute = 0, CacheSeconds = 5 }
                }
            };

            ConfigManager.Initialize(config);
            StatusApiServer.Start();
            Thread.Sleep(500); // allow server to start
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            StatusApiServer.StopAsync().GetAwaiter().GetResult();
        }

        [TestMethod]
        public async Task StatusEndpoint_ReturnsOkAndJson()
        {
            var response = await _http.GetAsync(BaseUrl + "/api/status");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.IsTrue(json.TryGetProperty("uptimeSeconds", out _));
            Assert.IsTrue(json.TryGetProperty("version", out _));
            Assert.IsTrue(json.TryGetProperty("startTime", out _));
        }

        [TestMethod]
        public async Task StatsPlayers_ReturnsOkAndJson()
        {
            var response = await _http.GetAsync(BaseUrl + "/api/stats/players");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.IsTrue(json.TryGetProperty("onlineCount", out _));
            Assert.IsTrue(json.TryGetProperty("players", out _));
        }

        [TestMethod]
        public async Task StatsCharacter_ReturnsNotFound()
        {
            var response = await _http.GetAsync(BaseUrl + "/api/stats/character/unknown");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task StatsPerformance_ReturnsOkAndJson()
        {
            var response = await _http.GetAsync(BaseUrl + "/api/stats/performance");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.IsTrue(json.TryGetProperty("uptimeSeconds", out _));
            Assert.IsTrue(json.TryGetProperty("cpuUsagePercent", out _));
            Assert.IsTrue(json.TryGetProperty("privateMemoryMB", out _));
            Assert.IsTrue(json.TryGetProperty("gcMemoryMB", out _));
        }

        [TestMethod]
        public async Task StatusEndpoint_CachesWithinTtl()
        {
            var json1 = await _http.GetFromJsonAsync<JsonElement>(BaseUrl + "/api/status");
            var uptime1 = json1.GetProperty("uptimeSeconds").GetDouble();
            await Task.Delay(500);
            var json2 = await _http.GetFromJsonAsync<JsonElement>(BaseUrl + "/api/status");
            var uptime2 = json2.GetProperty("uptimeSeconds").GetDouble();
            Assert.AreEqual(uptime1, uptime2);
        }

        [TestMethod]
        public async Task Responses_IncludeCorsHeader()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "/api/status");
            request.Headers.Add("Origin", "http://example.com");
            var response = await _http.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.Headers.Contains("Access-Control-Allow-Origin"));
        }
    }
}
