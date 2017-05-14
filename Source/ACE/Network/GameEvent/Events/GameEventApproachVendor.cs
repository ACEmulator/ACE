using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, WorldObject worldObject)
            : base(GameEventType.ApproachVendor, GameMessageGroup.Group09, session)
        {        
            Writer.WriteGuid(worldObject.Guid); // merchant id

            // Pack Categories Food & Trade Notes ?
            uint buycatgegories = 17291; 
            Writer.Write(buycatgegories); // item_types, not really relevant for selling items here. It's for what they will buy

            Writer.Write((uint)0); // min_value
            Writer.Write((uint)0); // max_value
            Writer.Write((uint)1); // magic
            Writer.Write((float)0.8); // buy_price
            Writer.Write((float)10); // sell_price
            Writer.Write((uint)0); // trade id .. is this a timestamp type val?
            Writer.Write((uint)0); // trade number .. is this a timestamp type val?
            Writer.WriteString16L("");

            int itemCount = 1;
            Writer.Write((uint)itemCount); // number of items

            // Write the item(s)
            Writer.Write((uint)0xFFFFFFFF); // first byte, if non-zero, tells it to use oldPublicWeenieDesc. We've only got (new)PublicWeenieDesc in gamedata
            Writer.Write((uint)0x889cab5e); // this should be the guid. I made this one up.
            GameDataTest apple = new GameDataTest(ObjectType.Food, "Apple", 258, 100667465);
            apple.WeenieFlags = Enum.WeenieHeaderFlag.Value | 
                                Enum.WeenieHeaderFlag.Container | 
                                Enum.WeenieHeaderFlag.StackSize |
                                Enum.WeenieHeaderFlag.MaxStackSize |
                                Enum.WeenieHeaderFlag.CombatUse |
                                Enum.WeenieHeaderFlag.ValidLocations |
                                Enum.WeenieHeaderFlag.HookType |
                                Enum.WeenieHeaderFlag.Burden | 
                                Enum.WeenieHeaderFlag.Usable;
            apple.Value = 1;
            apple.ContainerId = worldObject.Guid.Full; // because the vendor has it!
            apple.Usable = Enum.Usable.UsableNo;
            apple.StackSize = 1;
            apple.MaxStackSize = 100;
            apple.CombatUse = Enum.CombatUse.MissleWeapon;
            apple.ValidEquipLocations = (Enum.EquipMask)4194304;
            apple.HookType = (ushort)ObjectType.Armor;
            apple.Burden = 50;
            apple.Serialize(Writer);
            Writer.Align();
        }
    }
}
