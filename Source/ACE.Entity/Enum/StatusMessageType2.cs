
namespace ACE.Entity.Enum
{
    /// <summary>
    /// The StatusMessageType2 identifies the specific message to be displayed in the chat window.<para />
    /// Used with F7B0 028B: Game Event -> Display a parameterized status message in the chat window.<para />
    /// <para />
    /// StatusMessageType1 and StatusMessageType2 are actually a single enum in the clients.<para />   
    /// The combined enum is only used by 2 functions in the client.<para />
    /// We split the enum up into 2 enums because each function uses only a specific set of the enum values.<para />
    /// No one value is used by both functions.
    /// </summary>
    public enum StatusMessageType2
    {
        /// <summary>
        ///  is too busy to accept gifts right now.
        /// </summary>
        Enum_001E = 0x001E,

        /// <summary>
        ///  cannot carry anymore.
        /// </summary>
        Enum_002B = 0x002B,

        /// <summary>
        /// You fail to affect  because you cannot affect anyone!
        /// </summary>
        Enum_004E = 0x004E,

        /// <summary>
        /// You fail to affect  because $s cannot be harmed!
        /// </summary>
        Enum_004F = 0x004F,

        /// <summary>
        /// You fail to affect  because beneficial spells do not affect !
        /// </summary>
        Enum_0050 = 0x0050,

        /// <summary>
        /// You fail to affect  because you are not a player killer!
        /// </summary>
        Enum_0051 = 0x0051,

        /// <summary>
        /// You fail to affect  because  is not a player killer!
        /// </summary>
        Enum_0052 = 0x0052,

        /// <summary>
        /// You fail to affect  because you are not the same sort of player killer as !
        /// </summary>
        Enum_0053 = 0x0053,

        /// <summary>
        /// You fail to affect  because you are acting across a house boundary!
        /// </summary>
        Enum_0054 = 0x0054,

        /// <summary>
        ///  is not accepting gifts right now.
        /// </summary>
        Enum_03EF = 0x03EF,

        /// <summary>
        ///  cannot have any more Vassals
        /// </summary>
        Enum_0416 = 0x0416,

        /// <summary>
        ///  doesn't know what to do with that.
        /// </summary>
        Enum_046A = 0x046A,

        /// <summary>
        /// You must be above level  to purchase this dwelling.
        /// </summary>
        Enum_0488 = 0x0488,

        /// <summary>
        /// You must be at or below level  to purchase this dwelling.
        /// </summary>
        Enum_0489 = 0x0489,

        /// <summary>
        /// You must be at or below allegiance rank  to purchase this dwelling.
        /// </summary>
        Enum_048C = 0x048C,

        /// <summary>
        /// You must be  to use that item's magic.
        /// </summary>
        Enum_04C6 = 0x04C6,

        /// <summary>
        /// Your  is too low to use that item's magic.
        /// </summary>
        Enum_04C9 = 0x04C9,

        /// <summary>
        /// Only  may use that item's magic.
        /// </summary>
        Enum_04CA = 0x04CA,

        /// <summary>
        /// You must have  specialized to use that item's magic.
        /// </summary>
        Enum_04CB = 0x04CB,

        /// <summary>
        ///  is too busy to accept gifts right now.
        /// </summary>
        Enum_04CE = 0x04CE,

        /// <summary>
        ///  cannot accept stacked objects. Try giving one at a time.
        /// </summary>
        Enum_04CF = 0x04CF,

        /// <summary>
        /// You have failed to alter your skill.
        /// </summary>
        Enum_04D0 = 0x04D0,

        /// <summary>
        /// Your  skill must be trained, not untrained or specialized, in order to be altered in this way!
        /// </summary>
        Enum_04D1 = 0x04D1,

        /// <summary>
        /// You do not have enough skill credits to specialize your  skill.
        /// </summary>
        Enum_04D2 = 0x04D2,

        /// <summary>
        /// You have too many available experience points to be able to absorb the experience points from your  skill. Please spend some of your experience points and try again.
        /// </summary>
        Enum_04D3 = 0x04D3,

        /// <summary>
        /// Your  skill is already untrained!
        /// </summary>
        Enum_04D4 = 0x04D4,

        /// <summary>
        /// You are currently wielding items which require a certain level of .  Your  skill cannot be lowered while you are wielding these items.  Please remove these items and try again.
        /// </summary>
        Enum_04D5 = 0x04D5,

        /// <summary>
        /// You have succeeded in specializing your  skill!
        /// </summary>
        Enum_04D6 = 0x04D6,

        /// <summary>
        /// You have succeeded in lowering your  skill from specialized to trained!
        /// </summary>
        Enum_04D7 = 0x04D7,

        /// <summary>
        /// You have succeeded in untraining your  skill!
        /// </summary>
        Enum_04D8 = 0x04D8,

        /// <summary>
        /// Although you cannot untrain your  skill, you have succeeded in recovering all the experience you had invested in it.
        /// </summary>
        Enum_04D9 = 0x04D9,

        /// <summary>
        /// You have too many credits invested in specialized skills already! Before you can specialize your  skill, you will need to unspecialize some other skill.
        /// </summary>
        Enum_04DA = 0x04DA,

        /// <summary>
        /// The  cannot be used while on a hook and only the owner may open the hook.
        /// </summary>
        Enum_04E8 = 0x04E8,

        /// <summary>
        /// The  cannot be used while on a hook, use the '@house hooks on' command to make the hook openable.
        /// </summary>
        Enum_04E9 = 0x04E9,

        /// <summary>
        /// The  can only be used while on a hook.
        /// </summary>
        Enum_04EA = 0x04EA,

        /// <summary>
        ///  fails to affect you because $s cannot affect anyone!
        /// </summary>
        Enum_04F4 = 0x04F4,

        /// <summary>
        ///  fails to affect you because you cannot be harmed!
        /// </summary>
        Enum_04F5 = 0x04F5,

        /// <summary>
        ///  fails to affect you because  is not a player killer!
        /// </summary>
        Enum_04F6 = 0x04F6,

        /// <summary>
        ///  fails to affect you because you are not a player killer!
        /// </summary>
        Enum_04F7 = 0x04F7,

        /// <summary>
        ///  fails to affect you because you are not the same sort of player killer as !
        /// </summary>
        Enum_04F8 = 0x04F8,

        /// <summary>
        ///  fails to affect you across a house boundary!
        /// </summary>
        Enum_04F9 = 0x04F9,

        /// <summary>
        ///  is an invalid target.
        /// </summary>
        Enum_04FA = 0x04FA,

        /// <summary>
        /// You are an invalid target for the spell of .
        /// </summary>
        Enum_04FB = 0x04FB,

        /// <summary>
        ///  is already at full health!
        /// </summary>
        Enum_04FF = 0x04FF,

        /// <summary>
        ///  has no appropriate targets equipped for this spell.
        /// </summary>
        Enum_0509 = 0x0509,

        /// <summary>
        /// You have no appropriate targets equipped for 's spell.
        /// </summary>
        Enum_050A = 0x050A,

        /// <summary>
        ///  is now an open fellowship; anyone may recruit new members.
        /// </summary>
        Enum_050B = 0x050B,

        /// <summary>
        ///  is now a closed fellowship.
        /// </summary>
        Enum_050C = 0x050C,

        /// <summary>
        ///  is now the leader of this fellowship.
        /// </summary>
        Enum_050D = 0x050D,

        /// <summary>
        /// You have passed leadership of the fellowship to 
        /// </summary>
        Enum_050E = 0x050E,

        /// <summary>
        /// You may not hook any more  on your house.  You already have the maximum number of  hooked or you are not permitted to hook any on your type of house.
        /// </summary>
        Enum_0510 = 0x0510,

        /// <summary>
        /// You now have the maximum number of  hooked.  You cannot hook any additional  until you remove one or more from your house.
        /// </summary>
        Enum_0514 = 0x0514,

        /// <summary>
        /// You no longer have the maximum number of  hooked.  You may hook additional .
        /// </summary>
        Enum_0515 = 0x0515,

        /// <summary>
        ///  is not close enough to your level.
        /// </summary>
        Enum_0517 = 0x0517,

        /// <summary>
        /// This fellowship is locked;  cannot be recruited into the fellowship.
        /// </summary>
        Enum_0518 = 0x0518,

        /// <summary>
        /// The fellowship is locked, you were not added to the fellowship.
        /// </summary>
        Enum_0519 = 0x0519,

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
        Enum_051E = 0x051E,

        /// <summary>
        ///  has been added to the list of people you can hear.
        /// </summary>
        Enum_0521 = 0x0521,

        /// <summary>
        ///  has been removed from the list of people you can hear.
        /// </summary>
        Enum_0522 = 0x0522,

        /// <summary>
        /// You fail to remove  from your loud list.
        /// </summary>
        Enum_0525 = 0x0525,

        /// <summary>
        /// The fellowship is locked; you cannot open locked fellowships.
        /// </summary>
        Enum_0528 = 0x0528,

        /// <summary>
        /// You are now snooping on .
        /// </summary>
        Enum_052C = 0x052C,

        /// <summary>
        /// You are no longer snooping on .
        /// </summary>
        Enum_052D = 0x052D,

        /// <summary>
        /// You fail to snoop on .
        /// </summary>
        Enum_052E = 0x052E,

        /// <summary>
        ///  attempted to snoop on you.
        /// </summary>
        Enum_052F = 0x052F,

        /// <summary>
        ///  is already being snooped on, only one person may snoop on another at a time.
        /// </summary>
        Enum_0530 = 0x0530,

        /// <summary>
        ///  is in limbo and cannot receive your message.
        /// </summary>
        Enum_0531 = 0x0531,

        /// <summary>
        ///  has been booted from the allegiance chat room.
        /// </summary>
        Enum_0534 = 0x0534,

        /// <summary>
        /// The account of  is already banned from the allegiance.
        /// </summary>
        Enum_0536 = 0x0536,

        /// <summary>
        /// The account of  is not banned from the allegiance.
        /// </summary>
        Enum_0537 = 0x0537,

        /// <summary>
        /// The account of  was not unbanned from the allegiance.
        /// </summary>
        Enum_0538 = 0x0538,

        /// <summary>
        /// The account of  has been banned from the allegiance.
        /// </summary>
        Enum_0539 = 0x0539,

        /// <summary>
        /// The account of  is no longer banned from the allegiance.
        /// </summary>
        Enum_053A = 0x053A,

        /// <summary>
        /// Banned Characters: 
        /// </summary>
        Enum_053B = 0x053B,

        /// <summary>
        ///  is banned from the allegiance!
        /// </summary>
        Enum_053E = 0x053E,

        /// <summary>
        /// You are banned from 's allegiance!
        /// </summary>
        Enum_053F = 0x053F,

        /// <summary>
        ///  is now an allegiance officer.
        /// </summary>
        Enum_0541 = 0x0541,

        /// <summary>
        /// An unspecified error occurred while attempting to set  as an allegiance officer.
        /// </summary>
        Enum_0542 = 0x0542,

        /// <summary>
        ///  is no longer an allegiance officer.
        /// </summary>
        Enum_0543 = 0x0543,

        /// <summary>
        /// An unspecified error occurred while attempting to remove  as an allegiance officer.
        /// </summary>
        Enum_0544 = 0x0544,

        /// <summary>
        /// You must wait  before communicating again!
        /// </summary>
        Enum_0547 = 0x0547,

        /// <summary>
        /// Your allegiance officer status has been modified. You now hold the position of: .
        /// </summary>
        Enum_0549 = 0x0549,

        /// <summary>
        ///  is already an allegiance officer of that level.
        /// </summary>
        Enum_054B = 0x054B,

        /// <summary>
        /// You are not listening to the  channel!
        /// </summary>
        Enum_0551 = 0x0551,

        /// <summary>
        /// Congratulations! You have succeeded in acquiring the  augmentation.
        /// </summary>
        Enum_055B = 0x055B,

        /// <summary>
        /// Although your augmentation will not allow you to untrain your  skill, you have succeeded in recovering all the experience you had invested in it.
        /// </summary>
        Enum_055C = 0x055C,

        /// <summary>
        ///  is already on your friends list!
        /// </summary>
        Enum_0562 = 0x0562,

        /// <summary>
        /// Your allegiance is currently: .
        /// </summary>
        Enum_0574 = 0x0574,

        /// <summary>
        /// Your allegiance is now: .
        /// </summary>
        Enum_0575 = 0x0575,

        /// <summary>
        /// You may not accept the offer of allegiance from  because your allegiance is locked.
        /// </summary>
        Enum_0576 = 0x0576,

        /// <summary>
        /// You may not swear allegiance at this time because the allegiance of  is locked.
        /// </summary>
        Enum_0577 = 0x0577,

        /// <summary>
        /// You have pre-approved  to join your allegiance.
        /// </summary>
        Enum_0578 = 0x0578,

        /// <summary>
        ///  is already a member of your allegiance!
        /// </summary>
        Enum_057A = 0x057A,

        /// <summary>
        ///  has been pre-approved to join your allegiance.
        /// </summary>
        Enum_057B = 0x057B,

        /// <summary>
        /// Your allegiance chat privileges have been temporarily removed by . Until they are restored, you may not view or speak in the allegiance chat channel.
        /// </summary>
        Enum_057F = 0x057F,

        /// <summary>
        ///  is now temporarily unable to view or speak in allegiance chat. The gag will run out in 5 minutes, or  may be explicitly ungagged before then.
        /// </summary>
        Enum_0580 = 0x0580,

        /// <summary>
        /// Your allegiance chat privileges have been restored by .
        /// </summary>
        Enum_0582 = 0x0582,

        /// <summary>
        /// You have restored allegiance chat privileges to .
        /// </summary>
        Enum_0583 = 0x0583,

        /// <summary>
        ///  cowers from you!
        /// </summary>
        Enum_058A = 0x058A,
    }
}
