namespace ACE.Entity.Enum
{
    public enum ChessAiState
    {
        None,
        WaitingToStart,     // work has been requested, future object will be initialised and made valid next update
        WaitingForWorker,   // future object is valid but has yet to start a worker thread
        InProgress,         // worker is currently calculating ai move
        WaitingForFinish    // worker has finished calculating ai move, future get will not block or block for a short time
    }
}
