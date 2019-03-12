using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

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
            if (weenie == null || guid == null)
                return null;

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
                    return new SpellProjectile(weenie, guid);
                case WeenieType.HotSpot:
                    return new Hotspot(weenie, guid);
                case WeenieType.ManaStone:
                    return new ManaStone(weenie, guid);
                case WeenieType.House:
                    return new House(weenie, guid);
                case WeenieType.SlumLord:
                    return new SlumLord(weenie, guid);
                case WeenieType.Storage:
                    return new Storage(weenie, guid);
                case WeenieType.Hook:
                    return new Hook(weenie, guid);
                case WeenieType.Hooker:
                    return new Hooker(weenie, guid);
                case WeenieType.HousePortal:
                    return new WorldObjects.HousePortal(weenie, guid);
                case WeenieType.SkillAlterationDevice:
                    return new SkillAlterationDevice(weenie, guid);
                case WeenieType.PressurePlate:
                    return new PressurePlate(weenie, guid);
                case WeenieType.PetDevice:
                    return new PetDevice(weenie, guid);
                case WeenieType.Pet:
                    return new Pet(weenie, guid);
                case WeenieType.CombatPet:
                    return new CombatPet(weenie, guid);
                case WeenieType.Allegiance:
                    return new Allegiance(weenie, guid);
                case WeenieType.AugmentationDevice:
                    return new AugmentationDevice(weenie, guid);
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
                case WeenieType.HotSpot:
                    return new Hotspot(biota);
                case WeenieType.ManaStone:
                    return new ManaStone(biota);
                case WeenieType.House:
                    return new House(biota);
                case WeenieType.SlumLord:
                    return new SlumLord(biota);
                case WeenieType.Storage:
                    return new Storage(biota);
                case WeenieType.Hook:
                    return new Hook(biota);
                case WeenieType.Hooker:
                    return new Hooker(biota);
                case WeenieType.HousePortal:
                    return new WorldObjects.HousePortal(biota);
                case WeenieType.SkillAlterationDevice:
                    return new SkillAlterationDevice(biota);
                case WeenieType.PressurePlate:
                    return new PressurePlate(biota);
                case WeenieType.PetDevice:
                    return new PetDevice(biota);
                case WeenieType.Pet:
                    return new Pet(biota);
                case WeenieType.CombatPet:
                    return new CombatPet(biota);
                case WeenieType.Allegiance:
                    return new Allegiance(biota);
                case WeenieType.AugmentationDevice:
                    return new AugmentationDevice(biota);
                default:
                    return new GenericObject(biota);
            }
        }

        /// <summary>
        /// This will create a list of WorldObjects, all with new GUIDs and for every position provided.
        /// </summary>
        public static List<WorldObject> CreateNewWorldObjects(List<LandblockInstance> sourceObjects, List<Biota> biotas, uint? restrict_wcid = null)
        {
            var results = new List<WorldObject>();

            // spawn direct landblock objects
            foreach (var instance in sourceObjects.Where(x => x.IsLinkChild == false))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(instance.WeenieClassId);

                if (weenie == null)
                    continue;

                if (restrict_wcid != null && restrict_wcid.Value != instance.WeenieClassId)
                    continue;

                var guid = new ObjectGuid(instance.Guid);

                WorldObject worldObject;

                var biota = biotas.FirstOrDefault(b => b.Id == instance.Guid);
                if (biota == null)
                {
                    worldObject = CreateWorldObject(weenie, guid);

                    worldObject.Location = new Position(instance.ObjCellId, instance.OriginX, instance.OriginY, instance.OriginZ, instance.AnglesX, instance.AnglesY, instance.AnglesZ, instance.AnglesW);
                }
                else
                {
                    worldObject = CreateWorldObject(biota);
                }

                if (worldObject != null)
                {
                    // queue linked child objects
                    foreach (var link in instance.LandblockInstanceLink)
                    {
                        var linkInstance = sourceObjects.FirstOrDefault(x => x.Guid == link.ChildGuid);

                        if (linkInstance != null)
                            worldObject.LinkedInstances.Add(linkInstance);
                    }

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
            var weenie = DatabaseManager.World.GetCachedWeenie(weenieClassName);

            if (weenie == null)
                return null;

            return CreateNewWorldObject(weenie.ClassId);
        }
    }
}
