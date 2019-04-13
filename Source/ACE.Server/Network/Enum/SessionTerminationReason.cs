namespace ACE.Server.Network.Enum
{
    public enum SessionTerminationReason
    {
        None,
        PacketHeaderDisconnect,
        AccountSelectCallbackException,
        NetworkTimeout,
        /// <summary>
        /// a trusted packet had PacketHeaderFlags.NetErrorDisconnect header flag
        /// </summary>
        ClientSentNetworkErrorDisconnect,
        AccountBooted,
        BadHandshake,
        PongSentClosingConnection,
        NotAuthorizedNoPasswordOrGlsTicketIncludedInLoginReq,
        NotAuthorizedAccountNotFound,
        AccountInUse,
        NotAuthorizedPasswordMismatch,
        NotAuthorizedGlsTicketNotImplementedToProcLoginReq,
        /// <summary>
        /// The client connection is no longer able to send us packets with encrypted CRC
        /// </summary>
        ClientConnectionFailure,
        SendToSocketException
    }
}
