using ACE.Cryptography;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ACE.Network
{
    class PacketManager
    {
        delegate void FragmentHandler(PacketFragment fragement, Session session);
        private static Dictionary<FragmentOpcode, FragmentHandler> fragmentHandlers;

        public static void Initialise()
        {
            DefineFragmentHandlers();
        }

        private static void DefineFragmentHandlers()
        {
            fragmentHandlers = new Dictionary<FragmentOpcode, FragmentHandler>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                foreach (var methodInfo in type.GetMethods())
                    foreach (var fragmentHandlerAttribute in methodInfo.GetCustomAttributes<FragmentAttribute>())
                        fragmentHandlers[fragmentHandlerAttribute.Opcode] = (FragmentHandler)Delegate.CreateDelegate(typeof(FragmentHandler), methodInfo);
        }

        public static void HandleLoginPacket(ClientPacket packet, Session session)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
                HandleLoginRequest(packet, session);

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
                HandleConnectResponse(packet, session);

            foreach (ClientPacketFragment fragment in packet.Fragments)
            {
                var opcode = (FragmentOpcode)fragment.Payload.ReadUInt32();
                if (!fragmentHandlers.ContainsKey(opcode))
                    Console.WriteLine($"Received unhandled fragment opcode: 0x{((int)opcode).ToString("X4")}");
                else
                {
                    FragmentHandler fragmentHandler;
                    if (fragmentHandlers.TryGetValue(opcode, out fragmentHandler))
                        fragmentHandler.Invoke(fragment, session);
                }
            }
        }

        private static void HandleLoginRequest(ClientPacket packet, Session session)
        {
            string someString = packet.Payload.ReadString16L();
            packet.Payload.Skip(14);
            uint timestamp    = packet.Payload.ReadUInt32();
            string account    = packet.Payload.ReadString16L();
            packet.Payload.Skip(5);
            string glsTicket  = packet.Payload.ReadString32L();

            var connectResponse = new ServerPacket(0x0B, PacketHeaderFlags.ConnectRequest);
            connectResponse.Payload.Write(0u);
            connectResponse.Payload.Write(0u);
            connectResponse.Payload.Write(13626398284849559039ul); // some sort of check value?
            connectResponse.Payload.Write((ushort)0);
            connectResponse.Payload.Write((ushort)0);
            connectResponse.Payload.Write(ISAAC.ServerSeed);
            connectResponse.Payload.Write(ISAAC.ClientSeed);
            connectResponse.Payload.Write(0u);

            NetworkMgr.SendLoginPacket(connectResponse, session);
        }

        private static void HandleConnectResponse(ClientPacket packet, Session session)
        {
            ulong check = packet.Payload.ReadUInt64(); // 13626398284849559039 - sent in previous packet

            var characterList     = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterList);

            characterFragment.Payload.Write(0u);
            characterFragment.Payload.Write(1u /*characterCount*/);

            // character loop
            {
                characterFragment.Payload.Write(123u);
                characterFragment.Payload.WriteString16L("Test Character");
                characterFragment.Payload.Write(0u);
            }

            characterFragment.Payload.Write(0u);
            characterFragment.Payload.Write(11u /*slotCount*/);
            characterFragment.Payload.WriteString16L("accountname");
            characterFragment.Payload.Write(0u);
            characterFragment.Payload.Write(0u);
            characterList.Fragments.Add(characterFragment);

            NetworkMgr.SendLoginPacket(characterList, session);

            var serverName         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var serverNameFragment = new ServerPacketFragment(9, FragmentOpcode.ServerName);

            serverNameFragment.Payload.Write(0u);
            serverNameFragment.Payload.Write(0u);
            serverNameFragment.Payload.WriteString16L("ACEmulator");
            serverName.Fragments.Add(serverNameFragment);

            NetworkMgr.SendLoginPacket(serverName, session);

            // looks like account settings/info, expansion information ect?
            var packet75e5         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var packet75e5Fragment = new ServerPacketFragment(9, FragmentOpcode.Unknown75E5);

            packet75e5Fragment.Payload.Write(0u);
            packet75e5Fragment.Payload.Write(1u);
            packet75e5Fragment.Payload.Write(1u);
            packet75e5Fragment.Payload.Write(1u);
            packet75e5Fragment.Payload.Write(2u);
            packet75e5Fragment.Payload.Write(0u);
            packet75e5Fragment.Payload.Write(1u);
            packet75e5.Fragments.Add(packet75e5Fragment);

            NetworkMgr.SendLoginPacket(serverName, session);

            var patchStatus = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            patchStatus.Fragments.Add(new ServerPacketFragment(5, FragmentOpcode.PatchStatus));

            NetworkMgr.SendLoginPacket(patchStatus, session);
        }
    }
}
