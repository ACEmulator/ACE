namespace ACE.Entity.Enum
{
    public enum GamePieceState
    {
        None,
        MoveToSquare,
        WaitingForMoveToSquare,
        WaitingForMoveToSquareAnimComplete,
        MoveToAttack,
        WaitingForMoveToAttack,
        Combat
    }
}
