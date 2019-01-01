using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ACE.Server.TransferServer
{
    internal class TransferServerWrapper : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Dispose()
        {
            server.ContinueListen = false;
            try
            {
                server.server.Stop();
            }
            catch (Exception) { }
        }
        private TransferServerCore server = null;

        public void Listen(string address, int port, Action<TransferServerHttpRequest> reqHandler)
        {
            server = new TransferServerCore
            {
                RequestHandler = (req) =>
                {
                    try
                    {
                        reqHandler(req);

                        if (req.Client.Connected)
                        {
                            req.NetworkStream.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (req.Client.Connected)
                        {
                            try { req.NetworkStream.Close(); }
                            catch { }
                        }

                        log.Fatal(ex);
                    }
                }
            };
            server.OnLogMessage += (what) => { log.Info(what); };
            server.Listen(address, port);
        }

        public static void ServeZipFile(string filePath, NetworkStream ns)
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
Content-Length: {fi.Length}{"\r\n\r\n"}";
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            ns.Write(headerBytes, 0, headerBytes.Length);
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.CopyTo(ns);
            }
        }
        public static Dictionary<string, string> ParseQueryString(string queryStr)
        {
            return queryStr.Split('&')
                .Where(i => i.Contains("="))
                .Select(i => i.Trim().Split('='))
                .ToDictionary(i => i.First(), i => i.Last());
        }
    }
}
