using System;
using ACE.Server.Network.Structure;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAllegianceUpdate : GameEventMessage
    {
        /// <summary>
        /// Returns info related to a player's monarch, patron, and vassals.
        /// </summary>
        public GameEventAllegianceUpdate(Session session, Allegiance allegiance, AllegianceNode node)
            : base(GameEventType.AllegianceUpdate, GameMessageGroup.UIQueue, session)
        {
            var startPos = Writer.BaseStream.Position;

            // uint - rank - this player's rank within their allegiance
            // AllegianceProfile - prof
            uint rank = (node == null) ? 0 : node.Rank;
            //Console.WriteLine("Rank: " + rank);
            Writer.Write(rank);

            var prof = new AllegianceProfile(allegiance, node);
            Writer.Write(prof);

            var endPos = Writer.BaseStream.Position;

            var totalBytes = endPos - startPos;
            //Console.WriteLine("Allegiance bytes written: " + totalBytes);
        }
    }
}
