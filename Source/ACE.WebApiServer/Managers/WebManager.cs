using ACE.Common;
using log4net;
using System;
using System.Net;

namespace ACE.WebApiServer.Managers
{
    /// <summary>
    /// Web Manager handles the web host which honors HTTP and HTTPS external web requests
    /// </summary>
    public static class WebManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// start the web host
        /// </summary>
        public static void Initialize()
        {
            var gate = Gate.Instance; // unnecessarily spin up gate threads in preparation for web requests
            string listeningHost = ConfigManager.Config.WebApi.Host;
            int listeningPort = (int)ConfigManager.Config.WebApi.Port;
            log.Info($"Binding web host to {listeningHost}:{listeningPort}");
            try
            {
                if (!IPAddress.TryParse(listeningHost, out IPAddress listenAt))
                {
                    log.Error($"Unable to parse IP address {listeningHost}");
                    return;
                }
                WebHost.Run(listenAt, listeningPort);
            }
            catch (Exception exception)
            {
                log.FatalFormat("Web host has thrown: {0}", exception.Message);
            }
        }

        /// <summary>
        /// stop the web host, grace lasts for 1 second, then remaining requests are jettisoned.
        /// </summary>
        public static void Shutdown()
        {
            Gate.Shutdown();
            WebHost.Shutdown();
        }
    }
}
