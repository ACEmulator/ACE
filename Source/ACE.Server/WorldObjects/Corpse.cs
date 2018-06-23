using System;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Corpse : Container
    {
        /// <summary>
        /// The number of seconds for a corpse to disappear
        /// </summary>
        public double DecayTime = 120;

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

            CurrentMotionState = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

            ContainerCapacity = 10;
            ItemCapacity = 120;
        }

        /// <summary>
        /// Sets the object description for a corpse
        /// </summary>
        /// <returns></returns>
        public override ObjDesc CalculateObjDesc()
        {
            if (Biota.BiotaPropertiesAnimPart.Count == 0 && Biota.BiotaPropertiesPalette.Count == 0 && Biota.BiotaPropertiesTextureMap.Count == 0)
                return base.CalculateObjDesc(); // No Saved ObjDesc, let base handle it.

            var objDesc = new ObjDesc();

            AddBaseModelData(objDesc);

            foreach (var animPart in Biota.BiotaPropertiesAnimPart.OrderBy(b => b.Order))
                objDesc.AnimPartChanges.Add(new AnimationPartChange { PartIndex = animPart.Index, PartID = animPart.AnimationId });

            foreach (var subPalette in Biota.BiotaPropertiesPalette)
                objDesc.SubPalettes.Add(new SubPalette { SubID = subPalette.SubPaletteId, Offset = subPalette.Offset, NumColors = subPalette.Length });

            foreach (var textureMap in Biota.BiotaPropertiesTextureMap.OrderBy(b => b.Order))
                objDesc.TextureChanges.Add(new TextureMapChange { PartIndex = textureMap.Index, OldTexture = textureMap.OldId, NewTexture = textureMap.NewId });

            return objDesc;
        }

        /// <summary>
        /// Sets the decay time for player corpse
        /// </summary>
        public void SetDecayTime(Player player)
        {
            // a player corpse decays after 5 mins * playerLevel
            // with a minimum of 1 hour
            DecayTime = Math.Max(3600, (player.Level ?? 1) * 300);
        }

        /// <summary>
        /// Handles corpse decay and removal
        /// </summary>
        public override void HeartBeat()
        {
            DecayTime -= HeartbeatInterval ?? 5;

            if (DecayTime <= 0)
            {
                // if items are left on corpse,
                // create these items in the world

                // http://asheron.wikia.com/wiki/Item_Decay

                Destroy();
                return;
            }
            QueueNextHeartBeat();
        }
    }
}
