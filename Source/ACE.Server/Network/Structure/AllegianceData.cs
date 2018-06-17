using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.Structure
{
    public class AllegianceData
    {
        public AllegianceNode Node;

        public AllegianceData(AllegianceNode node)
        {
            Node = node;
        }
    }

    public static class AllegianceDataExtensions
    {
        public static void Write(this BinaryWriter writer, AllegianceData data)
        {
            // ObjectID - characterID - Character ID
            // uint - cpCached - XP gained while logged off
            // uint - cpTithed - Total allegiance XP contribution
            // uint - bitfield
            // Gender - gender - The gender of the character (for determining title)
            // HeritageGroup - hg - The heritage of the character (for determining title)
            // ushort - rank - The numerical rank (1 is lowest).

            // Choose valid sections by masking against bitfield
            // uint - level

            // ushort - loyalty - Character loyalty
            // ushort - leadership - Character leadership

            // Choose based on testing bitfield == 0x4
            // True:
            // ulong - timeOnline
            // False:
            // uint - timeOnline
            // uint - allegianceAge

            // string - name
            var characterID = new ObjectGuid(0);
            uint cpCached = 0;
            uint cpTithed = 0;
            var bitfield = AllegianceIndex.HasAllegianceAge | AllegianceIndex.HasPackedLevel | AllegianceIndex.LoggedIn;
            var gender = Gender.Female;
            var hg = HeritageGroup.Aluvian;
            ushort rank = 0;
            uint level = 0;
            ushort loyalty = 0;
            ushort leadership = 0;
            ulong uTimeOnline = 0;
            uint timeOnline = 0;
            uint allegianceAge = 0;
            var name = "";

            if (data.Node != null)
            {
                var node = data.Node;
                var player = node.Player;

                characterID = player.Guid;
                gender = (Gender)player.Gender;
                hg = (HeritageGroup)player.Heritage;
                rank = (ushort)node.Rank;
                level = (uint)player.Level;
                loyalty = (ushort)player.GetCreatureSkill(Skill.Loyalty).Current;
                leadership = (ushort)player.GetCreatureSkill(Skill.Leadership).Current;
                name = player.Name;
            }

            writer.Write(characterID.Full);
            writer.Write(cpCached);
            writer.Write(cpTithed);
            writer.Write((uint)bitfield);
            writer.Write((byte)gender);
            writer.Write((byte)hg);
            writer.Write(rank);

            if (bitfield.HasFlag(AllegianceIndex.HasPackedLevel))
                writer.Write(level);

            writer.Write(loyalty);
            writer.Write(leadership);

            if (!bitfield.HasFlag(AllegianceIndex.HasAllegianceAge))  // verify: reversed in aclogview?
                writer.Write(uTimeOnline);
            else
            {
                writer.Write(timeOnline);
                writer.Write(allegianceAge);
            }
            writer.WriteString16L(name);
        }
    }
}
