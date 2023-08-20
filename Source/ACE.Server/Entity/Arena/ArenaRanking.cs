using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Log;
using ACE.Database.Models.TownControl;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Managers;
using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace ACE.Server.Entity
{
    public static class ArenaRanking
    {
        public static float GetProbabilityWinning(float ratingPlayer1, float ratingPlayer2)
        {
            return 1f / (1f + MathF.Pow(10f, (ratingPlayer2 - ratingPlayer1) / 400f));
        }

        public static int GetRankChange(uint winnerCurrentRank, uint loserCurrentRank, int multiplier)
        {
            float probabilityWinPlayer1 = GetProbabilityWinning(winnerCurrentRank, loserCurrentRank);

            return Convert.ToInt32(Math.Round(multiplier * (1 - probabilityWinPlayer1)));
        }
    }
}
