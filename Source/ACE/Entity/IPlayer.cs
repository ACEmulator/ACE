using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// interface for mocking so that other stuff can be tested.  if you need something from
    /// a player when writing unit tests, put it here and mock the interface instead
    /// </summary>
    public interface IPlayer
    {
        WorldObject GetInventoryItem(ObjectGuid objectGuid);
    }
}
