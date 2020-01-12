namespace ACE.Entity.Enum
{
    /// <summary>
    /// The ChatNetworkBlobType identifies the type of Turbine Chat message.<para />
    /// Used with F7DE: Turbine Chat
    /// </summary>
    public enum ChatNetworkBlobType
    {
        NETBLOB_UNKNOWN = 0,

        /// <summary>
        /// Server -> Client
        /// </summary>
        NETBLOB_EVENT_BINARY = 1,
        NETBLOB_EVENT_XMLRPC = 2,

        /// <summary>
        /// Client -> Server
        /// </summary>
        NETBLOB_REQUEST_BINARY = 3,
        NETBLOB_REQUEST_XMLRPC = 4,

        /// <summary>
        /// Server -> Client
        /// </summary>
        NETBLOB_RESPONSE_BINARY = 5,
        NETBLOB_RESPONSE_XMLRPC = 6,
    }
}
