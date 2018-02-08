using ACE.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// interface for mocking so that other stuff can be tested.  if you need something from
    /// a player when writing unit tests, put it here and mock the interface instead
    /// </summary>
    public interface IPlayer
    {
        WorldObject GetInventoryItem(ObjectGuid objectGuid);

        WorldObject AddNewItemToInventory(uint weenieClassId);

        void AddToInventoryEx(WorldObject inventoryItem, int placement = 0);
    }
}
