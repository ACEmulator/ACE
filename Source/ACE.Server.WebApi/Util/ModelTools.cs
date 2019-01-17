using Nancy;
using Newtonsoft.Json;

namespace ACE.Server.WebApi.Util
{
    internal static class ModelTools
    {
        public static Response AsJson(this /* ok MOM */ object Model)
        {
            Response response = JsonConvert.SerializeObject(Model, serializationSettings);
            response.ContentType = "application/json";
            return response;
        }
        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };
    }
}
