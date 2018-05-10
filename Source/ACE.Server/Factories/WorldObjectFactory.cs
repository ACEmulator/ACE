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
        public static WorldObject CreateWorldObject(Weenie weenie, ObjectGuid guid, uint spellId = 0)
        {
            var objWeenieType = (WeenieType)weenie.Type;

            switch (objWeenieType)
            {
                case WeenieType.Undef:
                    return null;
                case WeenieType.LifeStone:
                    return new Lifestone(weenie, guid);
                case WeenieType.Door:
                    return new Door(weenie, guid);
                case WeenieType.Portal:
                    return new Portal(weenie, guid);
                case WeenieType.Book:
                    return new Book(weenie, guid);
                case WeenieType.PKModifier:
                    return new PKModifier(weenie, guid);
                case WeenieType.Cow:
                    return new Cow(weenie, guid);
                case WeenieType.Creature:
                    return new Creature(weenie, guid);
                case WeenieType.Container:
                    return new Container(weenie, guid);
                case WeenieType.Scroll:
                    return new Scroll(weenie, guid);
                case WeenieType.Vendor:
                    return new Vendor(weenie, guid);
                case WeenieType.Coin:
                    return new Coin(weenie, guid);
                case WeenieType.Key:
                    return new Key(weenie, guid);
                case WeenieType.Food:
                    return new Food(weenie, guid);
                case WeenieType.Gem:
                    return new Gem(weenie, guid);
                case WeenieType.Game:
                    return new Game(weenie, guid);
                case WeenieType.GamePiece:
                    return new GamePiece(weenie, guid);
                case WeenieType.AllegianceBindstone:
                    return new Bindstone(weenie, guid);
                case WeenieType.Clothing:
                    return new Clothing(weenie, guid);
                case WeenieType.MeleeWeapon:
                    return new MeleeWeapon(weenie, guid);
                case WeenieType.MissileLauncher:
                    return new MissileLauncher(weenie, guid);
                case WeenieType.Ammunition:
                    return new Ammunition(weenie, guid);
                case WeenieType.Missile:
                    return new Missile(weenie, guid);
                case WeenieType.Corpse:
                    return new Corpse(weenie, guid);
                case WeenieType.Chest:
                    return new Chest(weenie, guid);
                case WeenieType.Stackable:
                    return new Stackable(weenie, guid);
                case WeenieType.SpellComponent:
                    return new SpellComponent(weenie, guid);
                case WeenieType.Switch:
                    return new Switch(weenie, guid);
                case WeenieType.AdvocateFane:
                    return new AdvocateFane(weenie, guid);
                case WeenieType.AdvocateItem:
                    return new AdvocateItem(weenie, guid);
                case WeenieType.Healer:
                    return new Healer(weenie, guid);
                case WeenieType.Lockpick:
                    return new Lockpick(weenie, guid);
                case WeenieType.Caster:
                    return new Caster(weenie, guid);
                case WeenieType.ProjectileSpell:
                    return new SpellProjectile(weenie, guid, spellId);
                default:
                    return new GenericObject(weenie, guid);
            }
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

                    foreach (var link in sourceObjects.Where(x => x.LinkSlot == instance.LinkSlot))
                        worldObject.LinkedInstances.Add(link);

                    results.Add(worldObject);
                }
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

        /// <summary>
        /// This will create a new SpellProjectile WorldObject with a new GUID.
        /// It will return null if weenieClassId was not found.
        /// </summary>
        public static WorldObject CreateNewWorldObject(uint weenieClassId, uint spellId)
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);

            if (weenie == null)
                return null;

            return CreateNewWorldObject(weenie, spellId);
        }

        /// <summary>
        /// This will create a new SpellProjectile WorldObject with a new GUID.
        /// </summary>
        public static WorldObject CreateNewWorldObject(Weenie weenie, uint spellId)
        {
            var worldObject = CreateWorldObject(weenie, GuidManager.NewDynamicGuid(), spellId);
            return worldObject;
        }
    }
}
