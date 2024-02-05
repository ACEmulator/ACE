namespace ACE.Common
{
    /// <summary>
    /// This section configures handling of client DAT patching from server's DAT files
    /// </summary>
    public class DDDConfiguration
    {
        /// <summary>
        /// Allow server to patch client DAT files using server's DAT files via DDDManager
        /// </summary>
        public bool EnableDATPatching { get; set; } = false;

        /// <summary>
        /// Upon server startup, precache all DAT files that would be sent as compressed data
        /// </summary>
        public bool PrecacheCompressedDATFiles { get; set; } = false;
    }
}
