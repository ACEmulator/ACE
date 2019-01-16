using ACE.Server.Managers;
using ACE.Server.WebApi.Requests;
using ACE.Server.WebApi.Services;
using ACE.Server.WebApi.Util;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using System.IO;
using System.Threading.Tasks;

namespace ACE.Server.WebApi.Modules
{
    public class CharacterTransferModule : NancyModule
    {
        private readonly ICharacterTransferService _charTransferService;

        public CharacterTransferModule(ICharacterTransferService charTransferService)
        {
            _charTransferService = charTransferService;
            Get("/DownloadCharacter", DownloadCharacterAsync);
            Get("/GetServerThumbprint", GetServerThumbprintAsync);
        }
        private async Task<object> GetServerThumbprintAsync(dynamic parameters)
        {
            return _charTransferService.GetServerThumbprint();
        }
        private async Task<object> DownloadCharacterAsync(dynamic parameters)
        {
            DownloadCharacterRequest request = this.BindAndValidate<DownloadCharacterRequest>();
            if (!ModelValidationResult.IsValid)
            {
                return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
            }
            TransferManager.CharacterDownload dl = _charTransferService.DownloadCharacter(request.Cookie);

            if (dl.Valid)
            {
                ReportingFileStream rfs = new ReportingFileStream(new FileStream(dl.FilePath, FileMode.Open));
                rfs.OnFileStreamClosed += (sender, e) =>
                {
                    dl.UploadCompleted();
                };
                string fileName = Path.GetFileName(dl.FilePath);
                StreamResponse response = new StreamResponse(() => rfs, MimeTypes.GetMimeType(fileName));
                return response.AsAttachment(fileName);
            }
            else
            {
                return 404;
            }
        }
    }
}
