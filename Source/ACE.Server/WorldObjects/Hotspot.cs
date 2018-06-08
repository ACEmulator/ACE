using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public override void OnCollideObjectEnd(WorldObject wo)
        {
            var player = wo as Player;
            if (player == null) return;
            if (Players.Any(k => k.Equals(player)))
                Players.Remove(player);
        }
        private List<Player> Players = new List<Player>();
        private ActionChain ActionLoop = null;
        public override void OnCollideObject(WorldObject wo)
        {
            var player = wo as Player;
            if (player == null) return;
            if (!Players.Any(k => k.Equals(player)))
                Players.Add(player);
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
        private float DamageNext
        {
            get
            {
                var r = GetBaseDamage();
                var p = Physics.Common.Random.RollDice(r.Min, r.Max);
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
        private void Activate()
        {
            Players.ForEach(player =>
            {
                Activate(player);
            });
        }
        private void Activate(Player plr)
        {
            var amount = DamageNext;
            switch (DamageType)
            {
                case DamageType.Mana:
                    var manaDmg = amount;
                    var result = plr.Mana.Current - manaDmg;
                    if (result < 0 && manaDmg > 0)
                        manaDmg += result;
                    else if (result > plr.Mana.MaxValue && manaDmg < 0)
                        manaDmg -= plr.Mana.MaxValue - result;
                    if (manaDmg != 0)
                        plr.UpdateVital(plr.Mana, plr.Mana.Current - (uint)Math.Round(manaDmg));
                    break;
                default:
                    amount = (float)plr.GetLifeResistance(DamageType) * amount;
                    plr.TakeDamage(this, DamageType, amount, Server.Entity.BodyPart.Foot);
                    break;
            }
        }
    }
}
