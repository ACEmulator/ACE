using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class GenericObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            List<WorldObject> results = new List<WorldObject>();

            foreach (var aceO in sourceObjects)
            {
                ObjectType ot = (ObjectType)aceO.TypeId;
                ObjectDescriptionFlag oDescFlag = (ObjectDescriptionFlag)aceO.WdescBitField;

                if ((oDescFlag & ObjectDescriptionFlag.LifeStone) != 0)
                {
                    results.Add(new Lifestone(aceO));
                    continue;
                }
                // else if ((oDescFlag & ObjectDescriptionFlag.BindStone) != 0)
                // {
                //    results.Add(new Bindstone(aceO));
                //    continue;
                // }
                // else if ((oDescFlag & ObjectDescriptionFlag.PkSwitch) != 0)
                // {
                //    results.Add(new PKSwitch(aceO));
                //    continue;
                // }
                // else if ((oDescFlag & ObjectDescriptionFlag.NpkSwitch) != 0)
                // {
                //    results.Add(new NPKSwitch(aceO));
                //    continue;
                // }
                // else if ((oDescFlag & ObjectDescriptionFlag.LockPick) != 0)
                // {
                //    results.Add(new Lockpick(aceO));
                //    continue;
                // }
                // else if ((oDescFlag & ObjectDescriptionFlag.Food) != 0)
                // {
                //    results.Add(new Food(aceO));
                //    continue;
                // }
                // else if ((oDescFlag & ObjectDescriptionFlag.Healer) != 0)
                // {
                //    results.Add(new Healer(aceO));
                //    continue;
                // }
                // else if ((oDescFlag & ObjectDescriptionFlag.Book) != 0)
                // {
                //    results.Add(new Book(aceO));
                //    continue;
                // }
                else if ((oDescFlag & ObjectDescriptionFlag.Portal) != 0)
                {
                    AcePortalObject acePO = DatabaseManager.World.GetPortalObjectsByAceObjectId(aceO.AceObjectId);

                    results.Add(new Portal(acePO));
                    continue;
                }
                else if ((oDescFlag & ObjectDescriptionFlag.Door) != 0)
                {
                    results.Add(new Door(aceO));
                    continue;
                }
                else if ((oDescFlag & ObjectDescriptionFlag.Vendor) != 0)
                {
                    // results.Add(new Vendor(aceO));
                    // continue;
                    switch (aceO.WeenieClassId)
                    {
                        // case 5772:
                        // case 5773:
                        // case 5774:
                        // case 5775:
                        // case 5776:
                        // case 5777:
                        //    results.Add(new TownCrier(aceO));
                        //    break;
                        default:
                            results.Add(new Vendor(aceO));
                            break;
                    }
                    continue;
                }
                else
                {
                    // if (ot == ObjectType.Misc)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new SpecificMisc(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new GenericMisc(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Container)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.None)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Writable)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                     // //else if (ot == ObjectType.Creature)
                     if (ot == ObjectType.Creature)
                    {
                        switch (aceO.WeenieClassId)
                        {
                            case 5772:
                            case 5773:
                            case 5774:
                            case 5775:
                            case 5776:
                            case 5777:
                                results.Add(new TownCrier(aceO));
                                break;
                            case 6873:
                                results.Add(new AyanBaqurDrunkenScholar(aceO));
                                break;
                            case 29317:
                                results.Add(new AcademyGuardExitYaraq(aceO));
                                break;
                            case 29324:
                                results.Add(new AcademyGuardExitHoltburg(aceO));
                                break;
                            case 29325:
                                results.Add(new AcademyGuardExitSanamar(aceO));
                                break;
                            case 29326:
                                results.Add(new AcademyGuardExitShoushi(aceO));
                                break;
                            case 30991:
                                results.Add(new AcademyGreeter(aceO));
                                break;
                            default:
                                // results.Add(new Generic(aceO));
                                // results.Add(new Creature(aceO));
                                results.Add(new DebugObject(aceO));
                                break;
                        }
                        continue;
                    }
                    // else if (ot == ObjectType.Armor)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.MeleeWeapon)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Clothing)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Jewelry)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Food)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Money)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.MissileWeapon)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Gem)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.SpellComponents)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Key)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.Caster)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.ManaStone)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.PromissoryNote)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.CraftAlchemyBase)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.CraftAlchemyIntermediate)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.CraftCookingBase)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.CraftFletchingBase)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else if (ot == ObjectType.CraftFletchingIntermediate)
                    // {
                    //    switch (aceO.WeenieClassId)
                    //    {
                    //        case 1111:
                    //            results.Add(new Specific(aceO));
                    //            break;
                    //        default:
                    //            results.Add(new Generic(aceO));
                    //            break;
                    //    }
                    //    continue;
                    // }
                    // else
                    // {
#if DEBUG
                    // Use the DebugObject to assist in building proper objects for weenies
                    results.Add(new DebugObject(aceO));
#endif
                    // }
                }
            }
            return results;
        }
    }
}
