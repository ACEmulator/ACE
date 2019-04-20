namespace ACE.Entity.Enum
{
    /// <summary>
    /// These were tested against the last available client version: 0.0.11.6096<para/>
    /// The WeenieError identifies the specific message to be displayed in the chat window or top of the UI.<para/>
    /// Used with F7B0 028A: Game Event -> Display an error message in the chat window if necessary.<para/>
    /// <para/>
    /// WeenieError and WeenieErrorWithString are actually a single enum in the client.<para/>   
    /// The enum is used in handling 0x028A and 0x028B messages and also some other messages like UseDone.<para/>
    /// We split the enum up into 2 enums because each function uses only a specific set of the enum values.<para/>
    /// There are cases where the value was used by multiple messages e.g. 0x0036 ActionCancelled.
    ///
    /// Client error messages:
    /// http://ac.yotesfan.com/client/errors.php
    /// </summary>
    public enum WeenieError
    {
        /// <summary>
        /// No error (success)
        /// </summary>
        None = 0x0000,

        NoMem = 0x0001,
        BadParam = 0x0002,
        DivZero = 0x0003,
        SegV = 0x0004,
        Unimplemented = 0x0005,
        UnknownMessageType = 0x0006,
        NoAnimationTable = 0x0007,
        NoPhysicsObject = 0x0008,
        NoBookieObject = 0x0009,
        NoWslObject = 0x000A,
        NoMotionInterpreter = 0x000B,
        UnhandledSwitch = 0x000C,
        DefaultConstructorCalled = 0x000D,
        InvalidCombatManeuver = 0x000E,
        BadCast = 0x000F,
        MissingQuality = 0x0010,
        // Skip 1
        MissingDatabaseObject = 0x0012,
        NoCallbackSet = 0x0013,
        CorruptQuality = 0x0014,
        BadContext = 0x0015,
        NoEphseqManager = 0x0016,

        /// <summary>
        /// You failed to go to non-combat mode.
        /// </summary>
        BadMovementEvent = 0x0017,

        CannotCreateNewObject = 0x0018,
        NoControllerObject = 0x0019,
        CannotSendEvent = 0x001A,
        PhysicsCantTransition = 0x001B,
        PhysicsMaxDistanceExceeded = 0x001C,

        /// <summary>
        /// You're too busy!
        /// </summary>
        YoureTooBusy = 0x001D,

        CannotSendMessage = 0x001F,

        /// <summary>
        /// You must control both objects!
        /// </summary>
        IllegalInventoryTransaction = 0x0020,

        ExternalWeenieObject = 0x0021,
        InternalWeenieObject = 0x0022,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        MotionFailure = 0x0023,

        /// <summary>
        /// You can't jump while in the air
        /// </summary>
        YouCantJumpWhileInTheAir = 0x0024,

        InqCylSphereFailure = 0x0025,

        /// <summary>
        /// That is not a valid command.
        /// </summary>
        ThatIsNotAValidCommand = 0x0026,

        CarryingItem = 0x0027,

        /// <summary>
        /// The item is under someone else's control!
        /// </summary>
        Frozen = 0x0028,

        /// <summary>
        /// You cannot pick that up!
        /// </summary>
        Stuck = 0x0029,

        /// <summary>
        /// You are too encumbered to carry that!
        /// </summary>
        YouAreTooEncumbered = 0x002A,

        BadContain = 0x002C,
        BadParent = 0x002D,
        BadDrop = 0x002E,
        BadRelease = 0x002F,
        MsgBadMsg = 0x0030,
        MsgUnpackFailed = 0x0031,
        MsgNoMsg = 0x0032,
        MsgUnderflow = 0x0033,
        MsgOverflow = 0x0034,
        MsgCallbackFailed = 0x0035,

        /// <summary>
        /// Action cancelled!
        /// </summary>
        ActionCancelled = 0x0036,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        ObjectGone = 0x0037,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        NoObject = 0x0038,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        CantGetThere = 0x0039,

        /// <summary>
        /// You can't do that... you're dead! or You can't do that... its dead!
        /// </summary>
        Dead = 0x003A,

        ILeftTheWorld = 0x003B,
        ITeleported = 0x003C,

        /// <summary>
        /// You charged too far!
        /// </summary>
        YouChargedTooFar = 0x003D,

        /// <summary>
        /// You are too tired to do that!
        /// </summary>
        YouAreTooTiredToDoThat = 0x003E,

        // Client side only
        CantCrouchInCombat = 0x003F,
        // Client side only
        CantSitInCombat = 0x0040,
        // Client side only
        CantLieDownInCombat = 0x0041,
        // Client side only
        CantChatEmoteInCombat = 0x0042,

        NoMtableData = 0x0043,

        // Client side only
        CantChatEmoteNotStanding = 0x0044,

        TooManyActions = 0x0045,
        Hidden = 0x0046,
        GeneralMovementFailure = 0x0047,

        /// <summary>
        /// You can't jump from this position
        /// </summary>
        YouCantJumpFromThisPosition = 0x0048,

        // Client side only
        CantJumpLoadedDown = 0x0049,

        /// <summary>
        /// Ack! You killed yourself!
        /// </summary>
        YouKilledYourself = 0x004A,

        MsgResponseFailure = 0x004B,
        ObjectIsStatic = 0x004C,

        /// <summary>
        /// Invalid PK Status!
        /// </summary>
        InvalidPkStatus = 0x004D,

        InvalidXpAmount = 0x03E9,
        InvalidPpCalculation = 0x03EA,
        InvalidCpCalculation = 0x03EB,
        UnhandledStatAnswer = 0x03EC,
        HeartAttack = 0x03ED,

        /// <summary>
        /// The container is closed!
        /// </summary>
        TheContainerIsClosed = 0x03EE,

        InvalidInventoryLocation = 0x03F0,

        /// <summary>
        /// You failed to go to non-combat mode.
        /// </summary>
        ChangeCombatModeFailure = 0x03F1,


        FullInventoryLocation = 0x03F2,
        ConflictingInventoryLocation = 0x03F3,
        ItemNotPending = 0x03F4,
        BeWieldedFailure = 0x03F5,
        BeDroppedFailure = 0x03F6,

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
        YourAttackMisfired = 0x03F9,

        /// <summary>
        /// You've attempted an impossible spell path!
        /// </summary>
        YouveAttemptedAnImpossibleSpellPath = 0x03FA,

        MagicIncompleteAnimList = 0x03FB,
        MagicInvalidSpellType = 0x03FC,
        MagicInqPositionAndVelocityFailure = 0x03FD,

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

        MagicSpellbookAddSpellFailure = 0x0405,
        MagicTargetOutOfRange = 0x0406,

        /// <summary>
        /// Your spell cannot be cast outside
        /// </summary>
        YourSpellCannotBeCastOutside = 0x0407,

        /// <summary>
        /// Your spell cannot be cast inside
        /// </summary>
        YourSpellCannotBeCastInside = 0x0408,

        MagicGeneralFailure = 0x0409,

        /// <summary>
        /// You are unprepared to cast a spell
        /// </summary>
        YouAreUnpreparedToCastASpell = 0x040A,

        /// <summary>
        /// You've already sworn your Allegiance
        /// </summary>
        YouveAlreadySwornAllegiance = 0x040B,

        /// <summary>
        /// You don't have enough experience available to swear Allegiance
        /// </summary>
        CantSwearAllegianceInsufficientXp = 0x040C,

        AllegianceIgnoringRequests = 0x040D,
        AllegianceSquelched = 0x040E,
        AllegianceMaxDistanceExceeded = 0x040F,
        AllegianceIllegalLevel = 0x0410,
        AllegianceBadCreation = 0x0411,
        AllegiancePatronBusy = 0x0412,

        /// <summary>
        /// You are not in an allegiance!
        /// </summary>
        YouAreNotInAllegiance = 0x0414,

        AllegianceRemoveHierarchyFailure = 0x0415,

        FellowshipIgnoringRequests = 0x0417,
        FellowshipSquelched = 0x0418,
        FellowshipMaxDistanceExceeded = 0x0419,
        FellowshipMember = 0x041A,
        FellowshipIllegalLevel = 0x041B,
        FellowshipRecruitBusy = 0x041C,

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

        LevelTooLow = 0x0420,
        LevelTooHigh = 0x0421,

        /// <summary>
        /// That channel doesn't exist.
        /// </summary>
        ThatChannelDoesntExist = 0x0422,

        /// <summary>
        /// You can't use that channel.
        /// </summary>
        YouCantUseThatChannel = 0x0423,

        /// <summary>
        /// You're already on that channel.
        /// </summary>
        YouAreAlreadyOnThatChannel = 0x0424,

        /// <summary>
        /// You're not currently on that channel.
        /// </summary>
        YouAreNotOnThatChannel = 0x0425,

        /// <summary>
        /// _ can't be dropped
        /// </summary>
        AttunedItem = 0x0426,

        /// <summary>
        /// You cannot merge different stacks!
        /// </summary>
        YouCannotMergeDifferentStacks = 0x0427,

        /// <summary>
        /// You cannot merge enchanted items!
        /// </summary>
        YouCannotMergeEnchantedItems = 0x0428,

        /// <summary>
        /// You must control at least one stack!
        /// </summary>
        YouMustControlAtLeastOneStack = 0x0429,

        CurrentlyAttacking = 0x042A,
        MissileAttackNotOk = 0x042B,
        TargetNotAcquired = 0x042C,
        ImpossibleShot = 0x042D,
        BadWeaponSkill = 0x042E,
        UnwieldFailure = 0x042F,
        LaunchFailure = 0x0430,
        ReloadFailure = 0x0431,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        UnableToMakeCraftReq = 0x0432,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        CraftAnimationFailed = 0x0433,

        /// <summary>
        /// Given that number of items, you cannot craft anything.
        /// </summary>
        YouCantCraftWithThatNumberOfItems = 0x0434,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        CraftGeneralErrorUiMsg = 0x0435,

        CraftGeneralErrorNoUiMsg = 0x0436,

        /// <summary>
        /// Either you or one of the items involved does not pass the requirements for this craft interaction.
        /// </summary>
        YouDoNotPassCraftingRequirements = 0x0437,

        /// <summary>
        /// You do not have all the neccessary items.
        /// </summary>
        YouDoNotHaveAllTheNecessaryItems = 0x0438,

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
        /// Your hands must be free.
        /// </summary>
        YourHandsMustBeFree = 0x043C,

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

        QuestUnknown = 0x0440,
        QuestTableCorrupt = 0x0441,
        QuestBad = 0x0442,
        QuestDuplicate = 0x0443,
        QuestUnsolved = 0x0444,

        /// <summary>
        /// This item requires you to complete a specific quest before you can pick it up!
        /// </summary>
        ItemRequiresQuestToBePickedUp = 0x0445,

        QuestSolvedTooLongAgo = 0x0446,

        TradeIgnoringRequests = 0x044C,
        TradeSquelched = 0x044D,
        TradeMaxDistanceExceeded = 0x044E,
        TradeAlreadyTrading = 0x044F,
        TradeBusy = 0x0450,
        TradeClosed = 0x0451,
        TradeExpired = 0x0452,
        TradeItemBeingTraded = 0x0453,
        TradeNonEmptyContainer = 0x0454,
        TradeNonCombatMode = 0x0455,
        TradeIncomplete = 0x0456,
        TradeStampMismatch = 0x0457,
        TradeUnopened = 0x0458,
        TradeEmpty = 0x0459,
        TradeAlreadyAccepted = 0x045A,
        TradeOutOfSync = 0x045B,

        /// <summary>
        /// Player killers may not interact with that portal!
        /// </summary>
        PKsMayNotUsePortal = 0x045C,

        /// <summary>
        /// Non-player killers may not interact with that portal!
        /// </summary>
        NonPKsMayNotUsePortal = 0x045D,

        /// <summary>
        ///  You do not own a house!
        /// </summary>
        HouseAbandoned = 0x045E,

        /// <summary>
        ///  You do not own a house!
        /// </summary>
        HouseEvicted = 0x045F,

        HouseAlreadyOwned = 0x0460,
        HouseBuyFailed = 0x0461,
        HouseRentFailed = 0x0462,
        Hooked = 0x0463,

        MagicInvalidPosition = 0x0465,

        /// <summary>
        /// You must purchase Asheron's Call: Dark Majesty to interact with that portal.
        /// </summary>
        YouMustHaveDarkMajestyToUsePortal = 0x0466,

        InvalidAmmoType = 0x0467,
        SkillTooLow = 0x0468,

        /// <summary>
        /// You have used all the hooks you are allowed to use for this house.
        /// </summary>
        YouHaveUsedAllTheHooks = 0x0469,

        /// <summary>
        /// _ doesn't know what to do with that.
        /// </summary>
        TradeAiDoesntWant = 0x046A,

        HookHouseNotOwned = 0x046B,

        /// <summary>
        /// You must complete a quest to interact with that portal.
        /// </summary>
        YouMustCompleteQuestToUsePortal = 0x0474,

        HouseNoAllegiance = 0x047E,

        /// <summary>
        /// You must own a house to use this command.
        /// </summary>
        YouMustOwnHouseToUseCommand = 0x047F,

        /// <summary>
        /// Your monarch does not own a mansion or a villa!
        /// </summary>
        YourMonarchDoesNotOwnAMansionOrVilla = 0x0480,

        /// <summary>
        /// Your monarch does not own a mansion or a villa!
        /// </summary>
        YourMonarchsHouseIsNotAMansionOrVilla = 0x0481,

        /// <summary>
        /// Your monarch has closed the mansion to the Allegiance.
        /// </summary>
        YourMonarchHasClosedTheMansion = 0x0482,

        /// <summary>
        /// You must be a monarch to purchase this dwelling.
        /// </summary>
        YouMustBeMonarchToPurchaseDwelling = 0x048A,

        // Perhaps 30 seconds like fellowship timeout?
        AllegianceTimeout = 0x048D,

        /// <summary>
        /// Your offer of Allegiance has been ignored.
        /// </summary>
        YourOfferOfAllegianceWasIgnored = 0x048E,

        /// <summary>
        /// You are already involved in something!
        /// </summary>
        ConfirmationInProgress = 0x048F,

        /// <summary>
        /// You must be a monarch to use this command.
        /// </summary>
        YouMustBeAMonarchToUseCommand = 0x0490,

        /// <summary>
        /// You must specify a character to boot. (From allegiance)
        /// </summary>
        YouMustSpecifyCharacterToBoot = 0x0491,

        /// <summary>
        /// You can't boot yourself! (From allegiance)
        /// </summary>
        YouCantBootYourself = 0x0492,

        /// <summary>
        /// That character does not exist.
        /// </summary>
        ThatCharacterDoesNotExist = 0x0493,

        /// <summary>
        /// That person is not a member of your Allegiance!
        /// </summary>
        ThatPersonIsNotInYourAllegiance = 0x0494,

        /// <summary>
        /// No patron from which to break!
        /// </summary>
        CantBreakFromPatronNotInAllegiance = 0x0495,

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
        /// That is not a valid destination!
        /// </summary>
        TeleToInvalidPosition = 0x0499,

        /// <summary>
        /// You must purchase Asheron's Call -- Dark Majesty to use this function.
        /// </summary>
        MustHaveDarkMajestyToUse = 0x049A,

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

        PortalAisNotAllowed = 0x04A9,

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
        /// The lock is already unlocked.
        /// </summary>
        LockAlreadyUnlocked = 0x04AF,

        /// <summary>
        /// You can't lock or unlock that!
        /// </summary>
        YouCannotLockOrUnlockThat = 0x04B0,

        /// <summary>
        /// You can't lock or unlock what is open!
        /// </summary>
        YouCannotLockWhatIsOpen = 0x04B1,

        /// <summary>
        /// The key doesn't fit this lock.
        /// </summary>
        KeyDoesntFitThisLock = 0x04B2,

        /// <summary>
        /// The lock has been used too recently.
        /// </summary>
        LockUsedTooRecently = 0x04B3,

        /// <summary>
        /// You aren't trained in lockpicking!
        /// </summary>
        YouArentTrainedInLockpicking = 0x04B4,

        /// <summary>
        /// You must specify a character to query.
        /// </summary>
        AllegianceInfoEmptyName = 0x04B5,

        /// <summary>
        /// Please use the allegiance panel to view your own information.
        /// </summary>
        AllegianceInfoSelf = 0x04B6,

        /// <summary>
        /// You have used that command too recently.
        /// </summary>
        AllegianceInfoTooRecent = 0x04B7,

        AbuseNoSuchCharacter = 0x04B8,
        AbuseReportedSelf = 0x04B9,
        AbuseComplaintHandled = 0x04BA,

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

        TradeAiRefuseEmote = 0x04CD,

        /// <summary>
        /// You have failed to alter your skill.
        /// </summary>
        YouFailToAlterSkill = 0x04D0,

        FellowshipDeclined = 0x04DB,

        /// <summary>
        /// Appears to be sent to a recruiter 30 seconds after the initial recruit message when the recruitee doesn't respond.
        /// </summary>
        FellowshipTimeout = 0x04DC,

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
        /// You can't do that while in the air!
        /// </summary>
        YouCantDoThatWhileInTheAir = 0x04EB,

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
        /// You don't own that healing kit!
        /// </summary>
        YouDontOwnThatHealingKit = 0x04FD,

        /// <summary>
        /// You can't heal that!
        /// </summary>
        YouCantHealThat = 0x04FE,

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
        /// You can't do that -- you're trading!
        /// </summary>
        CantDoThatTradeInProgress = 0x0506,

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
        YouDoNotBelongToAFellowship = 0x050F,

        UsingMaxHooksSilent = 0x0511,

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
        /// The fellowship is locked, you were not added to the fellowship.
        /// </summary>
        LockedFellowshipCannotRecruitYou = 0x0519,

        /// <summary>
        /// Only the original owner may use that item's magic.
        /// </summary>
        ActivationNotAllowedNotOwner = 0x051A,

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
        /// You cannot posssibly succeed.
        /// </summary>
        YouCanPossiblySucceed = 0x0527,

        /// <summary>
        /// The fellowship is locked; you cannot open locked fellowships.
        /// </summary>
        FellowshipIsLocked = 0x0528,

        /// <summary>
        /// Trade Complete!
        /// </summary>
        TradeComplete = 0x0529,

        /// <summary>
        /// That is not a salvaging tool.
        /// </summary>
        NotASalvageTool = 0x052A,

        /// <summary>
        /// That person is not available now.
        /// </summary>
        CharacterNotAvailable = 0x052B,

        /// <summary>
        /// You must wait 30 days after purchasing a house before you may purchase another with any character on the same account. This applies to all housing except apartments.
        /// </summary>
        YouMustWaitToPurchaseHouse = 0x0532,

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
        /// Out of Range!
        /// </summary>
        MissileOutOfRange = 0x0550,

        /// <summary>
        /// You must purchase Asheron's Call -- Throne of Destiny to use this function.
        /// </summary>
        MustPurchaseThroneOfDestinyToUseFunction = 0x0552,

        /// <summary>
        /// You must purchase Asheron's Call -- Throne of Destiny to use this item.
        /// </summary>
        MustPurchaseThroneOfDestinyToUseItem = 0x0553,

        /// <summary>
        /// You must purchase Asheron's Call -- Throne of Destiny to use this portal.
        /// </summary>
        MustPurchaseThroneOfDestinyToUsePortal = 0x0554,

        /// <summary>
        /// You must purchase Asheron's Call -- Throne of Destiny to access this quest.
        /// </summary>
        MustPurchaseThroneOfDestinyToAccessQuest = 0x0555,

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
        /// You may only have a maximum of 50 friends at once. If you wish to add more friends, you must first remove some.
        /// </summary>
        MaxFriendsExceeded = 0x0561,

        /// <summary>
        /// That character is not on your friends list!
        /// </summary>
        ThatCharacterNotOnYourFriendsList = 0x0563,

        /// <summary>
        /// Only the character who owns the house may use this command.
        /// </summary>
        OnlyHouseOwnerCanUseCommand = 0x0564,

        /// <summary>
        /// That allegiance name is invalid because it is empty. Please use the @allegiance name clear command to clear your allegiance name.
        /// </summary>
        InvalidAllegianceNameCantBeEmpty = 0x0565,

        /// <summary>
        /// That allegiance name is too long. Please choose another name.
        /// </summary>
        InvalidAllegianceNameTooLong = 0x0566,

        /// <summary>
        /// That allegiance name contains illegal characters. Please choose another name using only letters, spaces, - and '.
        /// </summary>
        InvalidAllegianceNameBadCharacters = 0x0567,

        /// <summary>
        /// That allegiance name is not appropriate. Please choose another name.
        /// </summary>
        InvalidAllegianceNameInappropriate = 0x0568,

        /// <summary>
        /// That allegiance name is already in use. Please choose another name.
        /// </summary>
        InvalidAllegianceNameAlreadyInUse = 0x0569,

        /// <summary>
        /// Your allegiance name has been cleared.
        /// </summary>
        AllegianceNameCleared = 0x056B,

        /// <summary>
        /// That is already the name of your allegiance!
        /// </summary>
        InvalidAllegianceNameSameName = 0x056C,

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
        /// You cannot pick up more of that item!
        /// </summary>
        TooManyUniqueItems = 0x0584,

        /// <summary>
        /// You are restricted to clothes and armor created for your race.
        /// </summary>
        HeritageRequiresSpecificArmor = 0x0585,

        /// <summary>
        /// That item was specifically created for another race.
        /// </summary>
        ArmorRequiresSpecificHeritage = 0x0586,

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

        ContractError = 0x0594
    }
}
