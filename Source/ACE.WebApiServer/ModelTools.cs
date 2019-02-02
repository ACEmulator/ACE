using Nancy;
using Newtonsoft.Json;

namespace ACE.WebApiServer
{
    internal static class ModelTools
    {
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, serializationSettings);
        }
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static Response AsJsonWebResponse(this object Model)
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
