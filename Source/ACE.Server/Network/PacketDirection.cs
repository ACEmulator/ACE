// UTF-8 BOM removed to ensure consistent encoding
namespace ACE.Server.Network
{
    public enum PacketDirection
    {
        None,

        /// <summary>
        /// Client->Server
        /// </summary>
        Client,

        /// <summary>
        /// Server->Client
        /// </summary>
        Server
    }
}