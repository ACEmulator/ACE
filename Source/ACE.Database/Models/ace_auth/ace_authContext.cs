using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.ace_auth
{
    public partial class ace_authContext : DbContext
    {
        public virtual DbSet<Accesslevel> Accesslevel { get; set; }
        public virtual DbSet<Account> Account { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = Common.ConfigManager.Config.MySql.Authentication;

            optionsBuilder.UseMySql($"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accesslevel>(entity =>
            {
                entity.HasKey(e => e.Level);

                entity.ToTable("accesslevel");

                entity.HasIndex(e => e.Level)
                    .HasName("level")
                    .IsUnique();

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.Prefix)
                    .HasColumnName("prefix")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.AccessLevel)
                    .HasName("accesslevel_idx");

                entity.HasIndex(e => e.AccountName)
                    .HasName("accountName_uidx")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.AccessLevel)
                    .HasColumnName("accessLevel")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("accountName")
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("passwordHash")
                    .HasMaxLength(88);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("passwordSalt")
                    .HasMaxLength(88);

                entity.HasOne(d => d.AccessLevelNavigation)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.AccessLevel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_accesslevel");
            });
        }
    }
}
