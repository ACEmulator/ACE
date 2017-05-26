using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Network.GameMessages;

namespace ACE.Entity
{
    /// <summary>
    /// Facilitates lazy broadcast list population within landblock
    /// FIXME(ddevec): This is a hacky quick-fix to make broadcasting both efficient and object-centric (instead of landblock-centric)
    /// The lists are managed by the landblock, and not updated during this portion of the run?
    ///   -- This may not be true when we have cross landblock dependencies - we'll have to reason about proper procedure then
    /// </summary>
    public class LazyBroadcastList
    {
        private List<Player> allPlayers;
        private List<Player> broadcast;
        private ObjectGuid sender;
        
        public LazyBroadcastList(ObjectGuid sender, List<Player> allplayers)
        {
            this.allPlayers = allplayers;
            this.broadcast = null;
            this.sender = sender;
        }

        public ICollection<Player> GetBroadcasters(params GameMessage[] msgs)
        {
            if (broadcast == null)
            {
                broadcast = allPlayers.Where(p => p.Guid != sender).ToList();
            }
            return broadcast;
        }
    }
}
