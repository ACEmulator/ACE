using ACE.Common;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ACE.FakeClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Parallel.ForEach(Enumerable.Range(1, 100000), (index) =>
            {
                Proc();
            });
        }
        private static void Proc()
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                udpClient.Connect("localhost", 9000);
                byte[] pktLogin = LoginPacket("fartwhif", "asdf");
                udpClient.Send(pktLogin, pktLogin.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static byte[] LoginPacket(string user, string pass)
        {
            ServerPacket packet = new ServerPacket();
            packet.Header.Flags |= PacketHeaderFlags.LoginRequest;
            packet.InitializeBodyWriter();

            packet.BodyWriter.WriteString16L("1802");

            long lenA = packet.BodyWriter.BaseStream.Position;

            packet.BodyWriter.Write(0);
            packet.BodyWriter.Write((uint)NetAuthType.AccountPassword);
            packet.BodyWriter.Write((uint)AuthFlags.None);
            packet.BodyWriter.Write((uint)Time.GetUnixTime());
            packet.BodyWriter.WriteString16L(user);
            packet.BodyWriter.WriteString16L("");
            WriteString32L(packet.BodyWriter, pass);

            long lenB = packet.BodyWriter.BaseStream.Position;

            long offs = lenB - lenA;

            packet.BodyWriter.BaseStream.Position -= offs;
            packet.BodyWriter.Write((uint)offs);

            byte[] sendBytes = new byte[500];
            packet.CreateReadyToSendPacket(sendBytes, out int size);
            byte[] p = new byte[size];
            Buffer.BlockCopy(sendBytes, 0, p, 0, size);
            return p;
        }

        private static void WriteString32L(BinaryWriter writer, string str)
        {
            if (str.Length > 255)
            {
                throw new Exception("sorry, implementation of writeString32L is incomplete and only works for strings of length < 256");
            }
            uint padMultiple = BinaryReaderExtensions.CalculatePadMultiple(sizeof(ushort) + (uint)str.Length, 4u);
            writer.Write((uint)str.Length + 1);
            writer.Write((byte)0);
            writer.Write(Encoding.ASCII.GetBytes(str));
            writer.Pad(padMultiple);
        }
    }
}
