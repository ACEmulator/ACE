using ACE.Entity;
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

            Entity.SpellEntity wo = new Entity.SpellEntity(position, velocity, friction, electicity);

            // todo: impletment more advanced templating.
            switch (templateId)
            {
                case 0:
                    wo.SpellID = Spell.FlameBolt;
                    wo.DefaultScript = Network.Enum.Effect.ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555469;
                    wo.PhysicsData.Stable = (uint)536870967;
                    wo.PhysicsData.Petable = (uint)872415237;
                    break;
                case 1:
                    wo.SpellID = Spell.ForceBolt;
                    wo.DefaultScript = Network.Enum.Effect.ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555443;
                    wo.PhysicsData.Stable = (uint)536870971;
                    wo.PhysicsData.Petable = (uint)872415241;
                    break;
                case 2:
                    // Lighting Bolt
                    wo.SpellID = Spell.LightningBolt;
                    wo.DefaultScript = Network.Enum.Effect.ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555440;
                    wo.PhysicsData.Stable = (uint)536870968;
                    wo.PhysicsData.Petable = (uint)872415239;
                    break;
                case 3:
                    // WCID = 1503
                    wo.SpellID = Spell.FrostBolt;
                    wo.DefaultScript = Network.Enum.Effect.ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555444;
                    wo.PhysicsData.Stable = (uint)536870966;
                    wo.PhysicsData.Petable = (uint)872415238;
                    break;
                case 4:
                    // WCID = 1636
                    wo.SpellID = Spell.WhirlingBlade;
                    wo.DefaultScript = Network.Enum.Effect.ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555452;
                    wo.PhysicsData.Stable = (uint)536870972;
                    wo.PhysicsData.Petable = (uint)872415240;
                    break;
            }

            return wo;
        }
    }
}