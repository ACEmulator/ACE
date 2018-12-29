using ACE.Server.Managers;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ACE.Server.TransferServer
{
    internal class TransferServerRequestHandler
    {
        public static bool Shutdown = false;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string HeaderTuplesToString(Tuple<string, string>[] additionalHeadersToSet)
        {
            if (additionalHeadersToSet == null || additionalHeadersToSet.Length < 1)
            {
                return string.Empty;
            }
            string x = "";
            foreach (Tuple<string, string> header in additionalHeadersToSet)
            {
                x += header.Item1 + ": " + header.Item2 + "\r\n";
            }
            return x;
        }
        public void HandleRequest(TransferServerHttpRequest req, Tuple<string, string>[] additionalHeadersToSet = null)
        {
            try
            {
                switch (req.Path)
                {
                    case "":
                        Dictionary<string, string> query = ParseQueryString(req.QueryString);
                        KeyValuePair<string, string> tuple = query.FirstOrDefault(k => k.Key == "get");
                        if (tuple.Key == null)
                        {
                            break;
                        }
                        string cookie = tuple.Value;
                        string filePath = TransferManager.GetTransferFilePath(cookie);
                        if (filePath == null)
                        {
                            break;
                        }
                        ServeZipFile(filePath, req.NetworkStream, additionalHeadersToSet);
                        log.Info($"transfer {cookie} uploaded to {req.Client.Client.RemoteEndPoint}");
                        TransferManager.DeleteTransfer(cookie);
                        break;
                    default:
                        byte[] byaResp3 = Encoding.UTF8.GetBytes(@"HTTP/1.1 404 Not Found
" + HeaderTuplesToString(additionalHeadersToSet) + @"
");
                        req.NetworkStream.Write(byaResp3, 0, byaResp3.Length);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        private Dictionary<string, string> ParseQueryString(string queryStr)
        {
            return queryStr.Split('&')
                .Where(i => i.Contains("="))
                .Select(i => i.Trim().Split('='))
                .ToDictionary(i => i.First(), i => i.Last());
        }
        private static Random rand = new Random();
        private static ConcurrentDictionary<string, TransferServerRequestHandler> Sessions = new ConcurrentDictionary<string, TransferServerRequestHandler>();
        public readonly string Name = string.Empty;
        public TransferServerRequestHandler(string Name)
        {
            this.Name = Name;
        }
        public static TransferServerRequestHandler GetSession(string sessionName)
        {
            if (Sessions.ContainsKey(sessionName))
            {
                return Sessions[sessionName];
            }
            return null;
        }
        private static readonly string sessionNameChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static TransferServerRequestHandler GetSession()
        {
            string x = string.Empty;
            lock (rand)
            {
                for (int i = 0; i < 25; i++)
                {
                    x += sessionNameChars[rand.Next(0, sessionNameChars.Length)];
                }
            }
            if (Sessions.ContainsKey(x))
            {
                throw new Exception("randomness error");
            }
            TransferServerRequestHandler newSess = new TransferServerRequestHandler(x);
            Sessions.AddOrUpdate(x, newSess, (a, b) => b);
            return newSess;
        }
        private static void ServeZipFile(string filePath, NetworkStream ns, Tuple<string, string>[] additionalHeadersToSet)
        {
            FileInfo fi = new FileInfo(filePath);
            string header = $@"HTTP/1.1 200 OK
Pragma: public
Expires: 0
Cache-Control: must-revalidate, post-check=0, pre-check=0
Cache-Control: public
Content-Description: File Transfer
Content-type: application/octet-stream
Content-Disposition: attachment; filename=""{fi.Name}""
Content-Transfer-Encoding: binary
Content-Length: {fi.Length}
" + HeaderTuplesToString(additionalHeadersToSet) + @"
";
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            ns.Write(headerBytes, 0, headerBytes.Length);
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.CopyTo(ns);
            }
        }
    }
}
