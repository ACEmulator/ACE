using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.WebApi.Model;
using AutoMapper;
using Nancy.Security;
using System.Threading.Tasks;

namespace ACE.Server.WebApi.Modules
{
    public class IndexModule : BaseModule
    {
        public async Task<IndexModel> GetModelAsync()
        {
            IndexModel model = Mapper.Map<IndexModel>(BaseModel);
            TaskCompletionSource<object> tsc = new TaskCompletionSource<object>();
            Gate.RunGatedAction(() =>
            {
                DatabaseManager.Shard.GetCharacters(uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value), true, (chars) =>
                {
                    model.Characters = chars;
                    tsc.SetResult(new object());
                });
            });
            await tsc.Task;
            return model;
        }

        public IndexModule()
        {
            this.RequiresAuthentication();

            this.RequiresAnyClaim(
                k => k.Type == AccessLevel.Admin.ToString(),
                k => k.Type == AccessLevel.Advocate.ToString(),
                k => k.Type == AccessLevel.Developer.ToString(),
                k => k.Type == AccessLevel.Envoy.ToString(),
                k => k.Type == AccessLevel.Player.ToString(),
                k => k.Type == AccessLevel.Sentinel.ToString());

            Get("/", GetIndexAsync);
        }
        private async Task<object> GetIndexAsync(dynamic parameters)
        {
            return View["index", await GetModelAsync()];
        }
    }
}
