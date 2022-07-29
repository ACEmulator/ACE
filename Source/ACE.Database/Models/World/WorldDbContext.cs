using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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
        public virtual DbSet<LandblockInstance> LandblockInstance { get; set; }
        public virtual DbSet<LandblockInstanceLink> LandblockInstanceLink { get; set; }
        public virtual DbSet<PointsOfInterest> PointsOfInterest { get; set; }
        public virtual DbSet<Quest> Quest { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
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
        public virtual DbSet<TreasureGemCount> TreasureGemCount { get; set; }
        public virtual DbSet<TreasureMaterialBase> TreasureMaterialBase { get; set; }
        public virtual DbSet<TreasureMaterialColor> TreasureMaterialColor { get; set; }
        public virtual DbSet<TreasureMaterialGroups> TreasureMaterialGroups { get; set; }
        public virtual DbSet<TreasureWielded> TreasureWielded { get; set; }
        public virtual DbSet<Version> Version { get; set; }
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
            if (!optionsBuilder.IsConfigured)
            {
                var config = Common.ConfigManager.Config.MySql.World;

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

            modelBuilder.Entity<CookBook>(entity =>
            {
                entity.ToTable("cook_book");

                entity.HasComment("Cook Book for Recipes");

                entity.HasIndex(e => new { e.RecipeId, e.SourceWCID, e.TargetWCID }, "recipe_source_target_uidx")
                    .IsUnique();

                entity.HasIndex(e => e.SourceWCID, "source_idx");

                entity.HasIndex(e => e.TargetWCID, "target_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this cook book instance");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.SourceWCID)
                    .HasColumnName("source_W_C_I_D")
                    .HasComment("Weenie Class Id of the source object for this recipe");

                entity.Property(e => e.TargetWCID)
                    .HasColumnName("target_W_C_I_D")
                    .HasComment("Weenie Class Id of the target object for this recipe");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.CookBook)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("cookbook_recipe");
            });

            modelBuilder.Entity<Encounter>(entity =>
            {
                entity.ToTable("encounter");

                entity.HasComment("Encounters");

                entity.HasIndex(e => new { e.Landblock, e.CellX, e.CellY }, "landblock_cellx_celly_uidx")
                    .IsUnique();

                entity.HasIndex(e => e.Landblock, "landblock_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Encounter");

                entity.Property(e => e.CellX)
                    .HasColumnName("cell_X")
                    .HasComment("CellX position of this Encounter");

                entity.Property(e => e.CellY)
                    .HasColumnName("cell_Y")
                    .HasComment("CellY position of this Encounter");

                entity.Property(e => e.Landblock)
                    .HasColumnName("landblock")
                    .HasComment("Landblock for this Encounter");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of generator/object to spawn for Encounter");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.HasComment("Events");

                entity.HasIndex(e => e.Name, "name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Event");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_Time")
                    .HasDefaultValueSql("'-1'")
                    .HasComment("Unixtime of Event End");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasComment("Unique Event of Quest");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_Time")
                    .HasDefaultValueSql("'-1'")
                    .HasComment("Unixtime of Event Start");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasComment("State of Event (GameEventState)");
            });

            modelBuilder.Entity<HousePortal>(entity =>
            {
                entity.ToTable("house_portal");

                entity.HasComment("House Portal Destinations");

                entity.HasIndex(e => new { e.HouseId, e.ObjCellId }, "house_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this House Portal");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.HouseId)
                    .HasColumnName("house_Id")
                    .HasComment("Unique Id of House");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
            });

            modelBuilder.Entity<LandblockInstance>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PRIMARY");

                entity.ToTable("landblock_instance");

                entity.HasComment("Weenie Instances for each Landblock");

                entity.HasIndex(e => e.Landblock, "instance_landblock_idx");

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasComment("Unique Id of this Instance");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.IsLinkChild)
                    .HasColumnName("is_Link_Child")
                    .HasComment("Is this a child link for any other instances?");

                entity.Property(e => e.Landblock)
                    .HasColumnName("landblock")
                    .HasComputedColumnSql("`obj_Cell_Id` >> 16", false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of object to spawn");
            });

            modelBuilder.Entity<LandblockInstanceLink>(entity =>
            {
                entity.ToTable("landblock_instance_link");

                entity.HasComment("Weenie Instance Links");

                entity.HasIndex(e => e.ChildGuid, "child_idx");

                entity.HasIndex(e => new { e.ParentGuid, e.ChildGuid }, "parent_child_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Instance Link");

                entity.Property(e => e.ChildGuid)
                    .HasColumnName("child_GUID")
                    .HasComment("GUID of child instance");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ParentGuid)
                    .HasColumnName("parent_GUID")
                    .HasComment("GUID of parent instance");

                entity.HasOne(d => d.ParentGu)
                    .WithMany(p => p.LandblockInstanceLink)
                    .HasForeignKey(d => d.ParentGuid)
                    .HasConstraintName("instance_link");
            });

            modelBuilder.Entity<PointsOfInterest>(entity =>
            {
                entity.ToTable("points_of_interest");

                entity.HasComment("Points of Interest for @telepoi command");

                entity.HasIndex(e => e.Name, "name_UNIQUE")
                    .IsUnique()
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 100 });

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this POI");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("name")
                    .HasComment("Name for POI");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of portal weenie to reference for destination of POI");
            });

            modelBuilder.Entity<Quest>(entity =>
            {
                entity.ToTable("quest");

                entity.HasComment("Quests");

                entity.HasIndex(e => e.Name, "name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Quest");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.MaxSolves)
                    .HasColumnName("max_Solves")
                    .HasComment("Maximum number of times Quest can be completed");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message")
                    .HasComment("Quest solved text - unused?");

                entity.Property(e => e.MinDelta)
                    .HasColumnName("min_Delta")
                    .HasComment("Minimum time between Quest completions");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasComment("Unique Name of Quest");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("recipe");

                entity.HasComment("Recipes");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe");

                entity.Property(e => e.DataId).HasColumnName("data_Id");

                entity.Property(e => e.Difficulty).HasColumnName("difficulty");

                entity.Property(e => e.FailAmount)
                    .HasColumnName("fail_Amount")
                    .HasComment("Amount of objects to create upon failing application of this recipe");

                entity.Property(e => e.FailDestroySourceAmount).HasColumnName("fail_Destroy_Source_Amount");

                entity.Property(e => e.FailDestroySourceChance).HasColumnName("fail_Destroy_Source_Chance");

                entity.Property(e => e.FailDestroySourceMessage)
                    .HasColumnType("text")
                    .HasColumnName("fail_Destroy_Source_Message");

                entity.Property(e => e.FailDestroyTargetAmount).HasColumnName("fail_Destroy_Target_Amount");

                entity.Property(e => e.FailDestroyTargetChance).HasColumnName("fail_Destroy_Target_Chance");

                entity.Property(e => e.FailDestroyTargetMessage)
                    .HasColumnType("text")
                    .HasColumnName("fail_Destroy_Target_Message");

                entity.Property(e => e.FailMessage)
                    .HasColumnType("text")
                    .HasColumnName("fail_Message");

                entity.Property(e => e.FailWCID)
                    .HasColumnName("fail_W_C_I_D")
                    .HasComment("Weenie Class Id of object to create upon failing application of this recipe");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.SalvageType).HasColumnName("salvage_Type");

                entity.Property(e => e.Skill).HasColumnName("skill");

                entity.Property(e => e.SuccessAmount)
                    .HasColumnName("success_Amount")
                    .HasComment("Amount of objects to create upon successful application of this recipe");

                entity.Property(e => e.SuccessDestroySourceAmount).HasColumnName("success_Destroy_Source_Amount");

                entity.Property(e => e.SuccessDestroySourceChance).HasColumnName("success_Destroy_Source_Chance");

                entity.Property(e => e.SuccessDestroySourceMessage)
                    .HasColumnType("text")
                    .HasColumnName("success_Destroy_Source_Message");

                entity.Property(e => e.SuccessDestroyTargetAmount).HasColumnName("success_Destroy_Target_Amount");

                entity.Property(e => e.SuccessDestroyTargetChance).HasColumnName("success_Destroy_Target_Chance");

                entity.Property(e => e.SuccessDestroyTargetMessage)
                    .HasColumnType("text")
                    .HasColumnName("success_Destroy_Target_Message");

                entity.Property(e => e.SuccessMessage)
                    .HasColumnType("text")
                    .HasColumnName("success_Message");

                entity.Property(e => e.SuccessWCID)
                    .HasColumnName("success_W_C_I_D")
                    .HasComment("Weenie Class Id of object to create upon successful application of this recipe");

                entity.Property(e => e.Unknown1).HasColumnName("unknown_1");
            });

            modelBuilder.Entity<RecipeMod>(entity =>
            {
                entity.ToTable("recipe_mod");

                entity.HasComment("Recipe Mods");

                entity.HasIndex(e => e.RecipeId, "recipeId_Mod");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.DataId).HasColumnName("data_Id");

                entity.Property(e => e.ExecutesOnSuccess).HasColumnName("executes_On_Success");

                entity.Property(e => e.Health).HasColumnName("health");

                entity.Property(e => e.InstanceId).HasColumnName("instance_Id");

                entity.Property(e => e.Mana).HasColumnName("mana");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stamina).HasColumnName("stamina");

                entity.Property(e => e.Unknown7).HasColumnName("unknown_7");

                entity.Property(e => e.Unknown9).HasColumnName("unknown_9");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeMod)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_Mod");
            });

            modelBuilder.Entity<RecipeModsBool>(entity =>
            {
                entity.ToTable("recipe_mods_bool");

                entity.HasComment("Recipe Bool Mods");

                entity.HasIndex(e => e.RecipeModId, "recipeId_mod_bool");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasComment("Unique Id of Recipe Mod");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsBool)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_bool");
            });

            modelBuilder.Entity<RecipeModsDID>(entity =>
            {
                entity.ToTable("recipe_mods_d_i_d");

                entity.HasComment("Recipe DID Mods");

                entity.HasIndex(e => e.RecipeModId, "recipeId_mod_did");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasComment("Unique Id of Recipe Mod");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsDID)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_did");
            });

            modelBuilder.Entity<RecipeModsFloat>(entity =>
            {
                entity.ToTable("recipe_mods_float");

                entity.HasComment("Recipe Float Mods");

                entity.HasIndex(e => e.RecipeModId, "recipeId_mod_float");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasComment("Unique Id of Recipe Mod");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsFloat)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_float");
            });

            modelBuilder.Entity<RecipeModsIID>(entity =>
            {
                entity.ToTable("recipe_mods_i_i_d");

                entity.HasComment("Recipe IID Mods");

                entity.HasIndex(e => e.RecipeModId, "recipeId_mod_iid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasComment("Unique Id of Recipe Mod");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsIID)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_iid");
            });

            modelBuilder.Entity<RecipeModsInt>(entity =>
            {
                entity.ToTable("recipe_mods_int");

                entity.HasComment("Recipe Int Mods");

                entity.HasIndex(e => e.RecipeModId, "recipeId_mod_int");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasComment("Unique Id of Recipe Mod");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsInt)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_int");
            });

            modelBuilder.Entity<RecipeModsString>(entity =>
            {
                entity.ToTable("recipe_mods_string");

                entity.HasComment("Recipe String Mods");

                entity.HasIndex(e => e.RecipeModId, "recipeId_mod_string");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Mod instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.RecipeModId)
                    .HasColumnName("recipe_Mod_Id")
                    .HasComment("Unique Id of Recipe Mod");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value)
                    .HasColumnType("text")
                    .HasColumnName("value");

                entity.HasOne(d => d.RecipeMod)
                    .WithMany(p => p.RecipeModsString)
                    .HasForeignKey(d => d.RecipeModId)
                    .HasConstraintName("recipeId_mod_string");
            });

            modelBuilder.Entity<RecipeRequirementsBool>(entity =>
            {
                entity.ToTable("recipe_requirements_bool");

                entity.HasComment("Recipe Bool Requirments");

                entity.HasIndex(e => e.RecipeId, "recipeId_req_bool");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Requirement instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsBool)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_bool");
            });

            modelBuilder.Entity<RecipeRequirementsDID>(entity =>
            {
                entity.ToTable("recipe_requirements_d_i_d");

                entity.HasComment("Recipe DID Requirments");

                entity.HasIndex(e => e.RecipeId, "recipeId_req_did");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Requirement instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsDID)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_did");
            });

            modelBuilder.Entity<RecipeRequirementsFloat>(entity =>
            {
                entity.ToTable("recipe_requirements_float");

                entity.HasComment("Recipe Float Requirments");

                entity.HasIndex(e => e.RecipeId, "recipeId_req_float");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Requirement instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsFloat)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_float");
            });

            modelBuilder.Entity<RecipeRequirementsIID>(entity =>
            {
                entity.ToTable("recipe_requirements_i_i_d");

                entity.HasComment("Recipe IID Requirments");

                entity.HasIndex(e => e.RecipeId, "recipeId_req_iid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Requirement instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsIID)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_iid");
            });

            modelBuilder.Entity<RecipeRequirementsInt>(entity =>
            {
                entity.ToTable("recipe_requirements_int");

                entity.HasComment("Recipe Int Requirments");

                entity.HasIndex(e => e.RecipeId, "recipeId_req_int");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Requirement instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsInt)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_int");
            });

            modelBuilder.Entity<RecipeRequirementsString>(entity =>
            {
                entity.ToTable("recipe_requirements_string");

                entity.HasComment("Recipe String Requirments");

                entity.HasIndex(e => e.RecipeId, "recipeId_req_string");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Recipe Requirement instance");

                entity.Property(e => e.Enum).HasColumnName("enum");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.RecipeId)
                    .HasColumnName("recipe_Id")
                    .HasComment("Unique Id of Recipe");

                entity.Property(e => e.Stat).HasColumnName("stat");

                entity.Property(e => e.Value)
                    .HasColumnType("text")
                    .HasColumnName("value");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeRequirementsString)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("recipeId_req_string");
            });

            modelBuilder.Entity<Spell>(entity =>
            {
                entity.ToTable("spell");

                entity.HasComment("Spell Table Extended Data");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Spell");

                entity.Property(e => e.Align).HasColumnName("align");

                entity.Property(e => e.BaseIntensity).HasColumnName("base_Intensity");

                entity.Property(e => e.Boost).HasColumnName("boost");

                entity.Property(e => e.BoostVariance).HasColumnName("boost_Variance");

                entity.Property(e => e.CreateOffsetOriginX).HasColumnName("create_Offset_Origin_X");

                entity.Property(e => e.CreateOffsetOriginY).HasColumnName("create_Offset_Origin_Y");

                entity.Property(e => e.CreateOffsetOriginZ).HasColumnName("create_Offset_Origin_Z");

                entity.Property(e => e.CritFreq).HasColumnName("crit_Freq");

                entity.Property(e => e.CritMultiplier).HasColumnName("crit_Multiplier");

                entity.Property(e => e.DamageRatio).HasColumnName("damage_Ratio");

                entity.Property(e => e.DamageType).HasColumnName("damage_Type");

                entity.Property(e => e.DefaultLaunchAngle).HasColumnName("default_Launch_Angle");

                entity.Property(e => e.Destination).HasColumnName("destination");

                entity.Property(e => e.DimsOriginX).HasColumnName("dims_Origin_X");

                entity.Property(e => e.DimsOriginY).HasColumnName("dims_Origin_Y");

                entity.Property(e => e.DimsOriginZ).HasColumnName("dims_Origin_Z");

                entity.Property(e => e.DispelSchool).HasColumnName("dispel_School");

                entity.Property(e => e.DotDuration).HasColumnName("dot_Duration");

                entity.Property(e => e.DrainPercentage).HasColumnName("drain_Percentage");

                entity.Property(e => e.EType).HasColumnName("e_Type");

                entity.Property(e => e.ElementalModifier).HasColumnName("elemental_Modifier");

                entity.Property(e => e.IgnoreMagicResist).HasColumnName("ignore_Magic_Resist");

                entity.Property(e => e.ImbuedEffect).HasColumnName("imbued_Effect");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Link).HasColumnName("link");

                entity.Property(e => e.LossPercent).HasColumnName("loss_Percent");

                entity.Property(e => e.MaxBoostAllowed).HasColumnName("max_Boost_Allowed");

                entity.Property(e => e.MaxPower).HasColumnName("max_Power");

                entity.Property(e => e.MinPower).HasColumnName("min_Power");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("name");

                entity.Property(e => e.NonTracking).HasColumnName("non_Tracking");

                entity.Property(e => e.NumProjectiles).HasColumnName("num_Projectiles");

                entity.Property(e => e.NumProjectilesVariance).HasColumnName("num_Projectiles_Variance");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.NumberVariance).HasColumnName("number_Variance");

                entity.Property(e => e.PaddingOriginX).HasColumnName("padding_Origin_X");

                entity.Property(e => e.PaddingOriginY).HasColumnName("padding_Origin_Y");

                entity.Property(e => e.PaddingOriginZ).HasColumnName("padding_Origin_Z");

                entity.Property(e => e.PeturbationOriginX).HasColumnName("peturbation_Origin_X");

                entity.Property(e => e.PeturbationOriginY).HasColumnName("peturbation_Origin_Y");

                entity.Property(e => e.PeturbationOriginZ).HasColumnName("peturbation_Origin_Z");

                entity.Property(e => e.PositionAnglesW).HasColumnName("position_Angles_W");

                entity.Property(e => e.PositionAnglesX).HasColumnName("position_Angles_X");

                entity.Property(e => e.PositionAnglesY).HasColumnName("position_Angles_Y");

                entity.Property(e => e.PositionAnglesZ).HasColumnName("position_Angles_Z");

                entity.Property(e => e.PositionObjCellId).HasColumnName("position_Obj_Cell_ID");

                entity.Property(e => e.PositionOriginX).HasColumnName("position_Origin_X");

                entity.Property(e => e.PositionOriginY).HasColumnName("position_Origin_Y");

                entity.Property(e => e.PositionOriginZ).HasColumnName("position_Origin_Z");

                entity.Property(e => e.PowerVariance).HasColumnName("power_Variance");

                entity.Property(e => e.Proportion).HasColumnName("proportion");

                entity.Property(e => e.SlayerCreatureType).HasColumnName("slayer_Creature_Type");

                entity.Property(e => e.SlayerDamageBonus).HasColumnName("slayer_Damage_Bonus");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.SourceLoss).HasColumnName("source_Loss");

                entity.Property(e => e.SpreadAngle).HasColumnName("spread_Angle");

                entity.Property(e => e.StatModKey).HasColumnName("stat_Mod_Key");

                entity.Property(e => e.StatModType).HasColumnName("stat_Mod_Type");

                entity.Property(e => e.StatModVal).HasColumnName("stat_Mod_Val");

                entity.Property(e => e.TransferBitfield).HasColumnName("transfer_Bitfield");

                entity.Property(e => e.TransferCap).HasColumnName("transfer_Cap");

                entity.Property(e => e.Variance).HasColumnName("variance");

                entity.Property(e => e.VerticalAngle).HasColumnName("vertical_Angle");

                entity.Property(e => e.Wcid).HasColumnName("wcid");
            });

            modelBuilder.Entity<TreasureDeath>(entity =>
            {
                entity.ToTable("treasure_death");

                entity.HasComment("Death Treasure");

                entity.HasIndex(e => e.TreasureType, "treasureType_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Treasure");

                entity.Property(e => e.ItemChance).HasColumnName("item_Chance");

                entity.Property(e => e.ItemMaxAmount).HasColumnName("item_Max_Amount");

                entity.Property(e => e.ItemMinAmount).HasColumnName("item_Min_Amount");

                entity.Property(e => e.ItemTreasureTypeSelectionChances).HasColumnName("item_Treasure_Type_Selection_Chances");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LootQualityMod).HasColumnName("loot_Quality_Mod");

                entity.Property(e => e.MagicItemChance).HasColumnName("magic_Item_Chance");

                entity.Property(e => e.MagicItemMaxAmount).HasColumnName("magic_Item_Max_Amount");

                entity.Property(e => e.MagicItemMinAmount).HasColumnName("magic_Item_Min_Amount");

                entity.Property(e => e.MagicItemTreasureTypeSelectionChances).HasColumnName("magic_Item_Treasure_Type_Selection_Chances");

                entity.Property(e => e.MundaneItemChance).HasColumnName("mundane_Item_Chance");

                entity.Property(e => e.MundaneItemMaxAmount).HasColumnName("mundane_Item_Max_Amount");

                entity.Property(e => e.MundaneItemMinAmount).HasColumnName("mundane_Item_Min_Amount");

                entity.Property(e => e.MundaneItemTypeSelectionChances).HasColumnName("mundane_Item_Type_Selection_Chances");

                entity.Property(e => e.Tier).HasColumnName("tier");

                entity.Property(e => e.TreasureType)
                    .HasColumnName("treasure_Type")
                    .HasComment("Type of Treasure for this instance");

                entity.Property(e => e.UnknownChances).HasColumnName("unknown_Chances");
            });

            modelBuilder.Entity<TreasureGemCount>(entity =>
            {
                entity.ToTable("treasure_gem_count");

                entity.HasCharSet("utf16")
                    .UseCollation("utf16_general_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Chance).HasColumnName("chance");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.GemCode).HasColumnName("gem_Code");

                entity.Property(e => e.Tier).HasColumnName("tier");
            });

            modelBuilder.Entity<TreasureMaterialBase>(entity =>
            {
                entity.ToTable("treasure_material_base");

                entity.HasIndex(e => e.Tier, "tier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MaterialCode)
                    .HasColumnName("material_Code")
                    .HasComment("Derived from PropertyInt.TsysMutationData");

                entity.Property(e => e.MaterialId)
                    .HasColumnName("material_Id")
                    .HasComment("MaterialType");

                entity.Property(e => e.Probability).HasColumnName("probability");

                entity.Property(e => e.Tier)
                    .HasColumnName("tier")
                    .HasComment("Loot Tier");
            });

            modelBuilder.Entity<TreasureMaterialColor>(entity =>
            {
                entity.ToTable("treasure_material_color");

                entity.HasIndex(e => e.MaterialId, "material_Id");

                entity.HasIndex(e => e.ColorCode, "tsys_Mutation_Color");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ColorCode).HasColumnName("color_Code");

                entity.Property(e => e.MaterialId).HasColumnName("material_Id");

                entity.Property(e => e.PaletteTemplate).HasColumnName("palette_Template");

                entity.Property(e => e.Probability).HasColumnName("probability");
            });

            modelBuilder.Entity<TreasureMaterialGroups>(entity =>
            {
                entity.ToTable("treasure_material_groups");

                entity.HasIndex(e => e.Tier, "tier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MaterialGroup)
                    .HasColumnName("material_Group")
                    .HasComment("MaterialType Group");

                entity.Property(e => e.MaterialId)
                    .HasColumnName("material_Id")
                    .HasComment("MaterialType");

                entity.Property(e => e.Probability).HasColumnName("probability");

                entity.Property(e => e.Tier)
                    .HasColumnName("tier")
                    .HasComment("Loot Tier");
            });

            modelBuilder.Entity<TreasureWielded>(entity =>
            {
                entity.ToTable("treasure_wielded");

                entity.HasComment("Wielded Treasure");

                entity.HasIndex(e => e.TreasureType, "treasureType_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Treasure");

                entity.Property(e => e.ContinuesPreviousSet).HasColumnName("continues_Previous_Set");

                entity.Property(e => e.HasSubSet).HasColumnName("has_Sub_Set");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PaletteId)
                    .HasColumnName("palette_Id")
                    .HasComment("Palette Color of Object Generated");

                entity.Property(e => e.Probability).HasColumnName("probability");

                entity.Property(e => e.SetStart).HasColumnName("set_Start");

                entity.Property(e => e.Shade)
                    .HasColumnName("shade")
                    .HasComment("Shade of Object generated's Palette");

                entity.Property(e => e.StackSize)
                    .HasColumnName("stack_Size")
                    .HasDefaultValueSql("'1'")
                    .HasComment("Stack Size of object to create (-1 = infinite)");

                entity.Property(e => e.StackSizeVariance).HasColumnName("stack_Size_Variance");

                entity.Property(e => e.TreasureType)
                    .HasColumnName("treasure_Type")
                    .HasComment("Type of Treasure for this instance");

                entity.Property(e => e.Unknown1)
                    .HasColumnName("unknown_1")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown10)
                    .HasColumnName("unknown_10")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown11)
                    .HasColumnName("unknown_11")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown12)
                    .HasColumnName("unknown_12")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown3)
                    .HasColumnName("unknown_3")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown4)
                    .HasColumnName("unknown_4")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown5)
                    .HasColumnName("unknown_5")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.Unknown9)
                    .HasColumnName("unknown_9")
                    .HasComment("Always 0 in cache.bin");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of Treasure to Generate");
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.ToTable("version");

                entity.HasComment("Version Information");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaseVersion)
                    .HasMaxLength(45)
                    .HasColumnName("base_Version");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PatchVersion)
                    .HasMaxLength(45)
                    .HasColumnName("patch_Version");
            });

            modelBuilder.Entity<Weenie>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PRIMARY");

                entity.ToTable("weenie");

                entity.HasComment("Weenies");

                entity.HasIndex(e => e.ClassName, "className_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClassId)
                    .HasColumnName("class_Id")
                    .HasComment("Weenie Class Id (wcid) / (WCID) / (weenieClassId)");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("class_Name")
                    .HasComment("Weenie Class Name (W_????_CLASS)");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_Modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("WeenieType");
            });

            modelBuilder.Entity<WeeniePropertiesAnimPart>(entity =>
            {
                entity.ToTable("weenie_properties_anim_part");

                entity.HasComment("Animation Part Changes (from PCAPs) of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Index }, "object_Id_index_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.AnimationId).HasColumnName("animation_Id");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesAnimPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_animpart");
            });

            modelBuilder.Entity<WeeniePropertiesAttribute>(entity =>
            {
                entity.ToTable("weenie_properties_attribute");

                entity.HasComment("Attribute Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_attribute_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.CPSpent)
                    .HasColumnName("c_P_Spent")
                    .HasComment("XP spent on this attribute");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasComment("innate points");

                entity.Property(e => e.LevelFromCP)
                    .HasColumnName("level_From_C_P")
                    .HasComment("points raised");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyAttribute.????)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesAttribute)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute");
            });

            modelBuilder.Entity<WeeniePropertiesAttribute2nd>(entity =>
            {
                entity.ToTable("weenie_properties_attribute_2nd");

                entity.HasComment("Attribute2nd (Vital) Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_attribute2nd_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertyAttribute2nd.????)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesAttribute2nd)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_attribute2nd");
            });

            modelBuilder.Entity<WeeniePropertiesBodyPart>(entity =>
            {
                entity.ToTable("weenie_properties_body_part");

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
                    .WithMany(p => p.WeeniePropertiesBodyPart)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bodypart");
            });

            modelBuilder.Entity<WeeniePropertiesBook>(entity =>
            {
                entity.ToTable("weenie_properties_book");

                entity.HasComment("Book Properties of Weenies");

                entity.HasIndex(e => e.ObjectId, "wcid_bookdata_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.MaxNumCharsPerPage)
                    .HasColumnName("max_Num_Chars_Per_Page")
                    .HasDefaultValueSql("'1000'")
                    .HasComment("Maximum number of characters per page");

                entity.Property(e => e.MaxNumPages)
                    .HasColumnName("max_Num_Pages")
                    .HasDefaultValueSql("'1'")
                    .HasComment("Maximum number of pages per book");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.HasOne(d => d.Object)
                    .WithOne(p => p.WeeniePropertiesBook)
                    .HasForeignKey<WeeniePropertiesBook>(d => d.ObjectId)
                    .HasConstraintName("wcid_bookdata");
            });

            modelBuilder.Entity<WeeniePropertiesBookPageData>(entity =>
            {
                entity.ToTable("weenie_properties_book_page_data");

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
                    .WithMany(p => p.WeeniePropertiesBookPageData)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_pagedata");
            });

            modelBuilder.Entity<WeeniePropertiesBool>(entity =>
            {
                entity.ToTable("weenie_properties_bool");

                entity.HasComment("Bool Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_bool_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesBool)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_bool");
            });

            modelBuilder.Entity<WeeniePropertiesCreateList>(entity =>
            {
                entity.ToTable("weenie_properties_create_list");

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
                    .HasDefaultValueSql("'1'")
                    .HasComment("Stack Size of object to create (-1 = infinite)");

                entity.Property(e => e.TryToBond)
                    .HasColumnName("try_To_Bond")
                    .HasComment("Unused?");

                entity.Property(e => e.WeenieClassId)
                    .HasColumnName("weenie_Class_Id")
                    .HasComment("Weenie Class Id of object to Create");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesCreateList)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_createlist");
            });

            modelBuilder.Entity<WeeniePropertiesDID>(entity =>
            {
                entity.ToTable("weenie_properties_d_i_d");

                entity.HasComment("DataID Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_did_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesDID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_did");
            });

            modelBuilder.Entity<WeeniePropertiesEmote>(entity =>
            {
                entity.ToTable("weenie_properties_emote");

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
                    .HasDefaultValueSql("'1'")
                    .HasComment("Probability of this EmoteSet being chosen");

                entity.Property(e => e.Quest)
                    .HasColumnType("text")
                    .HasColumnName("quest");

                entity.Property(e => e.Style).HasColumnName("style");

                entity.Property(e => e.Substyle).HasColumnName("substyle");

                entity.Property(e => e.VendorType).HasColumnName("vendor_Type");

                entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesEmote)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_emote");
            });

            modelBuilder.Entity<WeeniePropertiesEmoteAction>(entity =>
            {
                entity.ToTable("weenie_properties_emote_action");

                entity.HasComment("EmoteAction Properties of Weenies");

                entity.HasIndex(e => new { e.EmoteId, e.Order }, "emoteid_order_uidx")
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
                    .HasDefaultValueSql("'1'")
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
                    .HasDefaultValueSql("'1'")
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
                    .WithMany(p => p.WeeniePropertiesEmoteAction)
                    .HasForeignKey(d => d.EmoteId)
                    .HasConstraintName("emoteid_emoteaction");
            });

            modelBuilder.Entity<WeeniePropertiesEventFilter>(entity =>
            {
                entity.ToTable("weenie_properties_event_filter");

                entity.HasComment("EventFilter Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Event }, "wcid_eventfilter_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasComment("Id of Event to filter");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesEventFilter)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_eventfilter");
            });

            modelBuilder.Entity<WeeniePropertiesFloat>(entity =>
            {
                entity.ToTable("weenie_properties_float");

                entity.HasComment("Float Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_float_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesFloat)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_float");
            });

            modelBuilder.Entity<WeeniePropertiesGenerator>(entity =>
            {
                entity.ToTable("weenie_properties_generator");

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
                    .HasComment("Amount of delay before generation");

                entity.Property(e => e.InitCreate)
                    .HasColumnName("init_Create")
                    .HasDefaultValueSql("'1'")
                    .HasComment("Number of object to generate initially");

                entity.Property(e => e.MaxCreate)
                    .HasColumnName("max_Create")
                    .HasDefaultValueSql("'1'")
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

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("'1'");

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
                    .HasDefaultValueSql("'2'")
                    .HasComment("When to generate the weenie object");

                entity.Property(e => e.WhereCreate)
                    .HasColumnName("where_Create")
                    .HasDefaultValueSql("'4'")
                    .HasComment("Where to generate the weenie object");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesGenerator)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_generator");
            });

            modelBuilder.Entity<WeeniePropertiesIID>(entity =>
            {
                entity.ToTable("weenie_properties_i_i_d");

                entity.HasComment("InstanceID Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_iid_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesIID)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_iid");
            });

            modelBuilder.Entity<WeeniePropertiesInt>(entity =>
            {
                entity.ToTable("weenie_properties_int");

                entity.HasComment("Int Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_int_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesInt)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int");
            });

            modelBuilder.Entity<WeeniePropertiesInt64>(entity =>
            {
                entity.ToTable("weenie_properties_int64");

                entity.HasComment("Int64 Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_int64_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesInt64)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_int64");
            });

            modelBuilder.Entity<WeeniePropertiesPalette>(entity =>
            {
                entity.ToTable("weenie_properties_palette");

                entity.HasComment("Palette Changes (from PCAPs) of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.SubPaletteId, e.Offset, e.Length }, "object_Id_subPaletteId_offset_length_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

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

                entity.HasComment("Position Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.PositionType }, "wcid_position_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Position");

                entity.Property(e => e.AnglesW).HasColumnName("angles_W");

                entity.Property(e => e.AnglesX).HasColumnName("angles_X");

                entity.Property(e => e.AnglesY).HasColumnName("angles_Y");

                entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");

                entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.OriginX).HasColumnName("origin_X");

                entity.Property(e => e.OriginY).HasColumnName("origin_Y");

                entity.Property(e => e.OriginZ).HasColumnName("origin_Z");

                entity.Property(e => e.PositionType)
                    .HasColumnName("position_Type")
                    .HasComment("Type of Position the value applies to (PositionType.????)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesPosition)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_position");
            });

            modelBuilder.Entity<WeeniePropertiesSkill>(entity =>
            {
                entity.ToTable("weenie_properties_skill");

                entity.HasComment("Skill Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_skill_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.InitLevel)
                    .HasColumnName("init_Level")
                    .HasComment("starting point for advancement of the skill (eg bonus points)");

                entity.Property(e => e.LastUsedTime)
                    .HasColumnName("last_Used_Time")
                    .HasComment("time skill was last used");

                entity.Property(e => e.LevelFromPP)
                    .HasColumnName("level_From_P_P")
                    .HasComment("points raised");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.PP)
                    .HasColumnName("p_p")
                    .HasComment("XP spent on this skill");

                entity.Property(e => e.ResistanceAtLastCheck)
                    .HasColumnName("resistance_At_Last_Check")
                    .HasComment("last use difficulty");

                entity.Property(e => e.SAC)
                    .HasColumnName("s_a_c")
                    .HasComment("skill state");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Type of Property the value applies to (PropertySkill.????)");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesSkill)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_skill");
            });

            modelBuilder.Entity<WeeniePropertiesSpellBook>(entity =>
            {
                entity.ToTable("weenie_properties_spell_book");

                entity.HasComment("SpellBook Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Spell }, "wcid_spellbook_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("'2'")
                    .HasComment("Chance to cast this spell");

                entity.Property(e => e.Spell)
                    .HasColumnName("spell")
                    .HasComment("Id of Spell");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesSpellBook)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_spellbook");
            });

            modelBuilder.Entity<WeeniePropertiesString>(entity =>
            {
                entity.ToTable("weenie_properties_string");

                entity.HasComment("String Properties of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_string_type_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

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
                    .WithMany(p => p.WeeniePropertiesString)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_string");
            });

            modelBuilder.Entity<WeeniePropertiesTextureMap>(entity =>
            {
                entity.ToTable("weenie_properties_texture_map");

                entity.HasComment("Texture Map Changes (from PCAPs) of Weenies");

                entity.HasIndex(e => new { e.ObjectId, e.Index, e.OldId }, "object_Id_index_oldId_uidx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Unique Id of this Property");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.NewId).HasColumnName("new_Id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasComment("Id of the object this property belongs to");

                entity.Property(e => e.OldId).HasColumnName("old_Id");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.WeeniePropertiesTextureMap)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("wcid_texturemap");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
