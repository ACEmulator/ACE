using System;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Corpse : Container
    {
        /// <summary>
        /// The default number of seconds for a corpse to disappear
        /// </summary>
        public static readonly double DefaultDecayTime = 120.0;

        /// <summary>
        /// Flag indicates if a corpse is from a monster or a player
        /// </summary>
        public bool IsMonster = false;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Corpse(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            TimeToRot = DefaultDecayTime;

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

            var timeToRot = GetProperty(PropertyFloat.TimeToRot);
            //Console.WriteLine("Corpse.TimeToRot: " + timeToRot);
        }

        /// <summary>
        /// Sets the object description for a corpse
        /// </summary>
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
            TimeToRot = Math.Max(3600, (player.Level ?? 1) * 300);
        }

        /// <summary>
        /// The maximum number of seconds
        /// for an empty corpse to stick around
        /// </summary>
        public static readonly double EmptyDecayTime = 15.0;

        /// <summary>
        /// Handles corpse decay and removal
        /// </summary>
        public override void HeartBeat()
        {
            TimeToRot -= HeartbeatInterval ?? 5;

            // empty corpses decay faster?
            if (Inventory.Count == 0 && TimeToRot > EmptyDecayTime)
                TimeToRot = EmptyDecayTime;

            if (TimeToRot <= 0)
            {
                // TODO: if items are left on corpse,
                // create these items in the world
                // http://asheron.wikia.com/wiki/Item_Decay

                if (!IsMonster)
                    DatabaseManager.Shard.RemoveBiota(Biota, BiotaDatabaseLock, result => { });

                Destroy();
                return;
            }
            QueueNextHeartBeat();
        }

        /// <summary>
        /// Called when a player attempts to loot a corpse
        /// </summary>
        public override void Open(Player player)
        {
            // check for looting permission
            if (Name != null && Name.StartsWith("Corpse of "))
            {
                var corpseName = Name.Replace("Corpse of ", "");
                if (!player.Name.Equals(corpseName) && AllowedActivator != null && player.Guid.Full != AllowedActivator)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("You don't have permission to loot the " + Name, ChatMessageType.Broadcast));
                    return;
                }
            }
            base.Open(player);
        }
    }
}
