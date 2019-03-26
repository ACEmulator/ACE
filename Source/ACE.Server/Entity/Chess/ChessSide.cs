using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Chess
{
    public class ChessSide
    {
        public ObjectGuid PlayerGuid;
        public ChessColor Color;
        public bool Stalemate;

        public ChessSide(ObjectGuid playerGuid, ChessColor color)
        {
            PlayerGuid = playerGuid;
            Color = color;
        }

        public bool IsAi()
        {
            return !PlayerGuid.IsPlayer();
        }

        public Player GetPlayer()
        {
            return PlayerManager.GetOnlinePlayer(PlayerGuid);
        }
    }
}
