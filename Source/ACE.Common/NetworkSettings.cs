
namespace ACE.Common
{
    public class NetworkSettings
    {
        public string Host { get; set; } = "127.0.0.1";

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
    }
}
