using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{

    /// <summary>
    /// Spell Factory for creating objects related to Spells.
    /// </summary>
    public class SpellObjectFactory
    {
        public static WorldObject CreateSpell(uint templateId, Position position, AceVector3 velocity, float friction, float electicity)
        {

            ushort weenieClassId = 0;
            MutableWorldObject mobj = new MutableWorldObject(ObjectType.MissileWeapon, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Spell", weenieClassId, ObjectDescriptionFlag.None, WeenieHeaderFlag.Spell, position);

            // Default
            mobj.GameData.Name = "Ball of Death";
            mobj.GameData.Spell = Spell.FlameBolt;
            mobj.PhysicsData.CSetup = (uint)33555469;
            mobj.PhysicsData.Stable = (uint)536870967;
            mobj.PhysicsData.Petable = (uint)872415237;
            mobj.PhysicsData.DefaultScript = (uint)Network.Enum.PlayScript.ProjectileCollision;
            mobj.PhysicsData.DefaultScriptIntensity = (float)1;

            mobj.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Velocity | PhysicsDescriptionFlag.Friction | PhysicsDescriptionFlag.Elastcity | PhysicsDescriptionFlag.DefaultScript | PhysicsDescriptionFlag.DefaultScriptIntensity;
            mobj.PhysicsData.Velocity = velocity;
            mobj.PhysicsData.Friction = (float)friction;
            mobj.PhysicsData.Elastcity = (float)electicity;
            mobj.PhysicsData.PhysicsState = PhysicsState.Spell;

            // todo: impletment more advanced templating.
            switch (templateId)
            {
                case 0:
                    mobj.WeenieClassid = 20974;
                    mobj.GameData.Spell = Spell.FlameBolt;
                    mobj.PhysicsData.CSetup = (uint)33555469;
                    mobj.PhysicsData.Stable = (uint)536870967;
                    mobj.PhysicsData.Petable = (uint)872415237;
                    break;
                case 1:
                    mobj.WeenieClassid = 7264;
                    mobj.GameData.Spell = Spell.ForceBolt;
                    mobj.PhysicsData.CSetup = (uint)33555443;
                    mobj.PhysicsData.Stable = (uint)536870971;
                    mobj.PhysicsData.Petable = (uint)872415241;
                    break;
                case 2:
                    mobj.WeenieClassid = 1635;
                    mobj.GameData.Spell = Spell.LightningBolt;
                    mobj.PhysicsData.CSetup = (uint)33555440;
                    mobj.PhysicsData.Stable = (uint)536870968;
                    mobj.PhysicsData.Petable = (uint)872415239;
                    break;
                case 3:
                    mobj.WeenieClassid = 1503;
                    mobj.GameData.Spell = Spell.FrostBolt;
                    mobj.PhysicsData.CSetup = (uint)33555444;
                    mobj.PhysicsData.Stable = (uint)536870966;
                    mobj.PhysicsData.Petable = (uint)872415238;
                    break;
                case 4:
                    mobj.WeenieClassid = 1636;
                    mobj.GameData.Spell = Spell.WhirlingBlade;
                    mobj.PhysicsData.CSetup = (uint)33555452;
                    mobj.PhysicsData.Stable = (uint)536870972;
                    mobj.PhysicsData.Petable = (uint)872415240;
                    break;
            }

            mobj.PlayerScript = PlayScript.Launch;

            return mobj;
        }
    }
}