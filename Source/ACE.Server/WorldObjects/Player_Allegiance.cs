using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity;
using ACE.Server.Managers;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public Allegiance Allegiance;
        public AllegianceNode AllegianceNode;

        public void HandleActionSwearAllegiance(ObjectGuid targetGuid)
        {
            Console.WriteLine("Target: " + targetGuid.Full.ToString("X8"));

            var target = WorldManager.GetPlayerByGuidId(targetGuid.Full);
            if (target == null)
            {
                Console.WriteLine("Couldn't find patron");
                return;
            }
            // exceptions:
            // player can't swear to themselves
            // 2 players can't swear to each other
            // prevent any loops in the allegiance chain
            // player already sworn?
            if (Patron != null)
            {
                Console.WriteLine("Existing patron: " + WorldManager.GetPlayerByGuidId(Patron.Value).Name);
                //return;
            }

            Patron = targetGuid.Full;
            Monarch = AllegianceManager.GetMonarch(target).Guid.Full;

            Console.WriteLine("Patron: " + WorldManager.GetPlayerByGuidId(Patron.Value).Name);
            Console.WriteLine("Monarch: " + WorldManager.GetPlayerByGuidId(Monarch.Value).Name);

            // send message to patron:
            // %vassal% has sworn Allegiance to you.
            target.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has sworn Allegiance to you.", ChatMessageType.Broadcast));

            // send message to vassal:
            // %patron% has accepted your oath of Allegiance!
            // Motion_Kneel
            Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} has accepted your oath of Allegiance!", ChatMessageType.Broadcast));

            var motion = new UniversalMotion(CurrentMotionState.Stance, new MotionItem(MotionCommand.Kneel));
            CurrentLandblock.EnqueueBroadcastMotion(this, motion);

            var allegiance = AllegianceManager.GetAllegiance(this);
        }
    }
}
