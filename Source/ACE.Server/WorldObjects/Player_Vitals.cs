using System;
using System.Collections.Generic;
using System.Text;

using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void RaiseVital(Ability ability, uint amount)
        {

        }

        // FIXME(ddevec): Copy pasted code for prototyping -- clean later
        protected override void VitalTickInternal(CreatureVital vital)
        {
            uint oldValue = vital.Current;
            base.VitalTickInternal(vital);

            if (vital.Current != oldValue)
            {
                // FIXME(ddevec): This is uglysauce.  CreatureVital should probably have it, but ACE.Entity doesn't seem to like importing ACE.Network.Enum
                Vital v;
                if (vital == Health)
                {
                    v = Vital.Health;
                }
                else if (vital == Stamina)
                {
                    v = Vital.Stamina;
                }
                else if (vital == Mana)
                {
                    v = Vital.Mana;
                }
                else
                {
                    log.Error("Unknown vital in UpdateVitalInternal: " + vital);
                    return;
                }

                Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(Session, v, vital.Current));
            }
        }

        protected override void UpdateVitalInternal(CreatureVital vital, uint newVal)
        {
            uint oldValue = vital.Current;
            base.UpdateVitalInternal(vital, newVal);

            if (vital.Current != oldValue)
            {
                // FIXME(ddevec): This is uglysauce.  CreatureVital should probably have it, but ACE.Entity doesn't seem to like importing ACE.Network.Enum
                Vital v;
                if (vital == Health)
                {
                    v = Vital.Health;
                }
                else if (vital == Stamina)
                {
                    v = Vital.Stamina;
                }
                else if (vital == Mana)
                {
                    v = Vital.Mana;
                }
                else
                {
                    log.Error("Unknown vital in UpdateVitalInternal: " + vital);
                    return;
                }

                Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(Session, v, vital.Current));
            }
        }

    }
}
