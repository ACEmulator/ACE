using System;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Entity.WorldObjects
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

            Stuck = true;
            Attackable = true;

            // Setup game variables...
            // TODO: so much more than what's here now.
        }

        public override void ActOnUse(ObjectGuid playerId)
        {
            ActOnJoin(playerId);
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
            throw new NotImplementedException();/*
            var drudgeRook1 = WorldObjectFactory.CreateNewWorldObject(14343) as GamePiece;
            drudgeRook1.Location = new Position(Location.Cell, Location.PositionX - 3.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgeRook1.EnterWorld();

            var drudgeKnight1 = WorldObjectFactory.CreateNewWorldObject(14344) as GamePiece;
            drudgeKnight1.Location = new Position(Location.Cell, Location.PositionX - 2.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgeKnight1.EnterWorld();

            var drudgeBishop1 = WorldObjectFactory.CreateNewWorldObject(14345) as GamePiece;
            drudgeBishop1.Location = new Position(Location.Cell, Location.PositionX - 1.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgeBishop1.EnterWorld();

            var drudgeQueen = WorldObjectFactory.CreateNewWorldObject(14346) as GamePiece;            
            drudgeQueen.Location = new Position(Location.Cell, Location.PositionX - 0.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09735f, 0, 0, 0, 1);
            drudgeQueen.EnterWorld();

            var drudgeKing = WorldObjectFactory.CreateNewWorldObject(14347) as GamePiece;
            drudgeKing.Location = new Position(Location.Cell, Location.PositionX + 0.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09718f, 0, 0, 0, 1);
            drudgeKing.EnterWorld();

            var drudgeBishop2 = WorldObjectFactory.CreateNewWorldObject(14345) as GamePiece;
            drudgeBishop2.Location = new Position(Location.Cell, Location.PositionX + 1.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgeBishop2.EnterWorld();

            var drudgeKnight2 = WorldObjectFactory.CreateNewWorldObject(14344) as GamePiece;
            drudgeKnight2.Location = new Position(Location.Cell, Location.PositionX + 2.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgeKnight2.EnterWorld();

            var drudgeRook2 = WorldObjectFactory.CreateNewWorldObject(14343) as GamePiece;
            drudgeRook2.Location = new Position(Location.Cell, Location.PositionX + 3.5f, Location.PositionY - 3.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgeRook2.EnterWorld();

            var drudgePawn1 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn1.Location = new Position(Location.Cell, Location.PositionX - 3.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgePawn1.EnterWorld();

            var drudgePawn2 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn2.Location = new Position(Location.Cell, Location.PositionX - 2.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgePawn2.EnterWorld();

            var drudgePawn3 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn3.Location = new Position(Location.Cell, Location.PositionX - 1.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgePawn3.EnterWorld();

            var drudgePawn4 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn4.Location = new Position(Location.Cell, Location.PositionX - 0.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09735f, 0, 0, 0, 1);
            drudgePawn4.EnterWorld();

            var drudgePawn5 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn5.Location = new Position(Location.Cell, Location.PositionX + 0.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09718f, 0, 0, 0, 1);
            drudgePawn5.EnterWorld();

            var drudgePawn6 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn6.Location = new Position(Location.Cell, Location.PositionX + 1.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09753f, 0, 0, 0, 1);
            drudgePawn6.EnterWorld();

            var drudgePawn7 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn7.Location = new Position(Location.Cell, Location.PositionX + 2.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09771f, 0, 0, 0, 1);
            drudgePawn7.EnterWorld();

            var drudgePawn8 = WorldObjectFactory.CreateNewWorldObject(14342) as GamePiece;
            drudgePawn8.Location = new Position(Location.Cell, Location.PositionX + 3.5f, Location.PositionY - 2.5f, Location.PositionZ - 0.09788f, 0, 0, 0, 1);
            drudgePawn8.EnterWorld();

            // Nobody ever actually started a game so the database is currently missing the mosswart versions of the above pieces. :(

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
            */
        }
    }
}
