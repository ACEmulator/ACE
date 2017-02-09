using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Friend
    {
        public ObjectGuid Id { get; set; }
        public string Name { get; set; }
        
        public List<ObjectGuid> FriendIdList { get; set; }
        public List<ObjectGuid> FriendOfIdList { get; set; }

        public Friend()
        {
            FriendOfIdList = new List<ObjectGuid>();
            FriendIdList = new List<ObjectGuid>();
        }
    }
}
