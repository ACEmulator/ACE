using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;

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

        //public double? ResetTime
        //{
        //    get => GetProperty(PropertyFloat.ResetInterval);
        //    set { if (value == null) RemoveProperty(PropertyFloat.ResetInterval); else SetProperty(PropertyFloat.ResetInterval, (double)value); }
        //}

        //public string ActivationFailiure
        //{
        //    get => GetProperty(PropertyString.ActivationFailure);
        //    set { if (value == null) RemoveProperty(PropertyString.ActivationFailure); else SetProperty(PropertyString.ActivationFailure, value); }
        //}

        //Dictionary<ObjectGuid, double> HitTimes = new Dictionary<ObjectGuid, double>();

        //private void pruneHits()
        //{
        //    var currentTime = Timer.CurrentTime;
        //    List<ObjectGuid> resets = new List<ObjectGuid>();
        //    foreach (var hit in HitTimes.Keys)
        //        if (currentTime - HitTimes[hit] > ResetTime)
        //            resets.Add(hit);
        //    resets.ForEach(k => HitTimes.Remove(k));
        //}

        //public override void HandleActionOnCollide(ObjectGuid playerId)
        //{
        //    if (!playerId.IsPlayer()) return;
        //    var player = CurrentLandblock?.GetObject(playerId) as Player;
        //    if (player == null) return;

        //    if (ResetTime.HasValue && ResetTime > 0)
        //    {
        //        pruneHits();
        //        if (HitTimes.ContainsKey(playerId))
        //            return;
        //        HitTimes.Add(playerId, Timer.CurrentTime);
        //    }

        //    //spell traps
        //    if (Spell.HasValue && SpellDID.HasValue)
        //    {
        //        CurrentLandblock?.EnqueueBroadcastSound(player, Sound.TriggerActivated);
        //        var spellTable = DatManager.PortalDat.SpellTable;
        //        if (!spellTable.Spells.ContainsKey((uint)SpellDID)) return;
        //        var spellBase = DatManager.PortalDat.SpellTable.Spells[(uint)SpellDID];
        //        var spell = DatabaseManager.World.GetCachedSpell((uint)SpellDID);
        //        var msg = string.Empty;
        //        player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateInstanceID(player, PropertyInstanceId.CurrentAttacker, this.Guid.Full));
        //        // TODO: Perform a spell resist check. Cast spell on resist failure or output ActivationFailiure text upon resisting the spell
        //        LifeMagic(player, spellBase, spell, out msg);
        //        player.PlayParticleEffect((PlayScript)spellBase.TargetEffect, player.Guid);
        //    }
        //}

        public uint? UseTargetAnimation
        {
            get => GetProperty(PropertyDataId.UseTargetAnimation);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.UseTargetAnimation); else SetProperty(PropertyDataId.UseTargetAnimation, value.Value); }
        }

        /// <summary>
        /// Called when a player or creature uses a switch
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            var creature = worldObject as Creature;
            if (creature == null) return;

            var actionChain = new ActionChain();

            // creature rotates towards switch
            var rotateTime = creature.Rotate(this);
            actionChain.AddDelaySeconds(rotateTime);

            // switch activate animation
            if (MotionTableId != 0)
            {
                var useAnimation = UseTargetAnimation != null ? (MotionCommand)UseTargetAnimation : MotionCommand.Twitch1;
                EnqueueMotion(actionChain, useAnimation);
            }

            actionChain.AddAction(creature, () =>
            {
                // switch activates target
                if (ActivationTarget > 0)
                {
                    var target = CurrentLandblock?.GetObject(new ObjectGuid(ActivationTarget));

                    target.ActOnUse(creature);
                }

                // only affects players?
                var player = creature as Player;
                if (player != null)
                {
                    if (Usable.HasValue && Usable == ACE.Entity.Enum.Usable.ViewedRemote && SpellDID.HasValue)
                    {
                        var spell = new Server.Entity.Spell((uint)SpellDID);
                        var enchantmentStatus = default(EnchantmentStatus);

                        LifeMagic(player, spell, out uint damage, out bool critical, out enchantmentStatus);    // always life magic?
                        player.PlayParticleEffect(spell.TargetEffect, player.Guid);

                        player.SendUseDoneEvent();
                    }
                }
            });
            actionChain.EnqueueChain();
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.ActivationTarget = Guid.Full;
        }
    }
}
