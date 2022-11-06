using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using ACE.Server.Managers;
using log4net;

namespace ACE.Server.Network
{
    public class ISPInfo
    {
        public string ASN { get; set; }
        public string Provider { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string IsoCode { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public string City { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Proxy { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"ASN = {ASN}, Provider = {Provider}, Continent = {Continent}, Country = {Country}, IsoCode = {IsoCode}, Region = {Region}, RegionCode = {RegionCode}, City = {City}, Latitude = {Latitude}, Longitude = {Longitude}, Proxy = {Proxy}, Type = {Type}";
        }
    }

    public static class VPNDetection
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ApiKey { get; set; } = PropertyManager.GetString("proxycheck_api_key").Item;

        public static async Task<ISPInfo> CheckVPN(string ip)
        {
            //Console.WriteLine("In VPNDetection.CheckVPN");
            if (string.IsNullOrEmpty(ip) || ip.Equals("127.0.0.1"))
            {
                return null;
            }

            var url = $"https://proxycheck.io/v2/{ip}?vpn=1&asn=1&key={ApiKey}";
            if (!string.IsNullOrWhiteSpace(ApiKey))
                url += "&key=" + ApiKey;
            var req = WebRequest.Create(url);
            var task = req.GetResponseAsync();
            if (!(await Task.WhenAny(task, Task.Delay(3000)) == task))
            {
                log.Warn($"VPNDetection.CheckVPN task timed out for ip = {ip}");
                return null; //timed out
            }
            var resp = task.Result;
            using (var stream = resp.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    var data = sr.ReadToEnd();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    var d1 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(data);
                    var d = d1[ip];
                    var ispinfo = new ISPInfo()
                    {
                        ASN = d["asn"],
                        Provider = d["provider"],
                        City = d["city"],
                        Continent = d["continent"],
                        Country = d["country"],
                        IsoCode = d["isocode"],
                        Latitude = d["latitude"],
                        Longitude = d["longitude"],
                        Proxy = d["proxy"],
                        Region = d["region"],
                        RegionCode = d["regioncode"],
                        Type = d["type"]
                    };

                    if (!string.IsNullOrEmpty(ispinfo.Proxy) && ispinfo.Proxy.ToLower().Equals("yes"))
                    {
                        log.Debug($"VPN detected for ip = {ip} with ISPInfo = {ispinfo.ToString()}");
                    }
                    //Console.WriteLine($"VPNDetection.CheckVPN returning ISPInfo = {ispinfo.ToString()}");
                    return ispinfo;
                }
            }
        }
    }
}
