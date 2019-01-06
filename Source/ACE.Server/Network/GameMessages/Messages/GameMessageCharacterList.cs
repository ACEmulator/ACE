using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.Shard;

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
                else if (character.IsPlussed)
                    Writer.WriteString16L("+" + character.Name);
                else
                    Writer.WriteString16L(character.Name);
                Writer.Write(character.DeleteTime != 0ul ? (uint)(Time.GetUnixTime() - character.DeleteTime) : 0u);
            }

            Writer.Write(0u);
            Writer.Write(11u /*slotCount*/);
            Writer.WriteString16L(session.Account);
            Writer.Write(1u /*useTurbineChat*/);
            Writer.Write(1u /*hasThroneOfDestiny*/);
        }
    }
}
