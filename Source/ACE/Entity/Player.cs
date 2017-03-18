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

namespace ACE.Entity
{
    public class Player : MutableWorldObject
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

        public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions
        {
            get { return character.CharacterOptions; }
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

        /// <summary>
        /// Accessor for strength.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Strength
        {
            get { return character.Strength; }
        }

        /// <summary>
        /// Accessor for Endurance.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Endurance
        {
            get { return character.Endurance; }
        }

        /// <summary>
        /// Accessor for Coordination.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Coordination
        {
            get { return character.Coordination; }
        }

        /// <summary>
        /// Accessor for Quickness.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Quickness
        {
            get { return character.Quickness; }
        }

        /// <summary>
        /// Accessor for Focus.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Focus
        {
            get { return character.Focus; }
        }

        /// <summary>
        /// Accessor for Self.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Self
        {
            get { return character.Self; }
        }

        /// <summary>
        /// Accessor for Health.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Health
        {
            get { return character.Health; }
        }

        /// <summary>
        /// Accessor for Stamina.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Stamina
        {
            get { return character.Stamina; }
        }

        /// <summary>
        /// Accessor for Mana.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Mana
        {
            get { return character.Mana; }
        }

        public uint TotalLogins
        {
            get { return character.TotalLogins; }
            set { character.TotalLogins = value; }
        }

        public Player(Session session) : base(ObjectType.Creature, session.CharacterRequested.Guid)
        {
            Session = session;
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Player | ObjectDescriptionFlag.Attackable;
            Name = session.CharacterRequested.Name;
            Icon = 0x1036;
            GameData.ItemCapacity = 102;  
            GameData.ContainerCapacity = 7;
            GameData.RadarBehavior = RadarBehavior.ShowAlways;
            GameData.RadarColour = RadarColor.White;
            GameData.Usable = Usable.UsableObjectSelf;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);
            PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position;
            WeenieFlags = WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar;

            // apply defaults.  "Load" should be overwriting these with values specific to the character
            PhysicsData.MTableResourceId = 0x09000001u;
            PhysicsData.Stable = 0x20000001u;
            PhysicsData.Petable = 0x34000004u;
            PhysicsData.CSetup = 0x02000001u;

            // radius for object updates
            ListeningRadius = 5f;
        }

        public async Task Load(Character preloadedCharacter = null)
        {
            character = preloadedCharacter ?? await DatabaseManager.Character.LoadCharacter(Guid.Low);

            if (Common.ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (Session.AccessLevel == AccessLevel.Admin)
                    character.IsAdmin = true;
                if (Session.AccessLevel == AccessLevel.Developer)
                    character.IsArch = true;
                if (Session.AccessLevel == AccessLevel.Envoy)
                    character.IsEnvoy = true;
                //TODO: Need to setup and account properly for IsSentinel and IsAdvocate.
                //if (Session.AccessLevel == AccessLevel.Sentinel)
                //    character.IsSentinel = true;
                //if (Session.AccessLevel == AccessLevel.Advocate)
                //    character.IsAdvocate= true;
            }

            Position = character.Position;
            IsOnline = true;

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

        /// <summary>
        /// Raise the available XP by a specified amount
        /// </summary>
        /// <param name="amount">A unsigned long containing the desired XP amount to raise</param>
        public void GrantXp(ulong amount)
        {
            //until we are max level we must make sure that we send 
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
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate, message);
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
                //increase the skill credits if the chart allows this level to grant a credit
                if (newLevel.GrantsSkillPoint)
                {
                    character.AvailableSkillCredits++;
                    character.TotalSkillCredits++;
                    creditEarned = true;
                }
                //break if we reach max
                if (character.Level == maxLevel.Level)
                {
                    PlayParticleEffect(Effect.WeddingBliss);
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
                string xpUpdateText = (character.AvailableSkillCredits > 0) ? $"You have {xpAvailable} experience points and {skillCredits} skill credits available to raise skills and attributes." : $"You have {xpAvailable} experience points available to raise skills and attributes."; ;
                var xpUpdateMessage = new GameMessageSystemChat(xpUpdateText, ChatMessageType.Advancement);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, character.AvailableSkillCredits);
                if (character.Level != maxLevel.Level && !creditEarned)
                {
                    string nextCreditAtText = $"You will earn another skill credit at {chart.Levels.Where(item => item.Level > character.Level).OrderBy(item => item.Level).First(item => item.GrantsSkillPoint).Level}";
                    var nextCreditMessage = new GameMessageSystemChat(nextCreditAtText, ChatMessageType.Advancement);
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits, nextCreditMessage);
                } else
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits);
                //play level up effect
                PlayParticleEffect(Effect.LevelUp);
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

                //checks if max rank is achieved and plays fireworks w/ special text
                if (IsAbilityMaxRank(ranks, isSecondary)) {
                    //fireworks
                    PlayParticleEffect(Effect.WeddingBliss);
                    messageText = $"Your base {ability} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {ability} is now {newValue}!";
                }
                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var soundEvent = new GameMessageSound(this.Guid, Network.Enum.Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);
                Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message);
            }
            else
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {ability} has failed.", ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Attributes are possibly out of sequence:
        ///             When you try to raise a primary attribute (Strength, Endurance, Coordination, Quickness
        ///             Focus, or Self) past 121 rank points, the client will not accept any more points and prevents
        ///             any other attributes from advancing.
        /// </remarks>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendAbilityXp(CharacterAbility ability, uint amount)
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

            //do not advance if we cannot spend xp to rank up our skill by 1 point
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
                //if the skill ranks out at the top of our xp chart 
                //then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(ranks, status))
                {
                    //fireworks on rank up is 0x8D
                    PlayParticleEffect(Effect.WeddingBliss);
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

        //plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(Effect effectId)
        {
            var effectEvent = new GameMessageEffect(this.Guid, effectId);
            Session.Network.EnqueueSend(effectEvent);
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
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

            //do not advance if we cannot spend xp to rank up our skill by 1 point
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
                    friendSession.Network.EnqueueSend(new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, GetVirtualOnlineStatus()));
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
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));

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
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));

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

        public void SaveCharacter()
        {
            if (character != null)
            {
                DatabaseManager.Character.UpdateCharacter(character);
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
            try
            {
                character.Age++;
            }
            catch (NullReferenceException)
            {

            }
        }

        public void SendAgeInt()
        {
            try
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Age, character.Age));
            }
            catch (NullReferenceException)
            {

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
            Session.Network.EnqueueSend(new GameMessageCreateObject(this), new GameMessagePlayerCreate(Guid));
            // TODO: gear and equip

            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);
        }
        
        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsData.PhysicsState = state;

            if (packet)
            {
                Session.Network.EnqueueSend(new GameMessageSetState(Guid, state, character.TotalLogins, ++PortalIndex));
                // TODO: this should be broadcast
            }
        }

        public void Teleport(Position newPosition)
        {
            if (!InWorld)
                return;

            InWorld = false;
            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide);

            Session.Network.EnqueueSend(new GameMessagePlayerTeleport(++TeleportIndex));

            lock(clientObjectMutex)
            {
                clientObjectList.Clear();
            }

            DelayedUpdatePosition(newPosition);
        }

        public void UpdatePosition(Position newPosition)
        {
            this.Position = newPosition;
            SendUpdatePosition();
        }

        private void DelayedUpdatePosition(Position newPosition)
        {
            var t = new Thread(() => { Thread.Sleep(10); this.Position = newPosition; SendUpdatePosition(); });
            t.Start();
        }

        public void SetTitle(uint title)
        {
            var updateTitle = new GameEventUpdateTitle(Session, title);
            var message = new GameMessageSystemChat($"Your title is now {title}!", ChatMessageType.Broadcast);
            Session.Network.EnqueueSend(updateTitle, message);
        }

        public void ObjectMoved(MutableWorldObject sender)
        {
            Session.Network.EnqueueSend(new GameMessageUpdatePosition(sender));
        }

        public void ReceiveChat(WorldObject sender, ChatMessageArgs e)
        {
            // TODO: Implement
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
                if (sendUpdate)
                    if (worldObject.LastUpdatedTicks < clientObjectList[worldObject.Guid])
                        return;

                if (!sendUpdate)
                    clientObjectList.Add(worldObject.Guid, WorldManager.PortalYearTicks);
                else
                    clientObjectList[worldObject.Guid] = WorldManager.PortalYearTicks;
            }

            log.Debug($"Telling {Name} about {worldObject.Name}");

            if (sendUpdate)
                Session.Network.EnqueueSend(new GameMessageUpdateObject(worldObject));
            else
                Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));
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

            // NOTE: Adding this here for now because some chracter options do not trigger the GameActionSetCharacterOptions packet to fire when apply is clicked (which is where we are currently saving to the db).
            // Once we get a CharacterSave method, we might consider removing this and putting it in that method instead.
            //DatabaseManager.Character.SaveCharacterOptions(character);

            //DatabaseManager.Character.UpdateCharacter(character);

            if (!clientSessionTerminatedAbruptly)
            {
                // TODO: Evt_Movement__MovementEvent_ID = F74C

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

        public void StopTrackingObject(ObjectGuid objectId)
        {
            bool sendUpdate = true;
            lock (clientObjectMutex)
            {
                sendUpdate = clientObjectList.ContainsKey(objectId);

                if (!sendUpdate)
                {
                    clientObjectList.Remove(objectId);
                }
            }

            if (sendUpdate)
            {
                Session.Network.EnqueueSend(new GameMessageRemoveObject(objectId));
            }
        }

        public void SendUpdatePosition()
        {
            this.LastMovementBroadcastTicks = WorldManager.PortalYearTicks;
            Session.Network.EnqueueSend(new GameMessageUpdatePosition(this));
        }

        public void SendAutonomousPosition()
        {
            
            // Session.Network.EnqueueSend(new GameMessageAutonomousPosition(this));
        }
    }
}
