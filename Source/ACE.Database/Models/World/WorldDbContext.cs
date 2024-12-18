using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ACE.Database.Models.World;

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
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<CookBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cook_book", tb => tb.HasComment("Cook Book for Recipes"));

            entity.HasIndex(e => new { e.RecipeId, e.SourceWCID, e.TargetWCID }, "recipe_source_target_uidx").IsUnique();

            entity.HasIndex(e => e.SourceWCID, "source_idx");

            entity.HasIndex(e => e.TargetWCID, "target_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this cook book instance")
                .HasColumnName("id");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.SourceWCID)
                .HasComment("Weenie Class Id of the source object for this recipe")
                .HasColumnName("source_W_C_I_D");
            entity.Property(e => e.TargetWCID)
                .HasComment("Weenie Class Id of the target object for this recipe")
                .HasColumnName("target_W_C_I_D");

            entity.HasOne(d => d.Recipe).WithMany(p => p.CookBook)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("cookbook_recipe");
        });

        modelBuilder.Entity<Encounter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("encounter", tb => tb.HasComment("Encounters"));

            entity.HasIndex(e => new { e.Landblock, e.CellX, e.CellY }, "landblock_cellx_celly_uidx").IsUnique();

            entity.HasIndex(e => e.Landblock, "landblock_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Encounter")
                .HasColumnName("id");
            entity.Property(e => e.CellX)
                .HasComment("CellX position of this Encounter")
                .HasColumnName("cell_X");
            entity.Property(e => e.CellY)
                .HasComment("CellY position of this Encounter")
                .HasColumnName("cell_Y");
            entity.Property(e => e.Landblock)
                .HasComment("Landblock for this Encounter")
                .HasColumnName("landblock");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of generator/object to spawn for Encounter")
                .HasColumnName("weenie_Class_Id");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("event", tb => tb.HasComment("Events"));

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Event")
                .HasColumnName("id");
            entity.Property(e => e.EndTime)
                .HasDefaultValueSql("'-1'")
                .HasComment("Unixtime of Event End")
                .HasColumnName("end_Time");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasComment("Unique Event of Quest")
                .HasColumnName("name");
            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("'-1'")
                .HasComment("Unixtime of Event Start")
                .HasColumnName("start_Time");
            entity.Property(e => e.State)
                .HasComment("State of Event (GameEventState)")
                .HasColumnName("state");
        });

        modelBuilder.Entity<HousePortal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("house_portal", tb => tb.HasComment("House Portal Destinations"));

            entity.HasIndex(e => new { e.HouseId, e.ObjCellId }, "house_Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this House Portal")
                .HasColumnName("id");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.HouseId)
                .HasComment("Unique Id of House")
                .HasColumnName("house_Id");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");
            entity.Property(e => e.OriginX).HasColumnName("origin_X");
            entity.Property(e => e.OriginY).HasColumnName("origin_Y");
            entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
        });

        modelBuilder.Entity<LandblockInstance>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PRIMARY");

            entity.ToTable("landblock_instance", tb => tb.HasComment("Weenie Instances for each Landblock"));

            entity.HasIndex(e => e.Landblock, "instance_landblock_idx");

            entity.Property(e => e.Guid)
                .HasComment("Unique Id of this Instance")
                .HasColumnName("guid");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.IsLinkChild)
                .HasComment("Is this a child link for any other instances?")
                .HasColumnName("is_Link_Child");
            entity.Property(e => e.Landblock)
                .HasComputedColumnSql("`obj_Cell_Id` >> 16", false)
                .HasColumnName("landblock");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");
            entity.Property(e => e.OriginX).HasColumnName("origin_X");
            entity.Property(e => e.OriginY).HasColumnName("origin_Y");
            entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of object to spawn")
                .HasColumnName("weenie_Class_Id");
        });

        modelBuilder.Entity<LandblockInstanceLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("landblock_instance_link", tb => tb.HasComment("Weenie Instance Links"));

            entity.HasIndex(e => e.ChildGuid, "child_idx");

            entity.HasIndex(e => new { e.ParentGuid, e.ChildGuid }, "parent_child_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Instance Link")
                .HasColumnName("id");
            entity.Property(e => e.ChildGuid)
                .HasComment("GUID of child instance")
                .HasColumnName("child_GUID");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.ParentGuid)
                .HasComment("GUID of parent instance")
                .HasColumnName("parent_GUID");

            entity.HasOne(d => d.Parent).WithMany(p => p.LandblockInstanceLink)
                .HasForeignKey(d => d.ParentGuid)
                .HasConstraintName("instance_link");
        });

        modelBuilder.Entity<PointsOfInterest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("points_of_interest", tb => tb.HasComment("Points of Interest for @telepoi command"));

            entity.HasIndex(e => e.Name, "name_UNIQUE")
                .IsUnique()
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 100 });

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this POI")
                .HasColumnName("id");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasComment("Name for POI")
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of portal weenie to reference for destination of POI")
                .HasColumnName("weenie_Class_Id");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("quest", tb => tb.HasComment("Quests"));

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Quest")
                .HasColumnName("id");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.MaxSolves)
                .HasComment("Maximum number of times Quest can be completed")
                .HasColumnName("max_Solves");
            entity.Property(e => e.Message)
                .HasComment("Quest solved text - unused?")
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.MinDelta)
                .HasComment("Minimum time between Quest completions")
                .HasColumnName("min_Delta");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasComment("Unique Name of Quest")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe", tb => tb.HasComment("Recipes"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Unique Id of this Recipe")
                .HasColumnName("id");
            entity.Property(e => e.DataId).HasColumnName("data_Id");
            entity.Property(e => e.Difficulty).HasColumnName("difficulty");
            entity.Property(e => e.FailAmount)
                .HasComment("Amount of objects to create upon failing application of this recipe")
                .HasColumnName("fail_Amount");
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
                .HasComment("Weenie Class Id of object to create upon failing application of this recipe")
                .HasColumnName("fail_W_C_I_D");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.SalvageType).HasColumnName("salvage_Type");
            entity.Property(e => e.Skill).HasColumnName("skill");
            entity.Property(e => e.SuccessAmount)
                .HasComment("Amount of objects to create upon successful application of this recipe")
                .HasColumnName("success_Amount");
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
                .HasComment("Weenie Class Id of object to create upon successful application of this recipe")
                .HasColumnName("success_W_C_I_D");
            entity.Property(e => e.Unknown1).HasColumnName("unknown_1");
        });

        modelBuilder.Entity<RecipeMod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mod", tb => tb.HasComment("Recipe Mods"));

            entity.HasIndex(e => e.RecipeId, "recipeId_Mod");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.DataId).HasColumnName("data_Id");
            entity.Property(e => e.ExecutesOnSuccess).HasColumnName("executes_On_Success");
            entity.Property(e => e.Health).HasColumnName("health");
            entity.Property(e => e.InstanceId).HasColumnName("instance_Id");
            entity.Property(e => e.Mana).HasColumnName("mana");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stamina).HasColumnName("stamina");
            entity.Property(e => e.Unknown7).HasColumnName("unknown_7");
            entity.Property(e => e.Unknown9).HasColumnName("unknown_9");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeMod)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_Mod");
        });

        modelBuilder.Entity<RecipeModsBool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mods_bool", tb => tb.HasComment("Recipe Bool Mods"));

            entity.HasIndex(e => e.RecipeModId, "recipeId_mod_bool");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.RecipeModId)
                .HasComment("Unique Id of Recipe Mod")
                .HasColumnName("recipe_Mod_Id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.RecipeMod).WithMany(p => p.RecipeModsBool)
                .HasForeignKey(d => d.RecipeModId)
                .HasConstraintName("recipeId_mod_bool");
        });

        modelBuilder.Entity<RecipeModsDID>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mods_d_i_d", tb => tb.HasComment("Recipe DID Mods"));

            entity.HasIndex(e => e.RecipeModId, "recipeId_mod_did");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.RecipeModId)
                .HasComment("Unique Id of Recipe Mod")
                .HasColumnName("recipe_Mod_Id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.RecipeMod).WithMany(p => p.RecipeModsDID)
                .HasForeignKey(d => d.RecipeModId)
                .HasConstraintName("recipeId_mod_did");
        });

        modelBuilder.Entity<RecipeModsFloat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mods_float", tb => tb.HasComment("Recipe Float Mods"));

            entity.HasIndex(e => e.RecipeModId, "recipeId_mod_float");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.RecipeModId)
                .HasComment("Unique Id of Recipe Mod")
                .HasColumnName("recipe_Mod_Id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.RecipeMod).WithMany(p => p.RecipeModsFloat)
                .HasForeignKey(d => d.RecipeModId)
                .HasConstraintName("recipeId_mod_float");
        });

        modelBuilder.Entity<RecipeModsIID>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mods_i_i_d", tb => tb.HasComment("Recipe IID Mods"));

            entity.HasIndex(e => e.RecipeModId, "recipeId_mod_iid");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.RecipeModId)
                .HasComment("Unique Id of Recipe Mod")
                .HasColumnName("recipe_Mod_Id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.RecipeMod).WithMany(p => p.RecipeModsIID)
                .HasForeignKey(d => d.RecipeModId)
                .HasConstraintName("recipeId_mod_iid");
        });

        modelBuilder.Entity<RecipeModsInt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mods_int", tb => tb.HasComment("Recipe Int Mods"));

            entity.HasIndex(e => e.RecipeModId, "recipeId_mod_int");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.RecipeModId)
                .HasComment("Unique Id of Recipe Mod")
                .HasColumnName("recipe_Mod_Id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.RecipeMod).WithMany(p => p.RecipeModsInt)
                .HasForeignKey(d => d.RecipeModId)
                .HasConstraintName("recipeId_mod_int");
        });

        modelBuilder.Entity<RecipeModsString>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_mods_string", tb => tb.HasComment("Recipe String Mods"));

            entity.HasIndex(e => e.RecipeModId, "recipeId_mod_string");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Mod instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.RecipeModId)
                .HasComment("Unique Id of Recipe Mod")
                .HasColumnName("recipe_Mod_Id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasColumnName("value");

            entity.HasOne(d => d.RecipeMod).WithMany(p => p.RecipeModsString)
                .HasForeignKey(d => d.RecipeModId)
                .HasConstraintName("recipeId_mod_string");
        });

        modelBuilder.Entity<RecipeRequirementsBool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_requirements_bool", tb => tb.HasComment("Recipe Bool Requirments"));

            entity.HasIndex(e => e.RecipeId, "recipeId_req_bool");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Requirement instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRequirementsBool)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_req_bool");
        });

        modelBuilder.Entity<RecipeRequirementsDID>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_requirements_d_i_d", tb => tb.HasComment("Recipe DID Requirments"));

            entity.HasIndex(e => e.RecipeId, "recipeId_req_did");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Requirement instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRequirementsDID)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_req_did");
        });

        modelBuilder.Entity<RecipeRequirementsFloat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_requirements_float", tb => tb.HasComment("Recipe Float Requirments"));

            entity.HasIndex(e => e.RecipeId, "recipeId_req_float");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Requirement instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRequirementsFloat)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_req_float");
        });

        modelBuilder.Entity<RecipeRequirementsIID>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_requirements_i_i_d", tb => tb.HasComment("Recipe IID Requirments"));

            entity.HasIndex(e => e.RecipeId, "recipeId_req_iid");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Requirement instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRequirementsIID)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_req_iid");
        });

        modelBuilder.Entity<RecipeRequirementsInt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_requirements_int", tb => tb.HasComment("Recipe Int Requirments"));

            entity.HasIndex(e => e.RecipeId, "recipeId_req_int");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Requirement instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRequirementsInt)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_req_int");
        });

        modelBuilder.Entity<RecipeRequirementsString>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recipe_requirements_string", tb => tb.HasComment("Recipe String Requirments"));

            entity.HasIndex(e => e.RecipeId, "recipeId_req_string");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Recipe Requirement instance")
                .HasColumnName("id");
            entity.Property(e => e.Enum).HasColumnName("enum");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RecipeId)
                .HasComment("Unique Id of Recipe")
                .HasColumnName("recipe_Id");
            entity.Property(e => e.Stat).HasColumnName("stat");
            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasColumnName("value");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRequirementsString)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipeId_req_string");
        });

        modelBuilder.Entity<Spell>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("spell", tb => tb.HasComment("Spell Table Extended Data"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Unique Id of this Spell")
                .HasColumnName("id");
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
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
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
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("treasure_death", tb => tb.HasComment("Death Treasure"));

            entity.HasIndex(e => e.TreasureType, "treasureType_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Treasure")
                .HasColumnName("id");
            entity.Property(e => e.ItemChance).HasColumnName("item_Chance");
            entity.Property(e => e.ItemMaxAmount).HasColumnName("item_Max_Amount");
            entity.Property(e => e.ItemMinAmount).HasColumnName("item_Min_Amount");
            entity.Property(e => e.ItemTreasureTypeSelectionChances).HasColumnName("item_Treasure_Type_Selection_Chances");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
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
                .HasComment("Type of Treasure for this instance")
                .HasColumnName("treasure_Type");
            entity.Property(e => e.UnknownChances).HasColumnName("unknown_Chances");
        });

        modelBuilder.Entity<TreasureGemCount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("treasure_gem_count")
                .HasCharSet("utf16")
                .UseCollation("utf16_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Chance).HasColumnName("chance");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.GemCode).HasColumnName("gem_Code");
            entity.Property(e => e.Tier).HasColumnName("tier");
        });

        modelBuilder.Entity<TreasureMaterialBase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("treasure_material_base");

            entity.HasIndex(e => e.Tier, "tier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialCode)
                .HasComment("Derived from PropertyInt.TsysMutationData")
                .HasColumnName("material_Code");
            entity.Property(e => e.MaterialId)
                .HasComment("MaterialType")
                .HasColumnName("material_Id");
            entity.Property(e => e.Probability).HasColumnName("probability");
            entity.Property(e => e.Tier)
                .HasComment("Loot Tier")
                .HasColumnName("tier");
        });

        modelBuilder.Entity<TreasureMaterialColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

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
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("treasure_material_groups");

            entity.HasIndex(e => e.Tier, "tier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialGroup)
                .HasComment("MaterialType Group")
                .HasColumnName("material_Group");
            entity.Property(e => e.MaterialId)
                .HasComment("MaterialType")
                .HasColumnName("material_Id");
            entity.Property(e => e.Probability).HasColumnName("probability");
            entity.Property(e => e.Tier)
                .HasComment("Loot Tier")
                .HasColumnName("tier");
        });

        modelBuilder.Entity<TreasureWielded>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("treasure_wielded", tb => tb.HasComment("Wielded Treasure"));

            entity.HasIndex(e => e.TreasureType, "treasureType_idx");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Treasure")
                .HasColumnName("id");
            entity.Property(e => e.ContinuesPreviousSet).HasColumnName("continues_Previous_Set");
            entity.Property(e => e.HasSubSet).HasColumnName("has_Sub_Set");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.PaletteId)
                .HasComment("Palette Color of Object Generated")
                .HasColumnName("palette_Id");
            entity.Property(e => e.Probability).HasColumnName("probability");
            entity.Property(e => e.SetStart).HasColumnName("set_Start");
            entity.Property(e => e.Shade)
                .HasComment("Shade of Object generated's Palette")
                .HasColumnName("shade");
            entity.Property(e => e.StackSize)
                .HasDefaultValueSql("'1'")
                .HasComment("Stack Size of object to create (-1 = infinite)")
                .HasColumnName("stack_Size");
            entity.Property(e => e.StackSizeVariance).HasColumnName("stack_Size_Variance");
            entity.Property(e => e.TreasureType)
                .HasComment("Type of Treasure for this instance")
                .HasColumnName("treasure_Type");
            entity.Property(e => e.Unknown1)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_1");
            entity.Property(e => e.Unknown10)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_10");
            entity.Property(e => e.Unknown11)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_11");
            entity.Property(e => e.Unknown12)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_12");
            entity.Property(e => e.Unknown3)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_3");
            entity.Property(e => e.Unknown4)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_4");
            entity.Property(e => e.Unknown5)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_5");
            entity.Property(e => e.Unknown9)
                .HasComment("Always 0 in cache.bin")
                .HasColumnName("unknown_9");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of Treasure to Generate")
                .HasColumnName("weenie_Class_Id");
        });

        modelBuilder.Entity<Version>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("version", tb => tb.HasComment("Version Information"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BaseVersion)
                .HasMaxLength(45)
                .HasColumnName("base_Version");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.PatchVersion)
                .HasMaxLength(45)
                .HasColumnName("patch_Version");
        });

        modelBuilder.Entity<Weenie>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PRIMARY");

            entity.ToTable("weenie", tb => tb.HasComment("Weenies"));

            entity.HasIndex(e => e.ClassName, "className_UNIQUE").IsUnique();

            entity.Property(e => e.ClassId)
                .HasComment("Weenie Class Id (wcid) / (WCID) / (weenieClassId)")
                .HasColumnName("class_Id");
            entity.Property(e => e.ClassName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Weenie Class Name (W_????_CLASS)")
                .HasColumnName("class_Name");
            entity.Property(e => e.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_Modified");
            entity.Property(e => e.Type)
                .HasComment("WeenieType")
                .HasColumnName("type");
        });

        modelBuilder.Entity<WeeniePropertiesAnimPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_anim_part", tb => tb.HasComment("Animation Part Changes (from PCAPs) of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Index }, "object_Id_index_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.AnimationId).HasColumnName("animation_Id");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesAnimPart)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_animpart");
        });

        modelBuilder.Entity<WeeniePropertiesAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_attribute", tb => tb.HasComment("Attribute Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_attribute_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.CPSpent)
                .HasComment("XP spent on this attribute")
                .HasColumnName("c_P_Spent");
            entity.Property(e => e.InitLevel)
                .HasComment("innate points")
                .HasColumnName("init_Level");
            entity.Property(e => e.LevelFromCP)
                .HasComment("points raised")
                .HasColumnName("level_From_C_P");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyAttribute.????)")
                .HasColumnName("type");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesAttribute)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_attribute");
        });

        modelBuilder.Entity<WeeniePropertiesAttribute2nd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_attribute_2nd", tb => tb.HasComment("Attribute2nd (Vital) Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_attribute2nd_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
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
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyAttribute2nd.????)")
                .HasColumnName("type");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesAttribute2nd)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_attribute2nd");
        });

        modelBuilder.Entity<WeeniePropertiesBodyPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_body_part", tb => tb.HasComment("Body Part Properties of Weenies"));

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

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesBodyPart)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_bodypart");
        });

        modelBuilder.Entity<WeeniePropertiesBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_book", tb => tb.HasComment("Book Properties of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_bookdata_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.MaxNumCharsPerPage)
                .HasDefaultValueSql("'1000'")
                .HasComment("Maximum number of characters per page")
                .HasColumnName("max_Num_Chars_Per_Page");
            entity.Property(e => e.MaxNumPages)
                .HasDefaultValueSql("'1'")
                .HasComment("Maximum number of pages per book")
                .HasColumnName("max_Num_Pages");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");

            entity.HasOne(d => d.Object).WithOne(p => p.WeeniePropertiesBook)
                .HasForeignKey<WeeniePropertiesBook>(d => d.ObjectId)
                .HasConstraintName("wcid_bookdata");
        });

        modelBuilder.Entity<WeeniePropertiesBookPageData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_book_page_data", tb => tb.HasComment("Page Properties of Weenies"));

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

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesBookPageData)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_pagedata");
        });

        modelBuilder.Entity<WeeniePropertiesBool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_bool", tb => tb.HasComment("Bool Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_bool_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyBool.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesBool)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_bool");
        });

        modelBuilder.Entity<WeeniePropertiesCreateList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_create_list", tb => tb.HasComment("CreateList Properties of Weenies"));

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
                .HasDefaultValueSql("'1'")
                .HasComment("Stack Size of object to create (-1 = infinite)")
                .HasColumnName("stack_Size");
            entity.Property(e => e.TryToBond)
                .HasComment("Unused?")
                .HasColumnName("try_To_Bond");
            entity.Property(e => e.WeenieClassId)
                .HasComment("Weenie Class Id of object to Create")
                .HasColumnName("weenie_Class_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesCreateList)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_createlist");
        });

        modelBuilder.Entity<WeeniePropertiesDID>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_d_i_d", tb => tb.HasComment("DataID Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_did_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyDataId.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesDID)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_did");
        });

        modelBuilder.Entity<WeeniePropertiesEmote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_emote", tb => tb.HasComment("Emote Properties of Weenies"));

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
                .HasDefaultValueSql("'1'")
                .HasComment("Probability of this EmoteSet being chosen")
                .HasColumnName("probability");
            entity.Property(e => e.Quest)
                .HasColumnType("text")
                .HasColumnName("quest");
            entity.Property(e => e.Style).HasColumnName("style");
            entity.Property(e => e.Substyle).HasColumnName("substyle");
            entity.Property(e => e.VendorType).HasColumnName("vendor_Type");
            entity.Property(e => e.WeenieClassId).HasColumnName("weenie_Class_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesEmote)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_emote");
        });

        modelBuilder.Entity<WeeniePropertiesEmoteAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_emote_action", tb => tb.HasComment("EmoteAction Properties of Weenies"));

            entity.HasIndex(e => new { e.EmoteId, e.Order }, "emoteid_order_uidx").IsUnique();

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
                .HasDefaultValueSql("'1'")
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
                .HasDefaultValueSql("'1'")
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

            entity.HasOne(d => d.Emote).WithMany(p => p.WeeniePropertiesEmoteAction)
                .HasForeignKey(d => d.EmoteId)
                .HasConstraintName("emoteid_emoteaction");
        });

        modelBuilder.Entity<WeeniePropertiesEventFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_event_filter", tb => tb.HasComment("EventFilter Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Event }, "wcid_eventfilter_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Event)
                .HasComment("Id of Event to filter")
                .HasColumnName("event");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesEventFilter)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_eventfilter");
        });

        modelBuilder.Entity<WeeniePropertiesFloat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_float", tb => tb.HasComment("Float Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_float_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyFloat.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesFloat)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_float");
        });

        modelBuilder.Entity<WeeniePropertiesGenerator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_generator", tb => tb.HasComment("Generator Properties of Weenies"));

            entity.HasIndex(e => e.ObjectId, "wcid_generator");

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.Delay)
                .HasComment("Amount of delay before generation")
                .HasColumnName("delay");
            entity.Property(e => e.InitCreate)
                .HasDefaultValueSql("'1'")
                .HasComment("Number of object to generate initially")
                .HasColumnName("init_Create");
            entity.Property(e => e.MaxCreate)
                .HasDefaultValueSql("'1'")
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
            entity.Property(e => e.Probability)
                .HasDefaultValueSql("'1'")
                .HasColumnName("probability");
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
                .HasDefaultValueSql("'2'")
                .HasComment("When to generate the weenie object")
                .HasColumnName("when_Create");
            entity.Property(e => e.WhereCreate)
                .HasDefaultValueSql("'4'")
                .HasComment("Where to generate the weenie object")
                .HasColumnName("where_Create");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesGenerator)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_generator");
        });

        modelBuilder.Entity<WeeniePropertiesIID>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_i_i_d", tb => tb.HasComment("InstanceID Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_iid_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyInstanceId.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesIID)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_iid");
        });

        modelBuilder.Entity<WeeniePropertiesInt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_int", tb => tb.HasComment("Int Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_int_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyInt.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesInt)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_int");
        });

        modelBuilder.Entity<WeeniePropertiesInt64>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_int64", tb => tb.HasComment("Int64 Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_int64_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertyInt64.????)")
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasComment("Value of this Property")
                .HasColumnName("value");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesInt64)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_int64");
        });

        modelBuilder.Entity<WeeniePropertiesPalette>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_palette", tb => tb.HasComment("Palette Changes (from PCAPs) of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.SubPaletteId, e.Offset, e.Length }, "object_Id_subPaletteId_offset_length_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Offset).HasColumnName("offset");
            entity.Property(e => e.SubPaletteId).HasColumnName("sub_Palette_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesPalette)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_palette");
        });

        modelBuilder.Entity<WeeniePropertiesPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_position", tb => tb.HasComment("Position Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.PositionType }, "wcid_position_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Position")
                .HasColumnName("id");
            entity.Property(e => e.AnglesW).HasColumnName("angles_W");
            entity.Property(e => e.AnglesX).HasColumnName("angles_X");
            entity.Property(e => e.AnglesY).HasColumnName("angles_Y");
            entity.Property(e => e.AnglesZ).HasColumnName("angles_Z");
            entity.Property(e => e.ObjCellId).HasColumnName("obj_Cell_Id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.OriginX).HasColumnName("origin_X");
            entity.Property(e => e.OriginY).HasColumnName("origin_Y");
            entity.Property(e => e.OriginZ).HasColumnName("origin_Z");
            entity.Property(e => e.PositionType)
                .HasComment("Type of Position the value applies to (PositionType.????)")
                .HasColumnName("position_Type");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesPosition)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_position");
        });

        modelBuilder.Entity<WeeniePropertiesSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_skill", tb => tb.HasComment("Skill Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_skill_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.InitLevel)
                .HasComment("starting point for advancement of the skill (eg bonus points)")
                .HasColumnName("init_Level");
            entity.Property(e => e.LastUsedTime)
                .HasComment("time skill was last used")
                .HasColumnName("last_Used_Time");
            entity.Property(e => e.LevelFromPP)
                .HasComment("points raised")
                .HasColumnName("level_From_P_P");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.PP)
                .HasComment("XP spent on this skill")
                .HasColumnName("p_p");
            entity.Property(e => e.ResistanceAtLastCheck)
                .HasComment("last use difficulty")
                .HasColumnName("resistance_At_Last_Check");
            entity.Property(e => e.SAC)
                .HasComment("skill state")
                .HasColumnName("s_a_c");
            entity.Property(e => e.Type)
                .HasComment("Type of Property the value applies to (PropertySkill.????)")
                .HasColumnName("type");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesSkill)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_skill");
        });

        modelBuilder.Entity<WeeniePropertiesSpellBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_spell_book", tb => tb.HasComment("SpellBook Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Spell }, "wcid_spellbook_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.Probability)
                .HasDefaultValueSql("'2'")
                .HasComment("Chance to cast this spell")
                .HasColumnName("probability");
            entity.Property(e => e.Spell)
                .HasComment("Id of Spell")
                .HasColumnName("spell");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesSpellBook)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_spellbook");
        });

        modelBuilder.Entity<WeeniePropertiesString>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_string", tb => tb.HasComment("String Properties of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Type }, "wcid_string_type_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
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

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesString)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_string");
        });

        modelBuilder.Entity<WeeniePropertiesTextureMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("weenie_properties_texture_map", tb => tb.HasComment("Texture Map Changes (from PCAPs) of Weenies"));

            entity.HasIndex(e => new { e.ObjectId, e.Index, e.OldId }, "object_Id_index_oldId_uidx").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Unique Id of this Property")
                .HasColumnName("id");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.NewId).HasColumnName("new_Id");
            entity.Property(e => e.ObjectId)
                .HasComment("Id of the object this property belongs to")
                .HasColumnName("object_Id");
            entity.Property(e => e.OldId).HasColumnName("old_Id");

            entity.HasOne(d => d.Object).WithMany(p => p.WeeniePropertiesTextureMap)
                .HasForeignKey(d => d.ObjectId)
                .HasConstraintName("wcid_texturemap");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
