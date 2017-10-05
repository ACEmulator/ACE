using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACE.Web.Models
{
    public class GithubConfig
    {
        [JsonProperty("githubClientId")]
        public string GithubClientId { get; set; }

        [JsonProperty("githubClientSecret")]
        public string GithubClientSecret { get; set; }

        public static GithubConfig Load()
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["githubConfigLocation"];
            string content = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GithubConfig>(content);
        }
    }
}