using ACE.Entity.Enum;
using ACE.Server.Command;
using ACE.Server.Mods;
using ACE.Server.Network;

namespace HelloCommand;

public sealed class Mod : IHarmonyMod
{
    private const string HelloCommandName = "hello";
    private const string ByeCommandName = "bye";
    private bool helloRegistered;
    private bool byeRegistered;

    public void Initialize()
    {
        helloRegistered = CommandManager.TryAddCommand(
            HandleHello,
            HelloCommandName,
            AccessLevel.Admin,
            CommandHandlerFlag.None,
            "Says hello to the current character.",
            "No parameters",
            overrides: false);
        byeRegistered = CommandManager.TryAddCommand(
            HandleBye,
            ByeCommandName,
            AccessLevel.Player,
            CommandHandlerFlag.None,
            "Logs the current character out.",
            "No parameters",
            overrides: false);

        Console.WriteLine($"[HelloCommand] /hello: {RegistrationStatus(helloRegistered)}; /bye: {RegistrationStatus(byeRegistered)}.");
    }

    public void Dispose()
    {
        if (helloRegistered)
            CommandManager.TryRemoveCommand(HelloCommandName);
        if (byeRegistered)
            CommandManager.TryRemoveCommand(ByeCommandName);
    }

    private static void HandleHello(Session session, string[] parameters)
    {
        if (session.Player is not null)
            session.Player.SendMessage($"Hello, {session.Player.Name}!");
    }

    private static void HandleBye(Session session, string[] parameters) => session.LogOffPlayer(true);

    private static string RegistrationStatus(bool registered) => registered ? "registered" : "name already in use";
}
