using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    [Flags]
    public enum HookFlags
    {
        Inscribable = 0x1,
        IsHealer    = 0x2,
        IsLockpick  = 0x8
    };

    [Flags]
    public enum HookAmmoType
    {
        ThrownWeapon = 0x0,
        Arrow = 0x1,
        Bolt = 0x2,
        Dart = 0x4,
    }

    public class HookProfile
    {
        public HookFlags Flags;
        public uint ValidLocations;
        public HookAmmoType AmmoType;
    }

    public static class HookProfileExtensions
    {
        public static void Write(this BinaryWriter writer, HookProfile hook)
        {
            writer.Write((uint)hook.Flags);
            writer.Write(hook.ValidLocations);
            writer.Write((ushort)hook.AmmoType);
        }
    }
}
