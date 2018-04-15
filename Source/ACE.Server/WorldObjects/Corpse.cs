using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.Motion;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class Corpse : Container
    {
        private static readonly UniversalMotion dead = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

        /// <summary>
        /// Timer's value is in heartbeats, which is every 5 seconds. This value equates for five minutes, including the first heartbeat queued at WO creation.
        /// </summary>
        int rotTimer = 55;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Corpse(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Corpse(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Corpse;

            CurrentMotionState = dead;

            ContainerCapacity = 10;
            ItemCapacity = 120;
        }

        /// <summary>
        /// Put in place to clean corpses, until more global cleanup management can be implemented
        /// </summary>
        public override void HeartBeat()
        {
            --rotTimer;

            if (rotTimer <= 0)
            {
                ActionChain selfDestructChain = new ActionChain();
                selfDestructChain.AddAction(this, () => LandblockManager.RemoveObject(this));
                selfDestructChain.EnqueueChain();
                return;
            }

            QueueNextHeartBeat();
        }

        public override ACE.Entity.ObjDesc CalculateObjDesc()
        {
            if (Biota.BiotaPropertiesAnimPart.Count == 0 && Biota.BiotaPropertiesPalette.Count == 0 && Biota.BiotaPropertiesTextureMap.Count == 0)
                return base.CalculateObjDesc(); // No Saved ObjDesc, let base handle it.

            ACE.Entity.ObjDesc objDesc = new ACE.Entity.ObjDesc();

            AddBaseModelData(objDesc);

            foreach (var animPart in Biota.BiotaPropertiesAnimPart)
                objDesc.AnimPartChanges.Add(new ACE.Entity.AnimationPartChange { PartIndex = (byte)animPart.Index, PartID = animPart.AnimationId });

            foreach (var subPalette in Biota.BiotaPropertiesPalette)
                objDesc.SubPalettes.Add(new ACE.Entity.SubPalette { SubID = subPalette.SubPaletteId, Offset = subPalette.Offset, NumColors = subPalette.Length });

            foreach (var textureMap in Biota.BiotaPropertiesTextureMap)
                objDesc.TextureChanges.Add(new ACE.Entity.TextureMapChange { PartIndex = (byte)textureMap.Index, OldTexture = textureMap.OldId, NewTexture = textureMap.NewId });

            return objDesc;
        }
    }
}
