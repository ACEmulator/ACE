namespace ACE.Entity.Enum
{
    /// <summary>
    /// These were tested against the last available client version: 0.0.11.6096<para/>
    /// The WeenieErrorWithString identifies the specific message to be displayed in the chat window along
    /// with a custom string.<para />
    /// Used with F7B0 028B: Game Event -> Display an error message in the chat window.<para />
    /// <para />
    /// WeenieError and WeenieErrorWithString are actually a single enum in the client.<para/>   
    /// The enum is used in handling 0x028A and 0x028B messages and also some other messages like UseDone.<para/>
    /// We split the enum up into 2 enums because each function uses only a specific set of the enum values.<para/>
    /// There are cases where the value was used by multiple messages e.g. 0x0036 ActionCancelled.
    /// </summary>
    public enum WeenieErrorWithString
    {
        /// <summary>
        ///  is too busy to accept gifts right now.
        /// </summary>
        _IsTooBusyToAcceptGifts = 0x001E,

        /// <summary>
        ///  cannot carry anymore.
        /// </summary>
        _CannotCarryAnymore = 0x002B,

        /// <summary>
        /// You fail to affect _ because you cannot affect anyone!
        /// </summary>
        YouFailToAffect_YouCannotAffectAnyone = 0x004E,

        /// <summary>
        /// You fail to affect  because $s cannot be harmed!
        /// </summary>
        YouFailToAffect_TheyCannotBeHarmed = 0x004F,

        /// <summary>
        /// You fail to affect  because beneficial spells do not affect !
        /// </summary>
        YouFailToAffect_WithBeneficialSpells = 0x0050,

        /// <summary>
        /// You fail to affect  because you are not a player killer!
        /// </summary>
        YouFailToAffect_YouAreNotPK = 0x0051,

        /// <summary>
        /// You fail to affect  because  is not a player killer!
        /// </summary>
        YouFailToAffect_TheyAreNotPK = 0x0052,

        /// <summary>
        /// You fail to affect  because you are not the same sort of player killer as !
        /// </summary>
        YouFailToAffect_NotSamePKType = 0x0053,

        /// <summary>
        /// You fail to affect  because you are acting across a house boundary!
        /// </summary>
        YouFailToAffect_AcrossHouseBoundary = 0x0054,

        /// <summary>
        ///  is not accepting gifts right now.
        /// </summary>
        _IsNotAcceptingGiftsRightNow = 0x03EF,

        /// <summary>
        ///  is already one of your followers
        /// </summary>
        _IsAlreadyOneOfYourFollowers = 0x0413,

        /// <summary>
        ///  cannot have any more Vassals
        /// </summary>
        _CannotHaveAnyMoreVassals = 0x0416,

        /// <summary>
        ///  doesn't know what to do with that.
        /// </summary>
        _DoesntKnowWhatToDoWithThat = 0x046A,

        /// <summary>
        /// You must be above level  to purchase this dwelling.
        /// </summary>
        YouMustBeAboveLevel_ToBuyHouse = 0x0488,

        /// <summary>
        /// You must be at or below level  to purchase this dwelling.
        /// </summary>
        YouMustBeAtOrBelowLevel_ToBuyHouse = 0x0489,

        /// <summary>
        /// You must be above allegiance rank  to purchase this dwelling.
        /// </summary>
        YouMustBeAboveAllegianceRank_ToBuyHouse = 0x048B,

        /// <summary>
        /// You must be at or below allegiance rank  to purchase this dwelling.
        /// </summary>
        YouMustBeAtOrBelowAllegianceRank_ToBuyHouse = 0x048C,

        /// <summary>
        /// "The  was not suitable for salvaging."
        /// </summary>
        The_WasNotSuitableForSalvaging = 0x04BF,

        /// <summary>
        /// The  contains the wrong material.
        /// </summary>
        The_ContainseTheWrongMaterial = 0x04C0,

        /// <summary>
        /// You must be  to use that item's magic.
        /// </summary>
        YouMustBe_ToUseItemMagic = 0x04C6,

        /// <summary>
        /// Your  is too low to use that item's magic.
        /// </summary>
        Your_IsTooLowToUseItemMagic = 0x04C9,

        /// <summary>
        /// Only  may use that item's magic.
        /// </summary>
        Only_MayUseItemMagic = 0x04CA,

        /// <summary>
        /// You must have  specialized to use that item's magic.
        /// </summary>
        YouMustSpecialize_ToUseItemMagic = 0x04CB,

        /// <summary>
        ///  is too busy to accept gifts right now.
        /// </summary>
        AiRefuseItemDuringEmote = 0x04CE,

        /// <summary>
        ///  cannot accept stacked objects. Try giving one at a time.
        /// </summary>
        _CannotAcceptStackedItems = 0x04CF,

        /// <summary>
        /// Your  skill must be trained, not untrained or specialized, in order to be altered in this way!
        /// </summary>
        Your_SkillMustBeTrained = 0x04D1,

        /// <summary>
        /// You do not have enough skill credits to specialize your  skill.
        /// </summary>
        NotEnoughSkillCreditsToSpecialize = 0x04D2,

        /// <summary>
        /// You have too many available experience points to be able to absorb the experience points from your  skill. Please spend some of your experience points and try again.
        /// </summary>
        TooMuchXPToRecoverFromSkill = 0x04D3,

        /// <summary>
        /// Your  skill is already untrained!
        /// </summary>
        Your_SkillIsAlreadyUntrained = 0x04D4,

        /// <summary>
        /// You are currently wielding items which require a certain level of .  Your  skill cannot be lowered while you are wielding these items.  Please remove these items and try again.
        /// </summary>
        CannotLowerSkillWhileWieldingItem = 0x04D5,

        /// <summary>
        /// You have succeeded in specializing your  skill!
        /// </summary>
        YouHaveSucceededSpecializing_Skill = 0x04D6,

        /// <summary>
        /// You have succeeded in lowering your  skill from specialized to trained!
        /// </summary>
        YouHaveSucceededUnspecializing_Skill = 0x04D7,

        /// <summary>
        /// You have succeeded in untraining your  skill!
        /// </summary>
        YouHaveSucceededUntraining_Skill = 0x04D8,

        /// <summary>
        /// Although you cannot untrain your  skill, you have succeeded in recovering all the experience you had invested in it.
        /// </summary>
        CannotUntrain_SkillButRecoveredXP = 0x04D9,

        /// <summary>
        /// You have too many credits invested in specialized skills already! Before you can specialize your  skill, you will need to unspecialize some other skill.
        /// </summary>
        TooManyCreditsInSpecializedSkills = 0x04DA,

        /// <summary>
        /// Sends {message}\n to the chat window. Unknown usage.
        /// </summary>
        AttributeTransferFromTooLow = 0x04DE,

        /// <summary>
        /// Sends {message}\n to the chat window. Unknown usage.
        /// </summary>
        AttributeTransferToTooHigh = 0x04DF,

        /// <summary>
        /// The  cannot be used while on a hook and only the owner may open the hook.
        /// </summary>
        ItemUnusableOnHook_CannotOpen = 0x04E8,

        /// <summary>
        /// The  cannot be used while on a hook, use the '@house hooks on' command to make the hook openable.
        /// </summary>
        ItemUnusableOnHook_CanOpen = 0x04E9,

        /// <summary>
        /// The  can only be used while on a hook.
        /// </summary>
        ItemOnlyUsableOnHook = 0x04EA,

        /// <summary>
        ///  fails to affect you because $s cannot affect anyone!
        /// </summary>
        _FailsToAffectYou_TheyCannotAffectAnyone = 0x04F4,

        /// <summary>
        ///  fails to affect you because you cannot be harmed!
        /// </summary>
        _FailsToAffectYou_YouCannotBeHarmed = 0x04F5,

        /// <summary>
        ///  fails to affect you because  is not a player killer!
        /// </summary>
        _FailsToAffectYou_TheyAreNotPK = 0x04F6,

        /// <summary>
        ///  fails to affect you because you are not a player killer!
        /// </summary>
        _FailsToAffectYou_YouAreNotPK = 0x04F7,

        /// <summary>
        ///  fails to affect you because you are not the same sort of player killer as !
        /// </summary>
        _FailsToAffectYou_NotSamePKType = 0x04F8,

        /// <summary>
        ///  fails to affect you across a house boundary!
        /// </summary>
        _FailsToAffectYouAcrossHouseBoundary = 0x04F9,

        /// <summary>
        ///  is an invalid target.
        /// </summary>
        _IsAnInvalidTarget = 0x04FA,

        /// <summary>
        /// You are an invalid target for the spell of .
        /// </summary>
        YouAreInvalidTargetForSpellOf_ = 0x04FB,

        /// <summary>
        ///  is already at full health!
        /// </summary>
        _IsAtFullHealth = 0x04FF,

        /// <summary>
        ///  has no appropriate targets equipped for this spell.
        /// </summary>
        _HasNoSpellTargets = 0x0509,

        /// <summary>
        /// You have no appropriate targets equipped for 's spell.
        /// </summary>
        YouHaveNoTargetsForSpellOf_ = 0x050A,

        /// <summary>
        ///  is now an open fellowship; anyone may recruit new members.
        /// </summary>
        _IsNowOpenFellowship = 0x050B,

        /// <summary>
        ///  is now a closed fellowship.
        /// </summary>
        _IsNowClosedFellowship = 0x050C,

        /// <summary>
        ///  is now the leader of this fellowship.
        /// </summary>
        _IsNowLeaderOfFellowship = 0x050D,

        /// <summary>
        /// You have passed leadership of the fellowship to 
        /// </summary>
        YouHavePassedFellowshipLeadershipTo_ = 0x050E,

        /// <summary>
        /// You may not hook any more  on your house.  You already have the maximum number of  hooked or you are not permitted to hook any on your type of house.
        /// </summary>
        MaxNumberOf_Hooked = 0x0510,

        /// <summary>
        /// You now have the maximum number of  hooked.  You cannot hook any additional  until you remove one or more from your house.
        /// </summary>
        MaxNumberOf_HookedUntilOneIsRemoved = 0x0514,

        /// <summary>
        /// You no longer have the maximum number of  hooked.  You may hook additional .
        /// </summary>
        NoLongerMaxNumberOf_Hooked = 0x0515,

        /// <summary>
        ///  is not close enough to your level.
        /// </summary>
        _IsNotCloseEnoughToYourLevel = 0x0517,

        /// <summary>
        /// This fellowship is locked;  cannot be recruited into the fellowship.
        /// </summary>
        LockedFellowshipCannotRecruit_ = 0x0518,

        /// <summary>
        /// You have entered the  channel.
        /// </summary>
        YouHaveEnteredThe_Channel = 0x051B,

        /// <summary>
        /// You have left the  channel.
        /// </summary>
        YouHaveLeftThe_Channel = 0x051C,

        /// <summary>
        ///  will not receive your message, please use urgent assistance to speak with an in-game representative
        /// </summary>
        _WillNotReceiveMessage = 0x051E,

        /// <summary>
        /// "Message Blocked: "
        /// </summary>
        MessageBlocked_ = 0x051F,

        /// <summary>
        ///  has been added to the list of people you can hear.
        /// </summary>
        _HasBeenAddedToHearList = 0x0521,

        /// <summary>
        ///  has been removed from the list of people you can hear.
        /// </summary>
        _HasBeenRemovedFromHearList = 0x0522,

        /// <summary>
        /// You fail to remove  from your loud list.
        /// </summary>
        FailToRemove_FromLoudList = 0x0525,

        /// <summary>
        /// The fellowship is locked; you cannot open locked fellowships.
        /// </summary>
        YouCannotOpenLockedFellowship = 0x0528,

        /// <summary>
        /// You are now snooping on .
        /// </summary>
        YouAreNowSnoopingOn_ = 0x052C,

        /// <summary>
        /// You are no longer snooping on .
        /// </summary>
        YouAreNoLongerSnoopingOn_ = 0x052D,

        /// <summary>
        /// You fail to snoop on .
        /// </summary>
        YouFailToSnoopOn_ = 0x052E,

        /// <summary>
        ///  attempted to snoop on you.
        /// </summary>
        _AttemptedToSnoopOnYou = 0x052F,

        /// <summary>
        ///  is already being snooped on, only one person may snoop on another at a time.
        /// </summary>
        _IsAlreadyBeingSnoopedOn = 0x0530,

        /// <summary>
        ///  is in limbo and cannot receive your message.
        /// </summary>
        _IsInLimbo = 0x0531,

        /// <summary>
        /// You have been booted from your allegiance chat room. Use "@allegiance chat on" to rejoin. ({message}).
        /// </summary>
        YouHaveBeenBootedFromAllegianceChat = 0x0533,

        /// <summary>
        ///  has been booted from the allegiance chat room.
        /// </summary>
        _HasBeenBootedFromAllegianceChat = 0x0534,

        /// <summary>
        /// The account of  is already banned from the allegiance.
        /// </summary>
        AccountOf_IsAlreadyBannedFromAllegiance = 0x0536,

        /// <summary>
        /// The account of  is not banned from the allegiance.
        /// </summary>
        AccountOf_IsNotBannedFromAllegiance = 0x0537,

        /// <summary>
        /// The account of  was not unbanned from the allegiance.
        /// </summary>
        AccountOf_WasNotUnbannedFromAllegiance = 0x0538,

        /// <summary>
        /// The account of  has been banned from the allegiance.
        /// </summary>
        AccountOf_IsBannedFromAllegiance = 0x0539,

        /// <summary>
        /// The account of  is no longer banned from the allegiance.
        /// </summary>
        AccountOf_IsUnbannedFromAllegiance = 0x053A,

        /// <summary>
        /// Banned Characters: 
        /// </summary>
        ListOfBannedCharacters = 0x053B,

        /// <summary>
        ///  is banned from the allegiance!
        /// </summary>
        _IsBannedFromAllegiance = 0x053E,

        /// <summary>
        /// You are banned from 's allegiance!
        /// </summary>
        YouAreBannedFromAllegiance = 0x053F,

        /// <summary>
        ///  is now an allegiance officer.
        /// </summary>
        _IsNowAllegianceOfficer = 0x0541,

        /// <summary>
        /// An unspecified error occurred while attempting to set  as an allegiance officer.
        /// </summary>
        ErrorSetting_AsAllegianceOfficer = 0x0542,

        /// <summary>
        ///  is no longer an allegiance officer.
        /// </summary>
        _IsNoLongerAllegianceOfficer = 0x0543,

        /// <summary>
        /// An unspecified error occurred while attempting to remove  as an allegiance officer.
        /// </summary>
        ErrorRemoving_AsAllegianceOFficer = 0x0544,

        /// <summary>
        /// You must wait  before communicating again!
        /// </summary>
        YouMustWait_BeforeCommunicating = 0x0547,

        /// <summary>
        /// Your allegiance officer status has been modified. You now hold the position of: .
        /// </summary>
        YourAllegianceOfficerStatusChanged = 0x0549,

        /// <summary>
        ///  is already an allegiance officer of that level.
        /// </summary>
        _IsAlreadyAllegianceOfficerOfThatLevel = 0x054B,

        /// <summary>
        /// The  is currently in use.
        /// </summary>
        The_IsCurrentlyInUse = 0x54D,

        /// <summary>
        /// You are not listening to the  channel!
        /// </summary>
        YouAreNotListeningTo_Channel = 0x0551,

        /// <summary>
        /// Sends "{message}\n" to the chat window. Unknown usage
        /// </summary>
        AugmentationSkillNotTrained = 0x055A,

        /// <summary>
        /// Congratulations! You have succeeded in acquiring the  augmentation.
        /// </summary>
        YouSuccededAcquiringAugmentation = 0x055B,

        /// <summary>
        /// Although your augmentation will not allow you to untrain your  skill, you have succeeded in recovering all the experience you had invested in it.
        /// </summary>
        YouSucceededRecoveringXPFromSkill_AugmentationNotUntrainable = 0x055C,

        /// <summary>
        /// Sends "{message}\n" to the chat window. Unknown usage
        /// </summary>
        AFK = 0x055E,

        /// <summary>
        ///  is already on your friends list!
        /// </summary>
        _IsAlreadyOnYourFriendsList = 0x0562,

        /// <summary>
        /// You may only change your allegiance name once every 24 hours. You may change your allegiance name again in {message}.
        /// </summary>
        YouMayOnlyChangeAllegianceNameOnceEvery24Hours = 0x056A,

        /// <summary>
        ///  is the monarch and cannot be promoted or demoted.
        /// </summary>
        _IsTheMonarchAndCannotBePromotedOrDemoted = 0x056D,

        /// <summary>
        /// That level of allegiance officer is now known as: {message}.
        /// </summary>
        ThatLevelOfAllegianceOfficerIsNowKnownAs_ = 0x056E,

        /// <summary>
        /// Your allegiance is currently: .
        /// </summary>
        YourAllegianceIsCurrently_ = 0x0574,

        /// <summary>
        /// Your allegiance is now: .
        /// </summary>
        YourAllegianceIsNow_ = 0x0575,

        /// <summary>
        /// You may not accept the offer of allegiance from  because your allegiance is locked.
        /// </summary>
        YouCannotAcceptAllegiance_YourAllegianceIsLocked = 0x0576,

        /// <summary>
        /// You may not swear allegiance at this time because the allegiance of  is locked.
        /// </summary>
        YouCannotSwearAllegiance_AllegianceOf_IsLocked = 0x0577,

        /// <summary>
        /// You have pre-approved  to join your allegiance.
        /// </summary>
        YouHavePreApproved_ToJoinAllegiance = 0x0578,

        /// <summary>
        ///  is already a member of your allegiance!
        /// </summary>
        _IsAlreadyMemberOfYourAllegiance = 0x057A,

        /// <summary>
        ///  has been pre-approved to join your allegiance.
        /// </summary>
        _HasBeenPreApprovedToJoinYourAllegiance = 0x057B,

        /// <summary>
        /// Your allegiance chat privileges have been temporarily removed by . Until they are restored, you may not view or speak in the allegiance chat channel.
        /// </summary>
        YourAllegianceChatPrivilegesRemoved = 0x057F,

        /// <summary>
        ///  is now temporarily unable to view or speak in allegiance chat. The gag will run out in 5 minutes, or  may be explicitly ungagged before then.
        /// </summary>
        _IsTemporarilyGaggedInAllegianceChat = 0x0580,

        /// <summary>
        /// Your allegiance chat privileges have been restored by .
        /// </summary>
        YourAllegianceChatPrivilegesRestoredBy_ = 0x0582,

        /// <summary>
        /// You have restored allegiance chat privileges to .
        /// </summary>
        YouRestoreAllegianceChatPrivilegesTo_ = 0x0583,

        /// <summary>
        ///  cowers from you!
        /// </summary>
        _CowersFromYou = 0x058A,
    }
}
