using ACE.Server.Web.Requests;
using ACE.Server.Web.Services;
using Nancy;
using Nancy.ModelBinding;
using System.Threading.Tasks;

namespace ACE.Server.Web.Modules
{
    public class CharacterTransferModule : NancyModule
    {
        private readonly ICharacterTransferService _charTransferService;

        public CharacterTransferModule(ICharacterTransferService charTransferService)
        {
            _charTransferService = charTransferService;
            Get("/downloadcharacter", DownloadCharacter);

            Get("/", args => "Hello from Nancy running on CoreCLR");
        }

        private async Task<object> DownloadCharacter(dynamic parameters)
        {
            DownloadCharacterRequest request = this.BindAndValidate<DownloadCharacterRequest>();
            if (!ModelValidationResult.IsValid)
            {
                return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
            }
            string filePath = await _charTransferService.DownloadCharacter(request.Cookie);
            return new
            {
                Result = "offering file attachment"
            };
        }
    }
}
