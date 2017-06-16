using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, uint objectID)
            : base(GameEventType.ApproachVendor, GameMessageGroup.Group09, session)
        {        
            // todo: turn this into a real vendor that reads from a database

            Writer.Write(objectID); // merchant id

            // bit mask ? categories / mask may need figured out more.
            // what will the vendor offer to buy
            ObjectDescriptionFlag buycatgegories;
            buycatgegories = ObjectDescriptionFlag.Food;
            Writer.Write((uint)buycatgegories); 

            Writer.Write((uint)0); // min_value
            Writer.Write((uint)0); // max_value
            Writer.Write((uint)1); // magic
            Writer.Write((float)0.8); // buy_price
            Writer.Write((float)10); // sell_price
            Writer.Write((uint)0); // trade id .. is this a timestamp type val?
            Writer.Write((uint)0); // trade number .. is this a timestamp type val?
            Writer.WriteString16L("");

            int itemCount = 2;
            Writer.Write((uint)itemCount); // number of items

            // Write the item(s)
            Writer.Write((uint)0xFFFFFFFF); // first byte, if non-zero, tells it to use oldPublicWeenieDesc. We've only got (new)PublicWeenieDesc in gamedata
            Writer.Write((uint)5); // this should be the iid. I made this one up.
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
            apple.Value = 10;
            apple.ContainerId = objectID; // because the vendor has it!
            apple.Usable = Enum.Usable.UsableNo;
            apple.StackSize = 1;
            apple.MaxStackSize = 1;
            apple.CombatUse = Enum.CombatUse.MissleWeapon;
            apple.ValidEquipLocations = (Enum.EquipMask)4194304;
            apple.HookType = (ushort)ObjectType.Armor;
            apple.Burden = 50;
            apple.Serialize(Writer);
            Writer.Align();

            Writer.Write((uint)0xFFFFFFFF); // first byte, if non-zero, tells it to use oldPublicWeenieDesc. We've only got (new)PublicWeenieDesc in gamedata
            Writer.Write((uint)10); // this should be the iid. I made this one up.
            GameDataTest applepie = new GameDataTest(ObjectType.Food, "Stacks Famous Pie", 4709, 100669942);
            applepie.WeenieFlags = Enum.WeenieHeaderFlag.Value |
                                Enum.WeenieHeaderFlag.Container |
                                Enum.WeenieHeaderFlag.StackSize |
                                Enum.WeenieHeaderFlag.MaxStackSize |
                                Enum.WeenieHeaderFlag.CombatUse |
                                Enum.WeenieHeaderFlag.ValidLocations |
                                Enum.WeenieHeaderFlag.HookType |
                                Enum.WeenieHeaderFlag.Burden |
                                Enum.WeenieHeaderFlag.Usable;
            applepie.Value = 1;
            applepie.ContainerId = objectID; // because the vendor has it!
            applepie.Usable = Enum.Usable.UsableNo;
            applepie.StackSize = 1;
            applepie.MaxStackSize = 1;
            applepie.CombatUse = Enum.CombatUse.MissleWeapon;
            applepie.ValidEquipLocations = (Enum.EquipMask)4194304;
            applepie.HookType = (ushort)ObjectType.Armor;
            applepie.Burden = 50;
            applepie.Serialize(Writer);
            Writer.Align();
        }
    }
}
