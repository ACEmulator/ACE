using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateArmor(int tier, bool isMagical, LootBias lootBias = LootBias.UnBiased)
        {
            int lowSpellTier = 0;
            int highSpellTier = 0;

            double armorModAcid = 0;
            double armorModBludge = 0;
            double armorModCold = 0;
            double armorModElectric = 0;
            double armorModFire = 0;
            double armorModNether = 0;
            double armorModPierce = 0;
            double armorModSlash = 0;

            int spellArray = 0;
            int cantripArray = 0;
            int equipSetId = 0;
            int wieldDifficulty = 0;

            int materialType = 0;
            int armorType = 0;
            int armorPieceType = 0;
            int armorWeenie = 0;

            switch (tier)
            {
                case 1:
                    lowSpellTier = 1;
                    highSpellTier = 3;
                    armorType = ThreadSafeRandom.Next(0, 3);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 11:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    // Chainmail
                    if (armorType == 2)
                    {
                        // Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Random Armor Items
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////bandana
                                armorWeenie = 28612;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 1:
                                ////beret
                                armorWeenie = 28605;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 2:
                                ////Cloth Cap
                                armorWeenie = 118;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 3:
                                ////Cloth gloves
                                armorWeenie = 121;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 4:
                                ////Cowl
                                armorWeenie = 119;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 5:
                                ////crown
                                armorWeenie = 296;
                                armorPieceType = 1;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 6:
                                ////Fez
                                armorWeenie = 5894;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 7:
                                ////hood
                                armorWeenie = 5905;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 8:
                                ////Kasa
                                armorWeenie = 5901;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 9:
                                ////Metal cap
                                armorWeenie = 46;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 10:
                                ////Qafiya
                                armorWeenie = 76;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 11:
                                ////turban
                                armorWeenie = 135;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 12:
                                ////loafers
                                armorWeenie = 28610;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 13:
                                ////sandals
                                armorWeenie = 129;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 14:
                                ////shoes
                                armorWeenie = 132;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 15:
                                ////slippers
                                armorWeenie = 133;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 16:
                                ////steel toed boots
                                armorWeenie = 7897;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(6, 1);
                                break;
                            case 17:
                                ////buckler
                                armorWeenie = 44;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 18:
                                ////kite shield
                                armorWeenie = 91;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 19:
                                ////large Kite Shield
                                armorWeenie = 92;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            default:
                                ////Round Shield
                                armorWeenie = 93;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                        }
                    }
                    break;
                case 2:
                    lowSpellTier = 3;
                    highSpellTier = 5;
                    armorType = ThreadSafeRandom.Next(0, 6);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Random Armor Items
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        switch (armorPiece)
                        {
                            case 0:
                                ////bandana
                                armorWeenie = 28612;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 1:
                                ////beret
                                armorWeenie = 28605;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 2:
                                ////Cloth Cap
                                armorWeenie = 118;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 3:
                                ////Cloth gloves
                                armorWeenie = 121;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 4:
                                ////Cowl
                                armorWeenie = 119;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 5:
                                ////crown
                                armorWeenie = 296;
                                armorPieceType = 1;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 6:
                                ////Fez
                                armorWeenie = 5894;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 7:
                                ////hood
                                armorWeenie = 5905;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 8:
                                ////Kasa
                                armorWeenie = 5901;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 9:
                                ////Metal cap
                                armorWeenie = 46;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 10:
                                ////Qafiya
                                armorWeenie = 76;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 11:
                                ////turban
                                armorWeenie = 135;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 12:
                                ////loafers
                                armorWeenie = 28610;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 13:
                                ////sandals
                                armorWeenie = 129;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 14:
                                ////shoes
                                armorWeenie = 132;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 15:
                                ////slippers
                                armorWeenie = 133;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 16:
                                ////steel toed boots
                                armorWeenie = 7897;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(6, 1);
                                break;
                            case 17:
                                ////buckler
                                armorWeenie = 44;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 18:
                                ////kite shield
                                armorWeenie = 91;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 19:
                                ////large Kite Shield
                                armorWeenie = 92;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 20:
                                ////Circlet
                                armorWeenie = 29528;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 21:
                                ////Armet
                                armorWeenie = 8488;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 22:
                                ////Baigha
                                armorWeenie = 550;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 23:
                                ////Heaume
                                armorWeenie = 74;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 24:
                                ////Helmet
                                armorWeenie = 75;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 25:
                                ////Kabuton
                                armorWeenie = 77;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 26:
                                ////sollerets
                                armorWeenie = 107;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 27:
                                ////viamontian laced boots
                                armorWeenie = 28611;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 28:
                                ////RTower Shield
                                armorWeenie = 95;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            default:
                                ////Round Shield
                                armorWeenie = 93;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                        }
                    }
                    break;
                case 3:
                    lowSpellTier = 4;
                    highSpellTier = 6;
                    armorType = ThreadSafeRandom.Next(0, 10);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 11);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 4:
                    lowSpellTier = 5;
                    highSpellTier = 6;
                    armorType = ThreadSafeRandom.Next(0, 10);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 5:
                    lowSpellTier = 5;
                    highSpellTier = 7;
                    armorType = ThreadSafeRandom.Next(0, 14);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 30950;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 28629;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 30951;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 28617;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            default:
                                armorWeenie = 28620;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 34)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 35)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                ///////
                ////

                case 6:
                    lowSpellTier = 6;
                    highSpellTier = 7;
                    armorType = ThreadSafeRandom.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28627;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 30950;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 28629;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 30951;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 28617;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            default:
                                armorWeenie = 28620;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    //////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43830;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 7:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    armorType = ThreadSafeRandom.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 30950;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28629;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 30951;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28617;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else
                        {
                            armorWeenie = 28620;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    //else if (armorPiece == 1)
                    //    //{
                    //    //    armorWeenie = 43830;
                    //    //    armorPieceType = 3;
                    //    //    spellArray = 5;
                    //    //    cantripArray = 5;
                    //    //}
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    wieldDifficulty = 150;
                    break;
                default:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    if (lootBias == LootBias.Armor) // Armor Mana Forge Chests don't include clothing type items
                        armorType = ThreadSafeRandom.Next(0, 14);
                    else
                        armorType = ThreadSafeRandom.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 30950;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 28629;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 30951;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 28617;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            default:
                                armorWeenie = 28620;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    //////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43830;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Alduressa
                    //if (armorType == 18)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37207;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37217;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37187;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 37200;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37195;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Amuli
                    //if (armorType == 19)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37208;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37299;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37188;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37201;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37196;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(6, 1);
                    //}
                    //////Olthoi Celdon
                    //if (armorType == 20)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 7);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37209;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37214;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37189;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37202;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 37192;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 37205;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37197;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Celdon
                    //if (armorType == 21)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37215;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37190;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37203;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37206;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37198;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        switch (armorPiece)
                        {
                            case 0:
                                ////bandana
                                armorWeenie = 28612;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 1:
                                ////beret
                                armorWeenie = 28605;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 2:
                                ////Cloth Cap
                                armorWeenie = 118;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 3:
                                ////Cloth gloves
                                armorWeenie = 121;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 4:
                                ////Cowl
                                armorWeenie = 119;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 5:
                                ////crown
                                armorWeenie = 296;
                                armorPieceType = 1;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 6:
                                ////Fez
                                armorWeenie = 5894;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 7:
                                ////hood
                                armorWeenie = 5905;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 8:
                                ////Kasa
                                armorWeenie = 5901;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 9:
                                ////Metal cap
                                armorWeenie = 46;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 10:
                                ////Qafiya
                                armorWeenie = 76;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 11:
                                ////turban
                                armorWeenie = 135;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 12:
                                ////loafers
                                armorWeenie = 28610;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 13:
                                ////sandals
                                armorWeenie = 129;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 14:
                                ////shoes
                                armorWeenie = 132;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 15:
                                ////slippers
                                armorWeenie = 133;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 16:
                                ////steel toed boots
                                armorWeenie = 7897;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(6, 1);
                                break;
                            case 17:
                                ////buckler
                                armorWeenie = 44;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 18:
                                ////kite shield
                                armorWeenie = 91;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 19:
                                ////large Kite Shield
                                armorWeenie = 92;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 20:
                                ////Circlet
                                armorWeenie = 29528;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 21:
                                ////Armet
                                armorWeenie = 8488;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 22:
                                ////Baigha
                                armorWeenie = 550;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 23:
                                ////Heaume
                                armorWeenie = 74;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 24:
                                ////Helmet
                                armorWeenie = 75;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 25:
                                ////Kabuton
                                armorWeenie = 77;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 26:
                                ////sollerets
                                armorWeenie = 107;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 27:
                                ////viamontian laced boots
                                armorWeenie = 28611;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 28:
                                ////RTower Shield
                                armorWeenie = 95;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            default:
                                ////Round Shield
                                armorWeenie = 93;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                        }
                    }
                    wieldDifficulty = 180;
                    break;
            }

            ////ArmorModVsSlash, with a value between 0.0-2.0.
            armorModSlash = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsPierce, with a value between 0.0-2.0.
            armorModPierce = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsBludgeon, with a value between 0.0-2.0.
            armorModBludge = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsCold, with a value between 0.0-2.0.
            armorModCold = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsFire, with a value between 0.0-2.0.
            armorModFire = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsAcid, with a value between 0.0-2.0.
            armorModAcid = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsElectric, with a value between 0.0-2.0.
            armorModElectric = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsNether, with a value between 0.0-2.0.
            armorModNether = .1 * ThreadSafeRandom.Next(1, 20);

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            wo.SetProperty(PropertyInt.AppraisalItemSkill, 7);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);

            wo.SetProperty(PropertyInt.MaterialType, materialType);

            int gemCount = ThreadSafeRandom.Next(1, 6);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship, LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.GemType)], LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.MaterialType)]));

            if (wieldDifficulty > 0)
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
            else
                wo.RemoveProperty(PropertyInt.WieldDifficulty);

            /////Setting random color
            wo.SetProperty(PropertyInt.PaletteTemplate, ThreadSafeRandom.Next(1, 2047));
            double shade = .1 * ThreadSafeRandom.Next(0, 9);
            wo.SetProperty(PropertyFloat.Shade, shade);

            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

            if (baseArmorLevel == 0)
                wo.RemoveProperty(PropertyInt.ArmorLevel);
            else
            {
                int adjustedArmorLevel = baseArmorLevel + GetArmorLevelModifier(tier, armorPieceType);
                wo.SetProperty(PropertyInt.ArmorLevel, adjustedArmorLevel);
            }

            wo.SetProperty(PropertyFloat.ArmorModVsSlash, armorModSlash);
            wo.SetProperty(PropertyFloat.ArmorModVsPierce, armorModPierce);
            wo.SetProperty(PropertyFloat.ArmorModVsCold, armorModCold);
            wo.SetProperty(PropertyFloat.ArmorModVsBludgeon, armorModBludge);
            wo.SetProperty(PropertyFloat.ArmorModVsFire, armorModFire);
            wo.SetProperty(PropertyFloat.ArmorModVsAcid, armorModAcid);
            wo.SetProperty(PropertyFloat.ArmorModVsElectric, armorModElectric);
            wo.SetProperty(PropertyFloat.ArmorModVsNether, armorModNether);
            wo.SetProperty(PropertyInt.EquipmentSetId, equipSetId);
            ////Encumberance will be added based on item in the future

            if (isMagical)
            {
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);
                int numSpells = GetNumSpells(tier);

                int spellcraft = GetSpellcraft(numSpells, tier);
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
                wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));

                int maxMana = GetMaxMana(numSpells, tier);
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, maxMana);

                int[][] spells;
                int[][] cantrips;
                switch (spellArray)
                {
                    case 1:
                        spells = LootTables.HeadSpells;
                        break;
                    case 2:
                        spells = LootTables.ChestSpells;
                        break;
                    case 3:
                        spells = LootTables.UpperArmSpells;
                        break;
                    case 4:
                        spells = LootTables.LowerArmSpells;
                        break;
                    case 5:
                        spells = LootTables.HandSpells;
                        break;
                    case 6:
                        spells = LootTables.AbdomenSpells;
                        break;
                    case 7:
                        spells = LootTables.UpperLegSpells;
                        break;
                    case 8:
                        spells = LootTables.LowerLegSpells;
                        break;
                    case 9:
                        spells = LootTables.FeetSpells;
                        break;
                    default:
                        spells = LootTables.ShieldSpells;
                        break;
                }

                switch (cantripArray)
                {
                    case 1:
                        cantrips = LootTables.HeadCantrips;
                        break;
                    case 2:
                        cantrips = LootTables.ChestCantrips;
                        break;
                    case 3:
                        cantrips = LootTables.UpperArmCantrips;
                        break;
                    case 4:
                        cantrips = LootTables.LowerArmCantrips;
                        break;
                    case 5:
                        cantrips = LootTables.HandCantrips;
                        break;
                    case 6:
                        cantrips = LootTables.AbdomenCantrips;
                        break;
                    case 7:
                        cantrips = LootTables.UpperLegCantrips;
                        break;
                    case 8:
                        cantrips = LootTables.LowerLegCantrips;
                        break;
                    case 9:
                        cantrips = LootTables.FeetCantrips;
                        break;
                    default:
                        cantrips = LootTables.ShieldCantrips;
                        break;
                }

                int[] shuffledValues = new int[spells.Length];
                for (int i = 0; i < spells.Length; i++)
                {
                    shuffledValues[i] = i;
                }

                Shuffle(shuffledValues);

                int minorCantrips = GetNumMinorCantrips(tier);
                int majorCantrips = GetNumMajorCantrips(tier);
                int epicCantrips = GetNumEpicCantrips(tier);
                int legendaryCantrips = GetNumLegendaryCantrips(tier);
                int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

                if (numSpells - numCantrips > 0)
                {
                    for (int a = 0; a < numSpells - numCantrips; a++)
                    {
                        int col = ThreadSafeRandom.Next(lowSpellTier - 1, highSpellTier - 1);
                        int spellID = spells[shuffledValues[a]][col];
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                }

                if (numCantrips > 0)
                {
                    shuffledValues = new int[cantrips.Length];
                    for (int i = 0; i < cantrips.Length; i++)
                    {
                        shuffledValues[i] = i;
                    }
                    Shuffle(shuffledValues);
                    int shuffledPlace = 0;
                    //minor cantripps
                    for (int a = 0; a < minorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                    //major cantrips
                    for (int a = 0; a < majorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                    // epic cantrips
                    for (int a = 0; a < epicCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                    //legendary cantrips
                    for (int a = 0; a < legendaryCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                }
            }
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            return wo;
        }

        private static int GetArmorLevelModifier(int tier, int armorType)
        {
            switch (tier)
            {
                case 1:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(10, 37);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(10, 33);
                    else
                        return ThreadSafeRandom.Next(10, 50);
                case 2:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(37, 72);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(34, 57);
                    else
                        return ThreadSafeRandom.Next(51, 90);
                case 3:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(73, 109);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(58, 82);
                    else
                        return ThreadSafeRandom.Next(92, 132);
                case 4:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(109, 145);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(82, 106);
                    else
                        return ThreadSafeRandom.Next(133, 173);
                case 5:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(145, 181);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(106, 130);
                    else
                        return ThreadSafeRandom.Next(173, 213);
                case 6:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(181, 217);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(130, 154);
                    else
                        return ThreadSafeRandom.Next(213, 254);
                case 7:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(217, 253);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(154, 178);
                    else
                        return ThreadSafeRandom.Next(254, 294);
                case 8:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(253, 304);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(178, 202);
                    else
                        return ThreadSafeRandom.Next(294, 335);
                default:
                    return 0;
            }
        }
    }
}
