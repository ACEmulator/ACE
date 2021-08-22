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
                log.Warn($"0x{Guid.Full.ToString()} {Name}.InitializeGenerator: {WeenieClassName} ({WeenieClassId}) MaxGeneratedObjects = {MaxGeneratedObjects} | InitGeneratedObjects = {InitGeneratedObjects}. Setting MaxGeneratedObjects = InitGeneratedObjects");
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

            var loopcount = 0;

            while (true)
            {
                if (StopConditionsInit)
                {
                    CurrentlyPoweringUp = false;
                    return;
                }

                var totalProbability = rng_selected ? GetTotalProbability() : 1.0f;
                var rng = ThreadSafeRandom.Next(0.0f, totalProbability);

                for (var i = 0; i < GeneratorProfiles.Count; i++)
                {
                    var profile = GeneratorProfiles[i];

                    // skip PlaceHolder objects
                    if (profile.IsPlaceholder)
                        continue;

                    // is this profile already at its max_create?
                    if (profile.MaxObjectsSpawned)
                        continue;

                    if (profile.RegenLocationType.HasFlag(RegenLocationType.Treasure))
                    {
                        if (profile.Biota.InitCreate > 1)
                        {
                            log.Warn($"0x{Guid} {Name}.SelectProfilesInit(): profile[{i}].RegenLocationType({profile.RegenLocationType}), profile.Biota.WCID({profile.Biota.WeenieClassId}), profile.Biota.InitCreate({profile.Biota.InitCreate}) > 1, set to 1. WCID: {WeenieClassId} - LOC: {Location.ToLOCString()}");
                            profile.Biota.InitCreate = 1;
                        }

                        if (profile.Biota.MaxCreate > 1)
                        {
                            log.Warn($"0x{Guid} {Name}.SelectProfilesInit(): profile[{i}].RegenLocationType({profile.RegenLocationType}), profile.Biota.WCID({profile.Biota.WeenieClassId}), profile.Biota.MaxCreate({profile.Biota.MaxCreate}) > 1, set to 1. WCID: {WeenieClassId} - LOC: {Location.ToLOCString()}");
                            profile.Biota.MaxCreate = 1;
                        }
                    }

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
                        if (StopConditionsInit)
                        {
                            CurrentlyPoweringUp = false;
                            return;
                        }
                    }
                }

                loopcount++;

                if (loopcount > 1000)
                {
                    log.Warn($"0x{Guid} {Name}.SelectProfilesInit(): loopcount > 1000, aborted. WCID: {WeenieClassId} - LOC: {Location.ToLOCString()}");
                    CurrentlyPoweringUp = false;
                    return;
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

                // skip PlaceHolder objects
                if (profile.IsPlaceholder)
                    continue;

                // is this profile already at its max_create?
                if (profile.MaxObjectsSpawned)
                    continue;

                var probability = GetAdjustedProbability(i);
                if (rng < probability || probability == -1)
                {
                    //var rng_str = probability == -1 ? "" : "RNG ";
                    var numObjects = GetMaxObjects(profile);
                    profile.Enqueue(numObjects);

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
        /// Adds more objects to the spawn queue based on RNG rolls
        /// </summary>
        public void SelectMoreProfiles()
        {
            //History.Add($"[{DateTime.UtcNow}] - SelectMoreProfiles()");

            // stop conditions
            if (StopConditionsMax) return;

            // only roll once here?
            var totalProbability = GetTotalProbability();
            var rng = ThreadSafeRandom.Next(0.0f, totalProbability);

            for (var i = 0; i < GeneratorProfiles.Count; i++)
            {
                var profile = GeneratorProfiles[i];

                // skip PlaceHolder objects
                if (profile.IsPlaceholder)
                    continue;

                // is this profile already at its max_create?
                if (profile.MaxObjectsSpawned)
                    continue;

                //var numObjects = 1;
                var numObjects = profile.Biota.InitCreate;
                if (numObjects == -1)
                    numObjects = 1;

                if (CurrentCreate + numObjects > MaxCreate)
                    continue;

                var probability = GetAdjustedProbability(i);
                if (rng < probability || probability == -1)
                {
                    //var rng_str = probability == -1 ? "" : "RNG ";
                    //var numObjects = GetMaxObjects(profile);
                    profile.Enqueue(numObjects);

                    //History.Add($"[{DateTime.UtcNow}] - SelectMoreProfiles() - {rng_str}selected slot {i} to spawn ({profile.CurrentCreate}/{profile.MaxCreate})");

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
                {
                    if (!profile.MaxObjectsSpawned)
                        return 1.0f;

                    continue;
                }
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
                profile = GeneratorProfiles[i];
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
        public int GetInitObjects(GeneratorProfile profile)
        {
            // get the number of objects to spawn for this profile
            // usually profile.InitCreate, not to exceed generator.InitCreate
            //var numObjects = profile.Biota.InitCreate;
            var initCreate = profile.Biota.InitCreate;
            var maxCreate = profile.Biota.MaxCreate;
            var numObjects = 0;

            if (initCreate == -1 || maxCreate == -1)
                numObjects = 1;
            else
                numObjects = initCreate;

            var leftObjects = InitCreate - CurrentCreate;

            //Console.WriteLine($"0x{Guid.ToString()} {Name} ({WeenieClassId}): CurrentCreate = {CurrentCreate} | profile.Biota.InitCreate = {profile.Biota.InitCreate} | profile.Biota.MaxCreate = {profile.Biota.MaxCreate} | InitCreate: {InitCreate} | MaxCreate: {MaxCreate} | initCreate: {initCreate} | maxCreate: {maxCreate} | fillToInit: {fillToInit} | fillToMax: {fillToMax} | leftObjects = {leftObjects} | numObjects: {numObjects}");

            if (numObjects > leftObjects && InitCreate != 0)
                return leftObjects;

            return numObjects;
        }

        /// <summary>
        /// Get the current number of objects to spawn
        /// for profile max
        /// </summary>
        public int GetMaxObjects(GeneratorProfile profile)
        {
            // get the number of objects to spawn for this profile
            // usually profile.MaxCreate, not to exceed generator.MaxCreate
            var numObjects = profile.Biota.MaxCreate;

            if (numObjects == -1)
                numObjects = MaxCreate;

            var leftObjects = MaxCreate - CurrentCreate;

            if (numObjects > leftObjects && InitCreate != 0)
                numObjects = leftObjects;

            //Console.WriteLine($"CurrentCreate = {CurrentCreate} | profile.Biota.MaxCreate = {profile.Biota.MaxCreate} | MaxCreate: {MaxCreate} | numObjects: {numObjects}");

            return numObjects;
        }

        /// <summary>
        /// Get the current number of objects to spawn
        /// for profile max
        /// </summary>
        public int GetRNGInitToMaxObjects(GeneratorProfile profile)
        {
            // get the number of objects to spawn for this profile
            var initCreate = profile.Biota.InitCreate;
            var maxCreate = profile.Biota.MaxCreate;
            var numObjects = 0;
            bool fillToInit = false;
            bool fillToMax = false;

            if (initCreate == -1 || maxCreate == -1)
            {
                if (initCreate == -1)
                    fillToInit = true;

                if (maxCreate == -1)
                    fillToMax = true;
            }

            if (initCreate <= 0)
                initCreate = 1;

            if (maxCreate < initCreate)
                maxCreate = initCreate;

            numObjects = ThreadSafeRandom.Next(initCreate, maxCreate);

            if (fillToInit)
                numObjects = InitCreate;

            if (fillToMax)
                numObjects = MaxCreate;

            var leftObjects = MaxCreate - CurrentCreate;

            //Console.WriteLine($"0x{Guid.ToString()} {Name} ({WeenieClassId}): CurrentCreate = {CurrentCreate} | profile.Biota.InitCreate = {profile.Biota.InitCreate} | profile.Biota.MaxCreate = {profile.Biota.MaxCreate} | InitCreate: {InitCreate} | MaxCreate: {MaxCreate} | initCreate: {initCreate} | maxCreate: {maxCreate} | fillToInit: {fillToInit} | fillToMax: {fillToMax} | leftObjects = {leftObjects} | numObjects: {numObjects}");

            if (numObjects > leftObjects && InitCreate != 0)
                return leftObjects;

            return numObjects;
        }

        /// <summary>
        /// Returns TRUE if stop conditions have been reached for initial generator spawn
        /// </summary>
        public bool StopConditionsInit
        {
            get
            {
                if (CurrentCreate >= InitCreate)
                {
                    //if (CurrentCreate > InitCreate)
                        //log.Debug($"{WeenieClassId} - 0x{Guid}:{Name}.StopConditionsInit(): CurrentCreate({CurrentCreate}) > InitCreate({InitCreate})");

                    return true;
                }
                return AllProfilesMaxed;
            }
        }

        /// <summary>
        /// Returns TRUE if stop conditions have been reached for subsequent generator spawn
        /// </summary>
        public bool StopConditionsMax
        {
            get
            {
                if (CurrentCreate >= MaxCreate && MaxCreate != 0)
                {
                    //if (CurrentCreate > MaxCreate && MaxCreate != 0)
                        //log.Debug($"{WeenieClassId} - 0x{Guid}:{Name}.StopConditionsMax(): CurrentCreate({CurrentCreate}) > MaxCreate({MaxCreate})");

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
                if (this is Container || (!string.IsNullOrEmpty(GeneratorEvent) && RegenerationInterval == 0))
                    Generator_Regeneration();

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
                        foreach (var rNode in generator.Spawned.Values)
                        {
                            var wo = rNode.TryGetWorldObject();

                            if (wo is Creature creature && !creature.IsDead)
                                creature.Smite(this, true);
                        }

                        generator.Spawned.Clear();
                        generator.SpawnQueue.Clear();
                    }
                    break;
                case GeneratorDestruct.Destroy:
                    foreach (var generator in GeneratorProfiles)
                    {
                        foreach (var rNode in generator.Spawned.Values)
                        {
                            var wo = rNode.TryGetWorldObject();

                            if (wo != null && (!(wo is Creature creature) || !creature.IsDead))
                                wo.Destroy(true, fromLandblockUnload);
                        }

                        generator.Spawned.Clear();
                        generator.SpawnQueue.Clear();
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

            //if (!Generator.GeneratorDisabled)
            //{
                var removeQueueTotal = 0;

                foreach (var generator in Generator.GeneratorProfiles)
                {
                    generator.NotifyGenerator(Guid, regenerationType);
                    removeQueueTotal += generator.RemoveQueue.Count;
                }

                if (Generator.GeneratorId > 0) // Generator is controlled by another generator.
                {
                    if ((!(Generator is Container) || Generator.GeneratorAutomaticDestruction) && Generator.InitCreate > 0 && (Generator.CurrentCreate - removeQueueTotal) == 0) // Parent generator is non-container (Container, Corpse, Chest, Slumlord, Storage, Hook, Creature) generator
                        Generator.Destroy(); // Generator's complete spawn count has been wiped out
                }
                else if (Generator.GeneratorAutomaticDestruction && Generator.InitCreate > 0 && (Generator.CurrentCreate - removeQueueTotal) == 0)
                {
                    Generator.Destroy(); // Generator's complete spawn count has been wiped out
                }
            //}

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

            //foreach (var generator in GeneratorProfiles)
            //    generator.Maintenance_HeartBeat();

            CheckGeneratorStatus();

            if (!GeneratorEnteredWorld)
            {
                CheckGeneratorStatus(); // due to staging if generator hadn't entered world, reprocess CheckGeneratorStatus for first generator status to update

                if (!GeneratorDisabled)
                    StartGenerator();   // spawn initial objects for this generator

                GeneratorEnteredWorld = true;
            }
            else
            {
                foreach (var profile in GeneratorProfiles)
                    profile.Maintenance_HeartBeat();
            }
        }

        /// <summary>
        /// Called every [RegenerationInterval] seconds<para />
        /// Also called from EmoteManager, Chest.Reset(), WorldObject.OnGenerate()
        /// </summary>
        public void Generator_Regeneration()
        {
            //Console.WriteLine($"{Name}.Generator_Regeneration({RegenerationInterval})");

            //foreach (var profile in GeneratorProfiles)
            //    profile.Maintenance_HeartBeat();

            if (!GeneratorDisabled)
            {
                if (CurrentlyPoweringUp || (this is Container container && container.ResetMessagePending))
                {
                    //Console.WriteLine($"{Name}.Generator_Regeneration({RegenerationInterval}) SelectProfilesInit: Init={InitCreate} Current={CurrentCreate} Max={MaxCreate}");
                    SelectProfilesInit();
                }
                else
                {
                    //Console.WriteLine($"{Name}.Generator_Regeneration({RegenerationInterval}) SelectMoreProfiles: Init={InitCreate} Current={CurrentCreate} Max={MaxCreate}");
                    SelectMoreProfiles();
                }
            }

            foreach (var profile in GeneratorProfiles)
                profile.Spawn_HeartBeat();
        }

        public virtual void ResetGenerator()
        {
            foreach (var generator in GeneratorProfiles)
            {
                foreach (var rNode in generator.Spawned.Values)
                {
                    var wo = rNode.TryGetWorldObject();

                    if (wo != null && !wo.IsGenerator)
                        wo.Destroy();
                    else if (wo != null && wo.IsGenerator)
                    {
                        wo.ResetGenerator();
                        wo.Destroy();
                    }
                }

                generator.Spawned.Clear();
                generator.SpawnQueue.Clear();
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
