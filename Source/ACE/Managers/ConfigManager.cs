using Newtonsoft.Json;
using System;
using System.IO;

namespace ACE.Managers
{
    public struct ConfigServerNetwork
    {
        public string Host { get; set; }
        public uint LoginPort { get; set; }
        public uint WorldPort { get; set; }
    }

    public struct ConfigServer
    {
        public string WorldName { get; set; }
        public string Welcome { get; set; }
        public ConfigServerNetwork Network { get; set; }
    }

    public struct ConfigMySqlDatabase
    {
        public string Host { get; set; }
        public uint Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public struct ConfigMySql
    {
        public ConfigMySqlDatabase Authentication { get; set; }
        public ConfigMySqlDatabase Character { get; set; }
        public ConfigMySqlDatabase World { get; set; }
        public ConfigMySqlDatabase Global { get; set; }
    }

    public struct Config
    {
        public ConfigServer Server { get; set; }
        public ConfigMySql MySql { get; set; }
    }

    public static class ConfigManager
    {
        public static Config Config { get; private set; }
        public static byte[] Host { get; } = new byte[4];

        public static void Initialise()
        {
            try
            {
                // should probably do validation of config data here
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@".\Config.json"));

                // cache this rather then calculating it each time a client is transfered to the world socket
                string[] hostSplit = Config.Server.Network.Host.Split('.');
                for (uint i = 0; i < 4; i++)
                    Host[i] = Convert.ToByte(hostSplit[i]);
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured while loading the configuration file!");
                Console.WriteLine($"Exception: {exception.Message}");
                Environment.Exit(-1);
            }
        }
    }
}
