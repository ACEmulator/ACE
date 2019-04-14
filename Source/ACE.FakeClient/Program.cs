using ACE.Server.Network;
using System;
using System.Net.Sockets;
using System.Text;

namespace ACE.FakeClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // This constructor arbitrarily assigns the local port number.
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                udpClient.Connect("localhost", 9000);

                ServerPacket packet = new ServerPacket();


                packet.Header.Flags |= PacketHeaderFlags.LoginRequest;

                // Sends a message to the host to which you have connected.
                byte[] sendBytes = new byte[500];
                int size = 0;
                packet.CreateReadyToSendPacket(sendBytes, out size);

                udpClient.Send(sendBytes, size);

                //// Sends a message to a different host using optional hostname and port parameters.
                //UdpClient udpClientB = new UdpClient();
                //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

                ////IPEndPoint object will allow us to read datagrams sent from any source.
                //IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                //// Blocks until a message returns on this socket from a remote host.
                //Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                //string returnData = Encoding.ASCII.GetString(receiveBytes);

                //// Uses the IPEndPoint object to determine which of these two hosts responded.
                //Console.WriteLine("This is the message you received " +
                //                             returnData.ToString());
                //Console.WriteLine("This message was sent from " +
                //                            RemoteIpEndPoint.Address.ToString() +
                //                            " on their port number " +
                //                            RemoteIpEndPoint.Port.ToString());

                //udpClient.Close();
                //udpClientB.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
