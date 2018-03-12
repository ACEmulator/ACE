using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.World
{
    public partial class WorldDbContext : DbContext
    {
        public virtual DbSet<AceRecipe> AceRecipe { get; set; }
        public virtual DbSet<LandblockInstances> LandblockInstances { get; set; }
        public virtual DbSet<PointsOfInterest> PointsOfInterest { get; set; }
        public virtual DbSet<Weenie> Weenie { get; set; }
        public virtual DbSet<WeeniePropertiesAnimPart> WeeniePropertiesAnimPart { get; set; }
        public virtual DbSet<WeeniePropertiesAttribute> WeeniePropertiesAttribute { get; set; }
        public virtual DbSet<WeeniePropertiesAttribute2nd> WeeniePropertiesAttribute2nd { get; set; }
        public virtual DbSet<WeeniePropertiesBodyPart> WeeniePropertiesBodyPart { get; set; }
        public virtual DbSet<WeeniePropertiesBook> WeeniePropertiesBook { get; set; }
        public virtual DbSet<WeeniePropertiesBookPageData> WeeniePropertiesBookPageData { get; set; }
        public virtual DbSet<WeeniePropertiesBool> WeeniePropertiesBool { get; set; }
        public virtual DbSet<WeeniePropertiesCreateList> WeeniePropertiesCreateList { get; set; }
        public virtual DbSet<WeeniePropertiesDID> WeeniePropertiesDID { get; set; }
        public virtual DbSet<WeeniePropertiesEmote> WeeniePropertiesEmote { get; set; }
        public virtual DbSet<WeeniePropertiesEmoteAction> WeeniePropertiesEmoteAction { get; set; }
        public virtual DbSet<WeeniePropertiesEventFilter> WeeniePropertiesEventFilter { get; set; }
        public virtual DbSet<WeeniePropertiesFloat> WeeniePropertiesFloat { get; set; }
        public virtual DbSet<WeeniePropertiesGenerator> WeeniePropertiesGenerator { get; set; }
        public virtual DbSet<WeeniePropertiesIID> WeeniePropertiesIID { get; set; }
        public virtual DbSet<WeeniePropertiesInt> WeeniePropertiesInt { get; set; }
        public virtual DbSet<WeeniePropertiesInt64> WeeniePropertiesInt64 { get; set; }
        public virtual DbSet<WeeniePropertiesPalette> WeeniePropertiesPalette { get; set; }
        public virtual DbSet<WeeniePropertiesPosition> WeeniePropertiesPosition { get; set; }
        public virtual DbSet<WeeniePropertiesSkill> WeeniePropertiesSkill { get; set; }
        public virtual DbSet<WeeniePropertiesSpellBook> WeeniePropertiesSpellBook { get; set; }
        public virtual DbSet<WeeniePropertiesString> WeeniePropertiesString { get; set; }
        public virtual DbSet<WeeniePropertiesTextureMap> WeeniePropertiesTextureMap { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = Common.ConfigManager.Config.MySql.World;

            optionsBuilder.UseMySql($"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AceRecipe>(entity =>
            {
                entity.HasKey(e => e.RecipeGuid);

                entity.ToTable("ace_recipe");

                entity.Property(e => e.RecipeGuid)
                    .HasColumnName("recipeGuid")
                    .HasColumnType("binary(16)");

                entity.Property(e => e.AlternateMessage)
                    .HasColumnName("alternateMessage")
                    .HasColumnType("text");

                entity.Property(e => e.FailMessage)
                    .HasColumnName("failMessage")
                    .HasColumnType("text");

                entity.Property(e => e.FailureItem1Wcid).HasColumnName("failureItem1Wcid");

                entity.Property(e => e.FailureItem2Wcid).HasColumnName("failureItem2Wcid");

                entity.Property(e => e.HealingAttribute).HasColumnName("healingAttribute");

                entity.Property(e => e.PartialFailDifficulty).HasColumnName("partialFailDifficulty");

                entity.Property(e => e.RecipeType).HasColumnName("recipeType");

                entity.Property(e => e.ResultFlags).HasColumnName("resultFlags");

                entity.Property(e => e.SkillDifficulty).HasColumnName("skillDifficulty");

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.Property(e => e.SourceWcid).HasColumnName("sourceWcid");

                entity.Property(e => e.SuccessItem1Wcid).HasColumnName("successItem1Wcid");

                entity.Property(e => e.SuccessItem2Wcid).HasColumnName("successItem2Wcid");

                entity.Property(e => e.SuccessMessage)
                    .HasColumnName("successMessage")
                    .HasColumnType("text");

                entity.Property(e => e.TargetWcid).HasColumnName("targetWcid");

                entity.Property(e => e.UserModified)
                    .HasColumnName("userModified")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<LandblockInstances>(entity =>
            {
                entity.ToTable("landblock_instances");

                entity.HasIndex(e => e.Guid)
                    .HasName("guid_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Landblock)
                    .HasName("instance_landblock_idx");

                entity.HasIndex(e => e.WeenieClassId)
                    .HasName("wcid_instance_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Landblock)
                    .HasColumnName("landblock")
                    .HasColumnType("int(5)");

                entity.Property(e => e.LinkController)
                    .HasColumnName("link_Controller")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.LinkSlot)
                    .HasColumnName("link_Slot")
                    .HasColumnType("int(5)");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.HasOne(d => d.WeenieClass)
                    .WithMany(p => p.LandblockInstances)
                    .HasForeignKey(d => d.WeenieClassId)
                    .HasConstraintName("wcid_instance");
            });

            modelBuilder.Entity<PointsOfInterest>(entity =>
            {
                entity.ToTable("points_of_interest");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.WeenieClassId)
                    .HasName("wcid_poi_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.HasOne(d => d.WeenieClass)
                    .WithMany(p => p.PointsOfInterest)
                    .HasForeignKey(d => d.WeenieClassId)
                    .HasConstraintName("wcid_poi");
            });

            modelBuilder.Entity<Weenie>(entity =>
            {
                entity.HasKey(e => e.ClassId);

                entity.ToTable("weenie");

                entity.HasIndex(e => e.ClassName)
                    .HasName("className_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClassId).HasColumnName("class_Id");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasColumnName("class_Name")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<WeeniePropertiesAnimPart>(entity =>
            {
                entity.ToTable("weenie_properties_anim_part");

                entity.HasIndex(e => new { e.ObjectId, e.Index })
                    .HasName("object_Id_index_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnimationId).HasColumnName("animation_Id");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesAnimPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_animpart");
            });

            modelBuilder.Entity<WeeniePropertiesAttribute>(entity =>
            {
                entity.ToTable("weenie_properties_attribute");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_attribute_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_attribute_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CPSpent)
                    .HasColumnName("c_P_Spent")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LevelFromCP)
                    .HasColumnName("level_From_C_P")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesAttribute)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute");
            });

            modelBuilder.Entity<WeeniePropertiesAttribute2nd>(entity =>
            {
                entity.ToTable("weenie_properties_attribute_2nd");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_attribute2nd_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_attribute2nd_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesAttribute2nd)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute2nd");
            });

            modelBuilder.Entity<WeeniePropertiesBodyPart>(entity =>
            {
                entity.ToTable("weenie_properties_body_part");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_bodypart_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Key })
                    .HasName("wcid_bodypart_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.Key)
                    .HasColumnName("key")
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

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesBodyPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bodypart");
            });

            modelBuilder.Entity<WeeniePropertiesBook>(entity =>
            {
                entity.ToTable("weenie_properties_book");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_bookdata_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MaxNumCharsPerPage)
                    .HasColumnName("max_Num_Chars_Per_Page")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1000'");

                entity.Property(e => e.MaxNumPages)
                    .HasColumnName("max_Num_Pages")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithOne(p => p.WeeniePropertiesBook)
                    .HasForeignKey<WeeniePropertiesBook>(d => d.ObjectId)
                    .HasConstraintName("wcid_bookdata");
            });

            modelBuilder.Entity<WeeniePropertiesBookPageData>(entity =>
            {
                entity.ToTable("weenie_properties_book_page_data");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_pagedata_idx");

                entity.HasIndex(e => new { e.ObjectId, e.PageId })
                    .HasName("wcid_pageid_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageId)
                    .HasColumnName("page_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageText)
                    .IsRequired()
                    .HasColumnName("page_Text")
                    .HasColumnType("text");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesBookPageData)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_pagedata");
            });

            modelBuilder.Entity<WeeniePropertiesBool>(entity =>
            {
                entity.ToTable("weenie_properties_bool");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_bool_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_bool_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesBool)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bool");
            });

            modelBuilder.Entity<WeeniePropertiesCreateList>(entity =>
            {
                entity.ToTable("weenie_properties_create_list");

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
                    .WithMany(p => p.WeeniePropertiesCreateList)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_createlist");
            });

            modelBuilder.Entity<WeeniePropertiesDID>(entity =>
            {
                entity.ToTable("weenie_properties_d_i_d");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_did_idx");

                entity.HasIndex(e => e.Type)
                    .HasName("wcid_did_type_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_did_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesDID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_did");
            });

            modelBuilder.Entity<WeeniePropertiesEmote>(entity =>
            {
                entity.ToTable("weenie_properties_emote");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_emote_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MaxHealth).HasColumnName("max_Health");

                entity.Property(e => e.MinHealth).HasColumnName("min_Health");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

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
                    .WithMany(p => p.WeeniePropertiesEmote)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emote");
            });

            modelBuilder.Entity<WeeniePropertiesEmoteAction>(entity =>
            {
                entity.ToTable("weenie_properties_emote_action");

                entity.HasIndex(e => e.EmoteId)
                    .HasName("emoteset_emoteaction_idx");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_emoteaction_idx");

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.EmoteId)
                    .HasColumnName("emote_Id")
                    .HasDefaultValueSql("'0'");

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

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

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

                entity.HasOne(d => d.Emote)
                    .WithMany(p => p.WeeniePropertiesEmoteAction)
                    .HasForeignKey(d => d.EmoteId)
                    .HasConstraintName("emote_emoteaction");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesEmoteAction)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emoteaction");
            });

            modelBuilder.Entity<WeeniePropertiesEventFilter>(entity =>
            {
                entity.ToTable("weenie_properties_event_filter");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_eventfilter_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Event })
                    .HasName("wcid_eventfilter_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesEventFilter)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_eventfilter");
            });

            modelBuilder.Entity<WeeniePropertiesFloat>(entity =>
            {
                entity.ToTable("weenie_properties_float");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_float_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_float_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesFloat)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_float");
            });

            modelBuilder.Entity<WeeniePropertiesGenerator>(entity =>
            {
                entity.ToTable("weenie_properties_generator");

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
                    .WithMany(p => p.WeeniePropertiesGenerator)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_generator");
            });

            modelBuilder.Entity<WeeniePropertiesIID>(entity =>
            {
                entity.ToTable("weenie_properties_i_i_d");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_iid_idx");

                entity.HasIndex(e => e.Type)
                    .HasName("wcid_did_type_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_iid_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesIID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_iid");
            });

            modelBuilder.Entity<WeeniePropertiesInt>(entity =>
            {
                entity.ToTable("weenie_properties_int");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_int_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_int_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesInt)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int");
            });

            modelBuilder.Entity<WeeniePropertiesInt64>(entity =>
            {
                entity.ToTable("weenie_properties_int64");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_int64_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_int64_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesInt64)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int64");
            });

            modelBuilder.Entity<WeeniePropertiesPalette>(entity =>
            {
                entity.ToTable("weenie_properties_palette");

                entity.HasIndex(e => new { e.ObjectId, e.SubPaletteId, e.Offset, e.Length })
                    .HasName("object_Id_subPaletteId_offset_length_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Offset).HasColumnName("offset");

                entity.Property(e => e.SubPaletteId).HasColumnName("sub_Palette_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesPalette)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_palette");
            });

            modelBuilder.Entity<WeeniePropertiesPosition>(entity =>
            {
                entity.ToTable("weenie_properties_position");

                entity.HasIndex(e => e.ObjCellId)
                    .HasName("objCellId_idx");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_position_idx");

                entity.HasIndex(e => new { e.ObjectId, e.PositionType })
                    .HasName("wcid_position_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.ObjectId).HasColumnName("object_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.PositionType).HasColumnName("position_Type");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesPosition)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_position");
            });

            modelBuilder.Entity<WeeniePropertiesSkill>(entity =>
            {
                entity.ToTable("weenie_properties_skill");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_skill_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_skill_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
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

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesSkill)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_skill");
            });

            modelBuilder.Entity<WeeniePropertiesSpellBook>(entity =>
            {
                entity.ToTable("weenie_properties_spell_book");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_spellbook_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Spell })
                    .HasName("wcid_spellbook_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("'2'");

                entity.Property(e => e.Spell)
                    .HasColumnName("spell")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesSpellBook)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_spellbook");
            });

            modelBuilder.Entity<WeeniePropertiesString>(entity =>
            {
                entity.ToTable("weenie_properties_string");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_string_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Type })
                    .HasName("wcid_string_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .WithMany(p => p.WeeniePropertiesString)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_string");
            });

            modelBuilder.Entity<WeeniePropertiesTextureMap>(entity =>
            {
                entity.ToTable("weenie_properties_texture_map");

                entity.HasIndex(e => new { e.ObjectId, e.Index, e.OldId })
                    .HasName("object_Id_index_oldId_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.NewId).HasColumnName("new_Id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OldId).HasColumnName("old_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesTextureMap)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_texturemap");
            });
        }
    }
}
