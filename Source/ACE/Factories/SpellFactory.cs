using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Factories
{
    /// <summary>
    /// factory class for creating objects related to administration or content-recreation
    /// </summary>
    public class SpellFactory
    {
        /// <summary>
        /// creates a spell with velocity directly in front of the player's position provided
        /// </summary>
        public static ImmutableWorldObject CreateCastedSpell(ushort weenieClassId, Position newPosition, ObjectGuid objectid, AceVector3 velocity)
        {

            // all we need right now ..  + required..
            var weenie = WeenieHeaderFlag.Spell;


            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.None, objectid, "Ball of Death", weenieClassId, ObjectDescriptionFlag.None, weenie, newPosition);

            // flame bolt
            wo.GameData.Spell = (ushort)27;
            // wo.Icon = (ushort)100667494;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Velocity | PhysicsDescriptionFlag.Friction | PhysicsDescriptionFlag.Elastcity | PhysicsDescriptionFlag.DefaultScript | PhysicsDescriptionFlag.DefaultScriptIntensity;

            // model id 0x000026 is one of several lifestone IDs
            wo.PhysicsData.CSetup = (uint)33555469;
            wo.PhysicsData.Stable = (uint)536870967;
            wo.PhysicsData.Petable = (uint)872415237;
            wo.PhysicsData.Velocity = velocity;
            wo.PhysicsData.Friction = (uint)1f;
            wo.PhysicsData.Elastcity = (uint)0;
            wo.PhysicsData.DefaultScript = (uint)90; // this needs a enum
            wo.PhysicsData.DefaultScriptIntensity = 1;
            wo.PhysicsData.PhysicsState = PhysicsState.Spell;

            return wo;
        }
    }
}
