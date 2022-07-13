using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ACE.Server
{
    public class WebClient: IDisposable
    {
        private readonly HttpClient httpClient;

        public WebClient()
        {
            httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("ACE.Server");
        }

        public async Task<string> GetStringFromURL(string url)
        {
            /* Initialize the request content 
               and 
               Send the Get
            */
            var response = await httpClient.GetAsync(url);

            //Check for error status
            response.EnsureSuccessStatusCode();

            //Get the response content
            return await response.Content.ReadAsStringAsync();
        }

        public async Task DownloadFile(string url, string fileName)
        {
            /* Initialize the request content 
               and 
               Send the Get
            */
            var response = await httpClient.GetAsync(url);

            //Check for error status
            response.EnsureSuccessStatusCode();

            //Get the response content
            using var fs = new FileStream(fileName, FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
        }

        public void Dispose()
        {
            httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
