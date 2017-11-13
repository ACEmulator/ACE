using Microsoft.Owin.Hosting;
using System;
using ACE.Common;
using ACE.Api.Common;
using System.Diagnostics;
using System.IO;
using NetFwTypeLib;
using System.Linq;

namespace ACE.AuthApi.Host
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceConfig.LoadServiceConfig();
            if (!string.IsNullOrWhiteSpace(ConfigManager.Config.AuthServer?.ListenUrl))
            {
                // Get the bind address and port from config
                var url = ConfigManager.Config.AuthServer.ListenUrl;
                bool success = false;
                try
                {
                    var server = WebApp.Start<Startup>(url);

                    success = true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    Console.WriteLine($"Failed to start AuthApi.Host due to the following error:");
                    Console.WriteLine($"{ex.ToString()}");
                    // Console.ReadLine(); // need a readline or the error flashes without being seen
                    Console.WriteLine($"attempting to fix... expect a UAC prompt to follow, click yes or this fix will fail.");
                    string fixedurl = url;
                    if (!fixedurl.EndsWith("/"))
                        fixedurl += "/";
                    string arguments = $"http add urlacl url={fixedurl} user={Environment.UserName}";
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", arguments);

                    Process proc = new Process();

                    procStartInfo.UseShellExecute = true;
                    procStartInfo.CreateNoWindow = true;
                    procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    procStartInfo.Verb = "runas";

                    proc.StartInfo = procStartInfo;

                    try
                    {
                        proc.Start();

                        proc.WaitForExit();

                        var server = WebApp.Start<Startup>(url); // try to start the api host a second time.

                        success = true;
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"Failed to fix TargetInvocationException with the following error:");
                        Console.WriteLine($"{ex2.ToString()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start AuthApi.Host due to the following error:");
                    Console.WriteLine($"{ex.ToString()}");
                    Console.ReadLine(); // need a readline or the error flashes without being seen
                }

                bool fwRuleFound = false;
                string fwCommand = "add";
                string fwRuleName = "ACEAuthApiHost";
                try
                {
                    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    INetFwRule firewallRule = firewallPolicy.Rules.OfType<INetFwRule>().Where(x => x.Name == fwRuleName).FirstOrDefault();

                    if (firewallRule != null)
                    {
                        Console.WriteLine(firewallRule.Name + " rule has been found in Firewall Policy");
                        fwRuleFound = true;
                        fwCommand = "set";
                    }
                }
                catch (Exception exFw)
                {
                    Console.WriteLine("Could not access Windows firewall to validate rules.");
                    Console.WriteLine(exFw.ToString());
                }

                Uri apiUri = new Uri(url.Replace("*", "localhost").Replace("+", "localhost")); // uri must be valid so swapping wildcards for localhost, this is just used to get port.

                if (!fwRuleFound)
                {
                    Console.WriteLine(fwRuleName + " has not been found in Firewall Policy");
                    Console.WriteLine($"attempting to fix... expect a UAC prompt to follow, click yes or this fix will fail.");
                    string argumentsFw = $"advfirewall firewall {fwCommand} rule name={fwRuleName} dir=in protocol=tcp localport={apiUri.Port} action=allow";
                    ProcessStartInfo procStartInfoFw = new ProcessStartInfo("netsh", argumentsFw);

                    Process procFw = new Process();

                    procStartInfoFw.UseShellExecute = true;
                    procStartInfoFw.CreateNoWindow = true;
                    procStartInfoFw.WindowStyle = ProcessWindowStyle.Hidden;
                    procStartInfoFw.Verb = "runas";

                    procFw.StartInfo = procStartInfoFw;

                    try
                    {
                        procFw.Start();

                        procFw.WaitForExit();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"Failed to run firewall {fwCommand} command with the following error:");
                        Console.WriteLine($"{ex2.ToString()}");
                    }
                }

                if (success)
                {
                    Console.WriteLine($"ACE Auth API listening at {url}");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"ACE Auth API failed to start listening.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("There was an error in your AuthApi configuration.");
                Console.ReadLine(); // need a readline or the error flashes without being seen
            }
        }
    }
}
