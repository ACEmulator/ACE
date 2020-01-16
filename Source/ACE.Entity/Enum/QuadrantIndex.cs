namespace ACE.Entity.Enum
{
    public enum QuadrantIndex
    {
        HLF = 0,
        MLF = 1,
        LLF = 2,

        HRF = 3,
        MRF = 4,
        LRF = 5,

        HLB = 6,
        MLB = 7,
        LLB = 8,

        HRB = 9,
        MRB = 10,
        LRB = 11
    };

    public static class QuadrantIndexExtensions
    {
        public const Quadrant HLF = Quadrant.High   | Quadrant.Left | Quadrant.Front;
        public const Quadrant MLF = Quadrant.Medium | Quadrant.Left | Quadrant.Front;
        public const Quadrant LLF = Quadrant.Low    | Quadrant.Left | Quadrant.Front;

        public const Quadrant HRF = Quadrant.High   | Quadrant.Right | Quadrant.Front;
        public const Quadrant MRF = Quadrant.Medium | Quadrant.Right | Quadrant.Front;
        public const Quadrant LRF = Quadrant.Low    | Quadrant.Right | Quadrant.Front;

        public const Quadrant HLB = Quadrant.High   | Quadrant.Left | Quadrant.Back;
        public const Quadrant MLB = Quadrant.Medium | Quadrant.Left | Quadrant.Back;
        public const Quadrant LLB = Quadrant.Low    | Quadrant.Left | Quadrant.Back;

        public const Quadrant HRB = Quadrant.High   | Quadrant.Right | Quadrant.Back;
        public const Quadrant MRB = Quadrant.Medium | Quadrant.Right | Quadrant.Back;
        public const Quadrant LRB = Quadrant.Low    | Quadrant.Right | Quadrant.Back;

        public static Quadrant ToQuadrant(this QuadrantIndex idx)
        {
            switch (idx)
            {
                case QuadrantIndex.HLF:
                    return HLF;
                case QuadrantIndex.MLF:
                    return MLF;
                case QuadrantIndex.LLF:
                    return LLF;

                case QuadrantIndex.HRF:
                    return HRF;
                case QuadrantIndex.MRF:
                    return MRF;
                case QuadrantIndex.LRF:
                    return LRF;

                case QuadrantIndex.HLB:
                    return HLB;
                case QuadrantIndex.MLB:
                    return MLB;
                case QuadrantIndex.LLB:
                    return LLB;

                case QuadrantIndex.HRB:
                    return HRB;
                case QuadrantIndex.MRB:
                    return MRB;
                case QuadrantIndex.LRB:
                    return LRB;

                default:
                    return Quadrant.None;
            }
        }

        public static QuadrantIndex GetIndex(this Quadrant quadrant)
        {
            switch (quadrant)
            {
                case HLF:
                    return QuadrantIndex.HLF;
                case MLF:
                    return QuadrantIndex.MLF;
                case LLF:
                    return QuadrantIndex.LLF;

                case HRF:
                    return QuadrantIndex.HRF;
                case MRF:
                    return QuadrantIndex.MRF;
                case LRF:
                    return QuadrantIndex.LRF;

                case HLB:
                    return QuadrantIndex.HLB;
                case MLB:
                    return QuadrantIndex.MLB;
                case LLB:
                    return QuadrantIndex.LLB;

                case HRB:
                    return QuadrantIndex.HRB;
                case MRB:
                    return QuadrantIndex.MRB;
                case LRB:
                    return QuadrantIndex.LRB;

                default:
                    return 0;
            }
        }
    }
}
