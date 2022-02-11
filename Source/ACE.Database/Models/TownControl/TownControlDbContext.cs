using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.TownControl
{
    public partial class TownControlDbContext : DbContext
    {
        public TownControlDbContext()
        {
        }

        public TownControlDbContext(DbContextOptions<TownControlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Town> Town { get; set; }

        public virtual DbSet<TownControlEvent> TownControlEvent { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = Common.ConfigManager.Config.MySql.TownControl;

                var connectionString = $"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database};TreatTinyAsBoolean=False";

                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
                {
                    builder.EnableRetryOnFailure(10);
                });
            }

#if EFTownControlDEBUG
            optionsBuilder.EnableSensitiveDataLogging(true);
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>(entity =>
            {
                entity.HasKey(e => e.TownId)
                    .HasName("PRIMARY");

                entity.Property(e => e.TownId).HasColumnName("town_id");

                entity.ToTable("town");


                entity.Property(e => e.TownName)
                    .HasColumnName("town_name");                

                entity.Property(e => e.ConflictLength)
                    .HasColumnName("conflict_length");

                entity.Property(e => e.ConflictRespiteLength)
                    .HasColumnName("conflict_respite_length");

                entity.Property(e => e.CurrentOwnerID)
                    .HasColumnName("owner_id");

                entity.Property(e => e.IsInConflict)
                    .HasColumnName("is_in_conflict");

                entity.Property(e => e.LastConflictStartDateTime)
                    .HasColumnName("last_conflict_start_time");

                entity.Property(e => e.AttackerAwardsPerPerson)
                    .HasColumnName("attacker_awards_individual");

                entity.Property(e => e.AttackerAwardsTotal)
                    .HasColumnName("attacker_awards_total");

                entity.Property(e => e.DefenderAwardsPerPerson)
                    .HasColumnName("defender_awards_individual");

                entity.Property(e => e.DefenderAwardsTotal)
                    .HasColumnName("defender_awards_total");

            });

            modelBuilder.Entity<TownControlEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PRIMARY");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.ToTable("town_control_event");

                entity.Property(e => e.TownId)
                    .HasColumnName("town_id");

                entity.Property(e => e.EventStartDateTime)
                    .HasColumnName("event_start_time");

                entity.Property(e => e.EventEndDateTime)
                    .HasColumnName("event_end_time");

                entity.Property(e => e.AttackingClanId)
                    .HasColumnName("attacker_id");

                entity.Property(e => e.AttackingClanName)
                    .HasColumnName("attacker_clan_name");

                entity.Property(e => e.DefendingClanId)
                    .HasColumnName("defender_id");

                entity.Property(e => e.DefendingClanName)
                    .HasColumnName("defender_clan_name");

                entity.Property(e => e.IsAttackSuccess)
                    .HasColumnName("is_attack_success");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
