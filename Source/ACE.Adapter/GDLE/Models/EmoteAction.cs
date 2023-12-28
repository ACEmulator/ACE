using Lifestoned.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ACE.Adapter.GDLE.Models
{
    public class EmoteAction
    {
        [JsonPropertyName("type")]
        public uint EmoteActionType { get; set; }

        [JsonIgnore]
        public EmoteType EmoteActionType_Binder
        {
            get
            {
                return (EmoteType)EmoteActionType;
            }
            set
            {
                EmoteActionType = (uint)value;
            }
        }

        [JsonPropertyName("delay")]
        public float? Delay { get; set; }

        [JsonPropertyName("extent")]
        public float? Extent { get; set; }

        [JsonPropertyName("amount")]
        [EmoteType(EmoteType.DecrementQuest)]
        [EmoteType(EmoteType.IncrementQuest)]
        [EmoteType(EmoteType.SetQuestCompletions)]
        [EmoteType(EmoteType.Generate)]
        [EmoteType(EmoteType.DecrementMyQuest)]
        [EmoteType(EmoteType.IncrementMyQuest)]
        [EmoteType(EmoteType.SetMyQuestCompletions)]
        [EmoteType(EmoteType.InqPackSpace)]
        [EmoteType(EmoteType.InqQuestBitsOn)]
        [EmoteType(EmoteType.InqQuestBitsOff)]
        [EmoteType(EmoteType.InqMyQuestBitsOn)]
        [EmoteType(EmoteType.InqMyQuestBitsOff)]
        [EmoteType(EmoteType.SetQuestBitsOn)]
        [EmoteType(EmoteType.SetQuestBitsOff)]
        [EmoteType(EmoteType.SetMyQuestBitsOn)]
        [EmoteType(EmoteType.SetMyQuestBitsOff)]
        [EmoteType(EmoteType.SetIntStat)]
        [EmoteType(EmoteType.IncrementIntStat)]
        [EmoteType(EmoteType.DecrementIntStat)]
        [EmoteType(EmoteType.SetBoolStat)]
        [EmoteType(EmoteType.AddCharacterTitle)]
        [EmoteType(EmoteType.AwardTrainingCredits)]
        [EmoteType(EmoteType.InflictVitaePenalty)]
        [EmoteType(EmoteType.RemoveVitaePenalty)]
        [EmoteType(EmoteType.AddContract)]
        [EmoteType(EmoteType.RemoveContract)]
        [EmoteType(EmoteType.AwardSkillXP)]
        [EmoteType(EmoteType.AwardSkillPoints)]
        [EmoteType(EmoteType.SetAltRacialSkills)]
        public uint? Amount { get; set; }

        [JsonPropertyName("motion")]
        [EmoteType(EmoteType.Motion)]
        [EmoteType(EmoteType.ForceMotion)]
        public uint? Motion { get; set; }

        [JsonIgnore]
        public MotionCommand? Motion_Binder
        {
            get
            {
                return (MotionCommand?)Motion;
            }
            set
            {
                Motion = (uint?)value;
            }
        }

        [JsonPropertyName("msg")]
        [EmoteType(EmoteType.Act)]
        [EmoteType(EmoteType.Say)]
        [EmoteType(EmoteType.Tell)]
        [EmoteType(EmoteType.TextDirect)]
        [EmoteType(EmoteType.WorldBroadcast)]
        [EmoteType(EmoteType.LocalBroadcast)]
        [EmoteType(EmoteType.DirectBroadcast)]
        [EmoteType(EmoteType.UpdateQuest)]
        [EmoteType(EmoteType.InqQuest)]
        [EmoteType(EmoteType.StampQuest)]
        [EmoteType(EmoteType.StartEvent)]
        [EmoteType(EmoteType.StopEvent)]
        [EmoteType(EmoteType.BLog)]
        [EmoteType(EmoteType.AdminSpam)]
        [EmoteType(EmoteType.EraseQuest)]
        [EmoteType(EmoteType.DecrementQuest)]
        [EmoteType(EmoteType.IncrementQuest)]
        [EmoteType(EmoteType.SetQuestCompletions)]
        [EmoteType(EmoteType.InqEvent)]
        [EmoteType(EmoteType.InqFellowQuest)]
        [EmoteType(EmoteType.UpdateFellowQuest)]
        [EmoteType(EmoteType.StampFellowQuest)]
        [EmoteType(EmoteType.TellFellow)]
        [EmoteType(EmoteType.FellowBroadcast)]
        [EmoteType(EmoteType.Goto)]
        [EmoteType(EmoteType.PopUp)]
        [EmoteType(EmoteType.InqNumCharacterTitles)]
        [EmoteType(EmoteType.UpdateMyQuest)]
        [EmoteType(EmoteType.InqMyQuest)]
        [EmoteType(EmoteType.StampMyQuest)]
        [EmoteType(EmoteType.EraseMyQuest)]
        [EmoteType(EmoteType.DecrementMyQuest)]
        [EmoteType(EmoteType.IncrementMyQuest)]
        [EmoteType(EmoteType.SetMyQuestCompletions)]
        [EmoteType(EmoteType.LocalSignal)]
        [EmoteType(EmoteType.InqPackSpace)]
        [EmoteType(EmoteType.InqQuestBitsOn)]
        [EmoteType(EmoteType.InqQuestBitsOff)]
        [EmoteType(EmoteType.InqMyQuestBitsOn)]
        [EmoteType(EmoteType.InqMyQuestBitsOff)]
        [EmoteType(EmoteType.SetQuestBitsOn)]
        [EmoteType(EmoteType.SetQuestBitsOff)]
        [EmoteType(EmoteType.SetMyQuestBitsOn)]
        [EmoteType(EmoteType.SetMyQuestBitsOff)]
        [EmoteType(EmoteType.InqContractsFull)]
        [EmoteType(EmoteType.InqQuestSolves)]
        [EmoteType(EmoteType.InqFellowNum)]
        [EmoteType(EmoteType.InqNumCharacterTitles)]
        [EmoteType(EmoteType.InqMyQuestSolves)]
        [EmoteType(EmoteType.InqOwnsItems)]
        [EmoteType(EmoteType.InqBoolStat)]
        [EmoteType(EmoteType.InqSkillTrained)]
        [EmoteType(EmoteType.InqSkillSpecialized)]
        [EmoteType(EmoteType.InqStringStat)]
        [EmoteType(EmoteType.InqYesNo)]
        [EmoteType(EmoteType.InqIntStat)]
        [EmoteType(EmoteType.InqAttributeStat)]
        [EmoteType(EmoteType.InqRawAttributeStat)]
        [EmoteType(EmoteType.InqSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqRawSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqSkillStat)]
        [EmoteType(EmoteType.InqRawSkillStat)]
        [EmoteType(EmoteType.InqInt64Stat)]
        [EmoteType(EmoteType.InqFloatStat)]
        public string Message { get; set; }

        [JsonPropertyName("amount64")]
        [EmoteType(EmoteType.AwardXP)]
        [EmoteType(EmoteType.AwardNoShareXP)]
        [EmoteType(EmoteType.SpendLuminance)]
        [EmoteType(EmoteType.AwardLuminance)]
        public long? Amount64 { get; set; }

        [JsonPropertyName("heroxp64")]
        [EmoteType(EmoteType.AwardXP)]
        [EmoteType(EmoteType.AwardNoShareXP)]
        [EmoteType(EmoteType.SpendLuminance)]
        public ulong? HeroXp64 { get; set; }

        [JsonPropertyName("cprof")]
        [EmoteType(EmoteType.Give)]
        [EmoteType(EmoteType.TakeItems)]
        [EmoteType(EmoteType.InqOwnsItems)]
        public CreateItem Item { get; set; }

        [JsonPropertyName("min64")]
        [EmoteType(EmoteType.InqInt64Stat)]
        [EmoteType(EmoteType.AwardLevelProportionalXP)]
        public long? Minimum64 { get; set; }

        [JsonPropertyName("max64")]
        [EmoteType(EmoteType.InqInt64Stat)]
        [EmoteType(EmoteType.AwardLevelProportionalXP)]
        public long? Maximum64 { get; set; }

        [JsonPropertyName("percent")]
        [EmoteType(EmoteType.SetFloatStat)]
        [EmoteType(EmoteType.AwardLevelProportionalXP)]
        [EmoteType(EmoteType.AwardLevelProportionalSkillXP)]
        public float? Percent { get; set; }

        [JsonPropertyName("display")]
        [EmoteType(EmoteType.AwardLevelProportionalXP)]
        [EmoteType(EmoteType.AwardLevelProportionalSkillXP)]
        public byte? Display_Binder { get; set; }

        [JsonIgnore]
        public bool? Display
        {
            get
            {
                return (!Display_Binder.HasValue) ? null : new bool?((Display_Binder.Value != 0) ? true : false);
            }
            set
            {
                Display_Binder = ((!value.HasValue) ? null : new byte?((byte)(value.Value ? 1 : 0)));
            }
        }

        [JsonPropertyName("max")]
        [EmoteType(EmoteType.InqQuestSolves)]
        [EmoteType(EmoteType.InqFellowNum)]
        [EmoteType(EmoteType.InqNumCharacterTitles)]
        [EmoteType(EmoteType.InqMyQuestSolves)]
        [EmoteType(EmoteType.InqIntStat)]
        [EmoteType(EmoteType.InqAttributeStat)]
        [EmoteType(EmoteType.InqRawAttributeStat)]
        [EmoteType(EmoteType.InqSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqRawSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqSkillStat)]
        [EmoteType(EmoteType.InqRawSkillStat)]
        [EmoteType(EmoteType.AwardLevelProportionalSkillXP)]
        public uint? Max { get; set; }

        [JsonPropertyName("min")]
        [EmoteType(EmoteType.InqQuestSolves)]
        [EmoteType(EmoteType.InqFellowNum)]
        [EmoteType(EmoteType.InqNumCharacterTitles)]
        [EmoteType(EmoteType.InqMyQuestSolves)]
        [EmoteType(EmoteType.InqIntStat)]
        [EmoteType(EmoteType.InqAttributeStat)]
        [EmoteType(EmoteType.InqRawAttributeStat)]
        [EmoteType(EmoteType.InqSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqRawSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqSkillStat)]
        [EmoteType(EmoteType.InqRawSkillStat)]
        [EmoteType(EmoteType.AwardLevelProportionalSkillXP)]
        public uint? Min { get; set; }

        [JsonPropertyName("fmax")]
        [EmoteType(EmoteType.InqFloatStat)]
        public float? FMax { get; set; }

        [JsonPropertyName("fmin")]
        [EmoteType(EmoteType.InqFloatStat)]
        public float? FMin { get; set; }

        [JsonPropertyName("stat")]
        [EmoteType(EmoteType.SetIntStat)]
        [EmoteType(EmoteType.IncrementIntStat)]
        [EmoteType(EmoteType.DecrementIntStat)]
        [EmoteType(EmoteType.SetBoolStat)]
        [EmoteType(EmoteType.SetInt64Stat)]
        [EmoteType(EmoteType.SetFloatStat)]
        [EmoteType(EmoteType.AwardSkillXP)]
        [EmoteType(EmoteType.AwardSkillPoints)]
        [EmoteType(EmoteType.UntrainSkill)]
        [EmoteType(EmoteType.InqBoolStat)]
        [EmoteType(EmoteType.InqSkillTrained)]
        [EmoteType(EmoteType.InqSkillSpecialized)]
        [EmoteType(EmoteType.InqStringStat)]
        [EmoteType(EmoteType.InqYesNo)]
        [EmoteType(EmoteType.InqIntStat)]
        [EmoteType(EmoteType.InqAttributeStat)]
        [EmoteType(EmoteType.InqRawAttributeStat)]
        [EmoteType(EmoteType.InqSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqRawSecondaryAttributeStat)]
        [EmoteType(EmoteType.InqSkillStat)]
        [EmoteType(EmoteType.InqRawSkillStat)]
        [EmoteType(EmoteType.InqInt64Stat)]
        [EmoteType(EmoteType.InqFloatStat)]
        [EmoteType(EmoteType.AwardLevelProportionalSkillXP)]
        public uint? Stat { get; set; }

        [JsonPropertyName("pscript")]
        [EmoteType(EmoteType.PhysScript)]
        public uint? PScript { get; set; }

        [JsonIgnore]
        public PhysicsScriptType? PScript_Binder
        {
            get
            {
                return PScript.HasValue ? new PhysicsScriptType?((PhysicsScriptType)PScript.Value) : null;
            }
            set
            {
                PScript = (uint?)value;
            }
        }

        [JsonPropertyName("sound")]
        [EmoteType(EmoteType.Sound)]
        public uint? Sound { get; set; }

        [JsonPropertyName("mPosition")]
        [EmoteType(EmoteType.SetSanctuaryPosition)]
        [EmoteType(EmoteType.TeleportTarget)]
        [EmoteType(EmoteType.TeleportSelf)]
        public Position MPosition { get; set; }

        [JsonPropertyName("frame")]
        [EmoteType(EmoteType.MoveHome)]
        [EmoteType(EmoteType.Move)]
        [EmoteType(EmoteType.Turn)]
        [EmoteType(EmoteType.MoveToPos)]
        public Frame Frame { get; set; }

        [JsonPropertyName("spellid")]
        [EmoteType(EmoteType.CastSpell)]
        [EmoteType(EmoteType.CastSpellInstant)]
        [EmoteType(EmoteType.TeachSpell)]
        [EmoteType(EmoteType.PetCastSpellOnOwner)]
        public uint? SpellId { get; set; }

        [JsonPropertyName("teststring")]
        [EmoteType(EmoteType.InqStringStat)]
        [EmoteType(EmoteType.InqYesNo)]
        public string TestString { get; set; }

        [JsonPropertyName("wealth_rating")]
        [EmoteType(EmoteType.CreateTreasure)]
        public uint? WealthRating { get; set; }

        [JsonIgnore]
        public WealthRating? WealthRating_Binder
        {
            get
            {
                return (WealthRating?)WealthRating;
            }
            set
            {
                WealthRating = (uint?)value;
            }
        }

        [JsonPropertyName("treasure_class")]
        [EmoteType(EmoteType.CreateTreasure)]
        public uint? TreasureClass { get; set; }

        [JsonIgnore]
        public TreasureClass? TreasureClass_Binder
        {
            get
            {
                return (TreasureClass?)TreasureClass;
            }
            set
            {
                TreasureClass = (uint?)value;
            }
        }

        [JsonPropertyName("treasure_type")]
        [EmoteType(EmoteType.CreateTreasure)]
        public int? TreasureType { get; set; }

        [JsonIgnore]
        public int? SortOrder { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }

        public static bool IsPropertyVisible(string propertyName, EmoteAction emote)
        {
            PropertyInfo property = typeof(EmoteAction).GetProperty(propertyName);
            List<EmoteTypeAttribute> list = property.GetCustomAttributes(typeof(EmoteTypeAttribute), inherit: false).Cast<EmoteTypeAttribute>().ToList();
            if (list == null || list.Count < 1)
            {
                return true;
            }

            return list.Any((EmoteTypeAttribute a) => a.TypeList.Contains(emote.EmoteActionType_Binder));
        }
    }
}
