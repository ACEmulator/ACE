using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common.Extensions;

namespace ACE.Entity
{
    public class Fellowship
    {
        public string FellowshipName;
        public uint FellowshipLeaderGuid;

        public bool ShareXP; // XP sharing: 0=no, 1=yes
        public bool Open; // open fellowship: 0=no, 1=yes

        public List<Player> FellowshipMembers;

        public Fellowship(Player leader, string fellowshipName, bool shareXP)
        {
            this.ShareXP = shareXP;
            this.FellowshipLeaderGuid = leader.Guid.Full;
            this.FellowshipName = fellowshipName;

            this.FellowshipMembers = new List<Player> { leader };

            this.Open = false;
        }

        public void AddFellowshipMember()
        {
            // todo
        }

        public void RemoveFellowshipMember()
        {
            // todo
        }

        public void QuitFellowship()
        {
            // todo
        }

        public void DisbandFellowship()
        {
            // todo
        }
    }
}
