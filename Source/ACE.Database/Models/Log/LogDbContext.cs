using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.EventLog
{
    public partial class LogDbContext : DbContext
    {
        public LogDbContext()
        {
        }

        public LogDbContext(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PKKill> PKKills { get; set; }

        public virtual DbSet<AccountSessionLog> AccountSessions { get; set; }

        public virtual DbSet<CharacterLoginLog> CharacterLogins { get; set; }

        public virtual DbSet<TinkerLog> TinkeringEvents { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = Common.ConfigManager.Config.MySql.Log;

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

            modelBuilder.Entity<TinkerLog>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("tinkerLogId");

                entity.ToTable("tinker_log");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("characterId");

                entity.Property(e => e.CharacterName)
                    .HasColumnName("characterName");

                entity.Property(e => e.ItemBiotaId)
                    .HasColumnName("itemBiotaId");

                entity.Property(e => e.TinkDateTime)
                    .HasColumnName("tinkDateTime");

                entity.Property(e => e.SuccessChance)
                    .HasColumnName("successChance");

                entity.Property(e => e.Roll)
                    .HasColumnName("roll");

                entity.Property(e => e.IsSuccess)
                    .HasColumnName("isSuccess");

                entity.Property(e => e.ItemNumPreviousTinks)
                    .HasColumnName("itemNumPreviousTinks");

                entity.Property(e => e.ItemWorkmanship)
                    .HasColumnName("itemWorkmanship");

                entity.Property(e => e.SalvageType)
                    .HasColumnName("salvageType");

                entity.Property(e => e.SalvageWorkmanship)
                    .HasColumnName("salvageWorkmanship");
            });

            modelBuilder.Entity<AccountSessionLog>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("sessionLogId");

                entity.ToTable("account_session_log");

                entity.Property(e => e.AccountId)
                    .HasColumnName("accountId");

                entity.Property(e => e.AccountName)
                    .HasColumnName("accountName");

                entity.Property(e => e.SessionIP)
                    .HasColumnName("sessionIP");

                entity.Property(e => e.LoginDateTime)
                    .HasColumnName("loginDateTime");

                entity.Property(e => e.LogoutDateTime)
                    .HasColumnName("logoutDateTime");
            });

            modelBuilder.Entity<CharacterLoginLog>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("characterLoginLogId");

                entity.ToTable("character_login_log");

                entity.Property(e => e.AccountId)
                    .HasColumnName("accountId");

                entity.Property(e => e.AccountName)
                    .HasColumnName("accountName");

                entity.Property(e => e.SessionIP)
                    .HasColumnName("sessionIP");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("characterId");

                entity.Property(e => e.CharacterName)
                    .HasColumnName("characterName");

                entity.Property(e => e.LoginDateTime)
                    .HasColumnName("loginDateTime");

                entity.Property(e => e.LogoutDateTime)
                    .HasColumnName("logoutDateTime");
            });

            modelBuilder.Entity<PKKill>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.ToTable("pk_kills_log");

                entity.Property(e => e.VictimId)
                    .HasColumnName("victim_id");

                entity.Property(e => e.KillerId)
                    .HasColumnName("killer_id");

                entity.Property(e => e.VictimMonarchId)
                    .HasColumnName("victim_monarch_id");

                entity.Property(e => e.KillerMonarchId)
                    .HasColumnName("killer_monarch_id");

                entity.Property(e => e.KillDateTime)
                    .HasColumnName("kill_datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
