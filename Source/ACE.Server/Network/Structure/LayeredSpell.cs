using System.IO;

namespace ACE.Server.Network.Structure
{
    public class LayeredSpell
    {
        public ushort SpellId;
        public ushort Layer;

        public LayeredSpell() { }

        public LayeredSpell(ushort spellId, ushort layer)
        {
            SpellId = spellId;
            Layer = layer;
        }
    }

    public static class LayeredSpellExtensions
    {
        public static LayeredSpell ReadLayeredSpell(this BinaryReader reader)
        {
            var layeredSpell = new LayeredSpell();
            layeredSpell.SpellId = reader.ReadUInt16();
            layeredSpell.Layer = reader.ReadUInt16();

            return layeredSpell;
        }

        public static void Write(this BinaryWriter writer, LayeredSpell spell)
        {
            writer.Write(spell.SpellId);
            writer.Write(spell.Layer);
        }
    }
}
