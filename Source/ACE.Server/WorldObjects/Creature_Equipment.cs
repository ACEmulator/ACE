using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public bool EquippedObjectsLoaded { get; private set; }

        /// <summary>
        /// Use EquipObject() and DequipObject() to manipulate this dictionary..<para />
        /// Do not manipulate this dictionary directly.
        /// </summary>
        public Dictionary<ObjectGuid, WorldObject> EquippedObjects { get; } = new Dictionary<ObjectGuid, WorldObject>();

        /// <summary>
        /// The only time this should be used is to populate EquippedObjects from the ctor.
        /// </summary>
        protected void AddBiotasToEquippedObjects(IEnumerable<Biota> wieldedItems)
        {
            foreach (var biota in wieldedItems)
            {
                var worldObject = WorldObjectFactory.CreateWorldObject(biota);
                EquippedObjects[worldObject.Guid] = worldObject;

                EncumbranceVal += worldObject.Burden;
            }

            EquippedObjectsLoaded = true;
        }

        public bool HasWieldedItem(ObjectGuid objectGuid)
        {
            return EquippedObjects.ContainsKey(objectGuid);
        }

        /// <summary>
        /// Get Wielded Item. Returns null if not found.
        /// </summary>
        public WorldObject GetWieldedItem(ObjectGuid objectGuid)
        {
            return EquippedObjects.TryGetValue(objectGuid, out var item) ? item : null;
        }

        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.
        /// </summary>
        public bool TryEquipObject(WorldObject worldObject, int wieldedLocation)
        {
            // todo see if the wielded location is in use, if so, return false

            worldObject.SetProperty(PropertyInt.CurrentWieldedLocation, wieldedLocation);
            worldObject.SetProperty(PropertyInstanceId.Wielder, (int)Biota.Id);

            EquippedObjects[worldObject.Guid] = worldObject;

            return true;
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.
        /// It does not add it to inventory as you could be unwielding to the ground or a chest. Og II
        /// </summary>
        public bool TryDequipObject(ObjectGuid objectGuid, out WorldObject item)
        {
            if (EquippedObjects.Remove(objectGuid, out item))
            {
                item.RemoveProperty(PropertyInt.CurrentWieldedLocation);
                item.RemoveProperty(PropertyInstanceId.Wielder);

                return true;
            }

            return false;
        }








        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        public void GenerateWieldList()
        {
            foreach (var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Wield))
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo != null)
                {
                    if (item.Palette > 0)
                        wo.PaletteTemplate = item.Palette;
                    if (item.Shade > 0)
                        wo.Shade = item.Shade;

                    TryEquipObject(wo, (int)wo.ValidLocations);
                }
            }

            //if (EquippedObjects != null)
            //    UpdateBaseAppearance();
        }
    }
}
