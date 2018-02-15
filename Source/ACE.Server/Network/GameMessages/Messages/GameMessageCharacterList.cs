using System.Collections.Generic;
using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterList : GameMessage
    {
        public GameMessageCharacterList(List<Character> characters, string clientAccountString) : base(GameMessageOpcode.CharacterList, GameMessageGroup.UIQueue)
        {
            // Remove any deleted characters from results
            List<Character> charactersTrimmed = new List<Character>();

            foreach (var character in characters)
            {
                if (!character.IsDeleted)
                    charactersTrimmed.Add(character);
            }

            Writer.Write(0u);
            Writer.Write(charactersTrimmed.Count);

            foreach (var character in charactersTrimmed)
            {
                Writer.WriteGuid(new ObjectGuid(character.BiotaId));
                Writer.WriteString16L(character.Name);
                Writer.Write(character.DeleteTime != 0ul ? (uint)(Time.GetUnixTime() - character.DeleteTime) : 0u);
            }

            Writer.Write(0u);
            Writer.Write(11u /*slotCount*/);
            Writer.WriteString16L(clientAccountString);
            Writer.Write(1u /*useTurbineChat*/);
            Writer.Write(1u /*hasThroneOfDestiny*/);
        }
    }
}
