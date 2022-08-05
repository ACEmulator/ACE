/// <summary>
/// Lets players know that a portal storm is brewing "This area is getting too crowded - a Portal Storm is brewing."
/// </summary>
namespace ACE.Server.Network.GameEvent.Events
{
    class GameEventPortalStormSubsided : GameEventMessage
    {
        public GameEventPortalStormSubsided(Session session)
            : base(GameEventType.MiscPortalstormSubsided, GameMessageGroup.UIQueue, session)
        {
        }
    }
}
