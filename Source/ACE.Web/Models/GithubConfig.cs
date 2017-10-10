using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace ACE.Web.Models
{
    public class GithubConfig
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [JsonProperty("githubClientId")]
        public string GithubClientId { get; set; }

        [JsonProperty("githubClientSecret")]
        public string GithubClientSecret { get; set; }

        public static GithubConfig Load()
        {
            try
            {
                string filePath = System.Configuration.ConfigurationManager.AppSettings["githubConfigLocation"];
                string content = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<GithubConfig>(content);
            }
            catch
            {
                log.Warn("Unable to load github config.");
                return new GithubConfig();
            }
        }
    }
}