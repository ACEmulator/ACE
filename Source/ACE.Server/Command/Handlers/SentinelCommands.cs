using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Command.Handlers
{
    public static class SentinelCommands
    {
        // cloak < on / off / player / creature >
        [CommandHandler("cloak", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Sets your cloaking state.",
            "< on / off / player / creature >\n" +
            "This command sets your current cloaking state\n" +
            "< on > You will be completely invisible to players.\n" +
            "< off > You will show up as a normal.\n" +
            "< player > You will appear as a player. (No + and a white radar dot.)\n" +
            "< creature > You will appear as a creature. (No + and an orange radar dot.)")]
        public static void HandleCloak(Session session, params string[] parameters)
        {
            // Please specify if you want cloaking on or off.usage: @cloak < on / off / player / creature >
            // This command sets your current cloaking state.
            // < on > You will be completely invisible to players.
            // < off > You will show up as a normal.
            // < player > You will appear as a player. (No + and a white radar dot.)
            // < creature > You will appear as a creature. (No + and an orange radar dot.)
            // @cloak - Sets your cloaking state.

            // TODO: investigate translucensy/visbility of other cloaked admins.

            switch (parameters?[0].ToLower())
            {
                case "off":
                    session.Player.Cloaked = false;
                    session.Player.Ethereal = false;
                    // session.Player.IgnoreCollisions = false;
                    session.Player.NoDraw = false;
                    // session.Player.ReportCollisions = true;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Translucency = null;
                    session.Player.Visibility = false;
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.Off);

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                case "on":
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.On);
                    session.Player.Cloaked = true;
                    session.Player.Ethereal = true;
                    // session.Player.IgnoreCollisions = true;
                    session.Player.NoDraw = true;
                    // session.Player.ReportCollisions = false;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Visibility = true;
                    session.Player.Translucency = 0.5f;

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Network.EnqueueSend(new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                case "player":
                    session.Player.Cloaked = false;
                    session.Player.Ethereal = false;
                    // session.Player.IgnoreCollisions = false;
                    session.Player.NoDraw = false;
                    // session.Player.ReportCollisions = true;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Translucency = null;
                    session.Player.Visibility = false;
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.Player);

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                case "creature":
                    session.Player.Cloaked = false;
                    session.Player.Ethereal = false;
                    // session.Player.IgnoreCollisions = false;
                    session.Player.NoDraw = false;
                    // session.Player.ReportCollisions = true;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Translucency = null;
                    session.Player.Visibility = false;
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.Creature);

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                default:
                    session.Network.EnqueueSend(new GameMessageSystemChat("Please specify if you want cloaking on or off.", ChatMessageType.Broadcast));
                    break;
            }
        }
    }
}
