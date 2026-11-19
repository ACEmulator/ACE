
namespace ACE.Common
{
    public class NetworkSettings
    {
        /// <summary>
        /// IP Address for World to listen on. 
        /// </summary>
        /// <remarks>
        /// In just about all cases, you will not need to change this setting from the default. Special networking conditions, or multiple network adapters would possibly be examples of times where you might need to change this.
        /// </remarks>
        public string Host { get; set; } = "0.0.0.0";

        /// <summary>
        /// Port for World to listen on. This also opens the next port up, 9001, for server to client communications. When changed, it will open the port specified and +1 of that port.
        /// </summary>
        /// <remarks>
        /// For firewalls, you would need to include opening both udp ports (9000 and 9001) for communications to work correctly.
        /// </remarks>
        public uint Port { get; set; } = 9000;

        /// <summary>
        /// Increasing this setting will allow more Accounts to connect with this server.
        /// </summary>
        /// <remarks>
        /// WARNING: Must be greater then 0 to allow users to connect.
        /// </remarks>
        public uint MaximumAllowedSessions { get; set; } = 128;

        /// <summary>
        /// The amount of seconds until an active session will be declared dead/inactive. Default value is 60 (for 1 minute).
        /// </summary>
        public uint DefaultSessionTimeout { get; set; } = 60;

        /// <summary>
        /// This setting will allow or restrict sessions based on the IPAddress connecting to the server.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public int MaximumAllowedSessionsPerIPAddress { get; set; } = -1;

        /// <summary>
        /// Will allow the given IP addresses to have unlimited sessions - recommend only use this for Admins
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string[] AllowUnlimitedSessionsFromIPAddresses { get; set; } = { };

        /// <summary>
        /// IP addresses of trusted UDP proxies (Cloudflare Spectrum, DDoS protection, relays) that prepend a Simple Proxy Protocol header to every datagram.
        /// </summary>
        /// <remarks>
        /// Datagrams from these addresses must start with a Simple Proxy Protocol header, otherwise they are dropped.
        /// The client address in the header is used as the session's IP address for MaximumAllowedSessionsPerIPAddress, AllowUnlimitedSessionsFromIPAddresses, account IP addresses and admin displays.
        /// Packets sent to these addresses are prepended with the header the proxy sent.
        /// Datagrams from any other address are processed as normal. Leave empty to disable.
        /// Reference: https://developers.cloudflare.com/spectrum/reference/simple-proxy-protocol-header/
        /// </remarks>
        public string[] TrustedProxyAddresses { get; set; } = { };
    }
}
