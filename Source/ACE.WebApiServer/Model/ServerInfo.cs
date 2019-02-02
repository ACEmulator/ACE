using ACE.Common;
using static ACE.Server.Managers.TransferManager;

namespace ACE.WebApiServer.Model
{
    public class ServerInfoResponseModel
    {
        public string WorldName { get; set; }
        public string Welcome { get; set; }
        public double Uptime { get; set; }
        public PlayerCountResponseModel PlayerCount { get; set; }
        public TransferManagerTransferConfigResponseModel Transfers { get; set; }
        public AccountDefaults AccountDefaults { get; set; }
    }
}
