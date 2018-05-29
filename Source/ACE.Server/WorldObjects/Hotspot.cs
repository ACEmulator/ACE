using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
//using System.Linq;

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
        }
        public override void HandleActionOnCollideEnd(ObjectGuid playerId)
        {
            if (!playerId.IsPlayer()) return;
            if (Players.Any(k => k == playerId))
                Players.Remove(playerId);
        }
        private List<ObjectGuid> Players = new List<ObjectGuid>();
        private ActionChain ActionLoop = null;
        public override void HandleActionOnCollide(ObjectGuid playerId)
        {
            if (!playerId.IsPlayer()) return;
            if (!Players.Any(k => k == playerId))
                Players.Add(playerId);

            if (ActionLoop == null)
            {
                ActionLoop = NextActionLoop;
                NextActionLoop.EnqueueChain();
            }
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

        public string ActivationTalkString
        {
            get => GetProperty(PropertyString.ActivationTalk);
            set { if (value == null) RemoveProperty(PropertyString.ActivationTalk); else SetProperty(PropertyString.ActivationTalk, value); }
        }

        private double CycleTimeNext
        {
            get
            {
                var variance = CycleTime * CycleTimeVariance;
                var min = CycleTime - variance;
                var max = CycleTime + variance;
                return (double)Physics.Common.Random.RollDice((float)min, (float)max);
            }
        }
        public double? CycleTime
        {
            get => GetProperty(PropertyFloat.HotspotCycleTime);
            set { if (value == null) RemoveProperty(PropertyFloat.HotspotCycleTime); else SetProperty(PropertyFloat.HotspotCycleTime, (double)value); }
        }
        public double? CycleTimeVariance
        {
            get => GetProperty(PropertyFloat.HotspotCycleTimeVariance);
            set { if (value == null) RemoveProperty(PropertyFloat.HotspotCycleTimeVariance); else SetProperty(PropertyFloat.HotspotCycleTimeVariance, (double)value); }
        }
        private int DamageNext
        {
            get
            {
                var r = Math.Abs((int)Damage);
                double variance = r * (double)DamageVariance;
                var min = Math.Floor(r - variance);
                var max = Math.Ceiling(r + variance);
                var p = Physics.Common.Random.RollDice((int)min, (int)max);
                return ((int)Damage < 0) ? p * -1 : p;
            }
        }
        public double? DamageVariance
        {
            get => GetProperty(PropertyFloat.DamageVariance);
            set { if (value == null) RemoveProperty(PropertyFloat.DamageVariance); else SetProperty(PropertyFloat.DamageVariance, (double)value); }
        }
        public int? Damage
        {
            get => GetProperty(PropertyInt.Damage);
            set { if (value == null) RemoveProperty(PropertyInt.Damage); else SetProperty(PropertyInt.Damage, (int)value); }
        }
        public int? _DamageType
        {
            get => GetProperty(PropertyInt.DamageType);
            set { if (value == null) RemoveProperty(PropertyInt.DamageType); else SetProperty(PropertyInt.DamageType, (int)value); }
        }
        private void Activate()
        {
            Players.ForEach(plrId =>
            {
                if (!Players.Any(k => k == plrId)) return;
                var player = CurrentLandblock.GetObject(plrId) as Player;
                if (player == null) return;
                Activate(player);
            });
        }
        private void Activate(Player plr)
        {
            var amount = DamageNext;
            switch ((DamageType)_DamageType)
            {
                case DamageType.Mana:
                    if (plr.Mana.Current >= plr.Mana.MaxValue) return;
                    amount = (int)Math.Max(plr.Mana.Current + amount - plr.Mana.MaxValue, amount); // to-do: implement Player properties ManaMod using enchantment manager
                    if (amount == 0) break;
                    if (amount > 0) break; // to-do: top it off
                    plr.Mana.Current -= (uint)amount;
                    var msg = new GameMessagePrivateUpdateAttribute2ndLevel(plr, Vital.Mana, plr.Mana.Current);
                    break;
                default:
                    amount = (int)Math.Ceiling(plr.GetLifeResistance((DamageType)_DamageType) * amount);
                    plr.TakeDamage(this, (DamageType)_DamageType, amount, Server.Entity.BodyPart.Foot, false);
                    break;
            }
            if (amount != 0 && !string.IsNullOrWhiteSpace(ActivationTalkString))
                plr.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalkString.Replace("%i", Math.Abs((int)amount).ToString()), ChatMessageType.Craft));
        }

    }
}
