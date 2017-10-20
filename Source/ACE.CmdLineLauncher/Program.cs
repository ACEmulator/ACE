using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.CmdLineLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            string username;
            string password;

            if (args.Length > 0)
                username = args[0];
            else
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(username))
                return;

            if (args.Length > 1)
                password = args[1];
            else
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(password))
                return;

            // attempt to log in with the password
            RestClient authClient = new RestClient(ConfigurationManager.AppSettings["LoginServer"]);
            var authRequest = new RestRequest("/Account/Authenticate", Method.POST);
            authRequest.AddJsonBody(new { Username = username, Password = password });
            var authResponse = authClient.Execute(authRequest);
            string authToken;

            if (authResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // show the error
                Console.WriteLine("Error logging in");
                Console.WriteLine(authResponse.Content);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Auth successful, retreiving subscriptions...");
            JObject response = JObject.Parse(authResponse.Content);
            authToken = (string)response.SelectToken("authToken");

            RestClient subClient = new RestClient(ConfigurationManager.AppSettings["GameApi"]);
            var subsRequest = new RestRequest("/Subscription/Get", Method.GET);
            subsRequest.AddHeader("Authorization", "Bearer " + authToken);
            var subsResponse = subClient.Execute(subsRequest);

            if (subsResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // show the error
                Console.WriteLine("Error getting subscriptions:");
                Console.WriteLine(subsResponse.Content);
                Console.ReadLine();
                return;
            }

            List<Subscription> subs = JsonConvert.DeserializeObject<List<Subscription>>(subsResponse.Content);

            if (subs.Count < 1)
            {
                Console.Write("No subscriptions found.  Create one? (Y/N) [y]");
                string yesno = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(yesno))
                    yesno = "y";

                if (yesno.ToLower() != "y")
                    return;

                Console.Write("Subscription Name: ");
                string subName = Console.ReadLine();

                // attempt to create subscription
                Console.WriteLine("Attempting to create a subscription...");
                subsRequest = new RestRequest("/Subscription/Create", Method.POST);
                subsRequest.AddQueryParameter("subscriptionName", subName);
                subsResponse = subClient.Execute(subsRequest);

                if (subsResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // show the error
                    Console.WriteLine(subsResponse.Content);
                    Console.ReadLine();
                    return;
                }

                Subscription sub = JsonConvert.DeserializeObject<Subscription>(subsResponse.Content);
                subs = new List<Subscription>() { sub };
            }

            string subscriptionId = null;
            int subIndex = 0;

            if (subs.Count == 1)
            {
                subscriptionId = subs[0].SubscriptionGuid.ToString();
            }
            else if (args.Length > 2 && subs.Count > 1 && int.TryParse(args[2], out subIndex) && subIndex < subs.Count)
            {
                // third param is the subscription index
                subscriptionId = subs[subIndex].SubscriptionGuid.ToString();
            }
            else
            {
                Console.WriteLine("Select a subscription.");

                // enumerate the subs
                for (int i = 0; i < subs.Count; i++)
                    Console.WriteLine($"{i}) {subs[i].Name}");

                string selectedSub = Console.ReadLine();
                
                if (int.TryParse(selectedSub, out subIndex) && subIndex < subs.Count)
                {
                    subscriptionId = subs[subIndex].SubscriptionGuid.ToString();
                }
            }

            Console.WriteLine($"ticket length: {authToken.Length}");

            if (subscriptionId != null)
            {
                string exe = ConfigurationManager.AppSettings["ClientExe"];
                string gameServer = ConfigurationManager.AppSettings["GameServer"];
                string gameArgs = $"-a {subscriptionId} -h {gameServer} -glsticketdirect {authToken}";
                ProcessStartInfo psi = new ProcessStartInfo(exe, gameArgs);
                Console.WriteLine($"Game Args: {gameArgs}");
                psi.WorkingDirectory = System.IO.Path.GetDirectoryName(exe);
                Process.Start(psi);

                Console.WriteLine("Press enter to close.");
                Console.ReadLine();
            }
        }
    }
}
