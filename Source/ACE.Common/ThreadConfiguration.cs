using System;
using System.Threading.Tasks;

namespace ACE.Common
{
    /// <summary>
    /// We determine the number of available threads using Environment.ProcessCount
    /// We allocate(int)Math.Max(Environment.ProcessorCount * multiplier, 1) to the World, and the remainder to the database.
    /// </summary>
    public static class ThreadConfiguration
    {
        /*
         * Multiplier of 0.34:
         * 1 vCPU = 1 thread world, 1 thread database
         * 2 vCPU = 1 thread world, 1 thread database
         * 3 vCPU = 1 thread world, 2 thread database
         * 4 vCPU = 1 thread world, 3 thread database
         * 5 vCPU = 1 thread world, 4 thread database
         * 6 vCPU = 2 thread world, 4 thread database
         * 7 vCPU = 2 thread world, 5 thread database
         * 8 vCPU = 2 thread world, 6 thread database
         * 9 vCPU = 3 thread world, 6 thread database
         * 10 vCPU = 3 thread world, 7 thread database
         */

        /// <summary>
        /// This is the number of threads used for World (Non Databae) operations
        /// </summary>
        public const double Multiplier = 0.34;



        // World Thread Management

        public static readonly int WorldThreadCount = (int)Math.Max(Environment.ProcessorCount * Multiplier, 1);

        public static readonly int LandblockManagerThreadCount = WorldThreadCount;
        public static readonly ParallelOptions LandblockManagerParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = LandblockManagerThreadCount };

        public static readonly int NetworkManagerThreadCount = WorldThreadCount;
        public static readonly ParallelOptions NetworkManagerParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = NetworkManagerThreadCount };


        // Database Thread Management

        public static readonly int DatabaseThreadCount = Math.Max(Environment.ProcessorCount - WorldThreadCount, 1);
        public static readonly ParallelOptions DatabaseParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = DatabaseThreadCount };
    }
}
