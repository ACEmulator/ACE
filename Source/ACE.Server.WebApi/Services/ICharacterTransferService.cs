using ACE.Server.Managers;

namespace ACE.Server.WebApi.Services
{
    public interface ICharacterTransferService
    {
        TransferManager.CharacterDownload DownloadCharacter(string cookie);
        string GetServerThumbprint();
    }
}
