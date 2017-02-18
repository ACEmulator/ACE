using System;
using System.Collections.ObjectModel;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.Fragments;
using ACE.Network.GameEvent;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACE.Managers;
using ACE.Network.Enum;

namespace ACE.Entity
{
    public class Player : MutableWorldObject
    {
        // all the objects being tracked
        private Dictionary<ObjectGuid, MutableWorldObject> subscribedObjects = new Dictionary<ObjectGuid, MutableWorldObject>();
        
        public Session Session { get; }

        public bool InWorld { get; set; }
        public bool IsOnline { get; private set; }  // Different than InWorld which is false when in portal space
        
        public uint PortalIndex { get; set; } = 1u; // amount of times this character has left a portal this session
        
        private Character character;

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
            Session           = session;
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Player | ObjectDescriptionFlag.Attackable;
            Name              = session.CharacterRequested.Name;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);

            // radius for object updates
            ListeningRadius = 5f;
        }
        
        public async void Load()
        {
            character = await DatabaseManager.Character.LoadCharacter(Guid.Low);
            Position  = character.Position;
            IsOnline = true;

            SendSelf();
            SendFriendStatusUpdates();
        }

        public void GrantXp(ulong amount)
        {
            character.GrantXp(amount);
            var xpAvailUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
            var xpTotalUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.TotalExperience, character.TotalExperience);

            xpAvailUpdate.Send();
            xpTotalUpdate.Send();
            ChatPacket.SendSystemMessage(Session, $"{amount} experience granted.");
        }

        private void CheckForLevelup()
        {
            var chart = DatabaseManager.Charts.GetLevelingXpChart();

            // TODO: implement.  just stubbing for now, will implement later.
        }

        public void SpendXp(Enum.Ability ability, uint amount)
        {
            uint baseValue = character.Abilities[ability].Base;
            uint result = SpendAbilityXp(character.Abilities[ability], amount);
            bool isSecondary = (ability == Enum.Ability.Health || ability == Enum.Ability.Stamina || ability == Enum.Ability.Mana);
            if (result > 0u)
            {
                uint ranks = character.Abilities[ability].Ranks;
                uint newValue = character.Abilities[ability].UnbuffedValue;
                var xpUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                GameEventPacket abilityUpdate;
                if (!isSecondary)
                {
                    abilityUpdate = new GameEventPrivateUpdateAbility(Session, ability, ranks, baseValue, result);
                }
                else
                {
                    abilityUpdate = new GameEventPrivateUpdateVital(Session, ability, ranks, baseValue, result, character.Abilities[ability].Current);
                }

                var soundEvent = new GameEventSound(Session, Network.Enum.Sound.AbilityIncrease, 1f);

                xpUpdate.Send();
                abilityUpdate.Send();
                soundEvent.Send();
                ChatPacket.SendSystemMessage(Session, $"Your base {ability} is now {newValue}!");
            }
            else
            {
                ChatPacket.SendSystemMessage(Session, $"Your attempt to raise {ability} has failed.");
            }
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. +10 skill throws an exception when it would go outside the bounds of ranks list
        ///         2. the client doesn't increase the "next point" amount properly when using +10
        ///         3. no fireworks for hitting max ranks
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

            uint rankUps = 0u;
            uint currentXp = chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            uint rank1 = chart.Ranks[Convert.ToInt32(ability.Ranks) + 1].XpFromPreviousRank;
            uint rank10 = chart.Ranks[Convert.ToInt32(ability.Ranks) + 10].TotalXp - chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
                rankUps = 10u;

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

        public void SpendXp(Skill skill, uint amount)
        {
            uint baseValue = 0;
            uint result = SpendSkillXp(character.Skills[skill], amount);
            if (result > 0u)
            {
                uint ranks = character.Skills[skill].Ranks;
                uint newValue = character.Skills[skill].UnbuffedValue;
                var status = character.Skills[skill].Status;
                var xpUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var ablityUpdate = new GameEventPrivateUpdateSkill(Session, skill, status, ranks, baseValue, result);
                var soundEvent = new GameEventSound(Session, Network.Enum.Sound.AbilityIncrease, 1f);

                xpUpdate.Send();
                ablityUpdate.Send();
                soundEvent.Send();
                ChatPacket.SendSystemMessage(Session, $"Your base {skill} is now {newValue}!");
            }
            else
            {
                ChatPacket.SendSystemMessage(Session, $"Your attempt to raise {skill} has failed.");
            }
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. +10 skill throws an exception when it would go outside the bounds of ranks list
        ///         2. the client doesn't increase the "next point" amount properly when using +10
        ///         3. no fireworks for hitting max ranks
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

            uint rankUps = 0u;
            uint currentXp = chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            uint rank1 = chart.Ranks[Convert.ToInt32(skill.Ranks) + 1].XpFromPreviousRank;
            uint rank10 = chart.Ranks[Convert.ToInt32(skill.Ranks) + 10].TotalXp - chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
                rankUps = 10u;

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
                    new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, IsOnline).Send();
                }
            }
        }

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
            new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend).Send();

            return AddFriendResult.Success;
        }

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
            new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove).Send();

            return RemoveFriendResult.Success;
        }

        public async void RemoveAllFriends()
        {
            // Remove all from DB
            await DatabaseManager.Character.RemoveAllFriends(Guid.Low);

            // Remove from character object
            character.RemoveAllFriends();
        }

        public void ChangeOnlineStatus(bool isOnline)
        {
            IsOnline = isOnline;
            SendFriendStatusUpdates();
        }

        private void SendSelf()
        {
            NetworkManager.SendPacket(ConnectionType.World, BuildObjectCreate(), Session);

            var playerCreate         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var playerCreateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.PlayerCreate);
            playerCreateFragment.Payload.WriteGuid(Guid);
            playerCreate.Fragments.Add(playerCreateFragment);

            NetworkManager.SendPacket(ConnectionType.World, playerCreate, Session);

            // TODO: gear and equip

            new GameEventPlayerDescription(Session).Send();
            new GameEventCharacterTitle(Session).Send();
            new GameEventFriendsListUpdate(Session).Send();
        }
        
        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsState = state;

            if (packet)
            {
                var setState         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
                var setStateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.SetState);
                setStateFragment.Payload.WriteGuid(Guid);
                setStateFragment.Payload.Write((uint)state);
                setStateFragment.Payload.Write((ushort)character.TotalLogins);
                setStateFragment.Payload.Write((ushort)++PortalIndex);
                setState.Fragments.Add(setStateFragment);

                // TODO: this should be broadcast
                NetworkManager.SendPacket(ConnectionType.World, setState, Session);
            }
        }

        public void Teleport(Position newPosition)
        {
            if (!InWorld)
                return;

            InWorld = false;
            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide);

            var playerTeleport         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var playerTeleportFragment = new ServerPacketFragment(0x0A, FragmentOpcode.PlayerTeleport);
            playerTeleportFragment.Payload.Write(++TeleportIndex);
            playerTeleportFragment.Payload.Write(0u);
            playerTeleportFragment.Payload.Write(0u);
            playerTeleportFragment.Payload.Write((ushort)0);
            playerTeleport.Fragments.Add(playerTeleportFragment);

            NetworkManager.SendPacket(ConnectionType.World, playerTeleport, Session);

            // must be sent after the teleport packet
            UpdatePosition(newPosition);
        }

        public void SetTitle(uint title)
        {
            var updateTitle = new GameEventUpdateTitle(Session, title);
            updateTitle.Send();

            ChatPacket.SendSystemMessage(Session, $"Your title is now {title}!");
        }

        public void Subscribe(MutableWorldObject worldObject)
        {
            subscribedObjects.Add(worldObject.Guid, worldObject);
        }

        public void Unsubscribe(ObjectGuid objectId)
        {
            if (subscribedObjects.ContainsKey(objectId))
            {
                subscribedObjects.Remove(objectId);

                // TODO: send a destroy packet
            }
        }
        
        /// <summary>
        /// Stuff to do when player logs out
        /// </summary>
        public void Logout()
        {
            IsOnline = false;
            SendFriendStatusUpdates();
        }

    }
}
