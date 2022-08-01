namespace ACE.Server.Command
{
    public enum CommandDigestionResult
    {
        Success,
        ParseError,
        InvocationError,
        CommandHandlerException,
        CommandHandlerResponseError
    }
}
