using System;
using System.Collections.Generic;
using System.Linq;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        // =====================================
        // Character Options
        // =====================================

        public bool GetCharacterOption(CharacterOption option)
        {
            var option1 = option.GetCharacterOptions1Attribute();

            if (option1 != null)
                return GetCharacterOptions1(option1.Option);

            return GetCharacterOptions2(option.GetCharacterOptions2Attribute().Option);
        }

        private bool GetCharacterOptions1(CharacterOptions1 option)
        {
            return (Character.CharacterOptions1 & (int)option) != 0;
        }

        private bool GetCharacterOptions2(CharacterOptions2 option)
        {
            return (Character.CharacterOptions2 & (int)option) != 0;
        }

        public void SetCharacterOption(CharacterOption option, bool value)
        {
            var option1 = option.GetCharacterOptions1Attribute();

            if (option1 != null)
                SetCharacterOptions1(option1.Option, value);
            else
                SetCharacterOptions2(option.GetCharacterOptions2Attribute().Option, value);
        }

        private void SetCharacterOptions1(CharacterOptions1 option, bool value)
        {
            var options = Character.CharacterOptions1;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetCharacterOptions1(options);
        }

        private void SetCharacterOptions2(CharacterOptions2 option, bool value)
        {
            var options = Character.CharacterOptions2;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetCharacterOptions2(options);
        }

        public void SetCharacterOptions1(int value)
        {
            CharacterDatabaseLock.EnterWriteLock();
            try
            {
                Character.CharacterOptions1 = value;
                CharacterChangesDetected = true;
            }
            finally
            {
                CharacterDatabaseLock.ExitWriteLock();
            }
        }

        public void SetCharacterOptions2(int value)
        {
            CharacterDatabaseLock.EnterWriteLock();
            try
            {
                Character.CharacterOptions2 = value;
                CharacterChangesDetected = true;
            }
            finally
            {
                CharacterDatabaseLock.ExitWriteLock();
            }
        }

        public void SetCharacterGameplayOptions(byte[] value)
        {
            CharacterDatabaseLock.EnterWriteLock();
            try
            {
                Character.GameplayOptions = value;
                CharacterChangesDetected = true;
            }
            finally
            {
                CharacterDatabaseLock.ExitWriteLock();
            }
        }


        // =====================================
        // CharacterPropertiesContract
        // =====================================


        // =====================================
        // CharacterPropertiesFillCompBook
        // =====================================


        // =====================================
        // Friends
        // =====================================

        /// <summary>
        /// Adds a friend and updates the database.
        /// </summary>
        /// <param name="friendName">The name of the friend that is being added.</param>
        public void HandleActionAddFriend(string friendName)
        {
            if (string.Equals(friendName, Name, StringComparison.CurrentCultureIgnoreCase))
            {
                ChatPacket.SendServerMessage(Session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);
                return;
            }

            // get friend player info
            var friend = PlayerManager.FindByName(friendName);

            if (friend == null)
            {
                ChatPacket.SendServerMessage(Session, "That character does not exist", ChatMessageType.Broadcast);
                return;
            }

            var newFriend = Character.AddFriend(friend.Guid.Full, CharacterDatabaseLock, out var friendAlreadyExists);

            if (friendAlreadyExists)
            {
                ChatPacket.SendServerMessage(Session, "That character is already in your friends list", ChatMessageType.Broadcast);
                return;
            }

            CharacterChangesDetected = true;

            // send network message
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));

            ChatPacket.SendServerMessage(Session, $"{friend.Name} has been added to your friends list.", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Remove a single friend and update the database.
        /// </summary>
        /// <param name="friendGuid">The ObjectGuid of the friend that is being removed</param>
        public void HandleActionRemoveFriend(uint friendGuid)
        {
            if (!Character.TryRemoveFriend(friendGuid, out var friendToRemove, CharacterDatabaseLock))
            {
                ChatPacket.SendServerMessage(Session, "That character is not in your friends list!", ChatMessageType.Broadcast);
                return;
            }

            CharacterChangesDetected = true;

            // send network message
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));

            // get friend player info
            var friend = PlayerManager.FindByGuid(friendToRemove.FriendId);

            if (friend == null) // shouldn't happen
                ChatPacket.SendServerMessage(Session, "Friend has been removed from your friends list.", ChatMessageType.Broadcast);
            else
                ChatPacket.SendServerMessage(Session, $"{friend.Name} has been removed from your friends list.", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Delete all friends and update the database.
        /// </summary>
        public void HandleActionRemoveAllFriends()
        {
            // Remove all from DB
            if (Character.ClearAllFriends(CharacterDatabaseLock))
            {
                //ChatPacket.SendServerMessage(Session, "Your friends list has been cleared.", ChatMessageType.Broadcast);
                CharacterChangesDetected = true;
            }
        }

        public bool GetAppearOffline()
        {
            return GetCharacterOption(CharacterOption.AppearOffline);
        }

        /// <summary>
        /// Set the AppearOffline option to the provided value.  It will also send out an update to all online clients that have this player as a friend. This option does not save to the database.
        /// </summary>
        public void SetAppearOffline(bool appearOffline)
        {
            var previousAppearOffline = GetAppearOffline();
            SetCharacterOption(CharacterOption.AppearOffline, appearOffline);
            SendFriendStatusUpdates(!previousAppearOffline, !GetAppearOffline());
        }


        // =====================================
        // CharacterPropertiesQuestRegistry
        // =====================================


        // =====================================
        // CharacterPropertiesShortcutBar
        // =====================================

        public List<Shortcut> GetShortcuts()
        {
            var shortcuts = new List<Shortcut>();

            foreach (var shortcut in Character.GetShortcuts(CharacterDatabaseLock))
                shortcuts.Add(new Shortcut(shortcut));

            return shortcuts;
        }

        /// <summary>
        /// Handles the adding of items to 1-9 shortcut bar in lower-right corner.<para />
        /// Note that there are two rows. The top row is 1-9, the bottom row has no hotkeys.
        /// </summary>
        public void HandleActionAddShortcut(Shortcut shortcut)
        {
            // When a shortcut is added on top of an existing item, the client automatically sends the RemoveShortcut command for that existing item first, then will add the new item, and re-add the existing item to the appropriate place.

            Character.AddOrUpdateShortcut(shortcut.Index, shortcut.ObjectId, CharacterDatabaseLock);
            CharacterChangesDetected = true;
        }

        /// <summary>
        /// Handles the removing of items from 1-9 shortcut bar in lower-right corner
        /// </summary>
        public void HandleActionRemoveShortcut(uint index)
        {
            if (Character.TryRemoveShortcut(index, out _, CharacterDatabaseLock))
                CharacterChangesDetected = true;
        }


        // =====================================
        // Spell Bar
        // =====================================

        /// <summary>
        /// Will return the spells in the bar, sorted by their position
        /// </summary>
        public List<SpellBarPositions> GetSpellsInSpellBar(int barId)
        {
            var spells = new List<SpellBarPositions>();

            var results = Character.GetSpellsInBar(barId, CharacterDatabaseLock);

            foreach (var result in results)
            {
                var entity = new SpellBarPositions(result.SpellBarNumber, result.SpellBarIndex, result.SpellId);

                spells.Add(entity);
            }

            //spells.Sort((a, b) => a.SpellBarPositionId.CompareTo(b.SpellBarPositionId));

            return spells;
        }

        /// <summary>
        /// This method implements player spell bar management for - adding a spell to a specific spell bar (0 based) at a specific slot (0 based).
        /// </summary>
        public void HandleActionAddSpellFavorite(uint spellId, uint spellBarPositionId, uint spellBarId)
        {
            if (spellBarId > 7 || spellBarPositionId > (uint)SpellId.NumSpells || !SpellIsKnown(spellId))
                return;

            if (Character.AddSpellToBar(spellBarId, spellBarPositionId, spellId, CharacterDatabaseLock))
                CharacterChangesDetected = true;
        }

        /// <summary>
        /// This method implements player spell bar management for - removing a spell to a specific spell bar (0 based)
        /// </summary>
        public void HandleActionRemoveSpellFavorite(uint spellId, uint spellBarId)
        {
            if (Character.TryRemoveSpellFromBar(spellBarId, spellId, out _, CharacterDatabaseLock))
                CharacterChangesDetected = true;
        }


        // =====================================
        // CharacterPropertiesTitleBook
        // =====================================

        /// <summary>
        /// Add Title to Title Registry
        /// </summary>
        /// <param name="titleId">Id of Title to Add</param>
        /// <param name="setAsDisplayTitle">If this is true, make this the player's current title</param>
        public void AddTitle(uint titleId, bool setAsDisplayTitle = false)
        {
            if (!Enum.IsDefined(typeof(CharacterTitle), titleId))
                return;

            Character.AddTitleToRegistry(titleId, CharacterDatabaseLock, out var titleAlreadyExists, out var numCharacterTitles);

            bool sendMsg = false;
            bool notifyNewTitle = false;

            if (!titleAlreadyExists)
            {
                CharacterChangesDetected = true;

                NumCharacterTitles = numCharacterTitles;

                sendMsg = true;
                notifyNewTitle = true;

            }

            if (setAsDisplayTitle && CharacterTitleId != titleId)
            {
                CharacterTitleId = (int)titleId;
                sendMsg = true;
            }

            if (sendMsg && FirstEnterWorldDone)
            {
                Session.Network.EnqueueSend(new GameEventUpdateTitle(Session, titleId, setAsDisplayTitle));

                if (notifyNewTitle)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You have been granted a new title."));
            }
        }

        public void AddTitle(CharacterTitle title, bool setAsDisplayTitle = false)
        {
            AddTitle((uint)title, setAsDisplayTitle);
        }

        public void HandleActionSetTitle(uint title)
        {
            AddTitle(title, true);
        }

        public void SetTitle(CharacterTitle title)
        {
            AddTitle(title, true);
        }

        public uint EnumMapper_CharacterTitle_FileID = 0x22000041;

        public string GetTitle(CharacterTitle title)
        {
            var titleEnums = DatManager.PortalDat.ReadFromDat<EnumMapper>(EnumMapper_CharacterTitle_FileID);
            if (!titleEnums.IdToStringMap.TryGetValue((uint)title, out var titleEnum))
                return null;

            var hash = SpellTable.ComputeHash(titleEnum);

            var entry = DatManager.LanguageDat.CharacterTitles.StringTableData.FirstOrDefault(i => i.Id == hash);
            if (entry == null)
                return null;

            return entry.Strings.FirstOrDefault();
        }

        // =====================================
        // Barber
        // =====================================

        public void StartBarber()
        {
            BarberActive = true;
            Session.Network.EnqueueSend(new GameEventStartBarber(Session));
        }

        public void HandleActionFinishBarber(ClientMessage message)
        {
            if (!BarberActive) return;

            // Read the payload sent from the client...
            var requestedPaletteBaseId = message.Payload.ReadUInt32();
            var requestedHeadObjectDID = message.Payload.ReadUInt32();
            var requestedCharacterHairTexture = message.Payload.ReadUInt32();
            var requestedCharacterDefaultHairTexture = message.Payload.ReadUInt32();
            var requestedEyesTextureDID = message.Payload.ReadUInt32();
            var requestedDefaultEyesTextureDID = message.Payload.ReadUInt32();
            var requestedNoseTextureDID = message.Payload.ReadUInt32();
            var requestedDefaultNoseTextureDID = message.Payload.ReadUInt32();
            var requestedMouthTextureDID = message.Payload.ReadUInt32();
            var requestedDefaultMouthTextureDID = message.Payload.ReadUInt32();
            var requestedSkinPaletteDID = message.Payload.ReadUInt32();
            var requestedHairPaletteDID = message.Payload.ReadUInt32();
            var requestedEyesPaletteDID = message.Payload.ReadUInt32();
            var requestedSetupTableId = message.Payload.ReadUInt32();

            uint option_bound = message.Payload.ReadUInt32(); // Supress Levitation - Empyrean Only
            uint option_unk = message.Payload.ReadUInt32(); // Unknown - Possibly set aside for future use?

            //var debugMsg = "Barber Change Request:";
            //debugMsg += System.Environment.NewLine + $"PaletteBaseId:                0x{PaletteBaseId:X8} to 0x{requestedPaletteBaseId:X8}";
            //debugMsg += System.Environment.NewLine + $"HeadObjectDID:                0x{HeadObjectDID ?? 0:X8} to 0x{requestedHeadObjectDID:X8}";
            //debugMsg += System.Environment.NewLine + $"Character.HairTexture:        0x{Character.HairTexture:X8} to 0x{requestedCharacterHairTexture:X8}";
            //debugMsg += System.Environment.NewLine + $"Character.DefaultHairTexture: 0x{Character.DefaultHairTexture:X8} to 0x{requestedCharacterDefaultHairTexture:X8}";
            //debugMsg += System.Environment.NewLine + $"EyesTextureDID:               0x{EyesTextureDID:X8} to 0x{requestedEyesTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"DefaultEyesTextureDID:        0x{DefaultEyesTextureDID:X8} to 0x{requestedDefaultEyesTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"NoseTextureDID:               0x{NoseTextureDID:X8} to 0x{requestedNoseTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"DefaultNoseTextureDID:        0x{DefaultNoseTextureDID:X8} to 0x{requestedDefaultNoseTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"MouthTextureDID:              0x{MouthTextureDID:X8} to 0x{requestedMouthTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"DefaultMouthTextureDID:       0x{DefaultMouthTextureDID:X8} to 0x{requestedDefaultMouthTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"SkinPaletteDID:               0x{SkinPaletteDID:X8} to 0x{requestedSkinPaletteDID:X8}";
            //debugMsg += System.Environment.NewLine + $"HairPaletteDID:               0x{HairPaletteDID:X8} to 0x{requestedHairPaletteDID:X8}";
            //debugMsg += System.Environment.NewLine + $"EyesPaletteDID:               0x{EyesPaletteDID:X8} to 0x{requestedEyesPaletteDID:X8}";
            //debugMsg += System.Environment.NewLine + $"SetupTableId:                 0x{SetupTableId:X8} to 0x{requestedSetupTableId:X8}";
            //debugMsg += System.Environment.NewLine + $"Option Bound:                 {option_bound}";
            //debugMsg += System.Environment.NewLine + $"Option Unknown:               {option_unk}" + System.Environment.NewLine;
            //Console.WriteLine(debugMsg);

            var heritageGroup = DatManager.PortalDat.CharGen.HeritageGroups[(uint)Heritage];
            var sex = heritageGroup.Genders[(int)Gender];

            var validPaletteBase = sex.BasePalette == requestedPaletteBaseId;

            var validHeadObject = ValidateHairStyle(requestedHeadObjectDID, sex.HairStyleList, out var validatedHairStyle);

            var validEyesTexture = ValidateEyeTexture(requestedEyesTextureDID, sex.EyeStripList, isBald: validatedHairStyle?.Bald ?? false);

            var validDefaultEyesTexture = ValidateEyeTexture(requestedDefaultEyesTextureDID, sex.EyeStripList, compareOldTexture: true, isBald: validatedHairStyle?.Bald ?? false);

            var validNoseTexture = ValidateFaceTexture(requestedNoseTextureDID, sex.NoseStripList);

            var validDefaultNoseTexture = ValidateFaceTexture(requestedDefaultNoseTextureDID, sex.NoseStripList, compareOldTexture: true);

            var validMouthTexture = ValidateFaceTexture(requestedMouthTextureDID, sex.MouthStripList);

            var validDefaultMouthTexture = ValidateFaceTexture(requestedDefaultMouthTextureDID, sex.MouthStripList, compareOldTexture: true);

            var validCharacterHairTexture = ValidateHairTexture(requestedCharacterHairTexture, validatedHairStyle);

            var validCharacterDefaultHairTexture = ValidateHairTexture(requestedCharacterDefaultHairTexture, validatedHairStyle, compareOldTexture: true);

            var validSkinPalette = ValidateSkinPalette(requestedSkinPaletteDID, sex.SkinPalSet);

            var validEyesPalette = ValidateEyesPalette(requestedEyesPaletteDID, sex.EyeColorList);

            var validHairPalette = ValidateHairPalette(requestedHairPaletteDID, sex.HairColorList);

            var validSetupTable = ValidateSetupTable(requestedSetupTableId, heritageGroup, sex, validatedHairStyle);

            var validChangeRequested = validPaletteBase && validHeadObject
                && validEyesTexture && validDefaultEyesTexture && validEyesPalette
                && validNoseTexture && validDefaultNoseTexture
                && validMouthTexture && validDefaultMouthTexture
                && validCharacterHairTexture && validCharacterDefaultHairTexture && validHairPalette
                && validSkinPalette && validSetupTable;

            //var previousSetupTableId = SetupTableId;
            //var previousMotionTableId = MotionTableId;

            if (!validChangeRequested)
            {
                // Don't know what, if anything, to send to player, so silently failing for now.
                //SendTransientError("The barber cannot do what you requested.");
                BarberActive = false;
                return;
            }
            else
            {
                if (requestedPaletteBaseId > 0)
                    PaletteBaseId = requestedPaletteBaseId;

                if (requestedHeadObjectDID > 0)
                    HeadObjectDID = requestedHeadObjectDID;

                if (requestedCharacterHairTexture > 0)
                {
                    Character.HairTexture = requestedCharacterHairTexture;
                    CharacterChangesDetected = true;
                }
                if (requestedCharacterDefaultHairTexture > 0)
                {
                    Character.DefaultHairTexture = requestedCharacterDefaultHairTexture;
                    CharacterChangesDetected = true;
                }

                if (requestedEyesTextureDID > 0)
                    EyesTextureDID = requestedEyesTextureDID;
                if (requestedDefaultEyesTextureDID > 0)
                    DefaultEyesTextureDID = requestedDefaultEyesTextureDID;

                if (requestedNoseTextureDID > 0)
                    NoseTextureDID = requestedNoseTextureDID;
                if (requestedDefaultNoseTextureDID > 0)
                    DefaultNoseTextureDID = requestedDefaultNoseTextureDID;

                if (requestedMouthTextureDID > 0)
                    MouthTextureDID = requestedMouthTextureDID;
                if (requestedDefaultMouthTextureDID > 0)
                    DefaultMouthTextureDID = requestedDefaultMouthTextureDID;

                if (requestedSkinPaletteDID > 0)
                    SkinPaletteDID = requestedSkinPaletteDID;

                if (requestedHairPaletteDID > 0)
                    HairPaletteDID = requestedHairPaletteDID;

                if (requestedEyesPaletteDID > 0)
                    EyesPaletteDID = requestedEyesPaletteDID;

                if (requestedSetupTableId > 0)
                    SetupTableId = requestedSetupTableId;
            }

            // Check if Character is Empyrean, and if we need to set/change/send new motion table
            if (Heritage == (int)HeritageGroup.Empyrean)
            {
                // These are the motion tables for Empyrean float and not-float (one for each gender). They are hard-coded into the client.
                const uint EmpyreanMaleFloatMotionDID = 0x0900020Bu;
                const uint EmpyreanFemaleFloatMotionDID = 0x0900020Au;
                const uint EmpyreanMaleMotionDID = 0x0900020Eu;
                const uint EmpyreanFemaleMotionDID = 0x0900020Du;

                // Check for the Levitation option for Empyrean. Shadow crown and Undead flames are handled by client.
                if (Gender == (int)ACE.Entity.Enum.Gender.Male) // Male
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanMaleMotionDID)
                    {
                        MotionTableId = EmpyreanMaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanMaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanMaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                }
                else // Female
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanFemaleMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanFemaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                }
            }


            // Broadcast updated character appearance
            EnqueueBroadcast(new GameMessageObjDescEvent(this));

            // The following code provides for updated [no]flame/[no]crown/[no]hover setups to be seen by others immediately without need for the player to log out/in
            // however it creates movement desync which must be the result of using UpdateObject message.
            // We don't have (that I could find) pcaps of barber changes from observer perspective so I'm uncertain if the visual desync between setup changes was resolved by some other method
            //
            //if (previousSetupTableId != SetupTableId || previousMotionTableId != MotionTableId)
            //{
            //    EnqueueBroadcast(false, new GameMessageUpdateObject(this));
            //    Session.Network.EnqueueSend(new GameMessageObjDescEvent(this));
            //}
            //else
            //    EnqueueBroadcast(new GameMessageObjDescEvent(this));

            BarberActive = false;
        }

        private bool ValidateSetupTable(uint requestedSetupTableId, HeritageGroupCG heritageGroup, SexCG sex, HairStyleCG validHairStyle)
        {
            if (validHairStyle?.AlternateSetup > 0 && validHairStyle?.AlternateSetup == requestedSetupTableId)
                return true;
            else if (sex.SetupID == requestedSetupTableId)
                return true;
            //else if (heritageGroup.SetupID == requestedSetupTableId)
            //    return true;

            if (ValidHeritageSetups.TryGetValue((HeritageGroup)Heritage, out var genders) && genders.TryGetValue((Gender)Gender, out var gender) && gender.Contains(requestedSetupTableId))
                return true;

            return false;
        }

        /// <summary>
        /// Valid Setups defined in acclient that are not defined in the dat files.
        /// </summary>
        private static readonly Dictionary<HeritageGroup, Dictionary<Gender, List<uint>>> ValidHeritageSetups = new()
        {
            { HeritageGroup.Shadowbound, new () {
                { ACE.Entity.Enum.Gender.Male, new() {
                    (uint)SetupConst.UmbraenMaleCrown, (uint)SetupConst.UmbraenMaleNoCrown }
                },
                { ACE.Entity.Enum.Gender.Female, new() {
                    (uint)SetupConst.UmbraenFemaleCrown, (uint)SetupConst.UmbraenFemaleNoCrown } }
                }
            },
            { HeritageGroup.Penumbraen, new () {
                { ACE.Entity.Enum.Gender.Male, new() {
                    (uint)SetupConst.PenumbraenMaleCrown, (uint)SetupConst.PenumbraenMaleNoCrown }
                },
                { ACE.Entity.Enum.Gender.Female, new() {
                    (uint)SetupConst.PenumbraenFemaleCrown, (uint)SetupConst.PenumbraenFemaleNoCrown } }
                }
            },
            { HeritageGroup.Undead, new () {
                { ACE.Entity.Enum.Gender.Male, new() {
                    (uint)SetupConst.UndeadMaleSkeleton, (uint)SetupConst.UndeadMaleSkeletonNoFlame,
                    (uint)SetupConst.UndeadMaleZombie, (uint)SetupConst.UndeadMaleZombieNoFlame }
                },
                { ACE.Entity.Enum.Gender.Female, new() {
                    (uint)SetupConst.UndeadFemaleSkeleton, (uint)SetupConst.UndeadFemaleSkeletonNoFlame,
                    (uint)SetupConst.UndeadFemaleZombie, (uint)SetupConst.UndeadFemaleZombieNoFlame } }
                }
            }
        };

        private bool ValidateHairStyle(uint requestedHeadObjectDID, List<HairStyleCG> hairStyleList, out HairStyleCG validHairStyle)
        {
            validHairStyle = null;

            //var validHairStyles = hairStyleList.Where(h => h.ObjDesc.AnimPartChanges[0].PartID == requestedHeadObjectDID).ToList();

            if (requestedHeadObjectDID == 0)
            {
                if (Heritage == (int)HeritageGroup.Gearknight || Heritage == (int)HeritageGroup.Olthoi || Heritage == (int)HeritageGroup.OlthoiAcid)
                    return true;
            }

            foreach (var hairStyle in hairStyleList)
            {
                foreach (var animPartChange in hairStyle.ObjDesc.AnimPartChanges)
                {
                    if (animPartChange.PartID == requestedHeadObjectDID)
                    {
                        validHairStyle = hairStyle;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ValidateEyeTexture(uint requestedEyesTextureDID, List<EyeStripCG> eyeStripList, bool compareOldTexture = false, bool isBald = false)
        {
            foreach (var eyeStrip in eyeStripList)
            {
                if (isBald)
                {
                    foreach (var textureMapChange in eyeStrip.ObjDescBald.TextureChanges)
                    {
                        if (compareOldTexture && textureMapChange.OldTexture == requestedEyesTextureDID)
                            return true;
                        else if (!compareOldTexture && textureMapChange.NewTexture == requestedEyesTextureDID)
                            return true;
                    }
                }
                else
                {
                    foreach (var textureMapChange in eyeStrip.ObjDesc.TextureChanges)
                    {
                        if (compareOldTexture && textureMapChange.OldTexture == requestedEyesTextureDID)
                            return true;
                        else if (!compareOldTexture && textureMapChange.NewTexture == requestedEyesTextureDID)
                            return true;
                    }
                }
            }

            return false;
        }

        private bool ValidateFaceTexture(uint requestedFaceTextureDID, List<FaceStripCG> faceStripList, bool compareOldTexture = false)
        {
            foreach (var faceStrip in faceStripList)
            {
                foreach (var textureMapChange in faceStrip.ObjDesc.TextureChanges)
                {
                    if (compareOldTexture && textureMapChange.OldTexture == requestedFaceTextureDID)
                        return true;
                    else if (!compareOldTexture && textureMapChange.NewTexture == requestedFaceTextureDID)
                        return true;
                }
            }

            return false;
        }

        private bool ValidateHairTexture(uint requestedHairTextureDID, HairStyleCG hairStyle, bool compareOldTexture = false)
        {
            if (requestedHairTextureDID == 0)
            {
                if (Heritage == (int)HeritageGroup.Gearknight || Heritage == (int)HeritageGroup.Olthoi || Heritage == (int)HeritageGroup.OlthoiAcid)
                    return true;
            }
            else if (hairStyle == null)
                return false;

            foreach (var textureMapChange in hairStyle.ObjDesc.TextureChanges)
            {
                if (compareOldTexture && textureMapChange.OldTexture == requestedHairTextureDID)
                    return true;
                else if (!compareOldTexture && textureMapChange.NewTexture == requestedHairTextureDID)
                    return true;
            }

            return false;
        }

        private bool ValidateSkinPalette(uint requestedSkinPaletteDID, uint paletteSet)
        {
            var skinPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(paletteSet);
            if (skinPalSet.PaletteList.Contains(requestedSkinPaletteDID))
                return true;

            return false;
        }

        private bool ValidateEyesPalette(uint requestedEyesPaletteDID, List<uint> eyeColorList)
        {
            if (eyeColorList.Contains(requestedEyesPaletteDID))
                return true;

            return false;
        }

        private bool ValidateHairPalette(uint requestedHairPaletteDID, List<uint> hairColorList)
        {
            foreach (var hairPalette in hairColorList)
            {
                var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(hairPalette);
                if (hairPalSet.PaletteList.Contains(requestedHairPaletteDID))
                    return true;
            }

            return false;
        }
    }
}
