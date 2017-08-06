////using ACE.Entity;
////using ACE.Entity.Enum;
////using ACE.Managers;
////using ACE.Network.Enum;

////namespace ACE.Factories
////{
////    /// <summary>
////    /// Spell Factory for creating objects related to Spells.
////    /// </summary>
////    public class SpellObjectFactory
////    {
////        public static WorldObject CreateSpell(uint templateId, Position position, AceVector3 velocity, float friction, float electicity)
////        {
////            ushort weenieClassId = 0;
////            Spell spellId = 0;
////            uint modelId = 0;
////            uint soundTableId = 0;
////            uint physicsTableId = 0;

////            // todo: impletment more advanced templating.
////            switch (templateId)
////            {
////                case 0:
////                    weenieClassId = 20974;
////                    spellId = Spell.;
////                    modelId = (uint)33555469;
////                    soundTableId = (uint)536870967;
////                    physicsTableId = (uint)872415237;
////                    break;
////                case 1:
////                    weenieClassId = 7264;
////                    spellId = Spell.ForceBolt;
////                    modelId = (uint)33555443;
////                    soundTableId = (uint)536870971;
////                    physicsTableId = (uint)872415241;
////                    break;
////                case 2:
////                    weenieClassId = 1635;
////                    spellId = Spell.LightningBolt;
////                    modelId = (uint)33555440;
////                    soundTableId = (uint)536870968;
////                    physicsTableId = (uint)872415239;
////                    break;
////                case 3:
////                    weenieClassId = 1503;
////                    spellId = Spell.FrostBolt;
////                    modelId = (uint)33555444;
////                    soundTableId = (uint)536870966;
////                    physicsTableId = (uint)872415238;
////                    break;
////                case 4:
////                    weenieClassId = 1636;
////                    spellId = Spell.WhirlingBlade;
////                    modelId = (uint)33555452;
////                    soundTableId = (uint)536870972;
////                    physicsTableId = (uint)872415240;
////                    break;
////            }

////            SpellLikeEffect mobj = new SpellLikeEffect(ItemType.MissileWeapon, GuidManager.NewItemGuid(), "Spell", weenieClassId, ObjectDescriptionFlag.None, WeenieHeaderFlag.Spell, position, spellId, modelId, soundTableId, physicsTableId);

////            mobj.DefaultScriptId = (uint)PlayScript.ProjectileCollision;
////            mobj.DefaultScriptIntensity = (float)1;

////            mobj.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.STable | PhysicsDescriptionFlag.PeTable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Velocity | PhysicsDescriptionFlag.Friction | PhysicsDescriptionFlag.Elasticity | PhysicsDescriptionFlag.DefaultScript | PhysicsDescriptionFlag.DefaultScriptIntensity;
////            mobj.Velocity = velocity;
////            mobj.Friction = (float)friction;
////            mobj.Elasticity = (float)electicity;
////            mobj.PhysicsState = PhysicsState.ReportCollision | PhysicsState.Missile | PhysicsState.AlignPath | PhysicsState.PathClipped | PhysicsState.Gravity | PhysicsState.LightingOn | PhysicsState.ScriptedCollision | PhysicsState.Inelastic;

////            mobj.PlayerScript = PlayScript.Launch;

////            return mobj;
////        }
////    }
////}