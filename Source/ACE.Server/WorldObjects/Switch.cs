using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.Motion;
using ACE.Server.Network.GameMessages.Messages;

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

        private static readonly UniversalMotion twitch = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Twitch1));

        public override void ActOnUse(WorldObject worldObject)
        {
            var switchTimer = new ActionChain();
            var turnToMotion = new UniversalMotion(MotionStance.Standing, Location, Guid);
            turnToMotion.MovementTypes = MovementTypes.TurnToObject;
            switchTimer.AddAction(this, () => worldObject.CurrentLandblock?.EnqueueBroadcastMotion(worldObject, turnToMotion));
            switchTimer.AddDelaySeconds(1);
            switchTimer.AddAction(worldObject, () =>
            {
                if (UseTargetAnimation.HasValue)
                    CurrentLandblock?.EnqueueBroadcastMotion(this, new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)UseTargetAnimation)));
                else
                    CurrentLandblock?.EnqueueBroadcastMotion(this, twitch);
            });
            if (UseTargetAnimation.HasValue)
                switchTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength((MotionCommand)UseTargetAnimation));
            else
                switchTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Twitch1));
            switchTimer.AddAction(worldObject, () =>
            {
                if (ActivationTarget > 0)
                {
                    var target = CurrentLandblock?.GetObject(new ObjectGuid(ActivationTarget.Value));

                    target.ActOnUse(worldObject);
                }

                //player.SendUseDoneEvent();

                //Reset();

                if (worldObject is Player)
                {
                    var player = worldObject as Player;
                    if (Usable.HasValue && Usable == ACE.Entity.Enum.Usable.ViewedRemote && Spell.HasValue && SpellDID.HasValue)
                    {
                        //taken from Gem.UseItem
                        var spellTable = DatManager.PortalDat.SpellTable;
                        if (!spellTable.Spells.ContainsKey((uint)SpellDID)) return;
                        var spellBase = DatManager.PortalDat.SpellTable.Spells[(uint)SpellDID];
                        var spell = DatabaseManager.World.GetCachedSpell((uint)SpellDID);
                        GameMessageSystemChat msg;
                        player.PlayParticleEffect((PlayScript)spellBase.TargetEffect, player.Guid);
                        LifeMagic(player, spellBase, spell, out uint damage, out bool critical, out msg);
                        //player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                        player.SendUseDoneEvent();
                        return;
                    }
                }
            });
            switchTimer.EnqueueChain();
        }

        public override void ActivateLinks()
        {
            if (LinkedInstances.Count > 0)
            {
                foreach (var link in LinkedInstances)
                {
                    var wo = WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie(link.WeenieClassId), new ObjectGuid(link.Guid));

                    if (wo != null)
                    {
                        wo.Location = new Position(link.ObjCellId, link.OriginX, link.OriginY, link.OriginZ, link.AnglesX, link.AnglesY, link.AnglesZ, link.AnglesW);

                        wo.ActivationTarget = Guid.Full;

                        CurrentLandblock?.AddWorldObject(wo);
                    }
                }
            }
        }
    }
}
