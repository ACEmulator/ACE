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
using log4net;
using ACE.Network.Sequence;
using ACE.Network.Motion;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using System.Diagnostics;
using ACE.Factories;

namespace ACE.Entity
{
    public sealed class Player : Creature, IPlayer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // TODO: link to Town Network marketplace portal destination in db, when db for that is finalized and implemented.
        private static readonly Position MarketplaceDrop = new Position(23855548, 49.206f, -31.935f, 0.005f, 0f, 0f, -0.7071068f, 0.7071068f); // PCAP verified drop
        private static readonly float PickUpDistance = .75f;
        private uint coinValue = 0;

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
        /// Temp tracked Objects of vendors / trade / containers.. needed for id / maybe more.
        /// </summary>
        public Dictionary<ObjectGuid, WorldObject> InteractiveWorldObjects = new Dictionary<ObjectGuid, WorldObject>();

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

        /// <summary>
        ///  The fellowship that this player belongs to
        /// </summary>
        public Fellowship Fellowship;

        // todo: Figure out if this is the best place to do this, and whether there are concurrency issues associated with it.
        public void CreateFellowship(string fellowshipName, bool shareXP)
        {
            this.Fellowship = new Fellowship(this, fellowshipName, shareXP);
        }

        private AceCharacter Character { get { return AceObject as AceCharacter; } }

        public List<AceObjectPropertiesSpellBarPositions> SpellsInSpellBars
        {
            get
            {
                return AceObject.SpellsInSpellBars;
            }
            set
            {
                AceObject.SpellsInSpellBars = value;
            }
        }

        public void SetCharacterOptions1(uint options1)
        {
            Character.CharacterOptions1Mapping = options1;
        }

        public void SetCharacterOptions2(uint options2)
        {
            Character.CharacterOptions2Mapping = options2;
        }

        public Dictionary<Skill, CreatureSkill> Skills
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

        public bool UnknownSpell(uint spellId)
        {
            return !(AceObject.SpellIdProperties.Exists(x => x.SpellId == spellId));
        }

        public void HandleActionMagicRemoveSpellId(uint spellId)
        {
            ActionChain unlearnSpellChain = new ActionChain();
            unlearnSpellChain.AddAction(
                this,
                () =>
                {
                    if (!AceObject.SpellIdProperties.Exists(x => x.SpellId == spellId))
                    {
                        log.Error("Invalid spellId passed to Player.RemoveSpellFromSpellBook");
                        return;
                    }

                    AceObject.SpellIdProperties.RemoveAt(AceObject.SpellIdProperties.FindIndex(x => x.SpellId == spellId));
                    GameEventMagicRemoveSpellId removeSpellEvent = new GameEventMagicRemoveSpellId(Session, spellId);
                    Session.Network.EnqueueSend(removeSpellEvent);
                });
            unlearnSpellChain.EnqueueChain();
        }

        public void HandleActionLearnSpell(uint spellId)
        {
            ActionChain learnSpellChain = new ActionChain();
            SpellTable spells = SpellTable.ReadFromDat();
            if (!spells.Spells.ContainsKey(spellId))
            {
                GameMessageSystemChat errorMessage = new GameMessageSystemChat("SpellID not found in Spell Table", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(errorMessage);
                return;
            }
            learnSpellChain.AddAction(this,
                () =>
                {
                    if (!UnknownSpell(spellId))
                    {
                        GameMessageSystemChat errorMessage = new GameMessageSystemChat("That spell is already known", ChatMessageType.Broadcast);
                        Session.Network.EnqueueSend(errorMessage);
                        return;
                    }
                    AceObjectPropertiesSpell newSpell = new AceObjectPropertiesSpell
                    {
                        AceObjectId = this.Guid.Full,
                        SpellId = spellId
                    };
                    AceObject.SpellIdProperties.Add(newSpell);
                    GameEventMagicUpdateSpell updateSpellEvent = new GameEventMagicUpdateSpell(Session, spellId);
                    Session.Network.EnqueueSend(updateSpellEvent);

                    // Always seems to be this SkillUpPurple effect
                    Session.Player.HandleActionApplyVisualEffect(Enum.PlayScript.SkillUpPurple);

                    string spellName = spells.Spells[spellId].Name;
                    string message = "You learn the " + spellName + " spell.\n";
                    GameMessageSystemChat learnMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                    Session.Network.EnqueueSend(learnMessage);
                });
            learnSpellChain.EnqueueChain();
        }

        /// <summary>
        /// This method implements player spell bar management for - adding a spell to a specific spell bar (0 based) at a specific slot (0 based).
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellBarPositionId"></param>
        /// <param name="spellBarId"></param>
        public void HandleActionAddSpellToSpellBar(uint spellId, uint spellBarPositionId, uint spellBarId)
        {
            // The spell bar magic happens here. First, let's mind our race conditions....
            ActionChain addSpellBarChain = new ActionChain();
            addSpellBarChain.AddAction(this, () =>
            {
                SpellsInSpellBars.Add(new AceObjectPropertiesSpellBarPositions()
                {
                    AceObjectId = AceObject.AceObjectId,
                    SpellId = spellId,
                    SpellBarId = spellBarId,
                    SpellBarPositionId = spellBarPositionId
                });
            });
            addSpellBarChain.EnqueueChain();
        }

        /// <summary>
        /// This method implements player spell bar management for - removing a spell to a specific spell bar (0 based)
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellBarId"></param>
        public void HandleActionRemoveSpellToSpellBar(uint spellId, uint spellBarId)
        {
            // More spell bar magic happens here. First, let's mind our race conditions....
            ActionChain removeSpellBarChain = new ActionChain();
            removeSpellBarChain.AddAction(this, () =>
            {
                SpellsInSpellBars.Remove(SpellsInSpellBars.Single(x => x.SpellBarId == spellBarId && x.SpellId == spellId));
                // Now I have to reorder
                var sorted = SpellsInSpellBars.FindAll(x => x.AceObjectId == AceObject.AceObjectId && x.SpellBarId == spellBarId).OrderBy(s => s.SpellBarPositionId);
                uint newSpellBarPosition = 0;
                foreach (AceObjectPropertiesSpellBarPositions spells in sorted)
                {
                    spells.SpellBarPositionId = newSpellBarPosition;
                    newSpellBarPosition++;
                }
            });
            removeSpellBarChain.EnqueueChain();
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
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelHealth, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelStamina, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelMana, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkill, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyString, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDataID, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInstanceId, new ByteSequence(false));

            // This is the default send upon log in and the most common.   Anything with a velocity will need to add that flag.
            PositionFlag |= UpdatePositionFlag.ZeroQx | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.Contact | UpdatePositionFlag.Placement;

            Player = true;

            // Admin = true; // Uncomment to enable Admin flag on Player objects. I would expect this would go in Admin.cs, replacing Player = true,
            // I don't believe both were on at the same time. -Ripley

            IgnoreCollision = true; Gravity = true; Hidden = true; EdgeSlide = true;

            // apply defaults.  "Load" should be overwriting these with values specific to the character
            // TODO: Load from database should be loading player data - including inventroy and positions
            CurrentMotionState = new UniversalMotion(MotionStance.Standing);

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
        { get { return (uint)Character.CreationTimestamp; } }

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
            PhysicsTableId = Character.PhysicsTableId;
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

            UpdateAppearance(this);
            Burden = UpdateBurden();

            // Save the the LoginTimestamp
            Character.SetDoubleTimestamp(PropertyDouble.LoginTimestamp);

            TotalLogins++;
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
            PaletteBaseId = Character.PaletteId;
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

            // These don't usually get saved back to the object so setting here for now.
            // Realisticly speaking, I think it will be possible to eliminate WeenieHeaderFlags and PhysicsDescriptionFlag from the datbase
            // AceObjectDescriptionFlag possibly could be eliminated as well... -Ripley
            // actually we do use those without creating a wo - so it would be needed to keep them in the database Og II
            obj.WeenieHeaderFlags = (uint)WeenieFlags;
            obj.PhysicsDescriptionFlag = (uint)PhysicsDescriptionFlag;

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
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.AvailableSkillCredits, Character.AvailableSkillCredits);

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
                    PlayParticleEffect(Enum.PlayScript.WeddingBliss, Guid);
                    break;
                }
            }

            if (Character.Level > startingLevel)
            {
                string level = $"{Character.Level}";
                string skillCredits = $"{Character.AvailableSkillCredits}";
                string xpAvailable = $"{Character.AvailableExperience:#,###0}";
                var levelUp = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.Level, Character.Level);
                string levelUpMessageText = (Character.Level == maxLevel.Level) ? $"You have reached the maximum level of {level}!" : $"You are now level {level}!";
                var levelUpMessage = new GameMessageSystemChat(levelUpMessageText, ChatMessageType.Advancement);
                string xpUpdateText = (Character.AvailableSkillCredits > 0) ? $"You have {xpAvailable} experience points and {skillCredits} skill credits available to raise skills and attributes." : $"You have {xpAvailable} experience points available to raise skills and attributes.";
                var xpUpdateMessage = new GameMessageSystemChat(xpUpdateText, ChatMessageType.Advancement);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.AvailableSkillCredits, Character.AvailableSkillCredits);
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
                PlayParticleEffect(Enum.PlayScript.LevelUp, Guid);
            }
        }

        public void SpendXp(Enum.Ability ability, uint amount)
        {
            bool isSecondary = false;
            ICreatureXpSpendableStat creatureStat;
            CreatureAbility creatureAbility;
            bool success = AceObject.AceObjectPropertiesAttributes.TryGetValue(ability, out creatureAbility);
            if (success)
            {
                creatureStat = creatureAbility;
            }
            else
            {
                CreatureVital v;
                success = AceObject.AceObjectPropertiesAttributes2nd.TryGetValue(ability, out v);

                // Invalid ability
                if (success)
                {
                    creatureStat = v;
                }
                else
                {
                    log.Error("Invalid ability passed to Player.SpendXp");
                    return;
                }
                isSecondary = true;
            }
            uint baseValue = creatureStat.Base;
            uint result = SpendAbilityXp(creatureStat, amount);
            uint ranks = creatureStat.Ranks;
            uint newValue = creatureStat.UnbuffedValue;
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
                    abilityUpdate = new GameMessagePrivateUpdateVital(Session, ability, ranks, baseValue, result, creatureStat.Current);
                }

                // checks if max rank is achieved and plays fireworks w/ special text
                if (IsAbilityMaxRank(ranks, isSecondary))
                {
                    // fireworks
                    PlayParticleEffect(Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {ability} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {ability} is now {newValue}!";
                }
                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, Character.AvailableExperience);
                var soundEvent = new GameMessageSound(this.Guid, Sound.RaiseTrait, 1f);
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
        private uint SpendAbilityXp(ICreatureXpSpendableStat ability, uint amount)
        {
            uint result = 0;
            ExperienceExpenditureChart chart;
            XpTable xpTable = XpTable.ReadFromDat();
            switch (ability.Ability)
            {
                case Enum.Ability.Health:
                case Enum.Ability.Stamina:
                case Enum.Ability.Mana:
                    chart = xpTable.VitalXpChart;
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
                // FIXME(ddevec):
                //      Really AddRank() should probably be a method of CreatureAbility/CreatureVital
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
            var soundEvent = new GameMessageSound(this.Guid, Sound.RaiseTrait, 1f);
            string messageText = "";

            if (result > 0u)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(ranks, status))
                {
                    // fireworks on rank up is 0x8D
                    PlayParticleEffect(Enum.PlayScript.WeddingBliss, Guid);
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
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.NumDeaths, Character.NumDeaths);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.DeathLevel, Character.DeathLevel);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.VitaeCpPool, Character.VitaeCpPool);
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

            if (examinationId.Full == 0)
            {
                // Deselect the formerly selected Target
                // selectedTarget = ObjectGuid.Invalid;
                return;
            }

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
                    // We could be wielded - let's check that next.
                    if (WieldedObjects.TryGetValue(examinationId, out wo))
                    {
                        wo.Examine(Session);
                    }
                    if (InteractiveWorldObjects.ContainsKey(examinationId))
                        InteractiveWorldObjects[examinationId].Examine(Session);
                    else
                    {
                        ActionChain chain = new ActionChain();
                        CurrentLandblock.ChainOnObject(
                            chain,
                            examinationId,
                            (WorldObject cwo) =>
                                {
                                    cwo.Examine(Session);
                                });
                        chain.EnqueueChain();
                    }
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

                ActionChain idChain = new ActionChain();
                CurrentLandblock.ChainOnObject(idChain, queryId, (WorldObject cwo) =>
                {
                    cwo.QueryHealth(Session);
                });
                idChain.EnqueueChain();
            });
            chain.EnqueueChain();
        }

        public void HandleActionQueryItemMana(ObjectGuid queryId)
        {
            if (queryId.Full == 0)
            {
                // Do nothing if the queryID is 0
                return;
            }

            ActionChain chain = new ActionChain();
            chain.AddAction(this, () =>
            {
                // the object could be in the world or on the player, first check player
                WorldObject wo = GetInventoryItem(queryId);
                if (wo != null)
                {
                    wo.QueryItemMana(Session);
                }
                else
                {
                    // We could be wielded - let's check that next.
                    if (WieldedObjects.TryGetValue(queryId, out wo))
                    {
                        wo.QueryItemMana(Session);
                    }
                    else
                    {
                        ActionChain idChain = new ActionChain();
                        CurrentLandblock.ChainOnObject(idChain, queryId, (WorldObject cwo) =>
                        {
                            cwo.QueryItemMana(Session);
                        });
                        idChain.EnqueueChain();
                    }
                }
            });
            chain.EnqueueChain();
        }

        public void HandleActionReadBookPage(ObjectGuid bookId, uint pageNum)
        {
            // TODO: Do we want to throttle this request, like appraisals?
            ActionChain bookChain = new ActionChain();

            // The object can be in two spots... on the player or on the landblock
            // First check the player
            bookChain.AddAction(this, () =>
            {
                WorldObject wo = GetInventoryItem(bookId);
                // book is in the player's inventory...
                if (wo != null)
                {
                    wo.ReadBookPage(Session, pageNum);
                }
                else
                {
                    ActionChain chain = new ActionChain();
                    CurrentLandblock.ChainOnObject(chain, bookId, (WorldObject cwo) =>
                    {
                        cwo.ReadBookPage(Session, pageNum);
                    });
                    chain.EnqueueChain();
                }
            });
            bookChain.EnqueueChain();
        }

        public void HandleActionBuy(ObjectGuid vendorId, List<ItemProfile> items)
        {
                ActionChain chain = new ActionChain();
                CurrentLandblock.ChainOnObject(chain, vendorId, (WorldObject vdr) =>
                {
                    (vdr as Vendor).BuyItems(vendorId, items, this);
                });
                chain.EnqueueChain();
        }

        public void HandleActionBuyTransaction(List<WorldObject> purchaselist, int cost)
        {
            new ActionChain(this, () =>
            {
                if (SpendCoin((uint)cost))
                {
                    foreach (WorldObject wo in purchaselist)
                    {
                        // todo: check for inventory space.
                        AddNewItemToInventory(wo.WeenieClassId);
                    }
                    // send updated vendor inventory
                    // todo: send unique items back to vendor so they can be removed from the list.
                    HandleActionApproachVendor(this, purchaselist);
                }
                else
                {
                    // todo: You dont have enough money to buy this + check for inventory space.
                }
            }).EnqueueChain();
        }

        /// <summary>
        /// Called when the Buy Packet is received, items are listed for sale and have been sent
        /// </summary>
        /// <param name="items"></param>
        /// <param name="vendorId"></param>
        public void HandleActionBeginSellTransaction(List<ItemProfile> items, ObjectGuid vendorId)
        {
            new ActionChain(this, () =>
            {
                List<WorldObject> purchaselist = new List<WorldObject>();
                // Player is selling items..
                // check players inventory for items..
                foreach (ItemProfile item in items)
                {
                    // check to see if item is in players inventory.
                    WorldObject wo = GetInventoryItem(item.Guid);
                    if (wo != null)
                        purchaselist.Add(wo);          
                }

                // Send Items to Vendor for processing..
                ActionChain vendorchain = new ActionChain();
                CurrentLandblock.ChainOnObject(vendorchain, vendorId, (WorldObject vdr) =>
                {
                    (vdr as Vendor).StartSellItems(purchaselist, this);
                });
                vendorchain.EnqueueChain();
            }).EnqueueChain();
        }
        public void HandleActionFinishSellTransaction(List<WorldObject> items, ObjectGuid vendorId, uint coin)
        {
            new ActionChain(this, () =>
            {
                foreach (WorldObject item in items)
                {
                    // check to see if item is in players inventory.
                    WorldObject wo = GetInventoryItem(item.Guid);

                    if (wo != null)
                        DestroyInventoryItem(item);
                }
                AddCoin(coin);
            }).EnqueueChain();
        }

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
        /// Adds a new object to the player's inventory of the specified weenie class.  intended use case: giving items to players
        /// while they are playing.  this calls all the necessary helper functions to have the item be tracked and sent to the client.
        /// </summary>
        /// <returns>the object created</returns>
        public WorldObject AddNewItemToInventory(uint weenieClassId)
        {
            var wo = Factories.WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            wo.ContainerId = Guid.Full;
            wo.Placement = 0;
            AddToInventory(wo);
            TrackObject(wo);
            UpdatePlayerBurden();

            return wo;
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
                newFriend.Id = new ObjectGuid(friendInfo.Guid);

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
                ChatPacket.SendServerMessage(Session, "That character is not in your friends list!", ChatMessageType.Broadcast);
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
        /// Set the currently position of the character, to later save in the database.
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
        /// This method is used to clear the wielded items list ( the list of ace objects used to save wielded items ) and loads it with a snapshot
        /// of the aceObjects from the current list of wielded world objects. Og II
        /// </summary>
        public void SnapshotWieldedItems(bool clearDirtyFlags = false)
        {
            WieldedItems.Clear();
            foreach (var wo in WieldedObjects)
            {
                WieldedItems.Add(wo.Value.Guid, wo.Value.SnapShotOfAceObject(clearDirtyFlags));
            }
        }

        /// <summary>
        /// Internal save character functionality
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.
        /// </summary>
        private void SaveCharacter()
        {
            if (Character != null)
            {
                // Save the current position to persistent storage, only during the server update interval
                SetPhysicalCharacterPosition();

                // Let's get a snapshot of our wielded items prior to save.

                SnapshotWieldedItems();

                DatabaseManager.Shard.SaveObject(GetSavableCharacter(), null);

                // FIXME : the issue is here - I still have the inventory in two dictionaries after clone.   I am missing something Og II
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
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.Age, Character.Age));
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

        /// <summary>
        /// This is called prior to SendSelf to load up the child list for wielded items that are held in a hand.
        /// </summary>
        private void SetChildren()
        {
            Children.Clear();

            foreach (WorldObject wieldedObject in WieldedObjects.Values)
            {
                WorldObject wo = wieldedObject;
                uint placementId;
                uint childLocation;
                if ((wo.CurrentWieldedLocation != null) && (((EquipMask)wo.CurrentWieldedLocation & EquipMask.Selectable) != 0))
                    SetChild(this, wo, (uint)wo.CurrentWieldedLocation, out placementId, out childLocation);
                else
                    log.Debug($"Error - item set as child that should not be set - no currentWieldedLocation {wo.Name} - {wo.Guid.Full:X}");
            }
        }

        private void SendSelf()
        {
            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);

            SetChildren();

            Session.Network.EnqueueSend(new GameMessagePlayerCreate(Guid),
                                        new GameMessageCreateObject(this));

            SendInventoryAndWieldedItems(Session);
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

            Hidden = true;
            IgnoreCollision = true;
            ReportCollision = false;
            EnqueueBroadcastPhysicsState();
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
                // This is a hack to make walking work correctly.   Og II
                if (holdKey != 0 || (md.ForwardCommand == (uint)MotionCommand.WalkForward))
                    newMotion.IsAutonomous = true;
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
                                            Session.Player.Sequences,
                                            PropertyInt.EncumbranceVal,
                                            Session.Player.Burden ?? 0u));
        }

        /// <summary>
        /// Tracks Interacive world object you are have interacted with recently.
        /// </summary>
        /// <param name="worldObject"></param>
        public void TrackInteractiveObjects(List<WorldObject> worldObjects)
        {
            // todo: figure out a way to expire objects.. objects clearly not in range of interaction /etc
            new ActionChain(this, () =>
            {
                foreach (WorldObject wo in worldObjects)
                {
                    if (InteractiveWorldObjects.ContainsKey(wo.Guid))
                        InteractiveWorldObjects[wo.Guid] = wo;
                    else
                        InteractiveWorldObjects.Add(wo.Guid, wo);
                }
            }).EnqueueChain();
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
                if (worldObject.DefaultScriptId != null)
                    Session.Network.EnqueueSend(new GameMessageScript(worldObject.Guid, (PlayScript)worldObject.DefaultScriptId));
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

                EnqueueBroadcastPhysicsState();

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

        public void HandleMRT()
        {
            ActionChain mrtChain = new ActionChain();

            // Handle MRT Toggle internal must decide what to do next...
            mrtChain.AddAction(this, new ActionEventDelegate(() => HandleMRTToggleInternal()));

            mrtChain.EnqueueChain();
        }

        private void HandleMRTToggleInternal()
        {
            // This requires the Admin flag set on ObjectDescriptionFlags
            // I would expect this flag to be set in Admin.cs which would be a subclass of Player
            // FIXME: maybe move to Admin class?
            // TODO: reevaluate class location

            if (!ImmuneCellRestrictions)
                ImmuneCellRestrictions = true;
            else
                ImmuneCellRestrictions = false;

            // The EnqueueBroadcastUpdateObject below sends the player back into teleport. I assume at this point, this was never done to players
            // EnqueueBroadcastUpdateObject();

            // The private message below worked as expected, but it only broadcast to the player. This would be a problem with for others in range seeing something try to
            // pass through a barrier but not being allowed.
            // var updateBool = new GameMessagePrivateUpdatePropertyBool(Session, PropertyBool.IgnoreHouseBarriers, ImmuneCellRestrictions);
            // Session.Network.EnqueueSend(updateBool);

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePropertyBool(this, PropertyBool.IgnoreHouseBarriers, ImmuneCellRestrictions));

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Bypass Housing Barriers now set to: {ImmuneCellRestrictions}", ChatMessageType.Broadcast));
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

                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.CombatMode, 1);

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

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.CombatMode, 1);

            var motionMarketplaceRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

            var animationEvent = new GameMessageUpdateMotion(Guid,
                                                             Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                                             Sequences, motionMarketplaceRecall);

            // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
            // FIX: Recall text isn't being broadcast yet, need to address
            Session.Network.EnqueueSend(updateCombatMode, sysChatMessage);
            DoMotion(motionMarketplaceRecall);
        }

        public void HandleActionFinishBarber(ClientMessage message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoFinishBarber(message));
            chain.EnqueueChain();
        }

        public void DoFinishBarber(ClientMessage message)
        {
            // Read the payload sent from the client...
            PaletteBaseId = message.Payload.ReadUInt32();
            Character.HeadObject = message.Payload.ReadUInt32();
            Character.HairTexture = message.Payload.ReadUInt32();
            Character.DefaultHairTexture = message.Payload.ReadUInt32();
            Character.EyesTexture = message.Payload.ReadUInt32();
            Character.DefaultEyesTexture = message.Payload.ReadUInt32();
            Character.NoseTexture = message.Payload.ReadUInt32();
            Character.DefaultNoseTexture = message.Payload.ReadUInt32();
            Character.MouthTexture = message.Payload.ReadUInt32();
            Character.DefaultMouthTexture = message.Payload.ReadUInt32();
            Character.SkinPalette = message.Payload.ReadUInt32();
            Character.HairPalette = message.Payload.ReadUInt32();
            Character.EyesPalette = message.Payload.ReadUInt32();
            Character.SetupTableId = message.Payload.ReadUInt32();

            uint option_bound = message.Payload.ReadUInt32(); // Supress Levitation - Empyrean Only
            uint option_unk = message.Payload.ReadUInt32(); // Unknown - Possibly set aside for future use?

            // Check if Character is Empyrean, and if we need to set/change/send new motion table
            if (Character.Heritage == 9)
            {
                // These are the motion tables for Empyrean float and not-float (one for each gender). They are hard-coded into the client.
                const uint EmpyreanMaleFloatMotionDID = 0x0900020Bu;
                const uint EmpyreanFemaleFloatMotionDID = 0x0900020Au;
                const uint EmpyreanMaleMotionDID = 0x0900020Eu;
                const uint EmpyreanFemaleMotionDID = 0x0900020Du;

                // Check for the Levitation option for Empyrean. Shadow crown and Undead flames are handled by client.
                if (Character.Gender == 1) // Male
                {
                    if (option_bound == 1 && Character.MotionTableId != EmpyreanMaleMotionDID)
                    {
                        Character.MotionTableId = EmpyreanMaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)Character.MotionTableId));
                    }
                    else if (option_bound == 0 && Character.MotionTableId != EmpyreanMaleFloatMotionDID)
                    {
                        Character.MotionTableId = EmpyreanMaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)Character.MotionTableId));
                    }
                }
                else // Female
                {
                    if (option_bound == 1 && Character.MotionTableId != EmpyreanFemaleMotionDID)
                    {
                        Character.MotionTableId = EmpyreanFemaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)Character.MotionTableId));
                    }
                    else if (option_bound == 0 && Character.MotionTableId != EmpyreanFemaleFloatMotionDID)
                    {
                        Character.MotionTableId = EmpyreanFemaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)Character.MotionTableId));
                    }
                }
            }

            UpdateAppearance(this);

            // Broadcast updated character appearance
            CurrentLandblock.EnqueueBroadcast(
                Location,
                Landblock.MaxObjectRange,
                new GameMessageObjDescEvent(this));
        }

        /// <summary>
        ///  Sends object description if the client requests it
        /// </summary>
        /// <param name="item"></param>
        public void HandleActionForceObjDescSend(ObjectGuid item)
        {
            ActionChain objDescChain = new ActionChain();
            objDescChain.AddAction(this, () =>
            {
                WorldObject wo = GetInventoryItem(item);
                if (wo != null)
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                        new GameMessageObjDescEvent(wo));
                else
                    log.Debug($"Error - requested object description for an item I do not know about - {item.Full:X}");
            });
            objDescChain.EnqueueChain();
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

        /// <summary>
        /// This method removes an item from Inventory and adds it to wielded items.
        /// It also clears all properties used when an object is contained and sets the needed properties to be wielded Og II
        /// </summary>
        /// <param name="item">The world object we are wielding</param>
        /// <param name="wielder">Who is wielding the item</param>
        /// <param name="currentWieldedLocation">What wield location are we going into</param>
        private void AddToWieldedObjects(ref WorldObject item, WorldObject wielder, EquipMask currentWieldedLocation)
        {
            RemoveFromInventory(InventoryObjects, item.Guid);
            // Unset container fields
            item.Placement = null;
            item.ContainerId = null;
            // Set fields needed to be wielded.
            item.WielderId = wielder.Guid.Full;
            item.CurrentWieldedLocation = currentWieldedLocation;

            if (!wielder.WieldedObjects.ContainsKey(item.Guid))
                wielder.WieldedObjects.Add(item.Guid, item);
        }

        /// <summary>
        /// This method is used to remove an item from the Wielded Objects dictionary.
        /// It does not add it to inventory as you could be unwielding to the ground or a chest. Og II
        /// </summary>
        /// <param name="itemGuid">Guid of the item to be unwielded.</param>
        private void RemoveFromWieldedObjects(ObjectGuid itemGuid)
        {
            if (WieldedObjects.ContainsKey(itemGuid))
                WieldedObjects.Remove(itemGuid);
        }

        /// <summary>
        /// This method iterates through your main pack, any packs and finds all the items contained
        /// It also iterates over your wielded items - it sends create object messages needed by the login process
        /// it is called from SendSelf as part of the login message traffic.   Og II
        /// </summary>
        /// <param name="session"></param>
        public void SendInventoryAndWieldedItems(Session session)
        {
            foreach (WorldObject invItem in InventoryObjects.Values)
            {
                session.Network.EnqueueSend(new GameMessageCreateObject(invItem));
                // Was the item I just send a container?   If so, we need to send the items in the container as well. Og II
                if (invItem.WeenieType != WeenieType.Container)
                    continue;

                Session.Network.EnqueueSend(new GameEventViewContents(Session, invItem.SnapShotOfAceObject()));
                foreach (WorldObject itemsInContainer in invItem.InventoryObjects.Values)
                {
                    session.Network.EnqueueSend(new GameMessageCreateObject(itemsInContainer));
                }
            }

            foreach (WorldObject wieldedObject in WieldedObjects.Values)
            {
                WorldObject item = wieldedObject;
                if ((item.CurrentWieldedLocation != null) && (((EquipMask)item.CurrentWieldedLocation & EquipMask.Selectable) != 0))
                {
                    uint placementId;
                    uint childLocation;
                    session.Player.SetChild(this, item, (uint)item.CurrentWieldedLocation, out placementId, out childLocation);
                }
                session.Network.EnqueueSend(new GameMessageCreateObject(item));
            }
        }

        /// <summary>
        /// This method is called in response to a put item in container message.  It is used when the item going
        /// into a container was wielded.   It sets the appropriate properties, sends out response messages
        /// and handles switching stances - for example if you have a bow wielded and are in bow combat stance,
        /// when you unwield the bow, this also sends the messages needed to go into unarmed combat mode. Og II
        /// </summary>
        /// <param name="container"></param>
        /// <param name="item"></param>
        /// <param name="placement"></param>
        /// <param name="inContainerChain"></param>
        private void HandleUnwieldItem(Container container, WorldObject item, uint placement, ActionChain inContainerChain)
        {
            EquipMask? oldLocation = item.CurrentWieldedLocation;

            item.ContainerId = container.Guid.Full;
            SetInventoryForContainer(item, placement);

            RemoveFromWieldedObjects(item.Guid);
            // We will always be updating the player appearance
            UpdateAppearance(this);

            if ((oldLocation & EquipMask.Selectable) != 0)
            {
                // We are coming from a hand shield slot.
                Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));
            }

            // Set the container stuff
            item.ContainerId = container.Guid.Full;
            item.Placement = placement;

            inContainerChain.AddAction(this, () =>
            {
                AddToInventory(item, placement);
            });

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, new ObjectGuid(0)),
                                            new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, 0),
                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, container.Guid),
                                            new GameMessagePickupEvent(item),
                                            new GameMessageSound(Guid, Sound.UnwieldObject, (float)1.0),
                                            new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                                            new GameMessageObjDescEvent(this));

            if ((oldLocation != EquipMask.MissileWeapon && oldLocation != EquipMask.Held && oldLocation != EquipMask.MeleeWeapon) || ((CombatMode & CombatMode.CombatCombat) == 0))
                return;
            HandleSwitchToPeaceMode(CombatMode);
            HandleSwitchToMeleeCombatMode(CombatMode);
        }

        /// <summary>
        /// Method is called in response to put item in container message.   This use case is we are just
        /// reorganizing our items.   It is either a in pack slot to slot move, or we could be going from one
        /// pack (container) to another. This method is called from an action chain.  Og II
        /// </summary>
        /// <param name="item">the item we are moving</param>
        /// <param name="container">what container are we going in</param>
        /// <param name="placement">what is my slot position within that container</param>
        private void HandleMove(ref WorldObject item, Container container, uint placement)
        {
            if (item.ContainerId != null && item.ContainerId != container.Guid.Full)
            {
                // We are changing containers
                if (item.ContainerId != this.Guid.Full)
                {
                    // The old container was not our main pack, the old container had to be in our inventory.
                    ObjectGuid priorContainerGuid = new ObjectGuid((uint)item.ContainerId);
                    RemoveFromInventory(InventoryObjects[priorContainerGuid].InventoryObjects, item.Guid);
                }
                else
                    RemoveFromInventory(this.InventoryObjects, item.Guid);
            }

            item.ContainerId = container.Guid.Full;
            item.Placement = placement;
            container.AddToInventory(item, placement);
            Session.Network.EnqueueSend(
                new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container,
                    container.Guid));
        }

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting
        /// item. Og II
        /// </summary>
        /// <param name="stackId"></param>
        /// <param name="containerId"></param>
        /// <param name="place"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void HandleActionStackableSplitToContainer(uint stackId, uint containerId, uint place, ushort amount)
        {
            // TODO: add the complementary method to combine items Og II
            ActionChain splitItemsChain = new ActionChain();
            splitItemsChain.AddAction(this, () =>
                {
                    Container container;
                    if (containerId == Guid.Full)
                    {
                        container = this;
                    }
                    else
                    {
                        container = (Container)GetInventoryItem(new ObjectGuid(containerId));
                    }

                    if (container == null)
                    {
                        log.InfoFormat("Asked to split stack {0} in container {1} - the container was not found",
                            stackId,
                            containerId);
                        return;
                    }
                    WorldObject stack = container.GetInventoryItem(new ObjectGuid(stackId));
                    if (stack == null)
                    {
                        log.InfoFormat("Asked to split stack {0} in container {1} - the stack item was not found",
                            stackId,
                            containerId);
                        return;
                    }
                    if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
                    {
                        log.InfoFormat(
                            "Asked to split stack {0} in container {1} - with amount of {2} but there is not enough to split",
                            stackId, containerId, amount);
                        return;
                    }

                    // Ok we are in business

                    WorldObject newStack = new GenericObject(stack.NewAceObjectFromCopy()); // This should probably be figuring out what the weenietype of an object is and returning that, yeah?
                    container.AddToInventory(newStack);
                    var valuePerItem = stack.Value / stack.StackSize;
                    var burdenPerItem = stack.Burden / stack.StackSize;
                    ushort oldStackSize = (ushort)stack.StackSize;
                    ushort newStackSize = (ushort)stack.StackSize;
                    newStackSize = (ushort)(newStackSize - amount);

                    newStack.StackSize = amount;
                    newStack.Value = newStack.StackSize * valuePerItem;
                    newStack.Burden = (ushort)(newStack.StackSize * burdenPerItem);

                    stack.StackSize = newStackSize;
                    stack.Value = stack.StackSize * valuePerItem;
                    stack.Burden = (ushort)(stack.StackSize * burdenPerItem);

                    GameMessagePrivateUpdatePropertyInt msgUpdateValue =
                        new GameMessagePrivateUpdatePropertyInt(container.Sequences, PropertyInt.Value, 1);
                    GameMessagePutObjectInContainer msgPutObjectInContainer =
                        new GameMessagePutObjectInContainer(Session, container.Guid, newStack, place);
                    Debug.Assert(stack.StackSize != null, "stack.StackSize != null");
                    Debug.Assert(stack.Value != null, "stack.Value != null");
                    GameMessageSetStackSize msgAdjustOldStackSize = new GameMessageSetStackSize(stack.Sequences,
                        stack.Guid, (int)stack.StackSize, oldStackSize);

                    GameMessageCreateObject msgNewStack = new GameMessageCreateObject(newStack);
                    CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgUpdateValue,
                        msgPutObjectInContainer, msgAdjustOldStackSize, msgNewStack);
                });
            splitItemsChain.EnqueueChain();
        }

        /// <summary>
        /// This method is used to pick items off the world - out of 3D space and into our inventory or to a wielded slot.
        /// It checks the use case needed, sends the appropriate response messages.   In addition, it will move to objects
        /// that are out of range in the attemp to pick them up.   It will call update apperiance if needed and you have
        /// wielded an item from the ground. Og II
        /// </summary>
        /// <param name="container"></param>
        /// <param name="itemGuid"></param>
        /// <param name="placement"></param>
        /// <param name="iidPropertyId"></param>
        private void HandlePickupItem(Container container, ObjectGuid itemGuid, uint placement, PropertyInstanceId iidPropertyId)
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
                CurrentLandblock.QueueItemTransfer(pickUpItemChain, itemGuid, container.Guid);
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
                    SetInventoryForContainer(item, placement);
                    // FIXME(ddevec): I'm not 100% sure which of these need to be broadcasts, and which are local sends...
                    var motion = new UniversalMotion(MotionStance.Standing);
                    if (iidPropertyId == PropertyInstanceId.Container)
                    {
                        Session.Network.EnqueueSend(
                            new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, UpdateBurden()),
                            new GameMessageSound(Guid, Sound.PickUpItem, 1.0f),
                            new GameMessageUpdateInstanceId(itemGuid, container.Guid, iidPropertyId),
                            new GameMessagePutObjectInContainer(Session, container.Guid, item, placement));
                    }
                    else
                    {
                        AddToWieldedObjects(ref item, container, (EquipMask)placement);
                        UpdateAppearance(container);
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                    new GameMessageObjDescEvent(this),
                                                    new GameMessageUpdateInstanceId(container.Guid, itemGuid, PropertyInstanceId.Wielder),
                                                    new GameEventWieldItem(Session, itemGuid.Full, placement));
                    }

                    CurrentLandblock.EnqueueBroadcast(
                        Location,
                        Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(
                            Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences,
                            motion),
                        new GameMessagePickupEvent(item));

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

        /// <summary>
        /// This method was developed by OptimShi.   It looks at currently wielded items and does the appropriate
        /// model replacements.   It then sends all uncovered  body parts. Og II
        /// </summary>
        /// <param name="container"></param>
        public void UpdateAppearance(Container container)
        {
            ClearObjDesc();
            AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette

            var coverage = new List<uint>();

            foreach (var w in WieldedObjects)
            {
                // We can wield things that are not part of our model, only use those items that can cover our model.
                if ((w.Value.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0)
                {
                    ClothingTable item;
                    if (w.Value.ClothingBase != null)
                        item = ClothingTable.ReadFromDat((uint)w.Value.ClothingBase);
                    else
                    {
                        ChatPacket.SendServerMessage(
                            Session,
                            "We have not implemented the visual appearance for that item yet. ",
                            ChatMessageType.AdminTell);
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

                        foreach (ModelPalette p in w.Value.GetPalettes)
                            AddPalette(p.PaletteId, p.Offset, p.Length);
                    }
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

        /// <summary>
        /// This method sets properties needed for items that will be child items.
        /// Items here are only items equipped in the hands.  This deals with the orientation
        /// and positioning for visual appearance of the child items held by the parent. Og II
        /// </summary>
        /// <param name="container">Who is the parent of this child item</param>
        /// <param name="item">The child item - we link them together</param>
        /// <param name="placement">Where is this on the parent - where is it equipped</param>
        /// <param name="placementId">out parameter - this deals with the orientation of the child item as it relates to parent model</param>
        /// <param name="childLocation">out parameter - this is another part of the orientation data for correct visual display</param>
        public void SetChild(Container container, WorldObject item, uint placement, out uint placementId, out uint childLocation)
        {
            placementId = 0;
            childLocation = 0;
            // TODO:   I think there is a state missing - it is one of the edge cases.   I need to revist this.   Og II
            switch ((EquipMask)placement)
            {
                case EquipMask.MissileWeapon:
                    {
                        if (item.DefaultCombatStyle == MotionStance.BowAttack ||
                            item.DefaultCombatStyle == MotionStance.CrossBowAttack ||
                            item.DefaultCombatStyle == MotionStance.AtlatlCombat)
                        {
                            childLocation = 2;
                            placementId = 3;
                        }
                        else
                        {
                            childLocation = 1;
                            placementId = 1;
                        }
                        break;
                    }
                case EquipMask.Shield:
                    {
                        if (item.ItemType == ItemType.Armor)
                        {
                            childLocation = 3;
                            placementId = 6;
                        }
                        else
                        {
                            childLocation = 8;
                            placementId = 1;
                        }
                        break;
                    }
                case EquipMask.Held:
                    {
                        childLocation = 1;
                        placementId = 1;
                        break;
                    }
                default:
                    {
                        childLocation = 1;
                        placementId = 1;
                        break;
                    }
            }
            if (item.CurrentWieldedLocation != null)
                container.Children.Add(new HeldItem(item.Guid.Full, childLocation, (EquipMask)item.CurrentWieldedLocation));
            item.ParentLocation = childLocation;
            item.Location = Location;
            item.AnimationFrame = placementId;
        }

        public void HandleActionWieldItem(Container container, uint itemId, uint placement)
        {
            ActionChain wieldChain = new ActionChain();
            wieldChain.AddAction(this, () =>
                {
                    ObjectGuid itemGuid = new ObjectGuid(itemId);
                    WorldObject item = GetInventoryItem(itemGuid);

                    if (item != null)
                    {
                        AddToWieldedObjects(ref item, container, (EquipMask)placement);

                        if ((EquipMask)placement == EquipMask.MissileAmmo)
                            Session.Network.EnqueueSend(new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                        new GameMessageSound(Guid, Sound.WieldObject, (float)1.0));
                        else
                        {
                            if (((EquipMask)placement & EquipMask.Selectable) != 0)
                            {
                                uint placementId;
                                uint childLocation;
                                SetChild(container, item, placement, out placementId, out childLocation);

                                UpdateAppearance(container);

                                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                                new GameMessageParentEvent(Session.Player, item, childLocation, placementId),
                                                                new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                                new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                                new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, new ObjectGuid(0)),
                                                                new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, container.Guid),
                                                                new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, placement));

                                if (CombatMode == CombatMode.NonCombat || CombatMode == CombatMode.Undef)
                                    return;
                                switch ((EquipMask)placement)
                                {
                                    case EquipMask.MissileWeapon:
                                        SetCombatMode(CombatMode.Missile);
                                        break;
                                    case EquipMask.Held:
                                        SetCombatMode(CombatMode.Magic);
                                        break;
                                    default:
                                        SetCombatMode(CombatMode.Melee);
                                        break;
                                }
                            }
                            else
                            {
                                UpdateAppearance(container);

                                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                            new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                            new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, new ObjectGuid(0)),
                                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, container.Guid),
                                                            new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, placement),
                                                            new GameMessageObjDescEvent(container));
                            }
                        }
                    }
                    else
                    {
                        HandlePickupItem(container, itemGuid, placement, PropertyInstanceId.Wielder);
                    }
                });
            wieldChain.EnqueueChain();
        }

        public void HandleActionPutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, uint placement = 0)
        {
            ActionChain inContainerChain = new ActionChain();
            inContainerChain.AddAction(
                this,
                () =>
                    {
                        Container container;

                        if (containerGuid.IsPlayer())
                            container = this;
                        else
                        {
                            // Ok I am going into player pack - not the main pack.

                            // TODO pick up here - I have a generic object for a container, need to find out why.
                            container = (Container)GetInventoryItem(containerGuid);
                        }

                        // is this something I already have? If not, it has to be a pickup - do the pickup and out.
                        if (!HasItem(itemGuid))
                        {
                            // This is a pickup into our main pack.
                            HandlePickupItem(container, itemGuid, placement, PropertyInstanceId.Container);
                            return;
                        }

                        // Ok, I know my container and I know I must have the item so let's get it.
                        WorldObject item = GetInventoryItem(itemGuid);

                        // Was I equiped?   If so, lets take care of that and unequip
                        if (item.WielderId != null)
                        {
                            HandleUnwieldItem(container, item, placement, inContainerChain);
                            return;
                        }

                        // if were are still here, this needs to do a pack pack or main pack move.
                        HandleMove(ref item, container, placement);
                    });
            inContainerChain.EnqueueChain();
        }

        /// <summary>
        /// Context: only call when in the player action loop
        /// </summary>
        public void DestroyInventoryItem(WorldObject wo)
        {
            RemoveFromInventory(InventoryObjects, wo.Guid);
            Session.Network.EnqueueSend(new GameMessageRemoveObject(wo));
            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, (uint)Burden));
        }

        private bool SpendCoin(uint value)
        {
            if (coinValue - value <= 0)
                return false;
            else
            {
                coinValue = coinValue - value;
                SetCoin(coinValue);
                return true;
            }
        }

        public void AddCoin(uint value)
        {
            coinValue += value;
            SetCoin(coinValue);
        }

        private void SetCoin(uint value)
        {
            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.CoinValue, value));
        }

        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            ActionChain dropChain = new ActionChain();

            // Goody Goody -- lets build  drop chain
            // First start drop animation
            dropChain.AddAction(this, () =>
            {
                WorldObject item = GetInventoryItem(itemGuid);
                if (item == null)
                {
                    // check to see if this item is wielded
                    if (!WieldedObjects.TryGetValue(itemGuid, out item))
                        return;
                    RemoveFromWieldedObjects(itemGuid);
                    UpdateAppearance(this);
                    ObjectGuid clearWielder = new ObjectGuid(0);
                    Session.Network.EnqueueSend(
                        new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                        new GameMessageObjDescEvent(this),
                        new GameMessageUpdateInstanceId(Guid, clearWielder, PropertyInstanceId.Wielder));
                }
                else
                {
                    RemoveFromInventory(InventoryObjects, itemGuid);
                }

                SetInventoryForWorld(item);
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, (uint)Burden));

                UniversalMotion motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
                ObjectGuid clearContainer = new ObjectGuid(0);
                Session.Network.EnqueueSend(
                    new GameMessageUpdateInstanceId(itemGuid, clearContainer, PropertyInstanceId.Container));

                // Set drop motion
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);

                // Now wait for Drop Motion to finish -- use ActionChain
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
                item.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                item.Sequences.GetNextSequence(SequenceType.ObjectVector);

                CurrentLandblock.AddWorldObject(item);

                // Ok we have handed off to the landblock, let's clean up the shard database.
                DatabaseManager.Shard.DeleteObject(item.SnapShotOfAceObject(), null);

                Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
            });

                chain.EnqueueChain();
            });

            dropChain.EnqueueChain();
        }

        public void HandleActionUseOnTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            ActionChain chain = new ActionChain(this, () =>
            {
                WorldObject invSource = GetInventoryItem(sourceObjectId);
                WorldObject invTarget = GetInventoryItem(targetObjectId);

                if (invTarget != null)
                {
                    // inventory on inventory, we can do this now
                    RecipeManager.UseObjectOnTarget(this, invSource, invTarget);
                }
                else if (targetObjectId == Guid)
                {
                    // using something on ourselves
                    RecipeManager.UseObjectOnTarget(this, invSource, this);
                }
                else
                {
                    ActionChain landblockChain = new ActionChain();
                    CurrentLandblock.ChainOnObject(landblockChain, targetObjectId, (WorldObject theTarget) =>
                    {
                        RecipeManager.UseObjectOnTarget(this, invSource, theTarget);
                    });
                    landblockChain.EnqueueChain();
                }
            });
            chain.EnqueueChain();
        }

        public void HandleActionUse(ObjectGuid usedItemId)
        {
            new ActionChain(this, () =>
            {
                WorldObject iwo = GetInventoryItem(usedItemId);
                if (iwo != null)
                {
                    iwo.OnUse(Session);
                }
                else
                {
                    if (CurrentLandblock != null)
                    {
                        // Just forward our action to the appropriate user...
                        ActionChain onUseChain = new ActionChain();
                        CurrentLandblock.ChainOnObject(onUseChain, usedItemId, (WorldObject wo) =>
                        {
                            if (wo != null)
                            {
                                wo.HandleActionOnUse(Guid);
                            }
                        });
                        onUseChain.EnqueueChain();
                    }
                }
            }).EnqueueChain();
        }

        public void HandleActionApplySoundEffect(Sound sound)
        {
            new ActionChain(this, () => PlaySound(sound, Guid)).EnqueueChain();
        }

        public void HandleActionApplyVisualEffect(PlayScript effect)
        {
            new ActionChain(this, () => PlayParticleEffect(effect, Guid)).EnqueueChain();
        }

        public ActionChain CreateMoveToChain(ObjectGuid target, float distance)
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

        public void TestWieldItem(Session session, uint modelId, int palOption)
        {
            // ClothingTable item = ClothingTable.ReadFromDat(0x1000002C); // Olthoi Helm
            // ClothingTable item = ClothingTable.ReadFromDat(0x10000867); // Cloak
            // ClothingTable item = ClothingTable.ReadFromDat(0x10000008); // Gloves
            // ClothingTable item = ClothingTable.ReadFromDat(0x100000AD); // Heaume
            ClothingTable item = ClothingTable.ReadFromDat(modelId);

            int palCount = 0;

            List<uint> coverage = new List<uint>(); // we'll store our fake coverage items here
            ClearObjDesc();
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

        public void HandleActionTalk(string message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoTalk(message));
            chain.EnqueueChain();
        }

        public void DoTalk(string message)
        {
            CurrentLandblock.EnqueueBroadcastLocalChat(this, message);
        }

        public void HandleActionEmote(string message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoEmote(message));
            chain.EnqueueChain();
        }

        public void HandleActionApproachVendor(WorldObject vendor, List<WorldObject> itemsForSale)
        {
            new ActionChain(this, () =>
            {
                Session.Network.EnqueueSend(new GameEventApproachVendor(Session, vendor, itemsForSale));
                SendUseDoneEvent();
            }).EnqueueChain();
        }

        public void DoEmote(string message)
        {
            CurrentLandblock.EnqueueBroadcastLocalChatEmote(this, message);
        }

        public void HandleActionSoulEmote(string message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoSoulEmote(message));
            chain.EnqueueChain();
        }

        public void DoSoulEmote(string message)
        {
            CurrentLandblock.EnqueueBroadcastLocalChatSoulEmote(this, message);
        }

        public void DoMoveTo(WorldObject wo)
        {
            ActionChain moveToObjectChain = new ActionChain();

            moveToObjectChain.AddChain(CreateMoveToChain(wo.Guid, 0.2f));
            moveToObjectChain.AddDelaySeconds(0.50);

            moveToObjectChain.AddAction(wo, () => wo.HandleActionOnUse(Guid));

            moveToObjectChain.EnqueueChain();
        }

        private const uint magicSkillCheckMargin = 50;

        public bool CanReadScroll(MagicSchool school, uint power)
        {
            bool ret = false;
            CreatureSkill creatureSkill;

            switch (school)
            {
                case MagicSchool.CreatureEnchantment:
                    creatureSkill = Skills[Skill.CreatureEnchantment];
                    break;
                case MagicSchool.WarMagic:
                    creatureSkill = Skills[Skill.WarMagic];
                    break;
                case MagicSchool.ItemEnchantment:
                    creatureSkill = Skills[Skill.ItemEnchantment];
                    break;
                case MagicSchool.LifeMagic:
                    creatureSkill = Skills[Skill.LifeMagic];
                    break;
                case MagicSchool.VoidMagic:
                    creatureSkill = Skills[Skill.VoidMagic];
                    break;
                default:
                    // Undefined magic school, something bad happened.
                    Debug.Assert((int)school > 5 || school <= 0, "Undefined magic school?");
                    return false;
            }

            if (creatureSkill.Status >= SkillStatus.Trained && creatureSkill.ActiveValue >= (power - magicSkillCheckMargin))
                ret = true;

            return ret;
        }

        public void SendUseDoneEvent()
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session));
        }
    }
}
