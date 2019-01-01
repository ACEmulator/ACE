using System;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Physics.Common;

using Position = ACE.Entity.Position;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAdvocateTeleport
    {
        [GameAction(GameActionType.AdvocateTeleport)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadString16L();
            var position = new Position(message.Payload);
            // this check is also done clientside, see: PlayerDesc::PlayerIsPSR
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            //Console.WriteLine($"Handle minimap teleport");
            //Console.WriteLine($"Client sent position: {position}");

            // Check if water block
            var landblock = LScape.get_landblock(position.LandblockId.Raw);
            if (landblock.WaterType == LandDefs.WaterType.EntirelyWater)
            {
                ChatPacket.SendServerMessage(session, $"Landblock 0x{position.LandblockId.Landblock:X4} is entirely filled with water, and is impassable", ChatMessageType.Broadcast);
                return;
            }

            // update z / indoor cell
            position.AdjustMapCoords();

            ChatPacket.SendServerMessage(session, $"Teleporting to: ({position.GetMapCoordStr()})", ChatMessageType.Broadcast);
            session.Player.Teleport(position);
        }
    }
}
