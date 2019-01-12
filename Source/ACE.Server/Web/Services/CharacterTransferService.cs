using ACE.Server.API;
using ACE.Server.API.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Web.Services
{
    internal class CharacterTransferService : ICharacterTransferService
    {
        public CharacterDownload DownloadCharacter(string cookie)
        {
            CharacterDownload dl = null;
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
