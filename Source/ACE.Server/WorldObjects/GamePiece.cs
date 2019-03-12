using System;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Entity.Chess;

namespace ACE.Server.WorldObjects
{
    public class GamePiece : Creature
    {
        public ChessMatch ChessMatch;

        public GamePieceState GamePieceState;
        public Position Position;
        public GamePiece TargetPiece;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public GamePiece(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public GamePiece(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            ObjScale = 0.5f;
            TimeToRot = -1;
        }

        public void Kill()
        {
            ActionChain killChain = new ActionChain();
            killChain.AddAction(this, () =>
            {
                EnqueueBroadcastMotion(new Motion(MotionStance.NonCombat, MotionCommand.Dead));
            });
            killChain.AddDelaySeconds(5);
            killChain.AddAction(this, () =>
            {
                ApplyVisualEffects(global::ACE.Entity.Enum.PlayScript.Destroy);
            });
            killChain.AddDelaySeconds(1);
            killChain.AddAction(this, () => Destroy());
            killChain.EnqueueChain();
        }

        public void MoveEnqueue(Position dest)
        {
            GamePieceState = GamePieceState.MoveToSquare;
            Position = dest;
        }

        public void AttackEnqueue(Position dest, ObjectGuid victim)
        {
            GamePieceState = GamePieceState.MoveToAttack;
            Position = dest;
            TargetPiece = CurrentLandblock.GetObject(victim) as GamePiece;
        }

        public override void Monster_Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;
            IsMonster = true;

            switch (GamePieceState)
            {
                case GamePieceState.MoveToSquare:
                    GamePieceState = GamePieceState.WaitingForMoveToSquare;
                    MoveWeenie(Position, 0.1f, true);
                    break;

                // visual awareness range of piece is only 1, make sure we are close enough to attack
                case GamePieceState.MoveToAttack:
                    MoveWeenie(Position, 0.8f, false);
                    GamePieceState = GamePieceState.WaitingForMoveToAttack;
                    break;

                case GamePieceState.WaitingForMoveToSquare:
                case GamePieceState.WaitingForMoveToAttack:
                    UpdatePosition();
                    break;

                case GamePieceState.WaitingForMoveToSquareAnimComplete:

                    /*if (PhysicsObj.IsAnimating)
                    {
                        UpdatePosition();
                        break;
                    }*/

                    ChessMatch.PieceReady(Guid);
                    GamePieceState = GamePieceState.None;
                    break;

                case GamePieceState.Combat:

                    if (CombatTable == null)
                        GetCombatTable();

                    MeleeAttack();
                    break;
            }
        }

        public override void OnMoveComplete(WeenieError status)
        {
            base.OnMoveComplete(status);

            switch (GamePieceState)
            {
                // we are done, tell the match so the turn can finish
                case GamePieceState.WaitingForMoveToSquare:
                    GamePieceState = GamePieceState.WaitingForMoveToSquareAnimComplete;
                    break;

                // there is another piece on this square, attack it!
                case GamePieceState.WaitingForMoveToAttack:
                    AttackTarget = TargetPiece;
                    GamePieceState = GamePieceState.Combat;
                    break;
            }
        }

        public void OnDealtDamage(/*DamageEvent damageData*/)
        {
            // weenie piece is dead, time to move into the square completely
            if (TargetPiece.IsDead)
                GamePieceState = GamePieceState.MoveToSquare;
        }

        public void MoveWeenie(Position to, float distanceToObject, bool finalHeading)
        {
            var moveToPosition = new Motion(this, to);
            moveToPosition.MoveToParameters.DistanceToObject = distanceToObject;
            if (finalHeading)
                moveToPosition.MoveToParameters.MovementParameters |= MovementParams.UseFinalHeading;

            var physPos = new Physics.Common.Position(to);
            moveToPosition.MoveToParameters.DesiredHeading = physPos.Frame.get_heading();

            SetWalkRunThreshold(moveToPosition, to);

            if (!InitSticky)
            {
                PhysicsObj.add_moveto_listener(OnMoveComplete);

                PhysicsObj.add_sticky_listener(OnSticky);
                PhysicsObj.add_unsticky_listener(OnUnsticky);
                InitSticky = true;
            }

            PhysicsObj.MoveToPosition(physPos, GetMovementParameters());
            IsMoving = true;
            MonsterState = State.Awake;
            IsAwake = true;

            EnqueueBroadcastMotion(moveToPosition);
        }
    }
}
