using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ACE.Database.Models.Shard;

public partial class ShardDbContext : DbContext
{
    public ShardDbContext()
    {
    }

    public ShardDbContext(DbContextOptions<ShardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Biota> Biota { get; set; }

    public virtual DbSet<BiotaPropertiesAllegiance> BiotaPropertiesAllegiance { get; set; }

    public virtual DbSet<BiotaPropertiesAnimPart> BiotaPropertiesAnimPart { get; set; }

    public virtual DbSet<BiotaPropertiesAttribute> BiotaPropertiesAttribute { get; set; }

    public virtual DbSet<BiotaPropertiesAttribute2nd> BiotaPropertiesAttribute2nd { get; set; }

    public virtual DbSet<BiotaPropertiesBodyPart> BiotaPropertiesBodyPart { get; set; }

    public virtual DbSet<BiotaPropertiesBook> BiotaPropertiesBook { get; set; }

    public virtual DbSet<BiotaPropertiesBookPageData> BiotaPropertiesBookPageData { get; set; }

    public virtual DbSet<BiotaPropertiesBool> BiotaPropertiesBool { get; set; }

    public virtual DbSet<BiotaPropertiesCreateList> BiotaPropertiesCreateList { get; set; }

    public virtual DbSet<BiotaPropertiesDID> BiotaPropertiesDID { get; set; }

    public virtual DbSet<BiotaPropertiesEmote> BiotaPropertiesEmote { get; set; }

    public virtual DbSet<BiotaPropertiesEmoteAction> BiotaPropertiesEmoteAction { get; set; }

    public virtual DbSet<BiotaPropertiesEnchantmentRegistry> BiotaPropertiesEnchantmentRegistry { get; set; }

    public virtual DbSet<BiotaPropertiesEventFilter> BiotaPropertiesEventFilter { get; set; }

    public virtual DbSet<BiotaPropertiesFloat> BiotaPropertiesFloat { get; set; }

    public virtual DbSet<BiotaPropertiesGenerator> BiotaPropertiesGenerator { get; set; }

    public virtual DbSet<BiotaPropertiesIID> BiotaPropertiesIID { get; set; }

    public virtual DbSet<BiotaPropertiesInt> BiotaPropertiesInt { get; set; }

    public virtual DbSet<BiotaPropertiesInt64> BiotaPropertiesInt64 { get; set; }

    public virtual DbSet<BiotaPropertiesPalette> BiotaPropertiesPalette { get; set; }

    public virtual DbSet<BiotaPropertiesPosition> BiotaPropertiesPosition { get; set; }

    public virtual DbSet<BiotaPropertiesSkill> BiotaPropertiesSkill { get; set; }

    public virtual DbSet<BiotaPropertiesSpellBook> BiotaPropertiesSpellBook { get; set; }

    public virtual DbSet<BiotaPropertiesString> BiotaPropertiesString { get; set; }

    public virtual DbSet<BiotaPropertiesTextureMap> BiotaPropertiesTextureMap { get; set; }

    public virtual DbSet<Character> Character { get; set; }

    public virtual DbSet<CharacterPropertiesContractRegistry> CharacterPropertiesContractRegistry { get; set; }

    public virtual DbSet<CharacterPropertiesFillCompBook> CharacterPropertiesFillCompBook { get; set; }

    public virtual DbSet<CharacterPropertiesFriendList> CharacterPropertiesFriendList { get; set; }

    public virtual DbSet<CharacterPropertiesQuestRegistry> CharacterPropertiesQuestRegistry { get; set; }

    public virtual DbSet<CharacterPropertiesShortcutBar> CharacterPropertiesShortcutBar { get; set; }

    public virtual DbSet<CharacterPropertiesSpellBar> CharacterPropertiesSpellBar { get; set; }

    public virtual DbSet<CharacterPropertiesSquelch> CharacterPropertiesSquelch { get; set; }

    public virtual DbSet<CharacterPropertiesTitleBook> CharacterPropertiesTitleBook { get; set; }

    public virtual DbSet<ConfigPropertiesBoolean> ConfigPropertiesBoolean { get; set; }

    public virtual DbSet<ConfigPropertiesDouble> ConfigPropertiesDouble { get; set; }

    public virtual DbSet<ConfigPropertiesLong> ConfigPropertiesLong { get; set; }

    public virtual DbSet<ConfigPropertiesString> ConfigPropertiesString { get; set; }

    public virtual DbSet<HousePermission> HousePermission { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = Common.ConfigManager.Config.MySql.Shard;

            var connectionString = $"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database};{config.ConnectionOptions}";

            optionsBuilder.UseMySql(connectionString, DatabaseManager.CachedServerVersionAutoDetect(config.Database, connectionString), builder =>
            {
                builder.EnableRetryOnFailure(10);
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Biota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota", tb => tb.HasComment("Dynamic Weenies of a Shard/World"));

            entity.HasIndex(e => e.WeenieType, "biota_type_idx");

            entity.HasIndex(e => e.WeenieClassId, "biota_wcid_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Object Id within the Shard")
                .HasColumnName("id");
            entity.Property(e => e.PopulatedCollectionFlags)
                .HasDefaultValueSql("'4294967295'")
                .HasColumnName("populated_Collection_Flags");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of the Weenie this Biota was created from")
                .HasColumnName("weenie_Class_Id");
            entity.Property(e => e.WeenieType)
                .HasComment("WeenieType for this Object")
                .HasColumnName("weenie_Type");
        });

        modelBuilder.Entity<BiotaPropertiesAllegiance>(entity =>
        {
            entity.HasKey(e => new { e.AllegianceId, e.CharacterId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_allegiance");

            entity.HasIndex(e => e.CharacterId, "FK_allegiance_character_Id");

            entity.Property(e => e.AllegianceId).HasColumnName("allegiance_Id");
            entity.Property(e => e.CharacterId).HasColumnName("character_Id");
            entity.Property(e => e.ApprovedVassal).HasColumnName("approved_Vassal");
            entity.Property(e => e.Banned).HasColumnName("banned");

            entity.HasOne(d => d.Allegiance).WithMany(p => p.BiotaPropertiesAllegiance)
                .HasForeignKey(d => d.AllegianceId)
                .HasConstraintName("FK_allegiance_biota_Id");

            entity.HasOne(d => d.Character).WithMany(p => p.BiotaPropertiesAllegiance)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_allegiance_character_Id");
        });

        modelBuilder.Entity<BiotaPropertiesAnimPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_anim_part", tb => tb.HasComment("Animation Part Changes (from PCAPs) of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_animpart_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.AnimationId).HasColumnName("animation_Id");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Order).HasColumnName("order");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesAnimPart)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_animpart");
        });

        modelBuilder.Entity<BiotaPropertiesAttribute>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_attribute", tb => tb.HasComment("Attribute Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyAttribute.????)")
                .HasColumnName("type");
            entity.Property(e => e.CPSpent)
                .HasComment("XP spent on this attribute")
                .HasColumnName("c_P_Spent");
            entity.Property(e => e.InitLevel)
                .HasComment("innate points")
                .HasColumnName("init_Level");
            entity.Property(e => e.LevelFromCP)
                .HasComment("points raised")
                .HasColumnName("level_From_C_P");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesAttribute)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_attribute");
        });

        modelBuilder.Entity<BiotaPropertiesAttribute2nd>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_attribute_2nd", tb => tb.HasComment("Attribute2nd (Vital) Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyAttribute2nd.????)")
                .HasColumnName("type");
            entity.Property(e => e.CPSpent)
                .HasComment("XP spent on this attribute")
                .HasColumnName("c_P_Spent");
            entity.Property(e => e.CurrentLevel)
                .HasComment("current value of the vital")
                .HasColumnName("current_Level");
            entity.Property(e => e.InitLevel)
                .HasComment("innate points")
                .HasColumnName("init_Level");
            entity.Property(e => e.LevelFromCP)
                .HasComment("points raised")
                .HasColumnName("level_From_C_P");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesAttribute2nd)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_attribute2nd");
        });

        modelBuilder.Entity<BiotaPropertiesBodyPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_body_part", tb => tb.HasComment("Body Part Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Key }, "wcid_bodypart_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ArmorVsAcid).HasColumnName("armor_Vs_Acid");
            entity.Property(e => e.ArmorVsBludgeon).HasColumnName("armor_Vs_Bludgeon");
            entity.Property(e => e.ArmorVsCold).HasColumnName("armor_Vs_Cold");
            entity.Property(e => e.ArmorVsElectric).HasColumnName("armor_Vs_Electric");
            entity.Property(e => e.ArmorVsFire).HasColumnName("armor_Vs_Fire");
            entity.Property(e => e.ArmorVsNether).HasColumnName("armor_Vs_Nether");
            entity.Property(e => e.ArmorVsPierce).HasColumnName("armor_Vs_Pierce");
            entity.Property(e => e.ArmorVsSlash).HasColumnName("armor_Vs_Slash");
            entity.Property(e => e.BH).HasColumnName("b_h");
            entity.Property(e => e.BaseArmor).HasColumnName("base_Armor");
            entity.Property(e => e.DType).HasColumnName("d_Type");
            entity.Property(e => e.DVal).HasColumnName("d_Val");
            entity.Property(e => e.DVar).HasColumnName("d_Var");
            entity.Property(e => e.HLB).HasColumnName("h_l_b");
            entity.Property(e => e.HLF).HasColumnName("h_l_f");
            entity.Property(e => e.HRB).HasColumnName("h_r_b");
            entity.Property(e => e.HRF).HasColumnName("h_r_f");
            entity.Property(e => e.Key)
                .HasComment("Type of Property the value applies to (PropertySkill.????)")
                .HasColumnName("key");
            entity.Property(e => e.LLB).HasColumnName("l_l_b");
            entity.Property(e => e.LLF).HasColumnName("l_l_f");
            entity.Property(e => e.LRB).HasColumnName("l_r_b");
            entity.Property(e => e.LRF).HasColumnName("l_r_f");
            entity.Property(e => e.MLB).HasColumnName("m_l_b");
            entity.Property(e => e.MLF).HasColumnName("m_l_f");
            entity.Property(e => e.MRB).HasColumnName("m_r_b");
            entity.Property(e => e.MRF).HasColumnName("m_r_f");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesBodyPart)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_bodypart");
        });

        modelBuilder.Entity<BiotaPropertiesBook>(entity =>
        {
            entity.HasKey(e => e.ObjectId).HasName("PRIMARY");

            entity.ToTable("biota_properties_book", tb => tb.HasComment("Book Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .ValueGeneratedNever()
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.MaxNumCharsPerPage)
                .HasComment("Maximum number of characters per page")
                .HasColumnName("max_Num_Chars_Per_Page");
            entity.Property(e => e.MaxNumPages)
                .HasComment("Maximum number of pages per book")
                .HasColumnName("max_Num_Pages");

            entity.HasOne(d => d.Object).WithOne(p => p.BiotaPropertiesBook)
                .HasForeignKey<BiotaPropertiesBook>(d => d.ObjectId)
                .HasConstraintName("wcid_bookdata");
        });

        modelBuilder.Entity<BiotaPropertiesBookPageData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_book_page_data", tb => tb.HasComment("Page Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.PageId }, "wcid_pageid_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.AuthorAccount)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("'prewritten'")
                .HasComment("Account Name of the Author of this page")
                .HasColumnName("author_Account");
            entity.Property(e => e.AuthorId)
                .HasComment("Id of the Author of this page")
                .HasColumnName("author_Id");
            entity.Property(e => e.AuthorName)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasComment("Character Name of the Author of this page")
                .HasColumnName("author_Name");
            entity.Property(e => e.IgnoreAuthor)
                .HasComment("if this is true, any character in the world can change the page")
                .HasColumnName("ignore_Author");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the Book object this page belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.PageId)
                .HasComment("Id of the page number for this page")
                .HasColumnName("page_Id");
            entity.Property(e => e.PageText)
                .IsRequired()
                .HasComment("Text of the Page")
                .HasColumnType("text")
                .HasColumnName("page_Text");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesBookPageData)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_pagedata");
        });

        modelBuilder.Entity<BiotaPropertiesBool>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_bool", tb => tb.HasComment("Bool Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyBool.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesBool)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_bool");
        });

        modelBuilder.Entity<BiotaPropertiesCreateList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_create_list", tb => tb.HasComment("CreateList Properties of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_createlist");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.DestinationType)
                .HasComment("Type of Destination the value applies to (DestinationType.????)")
                .HasColumnName("destination_Type");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Palette)
                .HasComment("Palette Color of Object")
                .HasColumnName("palette");
            entity.Property(e => e.Shade)
                .HasComment("Shade of Object's Palette")
                .HasColumnName("shade");
            entity.Property(e => e.StackSize)
                .HasComment("Stack Size of object to create (-1 = infinite)")
                .HasColumnName("stack_Size");
            entity.Property(e => e.TryToBond)
                .HasComment("Unused?")
                .HasColumnName("try_To_Bond");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of object to Create")
                .HasColumnName("weenie_Class_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesCreateList)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_createlist");
        });

        modelBuilder.Entity<BiotaPropertiesDID>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_d_i_d", tb => tb.HasComment("DataID Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyDataId.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesDID)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_did");
        });

        modelBuilder.Entity<BiotaPropertiesEmote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_emote", tb => tb.HasComment("Emote Properties of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_emote");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasComment("EmoteCategory")
                .HasColumnName("category");
            entity.Property(e => e.MaxHealth).HasColumnName("max_Health");
            entity.Property(e => e.MinHealth).HasColumnName("min_Health");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Probability)
                .HasComment("Probability of this EmoteSet being chosen")
                .HasColumnName("probability");
            entity.Property(e => e.Quest)
                .HasColumnType("text")
                .HasColumnName("quest");
            entity.Property(e => e.Style).HasColumnName("style");
            entity.Property(e => e.Substyle).HasColumnName("substyle");
            entity.Property(e => e.VendorType).HasColumnName("vendor_Type");
            entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesEmote)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_emote");
        });

        modelBuilder.Entity<BiotaPropertiesEmoteAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_emote_action", tb => tb.HasComment("EmoteAction Properties of Weenies"));

            entity.HasIndex(e => new { e.EmoteId, e.Order }, "wcid_category_set_order_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Amount64).HasColumnName("amount_64");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.Delay)
                .HasComment("Time to wait before EmoteAction starts execution")
                .HasColumnName("delay");
            entity.Property(e => e.DestinationType)
                .HasComment("Type of Destination the value applies to (DestinationType.????)")
                .HasColumnName("destination_Type");
            entity.Property(e => e.Display).HasColumnName("display");
            entity.Property(e => e.EmoteId)
                .HasComment("Id of the emote this property belongs to")
                .HasColumnName("emote_Id");
            entity.Property(e => e.Extent)
                .HasComment("?")
                .HasColumnName("extent");
            entity.Property(e => e.HeroXP64).HasColumnName("hero_X_P_64");
            entity.Property(e => e.Max).HasColumnName("max");
            entity.Property(e => e.Max64).HasColumnName("max_64");
            entity.Property(e => e.MaxDbl).HasColumnName("max_Dbl");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.Min).HasColumnName("min");
            entity.Property(e => e.Min64).HasColumnName("min_64");
            entity.Property(e => e.MinDbl).HasColumnName("min_Dbl");
            entity.Property(e => e.Motion).HasColumnName("motion");
            entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");
            entity.Property(e => e.Order)
                .HasComment("Emote Action Sequence Order")
                .HasColumnName("order");
            entity.Property(e => e.OriginX).HasColumnName("origin_X");
            entity.Property(e => e.OriginY).HasColumnName("origin_Y");
            entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
            entity.Property(e => e.PScript).HasColumnName("p_Script");
            entity.Property(e => e.Palette)
                .HasComment("Palette Color of Object")
                .HasColumnName("palette");
            entity.Property(e => e.Percent).HasColumnName("percent");
            entity.Property(e => e.Shade)
                .HasComment("Shade of Object's Palette")
                .HasColumnName("shade");
            entity.Property(e => e.Sound).HasColumnName("sound");
            entity.Property(e => e.SpellId).HasColumnName("spell_Id");
            entity.Property(e => e.StackSize)
                .HasComment("Stack Size of object to create (-1 = infinite)")
                .HasColumnName("stack_Size");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.TestString)
                .HasColumnType("text")
                .HasColumnName("test_String");
            entity.Property(e => e.TreasureClass).HasColumnName("treasure_Class");
            entity.Property(e => e.TreasureType).HasColumnName("treasure_Type");
            entity.Property(e => e.TryToBond)
                .HasComment("Unused?")
                .HasColumnName("try_To_Bond");
            entity.Property(e => e.Type)
                .HasComment("EmoteType")
                .HasColumnName("type");
            entity.Property(e => e.WealthRating).HasColumnName("wealth_Rating");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of object to Create")
                .HasColumnName("weenie_Class_Id");

            entity.HasOne(d => d.Emote).WithMany(p => p.BiotaPropertiesEmoteAction)
                .HasForeignKey(d => d.EmoteId)
                .HasConstraintName("emoteid_emoteaction");
        });

        modelBuilder.Entity<BiotaPropertiesEnchantmentRegistry>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.SpellId, e.CasterObjectId, e.LayerId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity.ToTable("biota_properties_enchantment_registry", tb => tb.HasComment("Enchantment Registry Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.SpellId, e.LayerId }, "wcid_enchantmentregistry_objectId_spellId_layerId_uidx").IsUnique();

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.SpellId)
                .HasComment("Id of Spell")
                .HasColumnName("spell_Id");
            entity.Property(e => e.CasterObjectId)
                .HasComment("Id of the object that cast this spell")
                .HasColumnName("caster_Object_Id");
            entity.Property(e => e.LayerId)
                .HasComment("Id of Layer")
                .HasColumnName("layer_Id");
            entity.Property(e => e.DegradeLimit)
                .HasComment("???")
                .HasColumnName("degrade_Limit");
            entity.Property(e => e.DegradeModifier)
                .HasComment("???")
                .HasColumnName("degrade_Modifier");
            entity.Property(e => e.Duration)
                .HasComment("the duration of the spell")
                .HasColumnName("duration");
            entity.Property(e => e.EnchantmentCategory)
                .HasComment("Which PackableList this Enchantment goes in (enchantmentMask)")
                .HasColumnName("enchantment_Category");
            entity.Property(e => e.HasSpellSetId)
                .HasComment("Has Spell Set Id?")
                .HasColumnName("has_Spell_Set_Id");
            entity.Property(e => e.LastTimeDegraded)
                .HasComment("the time when this enchantment was cast")
                .HasColumnName("last_Time_Degraded");
            entity.Property(e => e.PowerLevel)
                .HasComment("Power Level of Spell")
                .HasColumnName("power_Level");
            entity.Property(e => e.SpellCategory)
                .HasComment("Category of Spell")
                .HasColumnName("spell_Category");
            entity.Property(e => e.SpellSetId)
                .HasComment("Id of the Spell Set for this spell")
                .HasColumnName("spell_Set_Id");
            entity.Property(e => e.StartTime)
                .HasComment("the amount of time this enchantment has been active")
                .HasColumnName("start_Time");
            entity.Property(e => e.StatModKey)
                .HasComment("along with flags, indicates which attribute is affected by the spell")
                .HasColumnName("stat_Mod_Key");
            entity.Property(e => e.StatModType)
                .HasComment("flags that indicate the type of effect the spell has")
                .HasColumnName("stat_Mod_Type");
            entity.Property(e => e.StatModValue)
                .HasComment("the effect value/amount")
                .HasColumnName("stat_Mod_Value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesEnchantmentRegistry)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_enchantmentregistry");
        });

        modelBuilder.Entity<BiotaPropertiesEventFilter>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Event })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_event_filter", tb => tb.HasComment("EventFilter Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Event)
                .HasComment("Id of Event to filter")
                .HasColumnName("event");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesEventFilter)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_eventfilter");
        });

        modelBuilder.Entity<BiotaPropertiesFloat>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_float", tb => tb.HasComment("Float Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyFloat.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesFloat)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_float");
        });

        modelBuilder.Entity<BiotaPropertiesGenerator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_generator", tb => tb.HasComment("Generator Properties of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_generator");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.Delay)
                .HasDefaultValueSql("'0'")
                .HasComment("Amount of delay before generation")
                .HasColumnName("delay");
            entity.Property(e => e.InitCreate)
                .HasComment("Number of object to generate initially")
                .HasColumnName("init_Create");
            entity.Property(e => e.MaxCreate)
                .HasComment("Maximum amount of objects to generate")
                .HasColumnName("max_Create");
            entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.OriginX).HasColumnName("origin_X");
            entity.Property(e => e.OriginY).HasColumnName("origin_Y");
            entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
            entity.Property(e => e.PaletteId)
                .HasComment("Palette Color of Object Generated")
                .HasColumnName("palette_Id");
            entity.Property(e => e.Probability).HasColumnName("probability");
            entity.Property(e => e.Shade)
                .HasComment("Shade of Object generated's Palette")
                .HasColumnName("shade");
            entity.Property(e => e.StackSize)
                .HasComment("StackSize of object generated")
                .HasColumnName("stack_Size");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of object to generate")
                .HasColumnName("weenie_Class_Id");
            entity.Property(e => e.WhenCreate)
                .HasComment("When to generate the weenie object")
                .HasColumnName("when_Create");
            entity.Property(e => e.WhereCreate)
                .HasComment("Where to generate the weenie object")
                .HasColumnName("where_Create");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesGenerator)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_generator");
        });

        modelBuilder.Entity<BiotaPropertiesIID>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_i_i_d", tb => tb.HasComment("InstanceID Properties of Weenies"));

            entity.HasIndex(e => new { e.Type, e.Value }, "type_value_idx");

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyInstanceId.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesIID)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_iid");
        });

        modelBuilder.Entity<BiotaPropertiesInt>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_int", tb => tb.HasComment("Int Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyInt.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesInt)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_int");
        });

        modelBuilder.Entity<BiotaPropertiesInt64>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_int64", tb => tb.HasComment("Int64 Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyInt64.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesInt64)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_int64");
        });

        modelBuilder.Entity<BiotaPropertiesPalette>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_palette", tb => tb.HasComment("Palette Changes (from PCAPs) of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_palette_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Offset).HasColumnName("offset");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.SubPaletteId).HasColumnName("sub_Palette_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesPalette)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_palette");
        });

        modelBuilder.Entity<BiotaPropertiesPosition>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.PositionType })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_position", tb => tb.HasComment("Position Properties of Weenies"));

            entity.HasIndex(e => new { e.PositionType, e.ObjCellId }, "type_cell_idx");

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.PositionType)
                .HasComment("Type of Position the value applies to (PositionType.????)")
                .HasColumnName("position_Type");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");
            entity.Property(e => e.OriginX).HasColumnName("origin_X");
            entity.Property(e => e.OriginY).HasColumnName("origin_Y");
            entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesPosition)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_position");
        });

        modelBuilder.Entity<BiotaPropertiesSkill>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_skill", tb => tb.HasComment("Skill Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertySkill.????)")
                .HasColumnName("type");
            entity.Property(e => e.InitLevel)
                .HasComment("starting point for advancement of the skill (eg bonus points)")
                .HasColumnName("init_Level");
            entity.Property(e => e.LastUsedTime)
                .HasComment("time skill was last used")
                .HasColumnName("last_Used_Time");
            entity.Property(e => e.LevelFromPP)
                .HasComment("points raised")
                .HasColumnName("level_From_P_P");
            entity.Property(e => e.PP)
                .HasComment("XP spent on this skill")
                .HasColumnName("p_p");
            entity.Property(e => e.ResistanceAtLastCheck)
                .HasComment("last use difficulty")
                .HasColumnName("resistance_At_Last_Check");
            entity.Property(e => e.SAC)
                .HasComment("skill state")
                .HasColumnName("s_a_c");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesSkill)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_skill");
        });

        modelBuilder.Entity<BiotaPropertiesSpellBook>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Spell })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_spell_book", tb => tb.HasComment("SpellBook Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Spell)
                .HasComment("Id of Spell")
                .HasColumnName("spell");
            entity.Property(e => e.Probability)
                .HasComment("Chance to cast this spell")
                .HasColumnName("probability");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesSpellBook)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_spellbook");
        });

        modelBuilder.Entity<BiotaPropertiesString>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.Type })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("biota_properties_string", tb => tb.HasComment("String Properties of Weenies"));

            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyString.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .IsRequired()
                .HasComment("Value of this Property")
                .HasColumnType("text")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesString)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_string");
        });

        modelBuilder.Entity<BiotaPropertiesTextureMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("biota_properties_texture_map", tb => tb.HasComment("Texture Map Changes (from PCAPs) of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_texturemap_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.NewId).HasColumnName("new_Id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.OldId).HasColumnName("old_Id");
            entity.Property(e => e.Order).HasColumnName("order");

            entity.HasOne(d => d.Object).WithMany(p => p.BiotaPropertiesTextureMap)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_texturemap");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("character", tb => tb.HasComment("Int Properties of Weenies"));

            entity.HasIndex(e => e.AccountId, "character_account_idx");

            entity.HasIndex(e => e.Name, "character_name_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Id of the Biota for this Character")
                .HasColumnName("id");
            entity.Property(e => e.AccountId)
                .HasComment("Id of the Biota for this Character")
                .HasColumnName("account_Id");
            entity.Property(e => e.CharacterOptions1).HasColumnName("character_Options_1");
            entity.Property(e => e.CharacterOptions2).HasColumnName("character_Options_2");
            entity.Property(e => e.DefaultHairTexture).HasColumnName("default_Hair_Texture");
            entity.Property(e => e.DeleteTime)
                .HasComment("The character will be marked IsDeleted=True after this timestamp")
                .HasColumnName("delete_Time");
            entity.Property(e => e.GameplayOptions)
                .HasColumnType("blob")
                .HasColumnName("gameplay_Options");
            entity.Property(e => e.HairTexture).HasColumnName("hair_Texture");
            entity.Property(e => e.IsDeleted)
                .HasComment("Is this Character deleted?")
                .HasColumnName("is_Deleted");
            entity.Property(e => e.IsPlussed).HasColumnName("is_Plussed");
            entity.Property(e => e.LastLoginTimestamp)
                .HasComment("Timestamp the last time this character entered the world")
                .HasColumnName("last_Login_Timestamp");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasComment("Name of Character")
                .HasColumnName("name");
            entity.Property(e => e.SpellbookFilters)
                .HasDefaultValueSql("'16383'")
                .HasColumnName("spellbook_Filters");
            entity.Property(e => e.TotalLogins).HasColumnName("total_Logins");
        });

        modelBuilder.Entity<CharacterPropertiesContractRegistry>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.ContractId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_contract_registry");

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.ContractId).HasColumnName("contract_Id");
            entity.Property(e => e.DeleteContract).HasColumnName("delete_Contract");
            entity.Property(e => e.SetAsDisplayContract).HasColumnName("set_As_Display_Contract");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesContractRegistry)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("wcid_contract");
        });

        modelBuilder.Entity<CharacterPropertiesFillCompBook>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.SpellComponentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_fill_comp_book", tb => tb.HasComment("FillCompBook Properties of Weenies"));

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.SpellComponentId)
                .HasComment("Id of Spell Component")
                .HasColumnName("spell_Component_Id");
            entity.Property(e => e.QuantityToRebuy)
                .HasComment("Amount of this component to add to the buy list for repurchase")
                .HasColumnName("quantity_To_Rebuy");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesFillCompBook)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("wcid_fillcompbook");
        });

        modelBuilder.Entity<CharacterPropertiesFriendList>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.FriendId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_friend_list", tb => tb.HasComment("FriendList Properties of Weenies"));

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.FriendId)
                .HasComment("Id of Friend")
                .HasColumnName("friend_Id");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesFriendList)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("wcid_friend");
        });

        modelBuilder.Entity<CharacterPropertiesQuestRegistry>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.QuestName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_quest_registry", tb => tb.HasComment("QuestBook Properties of Weenies"));

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.QuestName)
                .HasComment("Unique Name of Quest")
                .HasColumnName("quest_Name");
            entity.Property(e => e.LastTimeCompleted)
                .HasComment("Timestamp of last successful completion")
                .HasColumnName("last_Time_Completed");
            entity.Property(e => e.NumTimesCompleted)
                .HasComment("Number of successful completions")
                .HasColumnName("num_Times_Completed");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesQuestRegistry)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("wcid_questbook");
        });

        modelBuilder.Entity<CharacterPropertiesShortcutBar>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.ShortcutBarIndex })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_shortcut_bar", tb => tb.HasComment("ShortcutBar Properties of Weenies"));

            entity.HasIndex(e => e.CharacterId, "wcid_shortcutbar_idx");

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.ShortcutBarIndex)
                .HasComment("Position (Slot) on the Shortcut Bar for this Object")
                .HasColumnName("shortcut_Bar_Index");
            entity.Property(e => e.ShortcutObjectId)
                .HasComment("Guid of the object at this Slot")
                .HasColumnName("shortcut_Object_Id");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesShortcutBar)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("wcid_shortcutbar");
        });

        modelBuilder.Entity<CharacterPropertiesSpellBar>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.SpellBarNumber, e.SpellId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("character_properties_spell_bar", tb => tb.HasComment("SpellBar Properties of Weenies"));

            entity.HasIndex(e => e.SpellBarIndex, "spellBar_idx");

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.SpellBarNumber)
                .HasComment("Id of Spell Bar")
                .HasColumnName("spell_Bar_Number");
            entity.Property(e => e.SpellId)
                .HasComment("Id of Spell on this Spell Bar at this Slot")
                .HasColumnName("spell_Id");
            entity.Property(e => e.SpellBarIndex)
                .HasComment("Position (Slot) on this Spell Bar for this Spell")
                .HasColumnName("spell_Bar_Index");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesSpellBar)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("characterId_spellbar");
        });

        modelBuilder.Entity<CharacterPropertiesSquelch>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.SquelchCharacterId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_squelch");

            entity.Property(e => e.CharacterId).HasColumnName("character_Id");
            entity.Property(e => e.SquelchCharacterId).HasColumnName("squelch_Character_Id");
            entity.Property(e => e.SquelchAccountId).HasColumnName("squelch_Account_Id");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesSquelch)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("squelch_character_Id_constraint");
        });

        modelBuilder.Entity<CharacterPropertiesTitleBook>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.TitleId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("character_properties_title_book", tb => tb.HasComment("TitleBook Properties of Weenies"));

            entity.Property(e => e.CharacterId)
                .HasComment("Id of the character this property belongs to")
                .HasColumnName("character_Id");
            entity.Property(e => e.TitleId)
                .HasComment("Id of Title")
                .HasColumnName("title_Id");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterPropertiesTitleBook)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("wcid_titlebook");
        });

        modelBuilder.Entity<ConfigPropertiesBoolean>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("config_properties_boolean");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<ConfigPropertiesDouble>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("config_properties_double");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<ConfigPropertiesLong>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("config_properties_long");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<ConfigPropertiesString>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("config_properties_string");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Value)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("value");
        });

        modelBuilder.Entity<HousePermission>(entity =>
        {
            entity.HasKey(e => new { e.HouseId, e.PlayerGuid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("house_permission");

            entity.HasIndex(e => e.HouseId, "biota_Id_house_Id_idx");

            entity.Property(e => e.HouseId)
                .HasComment("GUID of House Biota Object")
                .HasColumnName("house_Id");
            entity.Property(e => e.PlayerGuid)
                .HasComment("GUID of Player Biota Object being granted permission to this house")
                .HasColumnName("player_Guid");
            entity.Property(e => e.Storage)
                .HasComment("Permission includes access to House Storage")
                .HasColumnName("storage");

            entity.HasOne(d => d.House).WithMany(p => p.HousePermission)
                .HasForeignKey(d => d.HouseId)
                .HasConstraintName("biota_Id_house_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
