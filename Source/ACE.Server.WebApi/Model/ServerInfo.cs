using ACE.Common;

namespace ACE.Server.WebApi.Model
{
    public class ServerInfoResponseModel
    {
        public string WorldName { get; set; }
        public string Welcome { get; set; }
        public PlayerCountResponseModel PlayerCount { get; set; }
        public TransferConfigResponseModel Transfers { get; set; }
        public AccountDefaults AccountDefaults { get; set; }
    }
}
