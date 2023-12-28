using Lifestoned.DataModel.DerethForever;
using Lifestoned.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace ACE.Adapter.GDLE.Models
{
    public class LSDWeenie
    {
        //public class PropertyCopier<TParent, TChild> where TParent : class where TChild : class
        //{
        //    public static void Copy(TParent parent, TChild child)
        //    {
        //        PropertyInfo[] properties = parent.GetType().GetProperties();
        //        PropertyInfo[] properties2 = child.GetType().GetProperties();
        //        PropertyInfo[] array = properties;
        //        foreach (PropertyInfo propertyInfo in array)
        //        {
        //            PropertyInfo[] array2 = properties2;
        //            foreach (PropertyInfo propertyInfo2 in array2)
        //            {
        //                if (propertyInfo.Name == propertyInfo2.Name && propertyInfo.PropertyType == propertyInfo2.PropertyType)
        //                {
        //                    propertyInfo2.SetValue(child, propertyInfo.GetValue(parent));
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        [JsonIgnore]
        public bool IsCloneMode { get; set; } = false;


        [JsonPropertyName("wcid")]
        public uint WeenieId { get; set; }

        [JsonIgnore]
        public uint WeenieClassId
        {
            get
            {
                return WeenieId;
            }
            set
            {
                WeenieId = value;
            }
        }

        [JsonPropertyName("weenieType")]
        public int WeenieTypeId { get; set; }

        [JsonIgnore]
        public WeenieType WeenieType_Binder
        {
            get
            {
                return (WeenieType)WeenieTypeId;
            }
            set
            {
                WeenieTypeId = (int)value;
            }
        }

        [JsonPropertyName("attributes")]
        public AttributeSet Attributes { get; set; }

        [JsonPropertyName("body")]
        public Body Body { get; set; }

        [JsonPropertyName("pageDataList")]
        public Book Book { get; set; }

        [JsonPropertyName("boolStats")]
        public List<BoolStat> BoolStats { get; set; } = new List<BoolStat>();


        [JsonPropertyName("intStats")]
        public List<IntStat> IntStats { get; set; } = new List<IntStat>();


        [JsonPropertyName("didStats")]
        public List<DidStat> DidStats { get; set; } = new List<DidStat>();


        [JsonPropertyName("iidStats")]
        public List<IidStat> IidStats { get; set; } = new List<IidStat>();


        [JsonPropertyName("floatStats")]
        public List<FloatStat> FloatStats { get; set; } = new List<FloatStat>();


        [JsonPropertyName("int64Stats")]
        public List<Int64Stat> Int64Stats { get; set; } = new List<Int64Stat>();


        [JsonPropertyName("stringStats")]
        public List<StringStat> StringStats { get; set; } = new List<StringStat>();


        [JsonPropertyName("createList")]
        public List<CreateItem> CreateList { get; set; }

        [JsonPropertyName("skills")]
        public List<SkillListing> Skills { get; set; }

        [JsonPropertyName("emoteTable")]
        public List<EmoteCategoryListing> EmoteTable { get; set; }

        [JsonPropertyName("spellbook")]
        public List<SpellbookEntry> Spells { get; set; }

        [JsonPropertyName("posStats")]
        public List<PositionListing> Positions { get; set; }

        [JsonPropertyName("generatorTable")]
        public List<GeneratorTable> GeneratorTable { get; set; }

        [JsonIgnore]
        public string Name => StringStats?.FirstOrDefault((StringStat p) => p.Key == 1)?.Value ?? WeenieId.ToString();

        [JsonIgnore]
        public int? ItemType => IntStats.FirstOrDefault((IntStat d) => d.Key == 1)?.Value ?? 0;

        [JsonIgnore]
        public bool HasAbilities => (ItemType & 0x10u) > 0;

        [JsonIgnore]
        public bool HasGeneratorTable
        {
            get
            {
                List<GeneratorTable> generatorTable = GeneratorTable;
                return generatorTable != null && generatorTable.Count > 0;
            }
        }

        [JsonIgnore]
        public bool HasBodyPartList => (Body?.BodyParts.Count() ?? 0) > 0;

        [JsonIgnore]
        public int? UIEffects => IntStats.FirstOrDefault((IntStat d) => d.Key == 18)?.Value ?? 0;

        [JsonIgnore]
        public uint? IconId => DidStats.FirstOrDefault((DidStat d) => d.Key == 8)?.Value;

        [JsonIgnore]
        public uint? UnderlayId => DidStats.FirstOrDefault((DidStat d) => d.Key == 52)?.Value;

        [JsonIgnore]
        public uint? OverlayId => DidStats.FirstOrDefault((DidStat d) => d.Key == 50)?.Value;

        [JsonIgnore]
        public uint? OverlaySecondaryId => DidStats.FirstOrDefault((DidStat d) => d.Key == 51)?.Value;

        [JsonIgnore]
        public WeenieCommands? MvcAction { get; set; }

        [JsonIgnore]
        public string PropertyTab { get; set; }

        [JsonIgnore]
        public IntPropertyId? NewIntPropertyId { get; set; }

        [JsonIgnore]
        public StringPropertyId? NewStringPropertyId { get; set; }

        [JsonIgnore]
        public Int64PropertyId? NewInt64PropertyId { get; set; }

        [JsonIgnore]
        public DoublePropertyId? NewDoublePropertyId { get; set; }

        [JsonIgnore]
        public DidPropertyId? NewDidPropertyId { get; set; }

        [JsonIgnore]
        public IidPropertyId? NewIidPropertyId { get; set; }

        [JsonIgnore]
        public BoolPropertyId? NewBoolPropertyId { get; set; }

        [JsonIgnore]
        public SpellId? NewSpellId { get; set; }

        [JsonIgnore]
        public PositionType? NewPositionType { get; set; }

        [JsonIgnore]
        public SkillId? NewSkillId { get; set; }

        [JsonIgnore]
        public SkillStatus? NewSkillStatus { get; set; }

        [JsonIgnore]
        public BodyPartType? NewBodyPartType { get; set; }

        [JsonIgnore]
        public EmoteCategory NewEmoteCategory { get; set; }

        [JsonIgnore]
        public int? EmoteSetGuid { get; set; }

        [JsonPropertyName("lastModified")]
        [Display(Name = "Last Modified Date")]
        public DateTime? LastModified { get; set; }

        [JsonPropertyName("modifiedBy")]
        [Display(Name = "Last Modified By")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("changelog")]
        public List<ChangelogEntry> Changelog { get; set; } = new List<ChangelogEntry>();


        [JsonPropertyName("userChangeSummary")]
        public string UserChangeSummary { get; set; }

        [JsonPropertyName("isDone")]
        [Display(Name = "Is Done")]
        public bool IsDone { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        //public void CleanDeletedAndEmptyProperties()
        //{
        //    StringStats?.RemoveAll((StringStat x) => x == null || x.Deleted || string.IsNullOrEmpty(x.Value));
        //    IntStats?.RemoveAll((IntStat x) => x?.Deleted ?? true);
        //    Int64Stats?.RemoveAll((Int64Stat x) => x?.Deleted ?? true);
        //    FloatStats?.RemoveAll((FloatStat x) => x?.Deleted ?? true);
        //    BoolStats?.RemoveAll((BoolStat x) => x?.Deleted ?? true);
        //    DidStats?.RemoveAll((DidStat x) => x?.Deleted ?? true);
        //    Spells?.RemoveAll((SpellbookEntry x) => x?.Deleted ?? true);
        //    Book?.Pages?.RemoveAll((Page x) => x?.Deleted ?? true);
        //    Positions?.RemoveAll((PositionListing x) => x?.Deleted ?? true);
        //    EmoteTable?.ForEach(delegate (EmoteCategoryListing es)
        //    {
        //        es?.Emotes?.ForEach(delegate (Emote esa)
        //        {
        //            esa?.Actions?.RemoveAll((EmoteAction x) => x?.Deleted ?? true);
        //        });
        //    });
        //    EmoteTable?.ForEach(delegate (EmoteCategoryListing es)
        //    {
        //        es?.Emotes?.RemoveAll((Emote x) => x?.Deleted ?? true);
        //    });
        //    EmoteTable?.RemoveAll(delegate (EmoteCategoryListing x)
        //    {
        //        int result;
        //        if (x != null && !x.Deleted)
        //        {
        //            List<Emote> emotes = x.Emotes;
        //            result = ((emotes != null && emotes.Count < 1) ? 1 : 0);
        //        }
        //        else
        //        {
        //            result = 1;
        //        }

        //        return (byte)result != 0;
        //    });
        //    Body?.BodyParts?.RemoveAll((BodyPartListing x) => x?.Deleted ?? true);
        //    GeneratorTable?.RemoveAll((GeneratorTable x) => x?.Deleted ?? true);
        //    CreateList?.RemoveAll((CreateItem x) => x?.Deleted ?? true);
        //    Skills?.RemoveAll((SkillListing x) => x?.Deleted ?? true);
        //    IntStats?.RemoveAll((IntStat x) => x.Key == 9007);
        //    if (!HasAbilities)
        //    {
        //        Attributes = null;
        //    }
        //}

        //public static Weenie ConvertFromWeenie(Lifestoned.DataModel.DerethForever.Weenie df)
        //{
        //    Weenie gdle = new Weenie
        //    {
        //        WeenieId = df.WeenieClassId,
        //        WeenieTypeId = df.IntProperties.First((IntProperty i) => i.IntPropertyId == 9007).Value.Value,
        //        IsDone = df.IsDone,
        //        Comments = df.Comments,
        //        UserChangeSummary = df.UserChangeSummary,
        //        Changelog = df.Changelog?.ToList(),
        //        ModifiedBy = df.ModifiedBy,
        //        LastModified = df.LastModified
        //    };
        //    List<IntProperty> intProperties = df.IntProperties;
        //    if (intProperties != null && intProperties.Count > 0)
        //    {
        //        gdle.IntStats = new List<IntStat>();
        //    }

        //    df.IntProperties?.Where((IntProperty ip) => ip.Value.HasValue).ToList().ForEach(delegate (IntProperty ip)
        //    {
        //        gdle.IntStats.Add(new IntStat
        //        {
        //            Key = ip.IntPropertyId,
        //            Value = ip.Value.Value
        //        });
        //    });
        //    List<Int64Property> int64Properties = df.Int64Properties;
        //    if (int64Properties != null && int64Properties.Count > 0)
        //    {
        //        gdle.Int64Stats = new List<Int64Stat>();
        //    }

        //    df.Int64Properties?.Where((Int64Property ip) => ip.Value.HasValue).ToList().ForEach(delegate (Int64Property ip)
        //    {
        //        gdle.Int64Stats.Add(new Int64Stat
        //        {
        //            Key = ip.Int64PropertyId,
        //            Value = ip.Value.Value
        //        });
        //    });
        //    List<DoubleProperty> doubleProperties = df.DoubleProperties;
        //    if (doubleProperties != null && doubleProperties.Count > 0)
        //    {
        //        gdle.FloatStats = new List<FloatStat>();
        //    }

        //    df.DoubleProperties?.Where((DoubleProperty dp) => dp.Value.HasValue).ToList().ForEach(delegate (DoubleProperty dp)
        //    {
        //        gdle.FloatStats.Add(new FloatStat
        //        {
        //            Key = dp.DoublePropertyId,
        //            Value = (float)dp.Value.Value
        //        });
        //    });
        //    List<BoolProperty> boolProperties = df.BoolProperties;
        //    if (boolProperties != null && boolProperties.Count > 0)
        //    {
        //        gdle.BoolStats = new List<BoolStat>();
        //    }

        //    df.BoolProperties?.ForEach(delegate (BoolProperty bp)
        //    {
        //        gdle.BoolStats.Add(new BoolStat
        //        {
        //            Key = bp.BoolPropertyId,
        //            BoolValue = bp.Value
        //        });
        //    });
        //    List<DataIdProperty> didProperties = df.DidProperties;
        //    if (didProperties != null && didProperties.Count > 0)
        //    {
        //        gdle.DidStats = new List<DidStat>();
        //    }

        //    df.DidProperties?.Where((DataIdProperty dp) => dp.Value.HasValue).ToList().ForEach(delegate (DataIdProperty dp)
        //    {
        //        gdle.DidStats.Add(new DidStat
        //        {
        //            Key = dp.DataIdPropertyId,
        //            Value = dp.Value.Value
        //        });
        //    });
        //    List<InstanceIdProperty> iidProperties = df.IidProperties;
        //    if (iidProperties != null && iidProperties.Count > 0)
        //    {
        //        gdle.IidStats = new List<IidStat>();
        //    }

        //    df.IidProperties?.Where((InstanceIdProperty dp) => dp.Value.HasValue).ToList().ForEach(delegate (InstanceIdProperty dp)
        //    {
        //        gdle.IidStats.Add(new IidStat
        //        {
        //            Key = dp.IidPropertyId,
        //            Value = dp.Value.Value
        //        });
        //    });
        //    List<StringProperty> stringProperties = df.StringProperties;
        //    if (stringProperties != null && stringProperties.Count > 0)
        //    {
        //        gdle.StringStats = new List<StringStat>();
        //    }

        //    df.StringProperties?.ForEach(delegate (StringProperty sp)
        //    {
        //        gdle.StringStats.Add(new StringStat
        //        {
        //            Key = sp.StringPropertyId,
        //            Value = sp.Value
        //        });
        //    });
        //    if (df.BookProperties.Count > 0)
        //    {
        //        gdle.Book = new Book
        //        {
        //            Pages = new List<Page>(),
        //            MaxCharactersPerPage = 1000
        //        };
        //        df.BookProperties.ForEach(delegate (BookPage bp)
        //        {
        //            gdle.Book.Pages.Add(new Page
        //            {
        //                AuthorAccount = bp.AuthorAccount,
        //                AuthorId = bp.AuthorId,
        //                AuthorName = bp.AuthorName,
        //                IgnoreAuthor = bp.IgnoreAuthor,
        //                PageText = bp.PageText.Replace("\r\n", "\n").Replace("\r\n\r\n", "\n")
        //            });
        //        });
        //        gdle.Book.MaxNumberPages = gdle.Book.Pages.Count;
        //    }

        //    if (df.HasAbilities)
        //    {
        //        gdle.Attributes = new AttributeSet
        //        {
        //            Strength = Attribute.Convert(df.Abilities.Strength),
        //            Endurance = Attribute.Convert(df.Abilities.Endurance),
        //            Coordination = Attribute.Convert(df.Abilities.Coordination),
        //            Quickness = Attribute.Convert(df.Abilities.Quickness),
        //            Focus = Attribute.Convert(df.Abilities.Focus),
        //            Self = Attribute.Convert(df.Abilities.Self),
        //            Health = Vital.Convert(df.Vitals.Health),
        //            Stamina = Vital.Convert(df.Vitals.Stamina),
        //            Mana = Vital.Convert(df.Vitals.Mana)
        //        };
        //        List<Lifestoned.DataModel.DerethForever.Skill> skills = df.Skills;
        //        if (skills != null && skills.Count > 0)
        //        {
        //            gdle.Skills = new List<SkillListing>();
        //        }

        //        df.Skills.ForEach(delegate (Lifestoned.DataModel.DerethForever.Skill s)
        //        {
        //            gdle.Skills.Add(new SkillListing
        //            {
        //                SkillId = s.SkillId,
        //                Skill = new Skill
        //                {
        //                    Ranks = s.Ranks,
        //                    TrainedLevel = s.Status,
        //                    XpInvested = (s.ExperienceSpent ?? 0),
        //                    ResistanceOfLastCheck = 0u,
        //                    LevelFromPp = 0u,
        //                    LastUsed = 0f
        //                }
        //            });
        //        });
        //    }

        //    List<Lifestoned.DataModel.DerethForever.Position> positions = df.Positions;
        //    if (positions != null && positions.Count > 0)
        //    {
        //        gdle.Positions = new List<PositionListing>();
        //    }

        //    df.Positions?.ForEach(delegate (Lifestoned.DataModel.DerethForever.Position p)
        //    {
        //        gdle.Positions.Add(new PositionListing
        //        {
        //            PositionType = p.PositionType,
        //            Position = new Position
        //            {
        //                Frame = new Frame
        //                {
        //                    Position = new XYZ
        //                    {
        //                        X = p.X,
        //                        Y = p.Y,
        //                        Z = p.Z
        //                    },
        //                    Rotations = new Quaternion
        //                    {
        //                        W = p.QW,
        //                        X = p.QX,
        //                        Y = p.QY,
        //                        Z = p.QZ
        //                    }
        //                },
        //                LandCellId = p.Landblock
        //            }
        //        });
        //    });
        //    List<Spell> spells = df.Spells;
        //    if (spells != null && spells.Count > 0)
        //    {
        //        gdle.Spells = new List<SpellbookEntry>();
        //    }

        //    df.Spells?.ForEach(delegate (Spell s)
        //    {
        //        gdle.Spells.Add(new SpellbookEntry
        //        {
        //            SpellId = s.SpellId,
        //            Stats = new SpellCastingStats
        //            {
        //                CastingChance = s.Probability
        //            }
        //        });
        //    });
        //    List<CreationProfile> createList = df.CreateList;
        //    if (createList != null && createList.Count > 0)
        //    {
        //        gdle.CreateList = new List<CreateItem>();
        //    }

        //    df.CreateList?.ForEach(delegate (CreationProfile cl)
        //    {
        //        gdle.CreateList.Add(new CreateItem
        //        {
        //            Destination = cl.Destination,
        //            Palette = cl.Palette,
        //            Shade = cl.Shade,
        //            StackSize = cl.StackSize,
        //            TryToBond = (byte)((cl.TryToBond.HasValue && cl.TryToBond.Value) ? 1 : 0),
        //            WeenieClassId = cl.WeenieClassId
        //        });
        //    });
        //    List<Lifestoned.DataModel.DerethForever.GeneratorTable> generatorTable = df.GeneratorTable;
        //    if (generatorTable != null && generatorTable.Count > 0)
        //    {
        //        gdle.GeneratorTable = new List<GeneratorTable>();
        //    }

        //    df.GeneratorTable?.ForEach(delegate (Lifestoned.DataModel.DerethForever.GeneratorTable gt)
        //    {
        //        GeneratorTable generatorTable2 = new GeneratorTable
        //        {
        //            Delay = gt.Delay,
        //            InitCreate = gt.InitCreate,
        //            MaxNumber = gt.MaxNumber,
        //            ObjectCell = gt.ObjectCell,
        //            PaletteId = gt.PaletteId,
        //            Probability = gt.Probability,
        //            Shade = gt.Shade,
        //            Slot = gt.Slot,
        //            StackSize = gt.StackSize,
        //            WeenieClassId = gt.WeenieClassId,
        //            WhenCreate = gt.WhenCreate,
        //            WhereCreate = gt.WhereCreate
        //        };
        //        generatorTable2.Frame = new Frame
        //        {
        //            Position = new XYZ(),
        //            Rotations = new Quaternion
        //            {
        //                W = gt.Frame.Angles.W,
        //                X = gt.Frame.Angles.X,
        //                Y = gt.Frame.Angles.Y,
        //                Z = gt.Frame.Angles.Z
        //            }
        //        };
        //        generatorTable2.Frame.Position.X = gt.Frame.Origin.X;
        //        generatorTable2.Frame.Position.Y = gt.Frame.Origin.Y;
        //        generatorTable2.Frame.Position.X = gt.Frame.Origin.X;
        //        gdle.GeneratorTable.Add(generatorTable2);
        //    });
        //    List<EmoteSet> emoteTable = df.EmoteTable;
        //    if (emoteTable != null && emoteTable.Count > 0)
        //    {
        //        gdle.EmoteTable = new List<EmoteCategoryListing>();
        //        foreach (EmoteSet es in df.EmoteTable)
        //        {
        //            if (!gdle.EmoteTable.Any((EmoteCategoryListing et) => et.EmoteCategoryId == (int)es.EmoteCategoryId))
        //            {
        //                gdle.EmoteTable.Add(new EmoteCategoryListing
        //                {
        //                    EmoteCategoryId = (int)es.EmoteCategoryId,
        //                    Emotes = new List<Emote>()
        //                });
        //            }

        //            EmoteCategoryListing emoteCategoryListing = gdle.EmoteTable.First((EmoteCategoryListing et) => et.EmoteCategoryId == (int)es.EmoteCategoryId);
        //            Emote pwnEmote = new Emote
        //            {
        //                ClassId = es.ClassId,
        //                Category = es.EmoteCategoryId,
        //                MaxHealth = es.MaxHealth,
        //                MinHealth = es.MinHealth,
        //                Probability = es.Probability,
        //                Quest = es.Quest,
        //                Style = es.Style,
        //                SubStyle = es.SubStyle,
        //                VendorType = es.VendorType,
        //                Actions = new List<EmoteAction>()
        //            };
        //            es.Emotes.ForEach(delegate (Lifestoned.DataModel.DerethForever.Emote dfEmote)
        //            {
        //                EmoteAction emoteAction = new EmoteAction
        //                {
        //                    Amount = dfEmote.Amount,
        //                    Amount64 = dfEmote.Amount64,
        //                    Delay = dfEmote.Delay,
        //                    Display = dfEmote.Display,
        //                    EmoteActionType = dfEmote.EmoteTypeId,
        //                    Extent = dfEmote.Extent,
        //                    FMax = dfEmote.MaximumFloat,
        //                    FMin = dfEmote.MinimumFloat,
        //                    HeroXp64 = dfEmote.HeroXp64,
        //                    Max = dfEmote.Maximum,
        //                    Maximum64 = dfEmote.Maximum64,
        //                    Message = dfEmote.Message,
        //                    Min = dfEmote.Minimum,
        //                    Minimum64 = dfEmote.Minimum64,
        //                    Motion = dfEmote.MotionId,
        //                    Percent = (float?)dfEmote.Percent,
        //                    PScript = dfEmote.PhysicsScriptId,
        //                    Sound = dfEmote.Sound,
        //                    SpellId = dfEmote.SpellId,
        //                    Stat = dfEmote.Stat,
        //                    TestString = dfEmote.TestString,
        //                    TreasureClass = dfEmote.TreasureClassId,
        //                    TreasureType = (int?)dfEmote.TreasureType,
        //                    WealthRating = dfEmote.WealthRatingId
        //                };
        //                if (dfEmote.PositionLandBlockId.HasValue)
        //                {
        //                    emoteAction.MPosition = new Position();
        //                }

        //                if (dfEmote.PositionX.HasValue)
        //                {
        //                    emoteAction.MPosition = new Position();
        //                    emoteAction.MPosition.Frame = new Frame
        //                    {
        //                        Position = new XYZ
        //                        {
        //                            X = dfEmote.PositionX.Value,
        //                            Y = dfEmote.PositionY.Value,
        //                            Z = dfEmote.PositionZ.Value
        //                        },
        //                        Rotations = new Quaternion
        //                        {
        //                            W = dfEmote.RotationW.Value,
        //                            X = dfEmote.RotationX.Value,
        //                            Y = dfEmote.RotationY.Value,
        //                            Z = dfEmote.RotationZ.Value
        //                        }
        //                    };
        //                }

        //                if (dfEmote.CreationProfile != null && Lifestoned.DataModel.DerethForever.Emote.IsPropertyVisible("CreationProfile", dfEmote.EmoteType))
        //                {
        //                    emoteAction.Item = new CreateItem
        //                    {
        //                        Destination = dfEmote.CreationProfile.Destination,
        //                        Palette = dfEmote.CreationProfile.Palette,
        //                        Shade = dfEmote.CreationProfile.Shade,
        //                        StackSize = dfEmote.CreationProfile.StackSize,
        //                        TryToBond = ((dfEmote.CreationProfile.TryToBond.HasValue && dfEmote.CreationProfile.TryToBond.Value) ? new byte?(1) : new byte?(0)),
        //                        WeenieClassId = dfEmote.CreationProfile.WeenieClassId
        //                    };
        //                }

        //                pwnEmote.Actions.Add(emoteAction);
        //            });
        //            emoteCategoryListing.Emotes.Add(pwnEmote);
        //        }
        //    }

        //    List<Lifestoned.DataModel.DerethForever.BodyPart> bodyParts = df.BodyParts;
        //    if (bodyParts != null && bodyParts.Count > 0)
        //    {
        //        gdle.Body = new Body
        //        {
        //            BodyParts = new List<BodyPartListing>()
        //        };
        //    }

        //    df.BodyParts?.ForEach(delegate (Lifestoned.DataModel.DerethForever.BodyPart bp)
        //    {
        //        gdle.Body.BodyParts.Add(new BodyPartListing
        //        {
        //            Key = (int)bp.BodyPartType,
        //            BodyPart = new BodyPart
        //            {
        //                BH = bp.BodyHeight,
        //                DType = (int)bp.DamageType,
        //                DVal = bp.Damage,
        //                DVar = bp.DamageVariance,
        //                ArmorValues = new ArmorValues
        //                {
        //                    ArmorVsAcid = bp.ArmorValues.Acid,
        //                    ArmorVsBludgeon = bp.ArmorValues.Bludgeon,
        //                    ArmorVsCold = bp.ArmorValues.Cold,
        //                    ArmorVsElectric = bp.ArmorValues.Electric,
        //                    ArmorVsFire = bp.ArmorValues.Fire,
        //                    ArmorVsNether = bp.ArmorValues.Nether,
        //                    ArmorVsPierce = bp.ArmorValues.Pierce,
        //                    ArmorVsSlash = bp.ArmorValues.Slash,
        //                    BaseArmor = bp.ArmorValues.Base
        //                },
        //                SD = new Zones
        //                {
        //                    HLB = bp.TargetingData.HighLeftBack,
        //                    HLF = bp.TargetingData.HighLeftFront,
        //                    HRB = bp.TargetingData.HighRightBack,
        //                    HRF = bp.TargetingData.HighRightFront,
        //                    LLB = bp.TargetingData.LowLeftBack,
        //                    LLF = bp.TargetingData.LowLeftFront,
        //                    LRB = bp.TargetingData.LowRightBack,
        //                    LRF = bp.TargetingData.LowRightFront,
        //                    MLB = bp.TargetingData.MidLeftBack,
        //                    MLF = bp.TargetingData.MidLeftFront,
        //                    MRB = bp.TargetingData.MidRightBack,
        //                    MRF = bp.TargetingData.MidRightFront
        //                }
        //            }
        //        });
        //    });
        //    return gdle;
        //}
    }
}
