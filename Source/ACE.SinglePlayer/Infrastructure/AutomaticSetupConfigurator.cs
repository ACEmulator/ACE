using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Infrastructure;

public static class AutomaticSetupConfigurator
{
    public static bool Configure(LauncherSettings settings, string applicationDirectory, ISecretProtector protector)
    {
        var changed = SettingsPathRepairer.Repair(settings, applicationDirectory);
        if (!BundledDistribution.IsComplete(applicationDirectory))
            return changed;

        changed |= SetIfDifferent(settings.DatabaseMode, DatabaseMode.Private, value => settings.DatabaseMode = value);
        changed |= SetIfDifferent(settings.DatabaseHost, "127.0.0.1", value => settings.DatabaseHost = value);
        changed |= SetIfDifferent(settings.DatabaseUsername, "ace_singleplayer", value => settings.DatabaseUsername = value);

        if (settings.DatabasePort == 0 || settings.DatabasePort == 3306)
        {
            settings.DatabasePort = PrivateDatabasePortFinder.FindAvailablePort();
            changed = true;
        }

        if (string.IsNullOrWhiteSpace(settings.AccountName))
        {
            settings.AccountName = "singleplayer";
            changed = true;
        }

        if (string.IsNullOrWhiteSpace(settings.ProtectedAccountPassword))
        {
            settings.ProtectedAccountPassword = protector.Protect(SecretProtector.GeneratePassword());
            changed = true;
        }

        if (string.IsNullOrWhiteSpace(settings.ProtectedDatabasePassword))
        {
            settings.ProtectedDatabasePassword = protector.Protect(SecretProtector.GeneratePassword());
            changed = true;
        }

        if (!string.Equals(settings.ProtectedPrivateDatabasePassword, settings.ProtectedDatabasePassword, StringComparison.Ordinal))
        {
            settings.ProtectedPrivateDatabasePassword = settings.ProtectedDatabasePassword;
            changed = true;
        }

        if (string.IsNullOrWhiteSpace(settings.ProtectedPrivateDatabaseAdminPassword) &&
            !Directory.Exists(Path.Combine(settings.PrivateDatabaseDirectory, "mysql")))
        {
            settings.ProtectedPrivateDatabaseAdminPassword = protector.Protect(SecretProtector.GeneratePassword());
            changed = true;
        }

        return changed;
    }

    private static bool SetIfDifferent<T>(T current, T replacement, Action<T> setter)
    {
        if (EqualityComparer<T>.Default.Equals(current, replacement))
            return false;

        setter(replacement);
        return true;
    }
}
