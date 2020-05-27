namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDDDInterrogation : GameMessage
    {
        public GameMessageDDDInterrogation()
            : base(GameMessageOpcode.DDD_Interrogation, GameMessageGroup.DatabaseQueue)
        {
            Writer.Write(1u); // m_dwServersRegion
            Writer.Write(1u); // m_NameRuleLanguage
            Writer.Write(1u); // m_dwProductID
            Writer.Write(2u); // m_SupportedLanguages.Count
                Writer.Write(0u); // Invalid
                Writer.Write(1u); // English
        }
    }
}
