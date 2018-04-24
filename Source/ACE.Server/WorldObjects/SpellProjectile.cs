using System;
using System.IO;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class SpellProjectile : WorldObject
    {
        private WorldObject projectileCaster;
        private WorldObject projectileTarget;
        private uint spellId;
        private uint lifeProjectileDamage;

        public WorldObject ParentWorldObject { get => projectileCaster; set => projectileCaster = value; }
        public WorldObject TargetWorldObject { get => projectileTarget; set => projectileTarget = value; }
        public uint SpellId { get => spellId; set => spellId = value; }
        public uint LifeProjectileDamage { get => lifeProjectileDamage; set => lifeProjectileDamage = value; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SpellProjectile(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SpellProjectile(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            flags |= IdentifyResponseFlags.WeaponProfile;

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            WriteIdentifyObjectWeaponsProfile(writer, this, success);
        }

        public override void HeartBeat()
        {
            // First heartbeat always appears to happen after spell has already traveled its maximum range, already
            // ProjectileImpact();
            // TODO: Following line should be removed and previous line uncommented, once projectile movement and collision is server controlled
            HandleOnCollide(projectileTarget.Guid);
        }

        private void ProjectileImpact()
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            SpellBase spell = spellTable.Spells[spellId];

            ActionChain selfDestructChain = new ActionChain();
            selfDestructChain.AddAction(this, () =>
            {
                float effect = Math.Max(0.0f, Math.Min(1.0f, ((spell.Power - 1.0f) / 7.0f)));
                CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Explode, effect));
            });
            selfDestructChain.AddDelaySeconds(5.0);
            selfDestructChain.AddAction(this, () => LandblockManager.RemoveObject(this));
            selfDestructChain.EnqueueChain();
        }

        /// <summary>
        /// Handles collision with not the target
        /// </summary>
        public void HandleOnCollide()
        {
            ProjectileImpact();
        }

        public void HandleOnCollide(ObjectGuid target)
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            SpellBase spell = spellTable.Spells[spellId];

            Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);

            ProjectileImpact();
            if (target != TargetWorldObject.Guid)
                return;

            int newSpellTargetVital;

            if (spell.School == MagicSchool.LifeMagic)
            {
                if (TargetWorldObject.WeenieClassId == 1)
                {
                    // Player as the target
                    Player targetPlayer = (Player)TargetWorldObject;
                    if (spell.Name.Contains("Blight"))
                    {
                        newSpellTargetVital = (int)(targetPlayer.GetCurrentCreatureVital(PropertyAttribute2nd.Mana) - (LifeProjectileDamage * spellStatMod.DamageRatio));
                        if (newSpellTargetVital <= 0)
                            targetPlayer.Mana.Current = 0;
                        else
                            targetPlayer.Mana.Current = (uint)newSpellTargetVital;
                    }
                    else if (spell.Name.Contains("Tenacity"))
                    {
                        newSpellTargetVital = (int)(targetPlayer.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) - (LifeProjectileDamage * spellStatMod.DamageRatio));
                        if (newSpellTargetVital <= 0)
                            targetPlayer.Stamina.Current = 0;
                        else
                            targetPlayer.Stamina.Current = (uint)newSpellTargetVital;
                    }
                    else
                    {
                        newSpellTargetVital = (int)(targetPlayer.GetCurrentCreatureVital(PropertyAttribute2nd.Health) - (LifeProjectileDamage * spellStatMod.DamageRatio));
                        if (newSpellTargetVital <= 0)
                            targetPlayer.Health.Current = 0;
                        else
                            targetPlayer.Health.Current = (uint)newSpellTargetVital;
                    }

                    if (projectileCaster.WeenieClassId == 1)
                    {
                        Player player = (Player)projectileCaster;
                        string verb = null, plural = null;
                        Strings.GetAttackVerb(DamageType.Base, 0.30f, ref verb, ref plural);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You {verb} {targetPlayer.Name} for {(LifeProjectileDamage * spellStatMod.DamageRatio)} points of damage!", ChatMessageType.Broadcast));
                        targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} {plural} you for {(LifeProjectileDamage * spellStatMod.DamageRatio)} points of damage!", ChatMessageType.Broadcast));
                    }

                    if (targetPlayer.Health.Current <= 0)
                    {
                        targetPlayer.Die();
                        if (projectileCaster.WeenieClassId == 1)
                        {
                            Player player = (Player)projectileCaster;
                            Strings.DeathMessages.TryGetValue(DamageType.Base, out var messages);
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], targetPlayer.Name), ChatMessageType.Broadcast));
                        }
                        // TODO: death message to the player target
                    }
                }
                else
                {
                    // Creature as the target
                    Player player = (Player)projectileCaster;
                    string verb = null, plural = null;

                    Creature targetCreature = (Creature)TargetWorldObject;
                    if (spell.Name.Contains("Blight"))
                    {
                        newSpellTargetVital = (int)(targetCreature.GetCurrentCreatureVital(PropertyAttribute2nd.Mana) - (LifeProjectileDamage * spellStatMod.DamageRatio));
                        if (newSpellTargetVital <= 0)
                            targetCreature.Mana.Current = 0;
                        else
                            targetCreature.Mana.Current = (uint)newSpellTargetVital;
                    }
                    else if (spell.Name.Contains("Blight"))
                    {
                        newSpellTargetVital = (int)(targetCreature.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) - (LifeProjectileDamage * spellStatMod.DamageRatio));
                        if (newSpellTargetVital <= 0)
                            targetCreature.Stamina.Current = 0;
                        else
                            targetCreature.Stamina.Current = (uint)newSpellTargetVital;
                    }
                    else
                    {
                        newSpellTargetVital = (int)(targetCreature.GetCurrentCreatureVital(PropertyAttribute2nd.Health) - (LifeProjectileDamage * spellStatMod.DamageRatio));
                        if (newSpellTargetVital <= 0)
                            targetCreature.Health.Current = 0;
                        else
                            targetCreature.Health.Current = (uint)newSpellTargetVital;
                    }

                    Strings.GetAttackVerb(DamageType.Base, 0.30f, ref verb, ref plural);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You {verb} {targetCreature.Name} for {(LifeProjectileDamage * spellStatMod.DamageRatio)} points of damage!", ChatMessageType.Broadcast));

                    if (targetCreature.Health.Current <= 0)
                    {
                        targetCreature.Die();

                        Strings.DeathMessages.TryGetValue(DamageType.Base, out var messages);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], targetCreature.Name), ChatMessageType.Broadcast));
                        player.GrantXp((long)targetCreature.XpOverride, true);
                    }
                }
            }
            else
            {
                Random rng = new Random();

                int damage = rng.Next((int)spellStatMod.BaseIntensity, (int)(spellStatMod.Variance + spellStatMod.BaseIntensity));

                DamageType damageType;
                ResistanceType resistanceType;
                switch (spellStatMod.EType)
                {
                    case (uint)DamageType.Acid:
                        damageType = DamageType.Acid;
                        resistanceType = ResistanceType.Acid;
                        break;
                    case (uint)DamageType.Fire:
                        damageType = DamageType.Fire;
                        resistanceType = ResistanceType.Fire;
                        break;
                    case (uint)DamageType.Cold:
                        damageType = DamageType.Cold;
                        resistanceType = ResistanceType.Cold;
                        break;
                    case (uint)DamageType.Electric:
                        damageType = DamageType.Electric;
                        resistanceType = ResistanceType.Electric;
                        break;
                    case (uint)DamageType.Nether:
                        damageType = DamageType.Nether;
                        resistanceType = ResistanceType.Nether;
                        break;
                    case (uint)DamageType.Bludgeon:
                        damageType = DamageType.Bludgeon;
                        resistanceType = ResistanceType.Bludgeon;
                        break;
                    case (uint)DamageType.Pierce:
                        damageType = DamageType.Pierce;
                        resistanceType = ResistanceType.Pierce;
                        break;
                    default:
                        damageType = DamageType.Slash;
                        resistanceType = ResistanceType.Slash;
                        break;
                }
                Strings.DeathMessages.TryGetValue(damageType, out var messages);

                if (TargetWorldObject.WeenieClassId == 1)
                {
                    // Player as the target
                    Player targetPlayer = (Player)TargetWorldObject;
                    damage = (int)Math.Round(damage * targetPlayer.GetNaturalResistence(resistanceType));

                    newSpellTargetVital = (int)(targetPlayer.GetCurrentCreatureVital(PropertyAttribute2nd.Health)) - damage;
                    if (newSpellTargetVital <= 0)
                        targetPlayer.Health.Current = 0;
                    else
                        targetPlayer.Health.Current = (uint)newSpellTargetVital;

                    if (projectileCaster.WeenieClassId == 1)
                    {
                        Player player = (Player)projectileCaster;
                        string verb = null, plural = null;
                        Strings.GetAttackVerb(damageType, 0.30f, ref verb, ref plural);
                        var type = damageType.GetName().ToLower();
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You {verb} {targetPlayer.Name} for {damage} points of {type} damage!", ChatMessageType.Broadcast));
                        targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} {plural} you for {damage} points of {type} damage!", ChatMessageType.Broadcast));
                    }

                    if (targetPlayer.Health.Current <= 0)
                    {
                        targetPlayer.Die();
                        if (projectileCaster.WeenieClassId == 1)
                        {
                            Player player = (Player)projectileCaster;
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], targetPlayer.Name), ChatMessageType.Broadcast));
                        }
                        // TODO: death message to the player target
                    }
                }
                else
                {
                    // Creature as the target
                    Player player = (Player)projectileCaster;
                    string verb = null, plural = null;

                    Creature targetCreature = (Creature)TargetWorldObject;
                    damage = (int)Math.Round(damage * targetCreature.GetNaturalResistence(resistanceType));

                    newSpellTargetVital = (int)(targetCreature.GetCurrentCreatureVital(PropertyAttribute2nd.Health)) - damage;
                    if (newSpellTargetVital <= 0)
                        targetCreature.Health.Current = 0;
                    else
                        targetCreature.Health.Current = (uint)newSpellTargetVital;

                    Strings.GetAttackVerb(damageType, 0.30f, ref verb, ref plural);
                    var type = damageType.GetName().ToLower();
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You {verb} {targetCreature.Name} for {damage} points of {type} damage!", ChatMessageType.Broadcast));

                    if (targetCreature.Health.Current <= 0)
                    {
                        targetCreature.Die();

                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], targetCreature.Name), ChatMessageType.Broadcast));
                        player.GrantXp((long)targetCreature.XpOverride, true);
                    }
                }
            }
        }
    }
}
