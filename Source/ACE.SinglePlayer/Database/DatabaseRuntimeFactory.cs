using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class DatabaseRuntimeFactory
{
    private readonly DatabaseConnectionFactory connectionFactory;
    private readonly LauncherLog log;

    public DatabaseRuntimeFactory(DatabaseConnectionFactory connectionFactory, LauncherLog log)
    {
        this.connectionFactory = connectionFactory;
        this.log = log;
    }

    public IDatabaseRuntime Create(LauncherSettings settings)
    {
        var external = new ExternalMariaDbRuntime(connectionFactory);
        return settings.DatabaseMode == DatabaseMode.External
            ? external
            : new ManagedMariaDbRuntime(connectionFactory, external, log);
    }
}
