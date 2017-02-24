using System.Collections.Generic;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Common;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterList : GameMessage
    {
        public GameMessageCharacterList(List<CachedCharacter> characters, string account) 
            : base(GameMessageOpcode.CharacterList, 0x9)
        {
            Writer.Write(0u);
            Writer.Write(characters.Count);

            foreach (var character in characters)
            {
                Writer.WriteGuid(character.Guid);
                Writer.WriteString16L(character.Name);
                Writer.Write(character.DeleteTime != 0ul ? (uint)(Time.GetUnixTime() - character.DeleteTime) : 0u);
            }

            Writer.Write(0u);
            Writer.Write(11u /*slotCount*/);
            Writer.WriteString16L(account);
            Writer.Write(1u /*useTurbineChat*/);
            Writer.Write(0u /*hasThroneOfDestiny*/);
        }
    }
}