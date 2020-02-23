using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ACE.Common
{
    /// <summary>
    /// We determine the number of available threads using Environment.ProcessCount
    /// We allocate(int)Math.Max(Environment.ProcessorCount * WorldThreadCountMultiplier, 1) to the World, and the remainder to the database.
    /// </summary>
    public class ThreadConfiguration
    {
        private double worldThreadCountMultiplier;

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

        [System.ComponentModel.DefaultValue(0.34)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public double WorldThreadCountMultiplier
        {
            get => worldThreadCountMultiplier;
            set
            {
                worldThreadCountMultiplier = value;

                var worldThreadCount = (int)Math.Max(Environment.ProcessorCount * value, 1);
                var databaseThreadCount = Math.Max(Environment.ProcessorCount - worldThreadCount, 1);

                LandblockManagerParallelOptions.MaxDegreeOfParallelism = worldThreadCount;
                NetworkManagerParallelOptions.MaxDegreeOfParallelism = worldThreadCount;

                DatabaseParallelOptions.MaxDegreeOfParallelism = databaseThreadCount;
            }
        }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool MultiThreadedLandblockGroupPhysicsTicking { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool MultiThreadedLandblockGroupTicking { get; set; }


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
