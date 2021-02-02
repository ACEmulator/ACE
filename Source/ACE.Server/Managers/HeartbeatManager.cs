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
        private static RateLimiter updateHeartbeatManagerRateLimiter;

        /// <summary>
        /// Endpoint to send heartbeats to
        /// </summary>
        private static Uri endpoint;

        public static void Initialize()
        {
            //if (!ConfigManager.Config.Server.Heartbeat.Enabled)
            //{
            //    log.Debug("Not starting HeartbeatManager because it's disabled in config");

            //    return;
            //}

            // endpoint = new Uri(ConfigManager.Config.Server.Heartbeat.Endpoint);
            endpoint = new Uri("https://treestats-servers.herokuapp.com/");

            // updateHeartbeatManagerRateLimiter = new RateLimiter(1, TimeSpan.FromSeconds(ConfigManager.Config.Server.Heartbeat.Interval));
            updateHeartbeatManagerRateLimiter = new RateLimiter(1, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// One-off class to help serialize the heartbeat's JSON payload
        /// </summary>
        private class HeartbeatPayload
        {
            public string WorldName;
            public int OnlineCount;
        }

        /// <summary>
        /// Runs at intervals according to config
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
                    HeartbeatPayload body = new HeartbeatPayload
                    {
                        OnlineCount = PlayerManager.GetOnlineCount(),
                        WorldName = ConfigManager.Config.Server.WorldName
                    };

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(body));
                    response = client.PostAsync(endpoint, content).Result;

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
