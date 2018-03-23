using System;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    public class Game : WorldObject
    {
        private bool active;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Game(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Game(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // TODO we shouldn't be auto setting properties that come from our weenie by default

            // Setup game variables...
            // TODO: so much more than what's here now.
        }

        public override void ActOnUse(Player player)
        {
            ActOnJoin(player.Guid);
        }

        public void ActOnJoin(ObjectGuid playerId)
        {
            if (active)
                return;

            active = true;
            Player player = CurrentLandblock.GetObject(playerId) as Player;

            // team is either 0 or 1. -1 means failed to join
            var msgJoinResponse = new GameEventJoinGameResponse(player.Session, Guid.Full, 1);

            // 0 or 1 for winning team. -1 is used for stalemate, -2 (and gameId of 0) is used to exit game mode in client
            // var msgGameOver = new GameEventGameOver(player.Session, 0, -2);

            // player.Session.Network.EnqueueSend(msgJoinResponse, msgGameOver);
            player.Session.Network.EnqueueSend(msgJoinResponse);

            // 0xA9B2002E [135.97 133.313 94.4447] 1 0 0 0 (holtburg game location)

            // Drudges

            var drudgeRook1 = WorldObjectFactory.CreateNewWorldObject("drudgerook") as GamePiece;
            drudgeRook1.Location = new Position(Location.Cell, Location.PositionX - 3.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgeRook1.EnterWorld();

            var drudgeKnight1 = WorldObjectFactory.CreateNewWorldObject("drudgeknight") as GamePiece;
            drudgeKnight1.Location = new Position(Location.Cell, Location.PositionX - 2.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgeKnight1.EnterWorld();

            var drudgeBishop1 = WorldObjectFactory.CreateNewWorldObject("drudgebishop") as GamePiece;
            drudgeBishop1.Location = new Position(Location.Cell, Location.PositionX - 1.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgeBishop1.EnterWorld();

            var drudgeQueen = WorldObjectFactory.CreateNewWorldObject("drudgequeen") as GamePiece;            
            drudgeQueen.Location = new Position(Location.Cell, Location.PositionX - 0.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09735f, 0, 0, 0, 1);
            drudgeQueen.EnterWorld();

            var drudgeKing = WorldObjectFactory.CreateNewWorldObject("drudgeking") as GamePiece;
            drudgeKing.Location = new Position(Location.Cell, Location.PositionX + 0.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09718f, 0, 0, 0, 1);
            drudgeKing.EnterWorld();

            var drudgeBishop2 = WorldObjectFactory.CreateNewWorldObject("drudgebishop") as GamePiece;
            drudgeBishop2.Location = new Position(Location.Cell, Location.PositionX + 1.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgeBishop2.EnterWorld();

            var drudgeKnight2 = WorldObjectFactory.CreateNewWorldObject("drudgeknight") as GamePiece;
            drudgeKnight2.Location = new Position(Location.Cell, Location.PositionX + 2.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgeKnight2.EnterWorld();

            var drudgeRook2 = WorldObjectFactory.CreateNewWorldObject("drudgerook") as GamePiece;
            drudgeRook2.Location = new Position(Location.Cell, Location.PositionX + 3.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgeRook2.EnterWorld();

            var drudgePawn1 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn1.Location = new Position(Location.Cell, Location.PositionX - 3.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgePawn1.EnterWorld();

            var drudgePawn2 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn2.Location = new Position(Location.Cell, Location.PositionX - 2.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgePawn2.EnterWorld();

            var drudgePawn3 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn3.Location = new Position(Location.Cell, Location.PositionX - 1.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgePawn3.EnterWorld();

            var drudgePawn4 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn4.Location = new Position(Location.Cell, Location.PositionX - 0.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09735f, 0, 0, 0, 1);
            drudgePawn4.EnterWorld();

            var drudgePawn5 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn5.Location = new Position(Location.Cell, Location.PositionX + 0.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09718f, 0, 0, 0, 1);
            drudgePawn5.EnterWorld();

            var drudgePawn6 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn6.Location = new Position(Location.Cell, Location.PositionX + 1.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgePawn6.EnterWorld();

            var drudgePawn7 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn7.Location = new Position(Location.Cell, Location.PositionX + 2.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgePawn7.EnterWorld();

            var drudgePawn8 = WorldObjectFactory.CreateNewWorldObject("drudgepawn") as GamePiece;
            drudgePawn8.Location = new Position(Location.Cell, Location.PositionX + 3.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgePawn8.EnterWorld();

            // Mosswarts

            var mosswartRook1 = WorldObjectFactory.CreateNewWorldObject("mosswartrook") as GamePiece;
            mosswartRook1.Location = new Position(Location.Cell, Location.PositionX - 3.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09788f, 0, 0, 1, 0);
            mosswartRook1.EnterWorld();

            var mosswartKnight1 = WorldObjectFactory.CreateNewWorldObject("mosswartknight") as GamePiece;
            mosswartKnight1.Location = new Position(Location.Cell, Location.PositionX - 2.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09771f, 0, 0, 1, 0);
            mosswartKnight1.EnterWorld();

            var mosswartBishop1 = WorldObjectFactory.CreateNewWorldObject("mosswartbishop") as GamePiece;
            mosswartBishop1.Location = new Position(Location.Cell, Location.PositionX - 1.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09753f, 0, 0, 1, 0);
            mosswartBishop1.EnterWorld();

            var mosswartQueen = WorldObjectFactory.CreateNewWorldObject("mosswartqueen") as GamePiece;
            mosswartQueen.Location = new Position(Location.Cell, Location.PositionX - 0.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09735f, 0, 0, 1, 0);
            mosswartQueen.EnterWorld();

            var mosswartKing = WorldObjectFactory.CreateNewWorldObject("mosswartking") as GamePiece;
            mosswartKing.Location = new Position(Location.Cell, Location.PositionX + 0.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09718f, 0, 0, 1, 0);
            mosswartKing.EnterWorld();

            var mosswartBishop2 = WorldObjectFactory.CreateNewWorldObject("mosswartbishop") as GamePiece;
            mosswartBishop2.Location = new Position(Location.Cell, Location.PositionX + 1.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09753f, 0, 0, 1, 0);
            mosswartBishop2.EnterWorld();

            var mosswartKnight2 = WorldObjectFactory.CreateNewWorldObject("mosswartknight") as GamePiece;
            mosswartKnight2.Location = new Position(Location.Cell, Location.PositionX + 2.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09771f, 0, 0, 1, 0);
            mosswartKnight2.EnterWorld();

            var mosswartRook2 = WorldObjectFactory.CreateNewWorldObject("mosswartrook") as GamePiece;
            mosswartRook2.Location = new Position(Location.Cell, Location.PositionX + 3.5f, Location.PositionY + 3.5f, Location.PositionZ - 0.09788f, 0, 0, 1, 0);
            mosswartRook2.EnterWorld();

            var mosswartPawn1 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn1.Location = new Position(Location.Cell, Location.PositionX - 3.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09788f, 0, 0, 1, 0);
            mosswartPawn1.EnterWorld();

            var mosswartPawn2 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn2.Location = new Position(Location.Cell, Location.PositionX - 2.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09771f, 0, 0, 1, 0);
            mosswartPawn2.EnterWorld();

            var mosswartPawn3 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn3.Location = new Position(Location.Cell, Location.PositionX - 1.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09753f, 0, 0, 1, 0);
            mosswartPawn3.EnterWorld();

            var mosswartPawn4 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn4.Location = new Position(Location.Cell, Location.PositionX - 0.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09735f, 0, 0, 1, 0);
            mosswartPawn4.EnterWorld();

            var mosswartPawn5 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn5.Location = new Position(Location.Cell, Location.PositionX + 0.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09718f, 0, 0, 1, 0);
            mosswartPawn5.EnterWorld();

            var mosswartPawn6 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn6.Location = new Position(Location.Cell, Location.PositionX + 1.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09753f, 0, 0, 1, 0);
            mosswartPawn6.EnterWorld();

            var mosswartPawn7 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn7.Location = new Position(Location.Cell, Location.PositionX + 2.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09771f, 0, 0, 1, 0);
            mosswartPawn7.EnterWorld();

            var mosswartPawn8 = WorldObjectFactory.CreateNewWorldObject("mosswartpawn") as GamePiece;
            mosswartPawn8.Location = new Position(Location.Cell, Location.PositionX + 3.5f, Location.PositionY + 2.5f, Location.PositionZ - 0.09788f, 0, 0, 1, 0);
            mosswartPawn8.EnterWorld();

            // For HellsWrath...
            ActionChain gdlChain = new ActionChain();
            gdlChain.AddDelaySeconds(5);
            gdlChain.AddAction(this, () =>
            {
                drudgeRook1.Kill();
                drudgeBishop1.Kill();
                drudgeKnight1.Kill();
                drudgeQueen.Kill();
                drudgeKing.Kill();
                drudgeBishop2.Kill();
                drudgeKnight2.Kill();
                drudgeRook2.Kill();

                drudgePawn1.Kill();
                drudgePawn2.Kill();
                drudgePawn3.Kill();
                drudgePawn4.Kill();
                drudgePawn5.Kill();
                drudgePawn6.Kill();
                drudgePawn7.Kill();
                drudgePawn8.Kill();

                mosswartRook1.Kill();
                mosswartBishop1.Kill();
                mosswartKnight1.Kill();
                mosswartQueen.Kill();
                mosswartKing.Kill();
                mosswartBishop2.Kill();
                mosswartKnight2.Kill();
                mosswartRook2.Kill();

                mosswartPawn1.Kill();
                mosswartPawn2.Kill();
                mosswartPawn3.Kill();
                mosswartPawn4.Kill();
                mosswartPawn5.Kill();
                mosswartPawn6.Kill();
                mosswartPawn7.Kill();
                mosswartPawn8.Kill();

                var msgGameOver = new GameEventGameOver(player.Session, Guid.Full, 0);
                player.Session.Network.EnqueueSend(msgGameOver);
            });
            gdlChain.AddDelaySeconds(2);
            gdlChain.AddAction(this, () =>
            {
                byte[] msg = Convert.FromBase64String("Z2FtZXNkZWFkbG9s");
                var popupGDL = new GameEventPopupString(player.Session, System.Text.Encoding.UTF8.GetString(msg, 0, msg.Length));
                var msgGameOver2 = new GameEventGameOver(player.Session, 0, -2);
                player.Session.Network.EnqueueSend(popupGDL, msgGameOver2);
                player.ChessGamesLost++;
                player.ChessTotalGames++;               
                active = false;
            });
            gdlChain.EnqueueChain();
        }
    }
}
