using ACE.Entity.Enum;
using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    [Flags]
    public enum HookFlags
    {
        None        = 0x0,
        Inscribable = 0x1,
        IsHealer    = 0x2,
        IsFood      = 0x4,
        IsLockpick  = 0x8
    };

    public class HookProfile
    {
        public HookFlags Flags = HookFlags.None;
        public EquipMask ValidLocations = EquipMask.None;
        public AmmoType AmmoType = AmmoType.None;
    }

    public static class HookProfileExtensions
    {
        public static void Write(this BinaryWriter writer, HookProfile hook)
        {
            writer.Write((uint)hook.Flags);
            writer.Write((uint)hook.ValidLocations);
            writer.Write((uint)hook.AmmoType);
        }
    }
}
