using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ACE.FAKECLIENT
{
    internal class FakeUdpClient : NetCoreServer.UdpClient
    {
        public FakeUdpClient(string address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            Disconnect();
            while (IsConnected)
            {
                Thread.Yield();
            }
        }

        protected override void OnConnected()
        {
            // Start receive datagrams
            ReceiveAsync();
        }

        protected override void OnDisconnected()
        {
            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
            {
                Connect();
            }
        }

        public BlockingCollection<byte[]> packets = new BlockingCollection<byte[]>();

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {

            byte[] k = new byte[size];
            Buffer.BlockCopy(buffer, (int)offset, k, 0, (int)size);
            packets.Add(k);

            //Console.WriteLine(size);
            // Continue receive datagrams
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Echo UDP client caught an error with code {error}");
        }

        private bool _stop;
    }
}
