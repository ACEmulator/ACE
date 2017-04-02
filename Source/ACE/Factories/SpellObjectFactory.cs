using ACE.Entity;
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

            Entity.Spell wo = new Entity.Spell(position, velocity, friction, electicity);

            // this is temp ?
            switch (templateId)
            {
                case 0:
                    // fire ark ?
                    wo.SpellID = (ushort)27;
                    wo.DefaultScript = Network.Enum.DefaultScript.PS_ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555469;
                    wo.PhysicsData.Stable = (uint)536870967;
                    wo.PhysicsData.Petable = (uint)872415237;
                    break;
                case 1:
                    // force bolt
                    wo.SpellID = (ushort)86;
                    wo.DefaultScript = Network.Enum.DefaultScript.PS_ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555443;
                    wo.PhysicsData.Stable = (uint)536870971;
                    wo.PhysicsData.Petable = (uint)872415241;
                    break;
                case 2:
                    // Lighting Bolt
                    wo.SpellID = (ushort)75;
                    wo.DefaultScript = Network.Enum.DefaultScript.PS_ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555440;
                    wo.PhysicsData.Stable = (uint)536870968;
                    wo.PhysicsData.Petable = (uint)872415239;
                    break;
                case 3:
                    // Frost Bolt // WCID = 1503 - Maybe this should be settable after creation ?
                    wo.SpellID = (ushort)28;
                    wo.DefaultScript = Network.Enum.DefaultScript.PS_ProjectileCollision;
                    wo.DefaultScriptIntensity = (float)1;
                    wo.PhysicsData.CSetup = (uint)33555444;
                    wo.PhysicsData.Stable = (uint)536870966;
                    wo.PhysicsData.Petable = (uint)872415238;
                    break;
                case 4:
                    // Whirling Blade // WCID = 1636 - Maybe this should be settable after creation ?
                    wo.SpellID = (ushort)92;
                    wo.DefaultScript = Network.Enum.DefaultScript.PS_ProjectileCollision;
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