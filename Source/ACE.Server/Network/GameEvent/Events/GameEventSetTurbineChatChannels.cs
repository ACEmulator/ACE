namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSetTurbineChatChannels : GameEventMessage
    {
        /// <summary>
        /// societyCelestialHand, societyEldrytchWeb and societyRadiantBlood do not appear to be used by the latest client.
        /// </summary>
        public GameEventSetTurbineChatChannels(Session session, uint allegiance, uint general, uint trade, uint lfg, uint rolePlay, uint olthoi, uint society, uint societyCelestialHand, uint societyEldrytchWeb, uint societyRadiantBlood)
            : base(GameEventType.SetTurbineChatChannels, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(allegiance);
            Writer.Write(general);
            Writer.Write(trade);
            Writer.Write(lfg);
            Writer.Write(rolePlay);
            Writer.Write(olthoi);
            Writer.Write(society);
            Writer.Write(societyCelestialHand);
            Writer.Write(societyEldrytchWeb);
            Writer.Write(societyRadiantBlood);
        }
    }
}
