using ACE.Server.Managers;

namespace ACE.Server.WebApi.Services
{
    internal class CharacterTransferService : ICharacterTransferService
    {
        public TransferManager.CharacterDownload DownloadCharacter(string cookie)
        {
            TransferManager.CharacterDownload dl = null;
            Gate.RunGatedAction(() =>
            {
                dl = TransferManager.DownloadCharacter(cookie);
            });
            return dl;
        }

        public string GetServerThumbprint()
        {
            string thumb = null;
            Gate.RunGatedAction(() =>
            {
                thumb = CryptoManager.Thumbprint;
            });
            return thumb;
        }
    }
}
