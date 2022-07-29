using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Models;
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
        public bool IsGenerator { get => GeneratorProfiles != null && GeneratorProfiles.Count > 0; }
       
        //public List<string> History = new List<string>();

        /// <summary>
        /// A generator can have multiple profiles / spawn multiple types of objects
        /// Each generator profile can in turn spawn multiple objects (init_create / max_create)
        /// </summary>
        public List<GeneratorProfile> GeneratorProfiles;

        /// <summary>
        /// Creates a list of active generator profiles
        /// from a list of biota generators
        /// </summary>
        public void AddGeneratorProfiles()
        {
            GeneratorProfiles = new List<GeneratorProfile>();
            uint i = 0;

            if (Biota.PropertiesGenerator != null)
            {
                foreach (var generator in Biota.PropertiesGenerator)
                    GeneratorProfiles.Add(new GeneratorProfile(this, generator, i++));
            }
        }

        /// <summary>
        /// Initialize Generator system
        /// </summary>
        public void InitializeGenerator()
        {
            // ensure if Max <= 0 (or defaulted to 0 from null) is not less than Init.
            // Profiles may have different settings but the core slots require Max to be greater than 0 if Init is greater than 0
            // defaulting to Max == Init for our purposes.
            if ((MaxGeneratedObjects <= 0 || MaxGeneratedObjects < InitGeneratedObjects) && InitGeneratedObjects > 0)
            {
                log.Warn($"[GENERATOR] 0x{Guid.Full.ToString()} {Name}.InitializeGenerator: {WeenieClassName} ({WeenieClassId}) MaxGeneratedObjects = {MaxGeneratedObjects} | InitGeneratedObjects = {InitGeneratedObjects}. Setting MaxGeneratedObjects = InitGeneratedObjects");
                MaxGeneratedObjects = InitGeneratedObjects;
            }

            AddGeneratorProfiles();
        }

        /// <summary>
        /// The number of currently spawned objects +
        /// the number of objects currently in the spawn queue
        /// </summary>
        public int CurrentCreate { get => GeneratorProfiles.Select(i => i.CurrentCreate).Sum(); }

        /// <summary>
        /// A list of indices into GeneratorProfiles where CurrentCreate > 0 or is on cooldown
        /// </summary>
        public List<int> GeneratorActiveProfiles
        {
            get
            {
                var activeProfiles = new List<int>();

                for (var i = 0; i < GeneratorProfiles.Count; i++)
                {
                    var profile = GeneratorProfiles[i];
                    if (profile.CurrentCreate > 0 || !profile.IsAvailable)
                        activeProfiles.Add(i);
                }
                return activeProfiles;
            }
        }

        /// <summary>
        /// Returns TRUE if all generator profiles are at max objects created
        /// </summary>
        public bool AllProfilesMaxed => !GeneratorProfiles.Any(i => !i.IsPlaceholder && !i.IsMaxed);

        /// <summary>
        /// Returns TRUE if all generator profiles are unavailable
        /// </summary>
        public bool AllProfilesUnavailable => !GeneratorProfiles.Any(i => !i.IsPlaceholder && i.IsAvailable);

        /// <summary>
        /// Adds object(s) to the spawn queue from a single RNG roll
        /// </summary>
        public void SelectAProfile()
        {
            //History.Add($"[{DateTime.UtcNow}] - SelectAProfile()");

            //bool rng_selected = false;

            if (GenStopSelectProfileConditions)
                return;

            //var totalProbability = rng_selected ? GetTotalProbability() : 1.0f;
            //var rng = ThreadSafeRandom.Next(0.0f, totalProbability);
            //var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var rng = ThreadSafeRandom.Next(0.0f, GetTotalProbability());

            for (var i = 0; i < GeneratorProfiles.Count; i++)
            {
                var profile = GeneratorProfiles[i];

                // skip PlaceHolder objects
                if (profile.IsPlaceholder)
                    continue;

                // is this profile already at its max_create?
                if (profile.IsMaxed)
                    continue;

                // is this profile currently timed out?
                if (!profile.IsAvailable)
                    continue;

                if (profile.RegenLocationType.HasFlag(RegenLocationType.Treasure))
                {
                    if (profile.Biota.InitCreate > 1)
                    {
                        log.Warn($"[GENERATOR] 0x{Guid} {Name}.SelectAProfile(): profile[{i}].RegenLocationType({profile.RegenLocationType}), profile.Biota.WCID({profile.Biota.WeenieClassId}), profile.Biota.InitCreate({profile.Biota.InitCreate}) > 1, set to 1. WCID: {WeenieClassId} - LOC: {Location.ToLOCString()}");
                        profile.Biota.InitCreate = 1;
                    }

                    if (profile.Biota.MaxCreate > 1)
                    {
                        log.Warn($"[GENERATOR] 0x{Guid} {Name}.SelectAProfile(): profile[{i}].RegenLocationType({profile.RegenLocationType}), profile.Biota.WCID({profile.Biota.WeenieClassId}), profile.Biota.MaxCreate({profile.Biota.MaxCreate}) > 1, set to 1. WCID: {WeenieClassId} - LOC: {Location.ToLOCString()}");
                        profile.Biota.MaxCreate = 1;
                    }
                }

                //var probability = rng_selected ? GetAdjustedProbability(i) : profile.Biota.Probability;
                //var probability = profile.Biota.Probability;
                var probability = GetAdjustedProbability(i);

                if (rng < probability || probability == -1)
                {
                    var numObjects = GetSpawnObjectsForProfile(profile);
                    profile.Enqueue(numObjects);
                    //log.Info($"[GENERATOR] 0x{Guid} {Name}.SelectAProfile(): profile[{i}] Enqueued {numObjects} {profile.Biota.WeenieClassId} for spawning. MaxObjectsSpawned = {profile.MaxObjectsSpawned} | Exhusted = {profile.RemoveQueue.Count == profile.MaxCreate} | {profile.CurrentCreate} | {profile.MaxCreate} | {profile.Spawned.Count} | {profile.RemoveQueue.Count}");

                    //var rng_str = probability == -1 ? "" : "RNG ";
                    //History.Add($"[{DateTime.UtcNow}] - SelectAProfile() - {rng_str}selected slot {i} to spawn, adding {numObjects} objects ({profile.CurrentCreate}/{profile.MaxCreate})");

                    // if RNG rolled, we are done with this roll
                    if (profile.Biota.Probability != -1)
                    {
                        //rng_selected = true;
                        break;
                    }

                    // stop conditions
                    if (GenStopSelectProfileConditions)
                        return;
                }
            }
        }

        /// <summary>
        /// Returns the total probability of all RNG profiles
        /// which arent at max objects spawned yet or on cooldown
        /// </summary>
        public float GetTotalProbability()
        {
            var totalProbability = 0.0f;
            var lastProbability = 0.0f;

            foreach (var profile in GeneratorProfiles)
            {
                var probability = profile.Biota.Probability;

                if (probability == -1)
                {
                    //if (!profile.IsMaxed)
                    if (!profile.IsMaxed && profile.IsAvailable)
                        return 1.0f;

                    continue;
                }
                //if (!profile.IsMaxed)
                if (!profile.IsMaxed && profile.IsAvailable)
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
        /// taking into account previous profile probabilities which are already at max objects spawned or on cooldown
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
                profile = GeneratorProfiles[i];
                var probability = profile.Biota.Probability;

                if (probability == -1)
                    continue;

                //if (!profile.IsMaxed)
                if (!profile.IsMaxed && profile.IsAvailable)
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
        /// Get the current number of objects to spawn for a specific profile
        /// </summary>
        public int GetSpawnObjectsForProfile(GeneratorProfile profile)
        {
            // get the number of objects to spawn for this profile
            // usually profile.InitCreate, must be at least profile.InitCreate while not to exceed generator.MaxCreate and profile.MaxCreate,
            // -1 for profile.InitCreate == 1
            // -1 for profile.MaxCreate == profile can be spawned infinitely as long as generator.MaxCreate has not been met.

            var initCreate = profile.InitCreate;
            var maxCreate = profile.MaxCreate;
            var numObjects = 0;

            if (initCreate == -1 || maxCreate == -1)
                numObjects = 1;
            else
                numObjects = initCreate;

            //Console.WriteLine($"INIT - 0x{Guid.ToString()} {Name} ({WeenieClassId}): CurrentCreate = {CurrentCreate} | profile.Biota.InitCreate = {profile.Biota.InitCreate} | profile.Biota.MaxCreate = {profile.Biota.MaxCreate} | InitCreate: {InitCreate} | MaxCreate: {MaxCreate} | initCreate: {initCreate} | maxCreate: {maxCreate} | leftObjects = {leftObjects} | numObjects: {numObjects}");            

            var genSlotsAvailable = MaxCreate - CurrentCreate;
            var profileSlotsAvailable = profile.MaxCreate - profile.CurrentCreate;

            if (genSlotsAvailable < numObjects)
                numObjects = genSlotsAvailable;

            if (profile.MaxCreate != -1 && profileSlotsAvailable < numObjects)
                numObjects = profileSlotsAvailable;

            if (numObjects == 0 && initCreate == 0)
                log.Warn($"[GENERATOR] 0x{Guid}:{WeenieClassId} {Name}.GetSpawnObjectsForProfile(profile[{profile.LinkId}]): profile.InitCreate = {profile.InitCreate} | profile.MaxCreate = {profile.MaxCreate} | profile.WeenieClassId = {profile.WeenieClassId} | Profile Init invalid, cannot spawn.");
            else if (numObjects == 0)
               log.Warn($"[GENERATOR] 0x{Guid}:{WeenieClassId} {Name}.GetSpawnObjectsForProfile(profile[{profile.LinkId}]): profile.InitCreate = {profile.InitCreate} | profile.MaxCreate = {profile.MaxCreate} | profile.WeenieClassId = {profile.WeenieClassId} | genSlotsAvailable = {genSlotsAvailable} | profileSlotsAvailable = {profileSlotsAvailable} | numObjects = {numObjects}, cannot spawn.");

            return numObjects;
        }

        /// <summary>
        /// Returns TRUE if stop conditions have been reached for aborting generator profile selection
        /// </summary>
        public bool GenStopSelectProfileConditions
        {
            get
            {
                if (CurrentCreate >= MaxCreate)
                {
                    //if (CurrentCreate > InitCreate)
                    //log.Debug($"{WeenieClassId} - 0x{Guid}:{Name}.StopConditionsInit(): CurrentCreate({CurrentCreate}) > InitCreate({InitCreate})");

                    return true;
                }

                if (CurrentlyPoweringUp && CurrentCreate >= InitCreate)
                {
                    return true;
                }

                return AllProfilesUnavailable || AllProfilesMaxed;
            }
        }

        /// <summary>
        /// Enables/disables a generator based on time status
        /// </summary>
        public void CheckGeneratorStatus()
        {
            switch (GeneratorTimeType)
            {
                // TODO: defined
                case GeneratorTimeType.RealTime:
                    CheckRealTimeStatus();
                    break;
                case GeneratorTimeType.Event:
                    CheckEventStatus();
                    break;
                case GeneratorTimeType.Night:
                case GeneratorTimeType.Day:
                    CheckTimeOfDayStatus();
                    break;
            }            
        }

        /// <summary>
        /// Enables/disables a generator based on in-game time of day, Day or Night
        /// </summary>
        public void CheckTimeOfDayStatus()
        {
            var prevDisabled = GeneratorDisabled;
           
            var isDay = Timers.CurrentInGameTime.IsDay;
            var isDayGenerator = GeneratorTimeType == GeneratorTimeType.Day;

            //GeneratorDisabled = isDay != isDayGenerator;
            //HandleStatus(prevDisabled);

            HandleStatusStaged(prevDisabled, isDay, isDayGenerator);
        }

        /// <summary>
        /// Enables/disables a generator based on realtime between start/end time
        /// </summary>
        public void CheckRealTimeStatus()
        {
            var prevDisabled = GeneratorDisabled;

            var now = (int)Time.GetUnixTime();

            var start = (now < GeneratorStartTime) && (GeneratorStartTime > 0);
            var end = (now > GeneratorEndTime) && (GeneratorEndTime > 0);

            //GeneratorDisabled = ((now < GeneratorStartTime) && (GeneratorStartTime > 0)) || ((now > GeneratorEndTime) && (GeneratorEndTime > 0));
            //HandleStatus(prevDisabled);

            HandleStatusStaged(prevDisabled, start, end);
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
            var started = EventManager.IsEventStarted(GeneratorEvent, this, null);

            //GeneratorDisabled = !enabled || !started;
            //HandleStatus(prevState);

            HandleStatusStaged(prevState, enabled, started);
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

        private bool eventStatusChanged = false;

        /// <summary>
        /// Handles starting/stopping the generator
        /// </summary>
        public void HandleStatusStaged(bool prevDisabled, bool cond1, bool cond2)
        {
            var change = false;
            switch (GeneratorTimeType)
            {
                case GeneratorTimeType.RealTime:
                    change = cond1 || cond2;
                    break;
                case GeneratorTimeType.Event:
                    change = !cond1 || !cond2;
                    break;
                case GeneratorTimeType.Day:
                case GeneratorTimeType.Night:
                    change = cond1 != cond2;
                    break;
            }

            if (eventStatusChanged)
            {
                GeneratorDisabled = change;

                if (!GeneratorDisabled)
                    StartGenerator();
                else
                    DisableGenerator();

                eventStatusChanged = false;
            }
            else
            {
                if (prevDisabled != change)
                    eventStatusChanged = true;
            }
        }

        /// <summary>
        /// Called when a generator is first created
        /// </summary>
        public void StartGenerator()
        {
            //Console.WriteLine($"{Name}.StartGenerator()");

            if (CurrentlyPoweringUp)
                return;

            CurrentlyPoweringUp = true;

            if (GeneratorInitialDelay > 0)
            {
                NextGeneratorRegenerationTime = GetNextRegenerationTime(GeneratorInitialDelay);
                CurrentLandblock?.ResortWorldObjectIntoSortedGeneratorRegenerationList(this);
            }
            else
            {
                if (this is Container && this is not Creature || (!string.IsNullOrEmpty(GeneratorEvent) && RegenerationInterval == 0))
                    Generator_Generate();

                if (InitCreate == 0)
                    CurrentlyPoweringUp = false;
            }
        }

        private double GetNextRegenerationTime(double generatorInitialDelay)
        {
            if (RegenerationTimestamp == 0)
                return Time.GetUnixTime();

            return Time.GetUnixTime() + generatorInitialDelay;
        }

        /// <summary>
        /// Disables a generator and Processes GeneratorDestructionDirective
        /// </summary>
        public void DisableGenerator()
        {
            // generator has been disabled, potentially destroy, kill or leave behind everything in registry and reset back to defaults
            ProcessGeneratorDestructionDirective(GeneratorEndDestructionType);
        }

        /// <summary>
        /// Called upon death of a generator and Processes GeneratorDestructionDirective
        /// </summary>
        public void OnGeneratorDeath()
        {
            // generator has been killed, potentially destroy, kill or leave behind everything in registry and reset back to defaults
            ProcessGeneratorDestructionDirective(GeneratorDestructionType);
        }

        /// <summary>
        /// Called upon death of a generator and Processes GeneratorDestructionDirective
        /// </summary>
        public void OnGeneratorDestroy()
        {
            // generator has been destroyed, potentially destroy, kill or leave behind everything in registry and reset back to defaults
            ProcessGeneratorDestructionDirective(GeneratorDestructionType);
        }

        /// <summary>
        /// Destroys/Kills all of its spawned objects, if specifically directed, and resets back to default
        /// </summary>
        public void ProcessGeneratorDestructionDirective(GeneratorDestruct generatorDestructType, bool fromLandblockUnload = false)
        {
            switch (generatorDestructType)
            {
                case GeneratorDestruct.Kill:
                    foreach (var generator in GeneratorProfiles)
                    {
                        generator.KillAll();
                    }
                    break;
                case GeneratorDestruct.Destroy:
                    foreach (var generator in GeneratorProfiles)
                    {
                        generator.DestroyAll(fromLandblockUnload);
                    }
                    break;
                case GeneratorDestruct.Nothing:
                default:
                    break;
            }
        }

        /// <summary>
        /// Callback system for objects notifying their generators of events,
        /// ie. item pickup
        /// </summary>
        public void NotifyOfEvent(RegenerationType regenerationType)
        {
            if (GeneratorId == null || Generator == null) return;

            var generator = Generator.GeneratorProfiles.FirstOrDefault(g => g.Spawned.ContainsKey(Guid.Full));
            generator?.NotifyGenerator(Guid, regenerationType);

            if (Generator.GeneratorId > 0) // Generator is controlled by another generator.
            {
                if ((!(Generator is Container) || Generator.GeneratorAutomaticDestruction) && Generator.InitCreate > 0 && Generator.CurrentCreate == 0) // Parent generator is non-container (Container, Corpse, Chest, Slumlord, Storage, Hook, Creature) generator
                    Generator.Destroy(); // Generator's complete spawn count has been wiped out
            }
            else if (Generator.GeneratorAutomaticDestruction && Generator.InitCreate > 0 && Generator.CurrentCreate == 0)
            {
                Generator.Destroy(); // Generator's complete spawn count has been wiped out
            }

            //if (!Generator.IsDestroyed)
            //    Generator.SelectAProfile();

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
                var profile = new PropertiesGenerator();
                profile.WeenieClassId = link.WeenieClassId;
                profile.ObjCellId = link.ObjCellId;
                profile.OriginX = link.OriginX;
                profile.OriginY = link.OriginY;
                profile.OriginZ = link.OriginZ;
                profile.AnglesW = link.AnglesW;
                profile.AnglesX = link.AnglesX;
                profile.AnglesY = link.AnglesY;
                profile.AnglesZ = link.AnglesZ;
                profile.Delay = profileTemplate.Biota.Delay;
                profile.Probability = profileTemplate.Biota.Probability;
                profile.InitCreate = profileTemplate.Biota.InitCreate;
                profile.MaxCreate = profileTemplate.Biota.MaxCreate;
                profile.WhenCreate = profileTemplate.Biota.WhenCreate;
                profile.WhereCreate = profileTemplate.Biota.WhereCreate;

                GeneratorProfiles.Add(new GeneratorProfile(this, profile, link.Guid));
                if (profile.Probability == -1)
                {
                    InitCreate += profile.InitCreate;
                    MaxCreate += profile.MaxCreate;
                }
            }
        }

        /// <summary>
        /// Called every [RegenerationInterval] seconds<para />
        /// Also called from EmoteManager, Chest.Reset(), WorldObject.OnGenerate()
        /// </summary>
        public void Generator_Update()
        {
            //Console.WriteLine($"{Name}.Generator_HeartBeat({HeartbeatInterval})");

            if (!FirstEnterWorldDone)
                FirstEnterWorldDone = true;

            CheckGeneratorStatus();

            if (!GeneratorEnteredWorld)
            {
                CheckGeneratorStatus(); // due to staging if generator hadn't entered world, reprocess CheckGeneratorStatus for first generator status to update

                if (!GeneratorDisabled)
                    StartGenerator();   // spawn initial objects for this generator

                GeneratorEnteredWorld = true;
            }
        }

        /// <summary>
        /// Called every [RegenerationInterval] seconds<para />
        /// Also called from EmoteManager, Chest.Reset(), WorldObject.OnGenerate()
        /// </summary>
        public void Generator_Generate()
        {
            //Console.WriteLine($"{Name}.Generator_Generate({RegenerationInterval})");

            if (!GeneratorDisabled)
            {
                if (CurrentlyPoweringUp)
                {
                    //Console.WriteLine($"{Name}.Generator_Generate({RegenerationInterval}) SelectAProfile: Init={InitCreate} Current={CurrentCreate} Max={MaxCreate} GenStopSelectProfileConditions={GenStopSelectProfileConditions}");
                    var genLoopCount = 0;
                    while (!GenStopSelectProfileConditions)
                    {
                        SelectAProfile();
                        genLoopCount++;

                        if (genLoopCount > 1000)
                        {
                            log.Error($"[GENERATOR] 0x{Guid} {Name}.Generator_Generate(): genLoopCount > 1000, aborted init spawn. GenStopSelectProfileConditions: {GenStopSelectProfileConditions} | InitCreate: {InitCreate} | CurrentCreate: {CurrentCreate} | WCID: {WeenieClassId} - LOC: {Location.ToLOCString()}");
                            break;
                        }
                    }
                    CurrentlyPoweringUp = false;
                }
                else
                {
                    //Console.WriteLine($"{Name}.Generator_Generate({RegenerationInterval}) SelectAProfile: Init={InitCreate} Current={CurrentCreate} Max={MaxCreate}");
                    SelectAProfile();
                }
            }

            foreach (var profile in GeneratorProfiles)
                profile.Spawn_HeartBeat();
        }

        public virtual void ResetGenerator()
        {
            foreach (var generator in GeneratorProfiles)
            {
                generator.Reset();
            }
        }

        public uint? GetStaticGuid(uint dynamicGuid)
        {
            foreach (var profile in GeneratorProfiles)
            {
                if (profile.Spawned.ContainsKey(dynamicGuid))
                    return profile.Id;
            }
            return null;
        }
    }
}
