namespace ACE.Common
{
    public class WebApiConfiguration
    {
        public bool Enabled { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// Used to form WAN accessible URIs
        /// </summary>
        public string ExternalIPAddressOrDNSName { get; set; }

        /// <summary>
        /// Used to form WAN accessible URIs
        /// </summary>
        public string ExternalPort { get; set; }
    }
}
