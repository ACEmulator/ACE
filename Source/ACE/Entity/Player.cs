using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Entity.Events;
using log4net;
using ACE.Network.Sequence;
using System.Collections.Concurrent;
using ACE.Network.GameAction.Actions;
using ACE.Network.GameAction;
using ACE.Network.Motion;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.DatLoader;

namespace ACE.Entity
{
    public sealed class Player : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        public uint SelectedTarget { get; set; }

        /// <summary>
        /// Amount of times this character has left a portal this session
        /// </summary>
        public uint PortalIndex { get; set; } = 1u;

        /// <summary>
        /// tick-stamp for the server time of the last time the player changed state (combat state?)
        /// </summary>
        public double LastStateChangeTicks { get; set; }

        private Character character;

        private object clientObjectMutex = new object();

        private Dictionary<ObjectGuid, double> clientObjectList = new Dictionary<ObjectGuid, double>();
        
        // queue of all the "actions" that come from the player that require processing
        // aynchronous to or outside of the network thread
        private ConcurrentQueue<QueuedGameAction> actionQueue = new ConcurrentQueue<QueuedGameAction>();

        // examination queue is really a subset of the actionQueue, but this existed on
        // retail servers as it's own separate thing and was intentionally throttled.
        private ConcurrentQueue<QueuedGameAction> examinationQueue = new ConcurrentQueue<QueuedGameAction>();

        private object delayedActionsMutex = new object();

        // dictionary for delaying further actions for an objectID
        private Dictionary<uint, double> delayedActions = new Dictionary<uint, double>();

        public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions
        {
            get { return character.CharacterOptions; }
        }

        public ReadOnlyDictionary<PositionType, Position> Positions
        {
            get { return character.Positions; }
        }

        public ReadOnlyCollection<Friend> Friends
        {
            get { return character.Friends; }
        }

        public bool IsAdmin
        {
            get { return character.IsAdmin; }
            set { character.IsAdmin = value; }
        }

        public bool IsEnvoy
        {
            get { return character.IsEnvoy; }
            set { character.IsEnvoy = value; }
        }

        public bool IsArch
        {
            get { return character.IsArch; }
            set { character.IsArch = value; }
        }

        public bool IsPsr
        {
            get { return character.IsPsr; }
            set { character.IsPsr = value; }
        }

        public ReadOnlyDictionary<PropertyBool, bool> PropertiesBool
        {
            get { return character.PropertiesBool; }
        }

        public ReadOnlyDictionary<PropertyDouble, double> PropertiesDouble
        {
            get { return character.PropertiesDouble; }
        }

        public ReadOnlyDictionary<PropertyInt, uint> PropertiesInt
        {
            get { return character.PropertiesInt; }
        }

        public ReadOnlyDictionary<PropertyInt64, ulong> PropertiesInt64
        {
            get { return character.PropertiesInt64; }
        }

        public ReadOnlyDictionary<PropertyString, string> PropertiesString
        {
            get { return character.PropertiesString; }
        }

        public ReadOnlyDictionary<Skill, CharacterSkill> Skills
        {
            get { return new ReadOnlyDictionary<Skill, CharacterSkill>(character.Skills); }
        }
        
        public uint TotalLogins
        {
            get { return character.TotalLogins; }
            set { character.TotalLogins = value; }
        }

        public Player(Session session) : base(ObjectType.Creature, session.CharacterRequested.Guid, "Player", 1, ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Player | ObjectDescriptionFlag.Attackable, WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar, CharacterPositionExtensions.StartingPosition(session.CharacterRequested.Guid.Low))
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

            Name = session.CharacterRequested.Name;
            Icon = 0x1036;
            GameData.ItemCapacity = 102;
            GameData.ContainerCapacity = 7;
            GameData.RadarBehavior = RadarBehavior.ShowAlways;
            GameData.RadarColour = RadarColor.White;
            GameData.Usable = Usable.UsableObjectSelf;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);
            PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Movement;

            // apply defaults.  "Load" should be overwriting these with values specific to the character
            // TODO: Load from database should be loading player data - including inventroy and positions
            PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing);
            PhysicsData.MTableResourceId = 0x09000001u;
            PhysicsData.Stable = 0x20000001u;
            PhysicsData.Petable = 0x34000004u;
            PhysicsData.CSetup = 0x02000001u;

            // radius for object updates
            ListeningRadius = 5f;
        }

        public void Kill()
        {
            // create corpse at location
            // TODO: Once the corpse/container factories have been built

            // teleport to sanctuary or starting point
            if (Positions.ContainsKey(PositionType.Sanctuary))
                Teleport(Positions[PositionType.Sanctuary]);
            else
                Teleport(Positions[PositionType.Location]);

            // create and send the death event
            var yourDeathEvent = new GameEventYourDeath(Session);
            Session.EnqueueSend(yourDeathEvent);
        }

        public async Task Load(Character preloadedCharacter = null)
        {
            character = preloadedCharacter ?? await DatabaseManager.Character.LoadCharacter(Guid.Low);

            Strength = character.StrengthAbility;
            Endurance = character.EnduranceAbility;
            Coordination = character.CoordinationAbility;
            Quickness = character.QuicknessAbility;
            Focus = character.FocusAbility;
            Self = character.SelfAbility;
            Health = character.Health;
            Stamina = character.Stamina;
            Mana = character.Mana;

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

            // TODO: Move this all into Character Creation and store directly in the database.
            if (DatManager.PortalDat.AllFiles.ContainsKey(0x0E000002))
            {
                CharGen cg = CharGen.ReadFromDat(DatManager.PortalDat.GetReaderForFile(0x0E000002));

                int h = Convert.ToInt32(character.PropertiesInt[PropertyInt.HeritageGroup]);
                int s = Convert.ToInt32(character.PropertiesInt[PropertyInt.Gender]);
                SexCG sex = cg.HeritageGroups[h].SexList[s];
                // Set the character basics
                PhysicsData.MTableResourceId = sex.MotionTable;
                PhysicsData.Stable = sex.SoundTable;
                PhysicsData.Petable = sex.PhysicsTable;
                PhysicsData.CSetup = sex.SetupID;
                ModelData.PaletteGuid = sex.BasePalette;

                // Check the character scale
                if (sex.Scale != 100u)
                {
                    // Set the PhysicaData flag to let it know we're changing the scale
                    PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;
                    PhysicsData.ObjScale = sex.Scale / 100f; // Scale is stored as a percentage
                }

                // Get the hair first, because we need to know if you're bald, and that's the name of that tune!
                HairStyleCG hairstyle = sex.HairStyleList[Convert.ToInt32(character.Appearance.HairStyle)];
                bool isBald = hairstyle.Bald;

                // Apply the hair models & texture changes
                for (int i = 0; i < hairstyle.ObjDesc.AnimPartChanges.Count; i++)
                    ModelData.AddModel(hairstyle.ObjDesc.AnimPartChanges[i].PartIndex, hairstyle.ObjDesc.AnimPartChanges[i].PartID);
                for (int i = 0; i < hairstyle.ObjDesc.TextureChanges.Count; i++)
                    ModelData.AddTexture(hairstyle.ObjDesc.TextureChanges[i].PartIndex, hairstyle.ObjDesc.TextureChanges[i].OldTexture, hairstyle.ObjDesc.TextureChanges[i].NewTexture);

                // Eyes only have Texture Changes - eye color is set seperately
                ObjDesc eyes;
                if (hairstyle.Bald)
                    eyes = sex.EyeStripList[Convert.ToInt32(character.Appearance.Eyes)].ObjDescBald;
                else
                    eyes = sex.EyeStripList[Convert.ToInt32(character.Appearance.Eyes)].ObjDesc;
                for (int i = 0; i < eyes.TextureChanges.Count; i++)
                    ModelData.AddTexture(eyes.TextureChanges[i].PartIndex, eyes.TextureChanges[i].OldTexture, eyes.TextureChanges[i].NewTexture);
                
                // Eye color palette
                ModelData.AddPalette(sex.EyeColorList[Convert.ToInt32(character.Appearance.EyeColor)], 0x20, 0x8);

                // Nose only has Texture Changes
                ObjDesc nose = sex.NoseStripList[Convert.ToInt32(character.Appearance.Nose)].ObjDesc;
                for (int i = 0; i < nose.TextureChanges.Count; i++)
                    ModelData.AddTexture(nose.TextureChanges[i].PartIndex, nose.TextureChanges[i].OldTexture, nose.TextureChanges[i].NewTexture);

                // Mouth, suprise, only Texture Changes
                ObjDesc mouth = sex.MouthStripList[Convert.ToInt32(character.Appearance.Mouth)].ObjDesc;
                for (int i = 0; i < mouth.TextureChanges.Count; i++)
                    ModelData.AddTexture(mouth.TextureChanges[i].PartIndex, mouth.TextureChanges[i].OldTexture, mouth.TextureChanges[i].NewTexture);

                // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
                PaletteSet hairPalSet = PaletteSet.ReadFromDat(DatManager.PortalDat.GetReaderForFile(sex.HairColorList[Convert.ToInt32(character.Appearance.HairColor)]));
                // Hue is stored in DB as a percent of the total, so do some math to figure out the int position
                int hairPalIndex = Convert.ToInt32(Convert.ToDouble(hairPalSet.PaletteList.Count - 0.000001) * character.Appearance.HairHue); // Taken from acclient.c (PalSet::GetPaletteID)
                // Since the hue numbers are a little odd, make sure we're in the bounds.
                if (hairPalIndex < 0)
                    hairPalIndex = 0;
                if (hairPalIndex > hairPalSet.PaletteList.Count - 1)
                    hairPalIndex = hairPalSet.PaletteList.Count - 1;
                ushort hairPal = (ushort)(hairPalSet.PaletteList[hairPalIndex] & 0xFFFF); // Convert from 0x04001234 to just 0x1234
                ModelData.AddPalette(hairPal, 0x18, 0x8);

                // Skin is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
                PaletteSet skinPalSet = PaletteSet.ReadFromDat(DatManager.PortalDat.GetReaderForFile(sex.SkinPalSet));
                int skinPalIndex = Convert.ToInt32((skinPalSet.PaletteList.Count - 0.000001) * character.Appearance.SkinHue); // Taken from acclient.c (PalSet::GetPaletteID)
                // Since the hue numbers are a little odd, make sure we're in the bounds.
                if (skinPalIndex < 0)
                    skinPalIndex = 0;
                if (skinPalIndex > skinPalSet.PaletteList.Count - 1)
                    skinPalIndex = skinPalSet.PaletteList.Count - 1;
                ushort skinPal = (ushort)(skinPalSet.PaletteList[skinPalIndex] & 0xFFFF); // Convert from 0x04001234 to just 0x1234
                // Apply the skin palette...
                ModelData.AddPalette(skinPal, 0x0, 0x18);
            }

            IsOnline = true;

            this.TotalLogins = this.character.TotalLogins = this.character.TotalLogins + 1;
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
            Session.EnqueueSend(setTurbineChatChannels, general, trade, lfg, roleplay);
        }

        public void AddToActionQueue(QueuedGameAction action)
        {
            this.actionQueue.Enqueue(action);
        }

        public void AddToExaminationQueue(QueuedGameAction action)
        {
            this.examinationQueue.Enqueue(action);
        }

        public QueuedGameAction ActionQueuePop()
        {
            QueuedGameAction action = null;
            if (this.actionQueue.TryDequeue(out action))
            {
                lock (delayedActionsMutex)
                {
                    if (action.RespectDelay && delayedActions.ContainsKey(action.ObjectId))
                    {
                        double endTime = delayedActions[action.ObjectId];
                        if (action.StartTime > endTime)
                        {
                            // the new action starts after the old one is complete, so remove the old one
                            delayedActions.Remove(action.ObjectId);
                        }
                        else
                        {
                            // the new action should start before the old one is complete, so enqueue the new one again
                            action.StartTime = WorldManager.PortalYearTicks;
                            this.actionQueue.Enqueue(action);
                            return null;
                        }
                    }
                
                    // This is an action e.g. animation that takes time to complete
                    // Remember this object to delay further actions queued for the same object
                    if (action.EndTime > WorldManager.PortalYearTicks)
                    {
                        delayedActions.Add(action.ObjectId, action.EndTime);
                    }
                }
                return action;
            }
            else
                return null;
        }

        public QueuedGameAction ExaminationQueuePop()
        {
            QueuedGameAction action = null;
            if (this.examinationQueue.TryDequeue(out action))
                return action;
            else
                return null;
        }

        /// <summary>
        /// Raise the available XP by a specified amount
        /// </summary>
        /// <param name="amount">A unsigned long containing the desired XP amount to raise</param>
        public void GrantXp(ulong amount)
        {
            // until we are max level we must make sure that we send
            var chart = DatabaseManager.Charts.GetLevelingXpChart();
            CharacterLevel maxLevel = chart.Levels.Last();
            if (character.Level != maxLevel.Level)
            {
                ulong amountLeftToEnd = maxLevel.TotalXp - character.TotalExperience;
                if (amount > amountLeftToEnd)
                {
                    amount = amountLeftToEnd;
                }
                character.GrantXp(amount);
                CheckForLevelup();
                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.TotalExperience, character.TotalExperience);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var message = new GameMessageSystemChat($"{amount} experience granted.", ChatMessageType.Broadcast);
                Session.EnqueueSend(xpTotalUpdate, xpAvailUpdate, message);
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
            if (character.AvailableSkillCredits >= creditsSpent)
            {
                // attempt to train the specified skill
                bool trainNewSkill = character.TrainSkill(skill, creditsSpent);
                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, character.AvailableSkillCredits);
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
                    trainSkillMessageText = $"{SkillExtensions.ToSentence(skill)} trained. You now have {character.AvailableSkillCredits} credits available.";
                }
                else
                {
                    trainSkillMessageText = $"Failed to train {SkillExtensions.ToSentence(skill)}! You now have {character.AvailableSkillCredits} credits available.";
                }

                // create the final game message and send to the client
                var message = new GameMessageSystemChat(trainSkillMessageText, ChatMessageType.Advancement);
                Session.EnqueueSend(trainSkillUpdate, currentCredits, message);
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
            var startingLevel = character.Level;
            var chart = DatabaseManager.Charts.GetLevelingXpChart();
            CharacterLevel maxLevel = chart.Levels.Last();
            bool creditEarned = false;
            if (character.Level == maxLevel.Level) return;

            // increases until the correct level is found
            while (chart.Levels[Convert.ToInt32(character.Level)].TotalXp <= character.TotalExperience)
            {
                character.Level++;
                CharacterLevel newLevel = chart.Levels.FirstOrDefault(item => item.Level == character.Level);
                // increase the skill credits if the chart allows this level to grant a credit
                if (newLevel.GrantsSkillPoint)
                {
                    character.AvailableSkillCredits++;
                    character.TotalSkillCredits++;
                    creditEarned = true;
                }
                // break if we reach max
                if (character.Level == maxLevel.Level)
                {
                    ActionApplyVisualEffect(Network.Enum.PlayScript.WeddingBliss, this.Guid);
                    break;
                }
            }

            if (character.Level > startingLevel)
            {
                string level = $"{character.Level}";
                string skillCredits = $"{character.AvailableSkillCredits}";
                string xpAvailable = $"{character.AvailableExperience:#,###0}";
                var levelUp = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Level, character.Level);
                string levelUpMessageText = (character.Level == maxLevel.Level) ? $"You have reached the maximum level of {level}!" : $"You are now level {level}!";
                var levelUpMessage = new GameMessageSystemChat(levelUpMessageText, ChatMessageType.Advancement);
                string xpUpdateText = (character.AvailableSkillCredits > 0) ? $"You have {xpAvailable} experience points and {skillCredits} skill credits available to raise skills and attributes." : $"You have {xpAvailable} experience points available to raise skills and attributes.";
                var xpUpdateMessage = new GameMessageSystemChat(xpUpdateText, ChatMessageType.Advancement);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, character.AvailableSkillCredits);
                if (character.Level != maxLevel.Level && !creditEarned)
                {
                    string nextCreditAtText = $"You will earn another skill credit at {chart.Levels.Where(item => item.Level > character.Level).OrderBy(item => item.Level).First(item => item.GrantsSkillPoint).Level}";
                    var nextCreditMessage = new GameMessageSystemChat(nextCreditAtText, ChatMessageType.Advancement);
                    Session.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits, nextCreditMessage);
                }
                else
                    Session.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits);
                // play level up effect
                ActionApplyVisualEffect(Network.Enum.PlayScript.LevelUp, this.Guid);
            }
        }

        public void SpendXp(Enum.Ability ability, uint amount)
        {
            uint baseValue = character.Abilities[ability].Base;
            uint result = SpendAbilityXp(character.Abilities[ability], amount);
            bool isSecondary = (ability == Enum.Ability.Health || ability == Enum.Ability.Stamina || ability == Enum.Ability.Mana);
            uint ranks = character.Abilities[ability].Ranks;
            uint newValue = character.Abilities[ability].UnbuffedValue;
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
                    abilityUpdate = new GameMessagePrivateUpdateVital(Session, ability, ranks, baseValue, result, character.Abilities[ability].Current);
                }

                // checks if max rank is achieved and plays fireworks w/ special text
                if (IsAbilityMaxRank(ranks, isSecondary))
                {
                    // fireworks
                    ActionApplyVisualEffect(Network.Enum.PlayScript.WeddingBliss, this.Guid);
                    messageText = $"Your base {ability} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {ability} is now {newValue}!";
                }
                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var soundEvent = new GameMessageSound(this.Guid, Network.Enum.Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);
                Session.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message);
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

            switch (ability.Ability)
            {
                case Enum.Ability.Health:
                case Enum.Ability.Stamina:
                case Enum.Ability.Mana:
                    chart = DatabaseManager.Charts.GetVitalXpChart();
                    addToCurrentValue = true;
                    break;
                default:
                    chart = DatabaseManager.Charts.GetAbilityXpChart();
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
                ability.Current += addToCurrentValue ? rankUps : 0u;
                ability.Ranks += rankUps;
                ability.ExperienceSpent += amount;
                this.character.SpendXp(amount);
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

            if (isAbilityVitals)
                xpChart = DatabaseManager.Charts.GetVitalXpChart();
            else
                xpChart = DatabaseManager.Charts.GetAbilityXpChart();

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

            if (status == SkillStatus.Trained)
                xpChart = DatabaseManager.Charts.GetTrainedSkillXpChart();
            else if (status == SkillStatus.Specialized)
                xpChart = DatabaseManager.Charts.GetSpecializedSkillXpChart();

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
            uint result = SpendSkillXp(character.Skills[skill], amount);

            uint ranks = character.Skills[skill].Ranks;
            uint newValue = character.Skills[skill].UnbuffedValue;
            var status = character.Skills[skill].Status;
            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
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
                    ActionApplyVisualEffect(Network.Enum.PlayScript.WeddingBliss, this.Guid);
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
            Session.EnqueueSend(xpUpdate, skillUpdate, soundEvent, message);
        }

        public void ActionApplySoundEffect(Sound sound, ObjectGuid objectId)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, (uint)sound, GameActionType.ApplySoundEffect);
            AddToActionQueue(action);
        }

        public void ActionApplyVisualEffect(PlayScript effect, ObjectGuid objectId)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, (uint)effect, GameActionType.ApplyVisualEffect);
            AddToActionQueue(action);
        }

        public void EnqueueMovementEvent(GeneralMotion motion, ObjectGuid objectId)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, motion, GameActionType.MovementEvent);
            AddToActionQueue(action);
        }

        public void SendMovementEvent(GeneralMotion motion, WorldObject sender)
        {
            Session.Network.EnqueueSend(new GameMessageUpdateMotion(sender, motion));
        }

        // Play a sound
        public void PlaySound(Sound sound, ObjectGuid targetId)
        {
            Session.EnqueueSend(new GameMessageSound(targetId, sound, 1f));
        }

        // plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(PlayScript effectId, ObjectGuid targetId)
        {
            var effectEvent = new GameMessageScript(targetId, effectId);
            Session.EnqueueSend(effectEvent);
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Not checking and accounting for XP gained from skill usage.
        /// </remarks>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendSkillXp(CharacterSkill skill, uint amount)
        {
            uint result = 0u;
            ExperienceExpenditureChart chart;

            if (skill.Status == SkillStatus.Trained)
                chart = DatabaseManager.Charts.GetTrainedSkillXpChart();
            else if (skill.Status == SkillStatus.Specialized)
                chart = DatabaseManager.Charts.GetSpecializedSkillXpChart();
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
                this.character.SpendXp(amount);
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
                    friendSession.EnqueueSend(new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, GetVirtualOnlineStatus()));
                }
            }
        }

        /// <summary>
        /// Adds a friend and updates the database.
        /// </summary>
        /// <param name="friendName">The name of the friend that is being added.</param>
        public async Task<AddFriendResult> AddFriend(string friendName)
        {
            if (string.Equals(friendName, Name, StringComparison.CurrentCultureIgnoreCase))
                return AddFriendResult.FriendWithSelf;

            // Check if friend exists
            if (character.Friends.SingleOrDefault(f => string.Equals(f.Name, friendName, StringComparison.CurrentCultureIgnoreCase)) != null)
                return AddFriendResult.AlreadyInList;

            // TODO: check if player is online first to avoid database hit??
            // Get character record from DB
            Character friendCharacter = await DatabaseManager.Character.GetCharacterByName(friendName);

            if (friendCharacter == null)
                return AddFriendResult.CharacterDoesNotExist;

            Friend newFriend = new Friend();
            newFriend.Name = friendCharacter.Name;
            newFriend.Id = new ObjectGuid(friendCharacter.Id, GuidType.Player);

            // Save to DB
            await DatabaseManager.Character.AddFriend(Guid.Low, newFriend.Id.Low);

            // Add to character object
            character.AddFriend(newFriend);

            // Send packet
            Session.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));

            return AddFriendResult.Success;
        }

        /// <summary>
        /// Remove a single friend and update the database.
        /// </summary>
        /// <param name="friendId">The ObjectGuid of the friend that is being removed</param>
        public async Task<RemoveFriendResult> RemoveFriend(ObjectGuid friendId)
        {
            Friend friendToRemove = character.Friends.SingleOrDefault(f => f.Id.Low == friendId.Low);

            // Not in friend list
            if (friendToRemove == null)
                return RemoveFriendResult.NotInFriendsList;

            // Remove from DB
            await DatabaseManager.Character.DeleteFriend(Guid.Low, friendId.Low);

            // Remove from character object
            character.RemoveFriend(friendId.Low);

            // Send packet
            Session.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));

            return RemoveFriendResult.Success;
        }

        /// <summary>
        /// Delete all friends and update the database.
        /// </summary>
        public async void RemoveAllFriends()
        {
            // Remove all from DB
            await DatabaseManager.Character.RemoveAllFriends(Guid.Low);

            // Remove from character object
            character.RemoveAllFriends();
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
            character.SetCharacterOption(option, value);
        }

        /// <summary>
        /// Saves options to the database.  Options include things like spell tabs, settings (F11), chat windows, etc.
        /// </summary>
        public void SaveOptions()
        {
            if (character != null)
                DatabaseManager.Character.SaveCharacterOptions(character);

            // TODO: Save other options as we implement them.
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
        public void SetCharacterPosition(Position newPosition)
        {
            // Some positions come from outside of the Player and Character classes
            if (newPosition.CharacterId == 0) newPosition.CharacterId = Guid.Low;

            // reset the landblock id
            if (newPosition.LandblockId.Landblock == 0 && newPosition.Cell > 0)
            {
                newPosition.LandblockId = new LandblockId(newPosition.Cell);
            }

            character.SetCharacterPosition(newPosition);
            DatabaseManager.Character.SaveCharacterPosition(character, newPosition);
        }

        /// <summary>
        /// Saves a CharacterPosition to the character position dictionary
        /// </summary>
        public void SetCharacterPosition(PositionType type, Position newPosition)
        {
            newPosition.PositionType = type;
            SetCharacterPosition(newPosition);
        }

        /// <summary>
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.
        /// </summary>
        public void SaveCharacter()
        {
            if (character != null)
            {
                // Save the current position to persistent storage, only durring the server update interval
                SetPhysicalCharacterPosition();
                DatabaseManager.Character.UpdateCharacter(character);
#if DEBUG
                if (Session.Player != null)
                {
                    Session.EnqueueSend(new GameMessageSystemChat($"{Session.Player.Name} has been saved.", ChatMessageType.Broadcast));
                }
#endif
            }
        }

        public void UpdateAge()
        {
            if (character != null)
                character.Age++;
        }

        public void SendAgeInt()
        {
            try
            {
                Session.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Age, character.Age));
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
            if (character.CharacterOptions[CharacterOption.AppearOffline] == true)
                return false;

            return IsOnline;
        }

        private void SendSelf()
        {
            Session.EnqueueSend(new GameMessageCreateObject(this), new GameMessagePlayerCreate(Guid));
            // TODO: gear and equip

            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.EnqueueSend(player, title, friends);
        }

        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsData.PhysicsState = state;

            if (packet)
            {
                Session.EnqueueSend(new GameMessageSetState(this, state));
                // TODO: this should be broadcast
            }
        }

        public void Teleport(Position newPosition)
        {
            if (!InWorld)
                return;

            InWorld = false;
            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide);

            Session.EnqueueSend(new GameMessagePlayerTeleport(this));

            lock (clientObjectMutex)
            {
                clientObjectList.Clear();

                Session.Player.Location = newPosition;
                SetPhysicalCharacterPosition();
            }

            DelayedUpdatePosition(newPosition);
        }

        public void UpdatePosition(Position newPosition)
        {
            this.Location = newPosition;
            // character.SetCharacterPosition(newPosition);
            SendUpdatePosition();
        }

        private void DelayedUpdatePosition(Position newPosition)
        {
            var t = new Thread(() =>
            {
                Thread.Sleep(10);
                this.Location = newPosition;
                // character.SetCharacterPosition(newPosition);
                SendUpdatePosition();
            });
            t.Start();
        }

        public void SetTitle(uint title)
        {
            var updateTitle = new GameEventUpdateTitle(Session, title);
            var message = new GameMessageSystemChat($"Your title is now {title}!", ChatMessageType.Broadcast);
            Session.EnqueueSend(updateTitle, message);
        }

        public void ReceiveChat(WorldObject sender, ChatMessageArgs e)
        {
            // TODO: Implement
        }

        /// <summary>
        /// returns a list of the ObjectGuids of all known creatures
        /// </summary>
        public List<ObjectGuid> GetKnownCreatures()
        {
            lock (clientObjectMutex)
            {
                return (List<ObjectGuid>)clientObjectList.Select(x => x.Key).Where(o => o.IsCreature()).ToList();
            }
        }

        /// <summary>
        /// forces either an update or a create object to be sent to the client
        /// </summary>
        public void TrackObject(WorldObject worldObject)
        {
            bool sendUpdate = true;

            if (worldObject.Guid == this.Guid)
                return;

            lock (clientObjectMutex)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);

                // check for a short circuit.  if we don't need to update, don't!
                // TODO: Fix this
                // I had to comment this optimization out for the moment - not sure how it works but it never lets us update or add. Og
                
                // if (sendUpdate)
                //   if (worldObject.LastUpdatedTicks < clientObjectList[worldObject.Guid])
                //       return;

                if (!sendUpdate)
                {
                    clientObjectList.Add(worldObject.Guid, WorldManager.PortalYearTicks);
                    worldObject.PlayScript(this.Session);
                }

                else
                    clientObjectList[worldObject.Guid] = WorldManager.PortalYearTicks;
            }

            log.Debug($"Telling {Name} about {worldObject.Name} - {worldObject.Guid.Full.ToString("X")}");

            if (sendUpdate)
                Session.EnqueueSend(new GameMessageUpdateObject(worldObject));
            else
                Session.EnqueueSend(new GameMessageCreateObject(worldObject));
        }

        /// <summary>
        /// Do the player log out work.<para />
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </summary>
        public void Logout(bool clientSessionTerminatedAbruptly = false)
        {
            if (!IsOnline)
                return;

            InWorld = false;
            IsOnline = false;

            SendFriendStatusUpdates();

            // remove the player from landblock management
            LandblockManager.RemoveObject(this);

            if (!clientSessionTerminatedAbruptly)
            {
                var logout = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.LogOut));
                Session.EnqueueSend(new GameMessageUpdateMotion(this, Session, logout));

                SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);

                // Thie retail server sends a ChatRoomTracker 0x0295 first, then the status message, 0x028B. It does them one at a time for each individual channel.
                // The ChatRoomTracker message doesn't seem to change at all.
                // For the purpose of ACE, we simplify this process.
                var general = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "General");
                var trade = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "Trade");
                var lfg = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "LFG");
                var roleplay = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "Roleplay");
                Session.EnqueueSend(general, trade, lfg, roleplay);
            }
        }

        public void StopTrackingObject(WorldObject worldObject)
        {
            bool sendUpdate = true;
            lock (clientObjectMutex)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);

                if (!sendUpdate)
                {
                    clientObjectList.Remove(worldObject.Guid);
                }
            }

            if (sendUpdate)
            {
                Session.EnqueueSend(new GameMessageRemoveObject(worldObject));
            }
        }

        public void SendUpdatePosition()
        {
            this.LastMovementBroadcastTicks = WorldManager.PortalYearTicks;
            Session.EnqueueSend(new GameMessageUpdatePosition(this));
        }

        public void SendAutonomousPosition()
        {
            // Session.EnqueueSend(new GameMessageAutonomousPosition(this));
        }

        public bool WaitingForDelayedTeleport { get; set; } = false;
        public Position DelayedTeleportDestination { get; private set; } = null;
        public DateTime DelayedTeleportTime { get; private set; } = DateTime.MinValue;

        public void SetDelayedTeleport(TimeSpan delay, Position destination)
        {
            DelayedTeleportDestination = destination;
            DelayedTeleportTime = DateTime.UtcNow + delay;
            WaitingForDelayedTeleport = true;
        }

        public void ClearDelayedTeleport()
        {
            DelayedTeleportDestination = null;
            DelayedTeleportTime = DateTime.MinValue;
            WaitingForDelayedTeleport = false;
        }
    }
}
