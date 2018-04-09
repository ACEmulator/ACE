using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity;
using System.Numerics;
using ACE.Server.Physics.Extensions;
using ACE.Server.Network.Motion;
using ACE.Entity.Enum;
using ACE.Server.Physics.Common;
using ACE.Server.Network.GameMessages.Messages;
using log4net;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void HandleActionTargetedMeleeAttack(ObjectGuid guid, uint attackHeight, float powerLevel)
        {
            /*Console.WriteLine("HandleActionTargetedMeleeAttack");
            Console.WriteLine("Target ID: " + guid.Full.ToString("X8"));
            Console.WriteLine("Attack height: " + attackHeight);
            Console.WriteLine("Power level: " + powerLevel);*/

            // sanity check
            powerLevel = Math.Clamp(powerLevel, 0.0f, 1.0f);

            // get world object of target guid
            var target = CurrentLandblock.GetObject(guid);
            if (target == null)
            {
                log.Warn("Unknown target guid " + guid);
                return;
            }

            // get distance from target
            var dist = GetDistance(target);

            // get angle to target
            var angle = GetAngle(target);

            //Console.WriteLine("Dist: " + dist);
            //Console.WriteLine("Angle: " + angle);

            // turn / moveto if required
            Rotate();
            MoveTo();

            // do melee attack
            Attack(target);
        }

        public float GetDistance(WorldObject target)
        {
            return Location.DistanceTo(target.Location);
        }

        public float GetAngle(WorldObject target)
        {
            var currentDir = Location.GetCurrentDir();
            var targetDir = GetDirection(target);

            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        /// <summary>
        /// Returns the 2D angle between 2 vectors
        /// </summary>
        public float GetAngle(Vector3 a, Vector3 b)
        {
            var cosTheta = a.Dot2D(b);
            var rads = Math.Acos(cosTheta);
            var angle = rads * (180.0f / Math.PI);
            return (float)angle;
        }

        public Vector3 GetDirection(WorldObject target)
        {
            // draw a line from current location
            // to target location
            var offset = target.Location.Pos - Location.Pos;
            offset = offset.Normalize();

            return offset;
        }

        public void Rotate()
        {

        }

        public void MoveTo()
        {

        }

        public void Attack(WorldObject target)
        {
            DoSwingMotion(target);

            if (!(target is Creature))
                return;

            var damage = CalculateDamage(target);
            if (damage > 0.0f)
                target.TakeDamage(this, damage);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} evaded your attack.", ChatMessageType.CombatEnemy));
        }

        public void DoSwingMotion(WorldObject target)
        {
            var motion = new UniversalMotion(MotionStance.UaNoShieldAttack, new MotionItem(MotionCommand.AttackLow2, 1.0f));
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.TurnSpeed = 2.25f;
            motion.HasTarget = true;
            motion.TargetGuid = target.Guid;
            DoMotion(motion);
            CurrentMotionState = motion;
        }

        public float CalculateDamage(WorldObject target)
        {
            var critical = 0.1f;
            var variance = 0.2f;
            var evade = 0.25f;

            // evasion chance
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evade)
                return 0.0f;

            // test: 1/5 of monster total health
            var creature = target as Creature;

            var baseDamage = creature.Health.MaxValue / 5.0f;

            var thisVar = Physics.Common.Random.RollDice(-variance * baseDamage, variance * baseDamage);
            var damage = baseDamage + thisVar;

            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
            if (rng < critical)
                damage *= 2.5f;

            return damage;
        }
    }
}
