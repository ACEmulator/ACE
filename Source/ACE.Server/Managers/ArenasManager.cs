using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

using Biota = ACE.Entity.Models.Biota;

using ACE.Server.Entity.Arenas;
namespace ACE.Server.Managers
{
    public static class ArenasManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        //private static readonly List<Arena> arenas = new List<Arena>();
        public static readonly Arena onesArena = new Arena();
        public static readonly Arena threesArena = new Arena();
        public static readonly Arena fivesArena = new Arena();

        public static void PlayerDeath(Player player, Arena arena) { arena.PlayerDeath(player); }
        public static void ResetArena(Arena arena) { arena.ResetArena(true); }
        public static void RemovePlayer(Player player) { onesArena.RemovePlayer(player); threesArena.RemovePlayer(player); fivesArena.RemovePlayer(player); }
        

        public static void Initialize()
        {

            //0x33DA0025 [113.822762 103.755890 60.005001] -0.912438 0.000000 0.000000 -0.409214
            //0x00670109 [13.236547 -25.043076 0.005000] -0.697791 0.000000 0.000000 0.716301
            onesArena.Team1Position = new Position(0x00670109, (float)13.236547, (float)-25.043076, (float)0.005000, (float)-0.697791, (float)0.000000, (float)0.000000, (float)0.716301);
            //0x33DA0025 [104.038803 112.469658 60.005001] 0.343646 0.000000 0.000000 -0.939099
            // 0x00670121 [46.750183 -25.011667 0.005000] -0.718731 0.000000 0.000000 -0.695288
            onesArena.Team2Position = new Position(0x00670121, (float)46.750183, (float)-25.011667, (float)0.005000, (float)-0.718731, (float)0.000000, (float)0.000000, (float)-0.695288);
            onesArena.Landcells = new uint[] { 0x00670100, 0x00670101, 0x00670102, 0x00670103, 0x00670104, 0x00670105, 0x00670106, 0x00670107, 0x00670108, 0x00670109, 0x00670110, 0x0067011, 0x0067012, 0x0067013, 0x0067014, 0x0067015, 0x0067016, 0x0067017, 0x0067018, 0x0067019, 0x00670120, 0x00670121, 0x00670122, 0x00670123, 0x00670124, 0x00670125, 0x00670126, 0x00670127, 0x00670128, 0x00670129, 0x0067011A, 0x0067011B, 0x0067011C, 0x0067011D, 0x0067011E, 0x0067011F, 0x0067010A, 0x0067010B, 0x0067010C, 0x0067010D, 0x0067010E, 0x0067010F };
            onesArena.ArenaType = "1v1";
            onesArena.CreateTeamsAt = 2;
            onesArena.TeamSize = 1;
            //            arenas.Add(arena);

            // 0x595002C2 [140.002243 -159.116318 0.005000] 0.999970 0.000000 0.000000 0.007687
            threesArena.Team1Position = new Position(0x595002C2, (float)140.002243, (float)-159.116318, (float)0.005000, (float)0.000000, (float)0.000000, (float)0.000000, (float)0.007687);
           // 0x595002BF [139.667053 - 130.113495 0.005000] 0.019124 0.000000 0.000000 0.999817
            threesArena.Team2Position = new Position(0x595002BF, (float)139.667053, (float)-130.113495, (float)0.005000, (float)0.019124, (float)0.000000, (float)0.000000, (float)0.999817);
            threesArena.Landcells = new uint[] { 0x595002D0, 0x595002D1, 0x595002DB, 0x595002DC, 0x595002DD, 0x595002D6, 0x595002D7, 0x595002BF, 0x595002A1, 0x595002A2, 0x595002A3, 0x595002A8, 0x595002A9, 0x595002AE, 0x595002AF, 0x595002B4, 0x595002B5, 0x595002C0, 0x595002C1, 0x595002C1, 0x595002E2, 0x595002E3 };
            threesArena.ArenaType = "1v1";
            threesArena.CreateTeamsAt = 6;
            threesArena.TeamSize = 3;

            //0xF6F20024[112.634331 80.769066 0.005000] - 0.179465 0.000000 0.000000 0.983764
            fivesArena.Team1Position = new Position(0xF6F20024, (float)112.634331, (float)80.769066, (float)0.005000, (float)0.000000, (float)0.000000, (float)0.983764, (float)-0.179465);
            //0xF6F2002A [125.101768 29.044254 0.005000] -0.996179 0.000000 0.000000 -0.087335
            fivesArena.Team2Position = new Position(0xF6F2002A, (float)125.101768, (float)29.044254, (float)0.005000, (float)0.000000, (float)0.000000, (float)-0.087335, (float)-0.996179);
            fivesArena.Landcells = new uint[] { 0xF6F20022, 0xF6F20023, 0xF6F20024, 0xF6F2002A, 0xF6F20029, 0xF6F2002B, 0xF6F2002C, 0xF6F20034, 0xF6F20033, 0xF6F20025, 0xF6F2001D, 0xF6F20015, 0xF6F20014, 0xF6F2001C, 0xF6F2001B, 0xF6F2001A, 0xF6F20013, 0xF6F20012 };
            fivesArena.ArenaType = "5v5";
            fivesArena.CreateTeamsAt = 10;
            fivesArena.TeamSize = 5;
        }

        public static Arena GetArena(string str)
        {
            switch (str)
            {
                case "ones":
                    return ArenasManager.onesArena;
                case "threes":
                    return ArenasManager.threesArena;
                case "fives":
                    return ArenasManager.fivesArena;
                default:
                    return null;
            }
        }

        public static Arena WhichArenaIsPlayerIn(Player player)
        {
            if (onesArena.ContainsPlayer(player))
                return onesArena;
            if (threesArena.ContainsPlayer(player))
                return threesArena;
            if (fivesArena.ContainsPlayer(player))
                return fivesArena;

            return null;
        }

        public static void Tick()
        {
            //log.Info("ArenasManager.Tick()...");
            TickArena(onesArena);
            TickArena(threesArena);
            TickArena(fivesArena);
        }

        public static void TickArena(Arena arena)
        {
            if (!arena.Occupied && arena.PlayerQueue.Count >= arena.CreateTeamsAt)
                arena.Init();

            //if (!arena.Occupied || arena.PlayerQueue.Count <= 1)
            //    return;

            if (arena.Occupied)
                arena.Tick();
        }

        public static void ClearQueue(string str)
        {
            switch (str)
            {
                case "ones":
                    onesArena.PlayerQueue = new List<Player>();
                    break;
                case "threes":
                    threesArena.PlayerQueue = new List<Player>();
                    break;
                case "fives":
                    fivesArena.PlayerQueue = new List<Player>();
                    break;
                default:
                    break;
            }
        }
    }
}
