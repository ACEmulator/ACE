using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.Auth
{
    public partial class AuthDbContext : DbContext
    {
        public AuthDbContext()
        {
        }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accesslevel> Accesslevel { get; set; }
        public virtual DbSet<Account> Account { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = Common.ConfigManager.Config.MySql.Authentication;

                optionsBuilder.UseMySql($"server={config.Host};port={config.Port};user={config.Username};password={config.Password};database={config.Database}");
            }

#if EFAUTHDEBUG
            optionsBuilder.EnableSensitiveDataLogging(true);
#endif
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
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Prefix)
                    .HasColumnName("prefix")
                    .HasColumnType("varchar(45)")
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
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BanExpireTime)
                    .HasColumnName("ban_Expire_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.BannedByAccountID).HasColumnName("banned_By_Account_I_D");

                entity.Property(e => e.BannedTime)
                    .HasColumnName("banned_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateIP)
                    .IsRequired()
                    .HasColumnName("create_I_P")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("'N/A'");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_Time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("email_Address")
                    .HasColumnType("varchar(320)")
                    .HasDefaultValueSql("'N/A'");

                entity.Property(e => e.IsBanned)
                    .HasColumnName("isBanned")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.LastLoginIP)
                    .IsRequired()
                    .HasColumnName("last_Login_I_P")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("'N/A'");

                entity.Property(e => e.LastLoginTime)
                    .HasColumnName("last_Login_Time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("passwordHash")
                    .HasColumnType("varchar(88)");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("passwordSalt")
                    .HasColumnType("varchar(88)")
                    .HasDefaultValueSql("'use bcrypt'");

                entity.HasOne(d => d.AccessLevelNavigation)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.AccessLevel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_accesslevel");
            });
        }
    }
}
