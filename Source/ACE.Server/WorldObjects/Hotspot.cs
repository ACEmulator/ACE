using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Hotspot : WorldObject
    {
        public Hotspot(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        public Hotspot(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // If CycleTime is less than 1, player has a very bad time.
            if ((CycleTime ?? 0) < 1)
                CycleTime = 1;
        }

        private HashSet<ObjectGuid> Creatures = new HashSet<ObjectGuid>();

        private ActionChain ActionLoop = null;

        public override void OnCollideObject(WorldObject wo)
        {
            if (!(wo is Creature creature))
                return;

            if (!AffectsAis && !(wo is Player))
                return;

            if (!Creatures.Contains(creature.Guid))
            {
                //Console.WriteLine($"{Name} ({Guid}).OnCollideObject({creature.Name})");
                Creatures.Add(creature.Guid);
            }

            if (ActionLoop == null)
            {
                ActionLoop = NextActionLoop;
                NextActionLoop.EnqueueChain();
            }
        }

        public override void OnCollideObjectEnd(WorldObject wo)
        {
            /*if (!(wo is Player player))
                return;

            if (Players.Contains(player.Guid))
                Players.Remove(player.Guid);*/
        }

        private ActionChain NextActionLoop
        {
            get
            {
                ActionLoop = new ActionChain();
                ActionLoop.AddDelaySeconds(CycleTimeNext);
                ActionLoop.AddAction(this, () =>
                {
                    if (Creatures.Any())
                    {
                        Activate();
                        NextActionLoop.EnqueueChain();
                    }
                    else
                    {
                        ActionLoop = null;
                    }
                });
                return ActionLoop;
            }
        }

        private double CycleTimeNext
        {
            get
            {
                var max = CycleTime;
                var min = max * (1.0f - CycleTimeVariance ?? 0.0f);

                return ThreadSafeRandom.Next((float)min, (float)max);
            }
        }

        public double? CycleTime
        {
            get => GetProperty(PropertyFloat.HotspotCycleTime);
            set { if (value == null) RemoveProperty(PropertyFloat.HotspotCycleTime); else SetProperty(PropertyFloat.HotspotCycleTime, (double)value); }
        }

        public double? CycleTimeVariance
        {
            get => GetProperty(PropertyFloat.HotspotCycleTimeVariance) ?? 0;
            set { if (value == null) RemoveProperty(PropertyFloat.HotspotCycleTimeVariance); else SetProperty(PropertyFloat.HotspotCycleTimeVariance, (double)value); }
        }

        private float DamageNext
        {
            get
            {
                var r = GetBaseDamage();
                var p = (float)ThreadSafeRandom.Next(r.MinDamage, r.MaxDamage);
                return p;
            }
        }

        private int? _DamageType
        {
            get => GetProperty(PropertyInt.DamageType);
            set { if (value == null) RemoveProperty(PropertyInt.DamageType); else SetProperty(PropertyInt.DamageType, (int)value); }
        }

        public DamageType DamageType
        {
            get { return (DamageType)_DamageType; }
        }

        public bool IsHot
        {
            get => GetProperty(PropertyBool.IsHot) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsHot); else SetProperty(PropertyBool.IsHot, value); }
        }

        public bool AffectsAis
        {
            get => GetProperty(PropertyBool.AffectsAis) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AffectsAis); else SetProperty(PropertyBool.AffectsAis, value); }
        }

        private void Activate()
        {
            foreach (var creatureGuid in Creatures.ToList())
            {
                var creature = CurrentLandblock.GetObject(creatureGuid) as Creature;

                // verify current state of collision here
                if (creature == null || !creature.PhysicsObj.is_touching(PhysicsObj))
                {
                    //Console.WriteLine($"{Name} ({Guid}).OnCollideObjectEnd({creature?.Name})");
                    Creatures.Remove(creatureGuid);
                    continue;
                }
                Activate(creature);
            }
        }

        private void Activate(Creature creature)
        {
            if (!IsHot) return;

            var amount = DamageNext;
            var iAmount = (int)Math.Round(amount);

            var player = creature as Player;

            switch (DamageType)
            {
                default:

                    if (creature.Invincible) return;

                    amount *= creature.GetResistanceMod(DamageType, this, null);

                    if (player != null)
                        iAmount = player.TakeDamage(this, DamageType, amount, Server.Entity.BodyPart.Foot);
                    else
                        iAmount = (int)creature.TakeDamage(this, DamageType, amount);

                    if (creature.IsDead && Creatures.Contains(creature.Guid))
                        Creatures.Remove(creature.Guid);

                    break;

                case DamageType.Mana:
                    iAmount = creature.UpdateVitalDelta(creature.Mana, -iAmount);
                    break;

                case DamageType.Stamina:
                    iAmount = creature.UpdateVitalDelta(creature.Stamina, -iAmount);
                    break;

                case DamageType.Health:
                    iAmount = creature.UpdateVitalDelta(creature.Health, -iAmount);

                    if (iAmount > 0)
                        creature.DamageHistory.OnHeal((uint)iAmount);
                    else
                        creature.DamageHistory.Add(this, DamageType.Health, (uint)-iAmount);

                    break;
            }

            if (!Visibility)
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.TriggerActivated, 1.0f));

            if (player != null && !string.IsNullOrWhiteSpace(ActivationTalk) && iAmount != 0)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk.Replace("%i", Math.Abs(iAmount).ToString()), ChatMessageType.Broadcast));

            // perform activation emote
            if (ActivationResponse.HasFlag(ActivationResponse.Emote))
                OnEmote(creature);
        }
    }
}
