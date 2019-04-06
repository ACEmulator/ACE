namespace ACE.Server.Network.Enum
{
    public enum SessionState
    {
        AuthLoginRequest,
        AuthConnectResponse,
        AuthConnected,
        WorldConnected,
        NetworkTimeout,
        /// <summary>
        /// a trusted packet had PacketHeaderFlags.NetErrorDisconnect header flag
        /// </summary>
        ClientSentNetErrorDisconnect,
        /// <summary>
        /// The client connection is no longer able to send us packets with encrypted CRC
        /// </summary>
        ClientConnectionFailure
    }
}
