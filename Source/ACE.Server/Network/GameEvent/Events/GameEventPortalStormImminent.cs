namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Lets players know that a portal storm is brewing "This area is getting too crowded - a Portal Storm is brewing."
    /// </summary>
    public class GameEventPortalStormImminent : GameEventMessage
    {
        public GameEventPortalStormImminent(Session session, float extent = 0.6f)
            : base(GameEventType.MiscPortalStormImminent, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(extent);
        }
    }
}
