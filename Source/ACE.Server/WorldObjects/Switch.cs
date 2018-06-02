using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages.Messages;
using System.Collections.Generic;

namespace ACE.Server.WorldObjects
{
    public class Switch : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Switch(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Switch(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }
        public double? ResetTime
        {
            get => GetProperty(PropertyFloat.ResetInterval);
            set { if (value == null) RemoveProperty(PropertyFloat.ResetInterval); else SetProperty(PropertyFloat.ResetInterval, (double)value); }
        }
        public string ActivationFailiure
        {
            get => GetProperty(PropertyString.ActivationFailure);
            set { if (value == null) RemoveProperty(PropertyString.ActivationFailure); else SetProperty(PropertyString.ActivationFailure, value); }
        }

        Dictionary<ObjectGuid, double> HitTimes = new Dictionary<ObjectGuid, double>();

        private void pruneHits()
        {
            var currentTime = Timer.CurrentTime;
            List<ObjectGuid> resets = new List<ObjectGuid>();
            foreach (var hit in HitTimes.Keys)
                if (currentTime - HitTimes[hit] > ResetTime)
                    resets.Add(hit);
            resets.ForEach(k => HitTimes.Remove(k));
        }

        public override void OnCollideObject(WorldObject wo)
        {
            var player = wo as Player;
            if (player == null) return;

            if (ResetTime.HasValue && ResetTime > 0)
            {
                pruneHits();
                if (HitTimes.ContainsKey(player.Guid))
                    return;
                HitTimes.Add(player.Guid, Timer.CurrentTime);
            }

            //spell traps
            if (Spell.HasValue && SpellDID.HasValue)
            {
                CurrentLandblock.EnqueueBroadcastSound(player, Sound.TriggerActivated);
                var spellTable = DatManager.PortalDat.SpellTable;
                if (!spellTable.Spells.ContainsKey((uint)SpellDID)) return;
                var spellBase = DatManager.PortalDat.SpellTable.Spells[(uint)SpellDID];
                var spell = DatabaseManager.World.GetCachedSpell((uint)SpellDID);
                var msg = string.Empty;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateInstanceID(player, PropertyInstanceId.CurrentAttacker, this.Guid.Full));
                // TODO: Perform a spell resist check. Cast spell on resist failure or output ActivationFailiure text upon resisting the spell
                LifeMagic(player, spellBase, spell, out msg);
                player.PlayParticleEffect((PlayScript)spellBase.TargetEffect, player.Guid);
            }
        }
    }
}
