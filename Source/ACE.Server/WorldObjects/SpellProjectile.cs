using System;
using System.Numerics;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Managers;

using Spell = ACE.Database.Models.World.Spell;

namespace ACE.Server.WorldObjects
{
    public class SpellProjectile : WorldObject
    {
        private uint spellId;
        private uint lifeProjectileDamage;

        public float DistanceToTarget { get; set; }
        public uint SpellId { get => spellId; private set => spellId = value; }
        public uint LifeProjectileDamage { get => lifeProjectileDamage; set => lifeProjectileDamage = value; }
        public float PlayscriptIntensity { get; set; }
        public ProjectileSpellType SpellType { get; set; }

        public ACE.Entity.Position SpawnPos;
        public SpellBase SpellBase;

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
            // Override weenie description defaults
            ValidLocations = null;
            DefaultScriptId = null;
        }

        /// <summary>
        /// Perfroms additional set up of the spell projectile based on the spell id or its derived type.
        /// </summary>
        /// <param name="spellId"></param>
        public void Setup(uint spellId)
        {
            InitPhysicsObj();

            SpellId = spellId;

            SpellType = GetProjectileSpellType(spellId);
            SpellBase = DatManager.PortalDat.SpellTable.Spells[SpellId];

            // Runtime changes to default state
            ReportCollisions = true;
            Missile = true;
            AlignPath = true;
            PathClipped = true;
            Ethereal = false;
            IgnoreCollisions = false;

            if (SpellType == ProjectileSpellType.Bolt || SpellType == ProjectileSpellType.Streak
                || SpellType == ProjectileSpellType.Arc || SpellType == ProjectileSpellType.Volley || SpellType == ProjectileSpellType.Blast
                || WeenieClassId == 7276 || WeenieClassId == 7277 || WeenieClassId == 7279 || WeenieClassId == 7280)
            {
                PhysicsObj.DefaultScript = ACE.Entity.Enum.PlayScript.ProjectileCollision;
                PhysicsObj.DefaultScriptIntensity = 1.0f;
                var spellLevel = CalculateSpellLevel(SpellBase);
                PlayscriptIntensity = GetProjectileScriptIntensity(SpellType, spellLevel);
            }

            // Some wall spells don't have scripted collisions
            if (WeenieClassId == 7278 || WeenieClassId == 7281 || WeenieClassId == 7282 || WeenieClassId == 23144)
            {
                ScriptedCollision = false;
            }

            AllowEdgeSlide = false;
            // No need to send an ObjScale of 1.0f over the wire since that is the default value
            if (ObjScale == 1.0f)
                ObjScale = null;

            if (SpellType == ProjectileSpellType.Ring)
            {
                ScriptedCollision = false;
                var spellLevel = CalculateSpellLevel(SpellBase);
                PlayscriptIntensity = GetProjectileScriptIntensity(SpellType, spellLevel);
            }

            // Whirling Blade spells get omega values and "align path" turned off which
            // creates the nice swirling animation
            if (WeenieClassId == 1636 || WeenieClassId == 7268 || WeenieClassId == 20979)
            {
                AlignPath = false;
                Omega = new AceVector3(12.56637f, 0f, 0f);
            }
        }

        public enum ProjectileSpellType
        {
            Undef,
            Bolt,
            Blast,
            Volley,
            Streak,
            Arc,
            Ring,
            Wall
        }

        public static ProjectileSpellType GetProjectileSpellType(uint SpellId)
        {
            var WeenieClassId = DatabaseManager.World.GetCachedSpell(SpellId).Wcid;
            if (WeenieClassId == null)
                return ProjectileSpellType.Undef;

            if (WeenieClassId >= 7262 && WeenieClassId <= 7268)
            {
                return ProjectileSpellType.Streak;
            }
            else if (WeenieClassId >= 7269 && WeenieClassId <= 7275)
            {
                return ProjectileSpellType.Ring;
            }
            else if (WeenieClassId >= 7276 && WeenieClassId <= 7282 || WeenieClassId == 23144)
            {
                return ProjectileSpellType.Wall;
            }
            else if (WeenieClassId >= 20973 && WeenieClassId <= 20979)
            {
                return ProjectileSpellType.Arc;
            }
            else if (WeenieClassId == 1499 || WeenieClassId == 1503 || (WeenieClassId >= 1633 && WeenieClassId <= 1667))
            {
                var spreadAngle = DatabaseManager.World.GetCachedSpell(SpellId).SpreadAngle;
                var dimsOriginX = DatabaseManager.World.GetCachedSpell(SpellId).DimsOriginX;
                if (spreadAngle > 0)
                {
                    return ProjectileSpellType.Blast;
                }
                else if (dimsOriginX > 1)
                {
                    return ProjectileSpellType.Volley;
                }
                else
                {
                    return ProjectileSpellType.Bolt;
                }
            }

            return ProjectileSpellType.Undef;
        }

        private float GetProjectileScriptIntensity(ProjectileSpellType spellType, SpellLevel spellLevel)
        {
            if (spellType == ProjectileSpellType.Wall)
            {
                return 0.4f;
            }
            if (spellType == ProjectileSpellType.Ring)
            {
                if (spellLevel == SpellLevel.Six)
                    return 0.4f;
                if (spellLevel == SpellLevel.Seven)
                    return 1.0f;
            }

            // Bolt, Blast, Volley, Streak and Arc all seem to use this scale
            switch (spellLevel)
            {
                case SpellLevel.One:
                    return 0f;
                case SpellLevel.Two:
                    return 0.2f;
                case SpellLevel.Three:
                    return 0.4f;
                case SpellLevel.Four:
                    return 0.6f;
                case SpellLevel.Five:
                    return 0.8f;
                case SpellLevel.Six:
                case SpellLevel.Seven:
                case SpellLevel.Eight:
                    return 1.0f;
                default:
                    return 0f;
            }
        }

        public void ProjectileImpact()
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            SpellBase spell = spellTable.Spells[spellId];

            ActionChain selfDestructChain = new ActionChain();
            selfDestructChain.AddAction(this, () =>
            {
                ReportCollisions = false;
                Ethereal = true;
                IgnoreCollisions = true;
                NoDraw = true;
                Cloaked = true;
                LightsStatus = false;

                PhysicsObj.set_active(false);

                EnqueueBroadcastPhysicsState();
                EnqueueBroadcast(new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Explode, PlayscriptIntensity));
            });
            selfDestructChain.AddDelaySeconds(5.0);
            selfDestructChain.AddAction(this, () => LandblockManager.RemoveObject(this));
            selfDestructChain.EnqueueChain();
        }

        /// <summary>
        /// Handles collision with scenery or other static objects that would block a projectile from reaching its target,
        /// in which case the projectile should be removed with no further processing.
        /// </summary>
        public override void OnCollideEnvironment()
        {
            //Console.WriteLine("SpellProjectile.OnCollideEnvironment()");
            ProjectileImpact();
        }

        public override void OnCollideObject(WorldObject _target)
        {
            //Console.WriteLine("SpellProjectile.OnCollideObject(" + _target.Name + ")");

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            SpellBase spell = spellTable.Spells[spellId];

            Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);

            var player = ProjectileSource as Player;

            if (player != null)
                player.LastHitSpellProjectile = spell;

            // ensure valid creature target
            // non-target objects will be excluded beforehand from collision detection
            var target = _target as Creature;
            if (target == null)
            {
                OnCollideEnvironment();
                return;
            }

            ProjectileImpact();

            var checkPKStatusVsTarget = CheckPKStatusVsTarget(player, (target as Player), spell);
            if (checkPKStatusVsTarget != null)
            {
                if (checkPKStatusVsTarget == false)
                {
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.InvalidPkStatus));
                    return;
                }
            }

            var critical = false;
            var damage = MagicDamageTarget(ProjectileSource as Creature, target, spell, spellStatMod, out DamageType damageType, ref critical, LifeProjectileDamage);

            var targetPlayer = target as Player;

            // null damage -> target resisted; damage of -1 -> target already dead
            if (damage != null && damage != -1)
            {
                uint amount;
                var percent = 0.0f;
                var sneakAttackMod = 1.0f;

                if (spell.School == MagicSchool.LifeMagic && (spell.Name.Contains("Blight") || spell.Name.Contains("Tenacity")))
                {
                    if (spell.Name.Contains("Blight"))
                    {
                        percent = (float)damage / targetPlayer.Mana.MaxValue;
                        amount = (uint)-target.UpdateVitalDelta(target.Mana, (int)-Math.Round(damage.Value));
                    }
                    else
                    {
                        percent = (float)damage / targetPlayer.Stamina.MaxValue;
                        amount = (uint)-target.UpdateVitalDelta(target.Stamina, (int)-Math.Round(damage.Value));
                    }
                }
                else
                {
                    // for possibly applying sneak attack to magic projectiles,
                    // only do this for health-damaging projectiles?
                    if (player != null)
                    {
                        // TODO: use target direction vs. projectile position, instead of player position
                        sneakAttackMod = player.GetSneakAttackMod(target);
                        //Console.WriteLine("Magic sneak attack:  + sneakAttackMod);
                        damage *= sneakAttackMod;
                    }

                    percent = (float)damage / target.Health.MaxValue;
                    amount = (uint)-target.UpdateVitalDelta(target.Health, (int)-Math.Round(damage.Value));
                    target.DamageHistory.Add(ProjectileSource, amount);
                }

                string verb = null, plural = null;
                Strings.DeathMessages.TryGetValue(damageType, out var messages);
                Strings.GetAttackVerb(damageType, percent, ref verb, ref plural);
                var type = damageType.GetName().ToLower();

                amount = (uint)Math.Round(damage.Value);    // full amount for debugging

                if (target.Health.Current <= 0)
                {
                    target.Die();

                    if (player != null)
                    {
                        var rng = Physics.Common.Random.RollDice(0, messages.Count - 1);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[rng], target.Name), ChatMessageType.Broadcast));
                        player.EarnXP((long)target.XpOverride);
                    }
                }
                else
                {
                    var critMsg = critical ? "Critical hit!  " : "";
                    var sneakMsg = sneakAttackMod > 1.0f ? "Sneak Attack! " : "";
                    if (player != null)
                    {
                        var attackerMsg = new GameMessageSystemChat($"{critMsg}{sneakMsg}You {verb} {target.Name} for {amount} points of {type} damage!", ChatMessageType.Magic);
                        var updateHealth = new GameEventUpdateHealth(player.Session, target.Guid.Full, (float)target.Health.Current / target.Health.MaxValue);

                        player.Session.Network.EnqueueSend(attackerMsg, updateHealth);
                    }

                    if (targetPlayer != null)
                        targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{critMsg}{sneakMsg}{ProjectileSource.Name} {plural} you for {amount} points of {type} damage!", ChatMessageType.Magic));
                }

                if (player != null)
                    Proficiency.OnSuccessUse(player, player.GetCreatureSkill(spell.School), Math.Max(25, spell.Power));
            }
            else
            {
                if (damage == -1)
                    return;

                ProjectileSource.EnqueueBroadcast(new GameMessageSound(ProjectileSource.Guid, Sound.ResistSpell, 1.0f));

                if (player != null)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} resists {spell.Name}", ChatMessageType.Magic));

                if (targetPlayer != null)
                {
                    Proficiency.OnSuccessUse(targetPlayer, targetPlayer.GetCreatureSkill(Skill.MagicDefense), (ProjectileSource as Creature).GetCreatureSkill(spell.School).Current);
                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You resist {ProjectileSource.Name}'s {spell.Name}", ChatMessageType.Magic));
                }
            }

            // also called on resist
            if (player != null && targetPlayer == null)
                player.OnAttackMonster(target);
        }


        /// <summary>
        /// Sets the physics state for a launched projectile
        /// </summary>
        public void SetProjectilePhysicsState(WorldObject target, bool useGravity)
        {
            if (useGravity) GravityStatus = true;

            CurrentMotionState = null;
            Placement = null;

            // TODO: Physics description timestamps (sequence numbers) don't seem to be getting updated

            //Console.WriteLine("SpellProjectile PhysicsState: " + PhysicsObj.State);

            var pos = Location.Pos;
            var rotation = Location.Rotation;
            PhysicsObj.Position.Frame.Origin = pos;
            PhysicsObj.Position.Frame.Orientation = rotation;

            var velocity = Velocity.Get();
            //velocity = Vector3.Transform(velocity, Matrix4x4.Transpose(Matrix4x4.CreateFromQuaternion(rotation)));
            PhysicsObj.Velocity = velocity;
            if (target != null)
                PhysicsObj.ProjectileTarget = target.PhysicsObj;

            PhysicsObj.set_active(true);
        }
    }
}
