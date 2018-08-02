using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.World
{
    public partial class WorldDbContext : DbContext
    {
        public WorldDbContext()
        {
        }

        public WorldDbContext(DbContextOptions<WorldDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CookBook> CookBook { get; set; }
        public virtual DbSet<Encounter> Encounter { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<HousePortal> HousePortal { get; set; }
        public virtual DbSet<LandblockInstances> LandblockInstances { get; set; }
        public virtual DbSet<PointsOfInterest> PointsOfInterest { get; set; }
        public virtual DbSet<Quest> Quest { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeComponent> RecipeComponent { get; set; }
        public virtual DbSet<RecipeMod> RecipeMod { get; set; }
        public virtual DbSet<RecipeModsBool> RecipeModsBool { get; set; }
        public virtual DbSet<RecipeModsDID> RecipeModsDID { get; set; }
        public virtual DbSet<RecipeModsFloat> RecipeModsFloat { get; set; }
        public virtual DbSet<RecipeModsIID> RecipeModsIID { get; set; }
        public virtual DbSet<RecipeModsInt> RecipeModsInt { get; set; }
        public virtual DbSet<RecipeModsString> RecipeModsString { get; set; }
        public virtual DbSet<RecipeRequirementsBool> RecipeRequirementsBool { get; set; }
        public virtual DbSet<RecipeRequirementsDID> RecipeRequirementsDID { get; set; }
        public virtual DbSet<RecipeRequirementsFloat> RecipeRequirementsFloat { get; set; }
        public virtual DbSet<RecipeRequirementsIID> RecipeRequirementsIID { get; set; }
        public virtual DbSet<RecipeRequirementsInt> RecipeRequirementsInt { get; set; }
        public virtual DbSet<RecipeRequirementsString> RecipeRequirementsString { get; set; }
        public virtual DbSet<Spell> Spell { get; set; }
        public virtual DbSet<TreasureDeath> TreasureDeath { get; set; }
        public virtual DbSet<TreasureWielded> TreasureWielded { get; set; }
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
            modelBuilder.Entity<CookBook>(entity =>
            {
                entity.ToTable("cook_book");

                entity.HasIndex(e => e.SourceWCID)
                    .HasName("source_idx");

                entity.HasIndex(e => e.TargetWCID)
                    .HasName("target_idx");

                entity.HasIndex(e => new { e.RecipeId, e.TargetWCID, e.SourceWCID })
                    .HasName("recipe_target_source_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SourceWCID)
                    .HasColumnName("source_W_C_I_D")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TargetWCID)
                    .HasColumnName("target_W_C_I_D")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.CookBook)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("cookbook_recipe");
            });

            modelBuilder.Entity<Encounter>(entity =>
            {
                entity.ToTable("encounter");

                entity.HasIndex(e => e.Landblock)
                    .HasName("landblock_idx");

                entity.HasIndex(e => new { e.Landblock, e.CellX, e.CellY })
                    .HasName("landblock_cellx_celly_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CellX)
                    .HasColumnName("cell_X")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CellY)
                    .HasColumnName("cell_Y")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Landblock)
                    .HasColumnName("landblock")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_Time")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_Time")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<HousePortal>(entity =>
            {
                entity.ToTable("house_portal");

                entity.HasIndex(e => new { e.HouseId, e.ObjCellId })
                    .HasName("house_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.HouseId).HasColumnName("house_Id");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
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

            modelBuilder.Entity<Quest>(entity =>
            {
                entity.ToTable("quest");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MaxSolves)
                    .HasColumnName("max_Solves")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.MinDelta)
                    .HasColumnName("min_Delta")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("recipe");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Difficulty)
                    .HasColumnName("difficulty")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FailAmount)
                    .HasColumnName("fail_Amount")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FailMessage)
                    .HasColumnName("fail_Message")
                    .HasColumnType("text");

                entity.Property(e => e.FailWCID)
                    .HasColumnName("fail_W_C_I_D")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SalvageType)
                    .HasColumnName("salvage_Type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Skill)
                    .HasColumnName("skill")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SuccessAmount)
                    .HasColumnName("success_Amount")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SuccessMessage)
                    .HasColumnName("success_Message")
                    .HasColumnType("text");

                entity.Property(e => e.SuccessWCID)
                    .HasColumnName("success_W_C_I_D")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<RecipeComponent>(entity =>
            {
                entity.ToTable("recipe_component");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DestroyAmount)
                    .HasColumnName("destroy_Amount")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DestroyChance)
                    .HasColumnName("destroy_Chance")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DestroyMessage)
                    .HasColumnName("destroy_Message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeComponent)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_component");
            });

            modelBuilder.Entity<RecipeMod>(entity =>
            {
                entity.ToTable("recipe_mod");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_Id")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Health)
                    .HasColumnName("health")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InstanceId)
                    .HasColumnName("instance_Id")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Mana)
                    .HasColumnName("mana")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown2)
                    .HasColumnName("unknown_2")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown4)
                    .HasColumnName("unknown_4")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown5)
                    .HasColumnName("unknown_5")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown6)
                    .HasColumnName("unknown_6")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown7)
                    .HasColumnName("unknown_7")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Unknown9)
                    .HasColumnName("unknown_9")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeMod)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_Mod");
            });

            modelBuilder.Entity<RecipeModsBool>(entity =>
            {
                entity.ToTable("recipe_mods_bool");

                entity.HasIndex(e => e.RecipeModId)
                    .HasName("recipe_mod_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsBool)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_bool");
            });

            modelBuilder.Entity<RecipeModsDID>(entity =>
            {
                entity.ToTable("recipe_mods_d_i_d");

                entity.HasIndex(e => e.RecipeModId)
                    .HasName("recipe_mod_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsDID)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_did");
            });

            modelBuilder.Entity<RecipeModsFloat>(entity =>
            {
                entity.ToTable("recipe_mods_float");

                entity.HasIndex(e => e.RecipeModId)
                    .HasName("recipe_mod_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsFloat)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_float");
            });

            modelBuilder.Entity<RecipeModsIID>(entity =>
            {
                entity.ToTable("recipe_mods_i_i_d");

                entity.HasIndex(e => e.RecipeModId)
                    .HasName("recipe_mod_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsIID)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_iid");
            });

            modelBuilder.Entity<RecipeModsInt>(entity =>
            {
                entity.ToTable("recipe_mods_int");

                entity.HasIndex(e => e.RecipeModId)
                    .HasName("recipe_mod_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsInt)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_int");
            });

            modelBuilder.Entity<RecipeModsString>(entity =>
            {
                entity.ToTable("recipe_mods_string");

                entity.HasIndex(e => e.RecipeModId)
                    .HasName("recipe_mod_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("text");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsString)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_string");
            });

            modelBuilder.Entity<RecipeRequirementsBool>(entity =>
            {
                entity.ToTable("recipe_requirements_bool");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsBool)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_bool");
            });

            modelBuilder.Entity<RecipeRequirementsDID>(entity =>
            {
                entity.ToTable("recipe_requirements_d_i_d");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsDID)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_did");
            });

            modelBuilder.Entity<RecipeRequirementsFloat>(entity =>
            {
                entity.ToTable("recipe_requirements_float");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsFloat)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_float");
            });

            modelBuilder.Entity<RecipeRequirementsIID>(entity =>
            {
                entity.ToTable("recipe_requirements_i_i_d");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsIID)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_iid");
            });

            modelBuilder.Entity<RecipeRequirementsInt>(entity =>
            {
                entity.ToTable("recipe_requirements_int");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsInt)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_int");
            });

            modelBuilder.Entity<RecipeRequirementsString>(entity =>
            {
                entity.ToTable("recipe_requirements_string");

                entity.HasIndex(e => e.RecipeId)
                    .HasName("recipe_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enum)
                    .HasColumnName("enum")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Stat)
                    .HasColumnName("stat")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("text");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsString)
                    .HasPrincipalKey(p => p.RecipeId)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_string");
            });

            modelBuilder.Entity<Spell>(entity =>
            {
                entity.ToTable("spell");

                entity.HasIndex(e => e.MetaSpellId)
                    .HasName("metaspell_id_uidx")
                    .IsUnique();

                entity.HasIndex(e => e.SpellId)
                    .HasName("spell_id_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Align)
                    .HasColumnName("align")
                    .HasColumnType("int(10)");

                entity.Property(e => e.BaseIntensity)
                    .HasColumnName("base_Intensity")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Bitfield)
                    .HasColumnName("bitfield")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Boost)
                    .HasColumnName("boost")
                    .HasColumnType("int(10)");

                entity.Property(e => e.BoostVariance)
                    .HasColumnName("boost_Variance")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CasterEffect)
                    .HasColumnName("caster_Effect")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ComponentLoss).HasColumnName("component_Loss");

                entity.Property(e => e.CreateOffsetOriginX).HasColumnName("create_Offset_Origin_X");

                entity.Property(e => e.CreateOffsetOriginY).HasColumnName("create_Offset_Origin_Y");

                entity.Property(e => e.CreateOffsetOriginZ).HasColumnName("create_Offset_Origin_Z");

                entity.Property(e => e.CritFreq).HasColumnName("crit_Freq");

                entity.Property(e => e.CritMultiplier).HasColumnName("crit_Multiplier");

                entity.Property(e => e.DamageRatio).HasColumnName("damage_Ratio");

                entity.Property(e => e.DamageType)
                    .HasColumnName("damage_Type")
                    .HasColumnType("int(10)");

                entity.Property(e => e.DefaultLaunchAngle).HasColumnName("default_Launch_Angle");

                entity.Property(e => e.DegradeLimit).HasColumnName("degrade_Limit");

                entity.Property(e => e.DegradeModifier).HasColumnName("degrade_Modifier");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Destination)
                    .HasColumnName("destination")
                    .HasColumnType("int(10)");

                entity.Property(e => e.DimsOriginX).HasColumnName("dims_Origin_X");

                entity.Property(e => e.DimsOriginY).HasColumnName("dims_Origin_Y");

                entity.Property(e => e.DimsOriginZ).HasColumnName("dims_Origin_Z");

                entity.Property(e => e.DispelSchool)
                    .HasColumnName("dispel_School")
                    .HasColumnType("int(10)");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_Order")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DrainPercentage).HasColumnName("drain_Percentage");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.EType).HasColumnName("e_Type");

                entity.Property(e => e.EconomyMod).HasColumnName("economy_Mod");

                entity.Property(e => e.ElementalModifier).HasColumnName("elemental_Modifier");

                entity.Property(e => e.FizzleEffect)
                    .HasColumnName("fizzle_Effect")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FormulaVersion)
                    .HasColumnName("formula_Version")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IconId)
                    .HasColumnName("icon_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IgnoreMagicResist)
                    .HasColumnName("ignore_Magic_Resist")
                    .HasColumnType("int(10)");

                entity.Property(e => e.ImbuedEffect).HasColumnName("imbued_Effect");

                entity.Property(e => e.Index)
                    .HasColumnName("index")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasColumnType("int(10)");

                entity.Property(e => e.LossPercent).HasColumnName("loss_Percent");

                entity.Property(e => e.Mana)
                    .HasColumnName("mana")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ManaMod)
                    .HasColumnName("mana_Mod")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MaxBoostAllowed)
                    .HasColumnName("max_Boost_Allowed")
                    .HasColumnType("int(10)");

                entity.Property(e => e.MaxPower)
                    .HasColumnName("max_Power")
                    .HasColumnType("int(10)");

                entity.Property(e => e.MetaSpellId)
                    .HasColumnName("meta_Spell_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MetaSpellType)
                    .HasColumnName("meta_Spell_Type")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MinPower)
                    .HasColumnName("min_Power")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.NonComponentTargetType)
                    .HasColumnName("non_Component_Target_Type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NonTracking)
                    .HasColumnName("non_Tracking")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.NumProjectiles)
                    .HasColumnName("num_Projectiles")
                    .HasColumnType("int(10)");

                entity.Property(e => e.NumProjectilesVariance)
                    .HasColumnName("num_Projectiles_Variance")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("int(10)");

                entity.Property(e => e.NumberVariance).HasColumnName("number_Variance");

                entity.Property(e => e.PaddingOriginX).HasColumnName("padding_Origin_X");

                entity.Property(e => e.PaddingOriginY).HasColumnName("padding_Origin_Y");

                entity.Property(e => e.PaddingOriginZ).HasColumnName("padding_Origin_Z");

                entity.Property(e => e.PeturbationOriginX).HasColumnName("peturbation_Origin_X");

                entity.Property(e => e.PeturbationOriginY).HasColumnName("peturbation_Origin_Y");

                entity.Property(e => e.PeturbationOriginZ).HasColumnName("peturbation_Origin_Z");

                entity.Property(e => e.PortalLifetime).HasColumnName("portal_Lifetime");

                entity.Property(e => e.PositionAnglesW).HasColumnName("position_Angles_W");

                entity.Property(e => e.PositionAnglesX).HasColumnName("position_Angles_X");

                entity.Property(e => e.PositionAnglesY).HasColumnName("position_Angles_Y");

                entity.Property(e => e.PositionAnglesZ).HasColumnName("position_Angles_Z");

                entity.Property(e => e.PositionObjCellId).HasColumnName("position_Obj_Cell_ID");

                entity.Property(e => e.PositionOriginX).HasColumnName("position_Origin_X");

                entity.Property(e => e.PositionOriginY).HasColumnName("position_Origin_Y");

                entity.Property(e => e.PositionOriginZ).HasColumnName("position_Origin_Z");

                entity.Property(e => e.Power)
                    .HasColumnName("power")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PowerVariance).HasColumnName("power_Variance");

                entity.Property(e => e.Proportion).HasColumnName("proportion");

                entity.Property(e => e.RangeConstant).HasColumnName("range_Constant");

                entity.Property(e => e.RangeMod).HasColumnName("range_Mod");

                entity.Property(e => e.RecoveryAmount).HasColumnName("recovery_Amount");

                entity.Property(e => e.RecoveryInterval).HasColumnName("recovery_Interval");

                entity.Property(e => e.School)
                    .HasColumnName("school")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SlayerCreatureType)
                    .HasColumnName("slayer_Creature_Type")
                    .HasColumnType("int(10)");

                entity.Property(e => e.SlayerDamageBonus).HasColumnName("slayer_Damage_Bonus");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasColumnType("int(10)");

                entity.Property(e => e.SourceLoss)
                    .HasColumnName("source_Loss")
                    .HasColumnType("int(10)");

                entity.Property(e => e.SpellFormulaComp1ComponentId)
                    .HasColumnName("spell_Formula_Comp_1_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp2ComponentId)
                    .HasColumnName("spell_Formula_Comp_2_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp3ComponentId)
                    .HasColumnName("spell_Formula_Comp_3_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp4ComponentId)
                    .HasColumnName("spell_Formula_Comp_4_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp5ComponentId)
                    .HasColumnName("spell_Formula_Comp_5_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp6ComponentId)
                    .HasColumnName("spell_Formula_Comp_6_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp7ComponentId)
                    .HasColumnName("spell_Formula_Comp_7_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellFormulaComp8ComponentId)
                    .HasColumnName("spell_Formula_Comp_8_Component_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpellId).HasColumnName("spell_Id");

                entity.Property(e => e.SpreadAngle).HasColumnName("spread_Angle");

                entity.Property(e => e.StatModKey).HasColumnName("stat_Mod_Key");

                entity.Property(e => e.StatModType).HasColumnName("stat_Mod_Type");

                entity.Property(e => e.StatModVal).HasColumnName("stat_Mod_Val");

                entity.Property(e => e.TargetEffect)
                    .HasColumnName("target_Effect")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TransferBitfield).HasColumnName("transfer_Bitfield");

                entity.Property(e => e.TransferCap)
                    .HasColumnName("transfer_Cap")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Variance)
                    .HasColumnName("variance")
                    .HasColumnType("int(10)");

                entity.Property(e => e.VerticalAngle).HasColumnName("vertical_Angle");

                entity.Property(e => e.Wcid).HasColumnName("wcid");
            });

            modelBuilder.Entity<TreasureDeath>(entity =>
            {
                entity.ToTable("treasure_death");

                entity.HasIndex(e => e.TreasureType)
                    .HasName("treasureType_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Tier)
                    .HasColumnName("tier")
                    .HasColumnType("int(10)");

                entity.Property(e => e.TreasureType).HasColumnName("treasure_Type");

                entity.Property(e => e.Unknown1).HasColumnName("unknown_1");

                entity.Property(e => e.Unknown10)
                    .HasColumnName("unknown_10")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown11)
                    .HasColumnName("unknown_11")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown12)
                    .HasColumnName("unknown_12")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown13)
                    .HasColumnName("unknown_13")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown14)
                    .HasColumnName("unknown_14")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown2)
                    .HasColumnName("unknown_2")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown3)
                    .HasColumnName("unknown_3")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown4)
                    .HasColumnName("unknown_4")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown5)
                    .HasColumnName("unknown_5")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown6)
                    .HasColumnName("unknown_6")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown7)
                    .HasColumnName("unknown_7")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown8)
                    .HasColumnName("unknown_8")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Unknown9)
                    .HasColumnName("unknown_9")
                    .HasColumnType("int(10)");
            });

            modelBuilder.Entity<TreasureWielded>(entity =>
            {
                entity.ToTable("treasure_wielded");

                entity.HasIndex(e => e.TreasureType)
                    .HasName("treasureType_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PaletteId).HasColumnName("palette_Id");

                entity.Property(e => e.Probability).HasColumnName("probability");

                entity.Property(e => e.Shade).HasColumnName("shade");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TreasureType).HasColumnName("treasure_Type");

                entity.Property(e => e.Unknown1).HasColumnName("unknown_1");

                entity.Property(e => e.Unknown10).HasColumnName("unknown_10");

                entity.Property(e => e.Unknown11).HasColumnName("unknown_11");

                entity.Property(e => e.Unknown12).HasColumnName("unknown_12");

                entity.Property(e => e.Unknown2).HasColumnName("unknown_2");

                entity.Property(e => e.Unknown3).HasColumnName("unknown_3");

                entity.Property(e => e.Unknown4).HasColumnName("unknown_4");

                entity.Property(e => e.Unknown5).HasColumnName("unknown_5");

                entity.Property(e => e.Unknown6)
                    .HasColumnName("unknown_6")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Unknown7)
                    .HasColumnName("unknown_7")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Unknown8)
                    .HasColumnName("unknown_8")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Unknown9).HasColumnName("unknown_9");

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");
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
                    .HasColumnType("varchar(100)");

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
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'prewritten'");

                entity.Property(e => e.AuthorId)
                    .HasColumnName("author_Id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasColumnName("author_Name")
                    .HasColumnType("varchar(255)")
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

                entity.HasIndex(e => e.Category)
                    .HasName("category_idx");

                entity.HasIndex(e => e.EmoteSetId)
                    .HasName("emoteset_idx");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_emote_idx");

                entity.HasIndex(e => new { e.ObjectId, e.Category, e.EmoteSetId })
                    .HasName("wcid_category_emoteset_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EmoteSetId)
                    .HasColumnName("emote_Set_Id")
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

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesEmote)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emote");
            });

            modelBuilder.Entity<WeeniePropertiesEmoteAction>(entity =>
            {
                entity.ToTable("weenie_properties_emote_action");

                entity.HasIndex(e => e.EmoteCategory)
                    .HasName("emotecategory_idx");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("wcid_emoteaction_idx");

                entity.HasIndex(e => e.Order)
                    .HasName("emoteorder_idx");

                entity.HasIndex(e => e.Type)
                    .HasName("emotetype_idx");

                entity.HasIndex(e => new { e.ObjectId, e.EmoteCategory, e.EmoteSetId, e.Order })
                    .HasName("wcid_category_set_order_uidx")
                    .IsUnique();

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

                entity.Property(e => e.EmoteCategory)
                    .HasColumnName("emote_Category")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EmoteSetId)
                    .HasColumnName("emote_Set_Id")
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

                entity.Property(e => e.Order)
                    .HasColumnName("order")
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

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesEmoteAction)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emoteaction");

                entity.HasOne(d => d.WeeniePropertiesEmote)
                    .WithMany(p => p.WeeniePropertiesEmoteAction)
                    .HasPrincipalKey(p => new { p.ObjectId, p.Category, p.EmoteSetId })
                    .HasForeignKey(d => new { d.ObjectId, d.EmoteCategory, d.EmoteSetId })
                    .HasConstraintName("wcid_emoteset");
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
