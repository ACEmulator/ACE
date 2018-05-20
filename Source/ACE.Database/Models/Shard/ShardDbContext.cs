using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.Shard
{
    public partial class ShardDbContext : DbContext
    {
        public virtual DbSet<Biota> Biota { get; set; }
        public virtual DbSet<BiotaPropertiesAnimPart> BiotaPropertiesAnimPart { get; set; }
        public virtual DbSet<BiotaPropertiesAttribute> BiotaPropertiesAttribute { get; set; }
        public virtual DbSet<BiotaPropertiesAttribute2nd> BiotaPropertiesAttribute2nd { get; set; }
        public virtual DbSet<BiotaPropertiesBodyPart> BiotaPropertiesBodyPart { get; set; }
        public virtual DbSet<BiotaPropertiesBook> BiotaPropertiesBook { get; set; }
        public virtual DbSet<BiotaPropertiesBookPageData> BiotaPropertiesBookPageData { get; set; }
        public virtual DbSet<BiotaPropertiesBool> BiotaPropertiesBool { get; set; }
        public virtual DbSet<BiotaPropertiesContract> BiotaPropertiesContract { get; set; }
        public virtual DbSet<BiotaPropertiesCreateList> BiotaPropertiesCreateList { get; set; }
        public virtual DbSet<BiotaPropertiesDID> BiotaPropertiesDID { get; set; }
        public virtual DbSet<BiotaPropertiesEmote> BiotaPropertiesEmote { get; set; }
        public virtual DbSet<BiotaPropertiesEmoteAction> BiotaPropertiesEmoteAction { get; set; }
        public virtual DbSet<BiotaPropertiesEnchantmentRegistry> BiotaPropertiesEnchantmentRegistry { get; set; }
        public virtual DbSet<BiotaPropertiesEventFilter> BiotaPropertiesEventFilter { get; set; }
        public virtual DbSet<BiotaPropertiesFillCompBook> BiotaPropertiesFillCompBook { get; set; }
        public virtual DbSet<BiotaPropertiesFloat> BiotaPropertiesFloat { get; set; }
        public virtual DbSet<BiotaPropertiesFriendList> BiotaPropertiesFriendList { get; set; }
        public virtual DbSet<BiotaPropertiesGenerator> BiotaPropertiesGenerator { get; set; }
        public virtual DbSet<BiotaPropertiesIID> BiotaPropertiesIID { get; set; }
        public virtual DbSet<BiotaPropertiesInt> BiotaPropertiesInt { get; set; }
        public virtual DbSet<BiotaPropertiesInt64> BiotaPropertiesInt64 { get; set; }
        public virtual DbSet<BiotaPropertiesPalette> BiotaPropertiesPalette { get; set; }
        public virtual DbSet<BiotaPropertiesPosition> BiotaPropertiesPosition { get; set; }
        public virtual DbSet<BiotaPropertiesQuestRegistry> BiotaPropertiesQuestRegistry { get; set; }
        public virtual DbSet<BiotaPropertiesShortcutBar> BiotaPropertiesShortcutBar { get; set; }
        public virtual DbSet<BiotaPropertiesSkill> BiotaPropertiesSkill { get; set; }
        public virtual DbSet<BiotaPropertiesSpellBar> BiotaPropertiesSpellBar { get; set; }
        public virtual DbSet<BiotaPropertiesSpellBook> BiotaPropertiesSpellBook { get; set; }
        public virtual DbSet<BiotaPropertiesString> BiotaPropertiesString { get; set; }
        public virtual DbSet<BiotaPropertiesTextureMap> BiotaPropertiesTextureMap { get; set; }
        public virtual DbSet<BiotaPropertiesTitleBook> BiotaPropertiesTitleBook { get; set; }
        public virtual DbSet<Character> Character { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = Common.ConfigManager.Config.MySql.Shard;

            optionsBuilder.UseMySql($"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Biota>(entity =>
            {
                entity.ToTable("biota");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.Property(e => e.WeenieType)
                    .HasColumnName("weenie_Type")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<BiotaPropertiesAnimPart>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Index });

                entity.ToTable("biota_properties_anim_part");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.AnimationId).HasColumnName("animation_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesAnimPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_animpart");
            });

            modelBuilder.Entity<BiotaPropertiesAttribute>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_attribute");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CPSpent)
                    .HasColumnName("c_P_Spent")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LevelFromCP)
                    .HasColumnName("level_From_C_P")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesAttribute)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute");
            });

            modelBuilder.Entity<BiotaPropertiesAttribute2nd>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_attribute_2nd");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CPSpent)
                    .HasColumnName("c_P_Spent")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CurrentLevel)
                    .HasColumnName("current_Level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LevelFromCP)
                    .HasColumnName("level_From_C_P")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesAttribute2nd)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute2nd");
            });

            modelBuilder.Entity<BiotaPropertiesBodyPart>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Key });

                entity.ToTable("biota_properties_body_part");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsAcid)
                    .HasColumnName("armor_Vs_Acid")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsBludgeon)
                    .HasColumnName("armor_Vs_Bludgeon")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsCold)
                    .HasColumnName("armor_Vs_Cold")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsElectric)
                    .HasColumnName("armor_Vs_Electric")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsFire)
                    .HasColumnName("armor_Vs_Fire")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsNether)
                    .HasColumnName("armor_Vs_Nether")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsPierce)
                    .HasColumnName("armor_Vs_Pierce")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ArmorVsSlash)
                    .HasColumnName("armor_Vs_Slash")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.BH)
                    .HasColumnName("b_h")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.BaseArmor)
                    .HasColumnName("base_Armor")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DType)
                    .HasColumnName("d_Type")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DVal)
                    .HasColumnName("d_Val")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DVar)
                    .HasColumnName("d_Var")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HLB)
                    .HasColumnName("h_l_b")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HLF)
                    .HasColumnName("h_l_f")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HRB)
                    .HasColumnName("h_r_b")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HRF)
                    .HasColumnName("h_r_f")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LLB)
                    .HasColumnName("l_l_b")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LLF)
                    .HasColumnName("l_l_f")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LRB)
                    .HasColumnName("l_r_b")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LRF)
                    .HasColumnName("l_r_f")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MLB)
                    .HasColumnName("m_l_b")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MLF)
                    .HasColumnName("m_l_f")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MRB)
                    .HasColumnName("m_r_b")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MRF)
                    .HasColumnName("m_r_f")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesBodyPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bodypart");
            });

            modelBuilder.Entity<BiotaPropertiesBook>(entity =>
            {
                entity.HasKey(e => e.ObjectId);

                entity.ToTable("biota_properties_book");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MaxNumCharsPerPage)
                    .HasColumnName("max_Num_Chars_Per_Page")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1000'");

                entity.Property(e => e.MaxNumPages)
                    .HasColumnName("max_Num_Pages")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Object)
                    .WithOne(p => p.BiotaPropertiesBook)
                    .HasForeignKey<BiotaPropertiesBook>(d => d.ObjectId)
                    .HasConstraintName("wcid_bookdata");
            });

            modelBuilder.Entity<BiotaPropertiesBookPageData>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.PageId });

                entity.ToTable("biota_properties_book_page_data");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageId)
                    .HasColumnName("page_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AuthorAccount)
                    .IsRequired()
                    .HasColumnName("author_Account")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'prewritten'");

                entity.Property(e => e.AuthorId)
                    .HasColumnName("author_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasColumnName("author_Name")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.IgnoreAuthor)
                    .HasColumnName("ignore_Author")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.PageText)
                    .IsRequired()
                    .HasColumnName("page_Text")
                    .HasColumnType("text");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesBookPageData)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_pagedata");
            });

            modelBuilder.Entity<BiotaPropertiesBool>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_bool");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesBool)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bool");
            });

            modelBuilder.Entity<BiotaPropertiesContract>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.ContractId });

                entity.ToTable("biota_properties_contract");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ContractId).HasColumnName("contract_Id");

                entity.Property(e => e.DeleteContract)
                    .HasColumnName("delete_Contract")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.SetAsDisplayContract)
                    .HasColumnName("set_As_Display_Contract")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Stage).HasColumnName("stage");

                entity.Property(e => e.TimeWhenDone).HasColumnName("time_When_Done");

                entity.Property(e => e.TimeWhenRepeats).HasColumnName("time_When_Repeats");

                entity.Property(e => e.Version).HasColumnName("version");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesContract)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_contract");
            });

            modelBuilder.Entity<BiotaPropertiesCreateList>(entity =>
            {
                entity.ToTable("biota_properties_create_list");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_createlist_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DestinationType)
                    .HasColumnName("destination_Type")
                    .HasColumnType("tinyint(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Palette)
                    .HasColumnName("palette")
                    .HasColumnType("tinyint(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Shade)
                    .HasColumnName("shade")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TryToBond)
                    .HasColumnName("try_To_Bond")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesCreateList)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_createlist");
            });

            modelBuilder.Entity<BiotaPropertiesDID>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_d_i_d");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesDID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_did");
            });

            modelBuilder.Entity<BiotaPropertiesEmote>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Category, e.EmoteSetId });

                entity.ToTable("biota_properties_emote");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EmoteSetId)
                    .HasColumnName("emote_Set_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MaxHealth).HasColumnName("max_Health");

                entity.Property(e => e.MinHealth).HasColumnName("min_Health");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Quest)
                    .HasColumnName("quest")
                    .HasColumnType("text");

                entity.Property(e => e.Style).HasColumnName("style");

                entity.Property(e => e.Substyle).HasColumnName("substyle");

                entity.Property(e => e.VendorType)
                    .HasColumnName("vendor_Type")
                    .HasColumnType("int(10)");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEmote)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emote");
            });

            modelBuilder.Entity<BiotaPropertiesEmoteAction>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.EmoteCategory, e.EmoteSetId, e.Order });

                entity.ToTable("biota_properties_emote_action");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EmoteCategory)
                    .HasColumnName("emote_Category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EmoteSetId)
                    .HasColumnName("emote_Set_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Amount64)
                    .HasColumnName("amount_64")
                    .HasColumnType("bigint(10)");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.Delay)
                    .HasColumnName("delay")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.DestinationType)
                    .HasColumnName("destination_Type")
                    .HasColumnType("tinyint(5)");

                entity.Property(e => e.Display)
                    .HasColumnName("display")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Extent)
                    .HasColumnName("extent")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.HeroXP64)
                    .HasColumnName("hero_X_P_64")
                    .HasColumnType("bigint(10)");

                entity.Property(e => e.Max)
                    .HasColumnName("max")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Max64)
                    .HasColumnName("max_64")
                    .HasColumnType("bigint(10)");

                entity.Property(e => e.MaxDbl).HasColumnName("max_Dbl");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.Min)
                    .HasColumnName("min")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Min64)
                    .HasColumnName("min_64")
                    .HasColumnType("bigint(10)");

                entity.Property(e => e.MinDbl).HasColumnName("min_Dbl");

                entity.Property(e => e.Motion)
                    .HasColumnName("motion")
                    .HasColumnType("int(10)");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.PScript)
                    .HasColumnName("p_Script")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Palette)
                    .HasColumnName("palette")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Percent).HasColumnName("percent");

                entity.Property(e => e.Shade).HasColumnName("shade");

                entity.Property(e => e.Sound)
                    .HasColumnName("sound")
                    .HasColumnType("int(10)");

                entity.Property(e => e.SpellId)
                    .HasColumnName("spell_Id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)");

                entity.Property(e => e.TestString)
                    .HasColumnName("test_String")
                    .HasColumnType("text");

                entity.Property(e => e.TreasureClass)
                    .HasColumnName("treasure_Class")
                    .HasColumnType("int(10)");

                entity.Property(e => e.TreasureType)
                    .HasColumnName("treasure_Type")
                    .HasColumnType("int(10)");

                entity.Property(e => e.TryToBond)
                    .HasColumnName("try_To_Bond")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.WealthRating)
                    .HasColumnName("wealth_Rating")
                    .HasColumnType("int(10)");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEmoteAction)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emoteaction");

                entity.HasOne(d => d.BiotaPropertiesEmote)
                    .WithMany(p => p.BiotaPropertiesEmoteAction)
                    .HasForeignKey(d => new { d.ObjectId, d.EmoteCategory, d.EmoteSetId })
                    .HasConstraintName("wcid_emoteset");
            });

            modelBuilder.Entity<BiotaPropertiesEnchantmentRegistry>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.EnchantmentCategory, e.SpellId, e.LayerId });

                entity.ToTable("biota_properties_enchantment_registry");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EnchantmentCategory)
                    .HasColumnName("enchantment_Category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellId)
                    .HasColumnName("spell_Id")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LayerId)
                    .HasColumnName("layer_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CasterObjectId)
                    .HasColumnName("caster_Object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DegradeLimit)
                    .HasColumnName("degrade_Limit")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DegradeModifier)
                    .HasColumnName("degrade_Modifier")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HasSpellSetId)
                    .HasColumnName("has_Spell_Set_Id")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.LastTimeDegraded)
                    .HasColumnName("last_Time_Degraded")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PowerLevel)
                    .HasColumnName("power_Level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellCategory)
                    .HasColumnName("spell_Category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellSetId)
                    .HasColumnName("spell_Set_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_Time")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StatModKey)
                    .HasColumnName("stat_Mod_Key")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StatModType)
                    .HasColumnName("stat_Mod_Type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StatModValue)
                    .HasColumnName("stat_Mod_Value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEnchantmentRegistry)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_enchantmentregistry");
            });

            modelBuilder.Entity<BiotaPropertiesEventFilter>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Event });

                entity.ToTable("biota_properties_event_filter");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesEventFilter)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_eventfilter");
            });

            modelBuilder.Entity<BiotaPropertiesFillCompBook>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.SpellComponentId });

                entity.ToTable("biota_properties_fill_comp_book");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellComponentId)
                    .HasColumnName("spell_Component_Id")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.QuantityToRebuy)
                    .HasColumnName("quantity_To_Rebuy")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesFillCompBook)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_fillcompbook");
            });

            modelBuilder.Entity<BiotaPropertiesFloat>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_float");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesFloat)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_float");
            });

            modelBuilder.Entity<BiotaPropertiesFriendList>(entity =>
            {
                entity.ToTable("biota_properties_friend_list");

                entity.HasIndex(e => e.AccountId)
                    .HasName("wcid_account_id_idx");

                entity.HasIndex(e => e.FriendId)
                    .HasName("wcid_friend_id_idx");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_friend_idx");

                entity.HasIndex(e => new { e.ObjectId, e.FriendId })
                    .HasName("wcid_friend_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FriendId)
                    .HasColumnName("friend_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.BiotaPropertiesFriendListFriend)
                    .HasForeignKey(d => d.FriendId)
                    .HasConstraintName("wcid_friend_id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesFriendListObject)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_friend");
            });

            modelBuilder.Entity<BiotaPropertiesGenerator>(entity =>
            {
                entity.ToTable("biota_properties_generator");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_generator_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.Delay)
                    .HasColumnName("delay")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InitCreate)
                    .HasColumnName("init_Create")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.MaxCreate)
                    .HasColumnName("max_Create")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.PaletteId).HasColumnName("palette_Id");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Shade).HasColumnName("shade");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasColumnType("int(10)");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.WhenCreate)
                    .HasColumnName("when_Create")
                    .HasDefaultValueSql("'2'");

                entity.Property(e => e.WhereCreate)
                    .HasColumnName("where_Create")
                    .HasDefaultValueSql("'4'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesGenerator)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_generator");
            });

            modelBuilder.Entity<BiotaPropertiesIID>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_i_i_d");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesIID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_iid");
            });

            modelBuilder.Entity<BiotaPropertiesInt>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_int");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesInt)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int");
            });

            modelBuilder.Entity<BiotaPropertiesInt64>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_int64");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("bigint(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesInt64)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int64");
            });

            modelBuilder.Entity<BiotaPropertiesPalette>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.SubPaletteId, e.Offset, e.Length });

                entity.ToTable("biota_properties_palette");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SubPaletteId).HasColumnName("sub_Palette_Id");

                entity.Property(e => e.Offset).HasColumnName("offset");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesPalette)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_palette");
            });

            modelBuilder.Entity<BiotaPropertiesPosition>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.PositionType });

                entity.ToTable("biota_properties_position");

                entity.HasIndex(e => e.ObjCellId)
                    .HasName("objCellId_idx");

                entity.Property(e => e.ObjectId).HasColumnName("object_Id");

                entity.Property(e => e.PositionType).HasColumnName("position_Type");

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

            modelBuilder.Entity<BiotaPropertiesQuestRegistry>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.QuestName });

                entity.ToTable("biota_properties_quest_registry");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.QuestName)
                    .HasColumnName("quest_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.LastTimeCompleted)
                    .HasColumnName("last_Time_Completed")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NumTimesCompleted)
                    .HasColumnName("num_Times_Completed")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesQuestRegistry)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_questbook");
            });

            modelBuilder.Entity<BiotaPropertiesShortcutBar>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.ShortcutBarIndex });

                entity.ToTable("biota_properties_shortcut_bar");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShortcutBarIndex)
                    .HasColumnName("shortcut_Bar_Index")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShortcutObjectId)
                    .HasColumnName("shortcut_Object_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesShortcutBar)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_shortcutbar");
            });

            modelBuilder.Entity<BiotaPropertiesSkill>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_skill");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AdjustPP)
                    .HasColumnName("adjust_P_P")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LastUsedTime)
                    .HasColumnName("last_Used_Time")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LevelFromPP)
                    .HasColumnName("level_From_P_P")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PP)
                    .HasColumnName("p_p")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ResistanceAtLastCheck)
                    .HasColumnName("resistance_At_Last_Check")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SAC)
                    .HasColumnName("s_a_c")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesSkill)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_skill");
            });

            modelBuilder.Entity<BiotaPropertiesSpellBar>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.SpellBarNumber, e.SpellBarIndex });

                entity.ToTable("biota_properties_spell_bar");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellBarNumber)
                    .HasColumnName("spell_Bar_Number")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellBarIndex)
                    .HasColumnName("spell_Bar_Index")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellId)
                    .HasColumnName("spell_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesSpellBar)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_spellbar");
            });

            modelBuilder.Entity<BiotaPropertiesSpellBook>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Spell });

                entity.ToTable("biota_properties_spell_book");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Spell)
                    .HasColumnName("spell")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("'2'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesSpellBook)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_spellbook");
            });

            modelBuilder.Entity<BiotaPropertiesString>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Type });

                entity.ToTable("biota_properties_string");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("text");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesString)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_string");
            });

            modelBuilder.Entity<BiotaPropertiesTextureMap>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.Index, e.OldId });

                entity.ToTable("biota_properties_texture_map");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.OldId).HasColumnName("old_Id");

                entity.Property(e => e.NewId).HasColumnName("new_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesTextureMap)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_texturemap");
            });

            modelBuilder.Entity<BiotaPropertiesTitleBook>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.TitleId });

                entity.ToTable("biota_properties_title_book");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TitleId)
                    .HasColumnName("title_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.BiotaPropertiesTitleBook)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_titlebook");
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.BiotaId });

                entity.ToTable("character");

                entity.HasIndex(e => e.AccountId)
                    .HasName("character_account_idx");

                entity.HasIndex(e => e.BiotaId)
                    .HasName("biota_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("character_name_idx");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.BiotaId)
                    .HasColumnName("biota_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("delete_Time")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_Deleted")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.LastLoginTimestamp)
                    .HasColumnName("last_Login_Timestamp")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Biota)
                    .WithOne(p => p.Character)
                    .HasForeignKey<Character>(d => d.BiotaId)
                    .HasConstraintName("biota_character");
            });
        }
    }
}
