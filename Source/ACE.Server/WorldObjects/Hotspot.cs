using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
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

        private HashSet<ObjectGuid> Players = new HashSet<ObjectGuid>();

        private ActionChain ActionLoop = null;

        public override void OnCollideObject(WorldObject wo)
        {
            if (!(wo is Player player))
                return;

            if (!Players.Contains(player.Guid))
                Players.Add(player.Guid);

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
            {
                Console.WriteLine($"{wo.Name} ({wo.Guid}).OnCollideObjectEnd({wo.Name})");
                Players.Remove(player.Guid);
            }*/
        }

        private ActionChain NextActionLoop
        {
            get
            {
                ActionLoop = new ActionChain();
                ActionLoop.AddDelaySeconds(CycleTimeNext);
                ActionLoop.AddAction(this, () =>
                {
                    if (Players.Any())
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
                var variance = CycleTime * CycleTimeVariance;
                var min = CycleTime - variance;
                var max = CycleTime + variance;
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
                var p = ThreadSafeRandom.Next(r.MinDamage, r.MaxDamage);
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

        private void Activate()
        {
            foreach (var playerGuid in Players.ToList())
            {
                var player = PlayerManager.GetOnlinePlayer(playerGuid);

                // verify current state of collision here
                if (player == null || !player.PhysicsObj.is_touching(PhysicsObj))
                {
                    Players.Remove(playerGuid);
                    continue;
                }
                Activate(player);
            }
        }

        private void Activate(Player player)
        {
            if (!IsHot) return;

            var amount = DamageNext;
            var iAmount = (int)Math.Round(amount);

            switch (DamageType)
            {
                default:
                    if (player.Invincible) return;
                    amount *= (float)player.GetLifeResistance(DamageType);
                    iAmount = player.TakeDamage(this, DamageType, amount, Server.Entity.BodyPart.Foot);
                    if (player.IsDead && Players.Contains(player.Guid))
                        Players.Remove(player.Guid);
                    break;

                case DamageType.Mana:
                    iAmount = player.UpdateVitalDelta(player.Mana, -iAmount);
                    break;
                case DamageType.Stamina:
                    iAmount = player.UpdateVitalDelta(player.Stamina, -iAmount);
                    break;
                case DamageType.Health:
                    iAmount = player.UpdateVitalDelta(player.Health, -iAmount);
                    break;
            }

            if (!Visibility)
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.TriggerActivated, 1.0f));

            if (!string.IsNullOrWhiteSpace(ActivationTalk) && iAmount != 0)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk.Replace("%i", Math.Abs(iAmount).ToString()), ChatMessageType.Broadcast));

            // perform activation emote
            if (ActivationResponse.HasFlag(ActivationResponse.Emote))
                OnEmote(player);
        }
    }
}
