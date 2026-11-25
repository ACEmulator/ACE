using System;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace ACE.Server
{
    partial class Program
    {
        private static void CheckForServerUpdate()
        {
            log.LogInformation("Automatic Server version check started...");
            try
            {
                var worldDb = new Database.WorldDatabase();
                var currentVersion = worldDb.GetVersion();
                log.LogInformation("Current Server Binary: {FullVersion}", ServerBuildInfo.FullVersion);

                var url = "https://api.github.com/repos/ACEmulator/ACE/releases/latest";
                using var client = new WebClient();
                var html = client.GetStringFromURL(url).Result;

                var json = JsonSerializer.Deserialize<JsonElement>(html);

                string tag = json.GetProperty("tag_name").GetString();
               
                //Split the tag from "v{version}.{build}" into discrete components  - "tag_name": "v1.39.4192"
                Version v = new Version(tag.Remove(0, 1));
                Version currentServerVersion = ServerBuildInfo.GetServerVersion();

                var versionStatus = v.CompareTo(currentServerVersion);
                // Status returns > 0 if the GitHub version is newer. (0 if the same, or < 0 if older.)
                if (versionStatus > 0)
                {
                    log.LogWarning("There is a newer version of ACE available!");
                    log.LogWarning("Please visit {String} for more information.", json.GetProperty("html_url").GetString());

                    // the Console.Title.Get() only works on Windows...
                    #pragma warning disable CA1416 // Validate platform compatibility
                    Console.Title += " -- Server Binary Update Available";
                    #pragma warning restore CA1416 // Validate platform compatibility
                }
                else
                {
                    log.LogInformation("Latest Server Version is {Tag} -- No Update Required!", tag);
                }
                return;
            }
            catch (Exception ex)
            {
                log.LogInformation("Unable to continue with Automatic Server Version Check due to the following error: {Ex}", ex);
            }
        }
    }
}
