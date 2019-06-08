using System.IO;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

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
            // AllegianceHierarchy - allegianceHierarchy

            uint totalMembers = 0; 
            uint totalVassals = 0;

            if (profile.Allegiance != null && profile.Node != null)
            {
                totalMembers = (uint)profile.Node.Monarch.TotalFollowers + 1;       // includes monarch
                totalVassals = (uint)profile.Node.TotalFollowers;
            }

            writer.Write(totalMembers);
            writer.Write(totalVassals);

            var allegianceHierarchy = new AllegianceHierarchy(profile);
            writer.Write(allegianceHierarchy);
        }
    }
}
