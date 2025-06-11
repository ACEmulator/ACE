using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using Microsoft.Extensions.Hosting;

#nullable enable

using ACE.Common;
using ACE.Server.Managers;
using System.Collections.Concurrent;
using ACE.Common.Performance;
using ACE.Server;
using ACE.Server.Entity;
using log4net;
using System.IO;

namespace ACE.Server.Api
{
    public static class StatusApiServer
    {
        private static WebApplication? _app;
        private static CancellationTokenSource? _cts;
        private sealed class LimiterEntry
        {
            public RateLimiter Limiter { get; }
            public DateTime LastAccess { get; set; }

            public LimiterEntry(RateLimiter limiter)
            {
                Limiter = limiter;
                LastAccess = DateTime.UtcNow;
            }
        }

        private static readonly ConcurrentDictionary<IPAddress, LimiterEntry> _rateLimiters = new();
        private static readonly TimeSpan LimiterExpiration = TimeSpan.FromMinutes(10);
        private static DateTime _lastCleanup = DateTime.UtcNow;

        private static readonly ConcurrentDictionary<string, (IResult Value, DateTime Expire)> _cache = new();

        private static FileSystemWatcher? _watcher;
        private static readonly ILog log = LogManager.GetLogger(typeof(StatusApiServer));

        private static IResult GetCached(string key, Func<IResult> factory)
        {
            var ttl = ConfigManager.Config.Server.Api.CacheSeconds;
            if (ttl == 0)
                return factory();

            CleanupCache();

            if (_cache.TryGetValue(key, out var entry) && entry.Expire > DateTime.UtcNow)
                return entry.Value;

            var value = factory();
            _cache[key] = (value, DateTime.UtcNow.AddSeconds(ttl));
            return value;
        }

        private static void CleanupCache()
        {
            var now = DateTime.UtcNow;
            foreach (var kvp in _cache)
            {
                if (kvp.Value.Expire <= now)
                    _cache.TryRemove(kvp.Key, out _);
            }
        }

        public static void Start()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Parse(ConfigManager.Config.Server.Api.Host), (int)ConfigManager.Config.Server.Api.Port, listenOptions =>
                {
                    if (ConfigManager.Config.Server.Api.UseHttps)
                    {
                        listenOptions.UseHttps(ConfigManager.Config.Server.Api.CertificatePath,
                                              ConfigManager.Config.Server.Api.CertificatePassword);
                    }
                });
            });


            _app = builder.Build();

            _app.MapGet("/", () => GetCached("/", () => Results.Json(new[]
            {
                "/api/status",
                "/api/stats/players",
                "/api/stats/character/{name}",
                "/api/stats/performance"
            }));

            if (ConfigManager.Config.Server.Api.RequireApiKey && !string.IsNullOrEmpty(ConfigManager.ConfigPath))
            {
                var dir = Path.GetDirectoryName(ConfigManager.ConfigPath);
                var file = Path.GetFileName(ConfigManager.ConfigPath);
                if (dir != null && file != null)
                {
                    _watcher = new FileSystemWatcher(dir, file)
                    {
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size
                    };
                    _watcher.Changed += (_, __) => ConfigManager.ReloadApiKeys(log);
                    _watcher.Created += (_, __) => ConfigManager.ReloadApiKeys(log);
                    _watcher.EnableRaisingEvents = true;
                }
            }

            _app.Use(async (context, next) =>
            {
                if (ConfigManager.Config.Server.Api.RequireApiKey)
                {
                    var provided = context.Request.Headers["X-API-Key"].FirstOrDefault()
                        ?? context.Request.Query["apikey"].FirstOrDefault();
                    if (string.IsNullOrEmpty(provided) ||
                        !ConfigManager.Config.Server.Api.ApiKeys.Contains(provided))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Missing or invalid API key");
                        return;
                    }
                }

                var ip = context.Connection.RemoteIpAddress;
                var limit = ConfigManager.Config.Server.Api.RequestsPerMinute;
                if (limit > 0 && ip != null)
                {
                    var entry = _rateLimiters.GetOrAdd(ip, _ => new LimiterEntry(new RateLimiter((int)limit, TimeSpan.FromMinutes(1))));
                    entry.LastAccess = DateTime.UtcNow;

                    if (entry.Limiter.GetSecondsToWaitBeforeNextEvent() > 0)
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too Many Requests");
                        CleanupLimiters();
                        return;
                    }

                    entry.Limiter.RegisterEvent();
                    CleanupLimiters();
                }

                await next();
            });

            _app.MapGet("/api/stats/players", () => GetCached("/api/stats/players", () =>
            {
                var result = new
                {
                    onlineCount = PlayerManager.GetOnlineCount(),
                    players = PlayerManager.GetAllOnline().Select(p => p.Name)
                };

                return Results.Json(result);
            }));

            _app.MapGet("/api/stats/character/{name}", (string name) => GetCached($"/api/stats/character/{name}", () =>
            {
                var player = PlayerManager.FindByName(name, out _);
                if (player == null)
                    return Results.NotFound();

                var gender = player.Gender.HasValue ?
                    Enum.GetName(typeof(Gender), player.Gender.Value) : null;

                var result = new
                {
                    allegiance_name = player.GetProperty(PropertyString.AllegianceName),
                    birth = player.GetProperty(PropertyString.DateOfBirth),
                    created_at = player.Account?.CreateTime,
                    deaths = player.GetProperty(PropertyInt.NumDeaths) ?? 0,
                    followers = player.GetProperty(PropertyInt.AllegianceFollowers) ?? 0,
                    gender = gender,
                    level = player.Level ?? 0,
                    luminance_total = player.GetProperty(PropertyInt64.MaximumLuminance) ?? 0,
                    luminance_earned = player.GetProperty(PropertyInt64.AvailableLuminance) ?? 0
                };

                return Results.Json(result);
            }));

            _app.MapGet("/api/stats/performance", () => GetCached("/api/stats/performance", () =>
            {
                var proc = Process.GetCurrentProcess();
                var uptimeSeconds = Timers.RunningTime;
                var cpuUsagePercent = uptimeSeconds > 0
                    ? proc.TotalProcessorTime.TotalSeconds * 100.0 / (uptimeSeconds * Environment.ProcessorCount)
                    : 0;
                var privateMemoryMB = proc.PrivateMemorySize64 / 1024.0 / 1024.0;
                var gcMemoryMB = GC.GetTotalMemory(false) / 1024.0 / 1024.0;

                string? monitor = null;
                if (ServerPerformanceMonitor.IsRunning)
                    monitor = ServerPerformanceMonitor.ToString();

                var result = new
                {
                    uptimeSeconds,
                    cpuUsagePercent,
                    privateMemoryMB,
                    gcMemoryMB,
                    monitor
                };

                return Results.Json(result);
            }));

            _app.MapGet("/api/status", () => GetCached("/api/status", () =>
            {
                var uptimeSeconds = Timers.RunningTime;
                var version = ServerBuildInfo.FullVersion;
                var startTime = Timers.WorldStartTime;
                return Results.Json(new { uptimeSeconds, version, startTime });
            }));

            _cts = new CancellationTokenSource();
            Task.Run(() => _app.RunAsync(_cts.Token));
        }

        public static async Task StopAsync()
        {
            if (_app != null && _cts != null)
            {
                _cts.Cancel();
                await _app.StopAsync();
                _watcher?.Dispose();
                _watcher = null;
                _cache.Clear();
                _rateLimiters.Clear();
            }
        }

        private static void CleanupLimiters()
        {
            var now = DateTime.UtcNow;
            if (now - _lastCleanup < LimiterExpiration)
                return;

            _lastCleanup = now;

            foreach (var kvp in _rateLimiters)
            {
                if (now - kvp.Value.LastAccess > LimiterExpiration)
                    _rateLimiters.TryRemove(kvp.Key, out _);
            }
        }
    }
}
