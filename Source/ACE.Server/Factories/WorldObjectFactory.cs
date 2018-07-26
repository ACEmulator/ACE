using System;
using System.Collections.Generic;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using System.Linq;

namespace ACE.Server.Factories
{
    public static class WorldObjectFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public static WorldObject CreateWorldObject(Weenie weenie, ObjectGuid guid)
        {
            var objWeenieType = (WeenieType)weenie.Type;

            WorldObject wo = null;

            switch (objWeenieType)
            {
                case WeenieType.Undef:
                    return null;
                case WeenieType.LifeStone:
                    wo = new Lifestone(weenie, guid); break;
                case WeenieType.Door:
                    wo = new Door(weenie, guid); break;
                case WeenieType.Portal:
                    wo = new Portal(weenie, guid); break;
                case WeenieType.Book:
                    wo = new Book(weenie, guid); break;
                case WeenieType.PKModifier:
                    wo = new PKModifier(weenie, guid); break;
                case WeenieType.Cow:
                    wo = new Cow(weenie, guid); break;
                case WeenieType.Creature:
                    wo = new Creature(weenie, guid); break;
                case WeenieType.Container:
                    wo = new Container(weenie, guid); break;
                case WeenieType.Scroll:
                    wo = new Scroll(weenie, guid); break;
                case WeenieType.Vendor:
                    wo = new Vendor(weenie, guid); break;
                case WeenieType.Coin:
                    wo = new Coin(weenie, guid); break;
                case WeenieType.Key:
                    wo = new Key(weenie, guid); break;
                case WeenieType.Food:
                    wo = new Food(weenie, guid); break;
                case WeenieType.Gem:
                    wo = new Gem(weenie, guid); break;
                case WeenieType.Game:
                    wo = new Game(weenie, guid); break;
                case WeenieType.GamePiece:
                    wo = new GamePiece(weenie, guid); break;
                case WeenieType.AllegianceBindstone:
                    wo = new Bindstone(weenie, guid); break;
                case WeenieType.Clothing:
                    wo = new Clothing(weenie, guid); break;
                case WeenieType.MeleeWeapon:
                    wo = new MeleeWeapon(weenie, guid); break;
                case WeenieType.MissileLauncher:
                    wo = new MissileLauncher(weenie, guid); break;
                case WeenieType.Ammunition:
                    wo = new Ammunition(weenie, guid); break;
                case WeenieType.Missile:
                    wo = new Missile(weenie, guid); break;
                case WeenieType.Corpse:
                    wo = new Corpse(weenie, guid); break;
                case WeenieType.Chest:
                    wo = new Chest(weenie, guid); break;
                case WeenieType.Stackable:
                    wo = new Stackable(weenie, guid); break;
                case WeenieType.SpellComponent:
                    wo = new SpellComponent(weenie, guid); break;
                case WeenieType.Switch:
                    wo = new Switch(weenie, guid); break;
                case WeenieType.AdvocateFane:
                    wo = new AdvocateFane(weenie, guid); break;
                case WeenieType.AdvocateItem:
                    wo = new AdvocateItem(weenie, guid); break;
                case WeenieType.Healer:
                    wo = new Healer(weenie, guid); break;
                case WeenieType.Lockpick:
                    wo = new Lockpick(weenie, guid); break;
                case WeenieType.Caster:
                    wo = new Caster(weenie, guid); break;
                case WeenieType.ProjectileSpell:
                    wo = new SpellProjectile(weenie, guid); break;
                case WeenieType.HotSpot:
                    wo = new Hotspot(weenie, guid); break;
                case WeenieType.ManaStone:
                    wo = new ManaStone(weenie, guid); break;
                default:
                    wo = new GenericObject(weenie, guid); break;
            }

            wo.PostInit();
            return wo;
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        public static WorldObject CreateWorldObject(Biota biota)
        {
            var objWeenieType = (WeenieType)biota.WeenieType;

            switch (objWeenieType)
            {
                case WeenieType.Undef:
                    return null;
                case WeenieType.LifeStone:
                    return new Lifestone(biota);
                case WeenieType.Door:
                    return new Door(biota);
                case WeenieType.Portal:
                    return new Portal(biota);
                case WeenieType.Book:
                    return new Book(biota);
                case WeenieType.PKModifier:
                    return new PKModifier(biota);
                case WeenieType.Cow:
                    return new Cow(biota);
                case WeenieType.Creature:
                    return new Creature(biota);
                case WeenieType.Container:
                    return new Container(biota);
                case WeenieType.Scroll:
                    return new Scroll(biota);
                case WeenieType.Vendor:
                    return new Vendor(biota);
                case WeenieType.Coin:
                    return new Coin(biota);
                case WeenieType.Key:
                    return new Key(biota);
                case WeenieType.Food:
                    return new Food(biota);
                case WeenieType.Gem:
                    return new Gem(biota);
                case WeenieType.Game:
                    return new Game(biota);
                case WeenieType.GamePiece:
                    return new GamePiece(biota);
                case WeenieType.AllegianceBindstone:
                    return new Bindstone(biota);
                case WeenieType.Clothing:
                    return new Clothing(biota);
                case WeenieType.MeleeWeapon:
                    return new MeleeWeapon(biota);
                case WeenieType.MissileLauncher:
                    return new MissileLauncher(biota);
                case WeenieType.Ammunition:
                    return new Ammunition(biota);
                case WeenieType.Missile:
                    return new Missile(biota);
                case WeenieType.Corpse:
                    return new Corpse(biota);
                case WeenieType.Chest:
                    return new Chest(biota);
                case WeenieType.Stackable:
                    return new Stackable(biota);
                case WeenieType.SpellComponent:
                    return new SpellComponent(biota);
                case WeenieType.Switch:
                    return new Switch(biota);
                case WeenieType.AdvocateFane:
                    return new AdvocateFane(biota);
                case WeenieType.AdvocateItem:
                    return new AdvocateItem(biota);
                case WeenieType.Healer:
                    return new Healer(biota);
                case WeenieType.Lockpick:
                    return new Lockpick(biota);
                case WeenieType.Caster:
                    return new Caster(biota);
                case WeenieType.HotSpot:
                    return new Hotspot(biota);
                case WeenieType.ManaStone:
                    return new ManaStone(biota);
                default:
                    return new GenericObject(biota);
            }
        }

        /// <summary>
        /// This will create a list of WorldObjects, all with new GUIDs and for every position provided.
        /// </summary>
        public static List<WorldObject> CreateNewWorldObjects(List<LandblockInstances> sourceObjects)
        {
            var results = new List<WorldObject>();

            foreach (var instance in sourceObjects.Where(x => x.LinkSlot is null))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(instance.WeenieClassId);

                if (weenie == null)
                    continue;

                ObjectGuid guid;

                if (instance.Guid != 0)
                    guid = new ObjectGuid(instance.Guid);
                else
                    guid = GuidManager.NewDynamicGuid();

                var worldObject = CreateWorldObject(weenie, guid);

                if (worldObject != null)
                {
                    worldObject.Location = new Position(instance.ObjCellId, instance.OriginX, instance.OriginY, instance.OriginZ, instance.AnglesX, instance.AnglesY, instance.AnglesZ, instance.AnglesW);

                    results.Add(worldObject);
                }
            }

            foreach (var instance in sourceObjects.Where(x => !(x.LinkController is null)))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(instance.WeenieClassId);

                if (weenie == null)
                    continue;

                ObjectGuid guid;

                if (instance.Guid != 0)
                    guid = new ObjectGuid(instance.Guid);
                else
                    guid = GuidManager.NewDynamicGuid();

                var worldObject = CreateWorldObject(weenie, guid);

                if (worldObject != null)
                {
                    worldObject.Location = new Position(instance.ObjCellId, instance.OriginX, instance.OriginY, instance.OriginZ, instance.AnglesX, instance.AnglesY, instance.AnglesZ, instance.AnglesW);

                    foreach (var link in sourceObjects.Where(x => x.LinkSlot == instance.LinkSlot && (x.LinkController ?? false) == false))
                        worldObject.LinkedInstances.Add(link);

                    results.Add(worldObject);
                }
            }

            return results;
        }

        /// <summary>
        /// Creates a list of WorldObjects from a list of Biotas
        /// </summary>
        public static List<WorldObject> CreateWorldObjects(List<Biota> biotas)
        {
            var results = new List<WorldObject>();

            foreach (var biota in biotas)
            {
                var worldObject = CreateWorldObject(biota);
                //worldObject.CalculateObjDesc();

                if (worldObject != null)
                    results.Add(worldObject);
            }
            return results;
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// </summary>
        public static WorldObject CreateNewWorldObject(Weenie weenie)
        {
            var worldObject = CreateWorldObject(weenie, GuidManager.NewDynamicGuid());

            return worldObject;
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// It will return null if weenieClassId was not found.
        /// </summary>
        public static WorldObject CreateNewWorldObject(uint weenieClassId)
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);

            if (weenie == null)
                return null;

            return CreateNewWorldObject(weenie);
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// It will return null if weenieClassName was not found.
        /// </summary>
        public static WorldObject CreateNewWorldObject(string weenieClassName)
        {
            var classId = DatabaseManager.World.GetWeenieClassId(weenieClassName);

            if (classId == 0)
                return null;

            return CreateNewWorldObject(classId);
        }
    }
}
