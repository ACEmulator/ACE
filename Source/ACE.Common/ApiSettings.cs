using System;

namespace ACE.Common
{
    public class ApiSettings
    {
        public bool Enabled { get; set; } = false;
        public string Host { get; set; } = "0.0.0.0";
        public uint Port { get; set; } = 5000;
        public bool UseHttps { get; set; } = false;
        public string CertificatePath { get; set; } = string.Empty;
        public string CertificatePassword { get; set; } = string.Empty;

        /// <summary>
        /// Maximum number of requests a single IP address may send to the API per minute.
        /// Set to 0 to disable rate limiting.
        /// </summary>
        public uint RequestsPerMinute { get; set; } = 60;

        /// <summary>
        /// When true, all API requests must include a valid API key in the
        /// <c>X-API-Key</c> header or <c>apikey</c> query string parameter.
        /// </summary>
        public bool RequireApiKey { get; set; } = false;

        /// <summary>
        /// List of valid API keys that clients may use when
        /// <see cref="RequireApiKey"/> is enabled.
        /// </summary>
        public string[] ApiKeys { get; set; } = Array.Empty<string>();
    }
}
