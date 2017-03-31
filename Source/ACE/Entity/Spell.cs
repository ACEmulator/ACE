using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using log4net;

namespace ACE.Entity
{
    /// <summary>
    /// factory class for creating objects related to administration or content-recreation
    /// </summary>
    public class Spell :  MutableWorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Spell(Position position, DefaultScript script, AceVector3 velocity, float friction, float elasticity) : base(ObjectType.MissileWeapon, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None))
        {

            Name = "Ball of Death";
            WeenieClassid = 0;
            Position = position;

            WeenieFlags = WeenieHeaderFlag.Spell;
            GameData.Spell = (ushort)27;

            PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Velocity | PhysicsDescriptionFlag.Friction | PhysicsDescriptionFlag.Elastcity | PhysicsDescriptionFlag.DefaultScript | PhysicsDescriptionFlag.DefaultScriptIntensity;

            PhysicsData.CSetup = (uint)33555469;
            PhysicsData.Stable = (uint)536870967;
            PhysicsData.Petable = (uint)872415237;
            PhysicsData.Velocity = velocity;
            PhysicsData.Friction = (float)friction;
            PhysicsData.Elastcity = (float)elasticity;
            PhysicsData.DefaultScript = (uint)script; // this needs a enum
            PhysicsData.DefaultScriptIntensity = 1;
            PhysicsData.PhysicsState = PhysicsState.Spell;
        }

        public override void PlayScript(Session session)
        {
            var effectEvent = new GameMessageEffect(this.Guid, Effect.Launch, 1f);
            session.Network.EnqueueSend(effectEvent);
        }

    }
}
