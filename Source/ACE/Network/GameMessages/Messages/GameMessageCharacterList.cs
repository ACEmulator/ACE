using System.Collections.Generic;

using ACE.Entity;
using ACE.Common;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterList : GameMessage
    {
        public GameMessageCharacterList(List<CachedCharacter> characters, string account) : base(GameMessageOpcode.CharacterList, GameMessageGroup.Group09)
        {
            // Remove any deleted characters from results
            List<CachedCharacter> charactersTrimmed = new List<CachedCharacter>();

            foreach (var character in characters)
            {
                if (!character.Deleted)
                    charactersTrimmed.Add(character);
            }

            Writer.Write(0u);
            Writer.Write(charactersTrimmed.Count);

            foreach (var character in charactersTrimmed)
            {
                Writer.WriteGuid(character.Guid);
                Writer.WriteString16L(character.Name);
                Writer.Write(character.DeleteTime != 0ul ? (uint)(Time.GetUnixTime() - character.DeleteTime) : 0u);
            }

            Writer.Write(0u);
            Writer.Write(11u /*slotCount*/);
            Writer.WriteString16L(account);
            Writer.Write(1u /*useTurbineChat*/);
            Writer.Write(1u /*hasThroneOfDestiny*/);
        }
    }
}