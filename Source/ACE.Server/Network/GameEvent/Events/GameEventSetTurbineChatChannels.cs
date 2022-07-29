using ACE.Server.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSetTurbineChatChannels : GameEventMessage
    {
        public GameEventSetTurbineChatChannels(Session session, uint allegiance = 0, uint society = 0)
            : base(GameEventType.SetTurbineChatChannels, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(allegiance);
            Writer.Write(TurbineChatChannel.General);
            Writer.Write(TurbineChatChannel.Trade);
            Writer.Write(TurbineChatChannel.LFG);
            Writer.Write(TurbineChatChannel.Roleplay);
            Writer.Write(TurbineChatChannel.Olthoi);
            Writer.Write(society);
            Writer.Write(TurbineChatChannel.SocietyCelestialHand);
            Writer.Write(TurbineChatChannel.SocietyEldrytchWeb);
            Writer.Write(TurbineChatChannel.SocietyRadiantBlood);
        }
    }
}
