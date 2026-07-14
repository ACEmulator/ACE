using System;
using System.IO;

namespace ACE.Server
{
    public sealed class ServerLaunchOptions
    {
        public string ConfigPath { get; private set; }

        public string ReadyFilePath { get; private set; }

        public static ServerLaunchOptions Parse(string[] args)
        {
            var options = new ServerLaunchOptions();

            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--config":
                        options.ConfigPath = ReadAbsolutePath(args, ref i, "--config");
                        break;
                    case "--ready-file":
                        options.ReadyFilePath = ReadAbsolutePath(args, ref i, "--ready-file");
                        break;
                }
            }

            return options;
        }

        private static string ReadAbsolutePath(string[] args, ref int index, string option)
        {
            if (index + 1 >= args.Length || string.IsNullOrWhiteSpace(args[index + 1]))
                throw new ArgumentException($"{option} requires an absolute path.");

            var path = args[++index];
            if (!Path.IsPathFullyQualified(path))
                throw new ArgumentException($"{option} requires an absolute path: {path}");

            return Path.GetFullPath(path);
        }
    }
}
