using ACE.Common;
using ACE.Server.Managers.TransferManager.Responses;

namespace ACE.WebApiServer.Model
{
    public class ServerInfoResponseModel
    {
        public string WorldName { get; set; }
        public string Welcome { get; set; }
        public double Uptime { get; set; }
        public PlayerCountResponseModel PlayerCount { get; set; }
        public TransferConfigResponseModel Transfers { get; set; }
        public AccountDefaults AccountDefaults { get; set; }
    }
}
