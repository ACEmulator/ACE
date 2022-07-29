using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

            modelBuilder.Entity<Accesslevel>(entity =>
            {
                entity.HasKey(e => e.Level)
                    .HasName("PRIMARY");

                entity.ToTable("accesslevel");

                entity.HasIndex(e => e.Level, "level")
                    .IsUnique();

                entity.Property(e => e.Level)
                    .ValueGeneratedNever()
                    .HasColumnName("level");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Prefix)
                    .HasMaxLength(45)
                    .HasColumnName("prefix")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.AccessLevel, "accesslevel_idx");

                entity.HasIndex(e => e.AccountName, "accountName_uidx")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.AccessLevel).HasColumnName("accessLevel");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("accountName");

                entity.Property(e => e.BanExpireTime)
                    .HasColumnType("datetime")
                    .HasColumnName("ban_Expire_Time");

                entity.Property(e => e.BanReason)
                    .HasMaxLength(1000)
                    .HasColumnName("ban_Reason");

                entity.Property(e => e.BannedByAccountId).HasColumnName("banned_By_Account_Id");

                entity.Property(e => e.BannedTime)
                    .HasColumnType("datetime")
                    .HasColumnName("banned_Time");

                entity.Property(e => e.CreateIP)
                    .HasMaxLength(16)
                    .HasColumnName("create_I_P");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_Time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(320)
                    .HasColumnName("email_Address");

                entity.Property(e => e.LastLoginIP)
                    .HasMaxLength(16)
                    .HasColumnName("last_Login_I_P");

                entity.Property(e => e.LastLoginTime)
                    .HasColumnType("datetime")
                    .HasColumnName("last_Login_Time");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(88)
                    .HasColumnName("passwordHash")
                    .HasComment("base64 encoded version of the hashed passwords.  88 characters are needed to base64 encode SHA512 output.");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(88)
                    .HasColumnName("passwordSalt")
                    .HasDefaultValueSql("'use bcrypt'")
                    .HasComment("This is no longer used, except to indicate if bcrypt is being employed for migration purposes. Previously: base64 encoded version of the password salt.  512 byte salts (88 characters when base64 encoded) are recommend for SHA512.");

                entity.Property(e => e.TotalTimesLoggedIn).HasColumnName("total_Times_Logged_In");

                entity.HasOne(d => d.AccessLevelNavigation)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.AccessLevel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_accesslevel");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
