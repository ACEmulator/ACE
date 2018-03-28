namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterDelete : GameMessage
    {
        public GameMessageCharacterDelete()
            : base(GameMessageOpcode.CharacterDelete, GameMessageGroup.UIQueue)
        {
        }
    }
}
