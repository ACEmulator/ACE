using System.Threading.Tasks;

namespace ACE.Server.Web.Services
{
    internal class CharacterTransferService : ICharacterTransferService
    {
        public Task<string> DownloadCharacter(string cookie)
        {
            return Task.FromResult("");
        }
    }
}
