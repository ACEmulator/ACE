namespace ACE.Server.Command
{
    public class CommandOverallResult
    {
        public CommandHandlerResponse? CommandHandlerResponse { get; set; }
        public CommandDigestionResult CommandResult { get; set; }
    }
}
