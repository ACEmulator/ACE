using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Common;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using Org.BouncyCastle.Asn1.X509;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public string TinkeringLotto_Mutate(string salvageType, int salvageWorkmanship)
        {
            var resultMessage = "";

            switch(salvageType)
            {
                case "Steel":
                    resultMessage = TinkeringLotto_PlaySteelLottery(salvageWorkmanship);
                    break;

                case "Iron":
                    resultMessage = TinkeringLotto_PlayIronLottery(salvageWorkmanship);
                    break;

                case "Granite":
                    resultMessage = TinkeringLotto_PlayGraniteLottery(salvageWorkmanship);
                    break;

                case "Green Garnet":
                    resultMessage = TinkeringLotto_PlayGreenGarnetLottery(salvageWorkmanship);
                    break;

                case "Mahogany":
                    resultMessage = TinkeringLotto_PlayMahoganyLottery(salvageWorkmanship);
                    break;
                default:
                    return "";
            }

            return resultMessage;
        }

        private string TinkeringLotto_PlaySteelLottery(int salvageWorkmanship)
        {
            string resultMsg = "";
            
            Random rand = new Random();
            var roll = rand.NextDouble();

            if(roll < 0.025)
            {
                //Add 20 AL
                this.ArmorLevel += 20;
                resultMsg = "Jackpot! Improved Armor Level by 20";
            }
            else if (roll < 0.075)
            {
                //Add between 1 - 5 AL
                var alBonus = rand.Next(1, 6);
                this.ArmorLevel += alBonus;
                resultMsg = $"Improved Armor Level by {alBonus}";
            }

            return resultMsg;
        }

        private string TinkeringLotto_PlayIronLottery(int salvageWorkmanship)
        {
            //Iron - chance to add extra + 1 damage, chance to add slayers(I think I did this section in the code), chance to add rend for that element type.

            string resultMsg = "";

            Random rand = new Random();
            var dmgBonusroll = rand.NextDouble();

            if (dmgBonusroll < 0.05)
            {
                //Add +1 dmg
                this.Damage += 1;
                resultMsg = "Improved Damage by 1";
            }

            var slayerRoll = rand.NextDouble();
            if (slayerRoll < 0.025)
            {
                if (!this.SlayerCreatureType.HasValue)
                {
                    var slayerResult = TinkeringLotto_ApplySlayerMutation();
                    if (!string.IsNullOrEmpty(slayerResult))
                        resultMsg = string.IsNullOrEmpty(resultMsg) ? slayerResult : $"{resultMsg}\n{slayerResult}";
                }
            }

            var rendRoll = rand.NextDouble();
            if (rendRoll < 0.025)
            {
                var rendResult = TinkeringLotto_ApplyRendMutation();
                if (!string.IsNullOrEmpty(rendResult))
                    resultMsg = string.IsNullOrEmpty(resultMsg) ? rendResult : $"{resultMsg}\n{rendResult}";
            }

            return resultMsg;
        }


        private string TinkeringLotto_PlayGraniteLottery(int salvageWorkmanship)
        {
            //Granite - chance to add improve variance by equivalent of extra bag of granite, chance to add slayers(I think I did this section in the code), chance to add rend for that element type.

            string resultMsg = "";

            Random rand = new Random();
            var varianceBonusroll = rand.NextDouble();

            if (varianceBonusroll < 0.05)
            {
                //Add +1 variance
                this.DamageVariance *= 0.8f;
                resultMsg = "Improved Damage Variance by 20%";
            }

            var slayerRoll = rand.NextDouble();
            if (slayerRoll < 0.025)
            {
                if (!this.SlayerCreatureType.HasValue)
                {
                    var slayerResult = TinkeringLotto_ApplySlayerMutation();
                    if (!string.IsNullOrEmpty(slayerResult))
                        resultMsg = string.IsNullOrEmpty(resultMsg) ? slayerResult : $"{resultMsg}\n{slayerResult}";
                }
            }

            var rendRoll = rand.NextDouble();
            if (rendRoll < 0.025)
            {
                var rendResult = TinkeringLotto_ApplyRendMutation();
                if (!string.IsNullOrEmpty(rendResult))
                    resultMsg = string.IsNullOrEmpty(resultMsg) ? rendResult : $"{resultMsg}\n{rendResult}";
            }

            return resultMsg;
        }

        private string TinkeringLotto_PlayGreenGarnetLottery(int salvageWorkmanship)
        {
            //Green Garnet - chance to add extra + 1% damage, chance to add slayers (I think I did this section in the code), chance to add rend for that element type. 

            string resultMsg = "";

            Random rand = new Random();
            var dmgBonusroll = rand.NextDouble();

            if (dmgBonusroll < 0.05)
            {
                //Add +1% dmg bonus
                this.ElementalDamageMod = (this.ElementalDamageMod ?? 0.0f) + 0.01f;
                resultMsg = "Improved Elemental Damage Bonus by 1% vs Monsters and 0.25% against Players";
            }

            var slayerRoll = rand.NextDouble();
            if (slayerRoll < 0.025)
            {
                if (!this.SlayerCreatureType.HasValue)
                {
                    var slayerResult = TinkeringLotto_ApplySlayerMutation();
                    if (!string.IsNullOrEmpty(slayerResult))
                        resultMsg = string.IsNullOrEmpty(resultMsg) ? slayerResult : $"{resultMsg}\n{slayerResult}";
                }
            }

            var rendRoll = rand.NextDouble();
            if (rendRoll < 0.025)
            {
                var rendResult = TinkeringLotto_ApplyRendMutation();
                if (!string.IsNullOrEmpty(rendResult))
                    resultMsg = string.IsNullOrEmpty(resultMsg) ? rendResult : $"{resultMsg}\n{rendResult}";
            }

            return resultMsg;
        }

        private string TinkeringLotto_PlayMahoganyLottery(int salvageWorkmanship)
        {
            //Mahogany - chance to add extra + 4% damage mod, chance to add slayers (I think I did this section in the code), chance to add rend for that element type.

            string resultMsg = "";

            Random rand = new Random();
            var dmgBonusroll = rand.NextDouble();

            if (dmgBonusroll < 0.05)
            {
                //Add +4% dmg mod
                this.DamageMod += 0.04f;
                resultMsg = "Improved Missile Damage Mod by 4%";
            }

            var slayerRoll = rand.NextDouble();
            if (slayerRoll < 0.025)
            {
                if (!this.SlayerCreatureType.HasValue)
                {
                    var slayerResult = TinkeringLotto_ApplySlayerMutation();
                    if (!string.IsNullOrEmpty(slayerResult))
                        resultMsg = string.IsNullOrEmpty(resultMsg) ? slayerResult : $"{resultMsg}\n{slayerResult}";
                }
            }

            var rendRoll = rand.NextDouble();
            if (rendRoll < 0.025)
            {
                var rendResult = TinkeringLotto_ApplyRendMutation();
                if(!string.IsNullOrEmpty(rendResult))
                    resultMsg = string.IsNullOrEmpty(resultMsg) ? rendResult : $"{resultMsg}\n{rendResult}";
            }

            return resultMsg;
        }


        private string TinkeringLotto_ApplySlayerMutation()
        {
            var selectSlayerType = ThreadSafeRandom.Next(1, 19);
            this.SlayerDamageBonus = 1.20f;

            switch (selectSlayerType)
            {
                case 1:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Banderling;
                    break;

                case 2:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Drudge;
                    break;

                case 3:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Gromnie;
                    break;

                case 4:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Lugian;
                    break;

                case 5:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Grievver;
                    break;

                case 6:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mattekar;
                    break;

                case 7:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mite;
                    break;

                case 8:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mosswart;
                    break;

                case 9:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mumiyah;
                    break;

                case 10:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Olthoi;
                    break;

                case 11:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.PhyntosWasp;
                    break;

                case 12:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shadow;
                    break;

                case 13:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shreth;
                    break;

                case 14:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Skeleton;
                    break;

                case 15:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tumerok;
                    break;

                case 16:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tusker;
                    break;

                case 17:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Virindi;
                    break;

                case 18:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Wisp;
                    break;

                case 19:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Zefir;
                    break;

                default:
                    return "";
            }

            return $"Added {this.SlayerCreatureType} slayer";
        }

        private string TinkeringLotto_ApplyRendMutation()
        {
            string resultMsg = "";

            switch (this.W_DamageType)
            {
                case DamageType.Acid: // acid
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.AcidRending))
                    {
                        this.ImbuedEffect |= ImbuedEffectType.AcidRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x06003355);
                        resultMsg = $"Added Acid Rending";
                    }
                    break;

                case DamageType.Cold: // Cold
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.ColdRending))
                    {
                        this.ImbuedEffect |= ImbuedEffectType.ColdRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x06003353);
                        resultMsg = $"Added Cold Rending";
                    }
                    break;

                case DamageType.Electric: // Electric
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.ElectricRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.ElectricRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x06003354);
                        resultMsg = $"Added Lightning Rending";
                    }
                    break;

                case DamageType.Fire: // Fire
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.FireRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.FireRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x06003359);
                        resultMsg = $"Added Fire Rending";
                    }
                    break;

                case DamageType.Pierce: // Pierce
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.PierceRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.PierceRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335b);
                        resultMsg = $"Added Pierce Rending";
                    }
                    break;

                case DamageType.Slash: // Slash
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.SlashRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.SlashRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335c);
                        resultMsg = $"Added Slash Rending";
                    }
                    break;

                case DamageType.Bludgeon: // Bludgeon
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.BludgeonRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.BludgeonRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335a);
                        resultMsg = $"Added Bludgeon Rending";
                    }
                    break;

                case DamageType.Pierce | DamageType.Slash:
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.PierceRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.PierceRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335b);
                        resultMsg = $"Added Pierce and Slash Rending";
                    }
                    if (!this.ImbuedEffect.HasFlag(ImbuedEffectType.SlashRending))
                    {
                        this.ImbuedEffect = ImbuedEffectType.SlashRending;
                        this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335c);
                        resultMsg = $"Added Pierce and Slash Rending";
                    }
                    break;
            }

            return resultMsg;
        }
    }
}
