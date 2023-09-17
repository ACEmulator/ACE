using System.Text.Json.Serialization;

namespace ACE.Common
{
    public class MySqlConfiguration
    {
        public string Host { get; set; } = "127.0.0.1";

        public uint Port { get; set; } = 3306;

        public string Database { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        /// <summary>
        /// Common connection options used when connecting to SQL database engine.
        /// </summary>
        [JsonIgnore]
        public string ConnectionOptions { get; } = "DefaultCommandTimeout=120;TreatTinyAsBoolean=False;SslMode=None;AllowPublicKeyRetrieval=True;AllowUserVariables=True;ApplicationName=ACEmulator";
    }
}
