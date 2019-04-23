using ACE.Common;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.FAKECLIENT;
using ACE.Network.Messages;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.Managers;
using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.FakeClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ConfigManager.Initialize();
            InboundMessageManager.Initialize();
            Proc(1);
            Console.ReadKey();
        }

        private enum OverallState
        {
            waitForConnectReq,
            waitForConnectResp,
            CharListScreen,
            WaitForNewCharList,
            EnterWorldRequested,
            EnterReady,
            WaitForWorldEntered
        }
        [Flags]
        private enum StateWaitForConnectResp
        {
            GotCharList = 1,
            GotServerName = 2,
        }
        private class HeadlessState
        {
            public OverallState OverallState { get; set; } = OverallState.waitForConnectReq;
            public StateWaitForConnectResp ConnectResponseState { get; set; }
            public LoginCharacterSetS2C CharacterList { get; set; }
            public WorldInfoS2C WorldInfo { get; set; }
            public IPEndPoint them = new IPEndPoint(IPAddress.Parse("192.168.0.2"), 9000);
            public FakeUdpClient client = new FakeUdpClient("192.168.0.2", 9000);
            public FakeUdpClient client1 = new FakeUdpClient("192.168.0.2", 9001);
            public OutboundPacketQueue OutboundQueue { get; set; }
            public Session session = null;
        }
        private static HeadlessState State = new HeadlessState();
        private static void Proc(int i)
        {

            State.client.Connect();
            State.OutboundQueue = new OutboundPacketQueue(State.client.Socket);
            NetworkManager.OutboundQueue = State.OutboundQueue;
            State.client.SendAsync(LoginPacket($"test{i}", "*FEF436HDS#1${%gsdIUFH"));
            PacketInboundConnectRequest connectRequest = null;
            onPkt += (pkt) =>
            {
                if (!pkt.ValidateCRC(State.session?.CryptoClient))
                {
                    if (pkt.Header.Sequence != 0 && State.OverallState != OverallState.waitForConnectReq) // why that first S2C failing CRC?
                    {
                        log($"bad CRC for {pkt}");
                    }
                }
                switch (State.OverallState)
                {
                    case OverallState.waitForConnectReq:
                        if (pkt.Payload.BaseStream.Length == 32)
                        {
                            connectRequest = new PacketInboundConnectRequest(pkt);
                            State.session = new Session(State.them, pkt.Header.Id, (ushort)connectRequest.ClientId, TimeSpan.Zero);
                            State.session.ResetCrypto(connectRequest.IsaacServerSeed, connectRequest.IsaacClientSeed);
                            State.session.EndPoint = State.them;
                            State.client1.Connect();
                            State.client.SendAsync(State.client1.Endpoint, ConnectResponsePacket(connectRequest.Cookie));
                            State.client1.Disconnect();
                            State.OverallState = OverallState.waitForConnectResp;
                            State.session.State = SessionState.AuthConnected;
                            State.session.FragmentSequence = 1;
                            uint z = State.session.PacketSequence.NextValue;
                        }
                        else
                        {
                            State.session?.ProcessSessionPacket(pkt);
                        }
                        break;

                    case OverallState.CharListScreen:
                        State.session.ProcessSessionPacket(pkt);
                        if (State.CharacterList.Set.Count == 1)
                        {
                            GameMessageEnterWorldRequest ewr = new GameMessageEnterWorldRequest();
                            State.session.EnqueueSend(ewr);
                            State.OverallState = OverallState.EnterWorldRequested;
                            log($"Sending enter world request");
                        }
                        else
                        {
                            log($"no characters, and character creation not implemented yet!!!");
                        }
                        break;
                    case OverallState.EnterReady:
                        State.session.ProcessSessionPacket(pkt);
                        GameMessageEnterWorld ew = new GameMessageEnterWorld(State.CharacterList.Set[0].Gid, State.CharacterList.Account);
                        State.session.EnqueueSend(ew);
                        State.OverallState = OverallState.WaitForWorldEntered;
                        log($"Entering world with {State.CharacterList.Set[0].Name}");
                        break;

                    default:
                        State.session?.ProcessSessionPacket(pkt);
                        break;
                }
            };

            Thread InboundPacketQueueProcessor = new Thread(new ParameterizedThreadStart(NetConsumer))
            {
                Name = "InboundPacketQueueManager"
            };
            InboundPacketQueueProcessor.Start(State.client);


            while (true)
            {
                Thread.Sleep(300);
                WorldManager.InboundMessageQueue.RunActions();
                State.session?.TickOutbound();
                NetworkManager.DoSessionWork();
            }
        }

        public class GameMessageEnterWorldRequest : GameMessage
        {
            public GameMessageEnterWorldRequest()
                : base(GameMessageOpcode.CharacterEnterWorldRequest, GameMessageGroup.LoginQueue)
            {
                SendEnterWorldRequestC2S ew = new SendEnterWorldRequestC2S() { };
                ew.Pack(Writer);
            }
        }
        public class GameMessageEnterWorld : GameMessage
        {
            public GameMessageEnterWorld(uint gid, string account)
                : base(GameMessageOpcode.CharacterEnterWorld, GameMessageGroup.LoginQueue)
            {
                SendEnterWorldC2S ew = new SendEnterWorldC2S() { Gid = State.CharacterList.Set[0].Gid, Account = State.CharacterList.Account };
                ew.Pack(Writer);
            }
        }
        [GameMessage(GameMessageOpcode.CharacterEnterWorldServerReady, SessionState.AuthConnected)]
        public static void enterReady(ClientMessage message, Session session)
        {
            EnterGameServerReadyS2C z = new EnterGameServerReadyS2C(message.Payload);
            log($"Server said character enter ready");
            State.OverallState = OverallState.EnterReady;
        }
        [GameMessage(GameMessageOpcode.CharacterError, SessionState.AuthConnected)]
        public static void charError(ClientMessage message, Session session)
        {
            CharacterErrorS2C z = new CharacterErrorS2C(message.Payload);
            log($"Character Error: {z.Reason}");
        }

        [GameMessage(GameMessageOpcode.DDD_Interrogation, SessionState.AuthConnected)]
        public static void DDDInterrogation(ClientMessage message, Session session)
        {
            InterrogationMessageS2C z = new InterrogationMessageS2C(message.Payload);
            log($"Got DDD Interrogate!");
        }

        [GameMessage(GameMessageOpcode.ServerName, SessionState.AuthConnected)]
        public static void ServerName(ClientMessage message, Session session)
        {
            State.WorldInfo = new WorldInfoS2C(message.Payload);
            log($"World {State.WorldInfo.WorldName} ready, connections: {State.WorldInfo.Connections} / {State.WorldInfo.MaxConnections}");
            State.ConnectResponseState = State.ConnectResponseState | StateWaitForConnectResp.GotServerName;
            if (State.ConnectResponseState.HasFlag(StateWaitForConnectResp.GotCharList))
            {
                State.OverallState = OverallState.CharListScreen;
            }
        }

        [GameMessage(GameMessageOpcode.CharacterList, SessionState.AuthConnected)]
        public static void CharacterList(ClientMessage message, Session session)
        {
            State.CharacterList = new LoginCharacterSetS2C(message.Payload);
            log($"Account {State.CharacterList.Account} connected, characters: {State.CharacterList.Set.Count}");
            //session.Account = charSet.Account;
            State.ConnectResponseState = State.ConnectResponseState | StateWaitForConnectResp.GotCharList;
            if (State.ConnectResponseState.HasFlag(StateWaitForConnectResp.GotServerName))
            {
                State.OverallState = OverallState.CharListScreen;
            }
        }

        private static void log(string what)
        {
            Console.WriteLine(what);
        }
        private static event PacketDele onPkt;
        private delegate void PacketDele(ClientPacket cp);
        private static Task _readerTask;
        private static void NetConsumer(object _client)
        {
            FakeUdpClient client = (FakeUdpClient)_client;
            _readerTask = Task.Factory.StartNew(() =>
            {
                foreach (byte[] pkt in client.packets.GetConsumingEnumerable())
                {
                    ClientPacket k = new ClientPacket(pkt);
                    onPkt(k);
                }
            });
        }

        public class PacketInboundConnectRequest : ServerPacket
        {
            public double ServerTime { get; }
            public ulong Cookie { get; }
            public uint ClientId { get; }
            public byte[] IsaacServerSeed { get; }
            public byte[] IsaacClientSeed { get; }

            public PacketInboundConnectRequest(ClientPacket packet)
            {
                ServerTime = packet.Payload.ReadDouble();
                Cookie = packet.Payload.ReadUInt64();
                ClientId = packet.Payload.ReadUInt32();
                IsaacServerSeed = packet.Payload.ReadBytes(4);
                IsaacClientSeed = packet.Payload.ReadBytes(4);
            }
        }

        private static byte[] ConnectResponsePacket(ulong cookie)
        {
            ServerPacket packet = new ServerPacket();
            packet.Header.Flags |= PacketHeaderFlags.ConnectResponse;
            packet.InitializeBodyWriter();

            packet.BodyWriter.Write(cookie);

            byte[] sendBytes = new byte[500];
            packet.CreateReadyToSendPacket(sendBytes, out int size);
            byte[] p = new byte[size];
            Buffer.BlockCopy(sendBytes, 0, p, 0, size);
            return p;
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
