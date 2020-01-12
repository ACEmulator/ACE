namespace ACE.Server.Network.Packets
{
    public class PacketOutboundServerSwitch : ServerPacket
    {
        public PacketOutboundServerSwitch()
        {
            Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.ServerSwitch;

            InitializeDataWriter();

            DataWriter.Write((uint)0x18); // This value is currently the hard coded Server ID. It can be something different...
            DataWriter.Write((uint)0x00);
        }
    }
}
