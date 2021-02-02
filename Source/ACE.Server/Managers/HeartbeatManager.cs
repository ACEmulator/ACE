using System;

using log4net;

using ACE.Common.Performance;
using System.Net.Http;
using System.Text;
using ACE.Common;
using Newtonsoft.Json;

namespace ACE.Server.Managers
{
    public static class HeartbeatManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The rate at which HeartbeatManager.Tick() executes
        /// </summary>
        private static readonly RateLimiter updateHeartbeatManagerRateLimiter = new RateLimiter(1, TimeSpan.FromSeconds(10));

        private static Uri heartbeatUri;

        public static void Initialize()
        {

            // TODO: Factor out into config
            heartbeatUri = new Uri("https://servers.treestats.net/heartbeat");
        }

        /// <summary>
        /// One-off class to aid in serializing the heartbeat's JSON payload
        /// </summary>
        private class HeartbeatBody
        {
            public string server;
            public int online;
            public bool pk_server;
            public double xp_modifier;
        }

        /// <summary>
        /// Runs every ~1 minute
        /// </summary>
        public static void Tick()
        {

            if (updateHeartbeatManagerRateLimiter.GetSecondsToWaitBeforeNextEvent() > 0)
                return;

            log.Debug($"HeartbeatManager.Tick()");

            updateHeartbeatManagerRateLimiter.RegisterEvent();
            DoHeartbeat();
        }

        public static void DoHeartbeat()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;

                try
                {
                    HeartbeatBody body = new HeartbeatBody
                    {
                        online = PlayerManager.GetOnlineCount(),
                        server = ConfigManager.Config.Server.WorldName,
                        xp_modifier = PropertyManager.GetDouble("xp_modifier").Item,
                        pk_server = PropertyManager.GetBool("pk_server").Item
                    };

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(body));
                    response = client.PostAsync(heartbeatUri, content).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        log.Debug("Heartbeat Failed: " + response.Content);
                    }
                }
                catch (Exception e)
                {
                    log.Debug("Exception: " + e.Message);
                }
            }
        }
    }
}
