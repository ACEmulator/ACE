namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterEnterWorldServerReady : GameMessage
    {
        public GameMessageCharacterEnterWorldServerReady()
            : base(GameMessageOpcode.CharacterEnterWorldServerReady, GameMessageGroup.UIQueue)
        {
        }
    }
}
