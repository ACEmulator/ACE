using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionUseItem
    {
        [GameAction(GameActionType.EvtInventoryUseEvent)]
        public static void Handle(ClientMessage message, Session session)
        {
            // this is specifically for lifestones:
            // get the object id
            uint fullId = message.Payload.ReadUInt32();
            var objectId = new ObjectGuid(fullId);

            // get the worldobject from the id, if it exists
            AceObject landBlockObject = DatabaseManager.World.GetObjectsByLandblock(session.Player.Location.LandblockId.Landblock).Find(o => o.AceObjectId == objectId.Full);

            // float provided by OptimShi:
            float lifestoneUseRadius = 4.25395441f;

            string serverMessage = "You wandered too far to attune with the lifestone!";

            // TODO: Add adjacent landblock check or Fix the X/Y coordinates for a few lifestones.
            if (landBlockObject != null) {
                foreach (uint lsType in System.Enum.GetValues(typeof(LifestoneWeenieClasses)))
                {
                    if (landBlockObject.WeenieClassId == lsType)
                    {
                        float dx = Math.Abs(landBlockObject.Position.PositionX - session.Player.Location.PositionX);
                        float dy = Math.Abs(landBlockObject.Position.PositionY - session.Player.Location.PositionY);
                        float dz = Math.Abs(landBlockObject.Position.PositionZ - session.Player.Location.PositionZ);

                        // check if the distance is greater then
                        if (dx > lifestoneUseRadius) break;
                        if (dy > lifestoneUseRadius) break;
                        if (dz > lifestoneUseRadius) break;

                        Console.WriteLine("dx :" + dx.ToString() + " dy: " + dy.ToString() + " dz: " + dz.ToString());
                        Console.WriteLine("posx :" + session.Player.Location.PositionX.ToString() + " posy: " + session.Player.Location.PositionY.ToString() + " posz: " + session.Player.Location.PositionZ.ToString());
                        Console.WriteLine("rotx :" + session.Player.Location.RotationX.ToString() + " roty: " + session.Player.Location.RotationY.ToString() + " rotz: " + session.Player.Location.RotationZ.ToString() + " rotw: " + session.Player.Location.RotationW.ToString());

                        // bind the lifestone
                        session.Player.SetCharacterPosition(Entity.Enum.PositionType.Sanctuary, session.Player.Location);
                        serverMessage = "You have attuned your spirit to this lifestone. You will ressurect here after you die.";
                        break;
                    }
                }
            }
            // create the outbound server message
            var lifestoneBindMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.Broadcast);

            // always send useDone event
            var sendUseDoneEvent = new GameEventUseDone(session);
            session.Network.EnqueueSend(lifestoneBindMessage, sendUseDoneEvent);
        }
    }
}
