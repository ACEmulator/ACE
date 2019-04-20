using System;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipUpdateFellow : GameEventMessage
    {
        public GameEventFellowshipUpdateFellow(Session session, Player player, bool shareLoot, FellowUpdateType fellowUpdateType = FellowUpdateType.Full)
            : base(GameEventType.FellowshipUpdateFellow, GameMessageGroup.UIQueue, session)
        {
            // Information about fellow being added
            Writer.Write(player.Guid.Full);

            // TODO: move this to Fellow network structure
            Writer.Write(0u);           // cpCached - Perhaps cp stored up before distribution?
            Writer.Write(0u);           // lumCached - Perhaps luminance stored up before distribution?
            Writer.Write(player.Level ?? 1);
            Writer.Write(player.Health.MaxValue);
            Writer.Write(player.Stamina.MaxValue);
            Writer.Write(player.Mana.MaxValue);
            Writer.Write(player.Health.Current);
            Writer.Write(player.Stamina.Current);
            Writer.Write(player.Mana.Current);
            Writer.Write(Convert.ToUInt32(shareLoot) << 1);
            Writer.WriteString16L(player.Name);

            Writer.Write((uint)fellowUpdateType);
        }
    }
}
