using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Managers;
using ACE.Network.Enum;
using log4net;
using ACE.Network.Sequence;
using ACE.Network.Motion;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using System.IO;

namespace ACE.Entity
{
    public sealed class Player : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // TODO: link to Town Network marketplace portal destination in db, when db for that is finalized and implemented.
        private static readonly Position MarketplaceDrop = new Position(23855548, 49.206f, -31.935f, 0.005f, 0f, 0f, -0.7071068f, 0.7071068f); // PCAP verified drop
        private static readonly float PickUpDistance = .75f;

        public Session Session { get; }

        /// <summary>
        /// This will be false when in portal space
        /// </summary>
        public bool InWorld { get; set; }

        /// <summary>
        /// Different than InWorld which is false when in portal space
        /// </summary>
        public bool IsOnline { get; private set; }

        /// <summary>
        /// ObjectId of the currently selected Target (only players and creatures)
        /// </summary>
        private ObjectGuid selectedTarget = ObjectGuid.Invalid;

        /// <summary>
        /// Amount of times this character has left a portal this session
        /// </summary>
        public uint PortalIndex { get; set; } = 1u;

        /// <summary>
        /// tick-stamp for the server time of the last time the player changed state (combat state?)
        /// </summary>
        public double LastStateChangeTicks { get; set; }

        /// <summary>
        /// Last Streaming Object change tick
        /// </summary>
        public double LastStreamingObjectChange { get; set; }

        /// <summary>
        /// Level of the player
        /// </summary>
        public uint Level
        {
            get { return Character.Level; }
        }

        private AceCharacter Character { get { return AceObject as AceCharacter; } }

        public void SetCharacterOptions1(uint options1)
        {
            Character.CharacterOptions1Mapping = options1;
        }

        public void SetCharacterOptions2(uint options2)
        {
            Character.CharacterOptions2Mapping = options2;
        }

        private Dictionary<Skill, CreatureSkill> Skills
        {
            get { return AceObject.AceObjectPropertiesSkills; }
        }

        private readonly object clientObjectMutex = new object();

        /// <summary>
        /// FIXME(ddevec): This is the only object that need be locked in the player under the new model.
        ///   It must be locked because of how we handle obect updates -- We can clean this up in the future
        /// </summary>
        private readonly Dictionary<ObjectGuid, double> clientObjectList = new Dictionary<ObjectGuid, double>();

        public Dictionary<PositionType, Position> Positions
        {
            get { return AceObject.AceObjectPropertiesPositions; }
        }

        private Position PositionSanctuary
        {
            get
            {
                if (Positions.ContainsKey(PositionType.Sanctuary))
                {
                    return Positions[PositionType.Sanctuary];
                }
                return null;
            }
            set
            {
                Positions[PositionType.Sanctuary] = value;
            }
        }

        private Position PositionLastPortal
        {
            get
            {
                if (Positions.ContainsKey(PositionType.LastPortal))
                {
                    return Positions[PositionType.LastPortal];
                }
                return null;
            }
            set
            {
                Positions[PositionType.LastPortal] = value;
            }
        }

        public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions
        {
            get { return Character.CharacterOptions; }
        }

        public ReadOnlyCollection<Friend> Friends
        {
            get { return Character.Friends; }
        }

        public bool IsAdmin
        {
            get { return Character.IsAdmin; }
            set { Character.IsAdmin = value; }
        }

        public bool IsEnvoy
        {
            get { return Character.IsEnvoy; }
            set { Character.IsEnvoy = value; }
        }

        public bool IsArch
        {
            get { return Character.IsArch; }
            set { Character.IsArch = value; }
        }

        public bool IsPsr
        {
            get { return Character.IsPsr; }
            set { Character.IsPsr = value; }
        }

        public uint TotalLogins
        {
            get { return Character.TotalLogins; }
            set { Character.TotalLogins = value; }
        }

        /// <summary>
        /// This signature services MoveToObject and TurnToObject
        /// Update Position prior to start, start them moving or turning, set statemachine to moving.
        /// </summary>
        /// <param name="worldObjectPosition"></param>
        /// <param name="sequence"></param>
        /// <param name="movementType"></param>
        /// <returns>MovementStates</returns>
        public void OnAutonomousMove(Position worldObjectPosition, SequenceManager sequence, MovementTypes movementType, ObjectGuid targetGuid)
        {
            var newMotion = new UniversalMotion(MotionStance.Standing, worldObjectPosition, targetGuid);
            newMotion.DistanceFrom = 0.60f;
            newMotion.MovementTypes = MovementTypes.MoveToObject;
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePosition(this));
            CurrentLandblock.EnqueueBroadcastMotion(this, newMotion);
        }

        public Player(Session session, AceCharacter character)
            : base(character)
        {
            Session = session;

            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevel, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkill, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyString, new ByteSequence(false));

            // This is the default send upon log in and the most common.   Anything with a velocity will need to add that flag.
            PositionFlag |= UpdatePositionFlag.ZeroQx | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.Contact | UpdatePositionFlag.Placement;

            // FIXME(ddevec): Once physics data is refactored this shouldn't be needed
            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);
            PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Movement;

            // apply defaults.  "Load" should be overwriting these with values specific to the character
            // TODO: Load from database should be loading player data - including inventroy and positions
            CurrentMotionState = new UniversalMotion(MotionStance.Standing);

            // TODO: test and remove - this should not be needed.
            MotionTableId = 0x09000001u;
            SoundTableId = 0x20000001u;
            PhisicsTableId = 0x34000004u;
            SetupTableId = 0x02000001u;

            // radius for object updates
            ListeningRadius = 5f;
        }

        /// <summary>
        ///  Gets a list of Tracked Objects.
        /// </summary>
        public List<ObjectGuid> GetTrackedObjectGuids()
        {
            lock (clientObjectList)
            {
                return clientObjectList.Select(x => x.Key).ToList();
            }
        }

        public uint Age
        { get { return Character.Age; } }

        public uint CreationTimestamp
        { get { return Character.GetIntProperty(PropertyInt.CreationTimestamp) ?? 0; } }

        public AceObject GetAceObject()
        {
            return Character;
        }

        private MotionStance stance = MotionStance.Standing;

        // FIXME(ddevec): This should eventually be removed, with most of its contents making its way into the Player() constructor
        public void Load(AceCharacter character)
        {
            if (Common.ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (Session.AccessLevel == AccessLevel.Admin)
                    character.IsAdmin = true;
                if (Session.AccessLevel == AccessLevel.Developer)
                    character.IsArch = true;
                if (Session.AccessLevel == AccessLevel.Envoy)
                    character.IsEnvoy = true;
                // TODO: Need to setup and account properly for IsSentinel and IsAdvocate.
                // if (Session.AccessLevel == AccessLevel.Sentinel)
                //    character.IsSentinel = true;
                // if (Session.AccessLevel == AccessLevel.Advocate)
                //    character.IsAdvocate= true;
            }

            Location = character.Location;
            IsAlive = true;
            IsOnline = true;

            // TODO: test and remove - this should be coming in from AceObject.

            MotionTableId = Character.MotionTableId;
            SoundTableId = Character.SoundTableId;
            PhisicsTableId = Character.PhysicsTableId;
            SetupTableId = Character.SetupTableId;

            // Start vital ticking, if they need it
            if (Health.Current != Health.MaxValue)
            {
                VitalTickInternal(Health);
            }

            if (Stamina.Current != Stamina.MaxValue)
            {
                VitalTickInternal(Stamina);
            }

            if (Mana.Current != Mana.MaxValue)
            {
                VitalTickInternal(Mana);
            }

            ContainerCapacity = 7;

            if (Character.DefaultScale != null)
                ObjScale = Character.DefaultScale;

            AddCharacterBaseModelData();

            this.TotalLogins++;
            Sequences.AddOrSetSequence(SequenceType.ObjectInstance, new UShortSequence((ushort)TotalLogins));

            // SendSelf will trigger the entrance into portal space
            SendSelf();
            SendFriendStatusUpdates();

            // Init the client with the chat channel ID's, and then notify the player that they've choined the associated channels.
            var setTurbineChatChannels = new GameEventSetTurbineChatChannels(Session, 0, 1, 2, 3, 4, 6, 7, 0, 0, 0); // TODO these are hardcoded right now
            var general = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "General");
            var trade = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "Trade");
            var lfg = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "LFG");
            var roleplay = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "Roleplay");
            Session.Network.EnqueueSend(setTurbineChatChannels, general, trade, lfg, roleplay);
        }

        private void AddCharacterBaseModelData()
        {
            // Hair/head
            AddModel(0x10, Character.HeadObject);
            AddTexture(0x10, Character.DefaultHairTexture, Character.HairTexture);
            AddPalette(Character.HairPalette, 0x18, 0x8);

            // Skin
            PaletteGuid = Character.PaletteId;
            AddPalette(Character.SkinPalette, 0x0, 0x18);

            // Eyes
            AddTexture(0x10, Character.DefaultEyesTexture, Character.EyesTexture);
            AddPalette(Character.EyesPalette, 0x20, 0x8);

            // Nose & Mouth
            AddTexture(0x10, Character.DefaultNoseTexture, Character.NoseTexture);
            AddTexture(0x10, Character.DefaultMouthTexture, Character.MouthTexture);
        }

        public AceObject GetSavableCharacter()
        {
            // Clone Character
            AceObject obj = (AceObject)Character.Clone();
            // TODO: Fix this hack - not sure where but weenieclassid is getting set to 0 has to be 1 for players
            // this is crap and needs to be fixed.
            obj.WeenieClassId = 1;

            return obj;
        }

        /// <summary>
        /// Raise the available XP by a specified amount
        /// </summary>
        /// <param name="amount">A unsigned long containing the desired XP amount to raise</param>
        public void GrantXp(ulong amount)
        {
            // until we are max level we must make sure that we send
            XpTable xpTable = XpTable.ReadFromDat();
            var chart = xpTable.LevelingXpChart;
            CharacterLevel maxLevel = chart.Levels.Last();
            if (Character.Level != maxLevel.Level)
            {
                ulong amountLeftToEnd = maxLevel.TotalXp - Character.TotalExperience;
                if (amount > amountLeftToEnd)
                {
                    amount = amountLeftToEnd;
                }
                Character.GrantXp(amount);
                CheckForLevelup();
                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.TotalExperience, Character.TotalExperience);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, Character.AvailableExperience);
                var message = new GameMessageSystemChat($"{amount} experience granted.", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate, message);
            }
        }

        /// <summary>
        /// Public method for adding a new skill by spending skill credits.
        /// </summary>
        /// <remarks>
        ///  The client will throw up more then one train skill dialog and the user has the chance to spend twice.
        /// </remarks>
        /// <param name="skill"></param>
        /// <param name="creditsSpent"></param>
        public void TrainSkill(Skill skill, uint creditsSpent)
        {
            if (Character.AvailableSkillCredits >= creditsSpent)
            {
                // attempt to train the specified skill
                bool trainNewSkill = Character.TrainSkill(skill, creditsSpent);

                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, Character.AvailableSkillCredits);

                // as long as the skill is sent, the train new triangle button on the client will not lock up.
                // Sending Skill.None with status untrained worked in test
                var trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, Skill.None, SkillStatus.Untrained, 0, 0, 0);
                // create a string placeholder for the correct after
                string trainSkillMessageText = "";

                // if the skill has already been trained or we do not have enough credits, then trainNewSkill be set false
                if (trainNewSkill)
                {
                    // replace the trainSkillUpdate message with the correct skill assignment:
                    trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, SkillStatus.Trained, 0, 0, 0);
                    trainSkillMessageText = $"{SkillExtensions.ToSentence(skill)} trained. You now have {Character.AvailableSkillCredits} credits available.";
                }
                else
                {
                    trainSkillMessageText = $"Failed to train {SkillExtensions.ToSentence(skill)}! You now have {Character.AvailableSkillCredits} credits available.";
                }

                // create the final game message and send to the client
                var message = new GameMessageSystemChat(trainSkillMessageText, ChatMessageType.Advancement);
                Session.Network.EnqueueSend(trainSkillUpdate, currentCredits, message);
            }
        }

        /// <summary>
        /// Determines if the player has advanced a level
        /// </summary>
        /// <remarks>
        /// Known issues:
        ///         1. XP updates from outside of the grantxp command have not been done yet.
        /// </remarks>
        private void CheckForLevelup()
        {
            // Question: Where do *we* call CheckForLevelup()? :
            //      From within the player.cs file, the options might be:
            //           GrantXp()
            //      From outside of the player.cs file, we may call CheckForLevelup() durring? :
            //           XP Updates?
            var startingLevel = Character.Level;
            XpTable xpTable = XpTable.ReadFromDat();
            var chart = xpTable.LevelingXpChart;
            CharacterLevel maxLevel = chart.Levels.Last();
            bool creditEarned = false;
            if (Character.Level == maxLevel.Level) return;

            // increases until the correct level is found
            while (chart.Levels[Convert.ToInt32(Character.Level)].TotalXp <= Character.TotalExperience)
            {
                Character.Level++;
                CharacterLevel newLevel = chart.Levels.FirstOrDefault(item => item.Level == Character.Level);
                // increase the skill credits if the chart allows this level to grant a credit
                if (newLevel.GrantsSkillPoint)
                {
                    Character.AvailableSkillCredits++;
                    Character.TotalSkillCredits++;
                    creditEarned = true;
                }
                // break if we reach max
                if (Character.Level == maxLevel.Level)
                {
                    PlayParticleEffect(Network.Enum.PlayScript.WeddingBliss, Guid);
                    break;
                }
            }

            if (Character.Level > startingLevel)
            {
                string level = $"{Character.Level}";
                string skillCredits = $"{Character.AvailableSkillCredits}";
                string xpAvailable = $"{Character.AvailableExperience:#,###0}";
                var levelUp = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Level, Character.Level);
                string levelUpMessageText = (Character.Level == maxLevel.Level) ? $"You have reached the maximum level of {level}!" : $"You are now level {level}!";
                var levelUpMessage = new GameMessageSystemChat(levelUpMessageText, ChatMessageType.Advancement);
                string xpUpdateText = (Character.AvailableSkillCredits > 0) ? $"You have {xpAvailable} experience points and {skillCredits} skill credits available to raise skills and attributes." : $"You have {xpAvailable} experience points available to raise skills and attributes.";
                var xpUpdateMessage = new GameMessageSystemChat(xpUpdateText, ChatMessageType.Advancement);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, Character.AvailableSkillCredits);
                if (Character.Level != maxLevel.Level && !creditEarned)
                {
                    string nextCreditAtText = $"You will earn another skill credit at {chart.Levels.Where(item => item.Level > Character.Level).OrderBy(item => item.Level).First(item => item.GrantsSkillPoint).Level}";
                    var nextCreditMessage = new GameMessageSystemChat(nextCreditAtText, ChatMessageType.Advancement);
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits, nextCreditMessage);
                }
                else
                {
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits);
                }
                // play level up effect
                PlayParticleEffect(Network.Enum.PlayScript.LevelUp, Guid);
            }
        }

        public void SpendXp(Enum.Ability ability, uint amount)
        {
            bool isSecondary = false;
            CreatureAbility creatureAbility;
            bool success = AceObject.AceObjectPropertiesAttributes.TryGetValue(ability, out creatureAbility);
            if (!success)
            {
                CreatureVital v;
                success = AceObject.AceObjectPropertiesAttributes2nd.TryGetValue(ability, out v);
                // Invalid ability
                if (!success)
                {
                    log.Error("Invalid ability passed to Player.SpendXp");
                    return;
                }
                creatureAbility = v;
                isSecondary = true;
            }
            uint baseValue = creatureAbility.Base;
            uint result = SpendAbilityXp(creatureAbility, amount);
            uint ranks = creatureAbility.Ranks;
            uint newValue = creatureAbility.UnbuffedValue;
            string messageText = "";
            if (result > 0u)
            {
                GameMessage abilityUpdate;
                if (!isSecondary)
                {
                    abilityUpdate = new GameMessagePrivateUpdateAbility(Session, ability, ranks, baseValue, result);
                }
                else
                {
                    CreatureVital vital = creatureAbility as CreatureVital;
                    abilityUpdate = new GameMessagePrivateUpdateVital(Session, ability, ranks, baseValue, result, vital.Current);
                }

                // checks if max rank is achieved and plays fireworks w/ special text
                if (IsAbilityMaxRank(ranks, isSecondary))
                {
                    // fireworks
                    PlayParticleEffect(Network.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {ability} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {ability} is now {newValue}!";
                }
                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, Character.AvailableExperience);
                var soundEvent = new GameMessageSound(this.Guid, Network.Enum.Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);

                // This seems to be needed to keep health up to date properly.
                // Needed when increasing health and endurance.
                if (ability == Enum.Ability.Endurance)
                {
                    var healthUpdate = new GameMessagePrivateUpdateVital(Session, Enum.Ability.Health, Health.Ranks, Health.Base, Health.ExperienceSpent, Health.Current);
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message, healthUpdate);
                }
                else if (ability == Enum.Ability.Self)
                {
                    var manaUpdate = new GameMessagePrivateUpdateVital(Session, Enum.Ability.Mana, Mana.Ranks, Mana.Base, Mana.ExperienceSpent, Mana.Current);
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message, manaUpdate);
                }
                else
                {
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message);
                }
            }
            else
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {ability} has failed.", ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendAbilityXp(CreatureAbility ability, uint amount)
        {
            uint result = 0;
            bool addToCurrentValue = false;
            ExperienceExpenditureChart chart;
            XpTable xpTable = XpTable.ReadFromDat();
            switch (ability.Ability)
            {
                case Enum.Ability.Health:
                case Enum.Ability.Stamina:
                case Enum.Ability.Mana:
                    chart = xpTable.VitalXpChart;
                    addToCurrentValue = true;
                    break;
                default:
                    chart = xpTable.AbilityXpChart;
                    break;
            }

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (ability.Ranks >= (chart.Ranks.Count - 1))
                return result;

            uint rankUps = 0u;
            uint currentXp = chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            uint rank1 = chart.Ranks[Convert.ToInt32(ability.Ranks) + 1].XpFromPreviousRank;
            uint rank10 = 0u;
            int rank10Offset = 0;

            if (ability.Ranks + 10 >= (chart.Ranks.Count))
            {
                rank10Offset = 10 - (Convert.ToInt32(ability.Ranks + 10) - (chart.Ranks.Count - 1));
                rank10 = chart.Ranks[Convert.ToInt32(ability.Ranks) + rank10Offset].TotalXp - chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            }
            else
            {
                rank10 = chart.Ranks[Convert.ToInt32(ability.Ranks) + 10].TotalXp - chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            }

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
            {
                if (rank10Offset > 0u)
                {
                    rankUps = Convert.ToUInt32(rank10Offset);
                }
                else
                {
                    rankUps = 10u;
                }
            }

            if (rankUps > 0)
            {
                // FIXME(ddevec): This needs to be done for vitals only? Someone verify --
                //      Really AddRank() should probably be a method of CreatureAbility/CreatureVital
                CreatureVital vital = ability as CreatureVital;
                if (vital != null)
                {
                    vital.Current += addToCurrentValue ? rankUps : 0u;
                }
                ability.Ranks += rankUps;
                ability.ExperienceSpent += amount;
                this.Character.SpendXp(amount);
                result = ability.ExperienceSpent;
            }

            return result;
        }

        /// <summary>
        /// Check a rank against the ability charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if ability is max rank; false if ability is below max rank</returns>
        private bool IsAbilityMaxRank(uint rank, bool isAbilityVitals)
        {
            ExperienceExpenditureChart xpChart = new ExperienceExpenditureChart();
            XpTable xpTable = XpTable.ReadFromDat();

            if (isAbilityVitals)
                xpChart = xpTable.VitalXpChart;
            else
                xpChart = xpTable.AbilityXpChart;

            if (rank == (xpChart.Ranks.Count - 1))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check a rank against the skill charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if skill is max rank; false if skill is below max rank</returns>
        private bool IsSkillMaxRank(uint rank, SkillStatus status)
        {
            ExperienceExpenditureChart xpChart = new ExperienceExpenditureChart();
            XpTable xpTable = XpTable.ReadFromDat();

            if (status == SkillStatus.Trained)
                xpChart = xpTable.TrainedSkillXpChart;
            else if (status == SkillStatus.Specialized)
                xpChart = xpTable.SpecializedSkillXpChart;

            if (rank == (xpChart.Ranks.Count - 1))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Spend xp Skill ranks
        /// </summary>
        public void SpendXp(Skill skill, uint amount)
        {
            uint baseValue = 0;
            CreatureSkill creatureSkill = Skills[skill];
            uint result = SpendSkillXp(creatureSkill, amount);

            uint ranks = creatureSkill.Ranks;
            uint newValue = creatureSkill.UnbuffedValue;
            var status = creatureSkill.Status;
            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, Character.AvailableExperience);
            var skillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, status, ranks, baseValue, result);
            var soundEvent = new GameMessageSound(this.Guid, Network.Enum.Sound.RaiseTrait, 1f);
            string messageText = "";

            if (result > 0u)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(ranks, status))
                {
                    // fireworks on rank up is 0x8D
                    PlayParticleEffect(Network.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {skill} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {skill} is now {newValue}!";
                }
            }
            else
            {
                messageText = $"Your attempt to raise {skill} has failed!";
            }
            var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);
            Session.Network.EnqueueSend(xpUpdate, skillUpdate, soundEvent, message);
        }

        public override ActionChain GetOnKillChain(Session killerSession)
        {
            // First do on-kill
            ActionChain onKillChain = new ActionChain(this, () => OnKill(killerSession));
            onKillChain.AddChain(base.GetOnKillChain(killerSession));

            // Send the teleport out
            onKillChain.AddAction(this, () =>
            {
                // teleport to sanctuary or best location
                Position newPosition = PositionSanctuary ?? PositionLastPortal ?? Location;

                // Enqueue a teleport action, followed by Stand-up
                // Queue the teleport to lifestone
                ActionChain teleportChain = GetTeleportChain(newPosition);

                teleportChain.AddAction(this, () =>
                {
                    // Regenerate/ressurect?
                    UpdateVitalInternal(Health, 5);

                    // Stand back up
                    DoMotion(new UniversalMotion(MotionStance.Standing));

                    // add a Corpse at the current location via the ActionQueue to honor the motion and teleport delays
                    // QueuedGameAction addCorpse = new QueuedGameAction(this.Guid.Full, corpse, true, GameActionType.ObjectCreate);
                    // AddToActionQueue(addCorpse);
                    // If the player is outside of the landblock we just died in, then reboadcast the death for
                    // the players at the lifestone.
                    if (Positions.ContainsKey(PositionType.LastOutsideDeath) && Positions[PositionType.LastOutsideDeath].Cell != newPosition.Cell)
                    {
                        string currentDeathMessage = $"died to {killerSession.Player.Name}.";
                        ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerSession.Player.Guid);
                    }
                });
                teleportChain.EnqueueChain();
            });

            return onKillChain;
        }

        /// <summary>
        /// Player Death/Kill, use this to kill a session's player
        /// </summary>
        /// <remarks>
        ///     TODO:
        ///         1. Find the best vitae formula and add vitae
        ///         2. Generate the correct death message, or have it passed in as a parameter.
        ///         3. Find the correct player death noise based on the player model and play on death.
        ///         4. Determine if we need to Send Queued Action for Lifestone Materialize, after Action Location.
        ///         5. Find the health after death formula and Set the correct health
        /// </remarks>
        private void OnKill(Session killerSession)
        {
            ObjectGuid killerId = killerSession.Player.Guid;

            IsAlive = false;
            Health.Current = 0; // Set the health to zero
            Character.NumDeaths++; // Increase the NumDeaths counter
            Character.DeathLevel++; // Increase the DeathLevel

            // TODO: Find correct vitae formula/value
            Character.VitaeCpPool = 0; // Set vitae

            // TODO: Generate a death message based on the damage type to pass in to each death message:
            string currentDeathMessage = $"died to {killerSession.Player.Name}.";

            // Send Vicitim Notification, or "Your Death" event to the client:
            // create and send the client death event, GameEventYourDeath
            var msgYourDeath = new GameEventYourDeath(Session, $"You have {currentDeathMessage}");
            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Health, Health.Current);
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.NumDeaths, Character.NumDeaths);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.DeathLevel, Character.DeathLevel);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.VitaeCpPool, Character.VitaeCpPool);
            var msgPurgeEnchantments = new GameEventPurgeAllEnchantments(Session);
            // var msgDeathSound = new GameMessageSound(Guid, Sound.Death1, 1.0f);

            // Send first death message group
            Session.Network.EnqueueSend(msgHealthUpdate, msgYourDeath, msgNumDeaths, msgDeathLevel, msgVitaeCpPool, msgPurgeEnchantments);

            // Broadcast the 019E: Player Killed GameMessage
            ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerId);
        }

        public void HandleActionExamination(ObjectGuid examinationId)
        {
            // TODO: Throttle this request?. The live servers did this, likely for a very good reason, so we should, too.
            ActionChain examineChain = new ActionChain();

            // The object can be in two spots... on the player or on the landblock
            // First check the player
            examineChain.AddAction(this, () =>
            {
                WorldObject wo = GetInventoryItem(examinationId);
                if (wo != null)
                {
                    wo.Examine(Session);
                }
                else
                {
                    ActionChain chain = new ActionChain();
                    CurrentLandblock.ChainOnObject(chain, examinationId, (WorldObject cwo) =>
                    {
                        cwo.Examine(Session);
                    });
                    chain.EnqueueChain();
                }
            });
            examineChain.EnqueueChain();
        }

        public void HandleActionQueryHealth(ObjectGuid queryId)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () =>
            {
                if (queryId.Full == 0)
                {
                    // Deselect the formerly selected Target
                    selectedTarget = ObjectGuid.Invalid;
                    return;
                }

                // Remember the selected Target
                selectedTarget = queryId;

                // TODO: once items are implemented check if there are items that can trigger
                //       the QueryHealth event. So far I believe it only gets triggered for players and creatures
                if (queryId.IsPlayer() || queryId.IsCreature())
                {
                    // If we're on a landblock, id health, otherwise ignore
                    if (CurrentLandblock != null)
                    {
                        ActionChain idChain = new Actions.ActionChain();
                        CurrentLandblock.ChainOnObject(idChain, queryId, (WorldObject wo) =>
                        {
                            float healthPercentage = 1f;

                            if (queryId.IsPlayer())
                            {
                                Player tmpTarget = (Player)wo;
                                healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
                            }
                            if (queryId.IsCreature())
                            {
                                Creature tmpTarget = (Creature)wo;
                                healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
                            }
                            var updateHealth = new GameEventUpdateHealth(Session, queryId.Full, healthPercentage);
                            Session.Network.EnqueueSend(updateHealth);
                        });
                        idChain.EnqueueChain();
                    }
                }
            });
            chain.EnqueueChain();
        }

        // FIXME(ddevec): Reintroduce after getting vendor code stuck in.
        /*
        public void HandleActionBuy(ObjectGuid vendorId, List<ItemProfile> items)
        {
            new ActionChain(this, () =>
            {
                // todo: lots, need vendor list, money checks, etc.

                var money = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CoinValue, 4000);
                var sound = new GameMessageSound(Guid, Sound.PickUpItem, 1);
                var sendUseDoneEvent = new GameEventUseDone(Session);
                Session.Network.EnqueueSend(money, sound, sendUseDoneEvent);

                // send updated vendor inventory.
                Session.Network.EnqueueSend(new GameEventApproachVendor(Session, vendorId));

                // this is just some testing code for now.
                foreach (ItemProfile item in items)
                {
                    // todo: something with vendor id and profile list... iid list from vendor dbs.
                    // todo: something with amounts..

                    if (item.Iid == 5)
                    {
                        while (item.Amount > 0)
                        {
                            item.Amount--;
                            WorldObject loot = LootGenerationFactory.CreateTestWorldObject(5090);
                            AddToInventory(loot);
                            TrackObject(loot);
                        }
                        var rudecomment = "Who do you think you are, Johny Apple Seed ?";
                        var buyrudemsg = new GameMessageSystemChat(rudecomment, ChatMessageType.Tell);
                        Session.Network.EnqueueSend(buyrudemsg);
                    }
                    else if (item.Iid == 10)
                    {
                        while (item.Amount > 0)
                        {
                            item.Amount--;
                            WorldObject loot = LootGenerationFactory.CreateTestWorldObject(30537);
                            AddToInventory(loot);
                            TrackObject(loot);
                        }
                        var rudecomment = "That smells awful, Enjoy eating it!";
                        var buyrudemsg = new GameMessageSystemChat(rudecomment, ChatMessageType.Tell);
                        Session.Network.EnqueueSend(buyrudemsg);
                    }
                }
            }).EnqueueChain();
        }
        */

        public void HandleAddToInventory(WorldObject wo)
        {
            new ActionChain(this, () =>
            {
                AddToInventory(wo);
                TrackObject(wo);
                UpdatePlayerBurden();
            }).EnqueueChain();
        }

        /// <summary>
        /// Sends a death message broadcast all players on the landblock? that a killer has a victim
        /// </summary>
        /// <remarks>
        /// TODO:
        ///     1. Figure out who all receieves death messages, on what landblock and at what order -
        ///         Example: Does the players around the vicitm receive the message or do the players at the lifestone receieve the message, or both?
        /// </remarks>
        /// <param name="deathMessage"></param>
        /// <param name="victimId"></param>
        public void ActionBroadcastKill(string deathMessage, ObjectGuid victimId, ObjectGuid killerId)
        {
            var deathBroadcast = new GameMessagePlayerKilled(deathMessage, victimId, killerId);
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.OutdoorChatRange, deathBroadcast);
        }

        // Play a sound
        public void PlaySound(Sound sound, ObjectGuid targetId)
        {
            Session.Network.EnqueueSend(new GameMessageSound(targetId, sound, 1f));
        }

        // plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(PlayScript effectId, ObjectGuid targetId)
        {
            if (CurrentLandblock != null)
            {
                var effectEvent = new GameMessageScript(targetId, effectId);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, effectEvent);
            }
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Not checking and accounting for XP gained from skill usage.
        /// </remarks>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendSkillXp(CreatureSkill skill, uint amount)
        {
            uint result = 0u;
            ExperienceExpenditureChart chart;
            XpTable xpTable = XpTable.ReadFromDat();

            if (skill.Status == SkillStatus.Trained)
                chart = xpTable.TrainedSkillXpChart;
            else if (skill.Status == SkillStatus.Specialized)
                chart = xpTable.SpecializedSkillXpChart;
            else
                return result;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (skill.Ranks >= (chart.Ranks.Count - 1))
                return result;

            uint rankUps = 0u;
            uint currentXp = chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            uint rank1 = chart.Ranks[Convert.ToInt32(skill.Ranks) + 1].XpFromPreviousRank;
            uint rank10 = 0u;
            int rank10Offset = 0;

            if (skill.Ranks + 10 >= (chart.Ranks.Count))
            {
                rank10Offset = 10 - (Convert.ToInt32(skill.Ranks + 10) - (chart.Ranks.Count - 1));
                rank10 = chart.Ranks[Convert.ToInt32(skill.Ranks) + rank10Offset].TotalXp - chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            }
            else
            {
                rank10 = chart.Ranks[Convert.ToInt32(skill.Ranks) + 10].TotalXp - chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            }

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
            {
                if (rank10Offset > 0u)
                {
                    rankUps = Convert.ToUInt32(rank10Offset);
                }
                else
                {
                    rankUps = 10u;
                }
            }

            if (rankUps > 0)
            {
                skill.Ranks += rankUps;
                skill.ExperienceSpent += amount;
                this.Character.SpendXp(amount);
                result = skill.ExperienceSpent;
            }

            return result;
        }

        /// <summary>
        /// Will send out GameEventFriendsListUpdate packets to everyone online that has this player as a friend.
        /// </summary>
        private void SendFriendStatusUpdates()
        {
            List<Session> inverseFriends = WorldManager.FindInverseFriends(Guid);

            if (inverseFriends.Count > 0)
            {
                Friend playerFriend = new Friend();
                playerFriend.Id = Guid;
                playerFriend.Name = Name;
                foreach (var friendSession in inverseFriends)
                {
                    friendSession.Network.EnqueueSend(new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, GetVirtualOnlineStatus()));
                }
            }
        }

        /// <summary>
        /// Adds a friend and updates the database.
        /// </summary>
        /// <param name="friendName">The name of the friend that is being added.</param>
        public void AddFriend(string friendName)
        {
            if (string.Equals(friendName, Name, StringComparison.CurrentCultureIgnoreCase))
                ChatPacket.SendServerMessage(Session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);

            // Check if friend exists
            if (Character.Friends.SingleOrDefault(f => string.Equals(f.Name, friendName, StringComparison.CurrentCultureIgnoreCase)) != null)
                ChatPacket.SendServerMessage(Session, "That character is already in your friends list", ChatMessageType.Broadcast);

            // TODO: check if player is online first to avoid database hit??
            // Get character record from DB
            DatabaseManager.Shard.GetObjectInfoByName(friendName, ((ObjectInfo friendInfo) =>
            {
                if (friendInfo == null)
                {
                    ChatPacket.SendServerMessage(Session, "That character does not exist", ChatMessageType.Broadcast);
                    return;
                }

                Friend newFriend = new Friend();
                newFriend.Name = friendInfo.Name;
                newFriend.Id = new ObjectGuid(friendInfo.Guid, GuidType.Player);

                // Save to DB, assume success
                DatabaseManager.Shard.AddFriend(Guid.Low, newFriend.Id.Low, (() =>
                {
                    // Add to character object
                    Character.AddFriend(newFriend);

                    // Send packet
                    Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));
                }));
            }));
        }

        /// <summary>
        /// Remove a single friend and update the database.
        /// </summary>
        /// <param name="friendId">The ObjectGuid of the friend that is being removed</param>
        public void RemoveFriend(ObjectGuid friendId)
        {
            Friend friendToRemove = Character.Friends.SingleOrDefault(f => f.Id.Low == friendId.Low);

            // Not in friend list
            if (friendToRemove == null)
            {
                ChatPacket.SendServerMessage(Session, "That chracter is not in your friends list!", ChatMessageType.Broadcast);
                return;
            }

            // Remove from DB
            DatabaseManager.Shard.DeleteFriend(Guid.Low, friendId.Low, (() =>
            {
                // Remove from character object
                Character.RemoveFriend(friendId.Low);

                // Send packet
                Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));
            }));
        }

        /// <summary>
        /// Delete all friends and update the database.
        /// </summary>
        public void RemoveAllFriends()
        {
            // Remove all from DB
            DatabaseManager.Shard.RemoveAllFriends(Guid.Low, null);

            // Remove from character object
            Character.RemoveAllFriends();
        }

        /// <summary>
        /// Set the AppearOffline option to the provided value.  It will also send out an update to all online clients that have this player as a friend. This option does not save to the database.
        /// </summary>
        public void AppearOffline(bool appearOffline)
        {
            SetCharacterOption(CharacterOption.AppearOffline, appearOffline);
            SendFriendStatusUpdates();
        }

        /// <summary>
        /// Set a single character option to the provided value. This does not save to the database.
        /// </summary>
        public void SetCharacterOption(CharacterOption option, bool value)
        {
            Character.SetCharacterOption(option, value);
        }

        /// <summary>
        /// Set the currenly position of the character, to later save in the database.
        /// </summary>
        public void SetPhysicalCharacterPosition()
        {
            // Saves the current player position after converting from a Position Object, to a CharacterPosition object
            SetCharacterPosition(PositionType.Location, Session.Player.Location);
        }

        /// <summary>
        /// Saves a CharacterPosition to the character position dictionary
        /// </summary>
        public void SetCharacterPosition(PositionType type, Position newPosition)
        {
            // reset the landblock id
            if (newPosition.LandblockId.Landblock == 0 && newPosition.Cell > 0)
            {
                newPosition.LandblockId = new LandblockId(newPosition.Cell);
            }

            Positions[type] = newPosition;
        }

        // Just preps the character to save
        public void HandleActionSaveCharacter()
        {
            GetSaveChain().EnqueueChain();
        }

        // Gets the ActionChain to save a character
        public ActionChain GetSaveChain()
        {
            return new ActionChain(this, SaveCharacter);
        }

        /// <summary>
        /// Internal save character functionality
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.
        /// </summary>
        private void SaveCharacter()
        {
            if (Character != null)
            {
                // Save the current position to persistent storage, only durring the server update interval
                SetPhysicalCharacterPosition();
                // DatabaseManager.Shard.SaveObject(GetSavableCharacter());

                DatabaseManager.Shard.SaveObject(GetSavableCharacter(), null);
#if DEBUG
                if (Session.Player != null)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{Session.Player.Name} has been saved.", ChatMessageType.Broadcast));
                }
#endif
            }
        }

        public void UpdateAge()
        {
            if (Character != null)
                Character.Age++;
        }

        public void SendAgeInt()
        {
            try
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Age, Character.Age));
            }
            catch (NullReferenceException)
            {
                // Do Nothing since player data hasn't loaded in
            }
        }

        /// <summary>
        /// Returns false if the player has chosen to Appear Offline.  Otherwise it will return their actual online status.
        /// </summary>
        public bool GetVirtualOnlineStatus()
        {
            if (Character.CharacterOptions[CharacterOption.AppearOffline] == true)
                return false;

            return IsOnline;
        }

        private void SendSelf()
        {
            Session.Network.EnqueueSend(new GameMessageCreateObject(this), new GameMessagePlayerCreate(Guid));
            // TODO: gear and equip

            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);
        }

        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsState = state;

            if (packet)
            {
                // TODO: this should be broadcast
                if (CurrentLandblock != null)
                {
                    GameMessage msg = new GameMessageSetState(this, state);
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
                }
            }
        }

        public void Teleport(Position newPosition)
        {
            ActionChain chain = GetTeleportChain(newPosition);
            chain.EnqueueChain();
        }

        private ActionChain GetTeleportChain(Position newPosition)
        {
            ActionChain teleportChain = new ActionChain();

            teleportChain.AddAction(this, () => TeleportInternal(newPosition));

            teleportChain.AddDelaySeconds(3);
            // Once back in world we can start listening to the game's request for positions
            teleportChain.AddAction(this, () =>
            {
                InWorld = true;
            });

            return teleportChain;
        }

        private void TeleportInternal(Position newPosition)
        {
            if (!InWorld)
                return;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide);
            ExternalUpdatePosition(newPosition);
            InWorld = false;

            Session.Network.EnqueueSend(new GameMessagePlayerTeleport(this));

            lock (clientObjectList)
            {
                clientObjectList.Clear();
            }
        }

        public void RequestUpdatePosition(Position pos)
        {
            new ActionChain(this, () => ExternalUpdatePosition(pos)).EnqueueChain();
        }

        public void RequestUpdateMotion(uint holdKey, MovementData md, MotionItem[] commands)
        {
            new ActionChain(this, () =>
            {
                // Update our current style
                if ((md.MovementStateFlag & MovementStateFlag.CurrentStyle) != 0)
                {
                    MotionStance newStance = (MotionStance)md.CurrentStyle;
                    if (newStance != stance)
                    {
                        stance = (MotionStance)md.CurrentStyle;
                    }
                }

                md = md.ConvertToClientAccepted(holdKey, Skills[Skill.Run]);

                UniversalMotion newMotion = new UniversalMotion(stance, md);
                // FIXME(ddevec): May need to de-dupe animation/commands from client -- getting multiple (e.g. wave)
                // FIXME(ddevec): This is the operation that should update our velocity (for physics later)
                newMotion.Commands.AddRange(commands);

                CurrentLandblock.EnqueueBroadcastMotion(this, newMotion);
            }).EnqueueChain();
        }

        private void ExternalUpdatePosition(Position newPosition)
        {
            if (InWorld)
            {
                PrepUpdatePosition(newPosition);
            }
        }

        public void SetTitle(uint title)
        {
            var updateTitle = new GameEventUpdateTitle(Session, title);
            var message = new GameMessageSystemChat($"Your title is now {title}!", ChatMessageType.Broadcast);
            Session.Network.EnqueueSend(updateTitle, message);
        }

        /// <summary>
        /// returns a list of the ObjectGuids of all known creatures
        /// </summary>
        private List<ObjectGuid> GetKnownCreatures()
        {
            lock (clientObjectList)
            {
                return (List<ObjectGuid>)clientObjectList.Select(x => x.Key).Where(o => o.IsCreature()).ToList();
            }
        }

        public void UpdatePlayerBurden()
        {
            Session.Player.Burden = UpdateBurden();
            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(
                                            Session,
                                            PropertyInt.EncumbranceVal,
                                            Session.Player.Burden ?? 0u));
        }

        /// <summary>
        /// forces either an update or a create object to be sent to the client
        /// </summary>
        public void TrackObject(WorldObject worldObject)
        {
            bool sendUpdate = true;

            if (worldObject.Guid == this.Guid)
                return;

            lock (clientObjectList)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);

                if (!sendUpdate)
                {
                    clientObjectList.Add(worldObject.Guid, WorldManager.PortalYearTicks);
                    worldObject.PlayScript(this.Session);
                }
                else
                {
                    clientObjectList[worldObject.Guid] = WorldManager.PortalYearTicks;
                }
            }

            log.Debug($"Telling {Name} about {worldObject.Name} - {worldObject.Guid.Full:X}");

            if (sendUpdate)
            {
                // Session.Network.EnqueueSend(new GameMessageUpdateObject(worldObject));
                // TODO: Better handling of sending updates to client. The above line is causing much more problems then it is solving until we get proper movement.
                // Add this or something else back in when we handle movement better, until then, just send the create object once and move on.
            }
            else
            {
                Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));
                if (worldObject.DefaultScript != null)
                    Session.Network.EnqueueSend(new GameMessageScript(Guid, (PlayScript)worldObject.DefaultScript));
            }
        }

        public void HandleActionLogout(bool clientSessionTerminatedAbruptly = false)
        {
            GetLogoutChain().EnqueueChain();
        }

        public ActionChain GetLogoutChain(bool clientSessionTerminatedAbruptly = false)
        {
            ActionChain logoutChain = new ActionChain(this, () => LogoutInternal(clientSessionTerminatedAbruptly));

            // FIXME(ddevec): Constant time here for animation...
            logoutChain.AddDelaySeconds(2);

            // remove the player from landblock management -- after the animation has run
            logoutChain.AddChain(CurrentLandblock.GetRemoveWorldObjectChain(Guid, false));

            return logoutChain;
        }

        /// <summary>
        /// Do the player log out work.<para />
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </summary>
        private void LogoutInternal(bool clientSessionTerminatedAbruptly)
        {
            if (!IsOnline)
                return;

            InWorld = false;
            IsOnline = false;

            SendFriendStatusUpdates();

            if (!clientSessionTerminatedAbruptly)
            {
                var logout = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LogOut));
                CurrentLandblock.EnqueueBroadcastMotion(this, logout);

                SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);

                // Thie retail server sends a ChatRoomTracker 0x0295 first, then the status message, 0x028B. It does them one at a time for each individual channel.
                // The ChatRoomTracker message doesn't seem to change at all.
                // For the purpose of ACE, we simplify this process.
                var general = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "General");
                var trade = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "Trade");
                var lfg = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "LFG");
                var roleplay = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "Roleplay");
                Session.Network.EnqueueSend(general, trade, lfg, roleplay);
            }
        }

        public void StopTrackingObject(WorldObject worldObject, bool remove)
        {
            bool sendUpdate = true;
            lock (clientObjectList)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);
                if (sendUpdate)
                {
                    clientObjectList.Remove(worldObject.Guid);
                }
            }

            // Don't remove it if it went into our inventory...
            if (sendUpdate && remove)
            {
                Session.Network.EnqueueSend(new GameMessageRemoveObject(worldObject));
            }
        }

        public void SendAutonomousPosition()
        {
            // Session.Network.EnqueueSend(new GameMessageAutonomousPosition(this));
        }

        public void HandleActionTeleToPosition(PositionType position, Action onError = null)
        {
            GetTeleToPositionChain(position, onError).EnqueueChain();
        }

        public ActionChain GetTeleToPositionChain(PositionType position, Action onError)
        {
            ActionChain teleChain = new ActionChain();
            teleChain.AddAction(this, () =>
            {
                if (Positions.ContainsKey(position))
                {
                    Position dest = Positions[position];
                    Teleport(dest);
                }
                else
                {
                    if (onError != null)
                    {
                        onError();
                    }
                }
            });
            return teleChain;
        }

        public void HandleActionTeleToLifestone()
        {
            ActionChain lifestoneChain = new ActionChain();

            // Handle lifestone internal must decide what to do next...
            lifestoneChain.AddAction(this, new ActionEventDelegate(() => HandleActionTeleToLifestoneInternal()));

            lifestoneChain.EnqueueChain();
        }

        private void HandleActionTeleToLifestoneInternal()
        {
            if (Positions.ContainsKey(PositionType.Sanctuary))
            {
                // session.Player.Teleport(session.Player.Positions[PositionType.Sanctuary]);
                string msg = $"{Name} is recalling to the lifestone.";

                var sysChatMessage = new GameMessageSystemChat(msg, ChatMessageType.Recall);

                // FIXME(ddevec): I should probably make a better interface for this
                UpdateVitalInternal(Mana, Mana.Current / 2);

                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CombatMode, 1);

                var motionLifestoneRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LifestoneRecall));

                // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
                // FIX: Recall text isn't being broadcast yet, need to address
                Session.Network.EnqueueSend(updateCombatMode);
                CurrentLandblock.EnqueueBroadcastMotion(this, motionLifestoneRecall);
                DoMotion(motionLifestoneRecall);

                // FIXME(ddevec): How long is animation? we currently just wait 14 seconds
                // Wait for animation
                ActionChain lifestoneChain = new ActionChain();
                lifestoneChain.AddDelaySeconds(14);

                // Then do teleport
                lifestoneChain.AddChain(GetTeleportChain(Positions[PositionType.Sanctuary]));

                lifestoneChain.EnqueueChain();
            }
            else
            {
                ChatPacket.SendServerMessage(Session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }

        public void HandleActionTeleToMarketplace()
        {
            ActionChain mpChain = new ActionChain();
            mpChain.AddAction(this, () => HandleActionTeleToMarketplaceInternal());

            // TODO(ddevec): Read actual animation times?
            mpChain.AddDelaySeconds(14);

            // Then do teleport
            mpChain.AddChain(GetTeleportChain(MarketplaceDrop));

            // Set the chain to run
            mpChain.EnqueueChain();
        }

        private void HandleActionTeleToMarketplaceInternal()
        {
            string message = $"{Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CombatMode, 1);

            var motionMarketplaceRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

            var animationEvent = new GameMessageUpdateMotion(Guid,
                                                             Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                                             Sequences, motionMarketplaceRecall);

            // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
            // FIX: Recall text isn't being broadcast yet, need to address
            Session.Network.EnqueueSend(updateCombatMode, sysChatMessage);
            DoMotion(motionMarketplaceRecall);
        }

        public void HandleActionMotion(UniversalMotion motion)
        {
            if (CurrentLandblock != null)
            {
                DoMotion(motion);
            }
        }

        private void DoMotion(UniversalMotion motion)
        {
            CurrentLandblock.EnqueueBroadcastMotion(this, motion);
        }

        protected override void SendUpdatePosition()
        {
            base.SendUpdatePosition();
            GameMessage msg = new GameMessageUpdatePosition(this);
            Session.Network.EnqueueSend(msg);
        }

        private void HandleUnequip(Container container, WorldObject item, uint location, ActionChain inContainerChain)
        {
            UpdateAppearance(container, item.Guid.Full);
            if (item.CurrentWieldedLocation > EquipMask.RingLeft)
            {
                // We are coming from a weapon, wand or shield slot.
                Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));
                item.Parent = null;
                // Magic numbers - need to understand and fix.
                item.ParentLocation = null;
                item.CurrentWieldedLocation = null;
                inContainerChain.AddAction(this, () => CurrentLandblock.EnqueueBroadcast(Location,
                    Landblock.MaxObjectRange, new GameMessageRemoveObject(item)));
                item.Location = null;
                item.ContainerId = container.Guid.Full;
                item.SetPhysicsDescriptionFlag(item);
            }
            else
            {
                item.Parent = null;
                item.ParentLocation = null;
                item.CurrentWieldedLocation = null;
                item.Location = null;
                item.ContainerId = container.Guid.Full;
                item.SetPhysicsDescriptionFlag(item);
            }

            inContainerChain.AddDelaySeconds(0.001);
            inContainerChain.AddAction(this, () => Session.Network.EnqueueSend(new GameMessageCreateObject(item),
                new GameMessageUpdateInstanceId(item.Guid, container.Guid, PropertyInstanceId.Container),
                new GameMessageUpdateInstanceId(container.Guid, new ObjectGuid(0), PropertyInstanceId.Wielder),
                new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CurrentWieldedLocation, 0),
                new GameMessageObjDescEvent(this),
                new GameMessageSound(Guid, Sound.UnwieldObject, (float)1.0)));

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                new GameMessageObjDescEvent(this),
                                                new GameMessagePutObjectInContainer(Session, container.Guid, item, location));
        }

        private void HandleMove(WorldObject item, Container container, uint location)
        {
            // If this is not just a in container move - ie just moving inside the same pack, lets move the inventory
            if (item.ContainerId != container.Guid.Full)
            {
                Container previousContainer = null;
                if (item.ContainerId != null)
                {
                    var previousContainerGuid = new ObjectGuid((uint)item.ContainerId);
                    if (previousContainerGuid == Guid)
                        previousContainer = this;
                    else
                    {
                        previousContainer = (Container)GetInventoryItem(previousContainerGuid);
                    }
                }
                if (previousContainer != null)
                    previousContainer.RemoveFromInventory(item.Guid);
                container.AddToInventory(item);
                item.ContainerId = container.Guid.Full;
            }
            Session.Network.EnqueueSend(new GameMessagePutObjectInContainer(Session, container.Guid, item, location),
                                        new GameMessageUpdateInstanceId(item.Guid, container.Guid, PropertyInstanceId.Container));
        }

        private void HandlePickupItem(Container container, ObjectGuid itemGuid, uint location, PropertyInstanceId iidPropertyId)
        {
            // Logical operations:
            // !! FIXME: How to handle repeat on condition?
            // while (!objectInRange)
            //   try Move to object
            // !! FIXME: How to handle conditional
            // Try acquire from landblock
            // if acquire successful:
            //   add to container
            ActionChain pickUpItemChain = new ActionChain();

            // Move to the object
            pickUpItemChain.AddChain(CreateMoveToChain(itemGuid, PickUpDistance));

            // Pick up the object
            // Start pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                var motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                    new GameMessageUpdatePosition(this),
                    new GameMessageUpdateMotion(Guid,
                        Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                        Sequences, motion));
            });
            // Wait for animation to progress
            pickUpItemChain.AddDelaySeconds(0.75);

            // Ask landblock to transfer item
            // pickUpItemChain.AddAction(CurrentLandblock, () => CurrentLandblock.TransferItem(itemGuid, containerGuid));
            if (container.Guid.IsPlayer())
                CurrentLandblock.ScheduleItemTransfer(pickUpItemChain, itemGuid, container.Guid);
            else
                CurrentLandblock.ScheduleItemTransferInContainer(pickUpItemChain, itemGuid, (Container)GetInventoryItem(container.Guid));

            // Finish pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                // If success, the item is in our inventory:
                WorldObject item = GetInventoryItem(itemGuid);

                // Update all our stuff if we succeeded
                if (item != null)
                {
                    SetInventoryForOffWorld(item);
                    // FIXME(ddevec): I'm not 100% sure which of these need to be broadcasts, and which are local sends...
                    var motion = new UniversalMotion(MotionStance.Standing);
                    if (iidPropertyId == PropertyInstanceId.Container)
                    {
                        Session.Network.EnqueueSend(
                            new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbranceVal, UpdateBurden()),
                            new GameMessageSound(Guid, Sound.PickUpItem, 1.0f),
                            new GameMessageUpdateInstanceId(itemGuid, container.Guid, iidPropertyId),
                            new GameMessagePutObjectInContainer(Session, container.Guid, item, location));
                    }
                    else
                    {
                        UpdateAppearance(container, itemGuid.Full);
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                    new GameMessageObjDescEvent(this),
                                                    new GameMessageUpdateInstanceId(container.Guid, itemGuid, PropertyInstanceId.Wielder),
                                                    new GameEventWieldItem(Session, itemGuid.Full, location));
                    }

                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences, motion),
                        new GameMessagePickupEvent(Session, item));

                    if (iidPropertyId == PropertyInstanceId.Wielder)
                        CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageObjDescEvent(this));

                    // TODO: Og II - check this later to see if it is still required.
                    Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                }
                // If we didn't succeed, just stand up and be ashamed of ourself
                else
                {
                    var motion = new UniversalMotion(MotionStance.Standing);

                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences, motion));
                    // CurrentLandblock.EnqueueBroadcast(self shame);
                }
            });

            // Set chain to run
            pickUpItemChain.EnqueueChain();
        }

        public void UpdateAppearance(Container container, uint itemId)
        {
            UpdateWieldedItem(container, itemId);
            Clear();
            AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette
            var wieldeditems = GetCurrentlyWieldedItems();
            var coverage = new List<uint>();

            foreach (var w in wieldeditems)
            {
                var wo = w.Value;
                ClothingTable item;
                if (wo.ClothingBase != null) item = ClothingTable.ReadFromDat((uint)wo.ClothingBase);
                else
                {
                    ChatPacket.SendServerMessage(Session, "We have not implemented the visual appearance for that item yet. ", ChatMessageType.AdminTell);
                    return;
                }

                if (SetupTableId != null && item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
                // Check if the player model has data. Gear Knights, this is usually you.
                {
                    // Add the model and texture(s)
                    ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
                    foreach (CloObjectEffect t in clothingBaseEffec.CloObjectEffects)
                    {
                        byte partNum = (byte)t.Index;
                        AddModel((byte)t.Index, (ushort)t.ModelId);
                        coverage.Add(partNum);
                        foreach (CloTextureEffect t1 in t.CloTextureEffects)
                            AddTexture((byte)t.Index, (ushort)t1.OldTexture, (ushort)t1.NewTexture);
                    }

                    foreach (ModelPalette p in wo.GetPalettes)
                        AddPalette(p.PaletteId, p.Offset, p.Length);
                }
            }
            // Add the "naked" body parts. These are the ones not already covered.
            if (SetupTableId != null)
            {
                SetupModel baseSetup = SetupModel.ReadFromDat((uint)SetupTableId);
                for (byte i = 0; i < baseSetup.SubObjectIds.Count; i++)
                {
                    if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head, that was already covered by AddCharacterBaseModelData()
                        AddModel(i, baseSetup.SubObjectIds[i]);
                }
            }
        }

        public void HandleActionWieldItem(Container container, uint itemId, uint location)
        {
            uint placementId = 0;
            uint childLocation = 0;
            ActionChain wieldChain = new ActionChain();
            wieldChain.AddAction(
                this,
                () =>
                    {
                        ObjectGuid itemGuid = new ObjectGuid(itemId);
                        if (GetInventoryItem(itemGuid) == null)
                        {
                            HandlePickupItem(container, itemGuid, location, PropertyInstanceId.Wielder);
                            return;
                        }

                        // Ok do you have this - if not bail
                        var item = GetInventoryItem(itemGuid);
                        if (item == null)
                            return;
                        if ((EquipMask)location > EquipMask.RingLeft)
                        {
                            // We are going into a weapon, wand or shield slot.
                            switch ((EquipMask)location)
                            {
                                case EquipMask.MissileWeapon:
                                    {
                                        childLocation = 2;
                                        placementId = 3;
                                        break;
                                    }
                                case EquipMask.Shield:
                                    {
                                        childLocation = 3;
                                        placementId = 6;
                                        break;
                                    }
                                default:
                                    {
                                        placementId = 1;
                                        childLocation = 1;
                                        break;
                                    }
                            }
                            Children.Add(new EquippedItem(itemGuid.Full, (EquipMask)location));
                            item.Parent = container.Guid.Full;
                            // Magic numbers - need to understand and fix.
                            item.ParentLocation = 1;
                            item.CurrentWieldedLocation = (EquipMask)location;
                            item.Location = Location;
                            item.SetPhysicsDescriptionFlag(item);
                        }
                        else
                        {
                            item.CurrentWieldedLocation = (EquipMask)location;
                            item.SetPhysicsDescriptionFlag(item);
                        }
                        UpdateAppearance(container, itemId);
                        if ((EquipMask)location == EquipMask.Ammunition)
                            Session.Network.EnqueueSend(
                                new GameEventWieldItem(Session, itemGuid.Full, location),
                                new GameMessageSound(Guid, Sound.WieldObject, (float)1.0));
                        else
                        {
                            if ((EquipMask)location > EquipMask.RingLeft)
                            {
                                Session.Network.EnqueueSend(
                                    new GameMessageCreateObject(item),
                                    new GameMessageParentEvent(Session.Player, item.Guid, childLocation, placementId),
                                    new GameEventWieldItem(Session, itemGuid.Full, location),
                                    new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                    new GameMessageUpdateInstanceId(container.Guid, new ObjectGuid(0),
                                        PropertyInstanceId.Container),
                                    new GameMessageUpdateInstanceId(container.Guid, itemGuid,
                                        PropertyInstanceId.Wielder),
                                    new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CurrentWieldedLocation,
                                        location));
                                CurrentLandblock.EnqueueBroadcast(
                                    Location,
                                    Landblock.MaxObjectRange,
                                    new GameMessageCreateObject(item),
                                    new GameMessageParentEvent(Session.Player, item.Guid, childLocation, placementId),
                                    new GameEventWieldItem(Session, itemGuid.Full, location),
                                    new GameMessageObjDescEvent(item));
                            }
                            else
                            {
                                Session.Network.EnqueueSend(
                                    new GameEventWieldItem(Session, itemGuid.Full, location),
                                    new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                    new GameMessageUpdateInstanceId(container.Guid, new ObjectGuid(0), PropertyInstanceId.Container),
                                    new GameMessageUpdateInstanceId(container.Guid, itemGuid, PropertyInstanceId.Wielder),
                                    new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CurrentWieldedLocation, location));
                                CurrentLandblock.EnqueueBroadcast(
                                    Location,
                                    Landblock.MaxObjectRange,
                                    new GameMessageObjDescEvent(this));
                            }
                        }
                    });
            wieldChain.EnqueueChain();
        }

        public void HandleActionPutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, uint location = 0)
        {
            ActionChain inContainerChain = new ActionChain();
            WorldObject inventoryItem;
            inContainerChain.AddAction(
                this,
                () =>
                    {
                        Container container;

                        if (containerGuid.IsPlayer())
                            container = this;
                        else
                        {
                            // Ok I am going into player backpack (container) with something I have somewhere
                            container = (Container)GetInventoryItem(containerGuid);
                        }

                        // is this something I already have? If not, it has to be a pickup - do the pickup and out.
                        if (GetInventoryItem(itemGuid) == null)
                        {
                            // This is a pickup into our main pack.
                            HandlePickupItem(container, itemGuid, location, PropertyInstanceId.Container);
                            return;
                        }

                        if (containerGuid.IsPlayer())
                            container = this;
                        else
                        {
                            // Ok I am going into player backpack (container) with something I have somewhere
                            container = (Container)GetInventoryItem(containerGuid);
                        }

                        // Ok, I know my container and I know I must have the inventory item so let's get it.
                        inventoryItem = GetInventoryItem(itemGuid);

                        // Was I equiped?   If so, lets take care of that and unequip
                        if (inventoryItem.Wielder != null)
                        {
                            HandleUnequip(container, inventoryItem, location, inContainerChain);
                            return;
                        }

                        // if were are still here, this needs to do a pack pack or main pack move.
                        HandleMove(inventoryItem, container, location);
                    });
            inContainerChain.EnqueueChain();
        }

        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            ActionChain dropChain = new ActionChain();

            // Goody Goody -- lets build  drop chain
            // First start drop animation
            dropChain.AddAction(this, () =>
            {
                var inventoryItem = GetInventoryItem(itemGuid);
                if (inventoryItem == null)
                    return;
                if (inventoryItem.Wielder != null)
                {
                    UpdateAppearance(this, itemGuid.Full);
                    Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                new GameMessageObjDescEvent(this),
                                                new GameMessageUpdateInstanceId(Guid, new ObjectGuid(0), PropertyInstanceId.Wielder));
                }
                SetInventoryForWorld(inventoryItem);
                RemoveFromInventory(itemGuid);
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbranceVal, (uint)Burden));

                var motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
                var clearContainer = new ObjectGuid(0);
                Session.Network.EnqueueSend(
                    new GameMessageUpdateInstanceId(itemGuid, clearContainer, PropertyInstanceId.Container));

                // Set pickup motion
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);

                // Now wait for pickupMotion to finish -- use ActionChain
                ActionChain chain = new ActionChain();

                // TODO(ddevec): Need real animation delays...
                // Wait for drop animation
                chain.AddDelaySeconds(0.75);

                // Play drop sound
                // Put item on landblock
                chain.AddAction(this, () =>
                {
                    motion = new UniversalMotion(MotionStance.Standing);
                    CurrentLandblock.EnqueueBroadcastMotion(this, motion);
                    Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.DropItem, (float)1.0),
                        new GameMessagePutObjectIn3d(Session, this, itemGuid),
                        new GameMessageUpdateInstanceId(itemGuid, clearContainer, PropertyInstanceId.Container));

                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                    inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                    inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);

                    CurrentLandblock.AddWorldObject(inventoryItem);

                    Session.Network.EnqueueSend(new GameMessageUpdateObject(inventoryItem));
                });

                chain.EnqueueChain();
            });

            dropChain.EnqueueChain();
        }

        public void HandleActionUse(ObjectGuid usedItemId)
        {
            new ActionChain(this, () =>
            {
                if (CurrentLandblock != null)
                {
                    // Just forward our action to the appropriate user...
                    ActionChain onUseChain = new ActionChain();
                    CurrentLandblock.ChainOnObject(onUseChain, usedItemId, (WorldObject wo) =>
                    {
                        UsableObject uo = wo as UsableObject;
                        Portal p = wo as Portal;

                        // FIXME: OnCollide for portals -- portals should be usable?
                        if (p != null)
                        {
                            p.OnCollide(Guid);
                        }

                        if (uo != null)
                        {
                            uo.OnUse(Guid);
                        }
                    });
                    onUseChain.EnqueueChain();
                }
            }).EnqueueChain();
        }

        public void HandleActionApplySoundEffect(Network.Enum.Sound sound)
        {
            new ActionChain(this, () => PlaySound(sound, Guid)).EnqueueChain();
        }

        public void HandleActionApplyVisualEffect(Network.Enum.PlayScript effect)
        {
            new ActionChain(this, () => PlayParticleEffect(effect, Guid)).EnqueueChain();
        }

        private ActionChain CreateMoveToChain(ObjectGuid target, float distance)
        {
            ActionChain moveToChain = new ActionChain();
            // While !at(thing) moveToThing
            ActionChain moveToBody = new ActionChain();
            moveToChain.AddAction(this, () =>
            {
                Position dest = CurrentLandblock.GetPosition(target);
                if (dest == null)
                {
                    log.Error("FIXME: Need the ability to cancel actions on error");
                    return;
                }

                OnAutonomousMove(CurrentLandblock.GetPosition(target),
                                         Sequences, MovementTypes.MoveToObject, target);
            });

            // poll for arrival every .1 seconds
            moveToBody.AddDelaySeconds(.1);

            moveToChain.AddLoop(this, () =>
            {
                bool valid;
                float outdistance;
                // Break loop if CurrentLandblock == null (we portaled or logged out), or if we arrive at the item
                if (CurrentLandblock == null)
                {
                    return false;
                }

                bool ret = !CurrentLandblock.WithinUseRadius(Guid, target, out outdistance, out valid);
                if (!valid)
                {
                    // If one of the items isn't on a landblock
                    ret = false;
                }
                return ret;
            }, moveToBody);

            return moveToChain;
        }

        public void HandleActionSmiteAllNearby()
        {
            // Create smite action chain... then send it
            new ActionChain(this, () =>
            {
                foreach (ObjectGuid toSmite in GetKnownCreatures())
                {
                    ActionChain smiteChain = new ActionChain();
                    CurrentLandblock.ChainOnObject(smiteChain, toSmite, (WorldObject wo) =>
                    {
                        Creature c = wo as Creature;
                        if (c != null)
                        {
                            c.GetOnKillChain(Session).EnqueueChain();
                        }
                    });
                    smiteChain.EnqueueChain();
                }
            }).EnqueueChain();
        }

        public void HandleActionSmiteSelected()
        {
            new ActionChain(this, () =>
            {
                if (selectedTarget != ObjectGuid.Invalid)
                {
                    var target = selectedTarget;
                    if (target.IsCreature() || target.IsPlayer())
                    {
                        HandleActionKill(target);
                    }
                }
                else
                {
                    ChatPacket.SendServerMessage(Session, "No target selected, use @smite all to kill all creatures in radar range.", ChatMessageType.Broadcast);
                }
            }).EnqueueChain();
        }

        public void TestEquipItem(Session session, uint modelId, int palOption)
        {
            // ClothingTable item = ClothingTable.ReadFromDat(0x1000002C); // Olthoi Helm
            // ClothingTable item = ClothingTable.ReadFromDat(0x10000867); // Cloak
            // ClothingTable item = ClothingTable.ReadFromDat(0x10000008); // Gloves
            // ClothingTable item = ClothingTable.ReadFromDat(0x100000AD); // Heaume
            ClothingTable item = ClothingTable.ReadFromDat(modelId);

            int palCount = 0;

            List<uint> coverage = new List<uint>(); // we'll store our fake coverage items here
            Clear();
            AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette

            if (item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
            {
                // Add the model and texture(s)
                ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
                for (int i = 0; i < clothingBaseEffec.CloObjectEffects.Count; i++)
                {
                    byte partNum = (byte)clothingBaseEffec.CloObjectEffects[i].Index;
                    AddModel((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].ModelId);
                    coverage.Add(partNum);
                    for (int j = 0; j < clothingBaseEffec.CloObjectEffects[i].CloTextureEffects.Count; j++)
                        AddTexture((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
                }

                // Apply an appropriate palette. We'll just pick a random one if not specificed--it's a surprise every time!
                // For actual equipment, these should just be stored in the ace_object palette_change table and loaded from there
                if (item.ClothingSubPalEffects.Count > 0)
                {
                    List<CloSubPalEffect> values = Enumerable.ToList(item.ClothingSubPalEffects.Values);
                    int size = item.ClothingSubPalEffects.Count;
                    palCount = size;

                    // Generate a random index if one isn't provided
                    if (palOption < 0)
                    {
                        Random rand = new Random();
                        palOption = rand.Next(size);
                    }
                    if (palOption > size)
                        palOption = size - 1;

                    CloSubPalEffect itemSubPal = values[palOption];
                    for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
                    {
                        PaletteSet itemPalSet = PaletteSet.ReadFromDat(itemSubPal.CloSubPalettes[i].PaletteSet);
                        ushort itemPal = (ushort)itemPalSet.GetPaletteID(0);

                        for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                            uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                            AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
                        }
                    }
                }

                // Add the "naked" body parts. These are the ones not already covered.
                SetupModel baseSetup = SetupModel.ReadFromDat((uint)SetupTableId);
                for (byte i = 0; i < baseSetup.SubObjectIds.Count; i++)
                {
                    if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head.
                        AddModel(i, baseSetup.SubObjectIds[i]);
                }

                var objDescEvent = new GameMessageObjDescEvent(this);
                session.Network.EnqueueSend(objDescEvent);
                ChatPacket.SendServerMessage(session, "Equipping model " + modelId.ToString("X") + ", Applying palette index " + palOption.ToString() + " of " + palCount.ToString() + ".", ChatMessageType.Broadcast);
            }
            else
            {
                // Alert about the failure
                ChatPacket.SendServerMessage(session, "Could not match that item to your character model.", ChatMessageType.Broadcast);
            }
        }

        public void HandleActionTestCorpseDrop()
        {
            new ActionChain(this, () =>
            {
                if (selectedTarget != ObjectGuid.Invalid)
                {
                    // FIXME(ddevec): This is wrong
                    var target = selectedTarget;

                    if (target.IsCreature())
                    {
                        HandleActionKill(target);
                    }
                }
                else
                {
                    ChatPacket.SendServerMessage(Session, "No creature selected.", ChatMessageType.Broadcast);
                }
            }).EnqueueChain();
        }

        public void HandleActionKill(ObjectGuid toSmite)
        {
            // Create smite action chain... then send it
            ActionChain smiteChain = new ActionChain();
            CurrentLandblock.ChainOnObject(smiteChain, toSmite, (WorldObject wo) =>
            {
                Creature c = wo as Creature;
                if (c != null)
                {
                    c.GetOnKillChain(Session).EnqueueChain();
                }
            });

            smiteChain.EnqueueChain();
        }

        // FIXME(ddevec): Copy pasted code for prototyping -- clean later
        protected override void VitalTickInternal(CreatureVital vital)
        {
            uint oldValue = vital.Current;
            base.VitalTickInternal(vital);

            if (vital.Current != oldValue)
            {
                // FIXME(ddevec): This is uglysauce.  CreatureVital should probably have it, but ACE.Entity doesn't seem to like importing ACE.Network.Enum
                Vital v;
                if (vital == Health)
                {
                    v = Vital.Health;
                }
                else if (vital == Stamina)
                {
                    v = Vital.Stamina;
                }
                else if (vital == Mana)
                {
                    v = Vital.Mana;
                }
                else
                {
                    log.Error("Unknown vital in UpdateVitalInternal: " + vital);
                    return;
                }

                Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(Session, v, vital.Current));
            }
        }

        protected override void UpdateVitalInternal(CreatureVital vital, uint newVal)
        {
            uint oldValue = vital.Current;
            base.UpdateVitalInternal(vital, newVal);

            if (vital.Current != oldValue)
            {
                // FIXME(ddevec): This is uglysauce.  CreatureVital should probably have it, but ACE.Entity doesn't seem to like importing ACE.Network.Enum
                Vital v;
                if (vital == Health)
                {
                    v = Vital.Health;
                }
                else if (vital == Stamina)
                {
                    v = Vital.Stamina;
                }
                else if (vital == Mana)
                {
                    v = Vital.Mana;
                }
                else
                {
                    log.Error("Unknown vital in UpdateVitalInternal: " + vital);
                    return;
                }

                Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(Session, v, vital.Current));
            }
        }

        public void WriteIdentifyObjectHeader(BinaryWriter writer, IdentifyResponseFlags flags, bool success)
        {
            writer.Write((uint)flags); // Flags
            writer.Write(Convert.ToUInt32(success)); // Success bool
        }

        public void WriteIdentifyObjectIntProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt> propertiesInt)
        {
            const ushort tableSize = 16;
            if ((flags & IdentifyResponseFlags.IntStatsTable) == 0 || (propertiesInt.Count == 0)) return;
            writer.Write((ushort)propertiesInt.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt x in propertiesInt)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue);
            }
        }

        public void WriteIdentifyObjectInt64Properties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt64> propertiesInt64)
        {
            const ushort tableSize = 8;
            if ((flags & IdentifyResponseFlags.Int64StatsTable) == 0 || (propertiesInt64.Count == 0)) return;
            writer.Write((ushort)propertiesInt64.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt64 x in propertiesInt64)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue);
            }
        }

        public void WriteIdentifyObjectBoolProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesBool> propertiesBool)
        {
            const ushort tableSize = 8;
            if ((flags & IdentifyResponseFlags.BoolStatsTable) == 0 || (propertiesBool.Count == 0)) return;
            writer.Write((ushort)propertiesBool.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesBool x in propertiesBool)
            {
                writer.Write(x.PropertyId);
                writer.Write(Convert.ToUInt32(x.PropertyValue));
            }
        }

        public void WriteIdentifyObjectDoubleProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesDouble)
        {
            const ushort tableSize = 8;
            if ((flags & IdentifyResponseFlags.FloatStatsTable) == 0 || (propertiesDouble.Count == 0)) return;
            writer.Write((ushort)propertiesDouble.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDouble x in propertiesDouble)
            {
                writer.Write((uint)x.PropertyId);
                writer.Write(x.PropertyValue);
            }
        }

        public void WriteIdentifyObjectStringsProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesString> propertiesStrings)
        {
            const ushort tableSize = 8;
            if ((flags & IdentifyResponseFlags.StringStatsTable) == 0 || (propertiesStrings.Count == 0)) return;
            writer.Write((ushort)propertiesStrings.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesString x in propertiesStrings)
            {
                writer.Write((uint)x.PropertyId);
                writer.WriteString16L(x.PropertyValue);
            }
        }

        public void WriteIdentifyObjectDidProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDataId> propertiesDid)
        {
            const ushort tableSize = 16;
            if ((flags & IdentifyResponseFlags.DidStatsTable) == 0 || (propertiesDid.Count == 0)) return;
            writer.Write((ushort)propertiesDid.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDataId x in propertiesDid)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue);
            }
        }

        public void WriteIdentifyObjectSpellIdProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesSpell> propertiesSpellId)
        {
            if ((flags & IdentifyResponseFlags.SpellBook) == 0 || (propertiesSpellId.Count == 0)) return;
            writer.Write((uint)propertiesSpellId.Count);

            foreach (AceObjectPropertiesSpell x in propertiesSpellId)
            {
                    writer.Write(x.SpellId);
            }
        }

        public void WriteIdentifyObjectArmorProfile(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesArmor)
        {
            if ((flags & IdentifyResponseFlags.ArmorProfile) == 0 || (propertiesArmor.Count == 0)) return;

            foreach (AceObjectPropertiesDouble x in propertiesArmor)
            {
                writer.Write((float)x.PropertyValue);
            }
        }

        public void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature obj)
        {
            uint header = 8;
            // TODO: for now, we are always succeeding - will need to set this to 0 header for failure.   Og II
            writer.Write(header);
            writer.Write(obj.Health.Current);
            writer.Write(obj.Health.MaxValue);
            if (header == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    writer.Write(0u);
                }
            }
            else
            {
                // TODO: we probably need buffed values here  it may be set my the last flag I don't understand yet. - will need to revisit. Og II
                writer.Write(obj.Strength.UnbuffedValue);
                writer.Write(obj.Endurance.UnbuffedValue);
                writer.Write(obj.Quickness.UnbuffedValue);
                writer.Write(obj.Coordination.UnbuffedValue);
                writer.Write(obj.Focus.UnbuffedValue);
                writer.Write(obj.Self.UnbuffedValue);
                writer.Write(obj.Stamina.UnbuffedValue);
                writer.Write(obj.Mana.UnbuffedValue);
                writer.Write(obj.Stamina.MaxValue);
                writer.Write(obj.Mana.MaxValue);
                // this only gets sent if the header can be masked with 1
                // Writer.Write(0u);
            }
        }

        public void WriteIdentifyObjectWeaponsProfile(
            BinaryWriter writer,
            IdentifyResponseFlags flags,
            List<AceObjectPropertiesDouble> propertiesWeaponsD,
            List<AceObjectPropertiesInt> propertiesWeaponsI)
        {
            if ((flags & IdentifyResponseFlags.WeaponProfile) == 0) return;
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.DamageType)?.PropertyValue ?? 0u);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.WeaponTime)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.WeaponSkill)?.PropertyValue ?? 0u);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.Damage)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageVariance)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageMod)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponLength)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.MaximumVelocity)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponOffense)?.PropertyValue ?? 0.00);
            // This one looks to be 0 - I did not find one with this calculated.   It is called Max Velocity Calculated
            writer.Write(0u);
        }
    }
}
