using System;

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
        private Creature projectileCaster;
        private ObjectGuid targetGuid;
        private uint spellId;
        private uint lifeProjectileDamage;

        public Creature ParentWorldObject { get => projectileCaster; set => projectileCaster = value; }
        public ObjectGuid TargetGuid { get => targetGuid; set => targetGuid = value; }
        public uint SpellId { get => spellId; private set => spellId = value; }
        public uint LifeProjectileDamage { get => lifeProjectileDamage; set => lifeProjectileDamage = value; }
        public float FlightTime { get; set; }
        public float PlayscriptIntensity { get; set; }
        public ProjectileSpellType SpellType { get; set; }

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
            var spell = DatManager.PortalDat.SpellTable.Spells[SpellId];

            if (SpellType == ProjectileSpellType.Bolt || SpellType == ProjectileSpellType.Streak
                || SpellType == ProjectileSpellType.Arc || SpellType == ProjectileSpellType.Volley)
            {
                PhysicsObj.DefaultScript = ACE.Entity.Enum.PlayScript.ProjectileCollision;
                PhysicsObj.DefaultScriptIntensity = 1.0f;
                var spellLevel = CalculateSpellLevel(spell);
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
            else if (WeenieClassId >= 7269 && WeenieClassId <= 2725)
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
            if (spellType == ProjectileSpellType.Ring || spellType == ProjectileSpellType.Wall)
            {
                return 0.4f;
                // TODO: higher level ring spells use 1.0f intensity
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

        private void ProjectileImpact()
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

                SpellType = GetProjectileSpellType(spellId);
                var spellPower = spell.Power;
                var spellLevel = CalculateSpellLevel(spell);
                PlayscriptIntensity = GetProjectileScriptIntensity(SpellType, spellLevel);

                CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Explode, PlayscriptIntensity));
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

            var player = projectileCaster as Player;

            // ensure valid creature target
            // non-target objects will be excluded beforehand from collision detection
            var target = _target as Creature;
            if (target == null)
            {
                OnCollideEnvironment();
                return;
            }

            ProjectileImpact();

            var critical = false;
            var damage = MagicDamageTarget(projectileCaster, target, spell, spellStatMod, out DamageType damageType, ref critical, LifeProjectileDamage);

            var targetPlayer = target as Player;

            // null damage -> target resisted; damage of -1 -> target already dead
            if (damage != null && damage != -1)
            {
                uint amount;
                var percent = 0.0f;

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
                    percent = (float)damage / target.Health.MaxValue;
                    amount = (uint)-target.UpdateVitalDelta(target.Health, (int)-Math.Round(damage.Value));
                    target.DamageHistory.Add(projectileCaster, amount);
                }

                string verb = null, plural = null;
                Strings.DeathMessages.TryGetValue(damageType, out var messages);
                Strings.GetAttackVerb(damageType, percent, ref verb, ref plural);
                var type = damageType.GetName().ToLower();

                amount = (uint)Math.Round(damage.Value);    // full amount for debugging

                if (player != null)
                {
                    // is percent for the vital, or always based on health?
                    var attackerMsg = new GameEventAttackerNotification(player.Session, target.Name, damageType, percent, amount, critical, new AttackConditions());
                    player.Session.Network.EnqueueSend(attackerMsg, new GameEventUpdateHealth(player.Session, target.Guid.Full, (float)target.Health.Current / target.Health.MaxValue));
                }

                if (targetPlayer != null)
                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{projectileCaster.Name} {plural} you for {amount} points of {type} damage!", ChatMessageType.Magic));
                    //targetPlayer.Session.Network.EnqueueSend(new GameEventDefenderNotification(targetPlayer.Session, projectileCaster.Name, damageType, percent, amount, DamageLocation.Chest, critical, new AttackConditions()));    // damageLocation?

                if (target.Health.Current <= 0)
                {
                    target.Die();

                    if (player != null)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], target.Name), ChatMessageType.Broadcast));
                        player.EarnXP((long)target.XpOverride);
                    }
                }
            }
            else
            {
                if (damage == -1)
                    return;

                CurrentLandblock?.EnqueueBroadcastSound(projectileCaster, Sound.ResistSpell);

                if (player != null)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} resists {spell.Name}", ChatMessageType.Magic));

                if (targetPlayer != null)
                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You resist {ParentWorldObject.Name}'s {spell.Name}", ChatMessageType.Magic));

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
            // runtime changes to default state
            ReportCollisions = true;
            Missile = true;
            AlignPath = true;
            PathClipped = true;
            Ethereal = false;
            IgnoreCollisions = false;

            if (useGravity) GravityStatus = true;

            CurrentMotionState = null;
            Placement = null;

            // TODO: Physics description timestamps (sequence numbers) don't seem to be getting updated

            Console.WriteLine("Projectile PhysicsState: " + PhysicsObj.State);

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
