using System;
using System.IO;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
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
            // uint - bitfield - AllegianceIndex
            // Gender - gender - The gender of the character (for determining title)
            // HeritageGroup - hg - The heritage of the character (for determining title)
            // ushort - rank - The numerical rank (1 is lowest).

            // Choose valid sections by masking against bitfield
            // uint - level

            // ushort - loyalty - Character loyalty
            // ushort - leadership - Character leadership

            // Choose based on testing bitfield == 0x4 (HasAllegianceAge)
            // True:
            // uint - timeOnline
            // uint - allegianceAge
            // False: (Not found in later retail pcaps. Probably deprecated.)
            // ulong - uTimeOnline

            // string - name

            uint characterID = 0;
            uint cpCached = 0;
            uint cpTithed = 0;
            var bitfield = AllegianceIndex.HasAllegianceAge | AllegianceIndex.HasPackedLevel;
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
                var playerGuid = node.PlayerGuid;
                var player = PlayerManager.FindByGuid(playerGuid, out var playerIsOnline);
                
                characterID = player.Guid.Full;
                cpCached = (uint)Math.Min(player.AllegianceXPCached, uint.MaxValue);
                cpTithed = (uint)Math.Min(player.AllegianceXPGenerated, uint.MaxValue);

                if (playerIsOnline)
                    bitfield |= AllegianceIndex.LoggedIn;

                if (!node.IsMonarch && node.Player.ExistedBeforeAllegianceXpChanges)
                    bitfield |= AllegianceIndex.MayPassupExperience;

                gender = (Gender)player.Gender;
                hg = (HeritageGroup)player.Heritage;
                rank = (ushort)node.Rank;
                level = (uint)player.Level;
                loyalty = (ushort)player.GetCurrentLoyalty();
                leadership = (ushort)player.GetCurrentLeadership();
                
                //if (!node.IsMonarch)
                //{
                // TODO: Get/set total time sworn to patron (allegianceAge) and total in-game time since swearing to patron (timeOnline)
                //}

                name = player.Name;
                
            }

            writer.Write(characterID);
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

            if (bitfield.HasFlag(AllegianceIndex.HasAllegianceAge))
            {
                writer.Write(timeOnline);
                writer.Write(allegianceAge);
            }
            else
                writer.Write(uTimeOnline);

            writer.WriteString16L(name);
        }
    }
}
