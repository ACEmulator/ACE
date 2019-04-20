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
