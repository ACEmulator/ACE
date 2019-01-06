using System.Threading.Tasks;

namespace ACE.Server.Web.Services
{
    public interface ICharacterTransferService
    {
        Task<string> DownloadCharacter(string cookie);
    }
}
