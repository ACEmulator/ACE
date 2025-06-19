// UTF-8 BOM removed to ensure consistent encoding
namespace ACE.Server.Command
{
    public enum CommandHandlerResponse
    {
        Ok,
        SudoOk,
        InvalidCommand,
        NoConsoleInvoke,
        NotAuthorized,
        InvalidParameterCount,
        NotInWorld
    }
}
