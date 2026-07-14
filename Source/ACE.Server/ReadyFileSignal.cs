using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

using ACE.Common;

namespace ACE.Server
{
    public sealed class ReadyFileSignal
    {
        private readonly string path;

        public ReadyFileSignal(string path)
        {
            this.path = path;
        }

        public bool Enabled => !string.IsNullOrWhiteSpace(path);

        public void DeleteStale()
        {
            if (Enabled && File.Exists(path))
                File.Delete(path);
        }

        public void Write()
        {
            if (!Enabled)
                return;

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            var payload = new
            {
                ProcessId = Process.GetCurrentProcess().Id,
                WorldName = ConfigManager.Config.Server.WorldName,
                Host = ConfigManager.Config.Server.Network.Host,
                Port = ConfigManager.Config.Server.Network.Port,
                ReadyAtUtc = DateTime.UtcNow
            };

            var temporaryPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                File.WriteAllText(temporaryPath, JsonSerializer.Serialize(payload));
                File.Move(temporaryPath, path, true);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        public void Delete()
        {
            if (!Enabled)
                return;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // Process shutdown must continue even if cleanup is prevented.
            }
        }
    }
}
