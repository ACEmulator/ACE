using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACE.Database.Models.Log
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

        public virtual DbSet<ArenaEvent> ArenaEvents { get; set; }

        public virtual DbSet<ArenaPlayer> ArenaPlayers { get; set; }

        public virtual DbSet<ArenaCharacterStats> ArenaCharacterStats { get; set; }


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

                entity.Property(e => e.VictimArenaPlayerID)
                    .HasColumnName("victim_arena_player_id");

                entity.Property(e => e.KillerArenaPlayerID)
                    .HasColumnName("killer_arena_player_id");
            });

            modelBuilder.Entity<ArenaEvent>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.ToTable("arena_event");

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type");

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.Property(e => e.Location)
                    .HasColumnName("location");

                entity.Property(e => e.StartDateTime)
                    .HasColumnName("start_datetime");

                entity.Property(e => e.EndDateTime)
                    .HasColumnName("end_datetime");

                entity.Property(e => e.WinningTeamGuid)
                    .HasColumnName("winning_team_guid");

                entity.Property(e => e.CancelReason)
                    .HasColumnName("cancel_reason");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnName("create_datetime");
            });

            modelBuilder.Entity<ArenaPlayer>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.ToTable("arena_player");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_id");

                entity.Property(e => e.CharacterName)
                    .HasColumnName("character_name");

                entity.Property(e => e.CharacterLevel)
                    .HasColumnName("character_level");

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type");

                entity.Property(e => e.MonarchId)
                    .HasColumnName("monarch_id");

                entity.Property(e => e.MonarchName)
                    .HasColumnName("monarch_name");                

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id");

                entity.Property(e => e.TeamGuid)
                    .HasColumnName("team_guid");

                entity.Property(e => e.PlayerIP)
                    .HasColumnName("player_ip");

                entity.Property(e => e.IsEliminated)
                    .HasColumnName("is_eliminated");

                entity.Property(e => e.FinishPlace)
                    .HasColumnName("finish_place");

                entity.Property(e => e.TotalDeaths)
                    .HasColumnName("total_deaths");

                entity.Property(e => e.TotalKills)
                    .HasColumnName("total_kills");

                entity.Property(e => e.TotalDmgDealt)
                    .HasColumnName("total_dmg_dealt");

                entity.Property(e => e.TotalDmgReceived)
                    .HasColumnName("total_dmg_received");

                entity.Property(e => e.CreateDateTime)
                    .HasColumnName("create_datetime");

            });

            modelBuilder.Entity<ArenaCharacterStats>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.ToTable("arena_character_stats");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("character_id");

                entity.Property(e => e.CharacterName)
                    .HasColumnName("character_name");

                entity.Property(e => e.RankPoints)
                    .HasColumnName("rank_points");

                entity.Property(e => e.TotalMatches)
                    .HasColumnName("total_matches");

                entity.Property(e => e.TotalWins)
                    .HasColumnName("total_wins");

                entity.Property(e => e.TotalLosses)
                    .HasColumnName("total_losses");

                entity.Property(e => e.TotalDraws)
                    .HasColumnName("total_draws");

                entity.Property(e => e.TotalDisqualified)
                    .HasColumnName("total_disqualified");

                entity.Property(e => e.TotalDeaths)
                    .HasColumnName("total_deaths");

                entity.Property(e => e.TotalKills)
                    .HasColumnName("total_kills");

                entity.Property(e => e.TotalDmgDealt)
                    .HasColumnName("total_dmg_dealt");

                entity.Property(e => e.TotalDmgReceived)
                    .HasColumnName("total_dmg_received");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
