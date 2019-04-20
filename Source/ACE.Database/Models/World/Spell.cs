using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Spell
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint? StatModType { get; set; }
        public uint? StatModKey { get; set; }
        public float? StatModVal { get; set; }
        public uint? EType { get; set; }
        public int? BaseIntensity { get; set; }
        public int? Variance { get; set; }
        public uint? Wcid { get; set; }
        public int? NumProjectiles { get; set; }
        public int? NumProjectilesVariance { get; set; }
        public float? SpreadAngle { get; set; }
        public float? VerticalAngle { get; set; }
        public float? DefaultLaunchAngle { get; set; }
        public bool? NonTracking { get; set; }
        public float? CreateOffsetOriginX { get; set; }
        public float? CreateOffsetOriginY { get; set; }
        public float? CreateOffsetOriginZ { get; set; }
        public float? PaddingOriginX { get; set; }
        public float? PaddingOriginY { get; set; }
        public float? PaddingOriginZ { get; set; }
        public float? DimsOriginX { get; set; }
        public float? DimsOriginY { get; set; }
        public float? DimsOriginZ { get; set; }
        public float? PeturbationOriginX { get; set; }
        public float? PeturbationOriginY { get; set; }
        public float? PeturbationOriginZ { get; set; }
        public uint? ImbuedEffect { get; set; }
        public int? SlayerCreatureType { get; set; }
        public float? SlayerDamageBonus { get; set; }
        public double? CritFreq { get; set; }
        public double? CritMultiplier { get; set; }
        public int? IgnoreMagicResist { get; set; }
        public double? ElementalModifier { get; set; }
        public float? DrainPercentage { get; set; }
        public float? DamageRatio { get; set; }
        public int? DamageType { get; set; }
        public int? Boost { get; set; }
        public int? BoostVariance { get; set; }
        public int? Source { get; set; }
        public int? Destination { get; set; }
        public float? Proportion { get; set; }
        public float? LossPercent { get; set; }
        public int? SourceLoss { get; set; }
        public int? TransferCap { get; set; }
        public int? MaxBoostAllowed { get; set; }
        public uint? TransferBitfield { get; set; }
        public int? Index { get; set; }
        public int? Link { get; set; }
        public uint? PositionObjCellId { get; set; }
        public float? PositionOriginX { get; set; }
        public float? PositionOriginY { get; set; }
        public float? PositionOriginZ { get; set; }
        public float? PositionAnglesW { get; set; }
        public float? PositionAnglesX { get; set; }
        public float? PositionAnglesY { get; set; }
        public float? PositionAnglesZ { get; set; }
        public int? MinPower { get; set; }
        public int? MaxPower { get; set; }
        public float? PowerVariance { get; set; }
        public int? DispelSchool { get; set; }
        public int? Align { get; set; }
        public int? Number { get; set; }
        public float? NumberVariance { get; set; }
        public double? DotDuration { get; set; }
        public DateTime LastModified { get; set; }
    }
}
