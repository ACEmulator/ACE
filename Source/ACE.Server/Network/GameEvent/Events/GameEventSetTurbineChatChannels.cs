using ACE.Server.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSetTurbineChatChannels : GameEventMessage
    {
        /// <summary>
        /// societyCelestialHand, societyEldrytchWeb and societyRadiantBlood do not appear to be used by the latest client.
        /// </summary>
        public GameEventSetTurbineChatChannels(Session session, uint allegiance)
            : base(GameEventType.SetTurbineChatChannels, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(allegiance);
            Writer.Write(TurbineChatChannel.General);
            Writer.Write(TurbineChatChannel.Trade);
            Writer.Write(TurbineChatChannel.LFG);
            Writer.Write(TurbineChatChannel.Roleplay);
            Writer.Write(TurbineChatChannel.Olthoi);
            Writer.Write(TurbineChatChannel.Society);
            Writer.Write(TurbineChatChannel.SocietyCelestialHand);
            Writer.Write(TurbineChatChannel.SocietyEldrytchWeb);
            Writer.Write(TurbineChatChannel.SocietyRadiantBlood);
        }
    }
}
