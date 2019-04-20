using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// A generator handles spawning other WorldObjects
    /// </summary>
    partial class WorldObject
    {
        /// <summary>
        /// The generator that spawned this WorldObject
        /// </summary>
        public WorldObject Generator { get; set; }

        /// <summary>
        /// Returns TRUE if this object is a generator
        /// (spawns other world objects)
        /// </summary>
        public bool IsGenerator { get => GeneratorProfiles.Count > 0; }

        /// <summary>
        /// Returns TRUE if generator is linked
        /// </summary>
        public bool IsLinked;

        //public List<string> History = new List<string>();

        /// <summary>
        /// A generator can have multiple profiles / spawn multiple types of objects
        /// Each generator profile can in turn spawn multiple objects (init_create / max_create)
        /// </summary>
        public List<Generator> GeneratorProfiles;

        /// <summary>
        /// Creates a list of active generator profiles
        /// from a list of biota generators
        /// </summary>
        public void AddGeneratorProfiles()
        {
            GeneratorProfiles = new List<Generator>();

            foreach (var generator in Biota.BiotaPropertiesGenerator)
                GeneratorProfiles.Add(new Generator(this, generator));
        }

        /// <summary>
        /// The number of currently spawned objects +
        /// the number of objects currently in the spawn queue
        /// </summary>
        //public int CurrentCreate { get => GeneratorProfiles.Select(i => i.CurrentCreate).Sum(); }
        public int CurrentCreate;   // maintained directly

        /// <summary>
        /// A list of indices into GeneratorProfiles where CurrentCreate > 0
        /// </summary>
        public List<int> GeneratorActiveProfiles
        {
            get
            {
                var activeProfiles = new List<int>();

                for (var i = 0; i < GeneratorProfiles.Count; i++)
                {
                    var profile = GeneratorProfiles[i];
                    if (profile.CurrentCreate > 0)
                        activeProfiles.Add(i);
                }
                return activeProfiles;
            }
        }

        /// <summary>
        /// Returns TRUE if all generator profiles are at init objects created
        /// </summary>
        public bool AllProfilesInitted { get => GeneratorProfiles.Count(i => i.InitObjectsSpawned) == GeneratorProfiles.Count; }

        /// <summary>
        /// Retunrs TRUE if all generator profiles are at max objects created
        /// </summary>
        public bool AllProfilesMaxed { get => GeneratorProfiles.Count(i => i.MaxObjectsSpawned) == GeneratorProfiles.Count; }

        /// <summary>
        /// Adds initial objects to the spawn queue based on RNG rolls
        /// </summary>
        public void SelectProfilesInit()
        {
            //History.Add($"[{DateTime.UtcNow}] - SelectProfilesInit()");

            bool rng_selected = false;

            while (true)
            {
                if (StopConditionsInit) return;

                var totalProbability = rng_selected ? GetTotalProbability() : 1.0f;
                var rng = ThreadSafeRandom.Next(0.0f, totalProbability);

                for (var i = 0; i < GeneratorProfiles.Count; i++)
                {
                    var profile = GeneratorProfiles[i];

                    // is this profile already at its max_create?
                    if (profile.MaxObjectsSpawned)
                        continue;

                    var probability = rng_selected ? GetAdjustedProbability(i) : profile.Biota.Probability;

                    if (rng < probability || probability == -1)
                    {
                        var numObjects = GetInitObjects(profile);
                        profile.Enqueue(numObjects);

                        //var rng_str = probability == -1 ? "" : "RNG ";
                        //History.Add($"[{DateTime.UtcNow}] - SelectProfilesInit() - {rng_str}selected slot {i} to spawn, adding {numObjects} objects ({profile.CurrentCreate}/{profile.MaxCreate})");

                        // if RNG rolled, we are done with this roll
                        if (profile.Biota.Probability != -1)
                        {
                            rng_selected = true;
                            break;
                        }

                        // stop conditions
                        if (StopConditionsInit) return;
                    }
                }
            }
        }

        /// <summary>
        /// Adds subsequent objects to the spawn queue based on RNG rolls
        /// </summary>
        public void SelectProfilesMax()
        {
            //History.Add($"[{DateTime.UtcNow}] - SelectProfilesMax()");

            // stop conditions
            if (StopConditionsMax) return;

            // only roll once here?
            var totalProbability = GetTotalProbability();
            var rng = ThreadSafeRandom.Next(0.0f, totalProbability);

            for (var i = 0; i < GeneratorProfiles.Count; i++)
            {
                var profile = GeneratorProfiles[i];

                // is this profile already at its max_create?
                if (profile.MaxObjectsSpawned)
                    continue;

                var probability = GetAdjustedProbability(i);
                if (rng < probability || probability == -1)
                {
                    var rng_str = probability == -1 ? "" : "RNG ";
                    profile.Enqueue(1);

                    //History.Add($"[{DateTime.UtcNow}] - SelectProfilesMax() - {rng_str}selected slot {i} to spawn ({profile.CurrentCreate}/{profile.MaxCreate})");

                    // if RNG rolled, we are done with this roll
                    if (profile.Biota.Probability != -1)
                        break;

                    // stop conditions
                    if (StopConditionsMax) return;
                }
            }
        }

        /// <summary>
        /// Returns the total probability of all RNG profiles
        /// which arent at max objects spawned yet
        /// </summary>
        public float GetTotalProbability()
        {
            var totalProbability = 0.0f;
            var lastProbability = 0.0f;

            foreach (var profile in GeneratorProfiles)
            {
                var probability = profile.Biota.Probability;

                if (probability == -1)
                    continue;

                if (!profile.MaxObjectsSpawned)
                {
                    if (lastProbability > probability)
                        lastProbability = 0.0f;

                    var diff = probability - lastProbability;
                    totalProbability += diff;
                }
                lastProbability = probability;
            }

            return totalProbability;
        }

        /// <summary>
        /// Returns the max probability from all the generator profiles
        /// </summary>
        public float GetMaxProbability()
        {
            var maxProbability = float.MinValue;

            // note: this will also include maxed generator profiles!
            foreach (var profile in GeneratorProfiles)
            {
                var probability = profile.Biota.Probability;

                if (probability > maxProbability)
                    maxProbability = probability;
            }
            return maxProbability;
        }

        /// <summary>
        /// Returns the adjust probability for a generator profile index,
        /// taking into account previous profile probabilities which are already at max objects spawned
        /// </summary>
        public float GetAdjustedProbability(int index)
        {
            // say theres a generator with 2 profiles
            // the first has a 99% chance to spawn, and the second has a 1% chance
            // the generator init_create is 1, and the max_create is 2
            // when the generator first spawns in, the first object is created, as expected
            // then the hearbeat happens later, and it sees it can spawn up to 1 additional object
            // the rare item is the only item left, with the 1 % chance
            // so the question is, in that scenario, would the rare item always spawn then?
            // or would it only do 1 roll, and the rare item would still have only a 1% chance to spawn?

            var profile = GeneratorProfiles[index];
            if (profile.Biota.Probability == -1)
                return -1;

            var totalProbability = 0.0f;
            var lastProbability = 0.0f;

            for (var i = 0; i <= index; i++)
            {
                var probability = profile.Biota.Probability;

                if (probability == -1)
                    continue;

                if (!profile.MaxObjectsSpawned)
                {
                    if (lastProbability > probability)
                        lastProbability = 0.0f;

                    var diff = probability - lastProbability;
                    totalProbability += diff;
                }
                lastProbability = probability;
            }
            return totalProbability;
        }

        /// <summary>
        /// Get the current number of objects to spawn
        /// for profile initialization
        /// </summary>
        public int GetInitObjects(Generator profile)
        {
            // get the number of objects to spawn for this profile
            // usually profile.InitCreate, not to exceed generator.InitCreate
            var numObjects = profile.Biota.InitCreate;
            var leftObjects = InitCreate - CurrentCreate;

            if (numObjects > leftObjects && InitCreate != 0 && !IsLinked)
                numObjects = leftObjects;

            return numObjects;
        }

        /// <summary>
        /// Returns TRUE if stop conditions have been reached for initial generator spawn
        /// </summary>
        public bool StopConditionsInit
        {
            get
            {
                if (CurrentCreate >= InitCreate && !IsLinked)
                {
                    if (CurrentCreate > InitCreate)
                        log.Debug($"{WeenieClassId} - 0x{Guid.Full:X8}:{Name}.StopConditionsInit(): CurrentCreate({CurrentCreate}) > InitCreate({InitCreate})");

                    return true;
                }
                return AllProfilesInitted || AllProfilesMaxed;
            }
        }

        /// <summary>
        /// Returns TRUE if stop conditions have been reached for subsequent generator spawn
        /// </summary>
        public bool StopConditionsMax
        {
            get
            {
                if (CurrentCreate >= MaxCreate && MaxCreate != 0 && !IsLinked)
                {
                    if (CurrentCreate > MaxCreate && MaxCreate != 0)
                        log.Debug($"{WeenieClassId} - 0x{Guid.Full:X8}:{Name}.StopConditionsMax(): CurrentCreate({CurrentCreate}) > MaxCreate({MaxCreate})");

                    return true;
                }
                return AllProfilesMaxed;
            }
        }

        /// <summary>
        /// Enables/disables a generator based on time status
        /// </summary>
        public void CheckGeneratorStatus()
        {
            switch (GeneratorTimeType)
            {
                // TODO: defined/night/day
                case GeneratorTimeType.RealTime:
                    CheckRealTimeStatus();
                    break;
                case GeneratorTimeType.Event:
                    CheckEventStatus();
                    break;
            }            
        }

        /// <summary>
        /// Enables/disables a generator based on realtime between start/end time
        /// </summary>
        public void CheckRealTimeStatus()
        {
            var prevDisabled = GeneratorDisabled;

            var now = (int)Time.GetUnixTime();

            GeneratorDisabled = ((now < GeneratorStartTime) && (GeneratorStartTime > 0)) || ((now > GeneratorEndTime) && (GeneratorEndTime > 0));

            HandleStatus(prevDisabled);
        }

        /// <summary>
        /// Enables/disables a generator based on world event status
        /// </summary>
        public void CheckEventStatus()
        {
            if (string.IsNullOrEmpty(GeneratorEvent))
                return;

            var prevState = GeneratorDisabled;

            if (!EventManager.IsEventAvailable(GeneratorEvent))
                return;

            var enabled = EventManager.IsEventEnabled(GeneratorEvent);
            var started = EventManager.IsEventStarted(GeneratorEvent);

            GeneratorDisabled = !enabled || !started;

            HandleStatus(prevState);
        }

        /// <summary>
        /// Handles starting/stopping the generator
        /// </summary>
        public void HandleStatus(bool prevDisabled)
        {
            if (prevDisabled == GeneratorDisabled)
                return;     // no state change

            if (prevDisabled)
                StartGenerator();
            else
                DisableGenerator();
        }

        /// <summary>
        /// Called when a generator is first created
        /// </summary>
        public void StartGenerator()
        {
            CurrentlyPoweringUp = true;

            //foreach (var generator in GeneratorProfiles)
            //generator.TrySpawn(true);
            SelectProfilesInit();

            CurrentlyPoweringUp = false;
        }

        /// <summary>
        /// Disables a generator, and destroys all of its spawned objects
        /// </summary>
        public void DisableGenerator()
        {
            // generator has been disabled, de-spawn everything in registry and reset back to defaults
            switch (GeneratorEndDestructionType)
            {
                case GeneratorDestruct.Kill:
                    foreach (var generator in GeneratorProfiles)
                    {
                        foreach (var rNode in generator.Spawned.Values)
                        {
                            if (rNode.WorldObject is Creature wo)
                                wo.Smite(this);
                        }

                        generator.Spawned.Clear();
                        generator.SpawnQueue.Clear();
                    }
                    break;
                case GeneratorDestruct.Nothing:
                    break;
                case GeneratorDestruct.Destroy:
                default:
                    foreach (var generator in GeneratorProfiles)
                    {
                        foreach (var rNode in generator.Spawned.Values)
                            rNode.WorldObject.Destroy();

                        generator.Spawned.Clear();
                        generator.SpawnQueue.Clear();
                    }
                    break;
            }
        }

        /// <summary>
        /// Callback system for objects notifying their generators of events,
        /// ie. item pickup
        /// </summary>
        public void NotifyOfEvent(RegenerationType regenerationType)
        {
            if (GeneratorId == null) return;

            if (!Generator.GeneratorDisabled)
            {
                foreach (var generator in Generator.GeneratorProfiles)
                    generator.NotifyGenerator(Guid, regenerationType);
            }

            Generator = null;
            GeneratorId = null;
        }

        /// <summary>
        /// Called by ActivateLinks in WorldObject_Links for generators
        /// </summary>
        public void AddGeneratorLinks()
        {
            if (GeneratorProfiles == null || GeneratorProfiles.Count == 0)
            {
                Console.WriteLine($"{Name}.AddGeneratorLinks(): no profiles to link!");
                return;
            }

            var profileTemplate = GeneratorProfiles[0];

            foreach (var link in LinkedInstances)
            {
                var profile = new BiotaPropertiesGenerator();
                profile.WeenieClassId = link.WeenieClassId;
                profile.ObjCellId = link.ObjCellId;
                profile.OriginX = link.OriginX;
                profile.OriginY = link.OriginY;
                profile.OriginZ = link.OriginZ;
                profile.AnglesW = link.AnglesW;
                profile.AnglesX = link.AnglesX;
                profile.AnglesY = link.AnglesY;
                profile.AnglesZ = link.AnglesZ;
                profile.Probability = profileTemplate.Biota.Probability;
                profile.InitCreate = profileTemplate.Biota.InitCreate;
                profile.MaxCreate = profileTemplate.Biota.MaxCreate;
                profile.WhenCreate = profileTemplate.Biota.WhenCreate;
                profile.WhereCreate = profileTemplate.Biota.WhereCreate;

                GeneratorProfiles.Add(new Generator(this, profile));
            }

            IsLinked = true;
        }

        /// <summary>
        /// Called every ~5 seconds for object generators
        /// </summary>
        public void Generator_HeartBeat()
        {
            //Console.WriteLine($"{Name}.Generator_HeartBeat({HeartbeatInterval})");

            if (!FirstEnterWorldDone)
                FirstEnterWorldDone = true;

            CheckGeneratorStatus();

            if (!GeneratorEnteredWorld)
            {
                if (!GeneratorDisabled)
                    StartGenerator();   // spawn initial objects for this generator

                GeneratorEnteredWorld = true;
            }
            else if (!GeneratorDisabled)
                SelectProfilesMax();

            if (this is Chest chest)
            {
                // for chest generators, ResetMessage determines generator profile heartbeats
                if (!chest.ResetGenerator) return;

                chest.ResetGenerator = false;
            }

            foreach (var generator in GeneratorProfiles)
                generator.HeartBeat();
        }
    }
}
