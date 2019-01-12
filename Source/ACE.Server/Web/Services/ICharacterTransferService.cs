using ACE.Server.API.Entity;

namespace ACE.Server.Web.Services
{
    public interface ICharacterTransferService
    {
        CharacterDownload DownloadCharacter(string cookie);
        string GetServerThumbprint();
    }
}
