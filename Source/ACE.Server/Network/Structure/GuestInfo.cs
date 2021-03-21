using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Set of information related to a house guest
    /// </summary>
    public class GuestInfo
    {
        public bool ItemStoragePermission;  // False = access to house, True = access to house+storage
        public string GuestName;            // Name of the guest

        public GuestInfo()
        {
        }

        public GuestInfo(bool itemStoragePermission, string name)
        {
            ItemStoragePermission = itemStoragePermission;
            GuestName = name;
        }
    }

    public static class GuestInfoExtensions
    {
        public static void Write(this BinaryWriter writer, GuestInfo info)
        {
            writer.Write(Convert.ToUInt32(info.ItemStoragePermission));
            writer.WriteString16L(info.GuestName);
        }
    }
}
