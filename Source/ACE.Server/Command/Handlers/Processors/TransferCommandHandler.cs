using ACE.Server.Managers;
using ACE.Server.Network;
using log4net;
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
            Task.Run(() => RunExport(session));
        }
        public void RunImportAsync(Session session, params string[] parameters)
        {
            Task.Run(() => RunImport(session));
        }
        private async Task RunImport(Session session, params string[] parameters)
        {
            string charName = await TransferManager.Import(session, parameters);
        }
        private async Task RunExport(Session session, params string[] parameters)
        {
            string cookie = await TransferManager.Export(session);
            session.WorldBroadcast($"Character ready for transfer.  A secret cookie has been assigned for this transfer.  " +
                "Use the secret cookie to either download your character or transfer your character to another server by logging into " + "" +
                $"the target server and entering the command (replace <hostname> with the ip address or domain name of THIS server): @import-char ACE://<hostname>/{cookie}");
        }
    }
}
