using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;
using ACE.Server.Entity;

namespace ACE.Server.Network.Structure
{
    public class AllegianceProfile
    {
        public Allegiance Allegiance;
        public AllegianceNode Node;

        public AllegianceProfile(Allegiance allegiance, AllegianceNode node)
        {
            Allegiance = allegiance;
            Node = node;
        }
    }

    public static class AllegianceProfileExtensions
    {
        public static void Write(this BinaryWriter writer, AllegianceProfile profile)
        {
            // uint - totalMembers - The number of allegiance members.
            // uint - totalVassals - Your personal number of followers.
            // AllegianceHeirarchy - allegianceHeirarchy
            uint totalMembers = 0; 
            uint totalVassals = 0;

            if (profile.Allegiance != null)
            {
                // this is actually the total # of followers,
                // so we subtract 1 (the monarch)
                totalMembers = (uint)profile.Allegiance.TotalMembers - 1;
                totalVassals = (uint)profile.Node.TotalVassals;
            }

            writer.Write(totalMembers);
            writer.Write(totalVassals);

            var allegianceHeirarchy = new AllegianceHeirarchy(profile);
            writer.Write(allegianceHeirarchy);
        }
    }
}
