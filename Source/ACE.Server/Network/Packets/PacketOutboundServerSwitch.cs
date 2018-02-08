namespace ACE.Server.Network.Packets
{
    public class PacketOutboundServerSwitch : ServerPacket
    {
        public PacketOutboundServerSwitch() : base()
        {
            this.Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.ServerSwitch;
            BodyWriter.Write((uint)0x18); // This value is currently the hard coded Server ID. It can be something different...
            BodyWriter.Write((uint)0x00);
        }
    }
}
