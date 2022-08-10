namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Lets players know that a portal storm is brewing "This area is getting too crowded - a Portal Storm is brewing."
    /// </summary>
    public class GameEventPortalStormBrewing : GameEventMessage
    {
        public GameEventPortalStormBrewing(Session session, float extent = 0.4f)
            : base(GameEventType.MiscPortalStormBrewing, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(extent);
        }
    }
}
