using System;

using log4net;

using ACE.Common.Performance;
using System.Net.Http;
using System.Text;
using ACE.Common;
using Newtonsoft.Json;
using System.Threading;

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
            if (!ConfigManager.Config.Server.Heartbeat.Enabled)
            {
                log.Debug("Not starting HeartbeatManager because it's disabled in config");

                return;
            }

            endpoint = new Uri(ConfigManager.Config.Server.Heartbeat.Endpoint);
            updateHeartbeatManagerRateLimiter = new RateLimiter(1, TimeSpan.FromSeconds(ConfigManager.Config.Server.Heartbeat.Interval));
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

            Thread mythread = new Thread(DoHeartbeat);
            mythread.Start();
        }

        public static void DoHeartbeat()
        {
            log.Debug($"HeartbeatManager.DoHeartbeat Called");

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
                    response.EnsureSuccessStatusCode();

                    if (!response.IsSuccessStatusCode)
                    {
                        log.Debug($"Heartbeat request failed: {response.Content}");
                    }
                }
                catch (HttpRequestException e)
                {
                    log.Debug($"HttpRequestException while sending Heartbeat {e.Message}");
                }
                catch (Exception e)
                {
                    log.Debug($"Exception while sending Heartbeat: {e.Message}");
                }
            }
        }
    }
}
