namespace ACE.Entity.Enum
{
    /// <summary>
    /// The StatusMessageType1 identifies the specific message to be displayed in the chat window.<para />
    /// Used with F7B0 028A: Game Event -> Display a status message in the chat window.<para />
    /// <para />
    /// StatusMessageType1 and StatusMessageType2 are actually a single enum in the clients.<para />   
    /// The combined enum is only used by 2 functions in the client.<para />
    /// We split the enum up into 2 enums because each function uses only a specific set of the enum values.<para />
    /// No one value is used by both functions.
    /// </summary>
    public enum StatusMessageType1
    {
        /// <summary>
        /// You're too busy!
        /// </summary>
        YoureTooBusy = 0x001D,

        /// <summary>
        /// You can't jump while in the air
        /// </summary>
        YouCantJumpWhileInTheAir = 0x0024,

        /// <summary>
        /// That is not a valid command.
        /// </summary>
        ThatIsNotAValidCommand = 0x0026,

        /// <summary>
        /// You are too encumbered to carry that!
        /// </summary>
        YouAreTooEncumbered = 0x002A,

        /// <summary>
        /// Action cancelled!
        /// </summary>
        ActionCancelled = 0x0036,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        UnableToMoveToObject_37 = 0x0037,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        UnableToMoveToObject_39 = 0x0039,

        /// <summary>
        /// You can't do that... you're dead!
        /// </summary>
        YouCantDoThatYoureDead = 0x003A,

        Enum_003C = 0x003C,

        /// <summary>
        /// You charged too far!
        /// </summary>
        YouChargedTooFar = 0x003D,

        /// <summary>
        /// You are too tired to do that!
        /// </summary>
        YouAreTooTiredToDoThat = 0x003E,

        /// <summary>
        /// You can't jump from this position
        /// </summary>
        YouCantJumpFromThisPosition = 0x0048,

        /// <summary>
        /// Ack! You killed yourself!
        /// </summary>
        YouKilledYourself = 0x004A,

        Enum_004B = 0x004B,

        /// <summary>
        /// You are too fatigued to attack!
        /// </summary>
        YouAreTooFatiguedToAttack = 0x03F7,

        /// <summary>
        /// You are out of ammunition!
        /// </summary>
        YouAreOutOfAmmunition = 0x03F8,

        /// <summary>
        /// Your missile attack misfired!
        /// </summary>
        YourMissileAttackMisfired = 0x03F9,

        /// <summary>
        /// You've attempted an impossible spell path!
        /// </summary>
        YouveAttemptedAnImpossibleSpellPath = 0x03FA,

        /// <summary>
        /// You don't know that spell!
        /// </summary>
        YouDontKnowThatSpell = 0x03FE,

        /// <summary>
        /// Incorrect target type
        /// </summary>
        IncorrectTargetType = 0x03FF,

        /// <summary>
        /// You don't have all the components for this spell.
        /// </summary>
        YouDontHaveAllTheComponents = 0x0400,

        /// <summary>
        /// You don't have enough Mana to cast this spell.
        /// </summary>
        YouDontHaveEnoughManaToCast = 0x0401,

        /// <summary>
        /// Your spell fizzled.
        /// </summary>
        YourSpellFizzled = 0x0402,

        /// <summary>
        /// Your spell's target is missing!
        /// </summary>
        YourSpellTargetIsMissing = 0x0403,

        /// <summary>
        /// Your projectile spell mislaunched!
        /// </summary>
        YourProjectileSpellMislaunched = 0x0404,

        /// <summary>
        /// Your spell cannot be cast outside
        /// </summary>
        YourSpellCannotBeCastOutside = 0x0407,

        /// <summary>
        /// Your spell cannot be cast inside
        /// </summary>
        YourSpellCannotBeCastInside = 0x0408,

        Enum_0409 = 0x0409,

        /// <summary>
        /// You are unprepared to cast a spell
        /// </summary>
        YouAreUnpreparedToCastSpell = 0x040A,

        /// <summary>
        /// You are not in an allegiance!
        /// </summary>
        YouAreNotInAllegiance = 0x0414,

        /// <summary>
        /// You must be the leader of a Fellowship
        /// </summary>
        YouMustBeLeaderOfFellowship = 0x041D,

        /// <summary>
        /// Your Fellowship is full
        /// </summary>
        YourFellowshipIsFull = 0x041E,

        /// <summary>
        /// That Fellowship name is not permitted
        /// </summary>
        FellowshipNameIsNotPermitted = 0x041F,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        YourCraftAttemptFails_433 = 0x0433,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        YourCraftAttemptFails_435 = 0x0435,

        Enum_0436 = 0x0436,

        /// <summary>
        /// Either you or one of the items involved does not pass the requirements for this craft interaction.
        /// </summary>
        YouDoNotPassCraftingRequirements = 0x0437,

        /// <summary>
        /// You do not have all the neccessary items.
        /// </summary>
        YouDoNotHaveAllNecessaryItems = 0x0438,

        /// <summary>
        /// Not all the items are avaliable.
        /// </summary>
        NotAllTheItemsAreAvailable = 0x0439,

        /// <summary>
        /// You must be at rest in peace mode to do trade skills.
        /// </summary>
        YouMustBeInPeaceModeToTrade = 0x043A,

        /// <summary>
        /// You are not trained in that trade skill.
        /// </summary>
        YouAreNotTrainedInThatTradeSkill = 0x043B,

        /// <summary>
        /// You cannot link to that portal!
        /// </summary>
        YouCannotLinkToThatPortal = 0x043D,

        /// <summary>
        /// You have solved this quest too recently!
        /// </summary>
        YouHaveSolvedThisQuestTooRecently = 0x043E,

        /// <summary>
        /// You have solved this quest too many times!
        /// </summary>
        YouHaveSolvedThisQuestTooManyTimes = 0x043F,

        /// <summary>
        /// This item requires you to complete a specific quest before you can pick it up!
        /// </summary>
        ItemRequiresQuestToBePickedUp = 0x0445,

        Enum_0446 = 0x0446,

        /// <summary>
        /// Player killers may not interact with that portal!
        /// </summary>
        PKsMayNotUsePortal = 0x045C,

        /// <summary>
        /// Non-player killers may not interact with that portal!
        /// </summary>
        NonPKsMayNotUsePortal = 0x045D,

        Enum_0465 = 0x0465,

        /// <summary>
        /// You must purchase Asheron's Call: Dark Majesty to interact with that portal.
        /// </summary>
        YouMustHaveDarkMajestyToUsePortal = 0x0466,

        /// <summary>
        /// You have used all the hooks you are allowed to use for this house.
        /// </summary>
        YouHaveUsedAllTheHooks = 0x0469,

        /// <summary>
        /// You must complete a quest to interact with that portal.
        /// </summary>
        YouMustCompleteQuestToUsePortal = 0x0474,

        /// <summary>
        /// You must own a house to use this command.
        /// </summary>
        YouMustOwnHouseToUseCommand = 0x047F,

        /// <summary>
        /// You can't lock or unlock that!
        /// </summary>
        YouCannotLockOrUnlockThat = 0x0480,

        /// <summary>
        /// You can't lock or unlock what is open!
        /// </summary>
        YouCannotLockWhatIsOpen = 0x0481,

        /// <summary>
        /// You must be a monarch to purchase this dwelling.
        /// </summary>
        YouMustBeMonarchToPurchaseDwelling = 0x048A,

        /// <summary>
        /// Your Allegiance has been dissolved!
        /// </summary>
        YourAllegianceHasBeenDissolved = 0x0496,

        /// <summary>
        /// Your patron's Allegiance to you has been broken!
        /// </summary>
        YourPatronsAllegianceHasBeenBroken = 0x0497,

        /// <summary>
        /// You have moved too far!
        /// </summary>
        YouHaveMovedTooFar = 0x0498,

        /// <summary>
        /// You fail to link with the lifestone!
        /// </summary>
        YouFailToLinkWithLifestone = 0x049B,

        /// <summary>
        /// You wandered too far to link with the lifestone!
        /// </summary>
        YouWanderedTooFarToLinkWithLifestone = 0x049C,

        /// <summary>
        /// You successfully link with the lifestone!
        /// </summary>
        YouSuccessfullyLinkWithLifestone = 0x049D,

        /// <summary>
        /// You must have linked with a lifestone in order to recall to it!
        /// </summary>
        YouMustLinkToLifestoneToRecall = 0x049E,

        /// <summary>
        /// You fail to recall to the lifestone!
        /// </summary>
        YouFailToRecallToLifestone = 0x049F,

        /// <summary>
        /// You fail to link with the portal!
        /// </summary>
        YouFailToLinkWithPortal = 0x04A0,

        /// <summary>
        /// You successfully link with the portal!
        /// </summary>
        YouSuccessfullyLinkWithPortal = 0x04A1,

        /// <summary>
        /// You fail to recall to the portal!
        /// </summary>
        YouFailToRecallToPortal = 0x04A2,

        /// <summary>
        /// You must have linked with a portal in order to recall to it!
        /// </summary>
        YouMustLinkToPortalToRecall = 0x04A3,

        /// <summary>
        /// You fail to summon the portal!
        /// </summary>
        YouFailToSummonPortal = 0x04A4,

        /// <summary>
        /// You must have linked with a portal in order to summon it!
        /// </summary>
        YouMustLinkToPortalToSummonIt = 0x04A5,

        /// <summary>
        /// You fail to teleport!
        /// </summary>
        YouFailToTeleport = 0x04A6,

        /// <summary>
        /// You have been teleported too recently!
        /// </summary>
        YouHaveBeenTeleportedTooRecently = 0x04A7,

        /// <summary>
        /// You must be an Advocate to interact with that portal.
        /// </summary>
        YouMustBeAnAdvocateToUsePortal = 0x04A8,

        /// <summary>
        /// Players may not interact with that portal.
        /// </summary>
        PlayersMayNotUsePortal = 0x04AA,

        /// <summary>
        /// You are not powerful enough to interact with that portal!
        /// </summary>
        YouAreNotPowerfulEnoughToUsePortal = 0x04AB,

        /// <summary>
        /// You are too powerful to interact with that portal!
        /// </summary>
        YouAreTooPowerfulToUsePortal = 0x04AC,

        /// <summary>
        /// You cannot recall to that portal!
        /// </summary>
        YouCannotRecallPortal = 0x04AD,

        /// <summary>
        /// You cannot summon that portal!
        /// </summary>
        YouCannotSummonPortal = 0x04AE,

        /// <summary>
        /// The key doesn't fit this lock.
        /// </summary>
        KeyDoesntFitThisLock = 0x04B2,

        /// <summary>
        /// You do not own that salvage tool!
        /// </summary>
        YouDoNotOwnThatSalvageTool = 0x04BD,

        /// <summary>
        /// You do not own that item!
        /// </summary>
        YouDoNotOwnThatItem = 0x04BE,

        /// <summary>
        /// The material cannot be created.
        /// </summary>
        MaterialCannotBeCreated = 0x04C1,

        /// <summary>
        /// The list of items you are attempting to salvage is invalid.
        /// </summary>
        ItemsAttemptingToSalvageIsInvalid = 0x04C2,

        /// <summary>
        /// You cannot salvage items that you are trading!
        /// </summary>
        YouCannotSalvageItemsInTrading = 0x04C3,

        /// <summary>
        /// You must be a guest in this house to interact with that portal.
        /// </summary>
        YouMustBeHouseGuestToUsePortal = 0x04C4,

        /// <summary>
        /// Your Allegiance Rank is too low to use that item's magic.
        /// </summary>
        YourAllegianceRankIsTooLowToUseMagic = 0x04C5,

        /// <summary>
        /// Your Arcane Lore skill is too low to use that item's magic.
        /// </summary>
        YourArcaneLoreIsTooLowToUseMagic = 0x04C7,

        /// <summary>
        /// That item doesn't have enough Mana.
        /// </summary>
        ItemDoesntHaveEnoughMana = 0x04C8,

        /// <summary>
        /// You have been involved in a player killer battle too recently to do that!
        /// </summary>
        YouHaveBeenInPKBattleTooRecently = 0x04CC,

        Enum_04DC = 0x04DC,

        /// <summary>
        /// You have failed to alter your attributes.
        /// </summary>
        YouHaveFailedToAlterAttributes = 0x04DD,

        /// <summary>
        /// You are currently wielding items which require a certain level of skill. Your attributes cannot be transferred while you are wielding these items. Please remove these items and try again.
        /// </summary>
        CannotTransferAttributesWhileWieldingItem = 0x04E0,

        /// <summary>
        /// You have succeeded in transferring your attributes!
        /// </summary>
        YouHaveSucceededTransferringAttributes = 0x04E1,

        /// <summary>
        /// This hook is a duplicated housing object. You may not add items to a duplicated housing object. Please empty the hook and allow it to reset.
        /// </summary>
        HookIsDuplicated = 0x04E2,

        /// <summary>
        /// That item is of the wrong type to be placed on this hook.
        /// </summary>
        ItemIsWrongTypeForHook = 0x04E3,

        /// <summary>
        /// This chest is a duplicated housing object. You may not add items to a duplicated housing object. Please empty everything -- including backpacks -- out of the chest and allow the chest to reset.
        /// </summary>
        HousingChestIsDuplicated = 0x04E4,

        /// <summary>
        /// This hook was a duplicated housing object. Since it is now empty, it will be deleted momentarily. Once it is gone, it is safe to use the other, non-duplicated hook that is here.
        /// </summary>
        HookWillBeDeleted = 0x04E5,

        /// <summary>
        /// This chest was a duplicated housing object. Since it is now empty, it will be deleted momentarily. Once it is gone, it is safe to use the other, non-duplicated chest that is here.
        /// </summary>
        HousingChestWillBeDeleted = 0x04E6,

        /// <summary>
        /// You cannot swear allegiance to anyone because you own a monarch-only house. Please abandon your house and try again.
        /// </summary>
        CannotSwearAllegianceWhileOwningMansion = 0x04E7,

        /// <summary>
        /// You cannot modify your player killer status while you are recovering from a PK death.
        /// </summary>
        CannotChangePKStatusWhileRecovering = 0x04EC,

        /// <summary>
        /// Advocates may not change their player killer status!
        /// </summary>
        AdvocatesCannotChangePKStatus = 0x04ED,

        /// <summary>
        /// Your level is too low to change your player killer status with this object.
        /// </summary>
        LevelTooLowToChangePKStatusWithObject = 0x04EE,

        /// <summary>
        /// Your level is too high to change your player killer status with this object.
        /// </summary>
        LevelTooHighToChangePKStatusWithObject = 0x04EF,

        /// <summary>
        /// You feel a harsh dissonance, and you sense that an act of killing you have committed recently is interfering with the conversion.
        /// </summary>
        YouFeelAHarshDissonance = 0x04F0,

        /// <summary>
        /// Bael'Zharon's power flows through you again. You are once more a player killer.
        /// </summary>
        YouArePKAgain = 0x04F1,

        /// <summary>
        /// Bael'Zharon has granted you respite after your moment of weakness. You are temporarily no longer a player killer.
        /// </summary>
        YouAreTemporarilyNoLongerPK = 0x04F2,

        /// <summary>
        /// Lite Player Killers may not interact with that portal!
        /// </summary>
        PKLiteMayNotUsePortal = 0x04F3,

        /// <summary>
        /// You aren't trained in healing!
        /// </summary>
        YouArentTrainedInHealing = 0x04FC,

        /// <summary>
        /// You aren't ready to heal!
        /// </summary>
        YouArentReadyToHeal = 0x0500,

        /// <summary>
        /// You can only use Healing Kits on player characters.
        /// </summary>
        YouCanOnlyHealPlayers = 0x0501,

        /// <summary>
        /// The Lifestone's magic protects you from the attack!
        /// </summary>
        LifestoneMagicProtectsYou = 0x0502,

        /// <summary>
        /// The portal's residual energy protects you from the attack!
        /// </summary>
        PortalEnergyProtectsYou = 0x0503,

        /// <summary>
        /// You are enveloped in a feeling of warmth as you are brought back into the protection of the Light. You are once again a Non-Player Killer.
        /// </summary>
        YouAreNonPKAgain = 0x0504,

        /// <summary>
        /// You're too close to your sanctuary!
        /// </summary>
        YoureTooCloseToYourSanctuary = 0x0505,

        /// <summary>
        /// Only Non-Player Killers may enter PK Lite. Please see @help pklite for more details about this command.
        /// </summary>
        OnlyNonPKsMayEnterPKLite = 0x0507,

        /// <summary>
        /// A cold wind touches your heart. You are now a Player Killer Lite.
        /// </summary>
        YouAreNowPKLite = 0x0508,

        /// <summary>
        /// You do not belong to a Fellowship.
        /// </summary>
        YouDoNotBelongToFellowship = 0x050F,

        /// <summary>
        /// You are now using the maximum number of hooks.  You cannot use another hook until you take an item off one of your hooks.
        /// </summary>
        YouAreNowUsingMaxHooks = 0x0512,

        /// <summary>
        /// You are no longer using the maximum number of hooks.  You may again add items to your hooks.
        /// </summary>
        YouAreNoLongerUsingMaxHooks = 0x0513,

        /// <summary>
        /// You are not permitted to use that hook.
        /// </summary>
        YouAreNotPermittedToUseThatHook = 0x0516,

        /// <summary>
        /// You have entered your allegiance chat room.
        /// </summary>
        YouHaveEnteredYourAllegianceChat = 0x051B,

        /// <summary>
        /// You have left an allegiance chat room.
        /// </summary>
        YouHaveLeftAnAllegianceChat = 0x051C,

        /// <summary>
        /// Turbine Chat is enabled.
        /// </summary>
        TurbineChatIsEnabled = 0x051D,

        /// <summary>
        /// You cannot add anymore people to the list of players that you can hear.
        /// </summary>
        YouCannotAddPeopleToHearList = 0x0520,

        /// <summary>
        /// You are now deaf to player's screams.
        /// </summary>
        YouAreNowDeafTo_Screams = 0x0523,

        /// <summary>
        /// You can hear all players once again.
        /// </summary>
        YouCanHearAllPlayersOnceAgain = 0x0524,

        /// <summary>
        /// You chicken out.
        /// </summary>
        YouChickenOut = 0x0526,

        /// <summary>
        /// The fellowship is locked; you cannot open locked fellowships.
        /// </summary>
        FellowshipIsLocked = 0x0528,

        /// <summary>
        /// Trade Complete!
        /// </summary>
        TradeComplete = 0x0529,

        /// <summary>
        /// Character not available.
        /// </summary>
        CharacterNotAvailable = 0x052B,

        /// <summary>
        /// You must wait 30 days after purchasing a house before you may purchase another with any character on the same account. This applies to all housing except apartments.
        /// </summary>
        YouMustWaitToPurchaseHouse = 0x0532,

        /// <summary>
        /// You have been booted from your allegiance chat room. Use "@allegiance chat on" to rejoin. ().
        /// </summary>
        YouHaveBeenBootedFromAllegianceChat = 0x0533,

        /// <summary>
        /// You do not have the authority within your allegiance to do that.
        /// </summary>
        YouDoNotHaveAuthorityInAllegiance = 0x0535,

        /// <summary>
        /// You have the maximum number of accounts banned.!
        /// </summary>
        YouHaveMaxAccountsBanned = 0x0540,

        /// <summary>
        /// You already have the maximum number of allegiance officers. You must remove some before you add any more.
        /// </summary>
        YouHaveMaxAllegianceOfficers = 0x0545,

        /// <summary>
        /// Your allegiance officers have been cleared.
        /// </summary>
        YourAllegianceOfficersHaveBeenCleared = 0x0546,

        /// <summary>
        /// You cannot join any chat channels while gagged.
        /// </summary>
        YouCannotJoinChannelsWhileGagged = 0x0548,

        /// <summary>
        /// You are no longer an allegiance officer.
        /// </summary>
        YouAreNoLongerAllegianceOfficer = 0x054A,

        /// <summary>
        /// Your allegiance does not have a hometown.
        /// </summary>
        YourAllegianceDoesNotHaveHometown = 0x054C,

        /// <summary>
        /// The hook does not contain a usable item. You cannot open the hook because you do not own the house to which it belongs.
        /// </summary>
        HookItemNotUsable_CannotOpen = 0x054E,

        /// <summary>
        /// The hook does not contain a usable item. Use the '@house hooks on'command to make the hook openable.
        /// </summary>
        HookItemNotUsable_CanOpen = 0x054F,

        /// <summary>
        /// You have failed to complete the augmentation.
        /// </summary>
        YouFailedToCompleteAugmentation = 0x0556,

        /// <summary>
        /// You have used this augmentation too many times already.
        /// </summary>
        AugmentationUsedTooManyTimes = 0x0557,

        /// <summary>
        /// You have used augmentations of this type too many times already.
        /// </summary>
        AugmentationTypeUsedTooManyTimes = 0x0558,

        /// <summary>
        /// You do not have enough unspent experience available to purchase this augmentation.
        /// </summary>
        AugmentationNotEnoughExperience = 0x0559,

        /// <summary>
        /// You must exit the Training Academy before that command will be available to you.
        /// </summary>
        ExitTrainingAcademyToUseCommand = 0x055D,

        /// <summary>
        /// Only Player Killer characters may use this command!
        /// </summary>
        OnlyPKsMayUseCommand = 0x055F,

        /// <summary>
        /// Only Player Killer Lite characters may use this command!
        /// </summary>
        OnlyPKLiteMayUseCommand = 0x0560,

        /// <summary>
        /// That is an invalid officer level.
        /// </summary>
        InvalidOfficerLevel = 0x056F,

        /// <summary>
        /// That allegiance officer title is not appropriate.
        /// </summary>
        AllegianceOfficerTitleIsNotAppropriate = 0x0570,

        /// <summary>
        /// That allegiance name is too long. Please choose another name.
        /// </summary>
        AllegianceNameIsTooLong = 0x0571,

        /// <summary>
        /// All of your allegiance officer titles have been cleared.
        /// </summary>
        AllegianceOfficerTitlesCleared = 0x0572,

        /// <summary>
        /// That allegiance title contains illegal characters. Please choose another name using only letters, spaces, - and '.
        /// </summary>
        AllegianceTitleHasIllegalChars = 0x0573,

        /// <summary>
        /// You have not pre-approved any vassals to join your allegiance.
        /// </summary>
        YouHaveNotPreApprovedVassals = 0x0579,

        /// <summary>
        /// You have cleared the pre-approved vassal for your allegiance.
        /// </summary>
        YouHaveClearedPreApprovedVassal = 0x057C,

        /// <summary>
        /// That character is already gagged!
        /// </summary>
        CharIsAlreadyGagged = 0x057D,

        /// <summary>
        /// That character is not currently gagged!
        /// </summary>
        CharIsNotCurrentlyGagged = 0x057E,

        /// <summary>
        /// Your allegiance chat privileges have been restored.
        /// </summary>
        YourAllegianceChatPrivilegesRestored = 0x0581,

        /// <summary>
        /// Olthoi cannot interact with that!
        /// </summary>
        OlthoiCannotInteractWithThat = 0x0587,

        /// <summary>
        /// Olthoi cannot use regular lifestones! Asheron would not allow it!
        /// </summary>
        OlthoiCannotUseLifestones = 0x0588,

        /// <summary>
        /// The vendor looks at you in horror!
        /// </summary>
        OlthoiVendorLooksInHorror = 0x0589,

        /// <summary>
        /// As a mindless engine of destruction an Olthoi cannot join a fellowship!
        /// </summary>
        OlthoiCannotJoinFellowship = 0x058B,

        /// <summary>
        /// The Olthoi only have an allegiance to the Olthoi Queen!
        /// </summary>
        OlthoiCannotJoinAllegiance = 0x058C,

        /// <summary>
        /// You cannot use that item!
        /// </summary>
        YouCannotUseThatItem = 0x058D,

        /// <summary>
        /// This person will not interact with you!
        /// </summary>
        ThisPersonWillNotInteractWithYou = 0x058E,

        /// <summary>
        /// Only Olthoi may pass through this portal!
        /// </summary>
        OnlyOlthoiMayUsePortal = 0x058F,

        /// <summary>
        /// Olthoi may not pass through this portal!
        /// </summary>
        OlthoiMayNotUsePortal = 0x0590,

        /// <summary>
        /// You may not pass through this portal while Vitae weakens you!
        /// </summary>
        YouMayNotUsePortalWithVitae = 0x0591,

        /// <summary>
        /// This character must be two weeks old or have been created on an account at least two weeks old to use this portal!
        /// </summary>
        YouMustBeTwoWeeksOldToUsePortal = 0x0592,

        /// <summary>
        /// Olthoi characters can only use Lifestone and PK Arena recalls!
        /// </summary>
        OlthoiCanOnlyRecallToLifestone = 0x0593,
    }
}
