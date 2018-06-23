namespace ACE.Server.Entity
{
    /// <summary>
    /// not sure if this is going to be used.  i thought it was, but now i'm not sure
    /// </summary>
    public enum Adjacency
    {
        NorthWest       = 0,
        North           = 1,
        NorthEast       = 2,
        East            = 3,
        SouthEast       = 4,
        South           = 5,
        SouthWest       = 6,
        West            = 7
    }

    public static class AdjacencyHelper
    {
        public static Adjacency? GetInverse(Adjacency adj)
        {
            switch (adj)
            {
                case Adjacency.NorthWest:
                    return Adjacency.SouthEast;
                case Adjacency.North:
                    return Adjacency.South;
                case Adjacency.NorthEast:
                    return Adjacency.SouthWest;
                case Adjacency.East:
                    return Adjacency.West;
                case Adjacency.SouthEast:
                    return Adjacency.NorthWest;
                case Adjacency.South:
                    return Adjacency.North;
                case Adjacency.SouthWest:
                    return Adjacency.NorthEast;
                case Adjacency.West:
                    return Adjacency.East;
            }
            return null;
        }
    }
}
