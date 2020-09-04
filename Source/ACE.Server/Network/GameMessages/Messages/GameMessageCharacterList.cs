using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterList : GameMessage
    {
        public GameMessageCharacterList(List<Character> characters, Session session) : base(GameMessageOpcode.CharacterList, GameMessageGroup.UIQueue)
        {
            Writer.Write(0u);
            Writer.Write(characters.Count);

            foreach (var character in characters)
            {                
                Writer.Write(character.Id);
                if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && session.AccessLevel > ACE.Entity.Enum.AccessLevel.Advocate)
                    Writer.WriteString16L("+" + character.Name);
                else if (!ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && character.IsPlussed)
                    Writer.WriteString16L("+" + character.Name);
                else
                    Writer.WriteString16L(character.Name);

                // TODO: handle this better for char_delete_time=0
                Writer.Write(character.DeleteTime != 0ul ? (uint)Math.Max(1, Time.GetUnixTime() - character.DeleteTime) : 0u);
            }

            Writer.Write(0u);
            var slotCount = (uint)PropertyManager.GetLong("max_chars_per_account").Item;
            Writer.Write(slotCount);
            Writer.WriteString16L(session.Account);
            var useTurbineChat = Convert.ToUInt32(PropertyManager.GetBool("use_turbine_chat").Item);
            Writer.Write(useTurbineChat);
            Writer.Write(1u /*hasThroneOfDestiny*/);
        }
    }
}
