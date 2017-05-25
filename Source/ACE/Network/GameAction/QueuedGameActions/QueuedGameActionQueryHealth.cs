using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionQueryHealth : QueuedGameAction
    {
        public QueuedGameActionQueryHealth(uint objectId)
        {
            ObjectId = objectId;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        protected override void Handle(Player player)
        {
            // this may need some help - SO .. I had to mod this function to make it work outside.. it was tied to landblock cs internal functions.
            // todo : test and fix this 

            if (ObjectId == 0)
            {
                // Deselect the formerly selected Target
                player.SelectedTarget = 0;
                return;
            }

            // var obj = LandManager.OpenWorld.ReadOnlyClone(new ObjectGuid(ObjectId));

            object target = null;
            var targetId = new ObjectGuid(ObjectId);

            // Remember the selected Target
            player.SelectedTarget = ObjectId;

            // TODO: once items are implemented check if there are items that can trigger
            //       the QueryHealth event. So far I believe it only gets triggered for players and creatures
            if (targetId.IsPlayer() || targetId.IsCreature())

                if (target != null)
                {
                    float healthPercentage = 0;

                    if (targetId.IsPlayer())
                    {
                        Player tmpTarget = (Player)target;
                        healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
                    }
                    if (targetId.IsCreature())
                    {
                        Creature tmpTarget = (Creature)target;
                        healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
                    }
                    var updateHealth = new GameEventUpdateHealth(player.Session, targetId.Full, healthPercentage);
                    player.Session.Network.EnqueueSend(updateHealth);
                }
            }
        }
    }