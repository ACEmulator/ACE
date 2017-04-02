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
    /// Spell Entity
    /// </summary>
    public class Spell :  MutableWorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ushort SpellID;
        public DefaultScript DefaultScript;
        public float DefaultScriptIntensity;

        public Spell(Position position, AceVector3 velocity, float friction, float elasticity) : base(ObjectType.MissileWeapon, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None))
        {
            Position = position;

            WeenieFlags = WeenieHeaderFlag.Spell;
            GameData.Spell = SpellID;

            PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Velocity | PhysicsDescriptionFlag.Friction | PhysicsDescriptionFlag.Elastcity | PhysicsDescriptionFlag.DefaultScript | PhysicsDescriptionFlag.DefaultScriptIntensity;

            // default values - the correct value come from SpellFactory
            Name = "Ball of Death";
            WeenieClassid = 0;
            SpellID = (ushort)27;
            DefaultScript = Network.Enum.DefaultScript.PS_ProjectileCollision;
            DefaultScriptIntensity = (float)1;
            PhysicsData.CSetup = (uint)33555469;
            PhysicsData.Stable = (uint)536870967;
            PhysicsData.Petable = (uint)872415237;

            PhysicsData.Velocity = velocity;
            PhysicsData.Friction = (float)friction;
            PhysicsData.Elastcity = (float)elasticity;
            PhysicsData.DefaultScript = (uint)DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float)DefaultScriptIntensity;
            PhysicsData.PhysicsState = PhysicsState.Spell;
        }

        public override void PlayScript(Session session)
        {
            var effectEvent = new GameMessageEffect(this.Guid, Effect.Launch, 1f);
            session.Network.EnqueueSend(effectEvent);
        }

    }
}
