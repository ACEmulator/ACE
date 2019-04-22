using System.Collections.Generic;
using System.IO;

using ACE.Database.Models.Shard;

namespace ACE.Server.Network.Structure
{
    public class Shortcut
    {
        public uint Index;          // position on shortcut bar
        public uint ObjectId;
        public LayeredSpell Spell;  // unused?

        public Shortcut() { }

        public Shortcut(uint objectId, uint index)
        {
            Index = index;
            ObjectId = objectId;
            Spell = new LayeredSpell();
        }

        public Shortcut(CharacterPropertiesShortcutBar shortcut)
        {
            Index = shortcut.ShortcutBarIndex - 1;
            ObjectId = shortcut.ShortcutObjectId;
            Spell = new LayeredSpell();
        }
    }

    public static class ShortcutExtensions
    {
        public static Shortcut ReadShortcut(this BinaryReader reader)
        {
            var shortcut = new Shortcut();

            shortcut.Index = reader.ReadUInt32();
            shortcut.ObjectId = reader.ReadUInt32();
            shortcut.Spell = reader.ReadLayeredSpell();

            return shortcut;
        }

        public static void Write(this BinaryWriter writer, Shortcut shortcut)
        {
            writer.Write(shortcut.Index);
            writer.Write(shortcut.ObjectId);
            writer.Write(shortcut.Spell);
        }

        public static void Write(this BinaryWriter writer, ICollection<Shortcut> shortcuts)
        {
            writer.Write(shortcuts.Count);
            foreach (var shortcut in shortcuts)
                writer.Write(shortcut);
        }
    }
}
