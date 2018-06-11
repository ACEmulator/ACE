using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.Config
{
    public partial class ConfigDbContext : DbContext
    {
        public virtual DbSet<PropertiesBoolean> PropertiesBoolean { get; set; }
        public virtual DbSet<PropertiesDouble> PropertiesDouble { get; set; }
        public virtual DbSet<PropertiesLong> PropertiesLong { get; set; }
        public virtual DbSet<PropertiesString> PropertiesString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = Common.ConfigManager.Config.MySql.Config;

            optionsBuilder.UseMySql($"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertiesBoolean>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("properties_boolean");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("bit(1)");
            });

            modelBuilder.Entity<PropertiesDouble>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("properties_double");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<PropertiesLong>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("properties_long");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasMaxLength(45);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("bigint(20)");
            });

            modelBuilder.Entity<PropertiesString>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("properties_string");

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
