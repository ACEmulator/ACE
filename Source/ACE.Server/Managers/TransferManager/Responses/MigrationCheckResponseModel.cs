namespace ACE.Server.Managers.TransferManager.Responses
{
    public class MigrationCheckResponseModel
    {
        public TransferConfigResponseModel Config { get; set; }
        public string Nonce { get; set; }
        public bool Ready { get; set; }
        public string ReadyStatus { get; set; }
        public string Cookie { get; set; }
    }
}
