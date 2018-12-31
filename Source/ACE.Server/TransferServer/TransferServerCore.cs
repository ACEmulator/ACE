using HttpMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ACE.Server.TransferServer
{
    public class TransferServerCore
    {
        public Action<TransferServerHttpRequest> RequestHandler = null;
        public delegate void logDelegate(string what);
        public event logDelegate OnLogMessage;
        private void Log(string what)
        {
            OnLogMessage?.Invoke(what);
        }
        public bool ContinueListen = true;
        public TcpListener server = null;
        public void Listen(string address, int port)
        {
            try
            {
                server = new TcpListener(IPAddress.Parse(address), port);
                server.Start();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return;
            }
            new Thread((r) =>
            {
                try
                {
                    TcpListener srvr = (TcpListener)r;
                    while (ContinueListen)
                    {
                        DoBeginAcceptTcpClient(server);
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            }).Start(server);
        }
        // Thread signal.
        public ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        // Accept one client connection asynchronously.
        public void DoBeginAcceptTcpClient(TcpListener listener)
        {
            try
            {
                // Set the event to nonsignaled state.
                tcpClientConnected.Reset();
                // Start to listen for connections from a client.
                // Accept the connection. 
                // create the accepted socket.
                listener.BeginAcceptTcpClient(
                    new AsyncCallback(DoAcceptTcpClientCallback),
                    listener);
                // Wait until a connection is made and processed before 
                // continuing.
                tcpClientConnected.WaitOne();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
        // Process the client connection.
        public void DoAcceptTcpClientCallback(IAsyncResult iar)
        {
            // Get the listener that handles the client request.
            TcpListener l = (TcpListener)iar.AsyncState;
            TcpClient c;
            try
            {
                c = l.EndAcceptTcpClient(iar);
                new Thread((r) =>
                {
                    try
                    {
                        Parlay(c);
                    }
                    catch (Exception ex)
                    {
                        if (c.Connected)
                        {
                            try { c.Close(); }
                            catch { }
                        }

                        Log(ex.Message);
                    }
                }).Start(c);
                tcpClientConnected.Set();
            }
            catch (SocketException)
            {
                // unrecoverable
                tcpClientConnected.Set();
                return;
            }
            catch (ObjectDisposedException)
            {
                // The listener was Stop()'d, disposing the underlying socket and
                // triggering the completion of the callback. We're already exiting,
                // so just return.
                tcpClientConnected.Set();
                return;
            }
        }
        private void Parlay(TcpClient clnt)
        {
            using (NetworkStream clientStream = clnt.GetStream())
            {
                string strBuffer = string.Empty;
                List<byte> buffer = new List<byte>();
                byte[] q = new byte[8192];
                TransferServerHttpRequestParser parserDele = new TransferServerHttpRequestParser((req) =>
                {
                    if (RequestHandler != null)
                    {
                        req.NetworkStream = clientStream;
                        req.Client = clnt;
                        RequestHandler(req);
                    }
                });
                HttpParser parser = new HttpParser(parserDele);
                while (clnt.Connected)
                {
                    while (clnt.Connected && clientStream.DataAvailable)
                    {
                        int p = clientStream.Read(q, 0, 8192);
                        parser.Execute(new ArraySegment<byte>(q.Take(p).ToArray()));
                    }
                    Thread.Sleep(100);
                }
            }
        }
    }
}
