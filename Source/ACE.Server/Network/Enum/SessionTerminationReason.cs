namespace ACE.Server.Network.Enum
{
    public enum SessionTerminationReason
    {
        None,
        PacketHeaderDisconnect,
        AccountInformationInvalid,
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
        SendToSocketException,
        WorldClosed,
        AbnormalSequenceReceived,
        AccountLoggedIn,
        ServerShuttingDown,
        AccountBanned,
        ClientOutOfDate,
        ForcedLogOffRequested,
        CharacterSaveFailed,
        BiotaSaveFailed
    }
    public static class SessionTerminationReasonHelper
    {
        public static readonly string[] SessionTerminationReasonDescriptions =
        {
            "",
            "PacketHeader Disconnect",
            "Account Information Invalid",
            "AccountSelectCallback threw an exception",
            "Network Timeout",
            "client sent network error disconnect",
            "Account Booted",
            "Bad handshake",
            "Pong sent, closing connection.",
            "Not Authorized: No password or GlsTicket included in login request",
            "Not Authorized: Account Not Found",
            "Account In Use: Found another session already logged in for this account",
            "Not Authorized: Password does not match",
            "Not Authorized: GlsTicket is not implemented to process login request",
            "Client connection failure",
            "MainSocket.SendTo exception occured",
            "World is closed",
            "Client supplied an abnormal sequence",
            "Account was logged in, booting currently connected account in favor of new connection",
            "Server is shutting down",
            "Account is banned",
            "Client is not up to date",
            "Forced log off requested by Admin",
            "Character Save Failed",
            "Biota Save Failed"
        };
        public static string GetDescription(this SessionTerminationReason reason)
        {
            if ((int)reason > SessionTerminationReasonDescriptions.Length - 1)
            {
                return "<reason>";
            }
            return SessionTerminationReasonDescriptions[(int)reason];
        }
    }
}
