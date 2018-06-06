using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.Config
{
    public partial class ConfigDbContext : DbContext
    {
        public virtual DbSet<BoolStat> BoolStat { get; set; }
        public virtual DbSet<FloatStat> FloatStat { get; set; }
        public virtual DbSet<IntegerStat> IntegerStat { get; set; }
        public virtual DbSet<StringStat> StringStat { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = Common.ConfigManager.Config.MySql.Config;

                optionsBuilder.UseMySql($"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoolStat>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("bool_stat");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("tinyint(1)");
            });

            modelBuilder.Entity<FloatStat>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("float_stat");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<IntegerStat>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("integer_stat");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("int(10)");
            });

            modelBuilder.Entity<StringStat>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("string_stat");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(45);
            });
        }
    }
}
