using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Entity.Enum.Properties;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using System;
using log4net;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Sequence;
using System.Collections.Generic;
using System.Linq;
using ACE.DatLoader.Entity;
using System.IO;

namespace ACE.Entity
{
    public class Creature : Container
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CreatureAbility Strength
        {
            get { return AceObject.StrengthAbility; }
            set { AceObject.StrengthAbility = value; }
        }

        public CombatMode CombatMode { get; private set; }

        public AceObject AceCorpse
        {
            get { return AceObject; }
        }

        public CreatureAbility Endurance
        {
            get { return AceObject.EnduranceAbility; }
            set { AceObject.EnduranceAbility = value; }
        }

        public CreatureAbility Coordination
        {
            get { return AceObject.CoordinationAbility; }
            set { AceObject.CoordinationAbility = value; }
        }

        public CreatureAbility Quickness
        {
            get { return AceObject.QuicknessAbility; }
            set { AceObject.QuicknessAbility = value; }
        }

        public CreatureAbility Focus
        {
            get { return AceObject.FocusAbility; }
            set { AceObject.FocusAbility = value; }
        }

        public CreatureAbility Self
        {
            get { return AceObject.SelfAbility; }
            set { AceObject.SelfAbility = value; }
        }

        public CreatureVital Health
        {
            get { return AceObject.Health; }
            set { AceObject.Health = value; }
        }

        public CreatureVital Stamina
        {
            get { return AceObject.Stamina; }
            set { AceObject.Stamina = value; }
        }

        public CreatureVital Mana
        {
            get { return AceObject.Mana; }
            set { AceObject.Mana = value; }
        }

        /// <summary>
        /// This will be false when creature is dead and waits for respawn
        /// </summary>
        public bool IsAlive { get; set; }

        public double RespawnTime { get; set; }

        protected void SetupVitals()
        {
            if (Health.Current != Health.MaxValue)
            {
                VitalTick(Health);
            }
            if (Stamina.Current != Stamina.MaxValue)
            {
                VitalTick(Stamina);
            }
            if (Mana.Current != Mana.MaxValue)
            {
                VitalTick(Mana);
            }
        }

        public Creature(AceObject baseObject)
            : base(baseObject)
        {
            if (Attackable == false)
            {
                if (RadarColor != Enum.RadarColor.Yellow || (RadarColor == Enum.RadarColor.Yellow && CreatureType == null))
                        NpcLooksLikeObject = true;
            }
        }

        public virtual ActionChain GetOnKillChain(Session killerSession)
        {
            ActionChain onKillChain = new ActionChain();
            /*
            // Wait to respawn
            onKillChain.AddDelayTicks(10);
            */

            // Will start death animation
            onKillChain.AddAction(this, () => OnKill(killerSession));
            // Wait for kill animation
            onKillChain.AddDelaySeconds(2);
            onKillChain.AddChain(GetCreateCorpseChain());

            return onKillChain;
        }

        protected virtual float GetCorpseSpawnTime()
        {
            return 60;
        }

        public ActionChain GetCreateCorpseChain()
        {
            ActionChain createCorpseChain = new ActionChain(this, () =>
            {
                // Create Corspe and set a location on the ground
                // TODO: set text of killer in description and find a better computation for the location, some corpse could end up in the ground
                var corpse = CorpseObjectFactory.CreateCorpse(this, this.Location);
                // FIXME(ddevec): We don't have a real corpse yet, so these come in null -- this hack just stops them from crashing the game
                corpse.Location.PositionY -= (corpse.ObjScale ?? 0);
                corpse.Location.PositionZ -= (corpse.ObjScale ?? 0) / 2;

                // Corpses stay on the ground for 5 * player level but minimum 1 hour
                // corpse.DespawnTime = Math.Max((int)session.Player.PropertiesInt[Enum.Properties.PropertyInt.Level] * 5, 360) + WorldManager.PortalYearTicks; // as in live
                // corpse.DespawnTime = 20 + WorldManager.PortalYearTicks; // only for testing
                float despawnTime = GetCorpseSpawnTime();

                // Create corpse
                CurrentLandblock.AddWorldObject(corpse);
                // Create corpse decay
                ActionChain despawnChain = new ActionChain();
                despawnChain.AddDelaySeconds(despawnTime);
                despawnChain.AddAction(CurrentLandblock, () => corpse.CurrentLandblock.RemoveWorldObject(corpse.Guid, false));
                despawnChain.EnqueueChain();
            });
            return createCorpseChain;
        }

        private void OnKill(Session session)
        {
            IsAlive = false;
            // This will determine if the derived type is a player
            var isDerivedPlayer = Guid.IsPlayer();

            if (!isDerivedPlayer)
            {
                // Create and send the death notice
                string killMessage = $"{session.Player.Name} has killed {Name}.";
                var creatureDeathEvent = new GameEventDeathNotice(session, killMessage);
                session.Network.EnqueueSend(creatureDeathEvent);
            }

            // MovementEvent: (Hand-)Combat or in the case of smite: from Standing to Death
            // TODO: Check if the duration of the motion can somehow be computed
            UniversalMotion motionDeath = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            CurrentLandblock.EnqueueBroadcastMotion(this, motionDeath);

            // If the object is a creature, Remove it from from Landblock
            if (!isDerivedPlayer)
            {
                CurrentLandblock.RemoveWorldObject(Guid, false);
            }
        }

        /// <summary>
        /// Updates a vital, returns true if vital is now < max
        /// </summary>
        /// <param name="vital"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public void UpdateVital(CreatureVital vital, uint newVal)
        {
            EnqueueAction(new ActionEventDelegate(() => UpdateVitalInternal(vital, newVal)));
        }

        public void DeltaVital(CreatureVital vital, long delta)
        {
            EnqueueAction(new ActionEventDelegate(() => DeltaVitalInternal(vital, delta)));
        }

        private void DeltaVitalInternal(CreatureVital vital, long delta)
        {
            uint absVal;

            if (delta < 0 && Math.Abs(delta) > vital.Current)
            {
                absVal = (uint)(-1 * vital.Current);
            }
            else if (delta + vital.Current > vital.MaxValue)
            {
                absVal = (uint)(vital.MaxValue - vital.Current);
            }
            else
            {
                absVal = (uint)(vital.Current + delta);
            }

            UpdateVitalInternal(vital, absVal);
        }

        private void VitalTick(CreatureVital vital)
        {
            double tickTime = vital.NextTickTime;
            if (double.IsNegativeInfinity(tickTime))
            {
                tickTime = vital.RegenRate;
            }
            else
            {
                tickTime -= WorldManager.PortalYearTicks;
            }

            // Set up our next tick
            ActionChain tickChain = new ActionChain();
            tickChain.AddDelayTicks(tickTime);
            tickChain.AddAction(this, () => VitalTickInternal(vital));
            tickChain.EnqueueChain();
        }

        protected virtual void VitalTickInternal(CreatureVital vital)
        {
            vital.Tick(WorldManager.PortalYearTicks);
            if (vital.Current != vital.MaxValue)
            {
                VitalTick(vital);
            }
        }

        public void HandleSwitchToMagicCombatMode()
        {
            EquippedItem mEquipedWand = Children.Find(s => s.EquipMask == EquipMask.Held);
            if (mEquipedWand != null)
            {
                UniversalMotion mm = new UniversalMotion(MotionStance.Spellcasting);
                mm.MovementData.CurrentStyle = (ushort)MotionStance.Spellcasting;
                SetMotionState(this, mm);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.CombatMode, (uint)CombatMode.Magic));
            }
            else
                log.InfoFormat("Changing combat mode for {0} - could not locate a wielded magic caster", Guid);
        }

        public void HandleUnloadMissileAmmo(CombatMode oldCombatMode)
        {
            // Before I can switch to any non missile mode, do I have missile ammo I need to remove?
            WorldObject ammo = null;
            EquippedItem mEquipedAmmo = Children.Find(s => s.EquipMask == EquipMask.MissileAmmo);

            if (mEquipedAmmo != null)
                ammo = GetInventoryItem(new ObjectGuid(mEquipedAmmo.Guid));

            if (oldCombatMode == CombatMode.Missile)
            {
                if (ammo != null)
                {
                    ammo.Location = null;
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePickupEvent(ammo));
                }
            }
        }

        public void HandleSwitchToPeaceMode(CombatMode oldCombatMode, bool isAutonomous = false)
        {
            HandleUnloadMissileAmmo(oldCombatMode);

            // FIXME: (Og II)<this is a hack for now to be removed.> Placement has an issue we have not figured out.   It has to do with animation frame. Og II
            PositionFlag &= ~UpdatePositionFlag.Placement;
            // End hack
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePosition(this));
            UniversalMotion mm = new UniversalMotion(MotionStance.Standing);
            mm.MovementData.CurrentStyle = (ushort)MotionStance.Standing;
            SetMotionState(this, mm);
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.CombatMode, (uint)CombatMode.NonCombat));
        }

        public void HandleSwitchToMissileCombatMode(ActionChain combatModeChain)
        {
            EquippedItem mEquipedMissile = Children.Find(s => s.EquipMask == EquipMask.MissileWeapon);
            if (mEquipedMissile?.Guid != null)
            {
                WorldObject missileWeapon = GetInventoryItem(new ObjectGuid(mEquipedMissile.Guid));
                if (missileWeapon == null)
                {
                    log.InfoFormat("Changing combat mode for {0} - could not locate wielded weapon {1}", Guid, mEquipedMissile.Guid);
                    return;
                }
                EquippedItem mEquipedAmmo = Children.Find(s => s.EquipMask == EquipMask.MissileAmmo);
                MotionStance ms;

                if (missileWeapon.DefaultCombatStyle != null)
                    ms = (MotionStance)missileWeapon.DefaultCombatStyle;
                else
                {
                    log.InfoFormat("Changing combat mode for {0} - wielded item {1} has not be assigned a default combat style", Guid, mEquipedMissile.Guid);
                    return;
                }

                UniversalMotion mm = new UniversalMotion(ms);
                mm.MovementData.CurrentStyle = (ushort)ms;
                if (mEquipedAmmo == null)
                {
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePosition(this));
                    SetMotionState(this, mm);
                }
                else
                {
                    WorldObject ammo = GetInventoryItem(new ObjectGuid(mEquipedAmmo.Guid));
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePosition(this));
                    SetMotionState(this, mm);
                    mm.MovementData.ForwardCommand = (ushort)MotionCommand.Reload;
                    SetMotionState(this, mm);
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePosition(this));
                    // FIXME: (Og II)<this is a hack for now to be removed. Need to pull delay from dat file
                    combatModeChain.AddDelaySeconds(0.25);
                    // System.Threading.Thread.Sleep(250); // used for debugging
                    mm.MovementData.ForwardCommand = (ushort)MotionCommand.Invalid;
                    SetMotionState(this, mm);
                    // FIXME: (Og II)<this is a hack for now to be removed. Need to pull delay from dat file
                    combatModeChain.AddDelaySeconds(0.40);
                    combatModeChain.AddAction(this, () => CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageParentEvent(this, ammo, 1, 1)));
                    // CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageParentEvent(this, ammo, 1, 1)); // used for debugging
                }
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.CombatMode, (uint)CombatMode.Missile));
            }
        }

        public void HandleSwitchToMeleeCombatMode(CombatMode olCombatMode)
        {
            bool shieldEquiped = false;
            bool weaponInShieldSlot = false;

            // Check to see if we were in missile combat and have an arrow hanging around we might need to remove.
            HandleUnloadMissileAmmo(olCombatMode);

            EquippedItem mEquipedShieldSlot = Children.Find(s => s.EquipMask == EquipMask.Shield);
            if (mEquipedShieldSlot != null)
            {
                WorldObject itemInShieldSlot = GetInventoryItem(new ObjectGuid(mEquipedShieldSlot.Guid));
                if (itemInShieldSlot != null)
                {
                    if (itemInShieldSlot.ItemType == ItemType.Armor)
                        shieldEquiped = true;
                    else
                        weaponInShieldSlot = true;
                }
            }

            EquippedItem mEquipedMelee = Children.Find(s => s.EquipMask == EquipMask.MeleeWeapon);
            EquippedItem mEquipedTwoHanded = Children.Find(s => s.EquipMask == EquipMask.TwoHanded);
            MotionStance ms = MotionStance.Invalid;
            // are we unarmed?   If so, do we have a shield?
            if (mEquipedMelee == null && mEquipedTwoHanded == null && !weaponInShieldSlot)
            {
                if (!shieldEquiped)
                    ms = MotionStance.UaNoShieldAttack;
                else
                    ms = MotionStance.MeleeShieldAttack;
            }
            else if (weaponInShieldSlot)
                ms = MotionStance.DualWieldAttack;

            if (mEquipedTwoHanded != null)
            {
                WorldObject twoHandedWeapon = GetInventoryItem(new ObjectGuid(mEquipedTwoHanded.Guid));
                if (twoHandedWeapon.DefaultCombatStyle != null)
                    ms = twoHandedWeapon.DefaultCombatStyle.Value;
            }

            // Let's see if we are melee single handed / two handed with our without shield as appropriate.
            if (mEquipedMelee?.Guid != null && ms != MotionStance.DualWieldAttack)
            {
                WorldObject meleeWeapon = GetInventoryItem(new ObjectGuid(mEquipedMelee.Guid));

                if (meleeWeapon == null)
                {
                    log.InfoFormat("Changing combat mode for {0} - could not locate wielded weapon {1}", Guid, mEquipedMelee.Guid);
                    return;
                }

                if (!shieldEquiped)
                {
                    if (meleeWeapon.DefaultCombatStyle != null)
                        ms = meleeWeapon.DefaultCombatStyle.Value;
                }
                else
                {
                    switch (meleeWeapon.DefaultCombatStyle)
                    {
                        case MotionStance.MeleeNoShieldAttack:
                            ms = MotionStance.MeleeShieldAttack;
                            break;
                        case MotionStance.ThrownWeaponAttack:
                            ms = MotionStance.ThrownShieldCombat;
                            break;
                        case MotionStance.UaNoShieldAttack:
                            ms = MotionStance.MeleeShieldAttack;
                            break;
                        default:
                            log.InfoFormat(
                                "Changing combat mode for {0} - unable to determine correct combat stance for weapon {1}", Guid, mEquipedMelee.Guid);
                            return;
                    }
                }
            }
            if (ms != MotionStance.Invalid)
            {
                UniversalMotion mm = new UniversalMotion(ms);
                mm.MovementData.CurrentStyle = (ushort)ms;
                SetMotionState(this, mm);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.CombatMode, (uint)CombatMode.Melee));
            }
            else
                log.InfoFormat("Changing combat mode for {0} - wielded item {1} has not be assigned a default combat style", Guid, mEquipedMelee?.Guid ?? mEquipedTwoHanded?.Guid);
        }

        public void SetCombatMode(CombatMode newCombatMode)
        {
            log.InfoFormat("Changing combat mode for {0} to {1}", Guid, newCombatMode);

            ActionChain combatModeChain = new ActionChain();
            combatModeChain.AddAction(this, () =>
                {
                    CombatMode oldCombatMode = CombatMode;
                    CombatMode = newCombatMode;
                    switch (CombatMode)
                    {
                        case CombatMode.NonCombat:
                            HandleSwitchToPeaceMode(oldCombatMode);
                            break;
                        case CombatMode.Melee:
                            HandleSwitchToMeleeCombatMode(oldCombatMode);
                            break;
                        case CombatMode.Magic:
                            HandleSwitchToMagicCombatMode();
                            break;
                        case CombatMode.Missile:
                            HandleSwitchToMissileCombatMode(combatModeChain);
                            break;
                        default:
                            log.InfoFormat("Changing combat mode for {0} - something has gone wrong", Guid);
                            break;
                    }
                });
            combatModeChain.EnqueueChain();
        }

        public void SetMotionState(WorldObject obj, MotionState motionState)
        {
            CurrentMotionState = motionState;
            motionState.IsAutonomous = false;
            GameMessageUpdateMotion updateMotion = new GameMessageUpdateMotion(Guid, Sequences.GetCurrentSequence(SequenceType.ObjectInstance), obj.Sequences, motionState);
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, updateMotion);
        }

        protected virtual void UpdateVitalInternal(CreatureVital vital, uint newVal)
        {
            uint old = vital.Current;

            if (newVal > vital.MaxValue)
            {
                newVal = (vital.MaxValue - vital.Current);
            }

            vital.Current = newVal;

            // Check for amount
            if (old == vital.MaxValue && vital.Current != vital.MaxValue)
            {
                // Start up a vital ticker
                new ActionChain(this, () => VitalTickInternal(vital)).EnqueueChain();
            }
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            bool hideCreatureProfile = NpcLooksLikeObject ?? false;

            if (!hideCreatureProfile)
            {
                flags |= IdentifyResponseFlags.CreatureProfile;
            }

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            if (!hideCreatureProfile)
            {
                WriteIdentifyObjectCreatureProfile(writer, this, success);
            }
        }

        protected static void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature obj, bool success)
        {
            uint header = 0;
            // TODO: for now, we are always succeeding - will need to set this to 0 header for failure.   Og II
            if (success)
                header = 8;
            writer.Write(header);
            writer.Write(obj.Health.Current);
            writer.Write(obj.Health.MaxValue);
            if (header == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    writer.Write(0u);
                }
            }
            else
            {
                // TODO: we probably need buffed values here  it may be set my the last flag I don't understand yet. - will need to revisit. Og II
                writer.Write(obj.Strength.UnbuffedValue);
                writer.Write(obj.Endurance.UnbuffedValue);
                writer.Write(obj.Quickness.UnbuffedValue);
                writer.Write(obj.Coordination.UnbuffedValue);
                writer.Write(obj.Focus.UnbuffedValue);
                writer.Write(obj.Self.UnbuffedValue);
                writer.Write(obj.Stamina.UnbuffedValue);
                writer.Write(obj.Mana.UnbuffedValue);
                writer.Write(obj.Stamina.MaxValue);
                writer.Write(obj.Mana.MaxValue);
                // this only gets sent if the header can be masked with 1
                // Writer.Write(0u);
            }
        }
    }
}
