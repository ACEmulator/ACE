using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ACE.Database.Models.Shard
{
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

                var connectionString = $"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database};TreatTinyAsBoolean=False;SslMode=None;AllowPublicKeyRetrieval=true;ApplicationName=ACEmulator";

                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
                {
                    builder.EnableRetryOnFailure(10);
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Biota>(entity =>
            {
                entity.ToTable("biota");

                entity.HasComment("Dynamic Weenies of a Shard/World");

                entity.HasIndex(e => e.WeenieType, "biota_type_idx");

                entity.HasIndex(e => e.WeenieClassId, "biota_wcid_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Object Id within the Shard");

                entity.Property(e => e.PopulatedCollectionFlags)
                    .HasColumnName("populated_Collection_Flags")
                    .HasDefaultValueSql("'4294967295'");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of the Weenie this Biota was created from");

                entity.Property(e => e.WeenieType)
                    .HasColumnName("weenie_Type")
                    .HasComment("WeenieType for this Object");
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

                entity.HasOne(d => d.Allegiance)
                    .WithMany(p => p.BiotaPropertiesAllegiance)
                    .HasForeignKey(d => d.AllegianceId)
                    .HasConstraintName("FK_allegiance_biota_Id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.BiotaPropertiesAllegiance)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("FK_allegiance_character_Id");
            });

            modelBuilder.Entity<BiotaPropertiesAnimPart>(entity =>
            {
                entity.ToTable("biota_properties_anim_part");

                entity.HasComment("Animation Part Changes (from PCAPs) of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_animpart_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.AnimationId).HasColumnName("animation_Id");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesAnimPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_animpart");
            });

            modelBuilder.Entity<BiotaPropertiesAttribute>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_attribute");

                entity.HasComment("Attribute Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyAttribute.????)");

                entity.Property(e => e.CPSpent)
                    .HasColumnName("c_P_Spent")
                    .HasComment("XP spent on this attribute");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasComment("innate points");

                entity.Property(e => e.LevelFromCP)
                    .HasColumnName("level_From_C_P")
                    .HasComment("points raised");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesAttribute)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute");
            });

            modelBuilder.Entity<BiotaPropertiesAttribute2nd>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_attribute_2nd");

                entity.HasComment("Attribute2nd (Vital) Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyAttribute2nd.????)");

                entity.Property(e => e.CPSpent)
                    .HasColumnName("c_P_Spent")
                    .HasComment("XP spent on this attribute");

                entity.Property(e => e.CurrentLevel)
                    .HasColumnName("current_Level")
                    .HasComment("current value of the vital");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasComment("innate points");

                entity.Property(e => e.LevelFromCP)
                    .HasColumnName("level_From_C_P")
                    .HasComment("points raised");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesAttribute2nd)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute2nd");
            });

            modelBuilder.Entity<BiotaPropertiesBodyPart>(entity =>
            {
                entity.ToTable("biota_properties_body_part");

                entity.HasComment("Body Part Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Key }, "wcid_bodypart_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .HasColumnName("key")
                    .HasComment("Type of Property the value applies to (PropertySkill.????)");

                entity.Property(e => e.LLB).HasColumnName("l_l_b");

                entity.Property(e => e.LLF).HasColumnName("l_l_f");

                entity.Property(e => e.LRB).HasColumnName("l_r_b");

                entity.Property(e => e.LRF).HasColumnName("l_r_f");

                entity.Property(e => e.MLB).HasColumnName("m_l_b");

                entity.Property(e => e.MLF).HasColumnName("m_l_f");

                entity.Property(e => e.MRB).HasColumnName("m_r_b");

                entity.Property(e => e.MRF).HasColumnName("m_r_f");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesBodyPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bodypart");
            });

            modelBuilder.Entity<BiotaPropertiesBook>(entity =>
            {
                entity.HasKey(e => e.ObjectId)
                    .HasName("PRIMARY");

                entity.ToTable("biota_properties_book");

                entity.HasComment("Book Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .ValueGeneratedNever()
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.MaxNumCharsPerPage)
                    .HasColumnName("max_Num_Chars_Per_Page")
                    .HasComment("Maximum number of characters per page");

                entity.Property(e => e.MaxNumPages)
                    .HasColumnName("max_Num_Pages")
                    .HasComment("Maximum number of pages per book");

                entity.HasOne(d => d.Object)
                    .WithOne(p => p.BiotaPropertiesBook)
                    .HasForeignKey<BiotaPropertiesBook>(d => d.ObjectId)
                    .HasConstraintName("wcid_bookdata");
            });

            modelBuilder.Entity<BiotaPropertiesBookPageData>(entity =>
            {
                entity.ToTable("biota_properties_book_page_data");

                entity.HasComment("Page Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.PageId }, "wcid_pageid_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.AuthorAccount)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("author_Account")
                    .HasDefaultValueSql("'prewritten'")
                    .HasComment("Account Name of the Author of this page");

                entity.Property(e => e.AuthorId)
                    .HasColumnName("author_Id")
                    .HasComment("Id of the Author of this page");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("author_Name")
                    .HasDefaultValueSql("''")
                    .HasComment("Character Name of the Author of this page");

                entity.Property(e => e.IgnoreAuthor)
                    .HasColumnName("ignore_Author")
                    .HasComment("if this is true, any character in the world can change the page");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the Book object this page belongs to");

                entity.Property(e => e.PageId)
                    .HasColumnName("page_Id")
                    .HasComment("Id of the page number for this page");

                entity.Property(e => e.PageText)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("page_Text")
                    .HasComment("Text of the Page");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesBookPageData)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_pagedata");
            });

            modelBuilder.Entity<BiotaPropertiesBool>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_bool");

                entity.HasComment("Bool Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyBool.????)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesBool)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bool");
            });

            modelBuilder.Entity<BiotaPropertiesCreateList>(entity =>
            {
                entity.ToTable("biota_properties_create_list");

                entity.HasComment("CreateList Properties of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_createlist");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.DestinationType)
                    .HasColumnName("destination_Type")
                    .HasComment("Type of Destination the value applies to (DestinationType.????)");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Palette)
                    .HasColumnName("palette")
                    .HasComment("Palette Color of Object");

                entity.Property(e => e.Shade)
                    .HasColumnName("shade")
                    .HasComment("Shade of Object's Palette");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasComment("Stack Size of object to create (-1 = infinite)");

                entity.Property(e => e.TryToBond)
                    .HasColumnName("try_To_Bond")
                    .HasComment("Unused?");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of object to Create");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesCreateList)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_createlist");
            });

            modelBuilder.Entity<BiotaPropertiesDID>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_d_i_d");

                entity.HasComment("DataID Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyDataId.????)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesDID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_did");
            });

            modelBuilder.Entity<BiotaPropertiesEmote>(entity =>
            {
                entity.ToTable("biota_properties_emote");

                entity.HasComment("Emote Properties of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_emote");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasComment("EmoteCategory");

                entity.Property(e => e.MaxHealth).HasColumnName("max_Health");

                entity.Property(e => e.MinHealth).HasColumnName("min_Health");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasComment("Probability of this EmoteSet being chosen");

                entity.Property(e => e.Quest)
                    .HasColumnType("text")
                    .HasColumnName("quest");

                entity.Property(e => e.Style).HasColumnName("style");

                entity.Property(e => e.Substyle).HasColumnName("substyle");

                entity.Property(e => e.VendorType).HasColumnName("vendor_Type");

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEmote)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emote");
            });

            modelBuilder.Entity<BiotaPropertiesEmoteAction>(entity =>
            {
                entity.ToTable("biota_properties_emote_action");

                entity.HasComment("EmoteAction Properties of Weenies");

                entity.HasIndex(e => new { e.EmoteId, e.Order }, "wcid_category_set_order_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Amount64).HasColumnName("amount_64");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.Delay)
                    .HasColumnName("delay")
                    .HasComment("Time to wait before EmoteAction starts execution");

                entity.Property(e => e.DestinationType)
                    .HasColumnName("destination_Type")
                    .HasComment("Type of Destination the value applies to (DestinationType.????)");

                entity.Property(e => e.Display).HasColumnName("display");

                entity.Property(e => e.EmoteId)
                    .HasColumnName("emote_Id")
                    .HasComment("Id of the emote this property belongs to");

                entity.Property(e => e.Extent)
                    .HasColumnName("extent")
                    .HasComment("?");

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
                    .HasColumnName("order")
                    .HasComment("Emote Action Sequence Order");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.PScript).HasColumnName("p_Script");

                entity.Property(e => e.Palette)
                    .HasColumnName("palette")
                    .HasComment("Palette Color of Object");

                entity.Property(e => e.Percent).HasColumnName("percent");

                entity.Property(e => e.Shade)
                    .HasColumnName("shade")
                    .HasComment("Shade of Object's Palette");

                entity.Property(e => e.Sound).HasColumnName("sound");

                entity.Property(e => e.SpellId).HasColumnName("spell_Id");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasComment("Stack Size of object to create (-1 = infinite)");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.TestString)
                    .HasColumnType("text")
                    .HasColumnName("test_String");

                entity.Property(e => e.TreasureClass).HasColumnName("treasure_Class");

                entity.Property(e => e.TreasureType).HasColumnName("treasure_Type");

                entity.Property(e => e.TryToBond)
                    .HasColumnName("try_To_Bond")
                    .HasComment("Unused?");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("EmoteType");

                entity.Property(e => e.WealthRating).HasColumnName("wealth_Rating");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of object to Create");

                entity.HasOne(d => d.Emote)
                    .WithMany(p => p.BiotaPropertiesEmoteAction)
                    .HasForeignKey(d => d.EmoteId)
                    .HasConstraintName("emoteid_emoteaction");
            });

            modelBuilder.Entity<BiotaPropertiesEnchantmentRegistry>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.SpellId, e.CasterObjectId, e.LayerId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

                entity.ToTable("biota_properties_enchantment_registry");

                entity.HasComment("Enchantment Registry Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.SpellId, e.LayerId }, "wcid_enchantmentregistry_objectId_spellId_layerId_uidx")
                    .IsUnique();

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.SpellId)
                    .HasColumnName("spell_Id")
                    .HasComment("Id of Spell");

                entity.Property(e => e.CasterObjectId)
                    .HasColumnName("caster_Object_Id")
                    .HasComment("Id of the object that cast this spell");

                entity.Property(e => e.LayerId)
                    .HasColumnName("layer_Id")
                    .HasComment("Id of Layer");

                entity.Property(e => e.DegradeLimit)
                    .HasColumnName("degrade_Limit")
                    .HasComment("???");

                entity.Property(e => e.DegradeModifier)
                    .HasColumnName("degrade_Modifier")
                    .HasComment("???");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasComment("the duration of the spell");

                entity.Property(e => e.EnchantmentCategory)
                    .HasColumnName("enchantment_Category")
                    .HasComment("Which PackableList this Enchantment goes in (enchantmentMask)");

                entity.Property(e => e.HasSpellSetId)
                    .HasColumnName("has_Spell_Set_Id")
                    .HasComment("Has Spell Set Id?");

                entity.Property(e => e.LastTimeDegraded)
                    .HasColumnName("last_Time_Degraded")
                    .HasComment("the time when this enchantment was cast");

                entity.Property(e => e.PowerLevel)
                    .HasColumnName("power_Level")
                    .HasComment("Power Level of Spell");

                entity.Property(e => e.SpellCategory)
                    .HasColumnName("spell_Category")
                    .HasComment("Category of Spell");

                entity.Property(e => e.SpellSetId)
                    .HasColumnName("spell_Set_Id")
                    .HasComment("Id of the Spell Set for this spell");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_Time")
                    .HasComment("the amount of time this enchantment has been active");

                entity.Property(e => e.StatModKey)
                    .HasColumnName("stat_Mod_Key")
                    .HasComment("along with flags, indicates which attribute is affected by the spell");

                entity.Property(e => e.StatModType)
                    .HasColumnName("stat_Mod_Type")
                    .HasComment("flags that indicate the type of effect the spell has");

                entity.Property(e => e.StatModValue)
                    .HasColumnName("stat_Mod_Value")
                    .HasComment("the effect value/amount");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEnchantmentRegistry)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_enchantmentregistry");
            });

            modelBuilder.Entity<BiotaPropertiesEventFilter>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Event })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_event_filter");

                entity.HasComment("EventFilter Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasComment("Id of Event to filter");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEventFilter)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_eventfilter");
            });

            modelBuilder.Entity<BiotaPropertiesFloat>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_float");

                entity.HasComment("Float Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyFloat.????)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesFloat)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_float");
            });

            modelBuilder.Entity<BiotaPropertiesGenerator>(entity =>
            {
                entity.ToTable("biota_properties_generator");

                entity.HasComment("Generator Properties of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_generator");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.Delay)
                    .HasColumnName("delay")
                    .HasDefaultValueSql("'0'")
                    .HasComment("Amount of delay before generation");

                entity.Property(e => e.InitCreate)
                    .HasColumnName("init_Create")
                    .HasComment("Number of object to generate initially");

                entity.Property(e => e.MaxCreate)
                    .HasColumnName("max_Create")
                    .HasComment("Maximum amount of objects to generate");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.PaletteId)
                    .HasColumnName("palette_Id")
                    .HasComment("Palette Color of Object Generated");

                entity.Property(e => e.Probability).HasColumnName("probability");

                entity.Property(e => e.Shade)
                    .HasColumnName("shade")
                    .HasComment("Shade of Object generated's Palette");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasComment("StackSize of object generated");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of object to generate");

                entity.Property(e => e.WhenCreate)
                    .HasColumnName("when_Create")
                    .HasComment("When to generate the weenie object");

                entity.Property(e => e.WhereCreate)
                    .HasColumnName("where_Create")
                    .HasComment("Where to generate the weenie object");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesGenerator)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_generator");
            });

            modelBuilder.Entity<BiotaPropertiesIID>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_i_i_d");

                entity.HasComment("InstanceID Properties of Weenies");

                entity.HasIndex(e => new { e.Type, e.Value }, "type_value_idx");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyInstanceId.????)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesIID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_iid");
            });

            modelBuilder.Entity<BiotaPropertiesInt>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_int");

                entity.HasComment("Int Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyInt.????)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesInt)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int");
            });

            modelBuilder.Entity<BiotaPropertiesInt64>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_int64");

                entity.HasComment("Int64 Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyInt64.????)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesInt64)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int64");
            });

            modelBuilder.Entity<BiotaPropertiesPalette>(entity =>
            {
                entity.ToTable("biota_properties_palette");

                entity.HasComment("Palette Changes (from PCAPs) of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_palette_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Offset).HasColumnName("offset");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.SubPaletteId).HasColumnName("sub_Palette_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesPalette)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_palette");
            });

            modelBuilder.Entity<BiotaPropertiesPosition>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.PositionType })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_position");

                entity.HasComment("Position Properties of Weenies");

                entity.HasIndex(e => new { e.PositionType, e.ObjCellId }, "type_cell_idx");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.PositionType)
                    .HasColumnName("position_Type")
                    .HasComment("Type of Position the value applies to (PositionType.????)");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesPosition)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_position");
            });

            modelBuilder.Entity<BiotaPropertiesSkill>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_skill");

                entity.HasComment("Skill Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertySkill.????)");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasComment("starting point for advancement of the skill (eg bonus points)");

                entity.Property(e => e.LastUsedTime)
                    .HasColumnName("last_Used_Time")
                    .HasComment("time skill was last used");

                entity.Property(e => e.LevelFromPP)
                    .HasColumnName("level_From_P_P")
                    .HasComment("points raised");

                entity.Property(e => e.PP)
                    .HasColumnName("p_p")
                    .HasComment("XP spent on this skill");

                entity.Property(e => e.ResistanceAtLastCheck)
                    .HasColumnName("resistance_At_Last_Check")
                    .HasComment("last use difficulty");

                entity.Property(e => e.SAC)
                    .HasColumnName("s_a_c")
                    .HasComment("skill state");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesSkill)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_skill");
            });

            modelBuilder.Entity<BiotaPropertiesSpellBook>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Spell })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_spell_book");

                entity.HasComment("SpellBook Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Spell)
                    .HasColumnName("spell")
                    .HasComment("Id of Spell");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasComment("Chance to cast this spell");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesSpellBook)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_spellbook");
            });

            modelBuilder.Entity<BiotaPropertiesString>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("biota_properties_string");

                entity.HasComment("String Properties of Weenies");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyString.????)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("value")
                    .HasComment("Value of this Property");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesString)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_string");
            });

            modelBuilder.Entity<BiotaPropertiesTextureMap>(entity =>
            {
                entity.ToTable("biota_properties_texture_map");

                entity.HasComment("Texture Map Changes (from PCAPs) of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_texturemap_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.NewId).HasColumnName("new_Id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.OldId).HasColumnName("old_Id");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesTextureMap)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_texturemap");
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("character");

                entity.HasComment("Int Properties of Weenies");

                entity.HasIndex(e => e.AccountId, "character_account_idx");

                entity.HasIndex(e => e.Name, "character_name_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id")
                    .HasComment("Id of the Biota for this Character");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_Id")
                    .HasComment("Id of the Biota for this Character");

                entity.Property(e => e.CharacterOptions1).HasColumnName("character_Options_1");

                entity.Property(e => e.CharacterOptions2).HasColumnName("character_Options_2");

                entity.Property(e => e.DefaultHairTexture).HasColumnName("default_Hair_Texture");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("delete_Time")
                    .HasComment("The character will be marked IsDeleted=True after this timestamp");

                entity.Property(e => e.GameplayOptions)
                    .HasColumnType("blob")
                    .HasColumnName("gameplay_Options");

                entity.Property(e => e.HairTexture).HasColumnName("hair_Texture");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_Deleted")
                    .HasComment("Is this Character deleted?");

                entity.Property(e => e.IsPlussed).HasColumnName("is_Plussed");

                entity.Property(e => e.LastLoginTimestamp)
                    .HasColumnName("last_Login_Timestamp")
                    .HasComment("Timestamp the last time this character entered the world");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasComment("Name of Character");

                entity.Property(e => e.SpellbookFilters)
                    .HasColumnName("spellbook_Filters")
                    .HasDefaultValueSql("'16383'");

                entity.Property(e => e.TotalLogins).HasColumnName("total_Logins");
            });

            modelBuilder.Entity<CharacterPropertiesContractRegistry>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.ContractId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("character_properties_contract_registry");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.ContractId).HasColumnName("contract_Id");

                entity.Property(e => e.DeleteContract).HasColumnName("delete_Contract");

                entity.Property(e => e.SetAsDisplayContract).HasColumnName("set_As_Display_Contract");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesContractRegistry)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("wcid_contract");
            });

            modelBuilder.Entity<CharacterPropertiesFillCompBook>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.SpellComponentId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("character_properties_fill_comp_book");

                entity.HasComment("FillCompBook Properties of Weenies");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.SpellComponentId)
                    .HasColumnName("spell_Component_Id")
                    .HasComment("Id of Spell Component");

                entity.Property(e => e.QuantityToRebuy)
                    .HasColumnName("quantity_To_Rebuy")
                    .HasComment("Amount of this component to add to the buy list for repurchase");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesFillCompBook)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("wcid_fillcompbook");
            });

            modelBuilder.Entity<CharacterPropertiesFriendList>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.FriendId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("character_properties_friend_list");

                entity.HasComment("FriendList Properties of Weenies");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.FriendId)
                    .HasColumnName("friend_Id")
                    .HasComment("Id of Friend");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesFriendList)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("wcid_friend");
            });

            modelBuilder.Entity<CharacterPropertiesQuestRegistry>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.QuestName })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("character_properties_quest_registry");

                entity.HasComment("QuestBook Properties of Weenies");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.QuestName)
                    .HasColumnName("quest_Name")
                    .HasComment("Unique Name of Quest");

                entity.Property(e => e.LastTimeCompleted)
                    .HasColumnName("last_Time_Completed")
                    .HasComment("Timestamp of last successful completion");

                entity.Property(e => e.NumTimesCompleted)
                    .HasColumnName("num_Times_Completed")
                    .HasComment("Number of successful completions");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesQuestRegistry)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("wcid_questbook");
            });

            modelBuilder.Entity<CharacterPropertiesShortcutBar>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.ShortcutBarIndex })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("character_properties_shortcut_bar");

                entity.HasComment("ShortcutBar Properties of Weenies");

                entity.HasIndex(e => e.CharacterId, "wcid_shortcutbar_idx");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.ShortcutBarIndex)
                    .HasColumnName("shortcut_Bar_Index")
                    .HasComment("Position (Slot) on the Shortcut Bar for this Object");

                entity.Property(e => e.ShortcutObjectId)
                    .HasColumnName("shortcut_Object_Id")
                    .HasComment("Guid of the object at this Slot");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesShortcutBar)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("wcid_shortcutbar");
            });

            modelBuilder.Entity<CharacterPropertiesSpellBar>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.SpellBarNumber, e.SpellId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("character_properties_spell_bar");

                entity.HasComment("SpellBar Properties of Weenies");

                entity.HasIndex(e => e.SpellBarIndex, "spellBar_idx");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.SpellBarNumber)
                    .HasColumnName("spell_Bar_Number")
                    .HasComment("Id of Spell Bar");

                entity.Property(e => e.SpellId)
                    .HasColumnName("spell_Id")
                    .HasComment("Id of Spell on this Spell Bar at this Slot");

                entity.Property(e => e.SpellBarIndex)
                    .HasColumnName("spell_Bar_Index")
                    .HasComment("Position (Slot) on this Spell Bar for this Spell");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesSpellBar)
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

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesSquelch)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("squelch_character_Id_constraint");
            });

            modelBuilder.Entity<CharacterPropertiesTitleBook>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.TitleId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("character_properties_title_book");

                entity.HasComment("TitleBook Properties of Weenies");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_Id")
                    .HasComment("Id of the character this property belongs to");

                entity.Property(e => e.TitleId)
                    .HasColumnName("title_Id")
                    .HasComment("Id of Title");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterPropertiesTitleBook)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("wcid_titlebook");
            });

            modelBuilder.Entity<ConfigPropertiesBoolean>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PRIMARY");

                entity.ToTable("config_properties_boolean");

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<ConfigPropertiesDouble>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PRIMARY");

                entity.ToTable("config_properties_double");

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<ConfigPropertiesLong>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PRIMARY");

                entity.ToTable("config_properties_long");

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<ConfigPropertiesString>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PRIMARY");

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
                    .HasColumnName("house_Id")
                    .HasComment("GUID of House Biota Object");

                entity.Property(e => e.PlayerGuid)
                    .HasColumnName("player_Guid")
                    .HasComment("GUID of Player Biota Object being granted permission to this house");

                entity.Property(e => e.Storage)
                    .HasColumnName("storage")
                    .HasComment("Permission includes access to House Storage");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.HousePermission)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("biota_Id_house_Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
