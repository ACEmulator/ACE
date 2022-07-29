using ACE.Database;

using System;

namespace ACE.Server
{
    public static partial class ServerBuildInfo
    {
        public static string CommitID => Commit.Substring(0, 7);

        public static DateTime CompilationTimestampUtc => new DateTime(BuildYear, BuildMonth, BuildDay, BuildHour, BuildMinute, BuildSecond, DateTimeKind.Utc);

        public static string FullVersion = $"{Version}.{Build}.{CompilationTimestampUtc:yyyyMMddHHmmss}-{Branch}-{CommitID}";

        public static string GetVersionInfo()
        {
            var databaseVersion = DatabaseManager.World.GetVersion();

            var msg = $"Server binaries version {FullVersion} - compiled {CompilationTimestampUtc:ddd MMM d HH:mm:ss yyyy} : ACEmulator\n";

            msg += $"Server database version Base: {databaseVersion.BaseVersion} Patch: {databaseVersion.PatchVersion} - compiled {databaseVersion.LastModified:ddd MMM d HH:mm:ss yyyy}\n";

#if DEBUG
            msg += "Server is compiled in DEBUG mode\n";
#else
            msg += "Server is compiled in RELEASE mode\n";
#endif

            if (Program.IsRunningInContainer)
                msg += "Server is running inside a Container\n";
            return msg;
        }

        public static Version GetServerVersion()
        {
            return new Version(Version + "." + Build);
        }

    }
}
