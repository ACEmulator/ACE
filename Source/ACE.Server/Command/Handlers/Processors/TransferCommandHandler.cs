using ACE.Server.Managers;
using ACE.Server.Network;
using log4net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACE.Server.Command.Handlers.Processors
{
    /// <summary>
    /// TransferCommandHandler asynchronously handles transfer commands
    /// </summary>
    internal class TransferCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void RunExportAsync(Session session, params string[] parameters)
        {
            Task.Run(() => RunExport(session, parameters));
        }
        public void RunImportAsync(Session session, params string[] parameters)
        {
            Task.Run(() => RunImport(session, parameters));
        }
        private void RunImport(Session session, params string[] parameters)
        {

            List<CommandParameterHelpers.ACECommandParameter> aceParams = new List<CommandParameterHelpers.ACECommandParameter>()
            {
                new CommandParameterHelpers.ACECommandParameter() {
                    Type = CommandParameterHelpers.ACECommandParameterType.PlayerName,
                    Required = true,
                    ErrorMessage = "You must supply a new character name for this import."
                },
                new CommandParameterHelpers.ACECommandParameter()
                {
                    Type = CommandParameterHelpers.ACECommandParameterType.Uri,
                    Required = true,
                    ErrorMessage = "You must supply the URL to the zip file download containing the character to import."
                }
            };
            if (!CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams)) return;


            string charName = TransferManager.Import(session, aceParams[0].AsString, aceParams[1].AsUri);
            if (charName != null)
            {
                session.WorldBroadcast($"Character {charName} imported.");
            }
            else
            {
                session.WorldBroadcast($"Character import failed.");
            }
        }
        private async Task RunExport(Session session, params string[] parameters)
        {
            string url = await TransferManager.Export(session);
            session.WorldBroadcast($"Character ready for transfer.  A secret one-time cookie has been assigned for this transfer." +
                "\nTo use the secret cookie to either download your character with a browser or transfer your character to another server by" +
                "\nlogging into the target server and entering the command (replacing <name> with desired character name):" +
                $"\n@import-char <name> {url}");
        }
    }
}
