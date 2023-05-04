using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ACE.Common
{
    /// <summary>
    /// We determine the number of available threads using Environment.ProcessCount
    /// We allocate(int)Math.Max(Environment.ProcessorCount * WorldThreadCountMultiplier, 1) to the World, and the remainder to the database.
    /// </summary>
    public class ThreadConfiguration
    {
        private double worldThreadCountMultiplier = 0.34;
        private double databaseThreadCountMultiplier = 0.66;

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
         * 11 vCPU = 3 thread world, 8 thread database
         * 12 vCPU = 4 thread world, 8 thread database
         */

        public double WorldThreadCountMultiplier
        {
            get => worldThreadCountMultiplier;
            set
            {
                worldThreadCountMultiplier = value;

                var threadCount = (int)Math.Max(Environment.ProcessorCount * value, 1);

                LandblockManagerParallelOptions.MaxDegreeOfParallelism = threadCount;
                NetworkManagerParallelOptions.MaxDegreeOfParallelism = threadCount;
            }
        }

        public double DatabaseThreadCountMultiplier
        {
            get => databaseThreadCountMultiplier;
            set
            {
                // This is to support for older configs that do not have this property defined
                if (value == 0)
                {
                    var worldThreadCount = (int)Math.Max(Environment.ProcessorCount * WorldThreadCountMultiplier, 1);
                    var databaseThreadCount = Math.Max(Environment.ProcessorCount - worldThreadCount, 1);
                    DatabaseParallelOptions.MaxDegreeOfParallelism = databaseThreadCount;
                    return;
                }

                databaseThreadCountMultiplier = value;

                var threadCount = (int)Math.Max(Environment.ProcessorCount * value, 1);

                DatabaseParallelOptions.MaxDegreeOfParallelism = threadCount;
            }
        }

        public bool MultiThreadedLandblockGroupPhysicsTicking { get; set; } = false;

        public bool MultiThreadedLandblockGroupTicking { get; set; } = false;


        // World Thread Management
        [JsonIgnore]
        public readonly ParallelOptions LandblockManagerParallelOptions = new ParallelOptions();
        [JsonIgnore]
        public readonly ParallelOptions NetworkManagerParallelOptions = new ParallelOptions();

        // Database Thread Management
        [JsonIgnore]
        public readonly ParallelOptions DatabaseParallelOptions = new ParallelOptions();
    }
}
