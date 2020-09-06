using System.Collections.Generic;

using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class SpellLevelProgression
    {
        private static readonly Dictionary<SpellId, List<SpellId>> spellProgression = new Dictionary<SpellId, List<SpellId>>();

        public static readonly List<SpellId> StrengthOther = new List<SpellId>()
        {
            SpellId.StrengthOther1,
            SpellId.StrengthOther2,
            SpellId.StrengthOther3,
            SpellId.StrengthOther4,
            SpellId.StrengthOther5,
            SpellId.StrengthOther6,
            SpellId.StrengthOther7,
            SpellId.StrengthOther8,
        };

        public static readonly List<SpellId> StrengthSelf = new List<SpellId>()
        {
            SpellId.StrengthSelf1,
            SpellId.StrengthSelf2,
            SpellId.StrengthSelf3,
            SpellId.StrengthSelf4,
            SpellId.StrengthSelf5,
            SpellId.StrengthSelf6,
            SpellId.StrengthSelf7,
            SpellId.StrengthSelf8,
        };

        public static readonly List<SpellId> WeaknessOther = new List<SpellId>()
        {
            SpellId.WeaknessOther1,
            SpellId.WeaknessOther2,
            SpellId.WeaknessOther3,
            SpellId.WeaknessOther4,
            SpellId.WeaknessOther5,
            SpellId.WeaknessOther6,
            SpellId.WeaknessOther7,
            SpellId.WeaknessOther8,
        };

        public static readonly List<SpellId> WeaknessSelf = new List<SpellId>()
        {
            SpellId.WeaknessSelf1,
            SpellId.WeaknessSelf2,
            SpellId.WeaknessSelf3,
            SpellId.WeaknessSelf4,
            SpellId.WeaknessSelf5,
            SpellId.WeaknessSelf6,
            SpellId.WeaknessSelf7,
            SpellId.WeaknessSelf8,
        };

        public static readonly List<SpellId> HealOther = new List<SpellId>()
        {
            SpellId.HealOther1,
            SpellId.HealOther2,
            SpellId.HealOther3,
            SpellId.HealOther4,
            SpellId.HealOther5,
            SpellId.HealOther6,
            SpellId.HealOther7,
            SpellId.HealOther8,
        };

        public static readonly List<SpellId> HealSelf = new List<SpellId>()
        {
            SpellId.HealSelf1,
            SpellId.HealSelf2,
            SpellId.HealSelf3,
            SpellId.HealSelf4,
            SpellId.HealSelf5,
            SpellId.HealSelf6,
            SpellId.HealSelf7,
            SpellId.HealSelf8,
        };

        public static readonly List<SpellId> HarmOther = new List<SpellId>()
        {
            SpellId.HarmOther1,
            SpellId.HarmOther2,
            SpellId.HarmOther3,
            SpellId.HarmOther4,
            SpellId.HarmOther5,
            SpellId.HarmOther6,
            SpellId.HarmOther7,
            SpellId.HarmOther8,
        };

        public static readonly List<SpellId> HarmSelf = new List<SpellId>()
        {
            SpellId.HarmSelf1,
            SpellId.HarmSelf2,
            SpellId.HarmSelf3,
            SpellId.HarmSelf4,
            SpellId.HarmSelf5,
            SpellId.HarmSelf6,
            SpellId.HarmSelf7,
            SpellId.HarmSelf8,
        };

        public static readonly List<SpellId> InfuseMana = new List<SpellId>()
        {
            SpellId.InfuseMana1,
            SpellId.InfuseMana2,
            SpellId.InfuseMana3,
            SpellId.InfuseMana4,
            SpellId.InfuseMana5,
            SpellId.InfuseMana6,
            SpellId.InfuseMana7,
            SpellId.InfuseMana8,
        };

        public static readonly List<SpellId> VulnerabilityOther = new List<SpellId>()
        {
            SpellId.VulnerabilityOther1,
            SpellId.VulnerabilityOther2,
            SpellId.VulnerabilityOther3,
            SpellId.VulnerabilityOther4,
            SpellId.VulnerabilityOther5,
            SpellId.VulnerabilityOther6,
            SpellId.VulnerabilityOther7,
            SpellId.VulnerabilityOther8,
        };

        public static readonly List<SpellId> VulnerabilitySelf = new List<SpellId>()
        {
            SpellId.VulnerabilitySelf1,
            SpellId.VulnerabilitySelf2,
            SpellId.VulnerabilitySelf3,
            SpellId.VulnerabilitySelf4,
            SpellId.VulnerabilitySelf5,
            SpellId.VulnerabilitySelf6,
            SpellId.VulnerabilitySelf7,
            SpellId.VulnerabilitySelf8,
        };

        public static readonly List<SpellId> InvulnerabilityOther = new List<SpellId>()
        {
            SpellId.InvulnerabilityOther1,
            SpellId.InvulnerabilityOther2,
            SpellId.InvulnerabilityOther3,
            SpellId.InvulnerabilityOther4,
            SpellId.InvulnerabilityOther5,
            SpellId.InvulnerabilityOther6,
            SpellId.InvulnerabilityOther7,
            SpellId.InvulnerabilityOther8,
        };

        public static readonly List<SpellId> InvulnerabilitySelf = new List<SpellId>()
        {
            SpellId.InvulnerabilitySelf1,
            SpellId.InvulnerabilitySelf2,
            SpellId.InvulnerabilitySelf3,
            SpellId.InvulnerabilitySelf4,
            SpellId.InvulnerabilitySelf5,
            SpellId.InvulnerabilitySelf6,
            SpellId.InvulnerabilitySelf7,
            SpellId.InvulnerabilitySelf8,
        };

        public static readonly List<SpellId> FireProtectionOther = new List<SpellId>()
        {
            SpellId.FireProtectionOther1,
            SpellId.FireProtectionOther2,
            SpellId.FireProtectionOther3,
            SpellId.FireProtectionOther4,
            SpellId.FireProtectionOther5,
            SpellId.FireProtectionOther6,
            SpellId.FireProtectionOther7,
            SpellId.FireProtectionOther8,
        };

        public static readonly List<SpellId> FireProtectionSelf = new List<SpellId>()
        {
            SpellId.FireProtectionSelf1,
            SpellId.FireProtectionSelf2,
            SpellId.FireProtectionSelf3,
            SpellId.FireProtectionSelf4,
            SpellId.FireProtectionSelf5,
            SpellId.FireProtectionSelf6,
            SpellId.FireProtectionSelf7,
            SpellId.FireProtectionSelf8,
        };

        public static readonly List<SpellId> FireVulnerabilityOther = new List<SpellId>()
        {
            SpellId.FireVulnerabilityOther1,
            SpellId.FireVulnerabilityOther2,
            SpellId.FireVulnerabilityOther3,
            SpellId.FireVulnerabilityOther4,
            SpellId.FireVulnerabilityOther5,
            SpellId.FireVulnerabilityOther6,
            SpellId.FireVulnerabilityOther7,
            SpellId.FireVulnerabilityOther8,
        };

        public static readonly List<SpellId> FireVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.FireVulnerabilitySelf1,
            SpellId.FireVulnerabilitySelf2,
            SpellId.FireVulnerabilitySelf3,
            SpellId.FireVulnerabilitySelf4,
            SpellId.FireVulnerabilitySelf5,
            SpellId.FireVulnerabilitySelf6,
            SpellId.FireVulnerabilitySelf7,
            SpellId.FireVulnerabilitySelf8,
        };

        public static readonly List<SpellId> ArmorOther = new List<SpellId>()
        {
            SpellId.ArmorOther1,
            SpellId.ArmorOther2,
            SpellId.ArmorOther3,
            SpellId.ArmorOther4,
            SpellId.ArmorOther5,
            SpellId.ArmorOther6,
            SpellId.ArmorOther7,
            SpellId.ArmorOther8,
        };

        public static readonly List<SpellId> ArmorSelf = new List<SpellId>()
        {
            SpellId.ArmorSelf1,
            SpellId.ArmorSelf2,
            SpellId.ArmorSelf3,
            SpellId.ArmorSelf4,
            SpellId.ArmorSelf5,
            SpellId.ArmorSelf6,
            SpellId.ArmorSelf7,
            SpellId.ArmorSelf8,
        };

        public static readonly List<SpellId> ImperilOther = new List<SpellId>()
        {
            SpellId.ImperilOther1,
            SpellId.ImperilOther2,
            SpellId.ImperilOther3,
            SpellId.ImperilOther4,
            SpellId.ImperilOther5,
            SpellId.ImperilOther6,
            SpellId.ImperilOther7,
            SpellId.ImperilOther8,
        };

        public static readonly List<SpellId> ImperilSelf = new List<SpellId>()
        {
            SpellId.ImperilSelf1,
            SpellId.ImperilSelf2,
            SpellId.ImperilSelf3,
            SpellId.ImperilSelf4,
            SpellId.ImperilSelf5,
            SpellId.ImperilSelf6,
            SpellId.ImperilSelf7,
            SpellId.ImperilSelf8,
        };

        public static readonly List<SpellId> FlameBolt = new List<SpellId>()
        {
            SpellId.FlameBolt1,
            SpellId.FlameBolt2,
            SpellId.FlameBolt3,
            SpellId.FlameBolt4,
            SpellId.FlameBolt5,
            SpellId.FlameBolt6,
            SpellId.FlameBolt7,
            SpellId.FlameBolt8,
        };

        public static readonly List<SpellId> FrostBolt = new List<SpellId>()
        {
            SpellId.FrostBolt1,
            SpellId.FrostBolt2,
            SpellId.FrostBolt3,
            SpellId.FrostBolt4,
            SpellId.FrostBolt5,
            SpellId.FrostBolt6,
            SpellId.FrostBolt7,
            SpellId.FrostBolt8,
        };

        public static readonly List<SpellId> BloodDrinkerSelf = new List<SpellId>()
        {
            SpellId.BloodDrinkerSelf1,
            SpellId.BloodDrinkerSelf2,
            SpellId.BloodDrinkerSelf3,
            SpellId.BloodDrinkerSelf4,
            SpellId.BloodDrinkerSelf5,
            SpellId.BloodDrinkerSelf6,
            SpellId.BloodDrinkerSelf7,
            SpellId.BloodDrinkerSelf8,
        };

        public static readonly List<SpellId> BloodLoather = new List<SpellId>()
        {
            SpellId.BloodLoather,
            SpellId.BloodLoather2,
            SpellId.BloodLoather3,
            SpellId.BloodLoather4,
            SpellId.BloodLoather5,
            SpellId.BloodLoather6,
            SpellId.BloodLoather7,
            SpellId.BloodLoather8,
        };

        public static readonly List<SpellId> BladeBane = new List<SpellId>()
        {
            SpellId.BladeBane1,
            SpellId.BladeBane2,
            SpellId.BladeBane3,
            SpellId.BladeBane4,
            SpellId.BladeBane5,
            SpellId.BladeBane6,
            SpellId.BladeBane7,
            SpellId.BladeBane8,
        };

        public static readonly List<SpellId> BladeLure = new List<SpellId>()
        {
            SpellId.BladeLure1,
            SpellId.BladeLure2,
            SpellId.BladeLure3,
            SpellId.BladeLure4,
            SpellId.BladeLure5,
            SpellId.BladeLure6,
            SpellId.BladeLure7,
            SpellId.BladeLure8,
        };

        public static readonly List<SpellId> PortalTie = new List<SpellId>()
        {
            SpellId.PortalTie1,
            SpellId.PortalTie2,
        };

        public static readonly List<SpellId> PortalTieRecall = new List<SpellId>()
        {
            SpellId.PortalTieRecall1,
            SpellId.PortalTieRecall2,
        };

        public static readonly List<SpellId> SwiftKillerSelf = new List<SpellId>()
        {
            SpellId.SwiftKillerSelf1,
            SpellId.SwiftKillerSelf2,
            SpellId.SwiftKillerSelf3,
            SpellId.SwiftKillerSelf4,
            SpellId.SwiftKillerSelf5,
            SpellId.SwiftKillerSelf6,
            SpellId.SwiftKillerSelf7,
            SpellId.SwiftKillerSelf8,
        };

        public static readonly List<SpellId> LeadenWeapon = new List<SpellId>()
        {
            SpellId.LeadenWeapon1,
            SpellId.LeadenWeapon2,
            SpellId.LeadenWeapon3,
            SpellId.LeadenWeapon4,
            SpellId.LeadenWeapon5,
            SpellId.LeadenWeapon6,
            SpellId.LeadenWeapon7,
            SpellId.LeadenWeapon8,
        };

        public static readonly List<SpellId> Impenetrability = new List<SpellId>()
        {
            SpellId.Impenetrability1,
            SpellId.Impenetrability2,
            SpellId.Impenetrability3,
            SpellId.Impenetrability4,
            SpellId.Impenetrability5,
            SpellId.Impenetrability6,
            SpellId.Impenetrability7,
            SpellId.Impenetrability8,
        };

        public static readonly List<SpellId> RejuvenationOther = new List<SpellId>()
        {
            SpellId.RejuvenationOther1,
            SpellId.RejuvenationOther2,
            SpellId.RejuvenationOther3,
            SpellId.RejuvenationOther4,
            SpellId.RejuvenationOther5,
            SpellId.RejuvenationOther6,
            SpellId.RejuvenationOther7,
            SpellId.RejuvenationOther8,
        };

        public static readonly List<SpellId> RejuvenationSelf = new List<SpellId>()
        {
            SpellId.RejuvenationSelf1,
            SpellId.RejuvenationSelf2,
            SpellId.RejuvenationSelf3,
            SpellId.RejuvenationSelf4,
            SpellId.RejuvenationSelf5,
            SpellId.RejuvenationSelf6,
            SpellId.RejuvenationSelf7,
            SpellId.RejuvenationSelf8,
        };

        public static readonly List<SpellId> AcidStream = new List<SpellId>()
        {
            SpellId.AcidStream1,
            SpellId.AcidStream2,
            SpellId.AcidStream3,
            SpellId.AcidStream4,
            SpellId.AcidStream5,
            SpellId.AcidStream6,
            SpellId.AcidStream7,
            SpellId.AcidStream8,
        };

        public static readonly List<SpellId> ShockWave = new List<SpellId>()
        {
            SpellId.ShockWave1,
            SpellId.ShockWave2,
            SpellId.ShockWave3,
            SpellId.ShockWave4,
            SpellId.ShockWave5,
            SpellId.ShockWave6,
            SpellId.ShockWave7,
            SpellId.ShockWave8,
        };

        public static readonly List<SpellId> LightningBolt = new List<SpellId>()
        {
            SpellId.LightningBolt1,
            SpellId.LightningBolt2,
            SpellId.LightningBolt3,
            SpellId.LightningBolt4,
            SpellId.LightningBolt5,
            SpellId.LightningBolt6,
            SpellId.LightningBolt7,
            SpellId.LightningBolt8,
        };

        public static readonly List<SpellId> ForceBolt = new List<SpellId>()
        {
            SpellId.ForceBolt1,
            SpellId.ForceBolt2,
            SpellId.ForceBolt3,
            SpellId.ForceBolt4,
            SpellId.ForceBolt5,
            SpellId.ForceBolt6,
            SpellId.ForceBolt7,
            SpellId.ForceBolt8,
        };

        public static readonly List<SpellId> WhirlingBlade = new List<SpellId>()
        {
            SpellId.WhirlingBlade1,
            SpellId.WhirlingBlade2,
            SpellId.WhirlingBlade3,
            SpellId.WhirlingBlade4,
            SpellId.WhirlingBlade5,
            SpellId.WhirlingBlade6,
            SpellId.WhirlingBlade7,
            SpellId.WhirlingBlade8,
        };

        public static readonly List<SpellId> AcidBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.AcidBlast2,
            SpellId.AcidBlast3,
            SpellId.AcidBlast4,
            SpellId.AcidBlast5,
            SpellId.AcidBlast6,
            SpellId.AcidBlast7,
            SpellId.AcidBlast8,
        };

        public static readonly List<SpellId> ShockBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.ShockBlast1,
            //SpellId.ShockBlast2,
            SpellId.ShockBlast3,
            SpellId.ShockBlast4,
            SpellId.ShockBlast5,
            SpellId.ShockBlast6,
            SpellId.ShockBlast7,
            SpellId.ShockBlast8,
        };

        public static readonly List<SpellId> FrostBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.FrostBlast1,
            //SpellId.FrostBlast2,
            SpellId.FrostBlast3,
            SpellId.FrostBlast4,
            SpellId.FrostBlast5,
            SpellId.FrostBlast6,
            SpellId.FrostBlast7,
            SpellId.FrostBlast8,
        };

        public static readonly List<SpellId> LightningBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.LightningBlast1,
            //SpellId.LightningBlast2,
            SpellId.LightningBlast3,
            SpellId.LightningBlast4,
            SpellId.LightningBlast5,
            SpellId.LightningBlast6,
            SpellId.LightningBlast7,
            SpellId.LightningBlast8,
        };

        public static readonly List<SpellId> FlameBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.FlameBlast2,
            SpellId.FlameBlast3,
            SpellId.FlameBlast4,
            SpellId.FlameBlast5,
            SpellId.FlameBlast6,
            SpellId.FlameBlast7,
            SpellId.FlameBlast8,
        };

        public static readonly List<SpellId> ForceBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.ForceBlast1,
            //SpellId.ForceBlast2,
            SpellId.ForceBlast3,
            SpellId.ForceBlast4,
            SpellId.ForceBlast5,
            SpellId.ForceBlast6,
            SpellId.ForceBlast7,
            SpellId.ForceBlast8,
        };

        public static readonly List<SpellId> BladeBlast = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.BladeBlast1,
            //SpellId.BladeBlast2,
            SpellId.BladeBlast3,
            SpellId.BladeBlast4,
            SpellId.BladeBlast5,
            SpellId.BladeBlast6,
            SpellId.BladeBlast7,
            SpellId.BladeBlast8,
        };

        public static readonly List<SpellId> AcidVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.AcidVolley1,
            //SpellId.AcidVolley2,
            SpellId.AcidVolley3,
            SpellId.AcidVolley4,
            SpellId.AcidVolley5,
            SpellId.AcidVolley6,
            SpellId.AcidVolley7,
            SpellId.AcidVolley8,
        };

        public static readonly List<SpellId> BludgeoningVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.BludgeoningVolley1,
            //SpellId.BludgeoningVolley2,
            SpellId.BludgeoningVolley3,
            SpellId.BludgeoningVolley4,
            SpellId.BludgeoningVolley5,
            SpellId.BludgeoningVolley6,
            SpellId.BludgeoningVolley7,
            SpellId.BludgeoningVolley8,
        };

        public static readonly List<SpellId> FrostVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.FrostVolley1,
            //SpellId.FrostVolley2,
            SpellId.FrostVolley3,
            SpellId.FrostVolley4,
            SpellId.FrostVolley5,
            SpellId.FrostVolley6,
            SpellId.FrostVolley7,
            SpellId.FrostVolley8,
        };

        public static readonly List<SpellId> LightningVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.LightningVolley1,
            //SpellId.LightningVolley2,
            SpellId.LightningVolley3,
            SpellId.LightningVolley4,
            SpellId.LightningVolley5,
            SpellId.LightningVolley6,
            SpellId.LightningVolley7,
            SpellId.LightningVolley8,
        };

        public static readonly List<SpellId> FlameVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.FlameVolley1,
            //SpellId.FlameVolley2,
            SpellId.FlameVolley3,
            SpellId.FlameVolley4,
            SpellId.FlameVolley5,
            SpellId.FlameVolley6,
            SpellId.FlameVolley7,
            SpellId.FlameVolley8,
        };

        public static readonly List<SpellId> ForceVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.ForceVolley1,
            //SpellId.ForceVolley2,
            SpellId.ForceVolley3,
            SpellId.ForceVolley4,
            SpellId.ForceVolley5,
            SpellId.ForceVolley6,
            SpellId.ForceVolley7,
            SpellId.ForceVolley8,
        };

        public static readonly List<SpellId> BladeVolley = new List<SpellId>()
        {
            SpellId.Undef,
            SpellId.Undef,
            //SpellId.BladeVolley1,
            //SpellId.BladeVolley2,
            SpellId.BladeVolley3,
            SpellId.BladeVolley4,
            SpellId.BladeVolley5,
            SpellId.BladeVolley6,
            SpellId.BladeVolley7,
            SpellId.BladeVolley8,
        };

        public static readonly List<SpellId> SummonPortal = new List<SpellId>()
        {
            SpellId.SummonPortal1,
            SpellId.SummonPortal2,
            SpellId.SummonPortal3,
        };

        public static readonly List<SpellId> RegenerationOther = new List<SpellId>()
        {
            SpellId.RegenerationOther1,
            SpellId.RegenerationOther2,
            SpellId.RegenerationOther3,
            SpellId.RegenerationOther4,
            SpellId.RegenerationOther5,
            SpellId.RegenerationOther6,
            SpellId.RegenerationOther7,
            SpellId.RegenerationOther8,
        };

        public static readonly List<SpellId> RegenerationSelf = new List<SpellId>()
        {
            SpellId.RegenerationSelf1,
            SpellId.RegenerationSelf2,
            SpellId.RegenerationSelf3,
            SpellId.RegenerationSelf4,
            SpellId.RegenerationSelf5,
            SpellId.RegenerationSelf6,
            SpellId.RegenerationSelf7,
            SpellId.RegenerationSelf8,
        };

        public static readonly List<SpellId> FesterOther = new List<SpellId>()
        {
            SpellId.FesterOther1,
            SpellId.FesterOther2,
            SpellId.FesterOther3,
            SpellId.FesterOther4,
            SpellId.FesterOther5,
            SpellId.FesterOther6,
            SpellId.FesterOther7,
            SpellId.FesterOther8,
        };

        public static readonly List<SpellId> FesterSelf = new List<SpellId>()
        {
            SpellId.FesterSelf1,
            SpellId.FesterSelf2,
            SpellId.FesterSelf3,
            SpellId.FesterSelf4,
            SpellId.FesterSelf5,
            SpellId.FesterSelf6,
            SpellId.FesterSelf7,
            SpellId.FesterSelf8,
        };

        public static readonly List<SpellId> ExhaustionOther = new List<SpellId>()
        {
            SpellId.ExhaustionOther1,
            SpellId.ExhaustionOther2,
            SpellId.ExhaustionOther3,
            SpellId.ExhaustionOther4,
            SpellId.ExhaustionOther5,
            SpellId.ExhaustionOther6,
            SpellId.ExhaustionOther7,
            SpellId.ExhaustionOther8,
        };

        public static readonly List<SpellId> ExhaustionSelf = new List<SpellId>()
        {
            SpellId.ExhaustionSelf1,
            SpellId.ExhaustionSelf2,
            SpellId.ExhaustionSelf3,
            SpellId.ExhaustionSelf4,
            SpellId.ExhaustionSelf5,
            SpellId.ExhaustionSelf6,
            SpellId.ExhaustionSelf7,
            SpellId.ExhaustionSelf8,
        };

        public static readonly List<SpellId> ManaRenewalOther = new List<SpellId>()
        {
            SpellId.ManaRenewalOther1,
            SpellId.ManaRenewalOther2,
            SpellId.ManaRenewalOther3,
            SpellId.ManaRenewalOther4,
            SpellId.ManaRenewalOther5,
            SpellId.ManaRenewalOther6,
            SpellId.ManaRenewalOther7,
            SpellId.ManaRenewalOther8,
        };

        public static readonly List<SpellId> ManaRenewalSelf = new List<SpellId>()
        {
            SpellId.ManaRenewalSelf1,
            SpellId.ManaRenewalSelf2,
            SpellId.ManaRenewalSelf3,
            SpellId.ManaRenewalSelf4,
            SpellId.ManaRenewalSelf5,
            SpellId.ManaRenewalSelf6,
            SpellId.ManaRenewalSelf7,
            SpellId.ManaRenewalSelf8,
        };

        public static readonly List<SpellId> ManaDepletionOther = new List<SpellId>()
        {
            SpellId.ManaDepletionOther1,
            SpellId.ManaDepletionOther2,
            SpellId.ManaDepletionOther3,
            SpellId.ManaDepletionOther4,
            SpellId.ManaDepletionOther5,
            SpellId.ManaDepletionOther6,
            SpellId.ManaDepletionOther7,
            SpellId.ManaDepletionOther8,
        };

        public static readonly List<SpellId> ManaDepletionSelf = new List<SpellId>()
        {
            SpellId.ManaDepletionSelf1,
            SpellId.ManaDepletionSelf2,
            SpellId.ManaDepletionSelf3,
            SpellId.ManaDepletionSelf4,
            SpellId.ManaDepletionSelf5,
            SpellId.ManaDepletionSelf6,
            SpellId.ManaDepletionSelf7,
            SpellId.ManaDepletionSelf8,
        };

        public static readonly List<SpellId> ImpregnabilityOther = new List<SpellId>()
        {
            SpellId.ImpregnabilityOther1,
            SpellId.ImpregnabilityOther2,
            SpellId.ImpregnabilityOther3,
            SpellId.ImpregnabilityOther4,
            SpellId.ImpregnabilityOther5,
            SpellId.ImpregnabilityOther6,
            SpellId.ImpregnabilityOther7,
            SpellId.ImpregnabilityOther8,
        };

        public static readonly List<SpellId> ImpregnabilitySelf = new List<SpellId>()
        {
            SpellId.ImpregnabilitySelf1,
            SpellId.ImpregnabilitySelf2,
            SpellId.ImpregnabilitySelf3,
            SpellId.ImpregnabilitySelf4,
            SpellId.ImpregnabilitySelf5,
            SpellId.ImpregnabilitySelf6,
            SpellId.ImpregnabilitySelf7,
            SpellId.ImpregnabilitySelf8,
        };

        public static readonly List<SpellId> DefenselessnessOther = new List<SpellId>()
        {
            SpellId.DefenselessnessOther1,
            SpellId.DefenselessnessOther2,
            SpellId.DefenselessnessOther3,
            SpellId.DefenselessnessOther4,
            SpellId.DefenselessnessOther5,
            SpellId.DefenselessnessOther6,
            SpellId.DefenselessnessOther7,
            SpellId.DefenselessnessOther8,
        };

        public static readonly List<SpellId> MagicResistanceOther = new List<SpellId>()
        {
            SpellId.MagicResistanceOther1,
            SpellId.MagicResistanceOther2,
            SpellId.MagicResistanceOther3,
            SpellId.MagicResistanceOther4,
            SpellId.MagicResistanceOther5,
            SpellId.MagicResistanceOther6,
            SpellId.MagicResistanceOther7,
            SpellId.MagicResistanceOther8,
        };

        public static readonly List<SpellId> MagicResistanceSelf = new List<SpellId>()
        {
            SpellId.MagicResistanceSelf1,
            SpellId.MagicResistanceSelf2,
            SpellId.MagicResistanceSelf3,
            SpellId.MagicResistanceSelf4,
            SpellId.MagicResistanceSelf5,
            SpellId.MagicResistanceSelf6,
            SpellId.MagicResistanceSelf7,
            SpellId.MagicResistanceSelf8,
        };

        public static readonly List<SpellId> MagicYieldOther = new List<SpellId>()
        {
            SpellId.MagicYieldOther1,
            SpellId.MagicYieldOther2,
            SpellId.MagicYieldOther3,
            SpellId.MagicYieldOther4,
            SpellId.MagicYieldOther5,
            SpellId.MagicYieldOther6,
            SpellId.MagicYieldOther7,
            SpellId.MagicYieldOther8,
        };

        public static readonly List<SpellId> MagicYieldSelf = new List<SpellId>()
        {
            SpellId.MagicYieldSelf1,
            SpellId.MagicYieldSelf2,
            SpellId.MagicYieldSelf3,
            SpellId.MagicYieldSelf4,
            SpellId.MagicYieldSelf5,
            SpellId.MagicYieldSelf6,
            SpellId.MagicYieldSelf7,
            SpellId.MagicYieldSelf8,
        };

        public static readonly List<SpellId> LightWeaponsMasteryOther = new List<SpellId>()
        {
            SpellId.LightWeaponsMasteryOther1,
            SpellId.LightWeaponsMasteryOther2,
            SpellId.LightWeaponsMasteryOther3,
            SpellId.LightWeaponsMasteryOther4,
            SpellId.LightWeaponsMasteryOther5,
            SpellId.LightWeaponsMasteryOther6,
            SpellId.LightWeaponsMasteryOther7,
            SpellId.LightWeaponsMasteryOther8,
        };

        public static readonly List<SpellId> LightWeaponsMasterySelf = new List<SpellId>()
        {
            SpellId.LightWeaponsMasterySelf1,
            SpellId.LightWeaponsMasterySelf2,
            SpellId.LightWeaponsMasterySelf3,
            SpellId.LightWeaponsMasterySelf4,
            SpellId.LightWeaponsMasterySelf5,
            SpellId.LightWeaponsMasterySelf6,
            SpellId.LightWeaponsMasterySelf7,
            SpellId.LightWeaponsMasterySelf8,
        };

        public static readonly List<SpellId> LightWeaponsIneptitudeOther = new List<SpellId>()
        {
            SpellId.LightWeaponsIneptitudeOther1,
            SpellId.LightWeaponsIneptitudeOther2,
            SpellId.LightWeaponsIneptitudeOther3,
            SpellId.LightWeaponsIneptitudeOther4,
            SpellId.LightWeaponsIneptitudeOther5,
            SpellId.LightWeaponsIneptitudeOther6,
            SpellId.LightWeaponsIneptitudeOther7,
            SpellId.LightWeaponsIneptitudeOther8,
        };

        public static readonly List<SpellId> LightWeaponsIneptitudeSelf = new List<SpellId>()
        {
            SpellId.LightWeaponsIneptitudeSelf1,
            SpellId.LightWeaponsIneptitudeSelf2,
            SpellId.LightWeaponsIneptitudeSelf3,
            SpellId.LightWeaponsIneptitudeSelf4,
            SpellId.LightWeaponsIneptitudeSelf5,
            SpellId.LightWeaponsIneptitudeSelf6,
            SpellId.LightWeaponsIneptitudeSelf7,
            SpellId.LightWeaponsIneptitudeSelf8,
        };

        public static readonly List<SpellId> FinesseWeaponsMasteryOther = new List<SpellId>()
        {
            SpellId.FinesseWeaponsMasteryOther1,
            SpellId.FinesseWeaponsMasteryOther2,
            SpellId.FinesseWeaponsMasteryOther3,
            SpellId.FinesseWeaponsMasteryOther4,
            SpellId.FinesseWeaponsMasteryOther5,
            SpellId.FinesseWeaponsMasteryOther6,
            SpellId.FinesseWeaponsMasteryOther7,
            SpellId.FinesseWeaponsMasteryOther8,
        };

        public static readonly List<SpellId> FinesseWeaponsMasterySelf = new List<SpellId>()
        {
            SpellId.FinesseWeaponsMasterySelf1,
            SpellId.FinesseWeaponsMasterySelf2,
            SpellId.FinesseWeaponsMasterySelf3,
            SpellId.FinesseWeaponsMasterySelf4,
            SpellId.FinesseWeaponsMasterySelf5,
            SpellId.FinesseWeaponsMasterySelf6,
            SpellId.FinesseWeaponsMasterySelf7,
            SpellId.FinesseWeaponsMasterySelf8,
        };

        public static readonly List<SpellId> FinesseWeaponsIneptitudeOther = new List<SpellId>()
        {
            SpellId.FinesseWeaponsIneptitudeOther1,
            SpellId.FinesseWeaponsIneptitudeOther2,
            SpellId.FinesseWeaponsIneptitudeOther3,
            SpellId.FinesseWeaponsIneptitudeOther4,
            SpellId.FinesseWeaponsIneptitudeOther5,
            SpellId.FinesseWeaponsIneptitudeOther6,
            SpellId.FinesseWeaponsIneptitudeOther7,
            SpellId.FinesseWeaponsIneptitudeOther8,
        };

        public static readonly List<SpellId> FinesseWeaponsIneptitudeSelf = new List<SpellId>()
        {
            SpellId.FinesseWeaponsIneptitudeSelf1,
            SpellId.FinesseWeaponsIneptitudeSelf2,
            SpellId.FinesseWeaponsIneptitudeSelf3,
            SpellId.FinesseWeaponsIneptitudeSelf4,
            SpellId.FinesseWeaponsIneptitudeSelf5,
            SpellId.FinesseWeaponsIneptitudeSelf6,
            SpellId.FinesseWeaponsIneptitudeSelf7,
            SpellId.FinesseWeaponsIneptitudeSelf8,
        };

        public static readonly List<SpellId> MaceMasteryOther = new List<SpellId>()
        {
            SpellId.MaceMasteryOther1,
            SpellId.MaceMasteryOther2,
            SpellId.MaceMasteryOther3,
            SpellId.MaceMasteryOther4,
            SpellId.MaceMasteryOther5,
            SpellId.MaceMasteryOther6,
            SpellId.MaceMasteryOther7,
            SpellId.MaceMasteryOther8,
        };

        public static readonly List<SpellId> MaceMasterySelf = new List<SpellId>()
        {
            SpellId.MaceMasterySelf1,
            SpellId.MaceMasterySelf2,
            SpellId.MaceMasterySelf3,
            SpellId.MaceMasterySelf4,
            SpellId.MaceMasterySelf5,
            SpellId.MaceMasterySelf6,
            SpellId.MaceMasterySelf7,
            SpellId.MaceMasterySelf8,
        };

        public static readonly List<SpellId> MaceIneptitudeOther = new List<SpellId>()
        {
            SpellId.MaceIneptitudeOther1,
            SpellId.MaceIneptitudeOther2,
            SpellId.MaceIneptitudeOther3,
            SpellId.MaceIneptitudeOther4,
            SpellId.MaceIneptitudeOther5,
            SpellId.MaceIneptitudeOther6,
            SpellId.MaceIneptitudeOther7,
            SpellId.MaceIneptitudeOther8,
        };

        public static readonly List<SpellId> MaceIneptitudeSelf = new List<SpellId>()
        {
            SpellId.MaceIneptitudeSelf1,
            SpellId.MaceIneptitudeSelf2,
            SpellId.MaceIneptitudeSelf3,
            SpellId.MaceIneptitudeSelf4,
            SpellId.MaceIneptitudeSelf5,
            SpellId.MaceIneptitudeSelf6,
            SpellId.MaceIneptitudeSelf7,
            SpellId.MaceIneptitudeSelf8,
        };

        public static readonly List<SpellId> SpearMasteryOther = new List<SpellId>()
        {
            SpellId.SpearMasteryOther1,
            SpellId.SpearMasteryOther2,
            SpellId.SpearMasteryOther3,
            SpellId.SpearMasteryOther4,
            SpellId.SpearMasteryOther5,
            SpellId.SpearMasteryOther6,
            SpellId.SpearMasteryOther7,
            SpellId.SpearMasteryOther8,
        };

        public static readonly List<SpellId> SpearMasterySelf = new List<SpellId>()
        {
            SpellId.SpearMasterySelf1,
            SpellId.SpearMasterySelf2,
            SpellId.SpearMasterySelf3,
            SpellId.SpearMasterySelf4,
            SpellId.SpearMasterySelf5,
            SpellId.SpearMasterySelf6,
            SpellId.SpearMasterySelf7,
            SpellId.SpearMasterySelf8,
        };

        public static readonly List<SpellId> SpearIneptitudeOther = new List<SpellId>()
        {
            SpellId.SpearIneptitudeOther1,
            SpellId.SpearIneptitudeOther2,
            SpellId.SpearIneptitudeOther3,
            SpellId.SpearIneptitudeOther4,
            SpellId.SpearIneptitudeOther5,
            SpellId.SpearIneptitudeOther6,
            SpellId.SpearIneptitudeOther7,
            SpellId.SpearIneptitudeOther8,
        };

        public static readonly List<SpellId> SpearIneptitudeSelf = new List<SpellId>()
        {
            SpellId.SpearIneptitudeSelf1,
            SpellId.SpearIneptitudeSelf2,
            SpellId.SpearIneptitudeSelf3,
            SpellId.SpearIneptitudeSelf4,
            SpellId.SpearIneptitudeSelf5,
            SpellId.SpearIneptitudeSelf6,
            SpellId.SpearIneptitudeSelf7,
            SpellId.SpearIneptitudeSelf8,
        };

        public static readonly List<SpellId> StaffMasteryOther = new List<SpellId>()
        {
            SpellId.StaffMasteryOther1,
            SpellId.StaffMasteryOther2,
            SpellId.StaffMasteryOther3,
            SpellId.StaffMasteryOther4,
            SpellId.StaffMasteryOther5,
            SpellId.StaffMasteryOther6,
            SpellId.StaffMasteryOther7,
            SpellId.StaffMasteryOther8,
        };

        public static readonly List<SpellId> StaffMasterySelf = new List<SpellId>()
        {
            SpellId.StaffMasterySelf1,
            SpellId.StaffMasterySelf2,
            SpellId.StaffMasterySelf3,
            SpellId.StaffMasterySelf4,
            SpellId.StaffMasterySelf5,
            SpellId.StaffMasterySelf6,
            SpellId.StaffMasterySelf7,
            SpellId.StaffMasterySelf8,
        };

        public static readonly List<SpellId> StaffIneptitudeOther = new List<SpellId>()
        {
            SpellId.StaffIneptitudeOther1,
            SpellId.StaffIneptitudeOther2,
            SpellId.StaffIneptitudeOther3,
            SpellId.StaffIneptitudeOther4,
            SpellId.StaffIneptitudeOther5,
            SpellId.StaffIneptitudeOther6,
            SpellId.StaffIneptitudeOther7,
            SpellId.StaffIneptitudeOther8,
        };

        public static readonly List<SpellId> StaffIneptitudeSelf = new List<SpellId>()
        {
            SpellId.StaffIneptitudeSelf1,
            SpellId.StaffIneptitudeSelf2,
            SpellId.StaffIneptitudeSelf3,
            SpellId.StaffIneptitudeSelf4,
            SpellId.StaffIneptitudeSelf5,
            SpellId.StaffIneptitudeSelf6,
            SpellId.StaffIneptitudeSelf7,
            SpellId.StaffIneptitudeSelf8,
        };

        public static readonly List<SpellId> HeavyWeaponsMasteryOther = new List<SpellId>()
        {
            SpellId.HeavyWeaponsMasteryOther1,
            SpellId.HeavyWeaponsMasteryOther2,
            SpellId.HeavyWeaponsMasteryOther3,
            SpellId.HeavyWeaponsMasteryOther4,
            SpellId.HeavyWeaponsMasteryOther5,
            SpellId.HeavyWeaponsMasteryOther6,
            SpellId.HeavyWeaponsMasteryOther7,
            SpellId.HeavyWeaponsMasteryOther8,
        };

        public static readonly List<SpellId> HeavyWeaponsMasterySelf = new List<SpellId>()
        {
            SpellId.HeavyWeaponsMasterySelf1,
            SpellId.HeavyWeaponsMasterySelf2,
            SpellId.HeavyWeaponsMasterySelf3,
            SpellId.HeavyWeaponsMasterySelf4,
            SpellId.HeavyWeaponsMasterySelf5,
            SpellId.HeavyWeaponsMasterySelf6,
            SpellId.HeavyWeaponsMasterySelf7,
            SpellId.HeavyWeaponsMasterySelf8,
        };

        public static readonly List<SpellId> HeavyWeaponsIneptitudeOther = new List<SpellId>()
        {
            SpellId.HeavyWeaponsIneptitudeOther1,
            SpellId.HeavyWeaponsIneptitudeOther2,
            SpellId.HeavyWeaponsIneptitudeOther3,
            SpellId.HeavyWeaponsIneptitudeOther4,
            SpellId.HeavyWeaponsIneptitudeOther5,
            SpellId.HeavyWeaponsIneptitudeOther6,
            SpellId.HeavyWeaponsIneptitudeOther7,
            SpellId.HeavyWeaponsIneptitudeOther8,
        };

        public static readonly List<SpellId> HeavyWeaponsIneptitudeSelf = new List<SpellId>()
        {
            SpellId.HeavyWeaponsIneptitudeSelf1,
            SpellId.HeavyWeaponsIneptitudeSelf2,
            SpellId.HeavyWeaponsIneptitudeSelf3,
            SpellId.HeavyWeaponsIneptitudeSelf4,
            SpellId.HeavyWeaponsIneptitudeSelf5,
            SpellId.HeavyWeaponsIneptitudeSelf6,
            SpellId.HeavyWeaponsIneptitudeSelf7,
            SpellId.HeavyWeaponsIneptitudeSelf8,
        };

        public static readonly List<SpellId> UnarmedCombatMasteryOther = new List<SpellId>()
        {
            SpellId.UnarmedCombatMasteryOther1,
            SpellId.UnarmedCombatMasteryOther2,
            SpellId.UnarmedCombatMasteryOther3,
            SpellId.UnarmedCombatMasteryOther4,
            SpellId.UnarmedCombatMasteryOther5,
            SpellId.UnarmedCombatMasteryOther6,
            SpellId.UnarmedCombatMasteryOther7,
            SpellId.UnarmedCombatMasteryOther8,
        };

        public static readonly List<SpellId> UnarmedCombatMasterySelf = new List<SpellId>()
        {
            SpellId.UnarmedCombatMasterySelf1,
            SpellId.UnarmedCombatMasterySelf2,
            SpellId.UnarmedCombatMasterySelf3,
            SpellId.UnarmedCombatMasterySelf4,
            SpellId.UnarmedCombatMasterySelf5,
            SpellId.UnarmedCombatMasterySelf6,
            SpellId.UnarmedCombatMasterySelf7,
            SpellId.UnarmedCombatMasterySelf8,
        };

        public static readonly List<SpellId> UnarmedCombatIneptitudeOther = new List<SpellId>()
        {
            SpellId.UnarmedCombatIneptitudeOther1,
            SpellId.UnarmedCombatIneptitudeOther2,
            SpellId.UnarmedCombatIneptitudeOther3,
            SpellId.UnarmedCombatIneptitudeOther4,
            SpellId.UnarmedCombatIneptitudeOther5,
            SpellId.UnarmedCombatIneptitudeOther6,
            SpellId.UnarmedCombatIneptitudeOther7,
            SpellId.UnarmedCombatIneptitudeOther8,
        };

        public static readonly List<SpellId> UnarmedCombatIneptitudeSelf = new List<SpellId>()
        {
            SpellId.UnarmedCombatIneptitudeSelf1,
            SpellId.UnarmedCombatIneptitudeSelf2,
            SpellId.UnarmedCombatIneptitudeSelf3,
            SpellId.UnarmedCombatIneptitudeSelf4,
            SpellId.UnarmedCombatIneptitudeSelf5,
            SpellId.UnarmedCombatIneptitudeSelf6,
            SpellId.UnarmedCombatIneptitudeSelf7,
            SpellId.UnarmedCombatIneptitudeSelf8,
        };

        public static readonly List<SpellId> MissileWeaponsMasteryOther = new List<SpellId>()
        {
            SpellId.MissileWeaponsMasteryOther1,
            SpellId.MissileWeaponsMasteryOther2,
            SpellId.MissileWeaponsMasteryOther3,
            SpellId.MissileWeaponsMasteryOther4,
            SpellId.MissileWeaponsMasteryOther5,
            SpellId.MissileWeaponsMasteryOther6,
            SpellId.MissileWeaponsMasteryOther7,
            SpellId.MissileWeaponsMasteryOther8,
        };

        public static readonly List<SpellId> MissileWeaponsMasterySelf = new List<SpellId>()
        {
            SpellId.MissileWeaponsMasterySelf1,
            SpellId.MissileWeaponsMasterySelf2,
            SpellId.MissileWeaponsMasterySelf3,
            SpellId.MissileWeaponsMasterySelf4,
            SpellId.MissileWeaponsMasterySelf5,
            SpellId.MissileWeaponsMasterySelf6,
            SpellId.MissileWeaponsMasterySelf7,
            SpellId.MissileWeaponsMasterySelf8,
        };

        public static readonly List<SpellId> MissileWeaponsIneptitudeOther = new List<SpellId>()
        {
            SpellId.MissileWeaponsIneptitudeOther1,
            SpellId.MissileWeaponsIneptitudeOther2,
            SpellId.MissileWeaponsIneptitudeOther3,
            SpellId.MissileWeaponsIneptitudeOther4,
            SpellId.MissileWeaponsIneptitudeOther5,
            SpellId.MissileWeaponsIneptitudeOther6,
            SpellId.MissileWeaponsIneptitudeOther7,
            SpellId.MissileWeaponsIneptitudeOther8,
        };

        public static readonly List<SpellId> MissileWeaponsIneptitudeSelf = new List<SpellId>()
        {
            SpellId.MissileWeaponsIneptitudeSelf1,
            SpellId.MissileWeaponsIneptitudeSelf2,
            SpellId.MissileWeaponsIneptitudeSelf3,
            SpellId.MissileWeaponsIneptitudeSelf4,
            SpellId.MissileWeaponsIneptitudeSelf5,
            SpellId.MissileWeaponsIneptitudeSelf6,
            SpellId.MissileWeaponsIneptitudeSelf7,
            SpellId.MissileWeaponsIneptitudeSelf8,
        };

        public static readonly List<SpellId> CrossbowMasteryOther = new List<SpellId>()
        {
            SpellId.CrossbowMasteryOther1,
            SpellId.CrossbowMasteryOther2,
            SpellId.CrossbowMasteryOther3,
            SpellId.CrossbowMasteryOther4,
            SpellId.CrossbowMasteryOther5,
            SpellId.CrossbowMasteryOther6,
            SpellId.CrossbowMasteryOther7,
            SpellId.CrossbowMasteryOther8,
        };

        public static readonly List<SpellId> CrossbowMasterySelf = new List<SpellId>()
        {
            SpellId.CrossbowMasterySelf1,
            SpellId.CrossbowMasterySelf2,
            SpellId.CrossbowMasterySelf3,
            SpellId.CrossbowMasterySelf4,
            SpellId.CrossbowMasterySelf5,
            SpellId.CrossbowMasterySelf6,
            SpellId.CrossbowMasterySelf7,
            SpellId.CrossbowMasterySelf8,
        };

        public static readonly List<SpellId> CrossbowIneptitudeOther = new List<SpellId>()
        {
            SpellId.CrossbowIneptitudeOther1,
            SpellId.CrossbowIneptitudeOther2,
            SpellId.CrossbowIneptitudeOther3,
            SpellId.CrossbowIneptitudeOther4,
            SpellId.CrossbowIneptitudeOther5,
            SpellId.CrossbowIneptitudeOther6,
            SpellId.CrossbowIneptitudeOther7,
            SpellId.CrossbowIneptitudeOther8,
        };

        public static readonly List<SpellId> CrossbowIneptitudeSelf = new List<SpellId>()
        {
            SpellId.CrossbowIneptitudeSelf1,
            SpellId.CrossbowIneptitudeSelf2,
            SpellId.CrossbowIneptitudeSelf3,
            SpellId.CrossbowIneptitudeSelf4,
            SpellId.CrossbowIneptitudeSelf5,
            SpellId.CrossbowIneptitudeSelf6,
            SpellId.CrossbowIneptitudeSelf7,
            SpellId.CrossbowIneptitudeSelf8,
        };

        public static readonly List<SpellId> AcidProtectionOther = new List<SpellId>()
        {
            SpellId.AcidProtectionOther1,
            SpellId.AcidProtectionOther2,
            SpellId.AcidProtectionOther3,
            SpellId.AcidProtectionOther4,
            SpellId.AcidProtectionOther5,
            SpellId.AcidProtectionOther6,
            SpellId.AcidProtectionOther7,
            SpellId.AcidProtectionOther8,
        };

        public static readonly List<SpellId> AcidProtectionSelf = new List<SpellId>()
        {
            SpellId.AcidProtectionSelf1,
            SpellId.AcidProtectionSelf2,
            SpellId.AcidProtectionSelf3,
            SpellId.AcidProtectionSelf4,
            SpellId.AcidProtectionSelf5,
            SpellId.AcidProtectionSelf6,
            SpellId.AcidProtectionSelf7,
            SpellId.AcidProtectionSelf8,
        };

        public static readonly List<SpellId> AcidVulnerabilityOther = new List<SpellId>()
        {
            SpellId.AcidVulnerabilityOther1,
            SpellId.AcidVulnerabilityOther2,
            SpellId.AcidVulnerabilityOther3,
            SpellId.AcidVulnerabilityOther4,
            SpellId.AcidVulnerabilityOther5,
            SpellId.AcidVulnerabilityOther6,
            SpellId.AcidVulnerabilityOther7,
            SpellId.AcidVulnerabilityOther8,
        };

        public static readonly List<SpellId> AcidVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.AcidVulnerabilitySelf1,
            SpellId.AcidVulnerabilitySelf2,
            SpellId.AcidVulnerabilitySelf3,
            SpellId.AcidVulnerabilitySelf4,
            SpellId.AcidVulnerabilitySelf5,
            SpellId.AcidVulnerabilitySelf6,
            SpellId.AcidVulnerabilitySelf7,
            SpellId.AcidVulnerabilitySelf8,
        };

        public static readonly List<SpellId> ThrownWeaponMasteryOther = new List<SpellId>()
        {
            SpellId.ThrownWeaponMasteryOther1,
            SpellId.ThrownWeaponMasteryOther2,
            SpellId.ThrownWeaponMasteryOther3,
            SpellId.ThrownWeaponMasteryOther4,
            SpellId.ThrownWeaponMasteryOther5,
            SpellId.ThrownWeaponMasteryOther6,
            SpellId.ThrownWeaponMasteryOther7,
            SpellId.ThrownWeaponMasteryOther8,
        };

        public static readonly List<SpellId> ThrownWeaponMasterySelf = new List<SpellId>()
        {
            SpellId.ThrownWeaponMasterySelf1,
            SpellId.ThrownWeaponMasterySelf2,
            SpellId.ThrownWeaponMasterySelf3,
            SpellId.ThrownWeaponMasterySelf4,
            SpellId.ThrownWeaponMasterySelf5,
            SpellId.ThrownWeaponMasterySelf6,
            SpellId.ThrownWeaponMasterySelf7,
            SpellId.ThrownWeaponMasterySelf8,
        };

        public static readonly List<SpellId> ThrownWeaponIneptitudeOther = new List<SpellId>()
        {
            SpellId.ThrownWeaponIneptitudeOther1,
            SpellId.ThrownWeaponIneptitudeOther2,
            SpellId.ThrownWeaponIneptitudeOther3,
            SpellId.ThrownWeaponIneptitudeOther4,
            SpellId.ThrownWeaponIneptitudeOther5,
            SpellId.ThrownWeaponIneptitudeOther6,
            SpellId.ThrownWeaponIneptitudeOther7,
            SpellId.ThrownWeaponIneptitudeOther8,
        };

        public static readonly List<SpellId> ThrownWeaponIneptitudeSelf = new List<SpellId>()
        {
            SpellId.ThrownWeaponIneptitudeSelf1,
            SpellId.ThrownWeaponIneptitudeSelf2,
            SpellId.ThrownWeaponIneptitudeSelf3,
            SpellId.ThrownWeaponIneptitudeSelf4,
            SpellId.ThrownWeaponIneptitudeSelf5,
            SpellId.ThrownWeaponIneptitudeSelf6,
            SpellId.ThrownWeaponIneptitudeSelf7,
            SpellId.ThrownWeaponIneptitudeSelf8,
        };

        public static readonly List<SpellId> CreatureEnchantmentMasterySelf = new List<SpellId>()
        {
            SpellId.CreatureEnchantmentMasterySelf1,
            SpellId.CreatureEnchantmentMasterySelf2,
            SpellId.CreatureEnchantmentMasterySelf3,
            SpellId.CreatureEnchantmentMasterySelf4,
            SpellId.CreatureEnchantmentMasterySelf5,
            SpellId.CreatureEnchantmentMasterySelf6,
            SpellId.CreatureEnchantmentMasterySelf7,
            SpellId.CreatureEnchantmentMasterySelf8,
        };

        public static readonly List<SpellId> CreatureEnchantmentMasteryOther = new List<SpellId>()
        {
            SpellId.CreatureEnchantmentMasteryOther1,
            SpellId.CreatureEnchantmentMasteryOther2,
            SpellId.CreatureEnchantmentMasteryOther3,
            SpellId.CreatureEnchantmentMasteryOther4,
            SpellId.CreatureEnchantmentMasteryOther5,
            SpellId.CreatureEnchantmentMasteryOther6,
            SpellId.CreatureEnchantmentMasteryOther7,
            SpellId.CreatureEnchantmentMasteryOther8,
        };

        public static readonly List<SpellId> CreatureEnchantmentIneptitudeOther = new List<SpellId>()
        {
            SpellId.CreatureEnchantmentIneptitudeOther1,
            SpellId.CreatureEnchantmentIneptitudeOther2,
            SpellId.CreatureEnchantmentIneptitudeOther3,
            SpellId.CreatureEnchantmentIneptitudeOther4,
            SpellId.CreatureEnchantmentIneptitudeOther5,
            SpellId.CreatureEnchantmentIneptitudeOther6,
            SpellId.CreatureEnchantmentIneptitudeOther7,
            SpellId.CreatureEnchantmentIneptitudeOther8,
        };

        public static readonly List<SpellId> CreatureEnchantmentIneptitudeSelf = new List<SpellId>()
        {
            SpellId.CreatureEnchantmentIneptitudeSelf1,
            SpellId.CreatureEnchantmentIneptitudeSelf2,
            SpellId.CreatureEnchantmentIneptitudeSelf3,
            SpellId.CreatureEnchantmentIneptitudeSelf4,
            SpellId.CreatureEnchantmentIneptitudeSelf5,
            SpellId.CreatureEnchantmentIneptitudeSelf6,
            SpellId.CreatureEnchantmentIneptitudeSelf7,
            SpellId.CreatureEnchantmentIneptitudeSelf8,
        };

        public static readonly List<SpellId> ItemEnchantmentMasterySelf = new List<SpellId>()
        {
            SpellId.ItemEnchantmentMasterySelf1,
            SpellId.ItemEnchantmentMasterySelf2,
            SpellId.ItemEnchantmentMasterySelf3,
            SpellId.ItemEnchantmentMasterySelf4,
            SpellId.ItemEnchantmentMasterySelf5,
            SpellId.ItemEnchantmentMasterySelf6,
            SpellId.ItemEnchantmentMasterySelf7,
            SpellId.ItemEnchantmentMasterySelf8,
        };

        public static readonly List<SpellId> ItemEnchantmentMasteryOther = new List<SpellId>()
        {
            SpellId.ItemEnchantmentMasteryOther1,
            SpellId.ItemEnchantmentMasteryOther2,
            SpellId.ItemEnchantmentMasteryOther3,
            SpellId.ItemEnchantmentMasteryOther4,
            SpellId.ItemEnchantmentMasteryOther5,
            SpellId.ItemEnchantmentMasteryOther6,
            SpellId.ItemEnchantmentMasteryOther7,
            SpellId.ItemEnchantmentMasteryOther8,
        };

        public static readonly List<SpellId> ItemEnchantmentIneptitudeOther = new List<SpellId>()
        {
            SpellId.ItemEnchantmentIneptitudeOther1,
            SpellId.ItemEnchantmentIneptitudeOther2,
            SpellId.ItemEnchantmentIneptitudeOther3,
            SpellId.ItemEnchantmentIneptitudeOther4,
            SpellId.ItemEnchantmentIneptitudeOther5,
            SpellId.ItemEnchantmentIneptitudeOther6,
            SpellId.ItemEnchantmentIneptitudeOther7,
            SpellId.ItemEnchantmentIneptitudeOther8,
        };

        public static readonly List<SpellId> ItemEnchantmentIneptitudeSelf = new List<SpellId>()
        {
            SpellId.ItemEnchantmentIneptitudeSelf1,
            SpellId.ItemEnchantmentIneptitudeSelf2,
            SpellId.ItemEnchantmentIneptitudeSelf3,
            SpellId.ItemEnchantmentIneptitudeSelf4,
            SpellId.ItemEnchantmentIneptitudeSelf5,
            SpellId.ItemEnchantmentIneptitudeSelf6,
            SpellId.ItemEnchantmentIneptitudeSelf7,
            SpellId.ItemEnchantmentIneptitudeSelf8,
        };

        public static readonly List<SpellId> LifeMagicMasterySelf = new List<SpellId>()
        {
            SpellId.LifeMagicMasterySelf1,
            SpellId.LifeMagicMasterySelf2,
            SpellId.LifeMagicMasterySelf3,
            SpellId.LifeMagicMasterySelf4,
            SpellId.LifeMagicMasterySelf5,
            SpellId.LifeMagicMasterySelf6,
            SpellId.LifeMagicMasterySelf7,
            SpellId.LifeMagicMasterySelf8,
        };

        public static readonly List<SpellId> LifeMagicMasteryOther = new List<SpellId>()
        {
            SpellId.LifeMagicMasteryOther1,
            SpellId.LifeMagicMasteryOther2,
            SpellId.LifeMagicMasteryOther3,
            SpellId.LifeMagicMasteryOther4,
            SpellId.LifeMagicMasteryOther5,
            SpellId.LifeMagicMasteryOther6,
            SpellId.LifeMagicMasteryOther7,
            SpellId.LifeMagicMasteryOther8,
        };

        public static readonly List<SpellId> LifeMagicIneptitudeSelf = new List<SpellId>()
        {
            SpellId.LifeMagicIneptitudeSelf1,
            SpellId.LifeMagicIneptitudeSelf2,
            SpellId.LifeMagicIneptitudeSelf3,
            SpellId.LifeMagicIneptitudeSelf4,
            SpellId.LifeMagicIneptitudeSelf5,
            SpellId.LifeMagicIneptitudeSelf6,
            SpellId.LifeMagicIneptitudeSelf7,
            SpellId.LifeMagicIneptitudeSelf8,
        };

        public static readonly List<SpellId> LifeMagicIneptitudeOther = new List<SpellId>()
        {
            SpellId.LifeMagicIneptitudeOther1,
            SpellId.LifeMagicIneptitudeOther2,
            SpellId.LifeMagicIneptitudeOther3,
            SpellId.LifeMagicIneptitudeOther4,
            SpellId.LifeMagicIneptitudeOther5,
            SpellId.LifeMagicIneptitudeOther6,
            SpellId.LifeMagicIneptitudeOther7,
            SpellId.LifeMagicIneptitudeOther8,
        };

        public static readonly List<SpellId> WarMagicMasterySelf = new List<SpellId>()
        {
            SpellId.WarMagicMasterySelf1,
            SpellId.WarMagicMasterySelf2,
            SpellId.WarMagicMasterySelf3,
            SpellId.WarMagicMasterySelf4,
            SpellId.WarMagicMasterySelf5,
            SpellId.WarMagicMasterySelf6,
            SpellId.WarMagicMasterySelf7,
            SpellId.WarMagicMasterySelf8,
        };

        public static readonly List<SpellId> WarMagicMasteryOther = new List<SpellId>()
        {
            SpellId.WarMagicMasteryOther1,
            SpellId.WarMagicMasteryOther2,
            SpellId.WarMagicMasteryOther3,
            SpellId.WarMagicMasteryOther4,
            SpellId.WarMagicMasteryOther5,
            SpellId.WarMagicMasteryOther6,
            SpellId.WarMagicMasteryOther7,
            SpellId.WarMagicMasteryOther8,
        };

        public static readonly List<SpellId> WarMagicIneptitudeSelf = new List<SpellId>()
        {
            SpellId.WarMagicIneptitudeSelf1,
            SpellId.WarMagicIneptitudeSelf2,
            SpellId.WarMagicIneptitudeSelf3,
            SpellId.WarMagicIneptitudeSelf4,
            SpellId.WarMagicIneptitudeSelf5,
            SpellId.WarMagicIneptitudeSelf6,
            SpellId.WarMagicIneptitudeSelf7,
            SpellId.WarMagicIneptitudeSelf8,
        };

        public static readonly List<SpellId> WarMagicIneptitudeOther = new List<SpellId>()
        {
            SpellId.WarMagicIneptitudeOther1,
            SpellId.WarMagicIneptitudeOther2,
            SpellId.WarMagicIneptitudeOther3,
            SpellId.WarMagicIneptitudeOther4,
            SpellId.WarMagicIneptitudeOther5,
            SpellId.WarMagicIneptitudeOther6,
            SpellId.WarMagicIneptitudeOther7,
            SpellId.WarMagicIneptitudeOther8,
        };

        public static readonly List<SpellId> ManaMasterySelf = new List<SpellId>()
        {
            SpellId.ManaMasterySelf1,
            SpellId.ManaMasterySelf2,
            SpellId.ManaMasterySelf3,
            SpellId.ManaMasterySelf4,
            SpellId.ManaMasterySelf5,
            SpellId.ManaMasterySelf6,
            SpellId.ManaMasterySelf7,
            SpellId.ManaMasterySelf8,
        };

        public static readonly List<SpellId> ManaMasteryOther = new List<SpellId>()
        {
            SpellId.ManaMasteryOther1,
            SpellId.ManaMasteryOther2,
            SpellId.ManaMasteryOther3,
            SpellId.ManaMasteryOther4,
            SpellId.ManaMasteryOther5,
            SpellId.ManaMasteryOther6,
            SpellId.ManaMasteryOther7,
            SpellId.ManaMasteryOther8,
        };

        public static readonly List<SpellId> ManaIneptitudeSelf = new List<SpellId>()
        {
            SpellId.ManaIneptitudeSelf1,
            SpellId.ManaIneptitudeSelf2,
            SpellId.ManaIneptitudeSelf3,
            SpellId.ManaIneptitudeSelf4,
            SpellId.ManaIneptitudeSelf5,
            SpellId.ManaIneptitudeSelf6,
            SpellId.ManaIneptitudeSelf7,
            SpellId.ManaIneptitudeSelf8,
        };

        public static readonly List<SpellId> ManaIneptitudeOther = new List<SpellId>()
        {
            SpellId.ManaIneptitudeOther1,
            SpellId.ManaIneptitudeOther2,
            SpellId.ManaIneptitudeOther3,
            SpellId.ManaIneptitudeOther4,
            SpellId.ManaIneptitudeOther5,
            SpellId.ManaIneptitudeOther6,
            SpellId.ManaIneptitudeOther7,
            SpellId.ManaIneptitudeOther8,
        };

        public static readonly List<SpellId> ArcaneEnlightenmentSelf = new List<SpellId>()
        {
            SpellId.ArcaneEnlightenmentSelf1,
            SpellId.ArcaneEnlightenmentSelf2,
            SpellId.ArcaneEnlightenmentSelf3,
            SpellId.ArcaneEnlightenmentSelf4,
            SpellId.ArcaneEnlightenmentSelf5,
            SpellId.ArcaneEnlightenmentSelf6,
            SpellId.ArcaneEnlightenmentSelf7,
            SpellId.ArcaneEnlightenmentSelf8,
        };

        public static readonly List<SpellId> ArcaneEnlightenmentOther = new List<SpellId>()
        {
            SpellId.ArcaneEnlightenmentOther1,
            SpellId.ArcaneEnlightenmentOther2,
            SpellId.ArcaneEnlightenmentOther3,
            SpellId.ArcaneEnlightenmentOther4,
            SpellId.ArcaneEnlightenmentOther5,
            SpellId.ArcaneEnlightenmentOther6,
            SpellId.ArcaneEnlightenmentOther7,
            SpellId.ArcaneEnlightenmentOther8,
        };

        public static readonly List<SpellId> ArcaneBenightednessSelf = new List<SpellId>()
        {
            SpellId.ArcaneBenightednessSelf1,
            SpellId.ArcaneBenightednessSelf2,
            SpellId.ArcaneBenightednessSelf3,
            SpellId.ArcaneBenightednessSelf4,
            SpellId.ArcaneBenightednessSelf5,
            SpellId.ArcaneBenightednessSelf6,
            SpellId.ArcaneBenightednessSelf7,
            SpellId.ArcaneBenightednessSelf8,
        };

        public static readonly List<SpellId> ArcaneBenightednessOther = new List<SpellId>()
        {
            SpellId.ArcaneBenightednessOther1,
            SpellId.ArcaneBenightednessOther2,
            SpellId.ArcaneBenightednessOther3,
            SpellId.ArcaneBenightednessOther4,
            SpellId.ArcaneBenightednessOther5,
            SpellId.ArcaneBenightednessOther6,
            SpellId.ArcaneBenightednessOther7,
            SpellId.ArcaneBenightednessOther8,
        };

        public static readonly List<SpellId> ArmorExpertiseSelf = new List<SpellId>()
        {
            SpellId.ArmorExpertiseSelf1,
            SpellId.ArmorExpertiseSelf2,
            SpellId.ArmorExpertiseSelf3,
            SpellId.ArmorExpertiseSelf4,
            SpellId.ArmorExpertiseSelf5,
            SpellId.ArmorExpertiseSelf6,
            SpellId.ArmorExpertiseSelf7,
            SpellId.ArmorExpertiseSelf8,
        };

        public static readonly List<SpellId> ArmorExpertiseOther = new List<SpellId>()
        {
            SpellId.ArmorExpertiseOther1,
            SpellId.ArmorExpertiseOther2,
            SpellId.ArmorExpertiseOther3,
            SpellId.ArmorExpertiseOther4,
            SpellId.ArmorExpertiseOther5,
            SpellId.ArmorExpertiseOther6,
            SpellId.ArmorExpertiseOther7,
            SpellId.ArmorExpertiseOther8,
        };

        public static readonly List<SpellId> ArmorIgnoranceSelf = new List<SpellId>()
        {
            SpellId.ArmorIgnoranceSelf1,
            SpellId.ArmorIgnoranceSelf2,
            SpellId.ArmorIgnoranceSelf3,
            SpellId.ArmorIgnoranceSelf4,
            SpellId.ArmorIgnoranceSelf5,
            SpellId.ArmorIgnoranceSelf6,
            SpellId.ArmorIgnoranceSelf7,
            SpellId.ArmorIgnoranceSelf8,
        };

        public static readonly List<SpellId> ArmorIgnoranceOther = new List<SpellId>()
        {
            SpellId.ArmorIgnoranceOther1,
            SpellId.ArmorIgnoranceOther2,
            SpellId.ArmorIgnoranceOther3,
            SpellId.ArmorIgnoranceOther4,
            SpellId.ArmorIgnoranceOther5,
            SpellId.ArmorIgnoranceOther6,
            SpellId.ArmorIgnoranceOther7,
            SpellId.ArmorIgnoranceOther8,
        };

        public static readonly List<SpellId> ItemExpertiseSelf = new List<SpellId>()
        {
            SpellId.ItemExpertiseSelf1,
            SpellId.ItemExpertiseSelf2,
            SpellId.ItemExpertiseSelf3,
            SpellId.ItemExpertiseSelf4,
            SpellId.ItemExpertiseSelf5,
            SpellId.ItemExpertiseSelf6,
            SpellId.ItemExpertiseSelf7,
            SpellId.ItemExpertiseSelf8,
        };

        public static readonly List<SpellId> ItemExpertiseOther = new List<SpellId>()
        {
            SpellId.ItemExpertiseOther1,
            SpellId.ItemExpertiseOther2,
            SpellId.ItemExpertiseOther3,
            SpellId.ItemExpertiseOther4,
            SpellId.ItemExpertiseOther5,
            SpellId.ItemExpertiseOther6,
            SpellId.ItemExpertiseOther7,
            SpellId.ItemExpertiseOther8,
        };

        public static readonly List<SpellId> ItemIgnoranceSelf = new List<SpellId>()
        {
            SpellId.ItemIgnoranceSelf1,
            SpellId.ItemIgnoranceSelf2,
            SpellId.ItemIgnoranceSelf3,
            SpellId.ItemIgnoranceSelf4,
            SpellId.ItemIgnoranceSelf5,
            SpellId.ItemIgnoranceSelf6,
            SpellId.ItemIgnoranceSelf7,
            SpellId.ItemIgnoranceSelf8,
        };

        public static readonly List<SpellId> ItemIgnoranceOther = new List<SpellId>()
        {
            SpellId.ItemIgnoranceOther1,
            SpellId.ItemIgnoranceOther2,
            SpellId.ItemIgnoranceOther3,
            SpellId.ItemIgnoranceOther4,
            SpellId.ItemIgnoranceOther5,
            SpellId.ItemIgnoranceOther6,
            SpellId.ItemIgnoranceOther7,
            SpellId.ItemIgnoranceOther8,
        };

        public static readonly List<SpellId> MagicItemExpertiseSelf = new List<SpellId>()
        {
            SpellId.MagicItemExpertiseSelf1,
            SpellId.MagicItemExpertiseSelf2,
            SpellId.MagicItemExpertiseSelf3,
            SpellId.MagicItemExpertiseSelf4,
            SpellId.MagicItemExpertiseSelf5,
            SpellId.MagicItemExpertiseSelf6,
            SpellId.MagicItemExpertiseSelf7,
            SpellId.MagicItemExpertiseSelf8,
        };

        public static readonly List<SpellId> MagicItemExpertiseOther = new List<SpellId>()
        {
            SpellId.MagicItemExpertiseOther1,
            SpellId.MagicItemExpertiseOther2,
            SpellId.MagicItemExpertiseOther3,
            SpellId.MagicItemExpertiseOther4,
            SpellId.MagicItemExpertiseOther5,
            SpellId.MagicItemExpertiseOther6,
            SpellId.MagicItemExpertiseOther7,
            SpellId.MagicItemExpertiseOther8,
        };

        public static readonly List<SpellId> MagicItemIgnoranceSelf = new List<SpellId>()
        {
            SpellId.MagicItemIgnoranceSelf1,
            SpellId.MagicItemIgnoranceSelf2,
            SpellId.MagicItemIgnoranceSelf3,
            SpellId.MagicItemIgnoranceSelf4,
            SpellId.MagicItemIgnoranceSelf5,
            SpellId.MagicItemIgnoranceSelf6,
            SpellId.MagicItemIgnoranceSelf7,
            SpellId.MagicItemIgnoranceSelf8,
        };

        public static readonly List<SpellId> MagicItemIgnoranceOther = new List<SpellId>()
        {
            SpellId.MagicItemIgnoranceOther1,
            SpellId.MagicItemIgnoranceOther2,
            SpellId.MagicItemIgnoranceOther3,
            SpellId.MagicItemIgnoranceOther4,
            SpellId.MagicItemIgnoranceOther5,
            SpellId.MagicItemIgnoranceOther6,
            SpellId.MagicItemIgnoranceOther7,
            SpellId.MagicItemIgnoranceOther8,
        };

        public static readonly List<SpellId> WeaponExpertiseSelf = new List<SpellId>()
        {
            SpellId.WeaponExpertiseSelf1,
            SpellId.WeaponExpertiseSelf2,
            SpellId.WeaponExpertiseSelf3,
            SpellId.WeaponExpertiseSelf4,
            SpellId.WeaponExpertiseSelf5,
            SpellId.WeaponExpertiseSelf6,
            SpellId.WeaponExpertiseSelf7,
            SpellId.WeaponExpertiseSelf8,
        };

        public static readonly List<SpellId> WeaponExpertiseOther = new List<SpellId>()
        {
            SpellId.WeaponExpertiseOther1,
            SpellId.WeaponExpertiseOther2,
            SpellId.WeaponExpertiseOther3,
            SpellId.WeaponExpertiseOther4,
            SpellId.WeaponExpertiseOther5,
            SpellId.WeaponExpertiseOther6,
            SpellId.WeaponExpertiseOther7,
            SpellId.WeaponExpertiseOther8,
        };

        public static readonly List<SpellId> WeaponIgnoranceSelf = new List<SpellId>()
        {
            SpellId.WeaponIgnoranceSelf1,
            SpellId.WeaponIgnoranceSelf2,
            SpellId.WeaponIgnoranceSelf3,
            SpellId.WeaponIgnoranceSelf4,
            SpellId.WeaponIgnoranceSelf5,
            SpellId.WeaponIgnoranceSelf6,
            SpellId.WeaponIgnoranceSelf7,
            SpellId.WeaponIgnoranceSelf8,
        };

        public static readonly List<SpellId> WeaponIgnoranceOther = new List<SpellId>()
        {
            SpellId.WeaponIgnoranceOther1,
            SpellId.WeaponIgnoranceOther2,
            SpellId.WeaponIgnoranceOther3,
            SpellId.WeaponIgnoranceOther4,
            SpellId.WeaponIgnoranceOther5,
            SpellId.WeaponIgnoranceOther6,
            SpellId.WeaponIgnoranceOther7,
            SpellId.WeaponIgnoranceOther8,
        };

        public static readonly List<SpellId> MonsterAttunementSelf = new List<SpellId>()
        {
            SpellId.MonsterAttunementSelf1,
            SpellId.MonsterAttunementSelf2,
            SpellId.MonsterAttunementSelf3,
            SpellId.MonsterAttunementSelf4,
            SpellId.MonsterAttunementSelf5,
            SpellId.MonsterAttunementSelf6,
            SpellId.MonsterAttunementSelf7,
            SpellId.MonsterAttunementSelf8,
        };

        public static readonly List<SpellId> MonsterAttunementOther = new List<SpellId>()
        {
            SpellId.MonsterAttunementOther1,
            SpellId.MonsterAttunementOther2,
            SpellId.MonsterAttunementOther3,
            SpellId.MonsterAttunementOther4,
            SpellId.MonsterAttunementOther5,
            SpellId.MonsterAttunementOther6,
            SpellId.MonsterAttunementOther7,
            SpellId.MonsterAttunementOther8,
        };

        public static readonly List<SpellId> MonsterUnfamiliaritySelf = new List<SpellId>()
        {
            SpellId.MonsterUnfamiliaritySelf1,
            SpellId.MonsterUnfamiliaritySelf2,
            SpellId.MonsterUnfamiliaritySelf3,
            SpellId.MonsterUnfamiliaritySelf4,
            SpellId.MonsterUnfamiliaritySelf5,
            SpellId.MonsterUnfamiliaritySelf6,
            SpellId.MonsterUnfamiliaritySelf7,
            SpellId.MonsterUnfamiliaritySelf8,
        };

        public static readonly List<SpellId> MonsterUnfamiliarityOther = new List<SpellId>()
        {
            SpellId.MonsterUnfamiliarityOther1,
            SpellId.MonsterUnfamiliarityOther2,
            SpellId.MonsterUnfamiliarityOther3,
            SpellId.MonsterUnfamiliarityOther4,
            SpellId.MonsterUnfamiliarityOther5,
            SpellId.MonsterUnfamiliarityOther6,
            SpellId.MonsterUnfamiliarityOther7,
            SpellId.MonsterUnfamiliarityOther8,
        };

        public static readonly List<SpellId> PersonAttunementSelf = new List<SpellId>()
        {
            SpellId.PersonAttunementSelf1,
            SpellId.PersonAttunementSelf2,
            SpellId.PersonAttunementSelf3,
            SpellId.PersonAttunementSelf4,
            SpellId.PersonAttunementSelf5,
            SpellId.PersonAttunementSelf6,
            SpellId.PersonAttunementSelf7,
            SpellId.PersonAttunementSelf8,
        };

        public static readonly List<SpellId> PersonAttunementOther = new List<SpellId>()
        {
            SpellId.PersonAttunementOther1,
            SpellId.PersonAttunementOther2,
            SpellId.PersonAttunementOther3,
            SpellId.PersonAttunementOther4,
            SpellId.PersonAttunementOther5,
            SpellId.PersonAttunementOther6,
            SpellId.PersonAttunementOther7,
            SpellId.PersonAttunementOther8,
        };

        public static readonly List<SpellId> PersonUnfamiliaritySelf = new List<SpellId>()
        {
            SpellId.PersonUnfamiliaritySelf1,
            SpellId.PersonUnfamiliaritySelf2,
            SpellId.PersonUnfamiliaritySelf3,
            SpellId.PersonUnfamiliaritySelf4,
            SpellId.PersonUnfamiliaritySelf5,
            SpellId.PersonUnfamiliaritySelf6,
            SpellId.PersonUnfamiliaritySelf7,
            SpellId.PersonUnfamiliaritySelf8,
        };

        public static readonly List<SpellId> PersonUnfamiliarityOther = new List<SpellId>()
        {
            SpellId.PersonUnfamiliarityOther1,
            SpellId.PersonUnfamiliarityOther2,
            SpellId.PersonUnfamiliarityOther3,
            SpellId.PersonUnfamiliarityOther4,
            SpellId.PersonUnfamiliarityOther5,
            SpellId.PersonUnfamiliarityOther6,
            SpellId.PersonUnfamiliarityOther7,
            SpellId.PersonUnfamiliarityOther8,
        };

        public static readonly List<SpellId> DeceptionMasterySelf = new List<SpellId>()
        {
            SpellId.DeceptionMasterySelf1,
            SpellId.DeceptionMasterySelf2,
            SpellId.DeceptionMasterySelf3,
            SpellId.DeceptionMasterySelf4,
            SpellId.DeceptionMasterySelf5,
            SpellId.DeceptionMasterySelf6,
            SpellId.DeceptionMasterySelf7,
            SpellId.DeceptionMasterySelf8,
        };

        public static readonly List<SpellId> DeceptionMasteryOther = new List<SpellId>()
        {
            SpellId.DeceptionMasteryOther1,
            SpellId.DeceptionMasteryOther2,
            SpellId.DeceptionMasteryOther3,
            SpellId.DeceptionMasteryOther4,
            SpellId.DeceptionMasteryOther5,
            SpellId.DeceptionMasteryOther6,
            SpellId.DeceptionMasteryOther7,
            SpellId.DeceptionMasteryOther8,
        };

        public static readonly List<SpellId> DeceptionIneptitudeSelf = new List<SpellId>()
        {
            SpellId.DeceptionIneptitudeSelf1,
            SpellId.DeceptionIneptitudeSelf2,
            SpellId.DeceptionIneptitudeSelf3,
            SpellId.DeceptionIneptitudeSelf4,
            SpellId.DeceptionIneptitudeSelf5,
            SpellId.DeceptionIneptitudeSelf6,
            SpellId.DeceptionIneptitudeSelf7,
            SpellId.DeceptionIneptitudeSelf8,
        };

        public static readonly List<SpellId> DeceptionIneptitudeOther = new List<SpellId>()
        {
            SpellId.DeceptionIneptitudeOther1,
            SpellId.DeceptionIneptitudeOther2,
            SpellId.DeceptionIneptitudeOther3,
            SpellId.DeceptionIneptitudeOther4,
            SpellId.DeceptionIneptitudeOther5,
            SpellId.DeceptionIneptitudeOther6,
            SpellId.DeceptionIneptitudeOther7,
            SpellId.DeceptionIneptitudeOther8,
        };

        public static readonly List<SpellId> HealingMasterySelf = new List<SpellId>()
        {
            SpellId.HealingMasterySelf1,
            SpellId.HealingMasterySelf2,
            SpellId.HealingMasterySelf3,
            SpellId.HealingMasterySelf4,
            SpellId.HealingMasterySelf5,
            SpellId.HealingMasterySelf6,
            SpellId.HealingMasterySelf7,
            SpellId.HealingMasterySelf8,
        };

        public static readonly List<SpellId> HealingMasteryOther = new List<SpellId>()
        {
            SpellId.HealingMasteryOther1,
            SpellId.HealingMasteryOther2,
            SpellId.HealingMasteryOther3,
            SpellId.HealingMasteryOther4,
            SpellId.HealingMasteryOther5,
            SpellId.HealingMasteryOther6,
            SpellId.HealingMasteryOther7,
            SpellId.HealingMasteryOther8,
        };

        public static readonly List<SpellId> HealingIneptitudeSelf = new List<SpellId>()
        {
            SpellId.HealingIneptitudeSelf1,
            SpellId.HealingIneptitudeSelf2,
            SpellId.HealingIneptitudeSelf3,
            SpellId.HealingIneptitudeSelf4,
            SpellId.HealingIneptitudeSelf5,
            SpellId.HealingIneptitudeSelf6,
            SpellId.HealingIneptitudeSelf7,
            SpellId.HealingIneptitudeSelf8,
        };

        public static readonly List<SpellId> HealingIneptitudeOther = new List<SpellId>()
        {
            SpellId.HealingIneptitudeOther1,
            SpellId.HealingIneptitudeOther2,
            SpellId.HealingIneptitudeOther3,
            SpellId.HealingIneptitudeOther4,
            SpellId.HealingIneptitudeOther5,
            SpellId.HealingIneptitudeOther6,
            SpellId.HealingIneptitudeOther7,
            SpellId.HealingIneptitudeOther8,
        };

        public static readonly List<SpellId> LeadershipMasterySelf = new List<SpellId>()
        {
            SpellId.LeadershipMasterySelf1,
            SpellId.LeadershipMasterySelf2,
            SpellId.LeadershipMasterySelf3,
            SpellId.LeadershipMasterySelf4,
            SpellId.LeadershipMasterySelf5,
            SpellId.LeadershipMasterySelf6,
            SpellId.LeadershipMasterySelf7,
            SpellId.LeadershipMasterySelf8,
        };

        public static readonly List<SpellId> LeadershipMasteryOther = new List<SpellId>()
        {
            SpellId.LeadershipMasteryOther1,
            SpellId.LeadershipMasteryOther2,
            SpellId.LeadershipMasteryOther3,
            SpellId.LeadershipMasteryOther4,
            SpellId.LeadershipMasteryOther5,
            SpellId.LeadershipMasteryOther6,
            SpellId.LeadershipMasteryOther7,
            SpellId.LeadershipMasteryOther8,
        };

        public static readonly List<SpellId> LeadershipIneptitudeSelf = new List<SpellId>()
        {
            SpellId.LeadershipIneptitudeSelf1,
            SpellId.LeadershipIneptitudeSelf2,
            SpellId.LeadershipIneptitudeSelf3,
            SpellId.LeadershipIneptitudeSelf4,
            SpellId.LeadershipIneptitudeSelf5,
            SpellId.LeadershipIneptitudeSelf6,
            SpellId.LeadershipIneptitudeSelf7,
            SpellId.LeadershipIneptitudeSelf8,
        };

        public static readonly List<SpellId> LeadershipIneptitudeOther = new List<SpellId>()
        {
            SpellId.LeadershipIneptitudeOther1,
            SpellId.LeadershipIneptitudeOther2,
            SpellId.LeadershipIneptitudeOther3,
            SpellId.LeadershipIneptitudeOther4,
            SpellId.LeadershipIneptitudeOther5,
            SpellId.LeadershipIneptitudeOther6,
            SpellId.LeadershipIneptitudeOther7,
            SpellId.LeadershipIneptitudeOther8,
        };

        public static readonly List<SpellId> LockpickMasterySelf = new List<SpellId>()
        {
            SpellId.LockpickMasterySelf1,
            SpellId.LockpickMasterySelf2,
            SpellId.LockpickMasterySelf3,
            SpellId.LockpickMasterySelf4,
            SpellId.LockpickMasterySelf5,
            SpellId.LockpickMasterySelf6,
            SpellId.LockpickMasterySelf7,
            SpellId.LockpickMasterySelf8,
        };

        public static readonly List<SpellId> LockpickMasteryOther = new List<SpellId>()
        {
            SpellId.LockpickMasteryOther1,
            SpellId.LockpickMasteryOther2,
            SpellId.LockpickMasteryOther3,
            SpellId.LockpickMasteryOther4,
            SpellId.LockpickMasteryOther5,
            SpellId.LockpickMasteryOther6,
            SpellId.LockpickMasteryOther7,
            SpellId.LockpickMasteryOther8,
        };

        public static readonly List<SpellId> LockpickIneptitudeSelf = new List<SpellId>()
        {
            SpellId.LockpickIneptitudeSelf1,
            SpellId.LockpickIneptitudeSelf2,
            SpellId.LockpickIneptitudeSelf3,
            SpellId.LockpickIneptitudeSelf4,
            SpellId.LockpickIneptitudeSelf5,
            SpellId.LockpickIneptitudeSelf6,
            SpellId.LockpickIneptitudeSelf7,
            SpellId.LockpickIneptitudeSelf8,
        };

        public static readonly List<SpellId> LockpickIneptitudeOther = new List<SpellId>()
        {
            SpellId.LockpickIneptitudeOther1,
            SpellId.LockpickIneptitudeOther2,
            SpellId.LockpickIneptitudeOther3,
            SpellId.LockpickIneptitudeOther4,
            SpellId.LockpickIneptitudeOther5,
            SpellId.LockpickIneptitudeOther6,
            SpellId.LockpickIneptitudeOther7,
            SpellId.LockpickIneptitudeOther8,
        };

        public static readonly List<SpellId> FealtySelf = new List<SpellId>()
        {
            SpellId.FealtySelf1,
            SpellId.FealtySelf2,
            SpellId.FealtySelf3,
            SpellId.FealtySelf4,
            SpellId.FealtySelf5,
            SpellId.FealtySelf6,
            SpellId.FealtySelf7,
            SpellId.FealtySelf8,
        };

        public static readonly List<SpellId> FealtyOther = new List<SpellId>()
        {
            SpellId.FealtyOther1,
            SpellId.FealtyOther2,
            SpellId.FealtyOther3,
            SpellId.FealtyOther4,
            SpellId.FealtyOther5,
            SpellId.FealtyOther6,
            SpellId.FealtyOther7,
            SpellId.FealtyOther8,
        };

        public static readonly List<SpellId> FaithlessnessSelf = new List<SpellId>()
        {
            SpellId.FaithlessnessSelf1,
            SpellId.FaithlessnessSelf2,
            SpellId.FaithlessnessSelf3,
            SpellId.FaithlessnessSelf4,
            SpellId.FaithlessnessSelf5,
            SpellId.FaithlessnessSelf6,
            SpellId.FaithlessnessSelf7,
            SpellId.FaithlessnessSelf8,
        };

        public static readonly List<SpellId> FaithlessnessOther = new List<SpellId>()
        {
            SpellId.FaithlessnessOther1,
            SpellId.FaithlessnessOther2,
            SpellId.FaithlessnessOther3,
            SpellId.FaithlessnessOther4,
            SpellId.FaithlessnessOther5,
            SpellId.FaithlessnessOther6,
            SpellId.FaithlessnessOther7,
            SpellId.FaithlessnessOther8,
        };

        public static readonly List<SpellId> JumpingMasterySelf = new List<SpellId>()
        {
            SpellId.JumpingMasterySelf1,
            SpellId.JumpingMasterySelf2,
            SpellId.JumpingMasterySelf3,
            SpellId.JumpingMasterySelf4,
            SpellId.JumpingMasterySelf5,
            SpellId.JumpingMasterySelf6,
            SpellId.JumpingMasterySelf7,
            SpellId.JumpingMasterySelf8,
        };

        public static readonly List<SpellId> JumpingMasteryOther = new List<SpellId>()
        {
            SpellId.JumpingMasteryOther1,
            SpellId.JumpingMasteryOther2,
            SpellId.JumpingMasteryOther3,
            SpellId.JumpingMasteryOther4,
            SpellId.JumpingMasteryOther5,
            SpellId.JumpingMasteryOther6,
            SpellId.JumpingMasteryOther7,
            SpellId.JumpingMasteryOther8,
        };

        public static readonly List<SpellId> SprintSelf = new List<SpellId>()
        {
            SpellId.SprintSelf1,
            SpellId.SprintSelf2,
            SpellId.SprintSelf3,
            SpellId.SprintSelf4,
            SpellId.SprintSelf5,
            SpellId.SprintSelf6,
            SpellId.SprintSelf7,
            SpellId.SprintSelf8,
        };

        public static readonly List<SpellId> SprintOther = new List<SpellId>()
        {
            SpellId.SprintOther1,
            SpellId.SprintOther2,
            SpellId.SprintOther3,
            SpellId.SprintOther4,
            SpellId.SprintOther5,
            SpellId.SprintOther6,
            SpellId.SprintOther7,
            SpellId.SprintOther8,
        };

        public static readonly List<SpellId> LeadenFeetSelf = new List<SpellId>()
        {
            SpellId.LeadenFeetSelf1,
            SpellId.LeadenFeetSelf2,
            SpellId.LeadenFeetSelf3,
            SpellId.LeadenFeetSelf4,
            SpellId.LeadenFeetSelf5,
            SpellId.LeadenFeetSelf6,
            SpellId.LeadenFeetSelf7,
            SpellId.LeadenFeetSelf8,
        };

        public static readonly List<SpellId> LeadenFeetOther = new List<SpellId>()
        {
            SpellId.LeadenFeetOther1,
            SpellId.LeadenFeetOther2,
            SpellId.LeadenFeetOther3,
            SpellId.LeadenFeetOther4,
            SpellId.LeadenFeetOther5,
            SpellId.LeadenFeetOther6,
            SpellId.LeadenFeetOther7,
            SpellId.LeadenFeetOther8,
        };

        public static readonly List<SpellId> JumpingIneptitudeSelf = new List<SpellId>()
        {
            SpellId.JumpingIneptitudeSelf1,
            SpellId.JumpingIneptitudeSelf2,
            SpellId.JumpingIneptitudeSelf3,
            SpellId.JumpingIneptitudeSelf4,
            SpellId.JumpingIneptitudeSelf5,
            SpellId.JumpingIneptitudeSelf6,
            SpellId.JumpingIneptitudeSelf7,
            SpellId.JumpingIneptitudeSelf8,
        };

        public static readonly List<SpellId> JumpingIneptitudeOther = new List<SpellId>()
        {
            SpellId.JumpingIneptitudeOther1,
            SpellId.JumpingIneptitudeOther2,
            SpellId.JumpingIneptitudeOther3,
            SpellId.JumpingIneptitudeOther4,
            SpellId.JumpingIneptitudeOther5,
            SpellId.JumpingIneptitudeOther6,
            SpellId.JumpingIneptitudeOther7,
            SpellId.JumpingIneptitudeOther8,
        };

        public static readonly List<SpellId> BludgeonProtectionSelf = new List<SpellId>()
        {
            SpellId.BludgeonProtectionSelf1,
            SpellId.BludgeonProtectionSelf2,
            SpellId.BludgeonProtectionSelf3,
            SpellId.BludgeonProtectionSelf4,
            SpellId.BludgeonProtectionSelf5,
            SpellId.BludgeonProtectionSelf6,
            SpellId.BludgeonProtectionSelf7,
            SpellId.BludgeonProtectionSelf8,
        };

        public static readonly List<SpellId> BludgeonProtectionOther = new List<SpellId>()
        {
            SpellId.BludgeonProtectionOther1,
            SpellId.BludgeonProtectionOther2,
            SpellId.BludgeonProtectionOther3,
            SpellId.BludgeonProtectionOther4,
            SpellId.BludgeonProtectionOther5,
            SpellId.BludgeonProtectionOther6,
            SpellId.BludgeonProtectionOther7,
            SpellId.BludgeonProtectionOther8,
        };

        public static readonly List<SpellId> ColdProtectionSelf = new List<SpellId>()
        {
            SpellId.ColdProtectionSelf1,
            SpellId.ColdProtectionSelf2,
            SpellId.ColdProtectionSelf3,
            SpellId.ColdProtectionSelf4,
            SpellId.ColdProtectionSelf5,
            SpellId.ColdProtectionSelf6,
            SpellId.ColdProtectionSelf7,
            SpellId.ColdProtectionSelf8,
        };

        public static readonly List<SpellId> ColdProtectionOther = new List<SpellId>()
        {
            SpellId.ColdProtectionOther1,
            SpellId.ColdProtectionOther2,
            SpellId.ColdProtectionOther3,
            SpellId.ColdProtectionOther4,
            SpellId.ColdProtectionOther5,
            SpellId.ColdProtectionOther6,
            SpellId.ColdProtectionOther7,
            SpellId.ColdProtectionOther8,
        };

        public static readonly List<SpellId> BludgeonVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.BludgeonVulnerabilitySelf1,
            SpellId.BludgeonVulnerabilitySelf2,
            SpellId.BludgeonVulnerabilitySelf3,
            SpellId.BludgeonVulnerabilitySelf4,
            SpellId.BludgeonVulnerabilitySelf5,
            SpellId.BludgeonVulnerabilitySelf6,
            SpellId.BludgeonVulnerabilitySelf7,
            SpellId.BludgeonVulnerabilitySelf8,
        };

        public static readonly List<SpellId> BludgeonVulnerabilityOther = new List<SpellId>()
        {
            SpellId.BludgeonVulnerabilityOther1,
            SpellId.BludgeonVulnerabilityOther2,
            SpellId.BludgeonVulnerabilityOther3,
            SpellId.BludgeonVulnerabilityOther4,
            SpellId.BludgeonVulnerabilityOther5,
            SpellId.BludgeonVulnerabilityOther6,
            SpellId.BludgeonVulnerabilityOther7,
            SpellId.BludgeonVulnerabilityOther8,
        };

        public static readonly List<SpellId> ColdVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.ColdVulnerabilitySelf1,
            SpellId.ColdVulnerabilitySelf2,
            SpellId.ColdVulnerabilitySelf3,
            SpellId.ColdVulnerabilitySelf4,
            SpellId.ColdVulnerabilitySelf5,
            SpellId.ColdVulnerabilitySelf6,
            SpellId.ColdVulnerabilitySelf7,
            SpellId.ColdVulnerabilitySelf8,
        };

        public static readonly List<SpellId> ColdVulnerabilityOther = new List<SpellId>()
        {
            SpellId.ColdVulnerabilityOther1,
            SpellId.ColdVulnerabilityOther2,
            SpellId.ColdVulnerabilityOther3,
            SpellId.ColdVulnerabilityOther4,
            SpellId.ColdVulnerabilityOther5,
            SpellId.ColdVulnerabilityOther6,
            SpellId.ColdVulnerabilityOther7,
            SpellId.ColdVulnerabilityOther8,
        };

        public static readonly List<SpellId> LightningProtectionSelf = new List<SpellId>()
        {
            SpellId.LightningProtectionSelf1,
            SpellId.LightningProtectionSelf2,
            SpellId.LightningProtectionSelf3,
            SpellId.LightningProtectionSelf4,
            SpellId.LightningProtectionSelf5,
            SpellId.LightningProtectionSelf6,
            SpellId.LightningProtectionSelf7,
            SpellId.LightningProtectionSelf8,
        };

        public static readonly List<SpellId> LightningProtectionOther = new List<SpellId>()
        {
            SpellId.LightningProtectionOther1,
            SpellId.LightningProtectionOther2,
            SpellId.LightningProtectionOther3,
            SpellId.LightningProtectionOther4,
            SpellId.LightningProtectionOther5,
            SpellId.LightningProtectionOther6,
            SpellId.LightningProtectionOther7,
            SpellId.LightningProtectionOther8,
        };

        public static readonly List<SpellId> LightningVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.LightningVulnerabilitySelf1,
            SpellId.LightningVulnerabilitySelf2,
            SpellId.LightningVulnerabilitySelf3,
            SpellId.LightningVulnerabilitySelf4,
            SpellId.LightningVulnerabilitySelf5,
            SpellId.LightningVulnerabilitySelf6,
            SpellId.LightningVulnerabilitySelf7,
            SpellId.LightningVulnerabilitySelf8,
        };

        public static readonly List<SpellId> LightningVulnerabilityOther = new List<SpellId>()
        {
            SpellId.LightningVulnerabilityOther1,
            SpellId.LightningVulnerabilityOther2,
            SpellId.LightningVulnerabilityOther3,
            SpellId.LightningVulnerabilityOther4,
            SpellId.LightningVulnerabilityOther5,
            SpellId.LightningVulnerabilityOther6,
            SpellId.LightningVulnerabilityOther7,
            SpellId.LightningVulnerabilityOther8,
        };

        public static readonly List<SpellId> BladeProtectionSelf = new List<SpellId>()
        {
            SpellId.BladeProtectionSelf1,
            SpellId.BladeProtectionSelf2,
            SpellId.BladeProtectionSelf3,
            SpellId.BladeProtectionSelf4,
            SpellId.BladeProtectionSelf5,
            SpellId.BladeProtectionSelf6,
            SpellId.BladeProtectionSelf7,
            SpellId.BladeProtectionSelf8,
        };

        public static readonly List<SpellId> BladeProtectionOther = new List<SpellId>()
        {
            SpellId.BladeProtectionOther1,
            SpellId.BladeProtectionOther2,
            SpellId.BladeProtectionOther3,
            SpellId.BladeProtectionOther4,
            SpellId.BladeProtectionOther5,
            SpellId.BladeProtectionOther6,
            SpellId.BladeProtectionOther7,
            SpellId.BladeProtectionOther8,
        };

        public static readonly List<SpellId> BladeVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.BladeVulnerabilitySelf1,
            SpellId.BladeVulnerabilitySelf2,
            SpellId.BladeVulnerabilitySelf3,
            SpellId.BladeVulnerabilitySelf4,
            SpellId.BladeVulnerabilitySelf5,
            SpellId.BladeVulnerabilitySelf6,
            SpellId.BladeVulnerabilitySelf7,
            SpellId.BladeVulnerabilitySelf8,
        };

        public static readonly List<SpellId> BladeVulnerabilityOther = new List<SpellId>()
        {
            SpellId.BladeVulnerabilityOther1,
            SpellId.BladeVulnerabilityOther2,
            SpellId.BladeVulnerabilityOther3,
            SpellId.BladeVulnerabilityOther4,
            SpellId.BladeVulnerabilityOther5,
            SpellId.BladeVulnerabilityOther6,
            SpellId.BladeVulnerabilityOther7,
            SpellId.BladeVulnerabilityOther8,
        };

        public static readonly List<SpellId> PiercingProtectionSelf = new List<SpellId>()
        {
            SpellId.PiercingProtectionSelf1,
            SpellId.PiercingProtectionSelf2,
            SpellId.PiercingProtectionSelf3,
            SpellId.PiercingProtectionSelf4,
            SpellId.PiercingProtectionSelf5,
            SpellId.PiercingProtectionSelf6,
            SpellId.PiercingProtectionSelf7,
            SpellId.PiercingProtectionSelf8,
        };

        public static readonly List<SpellId> PiercingProtectionOther = new List<SpellId>()
        {
            SpellId.PiercingProtectionOther1,
            SpellId.PiercingProtectionOther2,
            SpellId.PiercingProtectionOther3,
            SpellId.PiercingProtectionOther4,
            SpellId.PiercingProtectionOther5,
            SpellId.PiercingProtectionOther6,
            SpellId.PiercingProtectionOther7,
            SpellId.PiercingProtectionOther8,
        };

        public static readonly List<SpellId> PiercingVulnerabilitySelf = new List<SpellId>()
        {
            SpellId.PiercingVulnerabilitySelf1,
            SpellId.PiercingVulnerabilitySelf2,
            SpellId.PiercingVulnerabilitySelf3,
            SpellId.PiercingVulnerabilitySelf4,
            SpellId.PiercingVulnerabilitySelf5,
            SpellId.PiercingVulnerabilitySelf6,
            SpellId.PiercingVulnerabilitySelf7,
            SpellId.PiercingVulnerabilitySelf8,
        };

        public static readonly List<SpellId> PiercingVulnerabilityOther = new List<SpellId>()
        {
            SpellId.PiercingVulnerabilityOther1,
            SpellId.PiercingVulnerabilityOther2,
            SpellId.PiercingVulnerabilityOther3,
            SpellId.PiercingVulnerabilityOther4,
            SpellId.PiercingVulnerabilityOther5,
            SpellId.PiercingVulnerabilityOther6,
            SpellId.PiercingVulnerabilityOther7,
            SpellId.PiercingVulnerabilityOther8,
        };

        public static readonly List<SpellId> RevitalizeSelf = new List<SpellId>()
        {
            SpellId.RevitalizeSelf1,
            SpellId.RevitalizeSelf2,
            SpellId.RevitalizeSelf3,
            SpellId.RevitalizeSelf4,
            SpellId.RevitalizeSelf5,
            SpellId.RevitalizeSelf6,
            SpellId.RevitalizeSelf7,
            SpellId.RevitalizeSelf8,
        };

        public static readonly List<SpellId> RevitalizeOther = new List<SpellId>()
        {
            SpellId.RevitalizeOther1,
            SpellId.RevitalizeOther2,
            SpellId.RevitalizeOther3,
            SpellId.RevitalizeOther4,
            SpellId.RevitalizeOther5,
            SpellId.RevitalizeOther6,
            SpellId.RevitalizeOther7,
            SpellId.RevitalizeOther8,
        };

        public static readonly List<SpellId> EnfeebleSelf = new List<SpellId>()
        {
            SpellId.EnfeebleSelf1,
            SpellId.EnfeebleSelf2,
            SpellId.EnfeebleSelf3,
            SpellId.EnfeebleSelf4,
            SpellId.EnfeebleSelf5,
            SpellId.EnfeebleSelf6,
            SpellId.EnfeebleSelf7,
            SpellId.EnfeebleSelf8,
        };

        public static readonly List<SpellId> EnfeebleOther = new List<SpellId>()
        {
            SpellId.EnfeebleOther1,
            SpellId.EnfeebleOther2,
            SpellId.EnfeebleOther3,
            SpellId.EnfeebleOther4,
            SpellId.EnfeebleOther5,
            SpellId.EnfeebleOther6,
            SpellId.EnfeebleOther7,
            SpellId.EnfeebleOther8,
        };

        public static readonly List<SpellId> ManaBoostSelf = new List<SpellId>()
        {
            SpellId.ManaBoostSelf1,
            SpellId.ManaBoostSelf2,
            SpellId.ManaBoostSelf3,
            SpellId.ManaBoostSelf4,
            SpellId.ManaBoostSelf5,
            SpellId.ManaBoostSelf6,
            SpellId.ManaBoostSelf7,
            SpellId.ManaBoostSelf8,
        };

        public static readonly List<SpellId> ManaBoostOther = new List<SpellId>()
        {
            SpellId.ManaBoostOther1,
            SpellId.ManaBoostOther2,
            SpellId.ManaBoostOther3,
            SpellId.ManaBoostOther4,
            SpellId.ManaBoostOther5,
            SpellId.ManaBoostOther6,
            SpellId.ManaBoostOther7,
            SpellId.ManaBoostOther8,
        };

        public static readonly List<SpellId> ManaDrainSelf = new List<SpellId>()
        {
            SpellId.ManaDrainSelf1,
            SpellId.ManaDrainSelf2,
            SpellId.ManaDrainSelf3,
            SpellId.ManaDrainSelf4,
            SpellId.ManaDrainSelf5,
            SpellId.ManaDrainSelf6,
            SpellId.ManaDrainSelf7,
            SpellId.ManaDrainSelf8,
        };

        public static readonly List<SpellId> ManaDrainOther = new List<SpellId>()
        {
            SpellId.ManaDrainOther1,
            SpellId.ManaDrainOther2,
            SpellId.ManaDrainOther3,
            SpellId.ManaDrainOther4,
            SpellId.ManaDrainOther5,
            SpellId.ManaDrainOther6,
            SpellId.ManaDrainOther7,
            SpellId.ManaDrainOther8,
        };

        public static readonly List<SpellId> InfuseHealth = new List<SpellId>()
        {
            SpellId.InfuseHealth1,
            SpellId.InfuseHealth2,
            SpellId.InfuseHealth3,
            SpellId.InfuseHealth4,
            SpellId.InfuseHealth5,
            SpellId.InfuseHealth6,
            SpellId.InfuseHealth7,
            SpellId.InfuseHealth8,
        };

        public static readonly List<SpellId> DrainHealth = new List<SpellId>()
        {
            SpellId.DrainHealth1,
            SpellId.DrainHealth2,
            SpellId.DrainHealth3,
            SpellId.DrainHealth4,
            SpellId.DrainHealth5,
            SpellId.DrainHealth6,
            SpellId.DrainHealth7,
            SpellId.DrainHealth8,
        };

        public static readonly List<SpellId> InfuseStamina = new List<SpellId>()
        {
            SpellId.InfuseStamina1,
            SpellId.InfuseStamina2,
            SpellId.InfuseStamina3,
            SpellId.InfuseStamina4,
            SpellId.InfuseStamina5,
            SpellId.InfuseStamina6,
            SpellId.InfuseStamina7,
            SpellId.InfuseStamina8,
        };

        public static readonly List<SpellId> DrainStamina = new List<SpellId>()
        {
            SpellId.DrainStamina1,
            SpellId.DrainStamina2,
            SpellId.DrainStamina3,
            SpellId.DrainStamina4,
            SpellId.DrainStamina5,
            SpellId.DrainStamina6,
            SpellId.DrainStamina7,
            SpellId.DrainStamina8,
        };

        public static readonly List<SpellId> DrainMana = new List<SpellId>()
        {
            SpellId.DrainMana1,
            SpellId.DrainMana2,
            SpellId.DrainMana3,
            SpellId.DrainMana4,
            SpellId.DrainMana5,
            SpellId.DrainMana6,
            SpellId.DrainMana7,
            SpellId.DrainMana8,
        };

        public static readonly List<SpellId> HealthToStaminaOther = new List<SpellId>()
        {
            SpellId.HealthToStaminaOther1,
            SpellId.HealthToStaminaOther2,
            SpellId.HealthToStaminaOther3,
            SpellId.HealthToStaminaOther4,
            SpellId.HealthToStaminaOther5,
            SpellId.HealthToStaminaOther6,
            SpellId.HealthToStaminaOther7,
            SpellId.HealthToStaminaOther8,
        };

        public static readonly List<SpellId> HealthToStaminaSelf = new List<SpellId>()
        {
            SpellId.HealthToStaminaSelf1,
            SpellId.HealthToStaminaSelf2,
            SpellId.HealthToStaminaSelf3,
            SpellId.HealthToStaminaSelf4,
            SpellId.HealthToStaminaSelf5,
            SpellId.HealthToStaminaSelf6,
            SpellId.HealthToStaminaSelf7,
            SpellId.HealthToStaminaSelf8,
        };

        public static readonly List<SpellId> HealthToManaSelf = new List<SpellId>()
        {
            SpellId.HealthToManaSelf1,
            SpellId.HealthToManaSelf2,
            SpellId.HealthToManaSelf3,
            SpellId.HealthToManaSelf4,
            SpellId.HealthToManaSelf5,
            SpellId.HealthToManaSelf6,
            SpellId.HealthToManaSelf7,
            SpellId.HealthToManaSelf8,
        };

        public static readonly List<SpellId> HealthToManaOther = new List<SpellId>()
        {
            SpellId.HealthToManaOther1,
            SpellId.HealthToManaOther2,
            SpellId.HealthToManaOther3,
            SpellId.HealthToManaOther4,
            SpellId.HealthToManaOther5,
            SpellId.HealthToManaOther6,
            SpellId.HealthToManaOther7,
            SpellId.HealthToManaOther8,
        };

        public static readonly List<SpellId> ManaToHealthOther = new List<SpellId>()
        {
            SpellId.ManaToHealthOther1,
            SpellId.ManaToHealthOther2,
            SpellId.ManaToHealthOther3,
            SpellId.ManaToHealthOther4,
            SpellId.ManaToHealthOther5,
            SpellId.ManaToHealthOther6,
            SpellId.ManaToHealthOther7,
            SpellId.ManaToHealthOther8,
        };

        public static readonly List<SpellId> ManaToHealthSelf = new List<SpellId>()
        {
            SpellId.ManaToHealthSelf1,
            SpellId.ManaToHealthSelf2,
            SpellId.ManaToHealthSelf3,
            SpellId.ManaToHealthSelf4,
            SpellId.ManaToHealthSelf5,
            SpellId.ManaToHealthSelf6,
            SpellId.ManaToHealthSelf7,
            SpellId.ManaToHealthSelf8,
        };

        public static readonly List<SpellId> ManaToStaminaSelf = new List<SpellId>()
        {
            SpellId.ManaToStaminaSelf1,
            SpellId.ManaToStaminaSelf2,
            SpellId.ManaToStaminaSelf3,
            SpellId.ManaToStaminaSelf4,
            SpellId.ManaToStaminaSelf5,
            SpellId.ManaToStaminaSelf6,
            SpellId.ManaToStaminaSelf7,
            SpellId.ManaToStaminaSelf8,
        };

        public static readonly List<SpellId> ManaToStaminaOther = new List<SpellId>()
        {
            SpellId.ManaToStaminaOther1,
            SpellId.ManaToStaminaOther2,
            SpellId.ManaToStaminaOther3,
            SpellId.ManaToStaminaOther4,
            SpellId.ManaToStaminaOther5,
            SpellId.ManaToStaminaOther6,
            SpellId.ManaToStaminaOther7,
            SpellId.ManaToStaminaOther8,
        };

        public static readonly List<SpellId> EnduranceSelf = new List<SpellId>()
        {
            SpellId.EnduranceSelf1,
            SpellId.EnduranceSelf2,
            SpellId.EnduranceSelf3,
            SpellId.EnduranceSelf4,
            SpellId.EnduranceSelf5,
            SpellId.EnduranceSelf6,
            SpellId.EnduranceSelf7,
            SpellId.EnduranceSelf8,
        };

        public static readonly List<SpellId> EnduranceOther = new List<SpellId>()
        {
            SpellId.EnduranceOther1,
            SpellId.EnduranceOther2,
            SpellId.EnduranceOther3,
            SpellId.EnduranceOther4,
            SpellId.EnduranceOther5,
            SpellId.EnduranceOther6,
            SpellId.EnduranceOther7,
            SpellId.EnduranceOther8,
        };

        public static readonly List<SpellId> FrailtySelf = new List<SpellId>()
        {
            SpellId.FrailtySelf1,
            SpellId.FrailtySelf2,
            SpellId.FrailtySelf3,
            SpellId.FrailtySelf4,
            SpellId.FrailtySelf5,
            SpellId.FrailtySelf6,
            SpellId.FrailtySelf7,
            SpellId.FrailtySelf8,
        };

        public static readonly List<SpellId> FrailtyOther = new List<SpellId>()
        {
            SpellId.FrailtyOther1,
            SpellId.FrailtyOther2,
            SpellId.FrailtyOther3,
            SpellId.FrailtyOther4,
            SpellId.FrailtyOther5,
            SpellId.FrailtyOther6,
            SpellId.FrailtyOther7,
            SpellId.FrailtyOther8,
        };

        public static readonly List<SpellId> CoordinationSelf = new List<SpellId>()
        {
            SpellId.CoordinationSelf1,
            SpellId.CoordinationSelf2,
            SpellId.CoordinationSelf3,
            SpellId.CoordinationSelf4,
            SpellId.CoordinationSelf5,
            SpellId.CoordinationSelf6,
            SpellId.CoordinationSelf7,
            SpellId.CoordinationSelf8,
        };

        public static readonly List<SpellId> CoordinationOther = new List<SpellId>()
        {
            SpellId.CoordinationOther1,
            SpellId.CoordinationOther2,
            SpellId.CoordinationOther3,
            SpellId.CoordinationOther4,
            SpellId.CoordinationOther5,
            SpellId.CoordinationOther6,
            SpellId.CoordinationOther7,
            SpellId.CoordinationOther8,
        };

        public static readonly List<SpellId> ClumsinessSelf = new List<SpellId>()
        {
            SpellId.ClumsinessSelf1,
            SpellId.ClumsinessSelf2,
            SpellId.ClumsinessSelf3,
            SpellId.ClumsinessSelf4,
            SpellId.ClumsinessSelf5,
            SpellId.ClumsinessSelf6,
            SpellId.ClumsinessSelf7,
            SpellId.ClumsinessSelf8,
        };

        public static readonly List<SpellId> ClumsinessOther = new List<SpellId>()
        {
            SpellId.ClumsinessOther1,
            SpellId.ClumsinessOther2,
            SpellId.ClumsinessOther3,
            SpellId.ClumsinessOther4,
            SpellId.ClumsinessOther5,
            SpellId.ClumsinessOther6,
            SpellId.ClumsinessOther7,
            SpellId.ClumsinessOther8,
        };

        public static readonly List<SpellId> QuicknessSelf = new List<SpellId>()
        {
            SpellId.QuicknessSelf1,
            SpellId.QuicknessSelf2,
            SpellId.QuicknessSelf3,
            SpellId.QuicknessSelf4,
            SpellId.QuicknessSelf5,
            SpellId.QuicknessSelf6,
            SpellId.QuicknessSelf7,
            SpellId.QuicknessSelf8,
        };

        public static readonly List<SpellId> QuicknessOther = new List<SpellId>()
        {
            SpellId.QuicknessOther1,
            SpellId.QuicknessOther2,
            SpellId.QuicknessOther3,
            SpellId.QuicknessOther4,
            SpellId.QuicknessOther5,
            SpellId.QuicknessOther6,
            SpellId.QuicknessOther7,
            SpellId.QuicknessOther8,
        };

        public static readonly List<SpellId> SlownessSelf = new List<SpellId>()
        {
            SpellId.SlownessSelf1,
            SpellId.SlownessSelf2,
            SpellId.SlownessSelf3,
            SpellId.SlownessSelf4,
            SpellId.SlownessSelf5,
            SpellId.SlownessSelf6,
            SpellId.SlownessSelf7,
            SpellId.SlownessSelf8,
        };

        public static readonly List<SpellId> SlownessOther = new List<SpellId>()
        {
            SpellId.SlownessOther1,
            SpellId.SlownessOther2,
            SpellId.SlownessOther3,
            SpellId.SlownessOther4,
            SpellId.SlownessOther5,
            SpellId.SlownessOther6,
            SpellId.SlownessOther7,
            SpellId.SlownessOther8,
        };

        public static readonly List<SpellId> FocusSelf = new List<SpellId>()
        {
            SpellId.FocusSelf1,
            SpellId.FocusSelf2,
            SpellId.FocusSelf3,
            SpellId.FocusSelf4,
            SpellId.FocusSelf5,
            SpellId.FocusSelf6,
            SpellId.FocusSelf7,
            SpellId.FocusSelf8,
        };

        public static readonly List<SpellId> FocusOther = new List<SpellId>()
        {
            SpellId.FocusOther1,
            SpellId.FocusOther2,
            SpellId.FocusOther3,
            SpellId.FocusOther4,
            SpellId.FocusOther5,
            SpellId.FocusOther6,
            SpellId.FocusOther7,
            SpellId.FocusOther8,
        };

        public static readonly List<SpellId> BafflementSelf = new List<SpellId>()
        {
            SpellId.BafflementSelf1,
            SpellId.BafflementSelf2,
            SpellId.BafflementSelf3,
            SpellId.BafflementSelf4,
            SpellId.BafflementSelf5,
            SpellId.BafflementSelf6,
            SpellId.BafflementSelf7,
            SpellId.BafflementSelf8,
        };

        public static readonly List<SpellId> BafflementOther = new List<SpellId>()
        {
            SpellId.BafflementOther1,
            SpellId.BafflementOther2,
            SpellId.BafflementOther3,
            SpellId.BafflementOther4,
            SpellId.BafflementOther5,
            SpellId.BafflementOther6,
            SpellId.BafflementOther7,
            SpellId.BafflementOther8,
        };

        public static readonly List<SpellId> WillpowerSelf = new List<SpellId>()
        {
            SpellId.WillpowerSelf1,
            SpellId.WillpowerSelf2,
            SpellId.WillpowerSelf3,
            SpellId.WillpowerSelf4,
            SpellId.WillpowerSelf5,
            SpellId.WillpowerSelf6,
            SpellId.WillpowerSelf7,
            SpellId.WillpowerSelf8,
        };

        public static readonly List<SpellId> WillpowerOther = new List<SpellId>()
        {
            SpellId.WillpowerOther1,
            SpellId.WillpowerOther2,
            SpellId.WillpowerOther3,
            SpellId.WillpowerOther4,
            SpellId.WillpowerOther5,
            SpellId.WillpowerOther6,
            SpellId.WillpowerOther7,
            SpellId.WillpowerOther8,
        };

        public static readonly List<SpellId> FeeblemindSelf = new List<SpellId>()
        {
            SpellId.FeeblemindSelf1,
            SpellId.FeeblemindSelf2,
            SpellId.FeeblemindSelf3,
            SpellId.FeeblemindSelf4,
            SpellId.FeeblemindSelf5,
            SpellId.FeeblemindSelf6,
            SpellId.FeeblemindSelf7,
            SpellId.FeeblemindSelf8,
        };

        public static readonly List<SpellId> FeeblemindOther = new List<SpellId>()
        {
            SpellId.FeeblemindOther1,
            SpellId.FeeblemindOther2,
            SpellId.FeeblemindOther3,
            SpellId.FeeblemindOther4,
            SpellId.FeeblemindOther5,
            SpellId.FeeblemindOther6,
            SpellId.FeeblemindOther7,
            SpellId.FeeblemindOther8,
        };

        public static readonly List<SpellId> HermeticVoid = new List<SpellId>()
        {
            SpellId.HermeticVoid1,
            SpellId.HermeticVoid2,
            SpellId.HermeticVoid3,
            SpellId.HermeticVoid4,
            SpellId.HermeticVoid5,
            SpellId.HermeticVoid6,
            SpellId.HermeticVoid7,
            SpellId.HermeticVoid8,
        };

        public static readonly List<SpellId> HermeticLinkSelf = new List<SpellId>()
        {
            SpellId.HermeticLinkSelf1,
            SpellId.HermeticLinkSelf2,
            SpellId.HermeticLinkSelf3,
            SpellId.HermeticLinkSelf4,
            SpellId.HermeticLinkSelf5,
            SpellId.HermeticLinkSelf6,
            SpellId.HermeticLinkSelf7,
            SpellId.HermeticLinkSelf8,
        };

        public static readonly List<SpellId> Brittlemail = new List<SpellId>()
        {
            SpellId.Brittlemail1,
            SpellId.Brittlemail2,
            SpellId.Brittlemail3,
            SpellId.Brittlemail4,
            SpellId.Brittlemail5,
            SpellId.Brittlemail6,
            SpellId.Brittlemail7,
            SpellId.Brittlemail8,
        };

        public static readonly List<SpellId> AcidBane = new List<SpellId>()
        {
            SpellId.AcidBane1,
            SpellId.AcidBane2,
            SpellId.AcidBane3,
            SpellId.AcidBane4,
            SpellId.AcidBane5,
            SpellId.AcidBane6,
            SpellId.AcidBane7,
            SpellId.AcidBane8,
        };

        public static readonly List<SpellId> AcidLure = new List<SpellId>()
        {
            SpellId.AcidLure1,
            SpellId.AcidLure2,
            SpellId.AcidLure3,
            SpellId.AcidLure4,
            SpellId.AcidLure5,
            SpellId.AcidLure6,
            SpellId.AcidLure7,
            SpellId.AcidLure8,
        };

        public static readonly List<SpellId> BludgeonLure = new List<SpellId>()
        {
            SpellId.BludgeonLure1,
            SpellId.BludgeonLure2,
            SpellId.BludgeonLure3,
            SpellId.BludgeonLure4,
            SpellId.BludgeonLure5,
            SpellId.BludgeonLure6,
            SpellId.BludgeonLure7,
            SpellId.BludgeonLure8,
        };

        public static readonly List<SpellId> BludgeonBane = new List<SpellId>()
        {
            SpellId.BludgeonBane1,
            SpellId.BludgeonBane2,
            SpellId.BludgeonBane3,
            SpellId.BludgeonBane4,
            SpellId.BludgeonBane5,
            SpellId.BludgeonBane6,
            SpellId.BludgeonBane7,
            SpellId.BludgeonBane8,
        };

        public static readonly List<SpellId> FrostLure = new List<SpellId>()
        {
            SpellId.FrostLure1,
            SpellId.FrostLure2,
            SpellId.FrostLure3,
            SpellId.FrostLure4,
            SpellId.FrostLure5,
            SpellId.FrostLure6,
            SpellId.FrostLure7,
            SpellId.FrostLure8,
        };

        public static readonly List<SpellId> FrostBane = new List<SpellId>()
        {
            SpellId.FrostBane1,
            SpellId.FrostBane2,
            SpellId.FrostBane3,
            SpellId.FrostBane4,
            SpellId.FrostBane5,
            SpellId.FrostBane6,
            SpellId.FrostBane7,
            SpellId.FrostBane8,
        };

        public static readonly List<SpellId> LightningLure = new List<SpellId>()
        {
            SpellId.LightningLure1,
            SpellId.LightningLure2,
            SpellId.LightningLure3,
            SpellId.LightningLure4,
            SpellId.LightningLure5,
            SpellId.LightningLure6,
            SpellId.LightningLure7,
            SpellId.LightningLure8,
        };

        public static readonly List<SpellId> LightningBane = new List<SpellId>()
        {
            SpellId.LightningBane1,
            SpellId.LightningBane2,
            SpellId.LightningBane3,
            SpellId.LightningBane4,
            SpellId.LightningBane5,
            SpellId.LightningBane6,
            SpellId.LightningBane7,
            SpellId.LightningBane8,
        };

        public static readonly List<SpellId> FlameLure = new List<SpellId>()
        {
            SpellId.FlameLure1,
            SpellId.FlameLure2,
            SpellId.FlameLure3,
            SpellId.FlameLure4,
            SpellId.FlameLure5,
            SpellId.FlameLure6,
            SpellId.FlameLure7,
            SpellId.FlameLure8,
        };

        public static readonly List<SpellId> FlameBane = new List<SpellId>()
        {
            SpellId.FlameBane1,
            SpellId.FlameBane2,
            SpellId.FlameBane3,
            SpellId.FlameBane4,
            SpellId.FlameBane5,
            SpellId.FlameBane6,
            SpellId.FlameBane7,
            SpellId.FlameBane8,
        };

        public static readonly List<SpellId> PiercingLure = new List<SpellId>()
        {
            SpellId.PiercingLure1,
            SpellId.PiercingLure2,
            SpellId.PiercingLure3,
            SpellId.PiercingLure4,
            SpellId.PiercingLure5,
            SpellId.PiercingLure6,
            SpellId.PiercingLure7,
            SpellId.PiercingLure8,
        };

        public static readonly List<SpellId> PiercingBane = new List<SpellId>()
        {
            SpellId.PiercingBane1,
            SpellId.PiercingBane2,
            SpellId.PiercingBane3,
            SpellId.PiercingBane4,
            SpellId.PiercingBane5,
            SpellId.PiercingBane6,
            SpellId.PiercingBane7,
            SpellId.PiercingBane8,
        };

        public static readonly List<SpellId> StrengthenLock = new List<SpellId>()
        {
            SpellId.StrengthenLock1,
            SpellId.StrengthenLock2,
            SpellId.StrengthenLock3,
            SpellId.StrengthenLock4,
            SpellId.StrengthenLock5,
            SpellId.StrengthenLock6,
            SpellId.StrengthenLock7,
            SpellId.StrengthenLock8,
        };

        public static readonly List<SpellId> WeakenLock = new List<SpellId>()
        {
            SpellId.WeakenLock1,
            SpellId.WeakenLock2,
            SpellId.WeakenLock3,
            SpellId.WeakenLock4,
            SpellId.WeakenLock5,
            SpellId.WeakenLock6,
            SpellId.WeakenLock7,
            SpellId.WeakenLock8,
        };

        public static readonly List<SpellId> HeartSeekerSelf = new List<SpellId>()
        {
            SpellId.HeartSeekerSelf1,
            SpellId.HeartSeekerSelf2,
            SpellId.HeartSeekerSelf3,
            SpellId.HeartSeekerSelf4,
            SpellId.HeartSeekerSelf5,
            SpellId.HeartSeekerSelf6,
            SpellId.HeartSeekerSelf7,
            SpellId.HeartSeekerSelf8,
        };

        public static readonly List<SpellId> TurnBlade = new List<SpellId>()
        {
            SpellId.TurnBlade1,
            SpellId.TurnBlade2,
            SpellId.TurnBlade3,
            SpellId.TurnBlade4,
            SpellId.TurnBlade5,
            SpellId.TurnBlade6,
            SpellId.TurnBlade7,
            SpellId.TurnBlade8,
        };

        public static readonly List<SpellId> DefenderSelf = new List<SpellId>()
        {
            SpellId.DefenderSelf1,
            SpellId.DefenderSelf2,
            SpellId.DefenderSelf3,
            SpellId.DefenderSelf4,
            SpellId.DefenderSelf5,
            SpellId.DefenderSelf6,
            SpellId.DefenderSelf7,
            SpellId.DefenderSelf8,
        };

        public static readonly List<SpellId> LureBlade = new List<SpellId>()
        {
            SpellId.LureBlade1,
            SpellId.LureBlade2,
            SpellId.LureBlade3,
            SpellId.LureBlade4,
            SpellId.LureBlade5,
            SpellId.LureBlade6,
            SpellId.LureBlade7,
            SpellId.LureBlade8,
        };

        public static readonly List<SpellId> DefenselessnessSelf = new List<SpellId>()
        {
            SpellId.DefenselessnessSelf1,
            SpellId.DefenselessnessSelf2,
            SpellId.DefenselessnessSelf3,
            SpellId.DefenselessnessSelf4,
            SpellId.DefenselessnessSelf5,
            SpellId.DefenselessnessSelf6,
            SpellId.DefenselessnessSelf7,
            SpellId.DefenselessnessSelf8,
        };

        public static readonly List<SpellId> StaminaToHealthOther = new List<SpellId>()
        {
            SpellId.StaminaToHealthOther1,
            SpellId.StaminaToHealthOther2,
            SpellId.StaminaToHealthOther3,
            SpellId.StaminaToHealthOther4,
            SpellId.StaminaToHealthOther5,
            SpellId.StaminaToHealthOther6,
            SpellId.StaminaToHealthOther7,
            SpellId.StaminaToHealthOther8,
        };

        public static readonly List<SpellId> StaminaToHealthSelf = new List<SpellId>()
        {
            SpellId.StaminaToHealthSelf1,
            SpellId.StaminaToHealthSelf2,
            SpellId.StaminaToHealthSelf3,
            SpellId.StaminaToHealthSelf4,
            SpellId.StaminaToHealthSelf5,
            SpellId.StaminaToHealthSelf6,
            SpellId.StaminaToHealthSelf7,
            SpellId.StaminaToHealthSelf8,
        };

        public static readonly List<SpellId> StaminaToManaOther = new List<SpellId>()
        {
            SpellId.StaminaToManaOther1,
            SpellId.StaminaToManaOther2,
            SpellId.StaminaToManaOther3,
            SpellId.StaminaToManaOther4,
            SpellId.StaminaToManaOther5,
            SpellId.StaminaToManaOther6,
            SpellId.StaminaToManaOther7,
            SpellId.StaminaToManaOther8,
        };

        public static readonly List<SpellId> StaminaToManaSelf = new List<SpellId>()
        {
            SpellId.StaminaToManaSelf1,
            SpellId.StaminaToManaSelf2,
            SpellId.StaminaToManaSelf3,
            SpellId.StaminaToManaSelf4,
            SpellId.StaminaToManaSelf5,
            SpellId.StaminaToManaSelf6,
            SpellId.StaminaToManaSelf7,
            SpellId.StaminaToManaSelf8,
        };

        public static readonly List<SpellId> CookingMasteryOther = new List<SpellId>()
        {
            SpellId.CookingMasteryOther1,
            SpellId.CookingMasteryOther2,
            SpellId.CookingMasteryOther3,
            SpellId.CookingMasteryOther4,
            SpellId.CookingMasteryOther5,
            SpellId.CookingMasteryOther6,
            SpellId.CookingMasteryOther7,
            SpellId.CookingMasteryOther8,
        };

        public static readonly List<SpellId> CookingMasterySelf = new List<SpellId>()
        {
            SpellId.CookingMasterySelf1,
            SpellId.CookingMasterySelf2,
            SpellId.CookingMasterySelf3,
            SpellId.CookingMasterySelf4,
            SpellId.CookingMasterySelf5,
            SpellId.CookingMasterySelf6,
            SpellId.CookingMasterySelf7,
            SpellId.CookingMasterySelf8,
        };

        public static readonly List<SpellId> CookingIneptitudeOther = new List<SpellId>()
        {
            SpellId.CookingIneptitudeOther1,
            SpellId.CookingIneptitudeOther2,
            SpellId.CookingIneptitudeOther3,
            SpellId.CookingIneptitudeOther4,
            SpellId.CookingIneptitudeOther5,
            SpellId.CookingIneptitudeOther6,
            SpellId.CookingIneptitudeOther7,
            SpellId.CookingIneptitudeOther8,
        };

        public static readonly List<SpellId> CookingIneptitudeSelf = new List<SpellId>()
        {
            SpellId.CookingIneptitudeSelf1,
            SpellId.CookingIneptitudeSelf2,
            SpellId.CookingIneptitudeSelf3,
            SpellId.CookingIneptitudeSelf4,
            SpellId.CookingIneptitudeSelf5,
            SpellId.CookingIneptitudeSelf6,
            SpellId.CookingIneptitudeSelf7,
            SpellId.CookingIneptitudeSelf8,
        };

        public static readonly List<SpellId> FletchingMasteryOther = new List<SpellId>()
        {
            SpellId.FletchingMasteryOther1,
            SpellId.FletchingMasteryOther2,
            SpellId.FletchingMasteryOther3,
            SpellId.FletchingMasteryOther4,
            SpellId.FletchingMasteryOther5,
            SpellId.FletchingMasteryOther6,
            SpellId.FletchingMasteryOther7,
            SpellId.FletchingMasteryOther8,
        };

        public static readonly List<SpellId> FletchingMasterySelf = new List<SpellId>()
        {
            SpellId.FletchingMasterySelf1,
            SpellId.FletchingMasterySelf2,
            SpellId.FletchingMasterySelf3,
            SpellId.FletchingMasterySelf4,
            SpellId.FletchingMasterySelf5,
            SpellId.FletchingMasterySelf6,
            SpellId.FletchingMasterySelf7,
            SpellId.FletchingMasterySelf8,
        };

        public static readonly List<SpellId> FletchingIneptitudeOther = new List<SpellId>()
        {
            SpellId.FletchingIneptitudeOther1,
            SpellId.FletchingIneptitudeOther2,
            SpellId.FletchingIneptitudeOther3,
            SpellId.FletchingIneptitudeOther4,
            SpellId.FletchingIneptitudeOther5,
            SpellId.FletchingIneptitudeOther6,
            SpellId.FletchingIneptitudeOther7,
            SpellId.FletchingIneptitudeOther8,
        };

        public static readonly List<SpellId> FletchingIneptitudeSelf = new List<SpellId>()
        {
            SpellId.FletchingIneptitudeSelf1,
            SpellId.FletchingIneptitudeSelf2,
            SpellId.FletchingIneptitudeSelf3,
            SpellId.FletchingIneptitudeSelf4,
            SpellId.FletchingIneptitudeSelf5,
            SpellId.FletchingIneptitudeSelf6,
            SpellId.FletchingIneptitudeSelf7,
            SpellId.FletchingIneptitudeSelf8,
        };

        public static readonly List<SpellId> AlchemyMasteryOther = new List<SpellId>()
        {
            SpellId.AlchemyMasteryOther1,
            SpellId.AlchemyMasteryOther2,
            SpellId.AlchemyMasteryOther3,
            SpellId.AlchemyMasteryOther4,
            SpellId.AlchemyMasteryOther5,
            SpellId.AlchemyMasteryOther6,
            SpellId.AlchemyMasteryOther7,
            SpellId.AlchemyMasteryOther8,
        };

        public static readonly List<SpellId> AlchemyMasterySelf = new List<SpellId>()
        {
            SpellId.AlchemyMasterySelf1,
            SpellId.AlchemyMasterySelf2,
            SpellId.AlchemyMasterySelf3,
            SpellId.AlchemyMasterySelf4,
            SpellId.AlchemyMasterySelf5,
            SpellId.AlchemyMasterySelf6,
            SpellId.AlchemyMasterySelf7,
            SpellId.AlchemyMasterySelf8,
        };

        public static readonly List<SpellId> AlchemyIneptitudeOther = new List<SpellId>()
        {
            SpellId.AlchemyIneptitudeOther1,
            SpellId.AlchemyIneptitudeOther2,
            SpellId.AlchemyIneptitudeOther3,
            SpellId.AlchemyIneptitudeOther4,
            SpellId.AlchemyIneptitudeOther5,
            SpellId.AlchemyIneptitudeOther6,
            SpellId.AlchemyIneptitudeOther7,
            SpellId.AlchemyIneptitudeOther8,
        };

        public static readonly List<SpellId> AlchemyIneptitudeSelf = new List<SpellId>()
        {
            SpellId.AlchemyIneptitudeSelf1,
            SpellId.AlchemyIneptitudeSelf2,
            SpellId.AlchemyIneptitudeSelf3,
            SpellId.AlchemyIneptitudeSelf4,
            SpellId.AlchemyIneptitudeSelf5,
            SpellId.AlchemyIneptitudeSelf6,
            SpellId.AlchemyIneptitudeSelf7,
            SpellId.AlchemyIneptitudeSelf8,
        };

        public static readonly List<SpellId> AcidStreak = new List<SpellId>()
        {
            SpellId.AcidStreak1,
            SpellId.AcidStreak2,
            SpellId.AcidStreak3,
            SpellId.AcidStreak4,
            SpellId.AcidStreak5,
            SpellId.AcidStreak6,
            SpellId.AcidStreak7,
            SpellId.AcidStreak8,
        };

        public static readonly List<SpellId> FlameStreak = new List<SpellId>()
        {
            SpellId.FlameStreak1,
            SpellId.FlameStreak2,
            SpellId.FlameStreak3,
            SpellId.FlameStreak4,
            SpellId.FlameStreak5,
            SpellId.FlameStreak6,
            SpellId.FlameStreak7,
            SpellId.FlameStreak8,
        };

        public static readonly List<SpellId> ForceStreak = new List<SpellId>()
        {
            SpellId.ForceStreak1,
            SpellId.ForceStreak2,
            SpellId.ForceStreak3,
            SpellId.ForceStreak4,
            SpellId.ForceStreak5,
            SpellId.ForceStreak6,
            SpellId.ForceStreak7,
            SpellId.ForceStreak8,
        };

        public static readonly List<SpellId> FrostStreak = new List<SpellId>()
        {
            SpellId.FrostStreak1,
            SpellId.FrostStreak2,
            SpellId.FrostStreak3,
            SpellId.FrostStreak4,
            SpellId.FrostStreak5,
            SpellId.FrostStreak6,
            SpellId.FrostStreak7,
            SpellId.FrostStreak8,
        };

        public static readonly List<SpellId> LightningStreak = new List<SpellId>()
        {
            SpellId.LightningStreak1,
            SpellId.LightningStreak2,
            SpellId.LightningStreak3,
            SpellId.LightningStreak4,
            SpellId.LightningStreak5,
            SpellId.LightningStreak6,
            SpellId.LightningStreak7,
            SpellId.LightningStreak8,
        };

        public static readonly List<SpellId> ShockwaveStreak = new List<SpellId>()
        {
            SpellId.ShockwaveStreak1,
            SpellId.ShockwaveStreak2,
            SpellId.ShockwaveStreak3,
            SpellId.ShockwaveStreak4,
            SpellId.ShockwaveStreak5,
            SpellId.ShockwaveStreak6,
            SpellId.ShockwaveStreak7,
            SpellId.ShockwaveStreak8,
        };

        public static readonly List<SpellId> WhirlingBladeStreak = new List<SpellId>()
        {
            SpellId.WhirlingBladeStreak1,
            SpellId.WhirlingBladeStreak2,
            SpellId.WhirlingBladeStreak3,
            SpellId.WhirlingBladeStreak4,
            SpellId.WhirlingBladeStreak5,
            SpellId.WhirlingBladeStreak6,
            SpellId.WhirlingBladeStreak7,
            SpellId.WhirlingBladeStreak8,
        };

        public static readonly List<SpellId> DispelAllNeutralOther = new List<SpellId>()
        {
            SpellId.DispelAllNeutralOther1,
            SpellId.DispelAllNeutralOther2,
            SpellId.DispelAllNeutralOther3,
            SpellId.DispelAllNeutralOther4,
            SpellId.DispelAllNeutralOther5,
            SpellId.DispelAllNeutralOther6,
            SpellId.DispelAllNeutralOther7,
            SpellId.DispelAllNeutralOther8,
        };

        public static readonly List<SpellId> DispelAllGoodOther = new List<SpellId>()
        {
            SpellId.DispelAllGoodOther1,
            SpellId.DispelAllGoodOther2,
            SpellId.DispelAllGoodOther3,
            SpellId.DispelAllGoodOther4,
            SpellId.DispelAllGoodOther5,
            SpellId.DispelAllGoodOther6,
            SpellId.DispelAllGoodOther7,
            SpellId.DispelAllGoodOther8,
        };

        public static readonly List<SpellId> DispelAllBadOther = new List<SpellId>()
        {
            SpellId.DispelAllBadOther1,
            SpellId.DispelAllBadOther2,
            SpellId.DispelAllBadOther3,
            SpellId.DispelAllBadOther4,
            SpellId.DispelAllBadOther5,
            SpellId.DispelAllBadOther6,
            SpellId.DispelAllBadOther7,
            SpellId.DispelAllBadOther8,
        };

        public static readonly List<SpellId> DispelAllNeutralSelf = new List<SpellId>()
        {
            SpellId.DispelAllNeutralSelf1,
            SpellId.DispelAllNeutralSelf2,
            SpellId.DispelAllNeutralSelf3,
            SpellId.DispelAllNeutralSelf4,
            SpellId.DispelAllNeutralSelf5,
            SpellId.DispelAllNeutralSelf6,
            SpellId.DispelAllNeutralSelf7,
            SpellId.DispelAllNeutralSelf8,
        };

        public static readonly List<SpellId> DispelAllGoodSelf = new List<SpellId>()
        {
            SpellId.DispelAllGoodSelf1,
            SpellId.DispelAllGoodSelf2,
            SpellId.DispelAllGoodSelf3,
            SpellId.DispelAllGoodSelf4,
            SpellId.DispelAllGoodSelf5,
            SpellId.DispelAllGoodSelf6,
            SpellId.DispelAllGoodSelf7,
            SpellId.DispelAllGoodSelf8,
        };

        public static readonly List<SpellId> DispelAllBadSelf = new List<SpellId>()
        {
            SpellId.DispelAllBadSelf1,
            SpellId.DispelAllBadSelf2,
            SpellId.DispelAllBadSelf3,
            SpellId.DispelAllBadSelf4,
            SpellId.DispelAllBadSelf5,
            SpellId.DispelAllBadSelf6,
            SpellId.DispelAllBadSelf7,
            SpellId.DispelAllBadSelf8,
        };

        public static readonly List<SpellId> DispelCreatureNeutralOther = new List<SpellId>()
        {
            SpellId.DispelCreatureNeutralOther1,
            SpellId.DispelCreatureNeutralOther2,
            SpellId.DispelCreatureNeutralOther3,
            SpellId.DispelCreatureNeutralOther4,
            SpellId.DispelCreatureNeutralOther5,
            SpellId.DispelCreatureNeutralOther6,
            SpellId.DispelCreatureNeutralOther7,
            SpellId.DispelCreatureNeutralOther8,
        };

        public static readonly List<SpellId> DispelCreatureGoodOther = new List<SpellId>()
        {
            SpellId.DispelCreatureGoodOther1,
            SpellId.DispelCreatureGoodOther2,
            SpellId.DispelCreatureGoodOther3,
            SpellId.DispelCreatureGoodOther4,
            SpellId.DispelCreatureGoodOther5,
            SpellId.DispelCreatureGoodOther6,
            SpellId.DispelCreatureGoodOther7,
            SpellId.DispelCreatureGoodOther8,
        };

        public static readonly List<SpellId> DispelCreatureBadOther = new List<SpellId>()
        {
            SpellId.DispelCreatureBadOther1,
            SpellId.DispelCreatureBadOther2,
            SpellId.DispelCreatureBadOther3,
            SpellId.DispelCreatureBadOther4,
            SpellId.DispelCreatureBadOther5,
            SpellId.DispelCreatureBadOther6,
            SpellId.DispelCreatureBadOther7,
            SpellId.DispelCreatureBadOther8,
        };

        public static readonly List<SpellId> DispelCreatureNeutralSelf = new List<SpellId>()
        {
            SpellId.DispelCreatureNeutralSelf1,
            SpellId.DispelCreatureNeutralSelf2,
            SpellId.DispelCreatureNeutralSelf3,
            SpellId.DispelCreatureNeutralSelf4,
            SpellId.DispelCreatureNeutralSelf5,
            SpellId.DispelCreatureNeutralSelf6,
            SpellId.DispelCreatureNeutralSelf7,
            SpellId.DispelCreatureNeutralSelf8,
        };

        public static readonly List<SpellId> DispelCreatureGoodSelf = new List<SpellId>()
        {
            SpellId.DispelCreatureGoodSelf1,
            SpellId.DispelCreatureGoodSelf2,
            SpellId.DispelCreatureGoodSelf3,
            SpellId.DispelCreatureGoodSelf4,
            SpellId.DispelCreatureGoodSelf5,
            SpellId.DispelCreatureGoodSelf6,
            SpellId.DispelCreatureGoodSelf7,
            SpellId.DispelCreatureGoodSelf8,
        };

        public static readonly List<SpellId> DispelCreatureBadSelf = new List<SpellId>()
        {
            SpellId.DispelCreatureBadSelf1,
            SpellId.DispelCreatureBadSelf2,
            SpellId.DispelCreatureBadSelf3,
            SpellId.DispelCreatureBadSelf4,
            SpellId.DispelCreatureBadSelf5,
            SpellId.DispelCreatureBadSelf6,
            SpellId.DispelCreatureBadSelf7,
            SpellId.DispelCreatureBadSelf8,
        };

        public static readonly List<SpellId> DispelItemNeutralOther = new List<SpellId>()
        {
            SpellId.DispelItemNeutralOther1,
            SpellId.DispelItemNeutralOther2,
            SpellId.DispelItemNeutralOther3,
            SpellId.DispelItemNeutralOther4,
            SpellId.DispelItemNeutralOther5,
            SpellId.DispelItemNeutralOther6,
            SpellId.DispelItemNeutralOther7,
            SpellId.DispelItemNeutralOther8,
        };

        public static readonly List<SpellId> DispelItemGoodOther = new List<SpellId>()
        {
            SpellId.DispelItemGoodOther1,
            SpellId.DispelItemGoodOther2,
            SpellId.DispelItemGoodOther3,
            SpellId.DispelItemGoodOther4,
            SpellId.DispelItemGoodOther5,
            SpellId.DispelItemGoodOther6,
            SpellId.DispelItemGoodOther7,
            SpellId.DispelItemGoodOther8,
        };

        public static readonly List<SpellId> DispelItemBadOther = new List<SpellId>()
        {
            SpellId.DispelItemBadOther1,
            SpellId.DispelItemBadOther2,
            SpellId.DispelItemBadOther3,
            SpellId.DispelItemBadOther4,
            SpellId.DispelItemBadOther5,
            SpellId.DispelItemBadOther6,
            SpellId.DispelItemBadOther7,
            SpellId.DispelItemBadOther8,
        };

        public static readonly List<SpellId> DispelItemNeutralSelf = new List<SpellId>()
        {
            SpellId.DispelItemNeutralSelf1,
            SpellId.DispelItemNeutralSelf2,
            SpellId.DispelItemNeutralSelf3,
            SpellId.DispelItemNeutralSelf4,
            SpellId.DispelItemNeutralSelf5,
            SpellId.DispelItemNeutralSelf6,
        };

        public static readonly List<SpellId> DispelItemGoodSelf = new List<SpellId>()
        {
            SpellId.DispelItemGoodSelf1,
            SpellId.DispelItemGoodSelf2,
            SpellId.DispelItemGoodSelf3,
            SpellId.DispelItemGoodSelf4,
            SpellId.DispelItemGoodSelf5,
            SpellId.DispelItemGoodSelf6,
        };

        public static readonly List<SpellId> DispelItemBadSelf = new List<SpellId>()
        {
            SpellId.DispelItemBadSelf1,
            SpellId.DispelItemBadSelf2,
            SpellId.DispelItemBadSelf3,
            SpellId.DispelItemBadSelf4,
            SpellId.DispelItemBadSelf5,
            SpellId.DispelItemBadSelf6,
        };

        public static readonly List<SpellId> DispelLifeNeutralOther = new List<SpellId>()
        {
            SpellId.DispelLifeNeutralOther1,
            SpellId.DispelLifeNeutralOther2,
            SpellId.DispelLifeNeutralOther3,
            SpellId.DispelLifeNeutralOther4,
            SpellId.DispelLifeNeutralOther5,
            SpellId.DispelLifeNeutralOther6,
            SpellId.DispelLifeNeutralOther7,
            SpellId.DispelLifeNeutralOther8,
        };

        public static readonly List<SpellId> DispelLifeGoodOther = new List<SpellId>()
        {
            SpellId.DispelLifeGoodOther1,
            SpellId.DispelLifeGoodOther2,
            SpellId.DispelLifeGoodOther3,
            SpellId.DispelLifeGoodOther4,
            SpellId.DispelLifeGoodOther5,
            SpellId.DispelLifeGoodOther6,
            SpellId.DispelLifeGoodOther7,
            SpellId.DispelLifeGoodOther8,
        };

        public static readonly List<SpellId> DispelLifeBadOther = new List<SpellId>()
        {
            SpellId.DispelLifeBadOther1,
            SpellId.DispelLifeBadOther2,
            SpellId.DispelLifeBadOther3,
            SpellId.DispelLifeBadOther4,
            SpellId.DispelLifeBadOther5,
            SpellId.DispelLifeBadOther6,
            SpellId.DispelLifeBadOther7,
            SpellId.DispelLifeBadOther8,
        };

        public static readonly List<SpellId> DispelLifeNeutralSelf = new List<SpellId>()
        {
            SpellId.DispelLifeNeutralSelf1,
            SpellId.DispelLifeNeutralSelf2,
            SpellId.DispelLifeNeutralSelf3,
            SpellId.DispelLifeNeutralSelf4,
            SpellId.DispelLifeNeutralSelf5,
            SpellId.DispelLifeNeutralSelf6,
            SpellId.DispelLifeNeutralSelf7,
            SpellId.DispelLifeNeutralSelf8,
        };

        public static readonly List<SpellId> DispelLifeGoodSelf = new List<SpellId>()
        {
            SpellId.DispelLifeGoodSelf1,
            SpellId.DispelLifeGoodSelf2,
            SpellId.DispelLifeGoodSelf3,
            SpellId.DispelLifeGoodSelf4,
            SpellId.DispelLifeGoodSelf5,
            SpellId.DispelLifeGoodSelf6,
            SpellId.DispelLifeGoodSelf7,
            SpellId.DispelLifeGoodSelf8,
        };

        public static readonly List<SpellId> DispelLifeBadSelf = new List<SpellId>()
        {
            SpellId.DispelLifeBadSelf1,
            SpellId.DispelLifeBadSelf2,
            SpellId.DispelLifeBadSelf3,
            SpellId.DispelLifeBadSelf4,
            SpellId.DispelLifeBadSelf5,
            SpellId.DispelLifeBadSelf6,
            SpellId.DispelLifeBadSelf7,
            SpellId.DispelLifeBadSelf8,
        };

        public static readonly List<SpellId> RecallAsmolum = new List<SpellId>()
        {
            SpellId.RecallAsmolum1,
            SpellId.RecallAsmolum2,
            SpellId.RecallAsmolum3,
        };

        public static readonly List<SpellId> PortalSendTrial = new List<SpellId>()
        {
            SpellId.PortalSendTrial1,
            SpellId.PortalSendTrial2,
            SpellId.PortalSendTrial3,
            SpellId.PortalSendTrial4,
            SpellId.PortalSendTrial5,
        };

        public static readonly List<SpellId> CANTRIPALCHEMICALPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPALCHEMICALPROWESS1,
            SpellId.CANTRIPALCHEMICALPROWESS2,
            SpellId.CANTRIPALCHEMICALPROWESS3,
            SpellId.CantripAlchemicalProwess4,
        };

        public static readonly List<SpellId> CANTRIPARCANEPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPARCANEPROWESS1,
            SpellId.CANTRIPARCANEPROWESS2,
            SpellId.CANTRIPARCANEPROWESS3,
            SpellId.CantripArcaneProwess4,
        };

        public static readonly List<SpellId> CANTRIPARMOREXPERTISE = new List<SpellId>()
        {
            SpellId.CANTRIPARMOREXPERTISE1,
            SpellId.CANTRIPARMOREXPERTISE2,
            SpellId.CANTRIPARMOREXPERTISE3,
            SpellId.CantripArmorExpertise4,
        };

        public static readonly List<SpellId> CANTRIPLIGHTWEAPONSAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1,
            SpellId.CANTRIPLIGHTWEAPONSAPTITUDE2,
            SpellId.CANTRIPLIGHTWEAPONSAPTITUDE3,
            SpellId.CantripLightWeaponsAptitude4,
        };

        public static readonly List<SpellId> CANTRIPMISSILEWEAPONSAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1,
            SpellId.CANTRIPMISSILEWEAPONSAPTITUDE2,
            SpellId.CANTRIPMISSILEWEAPONSAPTITUDE3,
            SpellId.CantripMissileWeaponsAptitude4,
        };

        public static readonly List<SpellId> CANTRIPCOOKINGPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPCOOKINGPROWESS1,
            SpellId.CANTRIPCOOKINGPROWESS2,
            SpellId.CANTRIPCOOKINGPROWESS3,
            SpellId.CantripCookingProwess4,
        };

        public static readonly List<SpellId> CANTRIPCREATUREENCHANTMENTAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE1,
            SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE2,
            SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE3,
            SpellId.CantripCreatureEnchantmentAptitude4,
        };

        public static readonly List<SpellId> CANTRIPCROSSBOWAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPCROSSBOWAPTITUDE1,
            SpellId.CANTRIPCROSSBOWAPTITUDE2,
            SpellId.CANTRIPCROSSBOWAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPFINESSEWEAPONSAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1,
            SpellId.CANTRIPFINESSEWEAPONSAPTITUDE2,
            SpellId.CANTRIPFINESSEWEAPONSAPTITUDE3,
            SpellId.CantripFinesseWeaponsAptitude4,
        };

        public static readonly List<SpellId> CANTRIPDECEPTIONPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPDECEPTIONPROWESS1,
            SpellId.CANTRIPDECEPTIONPROWESS2,
            SpellId.CANTRIPDECEPTIONPROWESS3,
            SpellId.CantripDeceptionProwess4,
        };

        public static readonly List<SpellId> CANTRIPFEALTY = new List<SpellId>()
        {
            SpellId.CANTRIPFEALTY1,
            SpellId.CANTRIPFEALTY2,
            SpellId.CANTRIPFEALTY3,
            SpellId.CantripFealty4,
        };

        public static readonly List<SpellId> CANTRIPFLETCHINGPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPFLETCHINGPROWESS1,
            SpellId.CANTRIPFLETCHINGPROWESS2,
            SpellId.CANTRIPFLETCHINGPROWESS3,
            SpellId.CantripFletchingProwess4,
        };

        public static readonly List<SpellId> CANTRIPHEALINGPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPHEALINGPROWESS1,
            SpellId.CANTRIPHEALINGPROWESS2,
            SpellId.CANTRIPHEALINGPROWESS3,
            SpellId.CantripHealingProwess4,
        };

        public static readonly List<SpellId> CANTRIPIMPREGNABILITY = new List<SpellId>()
        {
            SpellId.CANTRIPIMPREGNABILITY1,
            SpellId.CANTRIPIMPREGNABILITY2,
            SpellId.CANTRIPIMPREGNABILITY3,
            SpellId.CantripImpregnability4,
        };

        public static readonly List<SpellId> CANTRIPINVULNERABILITY = new List<SpellId>()
        {
            SpellId.CANTRIPINVULNERABILITY1,
            SpellId.CANTRIPINVULNERABILITY2,
            SpellId.CANTRIPINVULNERABILITY3,
            SpellId.CantripInvulnerability4,
        };

        public static readonly List<SpellId> CANTRIPITEMENCHANTMENTAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPITEMENCHANTMENTAPTITUDE1,
            SpellId.CANTRIPITEMENCHANTMENTAPTITUDE2,
            SpellId.CANTRIPITEMENCHANTMENTAPTITUDE3,
            SpellId.CantripItemEnchantmentAptitude4,
        };

        public static readonly List<SpellId> CANTRIPITEMEXPERTISE = new List<SpellId>()
        {
            SpellId.CANTRIPITEMEXPERTISE1,
            SpellId.CANTRIPITEMEXPERTISE2,
            SpellId.CANTRIPITEMEXPERTISE3,
            SpellId.CantripItemExpertise4,
        };

        public static readonly List<SpellId> CANTRIPJUMPINGPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPJUMPINGPROWESS1,
            SpellId.CANTRIPJUMPINGPROWESS2,
            SpellId.CANTRIPJUMPINGPROWESS3,
            SpellId.CantripJumpingProwess4,
        };

        public static readonly List<SpellId> CANTRIPLEADERSHIP = new List<SpellId>()
        {
            SpellId.CANTRIPLEADERSHIP1,
            SpellId.CANTRIPLEADERSHIP2,
            SpellId.CANTRIPLEADERSHIP3,
            SpellId.CantripLeadership4,
        };

        public static readonly List<SpellId> CANTRIPLIFEMAGICAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPLIFEMAGICAPTITUDE1,
            SpellId.CANTRIPLIFEMAGICAPTITUDE2,
            SpellId.CANTRIPLIFEMAGICAPTITUDE3,
            SpellId.CantripLifeMagicAptitude4,
        };

        public static readonly List<SpellId> CANTRIPLOCKPICKPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPLOCKPICKPROWESS1,
            SpellId.CANTRIPLOCKPICKPROWESS2,
            SpellId.CANTRIPLOCKPICKPROWESS3,
            SpellId.CantripLockpickProwess4,
        };

        public static readonly List<SpellId> CANTRIPMACEAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPMACEAPTITUDE1,
            SpellId.CANTRIPMACEAPTITUDE2,
            SpellId.CANTRIPMACEAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPMAGICITEMEXPERTISE = new List<SpellId>()
        {
            SpellId.CANTRIPMAGICITEMEXPERTISE1,
            SpellId.CANTRIPMAGICITEMEXPERTISE2,
            SpellId.CANTRIPMAGICITEMEXPERTISE3,
            SpellId.CantripMagicItemExpertise4,
        };

        public static readonly List<SpellId> CANTRIPMAGICRESISTANCE = new List<SpellId>()
        {
            SpellId.CANTRIPMAGICRESISTANCE1,
            SpellId.CANTRIPMAGICRESISTANCE2,
            SpellId.CANTRIPMAGICRESISTANCE3,
            SpellId.CantripMagicResistance4,
        };

        public static readonly List<SpellId> CANTRIPMANACONVERSIONPROWESS = new List<SpellId>()
        {
            SpellId.CANTRIPMANACONVERSIONPROWESS1,
            SpellId.CANTRIPMANACONVERSIONPROWESS2,
            SpellId.CANTRIPMANACONVERSIONPROWESS3,
            SpellId.CantripManaConversionProwess4,
        };

        public static readonly List<SpellId> CANTRIPMONSTERATTUNEMENT = new List<SpellId>()
        {
            SpellId.CANTRIPMONSTERATTUNEMENT1,
            SpellId.CANTRIPMONSTERATTUNEMENT2,
            SpellId.CANTRIPMONSTERATTUNEMENT3,
            SpellId.CantripMonsterAttunement4,
        };

        public static readonly List<SpellId> CANTRIPPERSONATTUNEMENT = new List<SpellId>()
        {
            SpellId.CANTRIPPERSONATTUNEMENT1,
            SpellId.CANTRIPPERSONATTUNEMENT2,
            SpellId.CANTRIPPERSONATTUNEMENT3,
            SpellId.CantripPersonAttunement4,
        };

        public static readonly List<SpellId> CANTRIPSPEARAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPSPEARAPTITUDE1,
            SpellId.CANTRIPSPEARAPTITUDE2,
            SpellId.CANTRIPSPEARAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPSPRINT = new List<SpellId>()
        {
            SpellId.CANTRIPSPRINT1,
            SpellId.CANTRIPSPRINT2,
            SpellId.CANTRIPSPRINT3,
            SpellId.CantripSprint4,
        };

        public static readonly List<SpellId> CANTRIPSTAFFAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPSTAFFAPTITUDE1,
            SpellId.CANTRIPSTAFFAPTITUDE2,
            SpellId.CANTRIPSTAFFAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPHEAVYWEAPONSAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1,
            SpellId.CANTRIPHEAVYWEAPONSAPTITUDE2,
            SpellId.CANTRIPHEAVYWEAPONSAPTITUDE3,
            SpellId.CantripHeavyWeaponsAptitude4,
        };

        public static readonly List<SpellId> CANTRIPTHROWNAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPTHROWNAPTITUDE1,
            SpellId.CANTRIPTHROWNAPTITUDE2,
            SpellId.CANTRIPTHROWNAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPUNARMEDAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPUNARMEDAPTITUDE1,
            SpellId.CANTRIPUNARMEDAPTITUDE2,
            SpellId.CANTRIPUNARMEDAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPWARMAGICAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPWARMAGICAPTITUDE1,
            SpellId.CANTRIPWARMAGICAPTITUDE2,
            SpellId.CANTRIPWARMAGICAPTITUDE3,
            SpellId.CantripWarMagicAptitude4,
        };

        public static readonly List<SpellId> CANTRIPWEAPONEXPERTISE = new List<SpellId>()
        {
            SpellId.CANTRIPWEAPONEXPERTISE1,
            SpellId.CANTRIPWEAPONEXPERTISE2,
            SpellId.CANTRIPWEAPONEXPERTISE3,
            SpellId.CantripWeaponExpertise4,
        };

        public static readonly List<SpellId> CANTRIPARMOR = new List<SpellId>()
        {
            SpellId.CANTRIPARMOR1,
            SpellId.CANTRIPARMOR2,
            SpellId.CANTRIPARMOR3,
            SpellId.CantripArmor4,
        };

        public static readonly List<SpellId> CANTRIPCOORDINATION = new List<SpellId>()
        {
            SpellId.CANTRIPCOORDINATION1,
            SpellId.CANTRIPCOORDINATION2,
            SpellId.CANTRIPCOORDINATION3,
            SpellId.CantripCoordination4,
        };

        public static readonly List<SpellId> CANTRIPENDURANCE = new List<SpellId>()
        {
            SpellId.CANTRIPENDURANCE1,
            SpellId.CANTRIPENDURANCE2,
            SpellId.CANTRIPENDURANCE3,
            SpellId.CantripEndurance4,
        };

        public static readonly List<SpellId> CANTRIPFOCUS = new List<SpellId>()
        {
            SpellId.CANTRIPFOCUS1,
            SpellId.CANTRIPFOCUS2,
            SpellId.CANTRIPFOCUS3,
            SpellId.CantripFocus4,
        };

        public static readonly List<SpellId> CANTRIPQUICKNESS = new List<SpellId>()
        {
            SpellId.CANTRIPQUICKNESS1,
            SpellId.CANTRIPQUICKNESS2,
            SpellId.CANTRIPQUICKNESS3,
            SpellId.CantripQuickness4,
        };

        public static readonly List<SpellId> CANTRIPSTRENGTH = new List<SpellId>()
        {
            SpellId.CANTRIPSTRENGTH1,
            SpellId.CANTRIPSTRENGTH2,
            SpellId.CANTRIPSTRENGTH3,
            SpellId.CantripStrength4,
        };

        public static readonly List<SpellId> CANTRIPWILLPOWER = new List<SpellId>()
        {
            SpellId.CANTRIPWILLPOWER1,
            SpellId.CANTRIPWILLPOWER2,
            SpellId.CANTRIPWILLPOWER3,
            SpellId.CantripWillpower4,
        };

        public static readonly List<SpellId> CANTRIPACIDBANE = new List<SpellId>()
        {
            SpellId.CANTRIPACIDBANE1,
            SpellId.CANTRIPACIDBANE2,
            SpellId.CANTRIPACIDBANE3,
            SpellId.CantripAcidBane4,
        };

        public static readonly List<SpellId> CANTRIPBLOODTHIRST = new List<SpellId>()
        {
            SpellId.CANTRIPBLOODTHIRST1,
            SpellId.CANTRIPBLOODTHIRST2,
            SpellId.CANTRIPBLOODTHIRST3,
            SpellId.CantripBloodThirst4,
        };

        public static readonly List<SpellId> CANTRIPBLUDGEONINGBANE = new List<SpellId>()
        {
            SpellId.CANTRIPBLUDGEONINGBANE1,
            SpellId.CANTRIPBLUDGEONINGBANE2,
            SpellId.CANTRIPBLUDGEONINGBANE3,
            SpellId.CantripBludgeoningBane4,
        };

        public static readonly List<SpellId> CANTRIPDEFENDER = new List<SpellId>()
        {
            SpellId.CANTRIPDEFENDER1,
            SpellId.CANTRIPDEFENDER2,
            SpellId.CANTRIPDEFENDER3,
            SpellId.CantripDefender4,
        };

        public static readonly List<SpellId> CANTRIPFLAMEBANE = new List<SpellId>()
        {
            SpellId.CANTRIPFLAMEBANE1,
            SpellId.CANTRIPFLAMEBANE2,
            SpellId.CANTRIPFLAMEBANE3,
            SpellId.CantripFlameBane4,
        };

        public static readonly List<SpellId> CANTRIPFROSTBANE = new List<SpellId>()
        {
            SpellId.CANTRIPFROSTBANE1,
            SpellId.CANTRIPFROSTBANE2,
            SpellId.CANTRIPFROSTBANE3,
            SpellId.CantripFrostBane4,
        };

        public static readonly List<SpellId> CANTRIPHEARTTHIRST = new List<SpellId>()
        {
            SpellId.CANTRIPHEARTTHIRST1,
            SpellId.CANTRIPHEARTTHIRST2,
            SpellId.CANTRIPHEARTTHIRST3,
            SpellId.CantripHeartThirst4,
        };

        public static readonly List<SpellId> CANTRIPIMPENETRABILITY = new List<SpellId>()
        {
            SpellId.CANTRIPIMPENETRABILITY1,
            SpellId.CANTRIPIMPENETRABILITY2,
            SpellId.CANTRIPIMPENETRABILITY3,
            SpellId.CantripImpenetrability4,
        };

        public static readonly List<SpellId> CANTRIPPIERCINGBANE = new List<SpellId>()
        {
            SpellId.CANTRIPPIERCINGBANE1,
            SpellId.CANTRIPPIERCINGBANE2,
            SpellId.CANTRIPPIERCINGBANE3,
            SpellId.CantripPiercingBane4,
        };

        public static readonly List<SpellId> CANTRIPSLASHINGBANE = new List<SpellId>()
        {
            SpellId.CANTRIPSLASHINGBANE1,
            SpellId.CANTRIPSLASHINGBANE2,
            SpellId.CANTRIPSLASHINGBANE3,
            SpellId.CantripSlashingBane4,
        };

        public static readonly List<SpellId> CANTRIPSTORMBANE = new List<SpellId>()
        {
            SpellId.CANTRIPSTORMBANE1,
            SpellId.CANTRIPSTORMBANE2,
            SpellId.CANTRIPSTORMBANE3,
            SpellId.CantripStormBane4,
        };

        public static readonly List<SpellId> CANTRIPSWIFTHUNTER = new List<SpellId>()
        {
            SpellId.CANTRIPSWIFTHUNTER1,
            SpellId.CANTRIPSWIFTHUNTER2,
            SpellId.CANTRIPSWIFTHUNTER3,
            SpellId.CantripSwiftHunter4,
        };

        public static readonly List<SpellId> CANTRIPACIDWARD = new List<SpellId>()
        {
            SpellId.CANTRIPACIDWARD1,
            SpellId.CANTRIPACIDWARD2,
            SpellId.CANTRIPACIDWARD3,
            SpellId.CantripAcidWard4,
        };

        public static readonly List<SpellId> CANTRIPBLUDGEONINGWARD = new List<SpellId>()
        {
            SpellId.CANTRIPBLUDGEONINGWARD1,
            SpellId.CANTRIPBLUDGEONINGWARD2,
            SpellId.CANTRIPBLUDGEONINGWARD3,
            SpellId.CantripBludgeoningWard4,
        };

        public static readonly List<SpellId> CANTRIPFLAMEWARD = new List<SpellId>()
        {
            SpellId.CANTRIPFLAMEWARD1,
            SpellId.CANTRIPFLAMEWARD2,
            SpellId.CANTRIPFLAMEWARD3,
            SpellId.CantripFlameWard4,
        };

        public static readonly List<SpellId> CANTRIPFROSTWARD = new List<SpellId>()
        {
            SpellId.CANTRIPFROSTWARD1,
            SpellId.CANTRIPFROSTWARD2,
            SpellId.CANTRIPFROSTWARD3,
            SpellId.CantripFrostWard4,
        };

        public static readonly List<SpellId> CANTRIPPIERCINGWARD = new List<SpellId>()
        {
            SpellId.CANTRIPPIERCINGWARD1,
            SpellId.CANTRIPPIERCINGWARD2,
            SpellId.CANTRIPPIERCINGWARD3,
            SpellId.CantripPiercingWard4,
        };

        public static readonly List<SpellId> CANTRIPSLASHINGWARD = new List<SpellId>()
        {
            SpellId.CANTRIPSLASHINGWARD1,
            SpellId.CANTRIPSLASHINGWARD2,
            SpellId.CANTRIPSLASHINGWARD3,
            SpellId.CantripSlashingWard4,
        };

        public static readonly List<SpellId> CANTRIPSTORMWARD = new List<SpellId>()
        {
            SpellId.CANTRIPSTORMWARD1,
            SpellId.CANTRIPSTORMWARD2,
            SpellId.CANTRIPSTORMWARD3,
            SpellId.CantripStormWard4,
        };

        public static readonly List<SpellId> CANTRIPHEALTHGAIN = new List<SpellId>()
        {
            SpellId.CANTRIPHEALTHGAIN1,
            SpellId.CANTRIPHEALTHGAIN2,
            SpellId.CANTRIPHEALTHGAIN3,
            SpellId.CantripHealthGain4,
        };

        public static readonly List<SpellId> CANTRIPMANAGAIN = new List<SpellId>()
        {
            SpellId.CANTRIPMANAGAIN1,
            SpellId.CANTRIPMANAGAIN2,
            SpellId.CANTRIPMANAGAIN3,
            SpellId.CantripManaGain4,
        };

        public static readonly List<SpellId> CANTRIPSTAMINAGAIN = new List<SpellId>()
        {
            SpellId.CANTRIPSTAMINAGAIN1,
            SpellId.CANTRIPSTAMINAGAIN2,
            SpellId.CANTRIPSTAMINAGAIN3,
            SpellId.CantripStaminaGain4,
        };

        public static readonly List<SpellId> SummonSecondPortal = new List<SpellId>()
        {
            SpellId.SummonSecondPortal1,
            SpellId.SummonSecondPortal2,
            SpellId.SummonSecondPortal3,
        };

        public static readonly List<SpellId> MartineRing = new List<SpellId>()
        {
            SpellId.MartineRing1,
            SpellId.MartineRing2,
        };

        public static readonly List<SpellId> ElementalFury = new List<SpellId>()
        {
            SpellId.ElementalFury1,
            SpellId.ElementalFury2,
            SpellId.ElementalFury3,
            SpellId.ElementalFury4,
        };

        public static readonly List<SpellId> AcidArc = new List<SpellId>()
        {
            SpellId.AcidArc1,
            SpellId.AcidArc2,
            SpellId.AcidArc3,
            SpellId.AcidArc4,
            SpellId.AcidArc5,
            SpellId.AcidArc6,
            SpellId.AcidArc7,
            SpellId.AcidArc8,
        };

        public static readonly List<SpellId> ForceArc = new List<SpellId>()
        {
            SpellId.ForceArc1,
            SpellId.ForceArc2,
            SpellId.ForceArc3,
            SpellId.ForceArc4,
            SpellId.ForceArc5,
            SpellId.ForceArc6,
            SpellId.ForceArc7,
            SpellId.ForceArc8,
        };

        public static readonly List<SpellId> FrostArc = new List<SpellId>()
        {
            SpellId.FrostArc1,
            SpellId.FrostArc2,
            SpellId.FrostArc3,
            SpellId.FrostArc4,
            SpellId.FrostArc5,
            SpellId.FrostArc6,
            SpellId.FrostArc7,
            SpellId.FrostArc8,
        };

        public static readonly List<SpellId> LightningArc = new List<SpellId>()
        {
            SpellId.LightningArc1,
            SpellId.LightningArc2,
            SpellId.LightningArc3,
            SpellId.LightningArc4,
            SpellId.LightningArc5,
            SpellId.LightningArc6,
            SpellId.LightningArc7,
            SpellId.LightningArc8,
        };

        public static readonly List<SpellId> FlameArc = new List<SpellId>()
        {
            SpellId.FlameArc1,
            SpellId.FlameArc2,
            SpellId.FlameArc3,
            SpellId.FlameArc4,
            SpellId.FlameArc5,
            SpellId.FlameArc6,
            SpellId.FlameArc7,
            SpellId.FlameArc8,
        };

        public static readonly List<SpellId> ShockArc = new List<SpellId>()
        {
            SpellId.ShockArc1,
            SpellId.ShockArc2,
            SpellId.ShockArc3,
            SpellId.ShockArc4,
            SpellId.ShockArc5,
            SpellId.ShockArc6,
            SpellId.ShockArc7,
            SpellId.ShockArc8,
        };

        public static readonly List<SpellId> BladeArc = new List<SpellId>()
        {
            SpellId.BladeArc1,
            SpellId.BladeArc2,
            SpellId.BladeArc3,
            SpellId.BladeArc4,
            SpellId.BladeArc5,
            SpellId.BladeArc6,
            SpellId.BladeArc7,
            SpellId.BladeArc8,
        };

        public static readonly List<SpellId> HealthBolt = new List<SpellId>()
        {
            SpellId.HealthBolt1,
            SpellId.HealthBolt2,
            SpellId.HealthBolt3,
            SpellId.HealthBolt4,
            SpellId.HealthBolt5,
            SpellId.HealthBolt6,
            SpellId.HealthBolt7,
            SpellId.HealthBolt8,
        };

        public static readonly List<SpellId> StaminaBolt = new List<SpellId>()
        {
            SpellId.StaminaBolt1,
            SpellId.StaminaBolt2,
            SpellId.StaminaBolt3,
            SpellId.StaminaBolt4,
            SpellId.StaminaBolt5,
            SpellId.StaminaBolt6,
            SpellId.StaminaBolt7,
            SpellId.StaminaBolt8,
        };

        public static readonly List<SpellId> ManaBolt = new List<SpellId>()
        {
            //SpellId.ManaBolt,
            SpellId.ManaBolt1,
            SpellId.ManaBolt2,
            SpellId.ManaBolt3,
            SpellId.ManaBolt4,
            SpellId.ManaBolt5,
            SpellId.ManaBolt6,
            SpellId.ManaBolt7,
            SpellId.ManaBolt8,
        };

        public static readonly List<SpellId> FireworkOutBlack = new List<SpellId>()
        {
            SpellId.FireworkOutBlack1,
            SpellId.FireworkOutBlack2,
            SpellId.FireworkOutBlack3,
            SpellId.FireworkOutBlack4,
            SpellId.FireworkOutBlack5,
            SpellId.FireworkOutBlack6,
            SpellId.FireworkOutBlack7,
            SpellId.FireworkOutBlack8,
        };

        public static readonly List<SpellId> FireworkOutBlue = new List<SpellId>()
        {
            SpellId.FireworkOutBlue1,
            SpellId.FireworkOutBlue2,
            SpellId.FireworkOutBlue3,
            SpellId.FireworkOutBlue4,
            SpellId.FireworkOutBlue5,
            SpellId.FireworkOutBlue6,
            SpellId.FireworkOutBlue7,
            SpellId.FireworkOutBlue8,
        };

        public static readonly List<SpellId> FireworkOutGreen = new List<SpellId>()
        {
            SpellId.FireworkOutGreen1,
            SpellId.FireworkOutGreen2,
            SpellId.FireworkOutGreen3,
            SpellId.FireworkOutGreen4,
            SpellId.FireworkOutGreen5,
            SpellId.FireworkOutGreen6,
            SpellId.FireworkOutGreen7,
            SpellId.FireworkOutGreen8,
        };

        public static readonly List<SpellId> FireworkOutOrange = new List<SpellId>()
        {
            SpellId.FireworkOutOrange1,
            SpellId.FireworkOutOrange2,
            SpellId.FireworkOutOrange3,
            SpellId.FireworkOutOrange4,
            SpellId.FireworkOutOrange5,
            SpellId.FireworkOutOrange6,
            SpellId.FireworkOutOrange7,
            SpellId.FireworkOutOrange8,
        };

        public static readonly List<SpellId> FireworkOutPurple = new List<SpellId>()
        {
            SpellId.FireworkOutPurple1,
            SpellId.FireworkOutPurple2,
            SpellId.FireworkOutPurple3,
            SpellId.FireworkOutPurple4,
            SpellId.FireworkOutPurple5,
            SpellId.FireworkOutPurple6,
            SpellId.FireworkOutPurple7,
            SpellId.FireworkOutPurple8,
        };

        public static readonly List<SpellId> FireworkOutRed = new List<SpellId>()
        {
            SpellId.FireworkOutRed1,
            SpellId.FireworkOutRed2,
            SpellId.FireworkOutRed3,
            SpellId.FireworkOutRed4,
            SpellId.FireworkOutRed5,
            SpellId.FireworkOutRed6,
            SpellId.FireworkOutRed7,
            SpellId.FireworkOutRed8,
        };

        public static readonly List<SpellId> FireworkOutWhite = new List<SpellId>()
        {
            SpellId.FireworkOutWhite1,
            SpellId.FireworkOutWhite2,
            SpellId.FireworkOutWhite3,
            SpellId.FireworkOutWhite4,
            SpellId.FireworkOutWhite5,
            SpellId.FireworkOutWhite6,
            SpellId.FireworkOutWhite7,
            SpellId.FireworkOutWhite8,
        };

        public static readonly List<SpellId> FireworkOutYellow = new List<SpellId>()
        {
            SpellId.FireworkOutYellow1,
            SpellId.FireworkOutYellow2,
            SpellId.FireworkOutYellow3,
            SpellId.FireworkOutYellow4,
            SpellId.FireworkOutYellow5,
            SpellId.FireworkOutYellow6,
            SpellId.FireworkOutYellow7,
            SpellId.FireworkOutYellow8,
        };

        public static readonly List<SpellId> FireworkUpBlack = new List<SpellId>()
        {
            SpellId.FireworkUpBlack1,
            SpellId.FireworkUpBlack2,
            SpellId.FireworkUpBlack3,
            SpellId.FireworkUpBlack4,
            SpellId.FireworkUpBlack5,
            SpellId.FireworkUpBlack6,
            SpellId.FireworkUpBlack7,
            SpellId.FireworkUpBlack8,
        };

        public static readonly List<SpellId> FireworkUpBlue = new List<SpellId>()
        {
            SpellId.FireworkUpBlue1,
            SpellId.FireworkUpBlue2,
            SpellId.FireworkUpBlue3,
            SpellId.FireworkUpBlue4,
            SpellId.FireworkUpBlue5,
            SpellId.FireworkUpBlue6,
            SpellId.FireworkUpBlue7,
            SpellId.FireworkUpBlue8,
        };

        public static readonly List<SpellId> FireworkUpGreen = new List<SpellId>()
        {
            SpellId.FireworkUpGreen1,
            SpellId.FireworkUpGreen2,
            SpellId.FireworkUpGreen3,
            SpellId.FireworkUpGreen4,
            SpellId.FireworkUpGreen5,
            SpellId.FireworkUpGreen6,
            SpellId.FireworkUpGreen7,
            SpellId.FireworkUpGreen8,
        };

        public static readonly List<SpellId> FireworkUpOrange = new List<SpellId>()
        {
            SpellId.FireworkUpOrange1,
            SpellId.FireworkUpOrange2,
            SpellId.FireworkUpOrange3,
            SpellId.FireworkUpOrange4,
            SpellId.FireworkUpOrange5,
            SpellId.FireworkUpOrange6,
            SpellId.FireworkUpOrange7,
            SpellId.FireworkUpOrange8,
        };

        public static readonly List<SpellId> FireworkUpPurple = new List<SpellId>()
        {
            SpellId.FireworkUpPurple1,
            SpellId.FireworkUpPurple2,
            SpellId.FireworkUpPurple3,
            SpellId.FireworkUpPurple4,
            SpellId.FireworkUpPurple5,
            SpellId.FireworkUpPurple6,
            SpellId.FireworkUpPurple7,
            SpellId.FireworkUpPurple8,
        };

        public static readonly List<SpellId> FireworkUpRed = new List<SpellId>()
        {
            SpellId.FireworkUpRed1,
            SpellId.FireworkUpRed2,
            SpellId.FireworkUpRed3,
            SpellId.FireworkUpRed4,
            SpellId.FireworkUpRed5,
            SpellId.FireworkUpRed6,
            SpellId.FireworkUpRed7,
            SpellId.FireworkUpRed8,
        };

        public static readonly List<SpellId> FireworkUpWhite = new List<SpellId>()
        {
            SpellId.FireworkUpWhite1,
            SpellId.FireworkUpWhite2,
            SpellId.FireworkUpWhite3,
            SpellId.FireworkUpWhite4,
            SpellId.FireworkUpWhite5,
            SpellId.FireworkUpWhite6,
            SpellId.FireworkUpWhite7,
            SpellId.FireworkUpWhite8,
        };

        public static readonly List<SpellId> FireworkUpYellow = new List<SpellId>()
        {
            SpellId.FireworkUpYellow1,
            SpellId.FireworkUpYellow2,
            SpellId.FireworkUpYellow3,
            SpellId.FireworkUpYellow4,
            SpellId.FireworkUpYellow5,
            SpellId.FireworkUpYellow6,
            SpellId.FireworkUpYellow7,
            SpellId.FireworkUpYellow8,
        };

        public static readonly List<SpellId> PortalSendingKnorr = new List<SpellId>()
        {
            SpellId.PortalSendingKnorr,
            SpellId.PortalSendingKnorr2,
            SpellId.PortalSendingKnorr3,
        };

        public static readonly List<SpellId> PortalSendingFellowshipLiazkBurun = new List<SpellId>()
        {
            SpellId.PortalSendingFellowshipLiazkBurun40,
            SpellId.PortalSendingFellowshipLiazkBurun60,
            SpellId.PortalSendingFellowshipLiazkBurun80,
            SpellId.PortalSendingFellowshipLiazkBurun100,
        };

        public static readonly List<SpellId> PortalSendingLiazkBurun = new List<SpellId>()
        {
            SpellId.PortalSendingLiazkBurun40,
            SpellId.PortalSendingLiazkBurun60,
            SpellId.PortalSendingLiazkBurun80,
            SpellId.PortalSendingLiazkBurun100,
        };

        public static readonly List<SpellId> PortalSendingLiazkJump = new List<SpellId>()
        {
            SpellId.PortalSendingLiazkJump40,
            SpellId.PortalSendingLiazkJump60,
            SpellId.PortalSendingLiazkJump80,
            SpellId.PortalSendingLiazkJump100,
        };

        public static readonly List<SpellId> PortalSendingLiazkSpirits = new List<SpellId>()
        {
            SpellId.PortalSendingLiazkSpirits40,
            SpellId.PortalSendingLiazkSpirits60,
            SpellId.PortalSendingLiazkSpirits80,
            SpellId.PortalSendingLiazkSpirits100,
        };

        public static readonly List<SpellId> PortalSendingLiazkTest = new List<SpellId>()
        {
            SpellId.PortalSendingLiazkTest40,
            SpellId.PortalSendingLiazkTest60,
            SpellId.PortalSendingLiazkTest80,
            SpellId.PortalSendingLiazkTest100,
        };

        public static readonly List<SpellId> CoordinationFellowship = new List<SpellId>()
        {
            SpellId.CoordinationFellowship4,
            SpellId.CoordinationFellowship5,
            SpellId.CoordinationFellowship6,
            SpellId.CoordinationFellowship7,
            SpellId.CoordinationFellowship8,
        };

        public static readonly List<SpellId> EnduranceFellowship = new List<SpellId>()
        {
            SpellId.EnduranceFellowship4,
            SpellId.EnduranceFellowship5,
            SpellId.EnduranceFellowship6,
            SpellId.EnduranceFellowship7,
            SpellId.EnduranceFellowship8,
        };

        public static readonly List<SpellId> FocusFellowship = new List<SpellId>()
        {
            SpellId.FocusFellowship4,
            SpellId.FocusFellowship5,
            SpellId.FocusFellowship6,
            SpellId.FocusFellowship7,
            SpellId.FocusFellowship8,
        };

        public static readonly List<SpellId> QuicknessFellowship = new List<SpellId>()
        {
            SpellId.QuicknessFellowship4,
            SpellId.QuicknessFellowship5,
            SpellId.QuicknessFellowship6,
            SpellId.QuicknessFellowship7,
            SpellId.QuicknessFellowship8,
        };

        public static readonly List<SpellId> SelfFellowship = new List<SpellId>()
        {
            SpellId.SelfFellowship4,
            SpellId.SelfFellowship5,
            SpellId.SelfFellowship6,
            SpellId.SelfFellowship7,
            SpellId.SelfFellowship8,
        };

        public static readonly List<SpellId> StrengthFellowship = new List<SpellId>()
        {
            SpellId.StrengthFellowship4,
            SpellId.StrengthFellowship5,
            SpellId.StrengthFellowship6,
            SpellId.StrengthFellowship7,
            SpellId.StrengthFellowship8,
        };

        public static readonly List<SpellId> CantripHermeticLink = new List<SpellId>()
        {
            SpellId.CantripHermeticLink1,
            SpellId.CantripHermeticLink2,
            SpellId.CantripHermeticLink3,
            SpellId.CantripHermeticLink4,
        };

        public static readonly List<SpellId> CantripSpiritThirst = new List<SpellId>()
        {
            SpellId.CantripSpiritThirst1,
            SpellId.CantripSpiritThirst2,
            SpellId.CANTRIPSPIRITTHIRST3,
            SpellId.CantripSpiritThirst4,
        };

        public static readonly List<SpellId> SpiritDrinkerSelf = new List<SpellId>()
        {
            SpellId.SpiritDrinkerSelf1,
            SpellId.SpiritDrinkerSelf2,
            SpellId.SpiritDrinkerSelf3,
            SpellId.SpiritDrinkerSelf4,
            SpellId.SpiritDrinkerSelf5,
            SpellId.SpiritDrinkerSelf6,
            SpellId.SpiritDrinkerSelf7,
            SpellId.SpiritDrinkerSelf8,
        };

        public static readonly List<SpellId> SpiritLoather = new List<SpellId>()
        {
            SpellId.SpiritLoather1,
            SpellId.SpiritLoather2,
            SpellId.SpiritLoather3,
            SpellId.SpiritLoather4,
            SpellId.SpiritLoather5,
            SpellId.SpiritLoather6,
            SpellId.SpiritLoather7,
            SpellId.SpiritLoather8,
        };

        public static readonly List<SpellId> PortalSendingHezhitFight = new List<SpellId>()
        {
            SpellId.PortalSendingHezhitFight1,
            SpellId.PortalSendingHezhitFight2,
            SpellId.PortalSendingHezhitFight3,
        };

        public static readonly List<SpellId> PortalSendingHezhitPrison = new List<SpellId>()
        {
            SpellId.PortalSendingHezhitPrison1,
            SpellId.PortalSendingHezhitPrison2,
            SpellId.PortalSendingHezhitPrison3,
            SpellId.PortalSendingHezhitPrison4,
            SpellId.PortalSendingHezhitPrison5,
            SpellId.PortalSendingHezhitPrison6,
        };

        public static readonly List<SpellId> PortalSendingHizkRiGauntlet = new List<SpellId>()
        {
            SpellId.PortalSendingHizkRiGauntlet60,
            SpellId.PortalSendingHizkRiGauntlet80,
            SpellId.PortalSendingHizkRiGauntlet100,
        };

        public static readonly List<SpellId> PortalSendingHizkRiWell = new List<SpellId>()
        {
            SpellId.PortalSendingHizkRiWell60,
            SpellId.PortalSendingHizkRiWell80,
            SpellId.PortalSendingHizkRiWell100,
        };

        public static readonly List<SpellId> PortalSendingJrvikFight = new List<SpellId>()
        {
            SpellId.PortalSendingJrvikFight1,
            SpellId.PortalSendingJrvikFight2,
            SpellId.PortalSendingJrvikFight3,
        };

        public static readonly List<SpellId> PortalSendingJrvikPrison = new List<SpellId>()
        {
            SpellId.PortalSendingJrvikPrison1,
            SpellId.PortalSendingJrvikPrison2,
            SpellId.PortalSendingJrvikPrison3,
            SpellId.PortalSendingJrvikPrison4,
            SpellId.PortalSendingJrvikPrison5,
            SpellId.PortalSendingJrvikPrison6,
        };

        public static readonly List<SpellId> PortalSendingZixkFight = new List<SpellId>()
        {
            SpellId.PortalSendingZixkFight1,
            SpellId.PortalSendingZixkFight2,
            SpellId.PortalSendingZixkFight3,
        };

        public static readonly List<SpellId> PortalSendingZixkPrison = new List<SpellId>()
        {
            SpellId.PortalSendingZixkPrison1,
            SpellId.PortalSendingZixkPrison2,
            SpellId.PortalSendingZixkPrison3,
            SpellId.PortalSendingZixkPrison4,
            SpellId.PortalSendingZixkPrison5,
            SpellId.PortalSendingZixkPrison6,
        };

        public static readonly List<SpellId> PortalSendingHizkRiGuruk = new List<SpellId>()
        {
            SpellId.PortalSendingHizkRiGuruk60,
            SpellId.PortalSendingHizkRiGuruk80,
            SpellId.PortalSendingHizkRiGuruk100,
        };

        public static readonly List<SpellId> AcidProtectionFellowship = new List<SpellId>()
        {
            SpellId.AcidProtectionFellowship4,
            SpellId.AcidProtectionFellowship5,
            SpellId.AcidProtectionFellowship6,
            SpellId.AcidProtectionFellowship7,
            SpellId.AcidProtectionFellowship8,
        };

        public static readonly List<SpellId> BladeProtectionFellowship = new List<SpellId>()
        {
            SpellId.BladeProtectionFellowship4,
            SpellId.BladeProtectionFellowship5,
            SpellId.BladeProtectionFellowship6,
            SpellId.BladeProtectionFellowship7,
            SpellId.BladeProtectionFellowship8,
        };

        public static readonly List<SpellId> BludgeonProtectionFellowship = new List<SpellId>()
        {
            SpellId.BludgeonProtectionFellowship4,
            SpellId.BludgeonProtectionFellowship5,
            SpellId.BludgeonProtectionFellowship6,
            SpellId.BludgeonProtectionFellowship7,
            SpellId.BludgeonProtectionFellowship8,
        };

        public static readonly List<SpellId> ColdProtectionFellowship = new List<SpellId>()
        {
            SpellId.ColdProtectionFellowship4,
            SpellId.ColdProtectionFellowship5,
            SpellId.ColdProtectionFellowship6,
            SpellId.ColdProtectionFellowship7,
            SpellId.ColdProtectionFellowship8,
        };

        public static readonly List<SpellId> FireProtectionFellowship = new List<SpellId>()
        {
            SpellId.FireProtectionFellowship4,
            SpellId.FireProtectionFellowship5,
            SpellId.FireProtectionFellowship6,
            SpellId.FireProtectionFellowship7,
            SpellId.FireProtectionFellowship8,
        };

        public static readonly List<SpellId> LightningProtectionFellowship = new List<SpellId>()
        {
            SpellId.LightningProtectionFellowship4,
            SpellId.LightningProtectionFellowship5,
            SpellId.LightningProtectionFellowship6,
            SpellId.LightningProtectionFellowship7,
            SpellId.LightningProtectionFellowship8,
        };

        public static readonly List<SpellId> PierceProtectionFellowship = new List<SpellId>()
        {
            SpellId.PierceProtectionFellowship4,
            SpellId.PierceProtectionFellowship5,
            SpellId.PierceProtectionFellowship6,
            SpellId.PierceProtectionFellowship7,
            SpellId.PierceProtectionFellowship8,
        };

        public static readonly List<SpellId> ImpregnabilityFellowship = new List<SpellId>()
        {
            SpellId.ImpregnabilityFellowship4,
            SpellId.ImpregnabilityFellowship5,
            SpellId.ImpregnabilityFellowship6,
            SpellId.ImpregnabilityFellowship7,
            SpellId.ImpregnabilityFellowship8,
        };

        public static readonly List<SpellId> InvulnerabilityFellowship = new List<SpellId>()
        {
            SpellId.InvulnerabilityFellowship4,
            SpellId.InvulnerabilityFellowship5,
            SpellId.InvulnerabilityFellowship6,
            SpellId.InvulnerabilityFellowship7,
            SpellId.InvulnerabilityFellowship8,
        };

        public static readonly List<SpellId> MagicResistanceFellowship = new List<SpellId>()
        {
            SpellId.MagicResistanceFellowship4,
            SpellId.MagicResistanceFellowship5,
            SpellId.MagicResistanceFellowship6,
            SpellId.MagicResistanceFellowship7,
            SpellId.MagicResistanceFellowship8,
        };

        public static readonly List<SpellId> CreatureEnchantmentMasteryFellow = new List<SpellId>()
        {
            SpellId.CreatureEnchantmentMasteryFellow4,
            SpellId.CreatureEnchantmentMasteryFellow5,
            SpellId.CreatureEnchantmentMasteryFellow6,
            SpellId.CreatureEnchantmentMasteryFellow7,
            SpellId.CreatureEnchantmentMasteryFellow8,
        };

        public static readonly List<SpellId> ItemEnchantmentMasteryFellow = new List<SpellId>()
        {
            SpellId.ItemEnchantmentMasteryFellow4,
            SpellId.ItemEnchantmentMasteryFellow5,
            SpellId.ItemEnchantmentMasteryFellow6,
            SpellId.ItemEnchantmentMasteryFellow7,
            SpellId.ItemEnchantmentMasteryFellow8,
        };

        public static readonly List<SpellId> LifeMagicMasteryFellow = new List<SpellId>()
        {
            SpellId.LifeMagicMasteryFellow4,
            SpellId.LifeMagicMasteryFellow5,
            SpellId.LifeMagicMasteryFellow6,
            SpellId.LifeMagicMasteryFellow7,
            SpellId.LifeMagicMasteryFellow8,
        };

        public static readonly List<SpellId> ManaConversionMasteryFellow = new List<SpellId>()
        {
            SpellId.ManaConversionMasteryFellow4,
            SpellId.ManaConversionMasteryFellow5,
            SpellId.ManaConversionMasteryFellow6,
            SpellId.ManaConversionMasteryFellow7,
            SpellId.ManaConversionMasteryFellow8,
        };

        public static readonly List<SpellId> WarMagicMasteryFellow = new List<SpellId>()
        {
            SpellId.WarMagicMasteryFellow4,
            SpellId.WarMagicMasteryFellow5,
            SpellId.WarMagicMasteryFellow6,
            SpellId.WarMagicMasteryFellow7,
            SpellId.WarMagicMasteryFellow8,
        };

        public static readonly List<SpellId> PortalSendingKivikLirArena = new List<SpellId>()
        {
            SpellId.PortalSendingKivikLirArena60,
            SpellId.PortalSendingKivikLirArena80,
            SpellId.PortalSendingKivikLirArena100,
        };

        public static readonly List<SpellId> PortalSendingKivikLirBoss = new List<SpellId>()
        {
            SpellId.PortalSendingKivikLirBoss60,
            SpellId.PortalSendingKivikLirBoss80,
            SpellId.PortalSendingKivikLirBoss100,
        };

        public static readonly List<SpellId> PortalSendingKivikLirHaven = new List<SpellId>()
        {
            SpellId.PortalSendingKivikLirHaven60,
            SpellId.PortalSendingKivikLirHaven80,
            SpellId.PortalSendingKivikLirHaven100,
        };

        public static readonly List<SpellId> ManaRenewalFellowship = new List<SpellId>()
        {
            SpellId.ManaRenewalFellowship4,
            SpellId.ManaRenewalFellowship5,
            SpellId.ManaRenewalFellowship6,
            SpellId.ManaRenewalFellowship7,
            SpellId.ManaRenewalFellowship8,
        };

        public static readonly List<SpellId> RegenerationFellowship = new List<SpellId>()
        {
            SpellId.RegenerationFellowship4,
            SpellId.RegenerationFellowship5,
            SpellId.RegenerationFellowship6,
            SpellId.RegenerationFellowship7,
            SpellId.RegenerationFellowship8,
        };

        public static readonly List<SpellId> RejuvenationFellowship = new List<SpellId>()
        {
            SpellId.RejuvenationFellowship4,
            SpellId.RejuvenationFellowship5,
            SpellId.RejuvenationFellowship6,
            SpellId.RejuvenationFellowship7,
            SpellId.RejuvenationFellowship8,
        };

        public static readonly List<SpellId> PortalSendingIzjiQoGauntlet = new List<SpellId>()
        {
            SpellId.PortalSendingIzjiQoGauntlet60,
            SpellId.PortalSendingIzjiQoGauntlet80,
            SpellId.PortalSendingIzjiQoGauntlet100,
        };

        public static readonly List<SpellId> PortalSendingIzjiQoReceivingChamber = new List<SpellId>()
        {
            SpellId.PortalSendingIzjiQoReceivingChamber,
            SpellId.PortalSendingIzjiQoReceivingChamber1,
            SpellId.PortalSendingIzjiQoReceivingChamber2,
            SpellId.PortalSendingIzjiQoReceivingChamber3,
            SpellId.PortalSendingIzjiQoReceivingChamber4,
            SpellId.PortalSendingIzjiQoReceivingChamber5,
            SpellId.PortalSendingIzjiQoReceivingChamber6,
            SpellId.PortalSendingIzjiQoReceivingChamber7,
        };

        public static readonly List<SpellId> PortalSendingIzjiQoTest = new List<SpellId>()
        {
            SpellId.PortalSendingIzjiQoTest60,
            SpellId.PortalSendingIzjiQoTest80,
            SpellId.PortalSendingIzjiQoTest100,
        };

        public static readonly List<SpellId> ArcanumSalvagingSelf = new List<SpellId>()
        {
            SpellId.ArcanumSalvagingSelf1,
            SpellId.ArcanumSalvagingSelf2,
            SpellId.ArcanumSalvagingSelf3,
            SpellId.ArcanumSalvagingSelf4,
            SpellId.ArcanumSalvagingSelf5,
            SpellId.ArcanumSalvagingSelf6,
            SpellId.ArcanumSalvagingSelf7,
            SpellId.ArcanumSalvagingSelf8,
        };

        public static readonly List<SpellId> ArcanumSalvagingOther = new List<SpellId>()
        {
            SpellId.ArcanumSalvagingOther1,
            SpellId.ArcanumSalvagingOther2,
            SpellId.ArcanumSalvagingOther3,
            SpellId.ArcanumSalvagingOther4,
            SpellId.ArcanumSalvagingOther5,
            SpellId.ArcanumSalvagingOther6,
            SpellId.ArcanumSalvagingOther7,
            SpellId.ArcanumSalvagingOther8,
        };

        public static readonly List<SpellId> NuhmudirasWisdom = new List<SpellId>()
        {
            SpellId.NuhmudirasWisdom1,
            SpellId.NuhmudirasWisdom2,
            SpellId.NuhmudirasWisdom3,
            SpellId.NuhmudirasWisdom4,
            SpellId.NuhmudirasWisdom5,
            SpellId.NuhmudirasWisdom6,
            SpellId.NuhmudirasWisdom7,
            SpellId.NuhmudirasWisdom8,
        };

        public static readonly List<SpellId> NuhmudirasWisdomOther = new List<SpellId>()
        {
            SpellId.NuhmudirasWisdomOther1,
            SpellId.NuhmudirasWisdomOther2,
            SpellId.NuhmudirasWisdomOther3,
            SpellId.NuhmudirasWisdomOther4,
            SpellId.NuhmudirasWisdomOther5,
            SpellId.NuhmudirasWisdomOther6,
            SpellId.NuhmudirasWisdomOther7,
            SpellId.NuhmudirasWisdomOther8,
        };

        public static readonly List<SpellId> Intoxication = new List<SpellId>()
        {
            SpellId.Intoxication1,
            SpellId.Intoxication2,
            SpellId.Intoxication3,
        };

        public static readonly List<SpellId> AxemansBoon = new List<SpellId>()
        {
            SpellId.AxemansBoon,
            SpellId.AxemansBoon3,
        };

        public static readonly List<SpellId> BowmansBoon = new List<SpellId>()
        {
            SpellId.BowmansBoon,
            SpellId.BowmansBoon3,
        };

        public static readonly List<SpellId> ChuckersBoon = new List<SpellId>()
        {
            SpellId.ChuckersBoon,
            SpellId.ChuckersBoon3,
        };

        public static readonly List<SpellId> CrossbowmansBoon = new List<SpellId>()
        {
            SpellId.CrossbowmansBoon,
            SpellId.CrossbowmansBoon3,
        };

        public static readonly List<SpellId> EnchantersBoon = new List<SpellId>()
        {
            SpellId.EnchantersBoon,
            SpellId.EnchantersBoon3,
        };

        public static readonly List<SpellId> HieromancersBoon = new List<SpellId>()
        {
            SpellId.HieromancersBoon,
            SpellId.HieromancersBoon3,
        };

        public static readonly List<SpellId> KnifersBoon = new List<SpellId>()
        {
            SpellId.KnifersBoon,
            SpellId.KnifersBoon3,
        };

        public static readonly List<SpellId> LifeGiversBoon = new List<SpellId>()
        {
            SpellId.LifeGiversBoon,
            SpellId.LifeGiversBoon3,
        };

        public static readonly List<SpellId> MacersBoon = new List<SpellId>()
        {
            SpellId.MacersBoon,
            SpellId.MacersBoon3,
        };

        public static readonly List<SpellId> PugilistsBoon = new List<SpellId>()
        {
            SpellId.PugilistsBoon,
            SpellId.PugilistsBoon3,
        };

        public static readonly List<SpellId> SpearmansBoon = new List<SpellId>()
        {
            SpellId.SpearmansBoon,
            SpellId.SpearmansBoon3,
        };

        public static readonly List<SpellId> StafferBoon = new List<SpellId>()
        {
            SpellId.StafferBoon,
            SpellId.StafferBoon3,
        };

        public static readonly List<SpellId> SwordsmansBoon = new List<SpellId>()
        {
            SpellId.SwordsmansBoon,
            SpellId.SwordsmansBoon3,
        };

        public static readonly List<SpellId> SalvagingMasteryForge = new List<SpellId>()
        {
            SpellId.SalvagingMasteryForge1,
            SpellId.SalvagingMasteryForge2,
        };

        public static readonly List<SpellId> AlchemyMasteryForge = new List<SpellId>()
        {
            SpellId.AlchemyMasteryForge1,
            SpellId.AlchemyMasteryForge2,
        };

        public static readonly List<SpellId> CookingMasteryForge = new List<SpellId>()
        {
            SpellId.CookingMasteryForge1,
            SpellId.CookingMasteryForge2,
        };

        public static readonly List<SpellId> FletchingMasteryForge = new List<SpellId>()
        {
            SpellId.FletchingMasteryForge1,
            SpellId.FletchingMasteryForge2,
        };

        public static readonly List<SpellId> LockpickMasteryForge = new List<SpellId>()
        {
            SpellId.LockpickMasteryForge1,
            SpellId.LockpickMasteryForge2,
        };

        public static readonly List<SpellId> PortalSendingPvPHate20Entry = new List<SpellId>()
        {
            SpellId.PortalSendingPvPHate20Entry1,
            SpellId.PortalSendingPvPHate20Entry2,
            SpellId.PortalSendingPvPHate20Entry3,
            SpellId.PortalSendingPvPHate20Entry4,
            SpellId.PortalSendingPvPHate20Entry5,
            SpellId.PortalSendingPvPHate20Entry6,
        };

        public static readonly List<SpellId> PortalSendingPvPHate40Entry = new List<SpellId>()
        {
            SpellId.PortalSendingPvPHate40Entry1,
            SpellId.PortalSendingPvPHate40Entry2,
            SpellId.PortalSendingPvPHate40Entry3,
            SpellId.PortalSendingPvPHate40Entry4,
            SpellId.PortalSendingPvPHate40Entry5,
            SpellId.PortalSendingPvPHate40Entry6,
        };

        public static readonly List<SpellId> PortalSendingPvPHate60Entry = new List<SpellId>()
        {
            SpellId.PortalSendingPvPHate60Entry1,
            SpellId.PortalSendingPvPHate60Entry2,
            SpellId.PortalSendingPvPHate60Entry3,
            SpellId.PortalSendingPvPHate60Entry4,
            SpellId.PortalSendingPvPHate60Entry5,
            SpellId.PortalSendingPvPHate60Entry6,
        };

        public static readonly List<SpellId> PortalSendingPvPHate80AccursedEntry = new List<SpellId>()
        {
            SpellId.PortalSendingPvPHate80AccursedEntry1,
            SpellId.PortalSendingPvPHate80AccursedEntry2,
            SpellId.PortalSendingPvPHate80AccursedEntry3,
            SpellId.PortalSendingPvPHate80AccursedEntry4,
            SpellId.PortalSendingPvPHate80AccursedEntry5,
            SpellId.PortalSendingPvPHate80AccursedEntry6,
        };

        public static readonly List<SpellId> PortalSendingPvPHate80UnholyEntry = new List<SpellId>()
        {
            SpellId.PortalSendingPvPHate80UnholyEntry1,
            SpellId.PortalSendingPvPHate80UnholyEntry2,
            SpellId.PortalSendingPvPHate80UnholyEntry3,
            SpellId.PortalSendingPvPHate80UnholyEntry4,
            SpellId.PortalSendingPvPHate80UnholyEntry5,
            SpellId.PortalSendingPvPHate80UnholyEntry6,
        };

        public static readonly List<SpellId> CantripSalvaging = new List<SpellId>()
        {
            SpellId.CantripSalvaging1,
            SpellId.CantripSalvaging2,
            SpellId.CANTRIPSALVAGING3,
            SpellId.CantripSalvaging4,
        };

        public static readonly List<SpellId> RecallSonPooky = new List<SpellId>()
        {
            SpellId.RecallSonPooky1,
            SpellId.RecallSonPooky2,
            SpellId.RecallSonPooky3,
        };

        public static readonly List<SpellId> PortalSendingColosseumA = new List<SpellId>()
        {
            SpellId.PortalSendingColosseumA1,
            SpellId.PortalSendingColosseumA6,
        };

        public static readonly List<SpellId> PortalSendingColosseumB = new List<SpellId>()
        {
            SpellId.PortalSendingColosseumB1,
            SpellId.PortalSendingColosseumB6,
        };

        public static readonly List<SpellId> PortalSendingColosseumC = new List<SpellId>()
        {
            SpellId.PortalSendingColosseumC1,
            SpellId.PortalSendingColosseumC6,
        };

        public static readonly List<SpellId> PortalSendingColosseumD = new List<SpellId>()
        {
            SpellId.PortalSendingColosseumD1,
            SpellId.PortalSendingColosseumD6,
        };

        public static readonly List<SpellId> PortalSendingColosseumE = new List<SpellId>()
        {
            SpellId.PortalSendingColosseumE1,
            SpellId.PortalSendingColosseumE6,
        };

        public static readonly List<SpellId> PortalSendingVisionQuestBranch4Stage = new List<SpellId>()
        {
            SpellId.PortalSendingVisionQuestBranch4Stage1,
            SpellId.PortalSendingVisionQuestBranch4Stage2,
            SpellId.PortalSendingVisionQuestBranch4Stage3,
            SpellId.PortalSendingVisionQuestBranch4Stage4,
            SpellId.PortalSendingVisionQuestBranch4Stage6,
        };

        public static readonly List<SpellId> PortalSendingVisionQuestBranch5Stage = new List<SpellId>()
        {
            SpellId.PortalSendingVisionQuestBranch5Stage1,
            SpellId.PortalSendingVisionQuestBranch5Stage2,
            SpellId.PortalSendingVisionQuestBranch5Stage3,
            SpellId.PortalSendingVisionQuestBranch5Stage4,
            SpellId.PortalSendingVisionQuestBranch5Stage6,
        };

        public static readonly List<SpellId> PortalSendingVisionQuestBranch1Stage = new List<SpellId>()
        {
            SpellId.PortalSendingVisionQuestBranch1Stage1,
            SpellId.PortalSendingVisionQuestBranch1Stage2,
            SpellId.PortalSendingVisionQuestBranch1Stage3,
            SpellId.PortalSendingVisionQuestBranch1Stage4,
            SpellId.PortalSendingVisionQuestBranch1Stage6,
        };

        public static readonly List<SpellId> PortalSendingVisionQuestBranch2Stage = new List<SpellId>()
        {
            SpellId.PortalSendingVisionQuestBranch2Stage1,
            SpellId.PortalSendingVisionQuestBranch2Stage2,
            SpellId.PortalSendingVisionQuestBranch2Stage3,
            SpellId.PortalSendingVisionQuestBranch2Stage4,
            SpellId.PortalSendingVisionQuestBranch2Stage6,
        };

        public static readonly List<SpellId> PortalSendingVisionQuestBranch3Stage = new List<SpellId>()
        {
            SpellId.PortalSendingVisionQuestBranch3Stage1,
            SpellId.PortalSendingVisionQuestBranch3Stage2,
            SpellId.PortalSendingVisionQuestBranch3Stage3,
            SpellId.PortalSendingVisionQuestBranch3Stage4,
            SpellId.PortalSendingVisionQuestBranch3Stage6,
        };

        public static readonly List<SpellId> PortalSendDarkCrypt = new List<SpellId>()
        {
            SpellId.PortalSendDarkCrypt1,
            SpellId.PortalSendDarkCrypt2,
            SpellId.PortalSendDarkCrypt3,
        };

        public static readonly List<SpellId> PortalSendJesterPrison = new List<SpellId>()
        {
            SpellId.PortalSendJesterPrison,
            SpellId.PortalSendJesterPrison2,
        };

        public static readonly List<SpellId> RecallJester = new List<SpellId>()
        {
            SpellId.RecallJester1,
            SpellId.RecallJester2,
            SpellId.RecallJester3,
            SpellId.RecallJester4,
            SpellId.RecallJester5,
            SpellId.RecallJester6,
            SpellId.RecallJester7,
            SpellId.RecallJester8,
        };

        public static readonly List<SpellId> PortalSendingCHDSStage = new List<SpellId>()
        {
            SpellId.PortalSendingCHDSStage1,
            SpellId.PortalSendingCHDSStage2,
            SpellId.PortalSendingCHDSStage3,
        };

        public static readonly List<SpellId> MoarsmanPoison = new List<SpellId>()
        {
            SpellId.MoarsmanPoison1,
            SpellId.MoarsmanPoison2,
            SpellId.MoarsmanPoison3,
        };

        public static readonly List<SpellId> SetCoordination = new List<SpellId>()
        {
            SpellId.SetCoordination1,
            SpellId.SetCoordination2,
            SpellId.SetCoordination3,
            SpellId.SetCoordination4,
        };

        public static readonly List<SpellId> SetEndurance = new List<SpellId>()
        {
            SpellId.SetEndurance1,
            SpellId.SetEndurance2,
            SpellId.SetEndurance3,
            SpellId.SetEndurance4,
        };

        public static readonly List<SpellId> SetFocus = new List<SpellId>()
        {
            SpellId.SetFocus1,
            SpellId.SetFocus2,
            SpellId.SetFocus3,
            SpellId.SetFocus4,
        };

        public static readonly List<SpellId> SetQuickness = new List<SpellId>()
        {
            SpellId.SetQuickness1,
            SpellId.SetQuickness2,
            SpellId.SetQuickness3,
            SpellId.SetQuickness4,
        };

        public static readonly List<SpellId> SetStrength = new List<SpellId>()
        {
            SpellId.SetStrength1,
            SpellId.SetStrength2,
            SpellId.SetStrength3,
            SpellId.SetStrength4,
        };

        public static readonly List<SpellId> SetWillpower = new List<SpellId>()
        {
            SpellId.SetWillpower1,
            SpellId.SetWillpower2,
            SpellId.SetWillpower3,
            SpellId.SetWillpower4,
        };

        public static readonly List<SpellId> SetHealth = new List<SpellId>()
        {
            SpellId.SetHealth2,
            SpellId.SetHealth3,
        };

        public static readonly List<SpellId> SetMana = new List<SpellId>()
        {
            SpellId.SetMana2,
            SpellId.SetMana3,
        };

        public static readonly List<SpellId> SetStamina = new List<SpellId>()
        {
            SpellId.SetStamina2,
            SpellId.SetStamina3,
        };

        public static readonly List<SpellId> SetAcidResistance = new List<SpellId>()
        {
            SpellId.SetAcidResistance1,
            SpellId.SetAcidResistance2,
            SpellId.SetAcidResistance3,
            SpellId.SetAcidResistance4,
        };

        public static readonly List<SpellId> SetBludgeonResistance = new List<SpellId>()
        {
            SpellId.SetBludgeonResistance1,
            SpellId.SetBludgeonResistance2,
            SpellId.SetBludgeonResistance3,
            SpellId.SetBludgeonResistance4,
        };

        public static readonly List<SpellId> SetFlameResistance = new List<SpellId>()
        {
            SpellId.SetFlameResistance1,
            SpellId.SetFlameResistance2,
            SpellId.SetFlameResistance3,
            SpellId.SetFlameResistance4,
        };

        public static readonly List<SpellId> SetFrostResistance = new List<SpellId>()
        {
            SpellId.SetFrostResistance1,
            SpellId.SetFrostResistance2,
            SpellId.SetFrostResistance3,
            SpellId.SetFrostResistance4,
        };

        public static readonly List<SpellId> SetLightningResistance = new List<SpellId>()
        {
            SpellId.SetLightningResistance1,
            SpellId.SetLightningResistance2,
            SpellId.SetLightningResistance3,
            SpellId.SetLightningResistance4,
        };

        public static readonly List<SpellId> SetPierceResistance = new List<SpellId>()
        {
            SpellId.SetPierceResistance1,
            SpellId.SetPierceResistance2,
            SpellId.SetPierceResistance3,
            SpellId.SetPierceResistance4,
        };

        public static readonly List<SpellId> SetSlashingResistance = new List<SpellId>()
        {
            SpellId.SetSlashingResistance1,
            SpellId.SetSlashingResistance2,
            SpellId.SetSlashingResistance3,
            SpellId.SetSlashingResistance4,
        };

        public static readonly List<SpellId> SetAlchemyAptitude = new List<SpellId>()
        {
            SpellId.SetAlchemyAptitude1,
            SpellId.SetAlchemyAptitude2,
            SpellId.SetAlchemyAptitude3,
            SpellId.SetAlchemyAptitude4,
        };

        public static readonly List<SpellId> SetArmorExpertiseAptitude = new List<SpellId>()
        {
            SpellId.SetArmorExpertiseAptitude1,
            SpellId.SetArmorExpertiseAptitude2,
            SpellId.SetArmorExpertiseAptitude3,
            SpellId.SetArmorExpertiseAptitude4,
        };

        public static readonly List<SpellId> SetAxeAptitude = new List<SpellId>()
        {
            SpellId.SetAxeAptitude1,
            SpellId.SetAxeAptitude2,
            SpellId.SetAxeAptitude3,
            SpellId.SetAxeAptitude4,
        };

        public static readonly List<SpellId> SetBowAptitude = new List<SpellId>()
        {
            SpellId.SetBowAptitude1,
            SpellId.SetBowAptitude2,
            SpellId.SetBowAptitude3,
            SpellId.SetBowAptitude4,
        };

        public static readonly List<SpellId> SetCookingAptitude = new List<SpellId>()
        {
            SpellId.SetCookingAptitude1,
            SpellId.SetCookingAptitude2,
            SpellId.SetCookingAptitude3,
            SpellId.SetCookingAptitude4,
        };

        public static readonly List<SpellId> SetCreatureEnchantmentAptitude = new List<SpellId>()
        {
            SpellId.SetCreatureEnchantmentAptitude1,
            SpellId.SetCreatureEnchantmentAptitude2,
            SpellId.SetCreatureEnchantmentAptitude3,
            SpellId.SetCreatureEnchantmentAptitude4,
        };

        public static readonly List<SpellId> SetCrossbowAptitude = new List<SpellId>()
        {
            SpellId.SetCrossbowAptitude1,
            SpellId.SetCrossbowAptitude2,
            SpellId.SetCrossbowAptitude3,
            SpellId.SetCrossbowAptitude4,
        };

        public static readonly List<SpellId> SetDaggerAptitude = new List<SpellId>()
        {
            SpellId.SetDaggerAptitude1,
            SpellId.SetDaggerAptitude2,
            SpellId.SetDaggerAptitude3,
            SpellId.SetDaggerAptitude4,
        };

        public static readonly List<SpellId> SetFletchingAptitude = new List<SpellId>()
        {
            SpellId.SetFletchingAptitude1,
            SpellId.SetFletchingAptitude2,
            SpellId.SetFletchingAptitude3,
            SpellId.SetFletchingAptitude4,
        };

        public static readonly List<SpellId> SetItemEnchantmentAptitude = new List<SpellId>()
        {
            SpellId.SetItemEnchantmentAptitude1,
            SpellId.SetItemEnchantmentAptitude2,
            SpellId.SetItemEnchantmentAptitude3,
            SpellId.SetItemEnchantmentAptitude4,
        };

        public static readonly List<SpellId> SetItemExpertiseAptitude = new List<SpellId>()
        {
            SpellId.SetItemExpertiseAptitude1,
            SpellId.SetItemExpertiseAptitude2,
            SpellId.SetItemExpertiseAptitude3,
            SpellId.SetItemExpertiseAptitude4,
        };

        public static readonly List<SpellId> SetJumpingAptitude = new List<SpellId>()
        {
            SpellId.SetJumpingAptitude1,
            SpellId.SetJumpingAptitude2,
            SpellId.SetJumpingAptitude3,
            SpellId.SetJumpingAptitude4,
        };

        public static readonly List<SpellId> SetLifeMagicAptitude = new List<SpellId>()
        {
            SpellId.SetLifeMagicAptitude1,
            SpellId.SetLifeMagicAptitude2,
            SpellId.SetLifeMagicAptitude3,
            SpellId.SetLifeMagicAptitude4,
        };

        public static readonly List<SpellId> SetLockpickAptitude = new List<SpellId>()
        {
            SpellId.SetLockpickAptitude1,
            SpellId.SetLockpickAptitude2,
            SpellId.SetLockpickAptitude3,
            SpellId.SetLockpickAptitude4,
        };

        public static readonly List<SpellId> SetLoyaltyAptitude = new List<SpellId>()
        {
            SpellId.SetLoyaltyAptitude1,
            SpellId.SetLoyaltyAptitude2,
        };

        public static readonly List<SpellId> SetMaceAptitude = new List<SpellId>()
        {
            SpellId.SetMaceAptitude1,
            SpellId.SetMaceAptitude2,
            SpellId.SetMaceAptitude3,
            SpellId.SetMaceAptitude4,
        };

        public static readonly List<SpellId> SetMagicDefenseAptitude = new List<SpellId>()
        {
            SpellId.SetMagicDefenseAptitude1,
            SpellId.SetMagicDefenseAptitude2,
            SpellId.SetMagicDefenseAptitude3,
            SpellId.SetMagicDefenseAptitude4,
        };

        public static readonly List<SpellId> SetMagicItemExpertiseAptitude = new List<SpellId>()
        {
            SpellId.SetMagicItemExpertiseAptitude1,
            SpellId.SetMagicItemExpertiseAptitude2,
            SpellId.SetMagicItemExpertiseAptitude3,
            SpellId.SetMagicItemExpertiseAptitude4,
        };

        public static readonly List<SpellId> SetMeleeDefenseAptitude = new List<SpellId>()
        {
            SpellId.SetMeleeDefenseAptitude1,
            SpellId.SetMeleeDefenseAptitude2,
            SpellId.SetMeleeDefenseAptitude3,
            SpellId.SetMeleeDefenseAptitude4,
        };

        public static readonly List<SpellId> SetMissileDefenseAptitude = new List<SpellId>()
        {
            SpellId.SetMissileDefenseAptitude1,
            SpellId.SetMissileDefenseAptitude2,
            SpellId.SetMissileDefenseAptitude3,
            SpellId.SetMissileDefenseAptitude4,
        };

        public static readonly List<SpellId> SetSalvagingAptitude = new List<SpellId>()
        {
            SpellId.SetSalvagingAptitude1,
            SpellId.SetSalvagingAptitude2,
        };

        public static readonly List<SpellId> SetSpearAptitude = new List<SpellId>()
        {
            SpellId.SetSpearAptitude1,
            SpellId.SetSpearAptitude2,
            SpellId.SetSpearAptitude3,
            SpellId.SetSpearAptitude4,
        };

        public static readonly List<SpellId> SetSprintAptitude = new List<SpellId>()
        {
            SpellId.SetSprintAptitude1,
            SpellId.SetSprintAptitude2,
            SpellId.SetSprintAptitude3,
            SpellId.SetSprintAptitude4,
        };

        public static readonly List<SpellId> SetStaffAptitude = new List<SpellId>()
        {
            SpellId.SetStaffAptitude1,
            SpellId.SetStaffAptitude2,
            SpellId.SetStaffAptitude3,
            SpellId.SetStaffAptitude4,
        };

        public static readonly List<SpellId> SetSwordAptitude = new List<SpellId>()
        {
            SpellId.SetSwordAptitude1,
            SpellId.SetSwordAptitude2,
            SpellId.SetSwordAptitude3,
            SpellId.SetSwordAptitude4,
        };

        public static readonly List<SpellId> SetThrownAptitude = new List<SpellId>()
        {
            SpellId.SetThrownAptitude1,
            SpellId.SetThrownAptitude2,
            SpellId.SetThrownAptitude3,
            SpellId.SetThrownAptitude4,
        };

        public static readonly List<SpellId> SetUnarmedAptitude = new List<SpellId>()
        {
            SpellId.SetUnarmedAptitude1,
            SpellId.SetUnarmedAptitude2,
            SpellId.SetUnarmedAptitude3,
            SpellId.SetUnarmedAptitude4,
        };

        public static readonly List<SpellId> SetWarMagicAptitude = new List<SpellId>()
        {
            SpellId.SetWarMagicAptitude1,
            SpellId.SetWarMagicAptitude2,
            SpellId.SetWarMagicAptitude3,
            SpellId.SetWarMagicAptitude4,
        };

        public static readonly List<SpellId> SetWeaponExpertiseAptitude = new List<SpellId>()
        {
            SpellId.SetWeaponExpertiseAptitude1,
            SpellId.SetWeaponExpertiseAptitude2,
            SpellId.SetWeaponExpertiseAptitude3,
            SpellId.SetWeaponExpertiseAptitude4,
        };

        public static readonly List<SpellId> SetSocietyAttributeAll = new List<SpellId>()
        {
            SpellId.SetSocietyAttributeAll1,
            SpellId.SetSocietyAttributeAll2,
            SpellId.SetSocietyAttributeAll3,
            SpellId.SetSocietyAttributeAll4,
            SpellId.SetSocietyAttributeAll5,
        };

        public static readonly List<SpellId> SetRejuvenation = new List<SpellId>()
        {
            SpellId.SetRejuvenation1,
            SpellId.SetRejuvenation2,
        };

        public static readonly List<SpellId> AcidStream8Spellpower = new List<SpellId>()
        {
            SpellId.AcidStream8Spellpower300,
            SpellId.AcidStream8Spellpower350,
        };

        public static readonly List<SpellId> MiniArcaneDeath = new List<SpellId>()
        {
            SpellId.MiniArcaneDeath,
            SpellId.MiniArcaneDeath2,
            SpellId.MiniArcaneDeath3,
            SpellId.MiniArcaneDeath4,
        };

        public static readonly List<SpellId> MiniFireball = new List<SpellId>()
        {
            SpellId.MiniFireball1,
            SpellId.MiniFireball2,
            SpellId.MiniFireball3,
            SpellId.MiniFireball4,
        };

        public static readonly List<SpellId> MiniIceball = new List<SpellId>()
        {
            SpellId.MiniIceball1,
            SpellId.MiniIceball2,
            SpellId.MiniIceball3,
            SpellId.MiniIceball4,
        };

        public static readonly List<SpellId> MiniArrow = new List<SpellId>()
        {
            SpellId.MiniArrow1,
            SpellId.MiniArrow2,
            SpellId.MiniArrow3,
            SpellId.MiniArrow4,
        };

        public static readonly List<SpellId> MiniRing = new List<SpellId>()
        {
            SpellId.MiniRing1,
            SpellId.MiniRing2,
            SpellId.MiniRing3,
            SpellId.MiniRing4,
        };

        public static readonly List<SpellId> PortalSendingAssassinsRoost = new List<SpellId>()
        {
            SpellId.PortalSendingAssassinsRoost1,
            SpellId.PortalSendingAssassinsRoost2,
            SpellId.PortalSendingAssassinsRoost3,
            SpellId.PortalSendingAssassinsRoost4,
            SpellId.PortalSendingAssassinsRoost5,
        };

        public static readonly List<SpellId> TwoHandedBoon = new List<SpellId>()
        {
            SpellId.TwoHandedBoon,
            SpellId.TwoHandedBoon3,
        };

        public static readonly List<SpellId> TwoHandedMasterySelf = new List<SpellId>()
        {
            SpellId.TwoHandedMasterySelf1,
            SpellId.TwoHandedMasterySelf2,
            SpellId.TwoHandedMasterySelf3,
            SpellId.TwoHandedMasterySelf4,
            SpellId.TwoHandedMasterySelf5,
            SpellId.TwoHandedMasterySelf6,
            SpellId.TwoHandedMasterySelf7,
            SpellId.TwoHandedMasterySelf8,
        };

        public static readonly List<SpellId> CANTRIPGEARCRAFTAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPGEARCRAFTAPTITUDE1,
            SpellId.CANTRIPGEARCRAFTAPTITUDE2,
            SpellId.CANTRIPGEARCRAFTAPTITUDE3,
        };

        public static readonly List<SpellId> CANTRIPTWOHANDEDAPTITUDE = new List<SpellId>()
        {
            SpellId.CANTRIPTWOHANDEDAPTITUDE1,
            SpellId.CANTRIPTWOHANDEDAPTITUDE2,
            SpellId.CANTRIPTWOHANDEDAPTITUDE3,
            SpellId.CantripTwoHandedAptitude4,
        };

        public static readonly List<SpellId> GearcraftIneptitude = new List<SpellId>()
        {
            SpellId.GearcraftIneptitude1,
            SpellId.GearcraftIneptitude2,
            SpellId.GearcraftIneptitude3,
            SpellId.GearcraftIneptitude4,
            SpellId.GearcraftIneptitude5,
            SpellId.GearcraftIneptitude6,
            SpellId.GearcraftIneptitude7,
            SpellId.GearcraftIneptitude8,
        };

        public static readonly List<SpellId> GearcraftIneptitudeSelf = new List<SpellId>()
        {
            SpellId.GearcraftIneptitudeSelf1,
            SpellId.GearcraftIneptitudeSelf2,
            SpellId.GearcraftIneptitudeSelf3,
            SpellId.GearcraftIneptitudeSelf4,
            SpellId.GearcraftIneptitudeSelf5,
            SpellId.GearcraftIneptitudeSelf6,
            SpellId.GearcraftIneptitudeSelf7,
            SpellId.GearcraftIneptitudeSelf8,
        };

        public static readonly List<SpellId> GearcraftMastery = new List<SpellId>()
        {
            SpellId.GearcraftMastery1,
            SpellId.GearcraftMastery2,
            SpellId.GearcraftMastery3,
            SpellId.GearcraftMastery4,
            SpellId.GearcraftMastery5,
            SpellId.GearcraftMastery6,
            SpellId.GearcraftMastery7,
            SpellId.GearcraftMastery8,
        };

        public static readonly List<SpellId> GearcraftMasterySelf = new List<SpellId>()
        {
            SpellId.GearcraftMasterySelf1,
            SpellId.GearcraftMasterySelf2,
            SpellId.GearcraftMasterySelf3,
            SpellId.GearcraftMasterySelf4,
            SpellId.GearcraftMasterySelf5,
            SpellId.GearcraftMasterySelf6,
            SpellId.GearcraftMasterySelf7,
            SpellId.GearcraftMasterySelf8,
        };

        public static readonly List<SpellId> TwoHandedIneptitude = new List<SpellId>()
        {
            SpellId.TwoHandedIneptitude1,
            SpellId.TwoHandedIneptitude2,
            SpellId.TwoHandedIneptitude3,
            SpellId.TwoHandedIneptitude4,
            SpellId.TwoHandedIneptitude5,
            SpellId.TwoHandedIneptitude6,
            SpellId.TwoHandedIneptitude7,
            SpellId.TwoHandedIneptitude8,
        };

        public static readonly List<SpellId> TwoHandedIneptitudeSelf = new List<SpellId>()
        {
            SpellId.TwoHandedIneptitudeSelf1,
            SpellId.TwoHandedIneptitudeSelf2,
            SpellId.TwoHandedIneptitudeSelf3,
            SpellId.TwoHandedIneptitudeSelf4,
            SpellId.TwoHandedIneptitudeSelf5,
            SpellId.TwoHandedIneptitudeSelf6,
            SpellId.TwoHandedIneptitudeSelf7,
            SpellId.TwoHandedIneptitudeSelf8,
        };

        public static readonly List<SpellId> TwoHandedMasteryOther = new List<SpellId>()
        {
            SpellId.TwoHandedMasteryOther1,
            SpellId.TwoHandedMasteryOther2,
            SpellId.TwoHandedMasteryOther3,
            SpellId.TwoHandedMasteryOther4,
            SpellId.TwoHandedMasteryOther5,
            SpellId.TwoHandedMasteryOther6,
            SpellId.TwoHandedMasteryOther7,
            SpellId.TwoHandedMasteryOther8,
        };

        public static readonly List<SpellId> SetGearCraftAptitude = new List<SpellId>()
        {
            SpellId.SetGearCraftAptitude1,
            SpellId.SetGearCraftAptitude2,
            SpellId.SetGearCraftAptitude3,
            SpellId.SetGearCraftAptitude4,
        };

        public static readonly List<SpellId> SetTwoHandedAptitude = new List<SpellId>()
        {
            SpellId.SetTwoHandedAptitude1,
            SpellId.SetTwoHandedAptitude2,
            SpellId.SetTwoHandedAptitude3,
            SpellId.SetTwoHandedAptitude4,
        };

        public static readonly List<SpellId> ExposeWeakness = new List<SpellId>()
        {
            SpellId.ExposeWeakness1,
            SpellId.ExposeWeakness2,
            SpellId.ExposeWeakness3,
            SpellId.ExposeWeakness4,
            SpellId.ExposeWeakness5,
            SpellId.ExposeWeakness6,
            SpellId.ExposeWeakness7,
            SpellId.ExposeWeakness8,
        };

        public static readonly List<SpellId> CallOfLeadership = new List<SpellId>()
        {
            SpellId.CallOfLeadership1,
            SpellId.CallOfLeadership2,
            SpellId.CallOfLeadership3,
            SpellId.CallOfLeadership4,
            SpellId.CallOfLeadership5,
        };

        public static readonly List<SpellId> AnswerOfLoyaltyMana = new List<SpellId>()
        {
            SpellId.AnswerOfLoyaltyMana1,
            SpellId.AnswerOfLoyaltyMana2,
            SpellId.AnswerOfLoyaltyMana3,
            SpellId.AnswerOfLoyaltyMana4,
            SpellId.AnswerOfLoyaltyMana5,
        };

        public static readonly List<SpellId> AnswerOfLoyaltyStam = new List<SpellId>()
        {
            SpellId.AnswerOfLoyaltyStam1,
            SpellId.AnswerOfLoyaltyStam2,
            SpellId.AnswerOfLoyaltyStam3,
            SpellId.AnswerOfLoyaltyStam4,
            SpellId.AnswerOfLoyaltyStam5,
        };

        public static readonly List<SpellId> TrinketXPBoost = new List<SpellId>()
        {
            SpellId.TrinketXPBoost1,
            SpellId.TrinketXPBoost2,
            SpellId.TrinketXPBoost3,
        };

        public static readonly List<SpellId> TrinketDamageBoost = new List<SpellId>()
        {
            SpellId.TrinketDamageBoost1,
            SpellId.TrinketDamageBoost2,
            SpellId.TrinketDamageBoost3,
        };

        public static readonly List<SpellId> TrinketDamageReduction = new List<SpellId>()
        {
            SpellId.TrinketDamageReduction1,
            SpellId.TrinketDamageReduction2,
            SpellId.TrinketDamageReduction3,
        };

        public static readonly List<SpellId> TrinketHealth = new List<SpellId>()
        {
            SpellId.TrinketHealth1,
            SpellId.TrinketHealth2,
            SpellId.TrinketHealth3,
        };

        public static readonly List<SpellId> TrinketMana = new List<SpellId>()
        {
            SpellId.TrinketMana1,
            SpellId.TrinketMana2,
            SpellId.TrinketMana3,
        };

        public static readonly List<SpellId> TrinketStamina = new List<SpellId>()
        {
            SpellId.TrinketStamina1,
            SpellId.TrinketStamina2,
            SpellId.TrinketStamina3,
        };

        public static readonly List<SpellId> DeceptionArcane = new List<SpellId>()
        {
            SpellId.DeceptionArcane1,
            SpellId.DeceptionArcane2,
            SpellId.DeceptionArcane3,
            SpellId.DeceptionArcane4,
            SpellId.DeceptionArcane5,
        };

        public static readonly List<SpellId> SpectralFountain_PortalMaze = new List<SpellId>()
        {
            SpellId.SpectralFountain_PortalMaze1,
            SpellId.SpectralFountain_PortalMaze2,
        };

        public static readonly List<SpellId> RareDamageBoost = new List<SpellId>()
        {
            SpellId.RareDamageBoost1,
            SpellId.RareDamageBoost2,
            SpellId.RareDamageBoost3,
            SpellId.RareDamageBoost4,
            SpellId.RareDamageBoost5,
            SpellId.RareDamageBoost6,
            SpellId.RareDamageBoost7,
            SpellId.RareDamageBoost8,
            SpellId.RareDamageBoost9,
            SpellId.RareDamageBoost10,
        };

        public static readonly List<SpellId> RareDamageReduction = new List<SpellId>()
        {
            SpellId.RareDamageReduction1,
            SpellId.RareDamageReduction2,
            SpellId.RareDamageReduction3,
            SpellId.RareDamageReduction4,
            SpellId.RareDamageReduction5,
            SpellId.RareDamageReduction6,
            SpellId.RareDamageReduction7,
            SpellId.RareDamageReduction8,
            SpellId.RareDamageReduction9,
            SpellId.RareDamageReduction10,
        };

        public static readonly List<SpellId> AetheriaCriticalDamageBoost = new List<SpellId>()
        {
            SpellId.AetheriaCriticalDamageBoost1,
            SpellId.AetheriaCriticalDamageBoost2,
            SpellId.AetheriaCriticalDamageBoost3,
            SpellId.AetheriaCriticalDamageBoost4,
            SpellId.AetheriaCriticalDamageBoost5,
            SpellId.AetheriaCriticalDamageBoost6,
            SpellId.AetheriaCriticalDamageBoost7,
            SpellId.AetheriaCriticalDamageBoost8,
            SpellId.AetheriaCriticalDamageBoost9,
            SpellId.AetheriaCriticalDamageBoost10,
            SpellId.AetheriaCriticalDamageBoost11,
            SpellId.AetheriaCriticalDamageBoost12,
            SpellId.AetheriaCriticalDamageBoost13,
            SpellId.AetheriaCriticalDamageBoost14,
            SpellId.AetheriaCriticalDamageBoost15,
        };

        public static readonly List<SpellId> AetheriaDamageBoost = new List<SpellId>()
        {
            SpellId.AetheriaDamageBoost1,
            SpellId.AetheriaDamageBoost2,
            SpellId.AetheriaDamageBoost3,
            SpellId.AetheriaDamageBoost4,
            SpellId.AetheriaDamageBoost5,
            SpellId.AetheriaDamageBoost6,
            SpellId.AetheriaDamageBoost7,
            SpellId.AetheriaDamageBoost8,
            SpellId.AetheriaDamageBoost9,
            SpellId.AetheriaDamageBoost10,
            SpellId.AetheriaDamageBoost11,
            SpellId.AetheriaDamageBoost12,
            SpellId.AetheriaDamageBoost13,
            SpellId.AetheriaDamageBoost14,
            SpellId.AetheriaDamageBoost15,
        };

        public static readonly List<SpellId> AetheriaDamageReduction = new List<SpellId>()
        {
            SpellId.AetheriaDamageReduction1,
            SpellId.AetheriaDamageReduction2,
            SpellId.AetheriaDamageReduction3,
            SpellId.AetheriaDamageReduction4,
            SpellId.AetheriaDamageReduction5,
            SpellId.AetheriaDamageReduction6,
            SpellId.AetheriaDamageReduction7,
            SpellId.AetheriaDamageReduction8,
            SpellId.AetheriaDamageReduction9,
            SpellId.AetheriaDamageReduction10,
            SpellId.AetheriaDamageReduction11,
            SpellId.AetheriaDamageReduction12,
            SpellId.AetheriaDamageReduction13,
            SpellId.AetheriaDamageReduction14,
            SpellId.AetheriaDamageReduction15,
        };

        public static readonly List<SpellId> AetheriaHealBuff = new List<SpellId>()
        {
            SpellId.AetheriaHealBuff1,
            SpellId.AetheriaHealBuff2,
            SpellId.AetheriaHealBuff3,
            SpellId.AetheriaHealBuff4,
            SpellId.AetheriaHealBuff5,
            SpellId.AetheriaHealBuff6,
            SpellId.AetheriaHealBuff7,
            SpellId.AetheriaHealBuff8,
            SpellId.AetheriaHealBuff9,
            SpellId.AetheriaHealBuff10,
            SpellId.AetheriaHealBuff11,
            SpellId.AetheriaHealBuff12,
            SpellId.AetheriaHealBuff13,
            SpellId.AetheriaHealBuff14,
            SpellId.AetheriaHealBuff15,
        };

        public static readonly List<SpellId> AetheriaHealth = new List<SpellId>()
        {
            SpellId.AetheriaHealth1,
            SpellId.AetheriaHealth2,
            SpellId.AetheriaHealth3,
            SpellId.AetheriaHealth4,
            SpellId.AetheriaHealth5,
            SpellId.AetheriaHealth6,
            SpellId.AetheriaHealth7,
            SpellId.AetheriaHealth8,
            SpellId.AetheriaHealth9,
            SpellId.AetheriaHealth10,
            SpellId.AetheriaHealth11,
            SpellId.AetheriaHealth12,
            SpellId.AetheriaHealth13,
            SpellId.AetheriaHealth14,
            SpellId.AetheriaHealth15,
        };

        public static readonly List<SpellId> AetheriaMana = new List<SpellId>()
        {
            SpellId.AetheriaMana1,
            SpellId.AetheriaMana2,
            SpellId.AetheriaMana3,
            SpellId.AetheriaMana4,
            SpellId.AetheriaMana5,
            SpellId.AetheriaMana6,
            SpellId.AetheriaMana7,
            SpellId.AetheriaMana8,
            SpellId.AetheriaMana9,
            SpellId.AetheriaMana10,
            SpellId.AetheriaMana11,
            SpellId.AetheriaMana12,
            SpellId.AetheriaMana13,
            SpellId.AetheriaMana14,
            SpellId.AetheriaMana15,
        };

        public static readonly List<SpellId> AetheriaStamina = new List<SpellId>()
        {
            SpellId.AetheriaStamina1,
            SpellId.AetheriaStamina2,
            SpellId.AetheriaStamina3,
            SpellId.AetheriaStamina4,
            SpellId.AetheriaStamina5,
            SpellId.AetheriaStamina6,
            SpellId.AetheriaStamina7,
            SpellId.AetheriaStamina8,
            SpellId.AetheriaStamina9,
            SpellId.AetheriaStamina10,
            SpellId.AetheriaStamina11,
            SpellId.AetheriaStamina12,
            SpellId.AetheriaStamina13,
            SpellId.AetheriaStamina14,
            SpellId.AetheriaStamina15,
        };

        public static readonly List<SpellId> AetheriaEndurance = new List<SpellId>()
        {
            SpellId.AetheriaEndurance1,
            SpellId.AetheriaEndurance2,
            SpellId.AetheriaEndurance3,
            SpellId.AetheriaEndurance4,
            SpellId.AetheriaEndurance5,
            SpellId.AetheriaEndurance6,
            SpellId.AetheriaEndurance7,
            SpellId.AetheriaEndurance8,
            SpellId.AetheriaEndurance9,
            SpellId.AetheriaEndurance10,
            SpellId.AetheriaEndurance11,
            SpellId.AetheriaEndurance12,
            SpellId.AetheriaEndurance13,
            SpellId.AetheriaEndurance14,
            SpellId.AetheriaEndurance15,
        };

        public static readonly List<SpellId> BaelzharonsCurseDestruction = new List<SpellId>()
        {
            SpellId.BaelzharonsCurseDestruction,
            SpellId.BaelzharonsCurseDestruction2,
        };

        public static readonly List<SpellId> CurseDestructionOther = new List<SpellId>()
        {
            SpellId.CurseDestructionOther1,
            SpellId.CurseDestructionOther2,
            SpellId.CurseDestructionOther3,
            SpellId.CurseDestructionOther4,
            SpellId.CurseDestructionOther5,
            SpellId.CurseDestructionOther6,
            SpellId.CurseDestructionOther7,
            SpellId.CurseDestructionOther8,
        };

        public static readonly List<SpellId> NetherStreak = new List<SpellId>()
        {
            SpellId.NetherStreak1,
            SpellId.NetherStreak2,
            SpellId.NetherStreak3,
            SpellId.NetherStreak4,
            SpellId.NetherStreak5,
            SpellId.NetherStreak6,
            SpellId.NetherStreak7,
            SpellId.NetherStreak8,
        };

        public static readonly List<SpellId> NetherBolt = new List<SpellId>()
        {
            SpellId.NetherBolt1,
            SpellId.NetherBolt2,
            SpellId.NetherBolt3,
            SpellId.NetherBolt4,
            SpellId.NetherBolt5,
            SpellId.NetherBolt6,
            SpellId.NetherBolt7,
            SpellId.NetherBolt8,
        };

        public static readonly List<SpellId> NetherArc = new List<SpellId>()
        {
            SpellId.NetherArc1,
            SpellId.NetherArc2,
            SpellId.NetherArc3,
            SpellId.NetherArc4,
            SpellId.NetherArc5,
            SpellId.NetherArc6,
            SpellId.NetherArc7,
            SpellId.NetherArc8,
        };

        public static readonly List<SpellId> CurseFestering = new List<SpellId>()
        {
            SpellId.CurseFestering1,
            SpellId.CurseFestering2,
            SpellId.CurseFestering3,
            SpellId.CurseFestering4,
            SpellId.CurseFestering5,
            SpellId.CurseFestering6,
            SpellId.CurseFestering7,
            SpellId.CurseFestering8,
        };

        public static readonly List<SpellId> CurseWeakness = new List<SpellId>()
        {
            SpellId.CurseWeakness1,
            SpellId.CurseWeakness2,
            SpellId.CurseWeakness3,
            SpellId.CurseWeakness4,
            SpellId.CurseWeakness5,
            SpellId.CurseWeakness6,
            SpellId.CurseWeakness7,
            SpellId.CurseWeakness8,
        };

        public static readonly List<SpellId> Corrosion = new List<SpellId>()
        {
            SpellId.Corrosion1,
            SpellId.Corrosion2,
            SpellId.Corrosion3,
            SpellId.Corrosion4,
            SpellId.Corrosion5,
            SpellId.Corrosion6,
            SpellId.Corrosion7,
            SpellId.Corrosion8,
        };

        public static readonly List<SpellId> Corruption = new List<SpellId>()
        {
            SpellId.Corruption1,
            SpellId.Corruption2,
            SpellId.Corruption3,
            SpellId.Corruption4,
            SpellId.Corruption5,
            SpellId.Corruption6,
            SpellId.Corruption7,
            SpellId.Corruption8,
        };

        public static readonly List<SpellId> VoidMagicMasteryOther = new List<SpellId>()
        {
            SpellId.VoidMagicMasteryOther1,
            SpellId.VoidMagicMasteryOther2,
            SpellId.VoidMagicMasteryOther3,
            SpellId.VoidMagicMasteryOther4,
            SpellId.VoidMagicMasteryOther5,
            SpellId.VoidMagicMasteryOther6,
            SpellId.VoidMagicMasteryOther7,
            SpellId.VoidMagicMasteryOther8,
        };

        public static readonly List<SpellId> VoidMagicMasterySelf = new List<SpellId>()
        {
            SpellId.VoidMagicMasterySelf1,
            SpellId.VoidMagicMasterySelf2,
            SpellId.VoidMagicMasterySelf3,
            SpellId.VoidMagicMasterySelf4,
            SpellId.VoidMagicMasterySelf5,
            SpellId.VoidMagicMasterySelf6,
            SpellId.VoidMagicMasterySelf7,
            SpellId.VoidMagicMasterySelf8,
        };

        public static readonly List<SpellId> VoidMagicIneptitudeOther = new List<SpellId>()
        {
            SpellId.VoidMagicIneptitudeOther1,
            SpellId.VoidMagicIneptitudeOther2,
            SpellId.VoidMagicIneptitudeOther3,
            SpellId.VoidMagicIneptitudeOther4,
            SpellId.VoidMagicIneptitudeOther5,
            SpellId.VoidMagicIneptitudeOther6,
            SpellId.VoidMagicIneptitudeOther7,
            SpellId.VoidMagicIneptitudeOther8,
        };

        public static readonly List<SpellId> CantripVoidMagicAptitude = new List<SpellId>()
        {
            SpellId.CantripVoidMagicAptitude1,
            SpellId.CantripVoidMagicAptitude2,
            SpellId.CantripVoidMagicAptitude3,
            SpellId.CantripVoidMagicAptitude4,
        };

        public static readonly List<SpellId> SetVoidMagicAptitude = new List<SpellId>()
        {
            SpellId.SetVoidMagicAptitude1,
            SpellId.SetVoidMagicAptitude2,
            SpellId.SetVoidMagicAptitude3,
            SpellId.SetVoidMagicAptitude4,
        };

        public static readonly List<SpellId> CorruptorsBoon = new List<SpellId>()
        {
            SpellId.CorruptorsBoon,
            SpellId.CorruptorsBoon3,
        };

        public static readonly List<SpellId> AcidSpitStreak = new List<SpellId>()
        {
            SpellId.AcidSpitStreak1,
            SpellId.AcidSpitStreak2,
        };

        public static readonly List<SpellId> AcidSpit = new List<SpellId>()
        {
            SpellId.AcidSpit1,
            SpellId.AcidSpit2,
        };

        public static readonly List<SpellId> AcidSpitArc = new List<SpellId>()
        {
            SpellId.AcidSpitArc1,
            SpellId.AcidSpitArc2,
        };

        public static readonly List<SpellId> AcidSpitBlast = new List<SpellId>()
        {
            SpellId.AcidSpitBlast1,
            SpellId.AcidspitBlast2,
        };

        public static readonly List<SpellId> AcidSpitVolley = new List<SpellId>()
        {
            SpellId.AcidSpitVolley1,
            SpellId.AcidSpitVolley2,
        };

        public static readonly List<SpellId> OlthoiCriticalDamageBoost = new List<SpellId>()
        {
            SpellId.OlthoiCriticalDamageBoost1,
            SpellId.OlthoiCriticalDamageBoost2,
            SpellId.OlthoiCriticalDamageBoost3,
            SpellId.OlthoiCriticalDamageBoost4,
            SpellId.OlthoiCriticalDamageBoost5,
            SpellId.OlthoiCriticalDamageBoost6,
            SpellId.OlthoiCriticalDamageBoost7,
            SpellId.OlthoiCriticalDamageBoost8,
            SpellId.OlthoiCriticalDamageBoost9,
            SpellId.OlthoiCriticalDamageBoost10,
            SpellId.OlthoiCriticalDamageBoost11,
        };

        public static readonly List<SpellId> OlthoiCriticalDamageReduction = new List<SpellId>()
        {
            SpellId.OlthoiCriticalDamageReduction1,
            SpellId.OlthoiCriticalDamageReduction2,
            SpellId.OlthoiCriticalDamageReduction3,
            SpellId.OlthoiCriticalDamageReduction4,
            SpellId.OlthoiCriticalDamageReduction5,
            SpellId.OlthoiCriticalDamageReduction6,
            SpellId.OlthoiCriticalDamageReduction7,
            SpellId.OlthoiCriticalDamageReduction8,
            SpellId.OlthoiCriticalDamageReduction9,
            SpellId.OlthoiCriticalDamageReduction10,
            SpellId.OlthoiCriticalDamageReduction11,
        };

        public static readonly List<SpellId> OlthoiDamageBoost = new List<SpellId>()
        {
            SpellId.OlthoiDamageBoost1,
            SpellId.OlthoiDamageBoost2,
            SpellId.OlthoiDamageBoost3,
            SpellId.OlthoiDamageBoost4,
            SpellId.OlthoiDamageBoost5,
            SpellId.OlthoiDamageBoost6,
            SpellId.OlthoiDamageBoost7,
            SpellId.OlthoiDamageBoost8,
            SpellId.OlthoiDamageBoost9,
            SpellId.OlthoiDamageBoost10,
            SpellId.OlthoiDamageBoost11,
        };

        public static readonly List<SpellId> OlthoiDamageReduction = new List<SpellId>()
        {
            SpellId.OlthoiDamageReduction1,
            SpellId.OlthoiDamageReduction2,
            SpellId.OlthoiDamageReduction3,
            SpellId.OlthoiDamageReduction4,
            SpellId.OlthoiDamageReduction5,
            SpellId.OlthoiDamageReduction6,
            SpellId.OlthoiDamageReduction7,
            SpellId.OlthoiDamageReduction8,
            SpellId.OlthoiDamageReduction9,
            SpellId.OlthoiDamageReduction10,
            SpellId.OlthoiDamageReduction11,
        };

        public static readonly List<SpellId> AcidSpitVulnerability = new List<SpellId>()
        {
            SpellId.AcidSpitVulnerability1,
            SpellId.AcidSpitVulnerability2,
        };

        public static readonly List<SpellId> BloodstoneBolt = new List<SpellId>()
        {
            SpellId.BloodstoneBolt1,
            SpellId.BloodstoneBolt2,
            SpellId.BloodstoneBolt3,
            SpellId.BloodstoneBolt4,
            SpellId.BloodstoneBolt5,
            SpellId.BloodstoneBolt6,
            SpellId.BloodstoneBolt7,
            SpellId.BloodstoneBolt8,
        };

        public static readonly List<SpellId> PortalSendingBloodstoneFactory = new List<SpellId>()
        {
            SpellId.PortalSendingBloodstoneFactory1,
            SpellId.PortalSendingBloodstoneFactory2,
        };

        public static readonly List<SpellId> PortalSendingRitualTime = new List<SpellId>()
        {
            SpellId.PortalSendingRitualTime1,
            SpellId.PortalSendingRitualTime2,
        };

        public static readonly List<SpellId> NetherBlast = new List<SpellId>()
        {
            SpellId.NetherBlast1,
            SpellId.NetherBlast2,
            SpellId.NetherBlast3,
            SpellId.NetherBlast4,
            SpellId.NetherBlast5,
            SpellId.NetherBlast6,
            SpellId.NetherBlast7,
            SpellId.NetherBlast8,
        };

        public static readonly List<SpellId> AetheriaDoTResistance = new List<SpellId>()
        {
            SpellId.AetheriaDoTResistance1,
            SpellId.AetheriaDoTResistance2,
            SpellId.AetheriaDoTResistance3,
            SpellId.AetheriaDoTResistance4,
            SpellId.AetheriaDoTResistance5,
            SpellId.AetheriaDoTResistance6,
            SpellId.AetheriaDoTResistance7,
            SpellId.AetheriaDoTResistance8,
            SpellId.AetheriaDoTResistance9,
            SpellId.AetheriaDoTResistance10,
            SpellId.AetheriaDoTResistance11,
            SpellId.AetheriaDoTResistance12,
            SpellId.AetheriaDoTResistance13,
            SpellId.AetheriaDoTResistance14,
            SpellId.AetheriaDoTResistance15,
        };

        public static readonly List<SpellId> AetheriaHealthResistance = new List<SpellId>()
        {
            SpellId.AetheriaHealthResistance1,
            SpellId.AetheriaHealthResistance2,
            SpellId.AetheriaHealthResistance3,
            SpellId.AetheriaHealthResistance4,
            SpellId.AetheriaHealthResistance5,
            SpellId.AetheriaHealthResistance6,
            SpellId.AetheriaHealthResistance7,
            SpellId.AetheriaHealthResistance8,
            SpellId.AetheriaHealthResistance9,
            SpellId.AetheriaHealthResistance10,
            SpellId.AetheriaHealthResistance11,
            SpellId.AetheriaHealthResistance12,
            SpellId.AetheriaHealthResistance13,
            SpellId.AetheriaHealthResistance14,
            SpellId.AetheriaHealthResistance15,
        };

        public static readonly List<SpellId> CloakAlchemyMastery = new List<SpellId>()
        {
            SpellId.CloakAlchemyMastery1,
            SpellId.CloakAlchemyMastery2,
            SpellId.CloakAlchemyMastery3,
            SpellId.CloakAlchemyMastery4,
            SpellId.CloakAlchemyMastery5,
        };

        public static readonly List<SpellId> CloakArcaneloreMastery = new List<SpellId>()
        {
            SpellId.CloakArcaneloreMastery1,
            SpellId.CloakArcaneloreMastery2,
            SpellId.CloakArcaneloreMastery3,
            SpellId.CloakArcaneloreMastery4,
            SpellId.CloakArcaneloreMastery5,
        };

        public static readonly List<SpellId> CloakArmortinkeringMastery = new List<SpellId>()
        {
            SpellId.CloakArmortinkeringMastery1,
            SpellId.CloakArmortinkeringMastery2,
            SpellId.CloakArmortinkeringMastery3,
            SpellId.CloakArmortinkeringMastery4,
            SpellId.CloakArmortinkeringMastery5,
        };

        public static readonly List<SpellId> CloakAssesspersonMastery = new List<SpellId>()
        {
            SpellId.CloakAssesspersonMastery1,
            SpellId.CloakAssesspersonMastery2,
            SpellId.CloakAssesspersonMastery3,
            SpellId.CloakAssesspersonMastery4,
            SpellId.CloakAssesspersonMastery5,
        };

        public static readonly List<SpellId> CloakAxeMastery = new List<SpellId>()
        {
            SpellId.CloakAxeMastery1,
            SpellId.CloakAxeMastery2,
            SpellId.CloakAxeMastery3,
            SpellId.CloakAxeMastery4,
            SpellId.CloakAxeMastery5,
        };

        public static readonly List<SpellId> CloakBowMastery = new List<SpellId>()
        {
            SpellId.CloakBowMastery1,
            SpellId.CloakBowMastery2,
            SpellId.CloakBowMastery3,
            SpellId.CloakBowMastery4,
            SpellId.CloakBowMastery5,
        };

        public static readonly List<SpellId> CloakCookingMastery = new List<SpellId>()
        {
            SpellId.CloakCookingMastery1,
            SpellId.CloakCookingMastery2,
            SpellId.CloakCookingMastery3,
            SpellId.CloakCookingMastery4,
            SpellId.CloakCookingMastery5,
        };

        public static readonly List<SpellId> CloakCreatureenchantmentMastery = new List<SpellId>()
        {
            SpellId.CloakCreatureenchantmentMastery1,
            SpellId.CloakCreatureenchantmentMastery2,
            SpellId.CloakCreatureenchantmentMastery3,
            SpellId.CloakCreatureenchantmentMastery4,
            SpellId.CloakCreatureenchantmentMastery5,
        };

        public static readonly List<SpellId> CloakCrossbowMastery = new List<SpellId>()
        {
            SpellId.CloakCrossbowMastery1,
            SpellId.CloakCrossbowMastery2,
            SpellId.CloakCrossbowMastery3,
            SpellId.CloakCrossbowMastery4,
            SpellId.CloakCrossbowMastery5,
        };

        public static readonly List<SpellId> CloakDaggerMastery = new List<SpellId>()
        {
            SpellId.CloakDaggerMastery1,
            SpellId.CloakDaggerMastery2,
            SpellId.CloakDaggerMastery3,
            SpellId.CloakDaggerMastery4,
            SpellId.CloakDaggerMastery5,
        };

        public static readonly List<SpellId> CloakDeceptionMastery = new List<SpellId>()
        {
            SpellId.CloakDeceptionMastery1,
            SpellId.CloakDeceptionMastery2,
            SpellId.CloakDeceptionMastery3,
            SpellId.CloakDeceptionMastery4,
            SpellId.CloakDeceptionMastery5,
        };

        public static readonly List<SpellId> CloakFletchingMastery = new List<SpellId>()
        {
            SpellId.CloakFletchingMastery1,
            SpellId.CloakFletchingMastery2,
            SpellId.CloakFletchingMastery3,
            SpellId.CloakFletchingMastery4,
            SpellId.CloakFletchingMastery5,
        };

        public static readonly List<SpellId> CloakHealingMastery = new List<SpellId>()
        {
            SpellId.CloakHealingMastery1,
            SpellId.CloakHealingMastery2,
            SpellId.CloakHealingMastery3,
            SpellId.CloakHealingMastery4,
            SpellId.CloakHealingMastery5,
        };

        public static readonly List<SpellId> CloakItemenchantmentMastery = new List<SpellId>()
        {
            SpellId.CloakItemenchantmentMastery1,
            SpellId.CloakItemenchantmentMastery2,
            SpellId.CloakItemenchantmentMastery3,
            SpellId.CloakItemenchantmentMastery4,
            SpellId.CloakItemenchantmentMastery5,
        };

        public static readonly List<SpellId> CloakItemtinkeringMastery = new List<SpellId>()
        {
            SpellId.CloakItemtinkeringMastery1,
            SpellId.CloakItemtinkeringMastery2,
            SpellId.CloakItemtinkeringMastery3,
            SpellId.CloakItemtinkeringMastery4,
            SpellId.CloakItemtinkeringMastery5,
        };

        public static readonly List<SpellId> CloakLeadershipMastery = new List<SpellId>()
        {
            SpellId.CloakLeadershipMastery1,
            SpellId.CloakLeadershipMastery2,
            SpellId.CloakLeadershipMastery3,
            SpellId.CloakLeadershipMastery4,
            SpellId.CloakLeadershipMastery5,
        };

        public static readonly List<SpellId> CloakLifemagicMastery = new List<SpellId>()
        {
            SpellId.CloakLifemagicMastery1,
            SpellId.CloakLifemagicMastery2,
            SpellId.CloakLifemagicMastery3,
            SpellId.CloakLifemagicMastery4,
            SpellId.CloakLifemagicMastery5,
        };

        public static readonly List<SpellId> CloakLoyaltyMastery = new List<SpellId>()
        {
            SpellId.CloakLoyaltyMastery1,
            SpellId.CloakLoyaltyMastery2,
            SpellId.CloakLoyaltyMastery3,
            SpellId.CloakLoyaltyMastery4,
            SpellId.CloakLoyaltyMastery5,
        };

        public static readonly List<SpellId> CloakMaceMastery = new List<SpellId>()
        {
            SpellId.CloakMaceMastery1,
            SpellId.CloakMaceMastery2,
            SpellId.CloakMaceMastery3,
            SpellId.CloakMaceMastery4,
            SpellId.CloakMaceMastery5,
        };

        public static readonly List<SpellId> CloakMagicdefenseMastery = new List<SpellId>()
        {
            SpellId.CloakMagicdefenseMastery1,
            SpellId.CloakMagicdefenseMastery2,
            SpellId.CloakMagicdefenseMastery3,
            SpellId.CloakMagicdefenseMastery4,
            SpellId.CloakMagicdefenseMastery5,
        };

        public static readonly List<SpellId> CloakMagictinkeringMastery = new List<SpellId>()
        {
            SpellId.CloakMagictinkeringMastery1,
            SpellId.CloakMagictinkeringMastery2,
            SpellId.CloakMagictinkeringMastery3,
            SpellId.CloakMagictinkeringMastery4,
            SpellId.CloakMagictinkeringMastery5,
        };

        public static readonly List<SpellId> CloakManaconversionMastery = new List<SpellId>()
        {
            SpellId.CloakManaconversionMastery1,
            SpellId.CloakManaconversionMastery2,
            SpellId.CloakManaconversionMastery3,
            SpellId.CloakManaconversionMastery4,
            SpellId.CloakManaconversionMastery5,
        };

        public static readonly List<SpellId> CloakMeleedefenseMastery = new List<SpellId>()
        {
            SpellId.CloakMeleedefenseMastery1,
            SpellId.CloakMeleedefenseMastery2,
            SpellId.CloakMeleedefenseMastery3,
            SpellId.CloakMeleedefenseMastery4,
            SpellId.CloakMeleedefenseMastery5,
        };

        public static readonly List<SpellId> CloakMissiledefenseMastery = new List<SpellId>()
        {
            SpellId.CloakMissiledefenseMastery1,
            SpellId.CloakMissiledefenseMastery2,
            SpellId.CloakMissiledefenseMastery3,
            SpellId.CloakMissiledefenseMastery4,
            SpellId.CloakMissiledefenseMastery5,
        };

        public static readonly List<SpellId> CloakSalvagingMastery = new List<SpellId>()
        {
            SpellId.CloakSalvagingMastery1,
            SpellId.CloakSalvagingMastery2,
            SpellId.CloakSalvagingMastery3,
            SpellId.CloakSalvagingMastery4,
            SpellId.CloakSalvagingMastery5,
        };

        public static readonly List<SpellId> CloakSpearMastery = new List<SpellId>()
        {
            SpellId.CloakSpearMastery1,
            SpellId.CloakSpearMastery2,
            SpellId.CloakSpearMastery3,
            SpellId.CloakSpearMastery4,
            SpellId.CloakSpearMastery5,
        };

        public static readonly List<SpellId> CloakStaffMastery = new List<SpellId>()
        {
            SpellId.CloakStaffMastery1,
            SpellId.CloakStaffMastery2,
            SpellId.CloakStaffMastery3,
            SpellId.CloakStaffMastery4,
            SpellId.CloakStaffMastery5,
        };

        public static readonly List<SpellId> CloakSwordMastery = new List<SpellId>()
        {
            SpellId.CloakSwordMastery1,
            SpellId.CloakSwordMastery2,
            SpellId.CloakSwordMastery3,
            SpellId.CloakSwordMastery4,
            SpellId.CloakSwordMastery5,
        };

        public static readonly List<SpellId> CloakThrownWeaponMastery = new List<SpellId>()
        {
            SpellId.CloakThrownWeaponMastery1,
            SpellId.CloakThrownWeaponMastery2,
            SpellId.CloakThrownWeaponMastery3,
            SpellId.CloakThrownWeaponMastery4,
            SpellId.CloakThrownWeaponMastery5,
        };

        public static readonly List<SpellId> CloakTwoHandedCombatMastery = new List<SpellId>()
        {
            SpellId.CloakTwoHandedCombatMastery1,
            SpellId.CloakTwoHandedCombatMastery2,
            SpellId.CloakTwoHandedCombatMastery3,
            SpellId.CloakTwoHandedCombatMastery4,
            SpellId.CloakTwoHandedCombatMastery5,
        };

        public static readonly List<SpellId> CloakUnarmedCombatMastery = new List<SpellId>()
        {
            SpellId.CloakUnarmedCombatMastery1,
            SpellId.CloakUnarmedCombatMastery2,
            SpellId.CloakUnarmedCombatMastery3,
            SpellId.CloakUnarmedCombatMastery4,
            SpellId.CloakUnarmedCombatMastery5,
        };

        public static readonly List<SpellId> CloakVoidMagicMastery = new List<SpellId>()
        {
            SpellId.CloakVoidMagicMastery1,
            SpellId.CloakVoidMagicMastery2,
            SpellId.CloakVoidMagicMastery3,
            SpellId.CloakVoidMagicMastery4,
            SpellId.CloakVoidMagicMastery5,
        };

        public static readonly List<SpellId> CloakWarMagicMastery = new List<SpellId>()
        {
            SpellId.CloakWarMagicMastery1,
            SpellId.CloakWarMagicMastery2,
            SpellId.CloakWarMagicMastery3,
            SpellId.CloakWarMagicMastery4,
            SpellId.CloakWarMagicMastery5,
        };

        public static readonly List<SpellId> CloakWeapontinkeringMastery = new List<SpellId>()
        {
            SpellId.CloakWeapontinkeringMastery1,
            SpellId.CloakWeapontinkeringMastery2,
            SpellId.CloakWeapontinkeringMastery3,
            SpellId.CloakWeapontinkeringMastery4,
            SpellId.CloakWeapontinkeringMastery5,
        };

        public static readonly List<SpellId> CloakAssessCreatureMastery = new List<SpellId>()
        {
            SpellId.CloakAssessCreatureMastery1,
            SpellId.CloakAssessCreatureMastery2,
            SpellId.CloakAssessCreatureMastery3,
            SpellId.CloakAssessCreatureMastery4,
            SpellId.CloakAssessCreatureMastery5,
        };

        public static readonly List<SpellId> DirtyFightingIneptitudeOther = new List<SpellId>()
        {
            SpellId.DirtyFightingIneptitudeOther1,
            SpellId.DirtyFightingIneptitudeOther2,
            SpellId.DirtyFightingIneptitudeOther3,
            SpellId.DirtyFightingIneptitudeOther4,
            SpellId.DirtyFightingIneptitudeOther5,
            SpellId.DirtyFightingIneptitudeOther6,
            SpellId.DirtyFightingIneptitudeOther7,
            SpellId.DirtyFightingIneptitudeOther8,
        };

        public static readonly List<SpellId> DirtyFightingMasteryOther = new List<SpellId>()
        {
            SpellId.DirtyFightingMasteryOther1,
            SpellId.DirtyFightingMasteryOther2,
            SpellId.DirtyFightingMasteryOther3,
            SpellId.DirtyFightingMasteryOther4,
            SpellId.DirtyFightingMasteryOther5,
            SpellId.DirtyFightingMasteryOther6,
            SpellId.DirtyFightingMasteryOther7,
            SpellId.DirtyFightingMasteryOther8,
        };

        public static readonly List<SpellId> DirtyFightingMasterySelf = new List<SpellId>()
        {
            SpellId.DirtyFightingMasterySelf1,
            SpellId.DirtyFightingMasterySelf2,
            SpellId.DirtyFightingMasterySelf3,
            SpellId.DirtyFightingMasterySelf4,
            SpellId.DirtyFightingMasterySelf5,
            SpellId.DirtyFightingMasterySelf6,
            SpellId.DirtyFightingMasterySelf7,
            SpellId.DirtyFightingMasterySelf8,
        };

        public static readonly List<SpellId> DualWieldIneptitudeOther = new List<SpellId>()
        {
            SpellId.DualWieldIneptitudeOther1,
            SpellId.DualWieldIneptitudeOther2,
            SpellId.DualWieldIneptitudeOther3,
            SpellId.DualWieldIneptitudeOther4,
            SpellId.DualWieldIneptitudeOther5,
            SpellId.DualWieldIneptitudeOther6,
            SpellId.DualWieldIneptitudeOther7,
            SpellId.DualWieldIneptitudeOther8,
        };

        public static readonly List<SpellId> DualWieldMasteryOther = new List<SpellId>()
        {
            SpellId.DualWieldMasteryOther1,
            SpellId.DualWieldMasteryOther2,
            SpellId.DualWieldMasteryOther3,
            SpellId.DualWieldMasteryOther4,
            SpellId.DualWieldMasteryOther5,
            SpellId.DualWieldMasteryOther6,
            SpellId.DualWieldMasteryOther7,
            SpellId.DualWieldMasteryOther8,
        };

        public static readonly List<SpellId> DualWieldMasterySelf = new List<SpellId>()
        {
            SpellId.DualWieldMasterySelf1,
            SpellId.DualWieldMasterySelf2,
            SpellId.DualWieldMasterySelf3,
            SpellId.DualWieldMasterySelf4,
            SpellId.DualWieldMasterySelf5,
            SpellId.DualWieldMasterySelf6,
            SpellId.DualWieldMasterySelf7,
            SpellId.DualWieldMasterySelf8,
        };

        public static readonly List<SpellId> RecklessnessIneptitudeOther = new List<SpellId>()
        {
            SpellId.RecklessnessIneptitudeOther1,
            SpellId.RecklessnessIneptitudeOther2,
            SpellId.RecklessnessIneptitudeOther3,
            SpellId.RecklessnessIneptitudeOther4,
            SpellId.RecklessnessIneptitudeOther5,
            SpellId.RecklessnessIneptitudeOther6,
            SpellId.RecklessnessIneptitudeOther7,
            SpellId.RecklessnessIneptitudeOther8,
        };

        public static readonly List<SpellId> RecklessnessMasteryOther = new List<SpellId>()
        {
            SpellId.RecklessnessMasteryOther1,
            SpellId.RecklessnessMasteryOther2,
            SpellId.RecklessnessMasteryOther3,
            SpellId.RecklessnessMasteryOther4,
            SpellId.RecklessnessMasteryOther5,
            SpellId.RecklessnessMasteryOther6,
            SpellId.RecklessnessMasteryOther7,
            SpellId.RecklessnessMasteryOther8,
        };

        public static readonly List<SpellId> RecklessnessMasterySelf = new List<SpellId>()
        {
            SpellId.RecklessnessMasterySelf1,
            SpellId.RecklessnessMasterySelf2,
            SpellId.RecklessnessMasterySelf3,
            SpellId.RecklessnessMasterySelf4,
            SpellId.RecklessnessMasterySelf5,
            SpellId.RecklessnessMasterySelf6,
            SpellId.RecklessnessMasterySelf7,
            SpellId.RecklessnessMasterySelf8,
        };

        public static readonly List<SpellId> ShieldIneptitudeOther = new List<SpellId>()
        {
            SpellId.ShieldIneptitudeOther1,
            SpellId.ShieldIneptitudeOther2,
            SpellId.ShieldIneptitudeOther3,
            SpellId.ShieldIneptitudeOther4,
            SpellId.ShieldIneptitudeOther5,
            SpellId.ShieldIneptitudeOther6,
            SpellId.ShieldIneptitudeOther7,
            SpellId.ShieldIneptitudeOther8,
        };

        public static readonly List<SpellId> ShieldMasteryOther = new List<SpellId>()
        {
            SpellId.ShieldMasteryOther1,
            SpellId.ShieldMasteryOther2,
            SpellId.ShieldMasteryOther3,
            SpellId.ShieldMasteryOther4,
            SpellId.ShieldMasteryOther5,
            SpellId.ShieldMasteryOther6,
            SpellId.ShieldMasteryOther7,
            SpellId.ShieldMasteryOther8,
        };

        public static readonly List<SpellId> ShieldMasterySelf = new List<SpellId>()
        {
            SpellId.ShieldMasterySelf1,
            SpellId.ShieldMasterySelf2,
            SpellId.ShieldMasterySelf3,
            SpellId.ShieldMasterySelf4,
            SpellId.ShieldMasterySelf5,
            SpellId.ShieldMasterySelf6,
            SpellId.ShieldMasterySelf7,
            SpellId.ShieldMasterySelf8,
        };

        public static readonly List<SpellId> SneakAttackIneptitudeOther = new List<SpellId>()
        {
            SpellId.SneakAttackIneptitudeOther1,
            SpellId.SneakAttackIneptitudeOther2,
            SpellId.SneakAttackIneptitudeOther3,
            SpellId.SneakAttackIneptitudeOther4,
            SpellId.SneakAttackIneptitudeOther5,
            SpellId.SneakAttackIneptitudeOther6,
            SpellId.SneakAttackIneptitudeOther7,
            SpellId.SneakAttackIneptitudeOther8,
        };

        public static readonly List<SpellId> SneakAttackMasteryOther = new List<SpellId>()
        {
            SpellId.SneakAttackMasteryOther1,
            SpellId.SneakAttackMasteryOther2,
            SpellId.SneakAttackMasteryOther3,
            SpellId.SneakAttackMasteryOther4,
            SpellId.SneakAttackMasteryOther5,
            SpellId.SneakAttackMasteryOther6,
            SpellId.SneakAttackMasteryOther7,
            SpellId.SneakAttackMasteryOther8,
        };

        public static readonly List<SpellId> SneakAttackMasterySelf = new List<SpellId>()
        {
            SpellId.SneakAttackMasterySelf1,
            SpellId.SneakAttackMasterySelf2,
            SpellId.SneakAttackMasterySelf3,
            SpellId.SneakAttackMasterySelf4,
            SpellId.SneakAttackMasterySelf5,
            SpellId.SneakAttackMasterySelf6,
            SpellId.SneakAttackMasterySelf7,
            SpellId.SneakAttackMasterySelf8,
        };

        public static readonly List<SpellId> CantripDirtyFightingProwess = new List<SpellId>()
        {
            SpellId.CantripDirtyFightingProwess1,
            SpellId.CantripDirtyFightingProwess2,
            SpellId.CantripDirtyFightingProwess3,
            SpellId.CantripDirtyFightingProwess4,
        };

        public static readonly List<SpellId> CantripDualWieldAptitude = new List<SpellId>()
        {
            SpellId.CantripDualWieldAptitude1,
            SpellId.CantripDualWieldAptitude2,
            SpellId.CantripDualWieldAptitude3,
            SpellId.CantripDualWieldAptitude4,
        };

        public static readonly List<SpellId> CantripRecklessnessProwess = new List<SpellId>()
        {
            SpellId.CantripRecklessnessProwess1,
            SpellId.CantripRecklessnessProwess2,
            SpellId.CantripRecklessnessProwess3,
            SpellId.CantripRecklessnessProwess4,
        };

        public static readonly List<SpellId> CantripShieldAptitude = new List<SpellId>()
        {
            SpellId.CantripShieldAptitude1,
            SpellId.CantripShieldAptitude2,
            SpellId.CantripShieldAptitude3,
            SpellId.CantripShieldAptitude4,
        };

        public static readonly List<SpellId> CantripSneakAttackProwess = new List<SpellId>()
        {
            SpellId.CantripSneakAttackProwess1,
            SpellId.CantripSneakAttackProwess2,
            SpellId.CantripSneakAttackProwess3,
            SpellId.CantripSneakAttackProwess4,
        };

        public static readonly List<SpellId> CloakDirtyFightingMastery = new List<SpellId>()
        {
            SpellId.CloakDirtyFightingMastery1,
            SpellId.CloakDirtyFightingMastery2,
            SpellId.CloakDirtyFightingMastery3,
            SpellId.CloakDirtyFightingMastery4,
            SpellId.CloakDirtyFightingMastery5,
        };

        public static readonly List<SpellId> CloakDualWieldMastery = new List<SpellId>()
        {
            SpellId.CloakDualWieldMastery1,
            SpellId.CloakDualWieldMastery2,
            SpellId.CloakDualWieldMastery3,
            SpellId.CloakDualWieldMastery4,
            SpellId.CloakDualWieldMastery5,
        };

        public static readonly List<SpellId> CloakRecklessnessMastery = new List<SpellId>()
        {
            SpellId.CloakRecklessnessMastery1,
            SpellId.CloakRecklessnessMastery2,
            SpellId.CloakRecklessnessMastery3,
            SpellId.CloakRecklessnessMastery4,
            SpellId.CloakRecklessnessMastery5,
        };

        public static readonly List<SpellId> CloakShieldMastery = new List<SpellId>()
        {
            SpellId.CloakShieldMastery1,
            SpellId.CloakShieldMastery2,
            SpellId.CloakShieldMastery3,
            SpellId.CloakShieldMastery4,
            SpellId.CloakShieldMastery5,
        };

        public static readonly List<SpellId> CloakSneakAttackMastery = new List<SpellId>()
        {
            SpellId.CloakSneakAttackMastery1,
            SpellId.CloakSneakAttackMastery2,
            SpellId.CloakSneakAttackMastery3,
            SpellId.CloakSneakAttackMastery4,
            SpellId.CloakSneakAttackMastery5,
        };

        public static readonly List<SpellId> SetDirtyFightingAptitude = new List<SpellId>()
        {
            SpellId.SetDirtyFightingAptitude1,
            SpellId.SetDirtyFightingAptitude2,
            SpellId.SetDirtyFightingAptitude3,
            SpellId.SetDirtyFightingAptitude4,
        };

        public static readonly List<SpellId> SetDualWieldAptitude = new List<SpellId>()
        {
            SpellId.SetDualWieldAptitude1,
            SpellId.SetDualWieldAptitude2,
            SpellId.SetDualWieldAptitude3,
            SpellId.SetDualWieldAptitude4,
        };

        public static readonly List<SpellId> SetRecklessnessAptitude = new List<SpellId>()
        {
            SpellId.SetRecklessnessAptitude1,
            SpellId.SetRecklessnessAptitude2,
            SpellId.SetRecklessnessAptitude3,
            SpellId.SetRecklessnessAptitude4,
        };

        public static readonly List<SpellId> SetShieldAptitude = new List<SpellId>()
        {
            SpellId.SetShieldAptitude1,
            SpellId.SetShieldAptitude2,
            SpellId.SetShieldAptitude3,
            SpellId.SetShieldAptitude4,
        };

        public static readonly List<SpellId> SetSneakAttackAptitude = new List<SpellId>()
        {
            SpellId.SetSneakAttackAptitude1,
            SpellId.SetSneakAttackAptitude2,
            SpellId.SetSneakAttackAptitude3,
            SpellId.SetSneakAttackAptitude4,
        };

        public static readonly List<SpellId> RareArmorDamageBoost = new List<SpellId>()
        {
            SpellId.RareArmorDamageBoost1,
            SpellId.RareArmorDamageBoost2,
            SpellId.RareArmorDamageBoost3,
            SpellId.RareArmorDamageBoost4,
            SpellId.RareArmorDamageBoost5,
        };

        public static readonly List<SpellId> HermeticLinkOther = new List<SpellId>()
        {
            SpellId.HermeticLinkOther1,
            SpellId.HermeticLinkOther2,
            SpellId.HermeticLinkOther3,
            SpellId.HermeticLinkOther4,
            SpellId.HermeticLinkOther5,
            SpellId.HermeticLinkOther6,
            SpellId.HermeticLinkOther7,
            SpellId.HermeticLinkOther8,
        };

        public static readonly List<SpellId> BloodDrinkerOther = new List<SpellId>()
        {
            SpellId.BloodDrinkerOther1,
            SpellId.BloodDrinkerOther2,
            SpellId.BloodDrinkerOther3,
            SpellId.BloodDrinkerOther4,
            SpellId.BloodDrinkerOther5,
            SpellId.BloodDrinkerOther6,
            SpellId.BloodDrinkerOther7,
            SpellId.BloodDrinkerOther8,
        };

        public static readonly List<SpellId> DefenderOther = new List<SpellId>()
        {
            SpellId.DefenderOther1,
            SpellId.DefenderOther2,
            SpellId.DefenderOther3,
            SpellId.DefenderOther4,
            SpellId.DefenderOther5,
            SpellId.DefenderOther6,
            SpellId.DefenderOther7,
            SpellId.DefenderOther8,
        };

        public static readonly List<SpellId> HeartSeekerOther = new List<SpellId>()
        {
            SpellId.HeartSeekerOther1,
            SpellId.HeartSeekerOther2,
            SpellId.HeartSeekerOther3,
            SpellId.HeartSeekerOther4,
            SpellId.HeartSeekerOther5,
            SpellId.HeartSeekerOther6,
            SpellId.HeartSeekerOther7,
            SpellId.HeartSeekerOther8,
        };

        public static readonly List<SpellId> SpiritDrinkerOther = new List<SpellId>()
        {
            SpellId.SpiritDrinkerOther1,
            SpellId.SpiritDrinkerOther2,
            SpellId.SpiritDrinkerOther3,
            SpellId.SpiritDrinkerOther4,
            SpellId.SpiritDrinkerOther5,
            SpellId.SpiritDrinkerOther6,
            SpellId.SpiritDrinkerOther7,
            SpellId.SpiritDrinkerOther8,
        };

        public static readonly List<SpellId> SwiftKillerOther = new List<SpellId>()
        {
            SpellId.SwiftKillerOther1,
            SpellId.SwiftKillerOther2,
            SpellId.SwiftKillerOther3,
            SpellId.SwiftKillerOther4,
            SpellId.SwiftKillerOther5,
            SpellId.SwiftKillerOther6,
            SpellId.SwiftKillerOther7,
            SpellId.SwiftKillerOther8,
        };

        public static readonly List<SpellId> SummoningMasteryOther = new List<SpellId>()
        {
            SpellId.SummoningMasteryOther1,
            SpellId.SummoningMasteryOther2,
            SpellId.SummoningMasteryOther3,
            SpellId.SummoningMasteryOther4,
            SpellId.SummoningMasteryOther5,
            SpellId.SummoningMasteryOther6,
            SpellId.SummoningMasteryOther7,
            SpellId.SummoningMasteryOther8,
        };

        public static readonly List<SpellId> SummoningMasterySelf = new List<SpellId>()
        {
            SpellId.SummoningMasterySelf1,
            SpellId.SummoningMasterySelf2,
            SpellId.SummoningMasterySelf3,
            SpellId.SummoningMasterySelf4,
            SpellId.SummoningMasterySelf5,
            SpellId.SummoningMasterySelf6,
            SpellId.SummoningMasterySelf7,
            SpellId.SummoningMasterySelf8,
        };

        public static readonly List<SpellId> CantripSummoningProwess = new List<SpellId>()
        {
            SpellId.CantripSummoningProwess1,
            SpellId.CantripSummoningProwess2,
            SpellId.CantripSummoningProwess3,
            SpellId.CantripSummoningProwess4,
        };

        public static readonly List<SpellId> SummoningIneptitudeOther = new List<SpellId>()
        {
            SpellId.SummoningIneptitudeOther1,
            SpellId.SummoningIneptitudeOther2,
            SpellId.SummoningIneptitudeOther3,
            SpellId.SummoningIneptitudeOther4,
            SpellId.SummoningIneptitudeOther5,
            SpellId.SummoningIneptitudeOther6,
            SpellId.SummoningIneptitudeOther7,
            SpellId.SummoningIneptitudeOther8,
        };

        public static readonly List<SpellId> CloakSummoningMastery = new List<SpellId>()
        {
            SpellId.CloakSummoningMastery1,
            SpellId.CloakSummoningMastery2,
            SpellId.CloakSummoningMastery3,
            SpellId.CloakSummoningMastery4,
            SpellId.CloakSummoningMastery5,
        };

        public static readonly List<SpellId> SetSummoningAptitude = new List<SpellId>()
        {
            SpellId.SetSummoningAptitude1,
            SpellId.SetSummoningAptitude2,
            SpellId.SetSummoningAptitude3,
            SpellId.SetSummoningAptitude4,
        };

        public static readonly List<SpellId> ReturnToTheStronghold = new List<SpellId>()
        {
            SpellId.ReturnToTheStronghold1,
            SpellId.ReturnToTheStronghold2,
            SpellId.ReturnToTheStronghold3,
        };

        public static readonly List<SpellId> ParagonsDualWieldMastery = new List<SpellId>()
        {
            SpellId.ParagonsDualWieldMasteryI,
            SpellId.ParagonsDualWieldMasteryII,
            SpellId.ParagonsDualWieldMasteryIII,
            SpellId.ParagonsDualWieldMasteryIV,
            SpellId.ParagonsDualWieldMasteryV,
        };

        public static readonly List<SpellId> ParagonsFinesseWeaponMastery = new List<SpellId>()
        {
            SpellId.ParagonsFinesseWeaponMasteryI,
            SpellId.ParagonsFinesseWeaponMasteryII,
            SpellId.ParagonsFinesseWeaponMasteryIII,
            SpellId.ParagonsFinesseWeaponMasteryIV,
            SpellId.ParagonsFinesseWeaponMasteryV,
        };

        public static readonly List<SpellId> ParagonsHeavyWeaponMastery = new List<SpellId>()
        {
            SpellId.ParagonsHeavyWeaponMasteryI,
            SpellId.ParagonsHeavyWeaponMasteryII,
            SpellId.ParagonsHeavyWeaponMasteryIII,
            SpellId.ParagonsHeavyWeaponMasteryIV,
            SpellId.ParagonsHeavyWeaponMasteryV,
        };

        public static readonly List<SpellId> ParagonsLifeMagicMastery = new List<SpellId>()
        {
            SpellId.ParagonsLifeMagicMasteryI,
            SpellId.ParagonsLifeMagicMasteryII,
            SpellId.ParagonsLifeMagicMasteryIII,
            SpellId.ParagonsLifeMagicMasteryIV,
            SpellId.ParagonsLifeMagicMasteryV,
        };

        public static readonly List<SpellId> ParagonsLightWeaponMastery = new List<SpellId>()
        {
            SpellId.ParagonsLightWeaponMasteryI,
            SpellId.ParagonsLightWeaponMasteryII,
            SpellId.ParagonsLightWeaponMasteryIII,
            SpellId.ParagonsLightWeaponMasteryIV,
            SpellId.ParagonsLightWeaponMasteryV,
        };

        public static readonly List<SpellId> ParagonsMissileWeaponMastery = new List<SpellId>()
        {
            SpellId.ParagonsMissileWeaponMasteryI,
            SpellId.ParagonsMissileWeaponMasteryII,
            SpellId.ParagonsMissileWeaponMasteryIII,
            SpellId.ParagonsMissileWeaponMasteryIV,
            SpellId.ParagonsMissileWeaponMasteryV,
        };

        public static readonly List<SpellId> ParagonsRecklessnessMastery = new List<SpellId>()
        {
            SpellId.ParagonsRecklessnessMasteryI,
            SpellId.ParagonsRecklessnessMasteryII,
            SpellId.ParagonsRecklessnessMasteryIII,
            SpellId.ParagonsRecklessnessMasteryIV,
            SpellId.ParagonsRecklessnessMasteryV,
        };

        public static readonly List<SpellId> ParagonsSneakAttackMastery = new List<SpellId>()
        {
            SpellId.ParagonsSneakAttackMasteryI,
            SpellId.ParagonsSneakAttackMasteryII,
            SpellId.ParagonsSneakAttackMasteryIII,
            SpellId.ParagonsSneakAttackMasteryIV,
            SpellId.ParagonsSneakAttackMasteryV,
        };

        public static readonly List<SpellId> ParagonsTwoHandedCombatMastery = new List<SpellId>()
        {
            SpellId.ParagonsTwoHandedCombatMasteryI,
            SpellId.ParagonsTwoHandedCombatMasteryII,
            SpellId.ParagonsTwoHandedCombatMasteryIII,
            SpellId.ParagonsTwoHandedCombatMasteryIV,
            SpellId.ParagonsTwoHandedCombatMasteryV,
        };

        public static readonly List<SpellId> ParagonsVoidMagicMastery = new List<SpellId>()
        {
            SpellId.ParagonsVoidMagicMasteryI,
            SpellId.ParagonsVoidMagicMasteryII,
            SpellId.ParagonsVoidMagicMasteryIII,
            SpellId.ParagonsVoidMagicMasteryIV,
            SpellId.ParagonsVoidMagicMasteryV,
        };

        public static readonly List<SpellId> ParagonsWarMagicMastery = new List<SpellId>()
        {
            SpellId.ParagonsWarMagicMasteryI,
            SpellId.ParagonsWarMagicMasteryII,
            SpellId.ParagonsWarMagicMasteryIII,
            SpellId.ParagonsWarMagicMasteryIV,
            SpellId.ParagonsWarMagicMasteryV,
        };

        public static readonly List<SpellId> ParagonsDirtyFightingMastery = new List<SpellId>()
        {
            SpellId.ParagonsDirtyFightingMasteryI,
            SpellId.ParagonsDirtyFightingMasteryII,
            SpellId.ParagonsDirtyFightingMasteryIII,
            SpellId.ParagonsDirtyFightingMasteryIV,
            SpellId.ParagonsDirtyFightingMasteryV,
        };

        public static readonly List<SpellId> ParagonsWillpower = new List<SpellId>()
        {
            SpellId.ParagonsWillpowerI,
            SpellId.ParagonsWillpowerII,
            SpellId.ParagonsWillpowerIII,
            SpellId.ParagonsWillpowerIV,
            SpellId.ParagonsWillpowerV,
        };

        public static readonly List<SpellId> ParagonsCoordination = new List<SpellId>()
        {
            SpellId.ParagonsCoordinationI,
            SpellId.ParagonsCoordinationII,
            SpellId.ParagonsCoordinationIII,
            SpellId.ParagonsCoordinationIV,
            SpellId.ParagonsCoordinationV,
        };

        public static readonly List<SpellId> ParagonsEndurance = new List<SpellId>()
        {
            SpellId.ParagonsEnduranceI,
            SpellId.ParagonsEnduranceII,
            SpellId.ParagonsEnduranceIII,
            SpellId.ParagonsEnduranceIV,
            SpellId.ParagonsEnduranceV,
        };

        public static readonly List<SpellId> ParagonsFocus = new List<SpellId>()
        {
            SpellId.ParagonsFocusI,
            SpellId.ParagonsFocusII,
            SpellId.ParagonsFocusIII,
            SpellId.ParagonsFocusIV,
            SpellId.ParagonsFocusV,
        };

        public static readonly List<SpellId> ParagonQuickness = new List<SpellId>()
        {
            SpellId.ParagonQuicknessI,
            SpellId.ParagonQuicknessII,
            SpellId.ParagonQuicknessIII,
            SpellId.ParagonQuicknessIV,
            SpellId.ParagonQuicknessV,
        };

        public static readonly List<SpellId> ParagonsStrength = new List<SpellId>()
        {
            SpellId.ParagonsStrengthI,
            SpellId.ParagonsStrengthII,
            SpellId.ParagonsStrengthIII,
            SpellId.ParagonsStrengthIV,
            SpellId.ParagonsStrengthV,
        };

        public static readonly List<SpellId> ParagonsStamina = new List<SpellId>()
        {
            SpellId.ParagonsStaminaI,
            SpellId.ParagonsStaminaII,
            SpellId.ParagonsStaminaIII,
            SpellId.ParagonsStaminaIV,
            SpellId.ParagonsStaminaV,
        };

        public static readonly List<SpellId> ParagonsCriticalDamageBoost = new List<SpellId>()
        {
            SpellId.ParagonsCriticalDamageBoostII,
            SpellId.ParagonsCriticalDamageBoostIII,
            SpellId.ParagonsCriticalDamageBoostIV,
            SpellId.ParagonsCriticalDamageBoostV,
        };

        public static readonly List<SpellId> ParagonsCriticalDamageReduction = new List<SpellId>()
        {
            SpellId.ParagonsCriticalDamageReductionI,
            SpellId.ParagonsCriticalDamageReductionII,
            SpellId.ParagonsCriticalDamageReductionIII,
            SpellId.ParagonsCriticalDamageReductionIV,
            SpellId.ParagonsCriticalDamageReductionV,
        };

        public static readonly List<SpellId> ParagonsDamageBoost = new List<SpellId>()
        {
            SpellId.ParagonsDamageBoostI,
            SpellId.ParagonsDamageBoostII,
            SpellId.ParagonsDamageBoostIII,
            SpellId.ParagonsDamageBoostIV,
            SpellId.ParagonsDamageBoostV,
        };

        public static readonly List<SpellId> ParagonsDamageReduction = new List<SpellId>()
        {
            SpellId.ParagonsDamageReductionI,
            SpellId.ParagonsDamageReductionII,
            SpellId.ParagonsDamageReductionIII,
            SpellId.ParagonsDamageReductionIV,
            SpellId.ParagonsDamageReductionV,
        };

        public static readonly List<SpellId> ParagonsMana = new List<SpellId>()
        {
            SpellId.ParagonsManaI,
            SpellId.ParagonsManaII,
            SpellId.ParagonsManaIII,
            SpellId.ParagonsManaIV,
            SpellId.ParagonsManaV,
        };

        public static readonly List<SpellId> GauntletCriticalDamageBoost = new List<SpellId>()
        {
            SpellId.GauntletCriticalDamageBoostI,
            SpellId.GauntletCriticalDamageBoostII,
        };

        public static readonly List<SpellId> GauntletDamageBoost = new List<SpellId>()
        {
            SpellId.GauntletDamageBoostI,
            SpellId.GauntletDamageBoostII,
        };

        public static readonly List<SpellId> GauntletDamageReduction = new List<SpellId>()
        {
            SpellId.GauntletDamageReductionI,
            SpellId.GauntletDamageReductionII,
        };

        public static readonly List<SpellId> GauntletCriticalDamageReduction = new List<SpellId>()
        {
            SpellId.GauntletCriticalDamageReductionI,
            SpellId.GauntletCriticalDamageReductionII,
        };

        public static readonly List<SpellId> GauntletHealingBoost = new List<SpellId>()
        {
            SpellId.GauntletHealingBoostI,
            SpellId.GauntletHealingBoostII,
        };

        public static readonly List<SpellId> GauntletVitality = new List<SpellId>()
        {
            SpellId.GauntletVitalityI,
            SpellId.GauntletVitalityII,
            SpellId.GauntletVitalityIII,
        };

        static SpellLevelProgression()
        {
            // takes ~5ms

            spellProgression = new Dictionary<SpellId, List<SpellId>>();

            AddSpells(StrengthOther);
            AddSpells(StrengthSelf);
            AddSpells(WeaknessOther);
            AddSpells(WeaknessSelf);
            AddSpells(HealOther);
            AddSpells(HealSelf);
            AddSpells(HarmOther);
            AddSpells(HarmSelf);
            AddSpells(InfuseMana);
            AddSpells(VulnerabilityOther);
            AddSpells(VulnerabilitySelf);
            AddSpells(InvulnerabilityOther);
            AddSpells(InvulnerabilitySelf);
            AddSpells(FireProtectionOther);
            AddSpells(FireProtectionSelf);
            AddSpells(FireVulnerabilityOther);
            AddSpells(FireVulnerabilitySelf);
            AddSpells(ArmorOther);
            AddSpells(ArmorSelf);
            AddSpells(ImperilOther);
            AddSpells(ImperilSelf);
            AddSpells(FlameBolt);
            AddSpells(FrostBolt);
            AddSpells(BloodDrinkerSelf);
            AddSpells(BloodLoather);
            AddSpells(BladeBane);
            AddSpells(BladeLure);
            AddSpells(PortalTie);
            AddSpells(PortalTieRecall);
            AddSpells(SwiftKillerSelf);
            AddSpells(LeadenWeapon);
            AddSpells(Impenetrability);
            AddSpells(RejuvenationOther);
            AddSpells(RejuvenationSelf);
            AddSpells(AcidStream);
            AddSpells(ShockWave);
            AddSpells(LightningBolt);
            AddSpells(ForceBolt);
            AddSpells(WhirlingBlade);
            AddSpells(AcidBlast);
            AddSpells(ShockBlast);
            AddSpells(FrostBlast);
            AddSpells(LightningBlast);
            AddSpells(FlameBlast);
            AddSpells(ForceBlast);
            AddSpells(BladeBlast);
            AddSpells(AcidVolley);
            AddSpells(BludgeoningVolley);
            AddSpells(FrostVolley);
            AddSpells(LightningVolley);
            AddSpells(FlameVolley);
            AddSpells(ForceVolley);
            AddSpells(BladeVolley);
            AddSpells(SummonPortal);
            AddSpells(RegenerationOther);
            AddSpells(RegenerationSelf);
            AddSpells(FesterOther);
            AddSpells(FesterSelf);
            AddSpells(ExhaustionOther);
            AddSpells(ExhaustionSelf);
            AddSpells(ManaRenewalOther);
            AddSpells(ManaRenewalSelf);
            AddSpells(ManaDepletionOther);
            AddSpells(ManaDepletionSelf);
            AddSpells(ImpregnabilityOther);
            AddSpells(ImpregnabilitySelf);
            AddSpells(DefenselessnessOther);
            AddSpells(MagicResistanceOther);
            AddSpells(MagicResistanceSelf);
            AddSpells(MagicYieldOther);
            AddSpells(MagicYieldSelf);
            AddSpells(LightWeaponsMasteryOther);
            AddSpells(LightWeaponsMasterySelf);
            AddSpells(LightWeaponsIneptitudeOther);
            AddSpells(LightWeaponsIneptitudeSelf);
            AddSpells(FinesseWeaponsMasteryOther);
            AddSpells(FinesseWeaponsMasterySelf);
            AddSpells(FinesseWeaponsIneptitudeOther);
            AddSpells(FinesseWeaponsIneptitudeSelf);
            AddSpells(MaceMasteryOther);
            AddSpells(MaceMasterySelf);
            AddSpells(MaceIneptitudeOther);
            AddSpells(MaceIneptitudeSelf);
            AddSpells(SpearMasteryOther);
            AddSpells(SpearMasterySelf);
            AddSpells(SpearIneptitudeOther);
            AddSpells(SpearIneptitudeSelf);
            AddSpells(StaffMasteryOther);
            AddSpells(StaffMasterySelf);
            AddSpells(StaffIneptitudeOther);
            AddSpells(StaffIneptitudeSelf);
            AddSpells(HeavyWeaponsMasteryOther);
            AddSpells(HeavyWeaponsMasterySelf);
            AddSpells(HeavyWeaponsIneptitudeOther);
            AddSpells(HeavyWeaponsIneptitudeSelf);
            AddSpells(UnarmedCombatMasteryOther);
            AddSpells(UnarmedCombatMasterySelf);
            AddSpells(UnarmedCombatIneptitudeOther);
            AddSpells(UnarmedCombatIneptitudeSelf);
            AddSpells(MissileWeaponsMasteryOther);
            AddSpells(MissileWeaponsMasterySelf);
            AddSpells(MissileWeaponsIneptitudeOther);
            AddSpells(MissileWeaponsIneptitudeSelf);
            AddSpells(CrossbowMasteryOther);
            AddSpells(CrossbowMasterySelf);
            AddSpells(CrossbowIneptitudeOther);
            AddSpells(CrossbowIneptitudeSelf);
            AddSpells(AcidProtectionOther);
            AddSpells(AcidProtectionSelf);
            AddSpells(AcidVulnerabilityOther);
            AddSpells(AcidVulnerabilitySelf);
            AddSpells(ThrownWeaponMasteryOther);
            AddSpells(ThrownWeaponMasterySelf);
            AddSpells(ThrownWeaponIneptitudeOther);
            AddSpells(ThrownWeaponIneptitudeSelf);
            AddSpells(CreatureEnchantmentMasterySelf);
            AddSpells(CreatureEnchantmentMasteryOther);
            AddSpells(CreatureEnchantmentIneptitudeOther);
            AddSpells(CreatureEnchantmentIneptitudeSelf);
            AddSpells(ItemEnchantmentMasterySelf);
            AddSpells(ItemEnchantmentMasteryOther);
            AddSpells(ItemEnchantmentIneptitudeOther);
            AddSpells(ItemEnchantmentIneptitudeSelf);
            AddSpells(LifeMagicMasterySelf);
            AddSpells(LifeMagicMasteryOther);
            AddSpells(LifeMagicIneptitudeSelf);
            AddSpells(LifeMagicIneptitudeOther);
            AddSpells(WarMagicMasterySelf);
            AddSpells(WarMagicMasteryOther);
            AddSpells(WarMagicIneptitudeSelf);
            AddSpells(WarMagicIneptitudeOther);
            AddSpells(ManaMasterySelf);
            AddSpells(ManaMasteryOther);
            AddSpells(ManaIneptitudeSelf);
            AddSpells(ManaIneptitudeOther);
            AddSpells(ArcaneEnlightenmentSelf);
            AddSpells(ArcaneEnlightenmentOther);
            AddSpells(ArcaneBenightednessSelf);
            AddSpells(ArcaneBenightednessOther);
            AddSpells(ArmorExpertiseSelf);
            AddSpells(ArmorExpertiseOther);
            AddSpells(ArmorIgnoranceSelf);
            AddSpells(ArmorIgnoranceOther);
            AddSpells(ItemExpertiseSelf);
            AddSpells(ItemExpertiseOther);
            AddSpells(ItemIgnoranceSelf);
            AddSpells(ItemIgnoranceOther);
            AddSpells(MagicItemExpertiseSelf);
            AddSpells(MagicItemExpertiseOther);
            AddSpells(MagicItemIgnoranceSelf);
            AddSpells(MagicItemIgnoranceOther);
            AddSpells(WeaponExpertiseSelf);
            AddSpells(WeaponExpertiseOther);
            AddSpells(WeaponIgnoranceSelf);
            AddSpells(WeaponIgnoranceOther);
            AddSpells(MonsterAttunementSelf);
            AddSpells(MonsterAttunementOther);
            AddSpells(MonsterUnfamiliaritySelf);
            AddSpells(MonsterUnfamiliarityOther);
            AddSpells(PersonAttunementSelf);
            AddSpells(PersonAttunementOther);
            AddSpells(PersonUnfamiliaritySelf);
            AddSpells(PersonUnfamiliarityOther);
            AddSpells(DeceptionMasterySelf);
            AddSpells(DeceptionMasteryOther);
            AddSpells(DeceptionIneptitudeSelf);
            AddSpells(DeceptionIneptitudeOther);
            AddSpells(HealingMasterySelf);
            AddSpells(HealingMasteryOther);
            AddSpells(HealingIneptitudeSelf);
            AddSpells(HealingIneptitudeOther);
            AddSpells(LeadershipMasterySelf);
            AddSpells(LeadershipMasteryOther);
            AddSpells(LeadershipIneptitudeSelf);
            AddSpells(LeadershipIneptitudeOther);
            AddSpells(LockpickMasterySelf);
            AddSpells(LockpickMasteryOther);
            AddSpells(LockpickIneptitudeSelf);
            AddSpells(LockpickIneptitudeOther);
            AddSpells(FealtySelf);
            AddSpells(FealtyOther);
            AddSpells(FaithlessnessSelf);
            AddSpells(FaithlessnessOther);
            AddSpells(JumpingMasterySelf);
            AddSpells(JumpingMasteryOther);
            AddSpells(SprintSelf);
            AddSpells(SprintOther);
            AddSpells(LeadenFeetSelf);
            AddSpells(LeadenFeetOther);
            AddSpells(JumpingIneptitudeSelf);
            AddSpells(JumpingIneptitudeOther);
            AddSpells(BludgeonProtectionSelf);
            AddSpells(BludgeonProtectionOther);
            AddSpells(ColdProtectionSelf);
            AddSpells(ColdProtectionOther);
            AddSpells(BludgeonVulnerabilitySelf);
            AddSpells(BludgeonVulnerabilityOther);
            AddSpells(ColdVulnerabilitySelf);
            AddSpells(ColdVulnerabilityOther);
            AddSpells(LightningProtectionSelf);
            AddSpells(LightningProtectionOther);
            AddSpells(LightningVulnerabilitySelf);
            AddSpells(LightningVulnerabilityOther);
            AddSpells(BladeProtectionSelf);
            AddSpells(BladeProtectionOther);
            AddSpells(BladeVulnerabilitySelf);
            AddSpells(BladeVulnerabilityOther);
            AddSpells(PiercingProtectionSelf);
            AddSpells(PiercingProtectionOther);
            AddSpells(PiercingVulnerabilitySelf);
            AddSpells(PiercingVulnerabilityOther);
            AddSpells(RevitalizeSelf);
            AddSpells(RevitalizeOther);
            AddSpells(EnfeebleSelf);
            AddSpells(EnfeebleOther);
            AddSpells(ManaBoostSelf);
            AddSpells(ManaBoostOther);
            AddSpells(ManaDrainSelf);
            AddSpells(ManaDrainOther);
            AddSpells(InfuseHealth);
            AddSpells(DrainHealth);
            AddSpells(InfuseStamina);
            AddSpells(DrainStamina);
            AddSpells(DrainMana);
            AddSpells(HealthToStaminaOther);
            AddSpells(HealthToStaminaSelf);
            AddSpells(HealthToManaSelf);
            AddSpells(HealthToManaOther);
            AddSpells(ManaToHealthOther);
            AddSpells(ManaToHealthSelf);
            AddSpells(ManaToStaminaSelf);
            AddSpells(ManaToStaminaOther);
            AddSpells(EnduranceSelf);
            AddSpells(EnduranceOther);
            AddSpells(FrailtySelf);
            AddSpells(FrailtyOther);
            AddSpells(CoordinationSelf);
            AddSpells(CoordinationOther);
            AddSpells(ClumsinessSelf);
            AddSpells(ClumsinessOther);
            AddSpells(QuicknessSelf);
            AddSpells(QuicknessOther);
            AddSpells(SlownessSelf);
            AddSpells(SlownessOther);
            AddSpells(FocusSelf);
            AddSpells(FocusOther);
            AddSpells(BafflementSelf);
            AddSpells(BafflementOther);
            AddSpells(WillpowerSelf);
            AddSpells(WillpowerOther);
            AddSpells(FeeblemindSelf);
            AddSpells(FeeblemindOther);
            AddSpells(HermeticVoid);
            AddSpells(HermeticLinkSelf);
            AddSpells(Brittlemail);
            AddSpells(AcidBane);
            AddSpells(AcidLure);
            AddSpells(BludgeonLure);
            AddSpells(BludgeonBane);
            AddSpells(FrostLure);
            AddSpells(FrostBane);
            AddSpells(LightningLure);
            AddSpells(LightningBane);
            AddSpells(FlameLure);
            AddSpells(FlameBane);
            AddSpells(PiercingLure);
            AddSpells(PiercingBane);
            AddSpells(StrengthenLock);
            AddSpells(WeakenLock);
            AddSpells(HeartSeekerSelf);
            AddSpells(TurnBlade);
            AddSpells(DefenderSelf);
            AddSpells(LureBlade);
            AddSpells(DefenselessnessSelf);
            AddSpells(StaminaToHealthOther);
            AddSpells(StaminaToHealthSelf);
            AddSpells(StaminaToManaOther);
            AddSpells(StaminaToManaSelf);
            AddSpells(CookingMasteryOther);
            AddSpells(CookingMasterySelf);
            AddSpells(CookingIneptitudeOther);
            AddSpells(CookingIneptitudeSelf);
            AddSpells(FletchingMasteryOther);
            AddSpells(FletchingMasterySelf);
            AddSpells(FletchingIneptitudeOther);
            AddSpells(FletchingIneptitudeSelf);
            AddSpells(AlchemyMasteryOther);
            AddSpells(AlchemyMasterySelf);
            AddSpells(AlchemyIneptitudeOther);
            AddSpells(AlchemyIneptitudeSelf);
            AddSpells(AcidStreak);
            AddSpells(FlameStreak);
            AddSpells(ForceStreak);
            AddSpells(FrostStreak);
            AddSpells(LightningStreak);
            AddSpells(ShockwaveStreak);
            AddSpells(WhirlingBladeStreak);
            AddSpells(DispelAllNeutralOther);
            AddSpells(DispelAllGoodOther);
            AddSpells(DispelAllBadOther);
            AddSpells(DispelAllNeutralSelf);
            AddSpells(DispelAllGoodSelf);
            AddSpells(DispelAllBadSelf);
            AddSpells(DispelCreatureNeutralOther);
            AddSpells(DispelCreatureGoodOther);
            AddSpells(DispelCreatureBadOther);
            AddSpells(DispelCreatureNeutralSelf);
            AddSpells(DispelCreatureGoodSelf);
            AddSpells(DispelCreatureBadSelf);
            AddSpells(DispelItemNeutralOther);
            AddSpells(DispelItemGoodOther);
            AddSpells(DispelItemBadOther);
            AddSpells(DispelItemNeutralSelf);
            AddSpells(DispelItemGoodSelf);
            AddSpells(DispelItemBadSelf);
            AddSpells(DispelLifeNeutralOther);
            AddSpells(DispelLifeGoodOther);
            AddSpells(DispelLifeBadOther);
            AddSpells(DispelLifeNeutralSelf);
            AddSpells(DispelLifeGoodSelf);
            AddSpells(DispelLifeBadSelf);
            AddSpells(RecallAsmolum);
            AddSpells(PortalSendTrial);
            AddSpells(CANTRIPALCHEMICALPROWESS);
            AddSpells(CANTRIPARCANEPROWESS);
            AddSpells(CANTRIPARMOREXPERTISE);
            AddSpells(CANTRIPLIGHTWEAPONSAPTITUDE);
            AddSpells(CANTRIPMISSILEWEAPONSAPTITUDE);
            AddSpells(CANTRIPCOOKINGPROWESS);
            AddSpells(CANTRIPCREATUREENCHANTMENTAPTITUDE);
            AddSpells(CANTRIPCROSSBOWAPTITUDE);
            AddSpells(CANTRIPFINESSEWEAPONSAPTITUDE);
            AddSpells(CANTRIPDECEPTIONPROWESS);
            AddSpells(CANTRIPFEALTY);
            AddSpells(CANTRIPFLETCHINGPROWESS);
            AddSpells(CANTRIPHEALINGPROWESS);
            AddSpells(CANTRIPIMPREGNABILITY);
            AddSpells(CANTRIPINVULNERABILITY);
            AddSpells(CANTRIPITEMENCHANTMENTAPTITUDE);
            AddSpells(CANTRIPITEMEXPERTISE);
            AddSpells(CANTRIPJUMPINGPROWESS);
            AddSpells(CANTRIPLEADERSHIP);
            AddSpells(CANTRIPLIFEMAGICAPTITUDE);
            AddSpells(CANTRIPLOCKPICKPROWESS);
            AddSpells(CANTRIPMACEAPTITUDE);
            AddSpells(CANTRIPMAGICITEMEXPERTISE);
            AddSpells(CANTRIPMAGICRESISTANCE);
            AddSpells(CANTRIPMANACONVERSIONPROWESS);
            AddSpells(CANTRIPMONSTERATTUNEMENT);
            AddSpells(CANTRIPPERSONATTUNEMENT);
            AddSpells(CANTRIPSPEARAPTITUDE);
            AddSpells(CANTRIPSPRINT);
            AddSpells(CANTRIPSTAFFAPTITUDE);
            AddSpells(CANTRIPHEAVYWEAPONSAPTITUDE);
            AddSpells(CANTRIPTHROWNAPTITUDE);
            AddSpells(CANTRIPUNARMEDAPTITUDE);
            AddSpells(CANTRIPWARMAGICAPTITUDE);
            AddSpells(CANTRIPWEAPONEXPERTISE);
            AddSpells(CANTRIPARMOR);
            AddSpells(CANTRIPCOORDINATION);
            AddSpells(CANTRIPENDURANCE);
            AddSpells(CANTRIPFOCUS);
            AddSpells(CANTRIPQUICKNESS);
            AddSpells(CANTRIPSTRENGTH);
            AddSpells(CANTRIPWILLPOWER);
            AddSpells(CANTRIPACIDBANE);
            AddSpells(CANTRIPBLOODTHIRST);
            AddSpells(CANTRIPBLUDGEONINGBANE);
            AddSpells(CANTRIPDEFENDER);
            AddSpells(CANTRIPFLAMEBANE);
            AddSpells(CANTRIPFROSTBANE);
            AddSpells(CANTRIPHEARTTHIRST);
            AddSpells(CANTRIPIMPENETRABILITY);
            AddSpells(CANTRIPPIERCINGBANE);
            AddSpells(CANTRIPSLASHINGBANE);
            AddSpells(CANTRIPSTORMBANE);
            AddSpells(CANTRIPSWIFTHUNTER);
            AddSpells(CANTRIPACIDWARD);
            AddSpells(CANTRIPBLUDGEONINGWARD);
            AddSpells(CANTRIPFLAMEWARD);
            AddSpells(CANTRIPFROSTWARD);
            AddSpells(CANTRIPPIERCINGWARD);
            AddSpells(CANTRIPSLASHINGWARD);
            AddSpells(CANTRIPSTORMWARD);
            AddSpells(CANTRIPHEALTHGAIN);
            AddSpells(CANTRIPMANAGAIN);
            AddSpells(CANTRIPSTAMINAGAIN);
            AddSpells(SummonSecondPortal);
            AddSpells(MartineRing);
            AddSpells(ElementalFury);
            AddSpells(AcidArc);
            AddSpells(ForceArc);
            AddSpells(FrostArc);
            AddSpells(LightningArc);
            AddSpells(FlameArc);
            AddSpells(ShockArc);
            AddSpells(BladeArc);
            AddSpells(HealthBolt);
            AddSpells(StaminaBolt);
            AddSpells(ManaBolt);
            AddSpells(FireworkOutBlack);
            AddSpells(FireworkOutBlue);
            AddSpells(FireworkOutGreen);
            AddSpells(FireworkOutOrange);
            AddSpells(FireworkOutPurple);
            AddSpells(FireworkOutRed);
            AddSpells(FireworkOutWhite);
            AddSpells(FireworkOutYellow);
            AddSpells(FireworkUpBlack);
            AddSpells(FireworkUpBlue);
            AddSpells(FireworkUpGreen);
            AddSpells(FireworkUpOrange);
            AddSpells(FireworkUpPurple);
            AddSpells(FireworkUpRed);
            AddSpells(FireworkUpWhite);
            AddSpells(FireworkUpYellow);
            AddSpells(PortalSendingKnorr);
            AddSpells(PortalSendingFellowshipLiazkBurun);
            AddSpells(PortalSendingLiazkBurun);
            AddSpells(PortalSendingLiazkJump);
            AddSpells(PortalSendingLiazkSpirits);
            AddSpells(PortalSendingLiazkTest);
            AddSpells(CoordinationFellowship);
            AddSpells(EnduranceFellowship);
            AddSpells(FocusFellowship);
            AddSpells(QuicknessFellowship);
            AddSpells(SelfFellowship);
            AddSpells(StrengthFellowship);
            AddSpells(CantripHermeticLink);
            AddSpells(CantripSpiritThirst);
            AddSpells(SpiritDrinkerSelf);
            AddSpells(SpiritLoather);
            AddSpells(PortalSendingHezhitFight);
            AddSpells(PortalSendingHezhitPrison);
            AddSpells(PortalSendingHizkRiGauntlet);
            AddSpells(PortalSendingHizkRiWell);
            AddSpells(PortalSendingJrvikFight);
            AddSpells(PortalSendingJrvikPrison);
            AddSpells(PortalSendingZixkFight);
            AddSpells(PortalSendingZixkPrison);
            AddSpells(PortalSendingHizkRiGuruk);
            AddSpells(AcidProtectionFellowship);
            AddSpells(BladeProtectionFellowship);
            AddSpells(BludgeonProtectionFellowship);
            AddSpells(ColdProtectionFellowship);
            AddSpells(FireProtectionFellowship);
            AddSpells(LightningProtectionFellowship);
            AddSpells(PierceProtectionFellowship);
            AddSpells(ImpregnabilityFellowship);
            AddSpells(InvulnerabilityFellowship);
            AddSpells(MagicResistanceFellowship);
            AddSpells(CreatureEnchantmentMasteryFellow);
            AddSpells(ItemEnchantmentMasteryFellow);
            AddSpells(LifeMagicMasteryFellow);
            AddSpells(ManaConversionMasteryFellow);
            AddSpells(WarMagicMasteryFellow);
            AddSpells(PortalSendingKivikLirArena);
            AddSpells(PortalSendingKivikLirBoss);
            AddSpells(PortalSendingKivikLirHaven);
            AddSpells(ManaRenewalFellowship);
            AddSpells(RegenerationFellowship);
            AddSpells(RejuvenationFellowship);
            AddSpells(PortalSendingIzjiQoGauntlet);
            AddSpells(PortalSendingIzjiQoReceivingChamber);
            AddSpells(PortalSendingIzjiQoTest);
            AddSpells(ArcanumSalvagingSelf);
            AddSpells(ArcanumSalvagingOther);
            AddSpells(NuhmudirasWisdom);
            AddSpells(NuhmudirasWisdomOther);
            AddSpells(Intoxication);
            AddSpells(AxemansBoon);
            AddSpells(BowmansBoon);
            AddSpells(ChuckersBoon);
            AddSpells(CrossbowmansBoon);
            AddSpells(EnchantersBoon);
            AddSpells(HieromancersBoon);
            AddSpells(KnifersBoon);
            AddSpells(LifeGiversBoon);
            AddSpells(MacersBoon);
            AddSpells(PugilistsBoon);
            AddSpells(SpearmansBoon);
            AddSpells(StafferBoon);
            AddSpells(SwordsmansBoon);
            AddSpells(SalvagingMasteryForge);
            AddSpells(AlchemyMasteryForge);
            AddSpells(CookingMasteryForge);
            AddSpells(FletchingMasteryForge);
            AddSpells(LockpickMasteryForge);
            AddSpells(PortalSendingPvPHate20Entry);
            AddSpells(PortalSendingPvPHate40Entry);
            AddSpells(PortalSendingPvPHate60Entry);
            AddSpells(PortalSendingPvPHate80AccursedEntry);
            AddSpells(PortalSendingPvPHate80UnholyEntry);
            AddSpells(CantripSalvaging);
            AddSpells(RecallSonPooky);
            AddSpells(PortalSendingColosseumA);
            AddSpells(PortalSendingColosseumB);
            AddSpells(PortalSendingColosseumC);
            AddSpells(PortalSendingColosseumD);
            AddSpells(PortalSendingColosseumE);
            AddSpells(PortalSendingVisionQuestBranch4Stage);
            AddSpells(PortalSendingVisionQuestBranch5Stage);
            AddSpells(PortalSendingVisionQuestBranch1Stage);
            AddSpells(PortalSendingVisionQuestBranch2Stage);
            AddSpells(PortalSendingVisionQuestBranch3Stage);
            AddSpells(PortalSendDarkCrypt);
            AddSpells(PortalSendJesterPrison);
            AddSpells(RecallJester);
            AddSpells(PortalSendingCHDSStage);
            AddSpells(MoarsmanPoison);
            AddSpells(SetCoordination);
            AddSpells(SetEndurance);
            AddSpells(SetFocus);
            AddSpells(SetQuickness);
            AddSpells(SetStrength);
            AddSpells(SetWillpower);
            AddSpells(SetHealth);
            AddSpells(SetMana);
            AddSpells(SetStamina);
            AddSpells(SetAcidResistance);
            AddSpells(SetBludgeonResistance);
            AddSpells(SetFlameResistance);
            AddSpells(SetFrostResistance);
            AddSpells(SetLightningResistance);
            AddSpells(SetPierceResistance);
            AddSpells(SetSlashingResistance);
            AddSpells(SetAlchemyAptitude);
            AddSpells(SetArmorExpertiseAptitude);
            AddSpells(SetAxeAptitude);
            AddSpells(SetBowAptitude);
            AddSpells(SetCookingAptitude);
            AddSpells(SetCreatureEnchantmentAptitude);
            AddSpells(SetCrossbowAptitude);
            AddSpells(SetDaggerAptitude);
            AddSpells(SetFletchingAptitude);
            AddSpells(SetItemEnchantmentAptitude);
            AddSpells(SetItemExpertiseAptitude);
            AddSpells(SetJumpingAptitude);
            AddSpells(SetLifeMagicAptitude);
            AddSpells(SetLockpickAptitude);
            AddSpells(SetLoyaltyAptitude);
            AddSpells(SetMaceAptitude);
            AddSpells(SetMagicDefenseAptitude);
            AddSpells(SetMagicItemExpertiseAptitude);
            AddSpells(SetMeleeDefenseAptitude);
            AddSpells(SetMissileDefenseAptitude);
            AddSpells(SetSalvagingAptitude);
            AddSpells(SetSpearAptitude);
            AddSpells(SetSprintAptitude);
            AddSpells(SetStaffAptitude);
            AddSpells(SetSwordAptitude);
            AddSpells(SetThrownAptitude);
            AddSpells(SetUnarmedAptitude);
            AddSpells(SetWarMagicAptitude);
            AddSpells(SetWeaponExpertiseAptitude);
            AddSpells(SetSocietyAttributeAll);
            AddSpells(SetRejuvenation);
            AddSpells(AcidStream8Spellpower);
            AddSpells(MiniArcaneDeath);
            AddSpells(MiniFireball);
            AddSpells(MiniIceball);
            AddSpells(MiniArrow);
            AddSpells(MiniRing);
            AddSpells(PortalSendingAssassinsRoost);
            AddSpells(TwoHandedBoon);
            AddSpells(TwoHandedMasterySelf);
            AddSpells(CANTRIPGEARCRAFTAPTITUDE);
            AddSpells(CANTRIPTWOHANDEDAPTITUDE);
            AddSpells(GearcraftIneptitude);
            AddSpells(GearcraftIneptitudeSelf);
            AddSpells(GearcraftMastery);
            AddSpells(GearcraftMasterySelf);
            AddSpells(TwoHandedIneptitude);
            AddSpells(TwoHandedIneptitudeSelf);
            AddSpells(TwoHandedMasteryOther);
            AddSpells(SetGearCraftAptitude);
            AddSpells(SetTwoHandedAptitude);
            AddSpells(ExposeWeakness);
            AddSpells(CallOfLeadership);
            AddSpells(AnswerOfLoyaltyMana);
            AddSpells(AnswerOfLoyaltyStam);
            AddSpells(TrinketXPBoost);
            AddSpells(TrinketDamageBoost);
            AddSpells(TrinketDamageReduction);
            AddSpells(TrinketHealth);
            AddSpells(TrinketMana);
            AddSpells(TrinketStamina);
            AddSpells(DeceptionArcane);
            AddSpells(SpectralFountain_PortalMaze);
            AddSpells(RareDamageBoost);
            AddSpells(RareDamageReduction);
            AddSpells(AetheriaCriticalDamageBoost);
            AddSpells(AetheriaDamageBoost);
            AddSpells(AetheriaDamageReduction);
            AddSpells(AetheriaHealBuff);
            AddSpells(AetheriaHealth);
            AddSpells(AetheriaMana);
            AddSpells(AetheriaStamina);
            AddSpells(AetheriaEndurance);
            AddSpells(BaelzharonsCurseDestruction);
            AddSpells(CurseDestructionOther);
            AddSpells(NetherStreak);
            AddSpells(NetherBolt);
            AddSpells(NetherArc);
            AddSpells(CurseFestering);
            AddSpells(CurseWeakness);
            AddSpells(Corrosion);
            AddSpells(Corruption);
            AddSpells(VoidMagicMasteryOther);
            AddSpells(VoidMagicMasterySelf);
            AddSpells(VoidMagicIneptitudeOther);
            AddSpells(CantripVoidMagicAptitude);
            AddSpells(SetVoidMagicAptitude);
            AddSpells(CorruptorsBoon);
            AddSpells(AcidSpitStreak);
            AddSpells(AcidSpit);
            AddSpells(AcidSpitArc);
            AddSpells(AcidSpitBlast);
            AddSpells(AcidSpitVolley);
            AddSpells(OlthoiCriticalDamageBoost);
            AddSpells(OlthoiCriticalDamageReduction);
            AddSpells(OlthoiDamageBoost);
            AddSpells(OlthoiDamageReduction);
            AddSpells(AcidSpitVulnerability);
            AddSpells(BloodstoneBolt);
            AddSpells(PortalSendingBloodstoneFactory);
            AddSpells(PortalSendingRitualTime);
            AddSpells(NetherBlast);
            AddSpells(AetheriaDoTResistance);
            AddSpells(AetheriaHealthResistance);
            AddSpells(CloakAlchemyMastery);
            AddSpells(CloakArcaneloreMastery);
            AddSpells(CloakArmortinkeringMastery);
            AddSpells(CloakAssesspersonMastery);
            AddSpells(CloakAxeMastery);
            AddSpells(CloakBowMastery);
            AddSpells(CloakCookingMastery);
            AddSpells(CloakCreatureenchantmentMastery);
            AddSpells(CloakCrossbowMastery);
            AddSpells(CloakDaggerMastery);
            AddSpells(CloakDeceptionMastery);
            AddSpells(CloakFletchingMastery);
            AddSpells(CloakHealingMastery);
            AddSpells(CloakItemenchantmentMastery);
            AddSpells(CloakItemtinkeringMastery);
            AddSpells(CloakLeadershipMastery);
            AddSpells(CloakLifemagicMastery);
            AddSpells(CloakLoyaltyMastery);
            AddSpells(CloakMaceMastery);
            AddSpells(CloakMagicdefenseMastery);
            AddSpells(CloakMagictinkeringMastery);
            AddSpells(CloakManaconversionMastery);
            AddSpells(CloakMeleedefenseMastery);
            AddSpells(CloakMissiledefenseMastery);
            AddSpells(CloakSalvagingMastery);
            AddSpells(CloakSpearMastery);
            AddSpells(CloakStaffMastery);
            AddSpells(CloakSwordMastery);
            AddSpells(CloakThrownWeaponMastery);
            AddSpells(CloakTwoHandedCombatMastery);
            AddSpells(CloakUnarmedCombatMastery);
            AddSpells(CloakVoidMagicMastery);
            AddSpells(CloakWarMagicMastery);
            AddSpells(CloakWeapontinkeringMastery);
            AddSpells(CloakAssessCreatureMastery);
            AddSpells(DirtyFightingIneptitudeOther);
            AddSpells(DirtyFightingMasteryOther);
            AddSpells(DirtyFightingMasterySelf);
            AddSpells(DualWieldIneptitudeOther);
            AddSpells(DualWieldMasteryOther);
            AddSpells(DualWieldMasterySelf);
            AddSpells(RecklessnessIneptitudeOther);
            AddSpells(RecklessnessMasteryOther);
            AddSpells(RecklessnessMasterySelf);
            AddSpells(ShieldIneptitudeOther);
            AddSpells(ShieldMasteryOther);
            AddSpells(ShieldMasterySelf);
            AddSpells(SneakAttackIneptitudeOther);
            AddSpells(SneakAttackMasteryOther);
            AddSpells(SneakAttackMasterySelf);
            AddSpells(CantripDirtyFightingProwess);
            AddSpells(CantripDualWieldAptitude);
            AddSpells(CantripRecklessnessProwess);
            AddSpells(CantripShieldAptitude);
            AddSpells(CantripSneakAttackProwess);
            AddSpells(CloakDirtyFightingMastery);
            AddSpells(CloakDualWieldMastery);
            AddSpells(CloakRecklessnessMastery);
            AddSpells(CloakShieldMastery);
            AddSpells(CloakSneakAttackMastery);
            AddSpells(SetDirtyFightingAptitude);
            AddSpells(SetDualWieldAptitude);
            AddSpells(SetRecklessnessAptitude);
            AddSpells(SetShieldAptitude);
            AddSpells(SetSneakAttackAptitude);
            AddSpells(RareArmorDamageBoost);
            AddSpells(HermeticLinkOther);
            AddSpells(BloodDrinkerOther);
            AddSpells(DefenderOther);
            AddSpells(HeartSeekerOther);
            AddSpells(SpiritDrinkerOther);
            AddSpells(SwiftKillerOther);
            AddSpells(SummoningMasteryOther);
            AddSpells(SummoningMasterySelf);
            AddSpells(CantripSummoningProwess);
            AddSpells(SummoningIneptitudeOther);
            AddSpells(CloakSummoningMastery);
            AddSpells(SetSummoningAptitude);
            AddSpells(ReturnToTheStronghold);
            AddSpells(ParagonsDualWieldMastery);
            AddSpells(ParagonsFinesseWeaponMastery);
            AddSpells(ParagonsHeavyWeaponMastery);
            AddSpells(ParagonsLifeMagicMastery);
            AddSpells(ParagonsLightWeaponMastery);
            AddSpells(ParagonsMissileWeaponMastery);
            AddSpells(ParagonsRecklessnessMastery);
            AddSpells(ParagonsSneakAttackMastery);
            AddSpells(ParagonsTwoHandedCombatMastery);
            AddSpells(ParagonsVoidMagicMastery);
            AddSpells(ParagonsWarMagicMastery);
            AddSpells(ParagonsDirtyFightingMastery);
            AddSpells(ParagonsWillpower);
            AddSpells(ParagonsCoordination);
            AddSpells(ParagonsEndurance);
            AddSpells(ParagonsFocus);
            AddSpells(ParagonQuickness);
            AddSpells(ParagonsStrength);
            AddSpells(ParagonsStamina);
            AddSpells(ParagonsCriticalDamageBoost);
            AddSpells(ParagonsCriticalDamageReduction);
            AddSpells(ParagonsDamageBoost);
            AddSpells(ParagonsDamageReduction);
            AddSpells(ParagonsMana);
            AddSpells(GauntletCriticalDamageBoost);
            AddSpells(GauntletDamageBoost);
            AddSpells(GauntletDamageReduction);
            AddSpells(GauntletCriticalDamageReduction);
            AddSpells(GauntletHealingBoost);
            AddSpells(GauntletVitality);
        }

        private static void AddSpells(List<SpellId> spells)
        {
            foreach (var spell in spells)
            {
                if (spell != SpellId.Undef)
                    spellProgression.Add(spell, spells);
            }
        }

        public static List<SpellId> GetSpellLevels(SpellId spellId)
        {
            return spellProgression.TryGetValue(spellId, out var progression) ? progression : null;
        }
    }
}
