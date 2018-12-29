using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

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
            TransferServerRequestHandler.Shutdown = true;
        }
        private TransferServerCore server = null;
        private static Dictionary<string, string> GetCookies(TransferServerHttpRequest req)
        {
            if (req.Headers.ContainsKey("Cookie"))
            {
                return req.Headers["Cookie"].Split(';')
                    .Where(i => i.Contains("="))
                    .Select(i => i.Trim().Split('='))
                    .ToDictionary(i => i.First(), i => i.Last());
            }
            return new Dictionary<string, string>();
        }
        public void Listen(string address, int port)
        {
            TransferServerRequestHandler.Shutdown = false;
            server = new TransferServerCore
            {

                RequestHandler = (req) =>
                {
                    try
                    {
                        Dictionary<string, string> cookies = GetCookies(req);
                        if (!cookies.ContainsKey("session") || TransferServerRequestHandler.GetSession(cookies["session"]) == null)
                        {
                            TransferServerRequestHandler newSess = TransferServerRequestHandler.GetSession();
                            newSess.HandleRequest(req, new Tuple<string, string>[] { new Tuple<string, string>("Set-Cookie", "session=" + newSess.Name + @";path=/") });
                        }
                        else
                        {
                            TransferServerRequestHandler.GetSession(cookies["session"]).HandleRequest(req);
                        }

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
    }
}
