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
        Enum_001D = 0x001D,

        /// <summary>
        /// You can't jump while in the air
        /// </summary>
        Enum_0024 = 0x0024,

        /// <summary>
        /// That is not a valid command.
        /// </summary>
        Enum_0026 = 0x0026,

        /// <summary>
        /// You are too encumbered to carry that!
        /// </summary>
        Enum_002A = 0x002A,

        /// <summary>
        /// Action cancelled!
        /// </summary>
        Enum_0036 = 0x0036,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        Enum_0037 = 0x0037,

        /// <summary>
        /// Unable to move to object!
        /// </summary>
        Enum_0039 = 0x0039,

        /// <summary>
        /// You can't do that... you're dead!
        /// </summary>
        Enum_003A = 0x003A,

        Enum_003C = 0x003C,

        /// <summary>
        /// You charged too far!
        /// </summary>
        Enum_003D = 0x003D,

        /// <summary>
        /// You are too tired to do that!
        /// </summary>
        Enum_003E = 0x003E,

        /// <summary>
        /// You can't jump from this position
        /// </summary>
        Enum_0048 = 0x0048,

        /// <summary>
        /// Ack! You killed yourself!
        /// </summary>
        Enum_004A = 0x004A,

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
        YourAttackMisfired = 0x03F9,

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
        YourSpellsTargetIsMissing = 0x0403,

        /// <summary>
        /// Your projectile spell mislaunched!
        /// </summary>
        YourProjectileSpellMislaunched = 0x0404,

        /// <summary>
        /// Your spell cannot be cast outside
        /// </summary>
        Enum_0407 = 0x0407,

        /// <summary>
        /// Your spell cannot be cast inside
        /// </summary>
        Enum_0408 = 0x0408,

        Enum_0409 = 0x0409,

        /// <summary>
        /// You are unprepared to cast a spell
        /// </summary>
        Enum_040A = 0x040A,

        /// <summary>
        /// You are not in an allegiance!
        /// </summary>
        YouAreNotInAnAllegiance = 0x0414,

        /// <summary>
        /// You must be the leader of a Fellowship
        /// </summary>
        Enum_041D = 0x041D,

        /// <summary>
        /// Your Fellowship is full
        /// </summary>
        Enum_041E = 0x041E,

        /// <summary>
        /// That Fellowship name is not permitted
        /// </summary>
        Enum_041F = 0x041F,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        Enum_0433 = 0x0433,

        /// <summary>
        /// Your craft attempt fails.
        /// </summary>
        Enum_0435 = 0x0435,

        Enum_0436 = 0x0436,

        /// <summary>
        /// Either you or one of the items involved does not pass the requirements for this craft interaction.
        /// </summary>
        Enum_0437 = 0x0437,

        /// <summary>
        /// You do not have all the neccessary items.
        /// </summary>
        Enum_0438 = 0x0438,

        /// <summary>
        /// Not all the items are avaliable.
        /// </summary>
        Enum_0439 = 0x0439,

        /// <summary>
        /// You must be at rest in peace mode to do trade skills.
        /// </summary>
        Enum_043A = 0x043A,

        /// <summary>
        /// You are not trained in that trade skill.
        /// </summary>
        Enum_043B = 0x043B,

        /// <summary>
        /// You cannot link to that portal!
        /// </summary>
        Enum_043D = 0x043D,

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
        Enum_0445 = 0x0445,

        Enum_0446 = 0x0446,

        /// <summary>
        /// Player killers may not interact with that portal!
        /// </summary>
        Enum_045C = 0x045C,

        /// <summary>
        /// Non-player killers may not interact with that portal!
        /// </summary>
        Enum_045D = 0x045D,

        Enum_0465 = 0x0465,

        /// <summary>
        /// You must purchase Asheron's Call: Dark Majesty to interact with that portal.
        /// </summary>
        Enum_0466 = 0x0466,

        /// <summary>
        /// You have used all the hooks you are allowed to use for this house.
        /// </summary>
        Enum_0469 = 0x0469,

        /// <summary>
        /// You must complete a quest to interact with that portal.
        /// </summary>
        Enum_0474 = 0x0474,

        /// <summary>
        /// You must own a house to use this command.
        /// </summary>
        Enum_047F = 0x047F,

        /// <summary>
        /// You can't lock or unlock that!
        /// </summary>
        Enum_0480 = 0x0480,

        /// <summary>
        /// You can't lock or unlock what is open!
        /// </summary>
        Enum_0481 = 0x0481,

        /// <summary>
        /// You must be a monarch to purchase this dwelling.
        /// </summary>
        Enum_048A = 0x048A,

        /// <summary>
        /// Your Allegiance has been dissolved!
        /// </summary>
        Enum_0496 = 0x0496,

        /// <summary>
        /// Your patron's Allegiance to you has been broken!
        /// </summary>
        Enum_0497 = 0x0497,

        /// <summary>
        /// You have moved too far!
        /// </summary>
        Enum_0498 = 0x0498,

        /// <summary>
        /// You fail to link with the lifestone!
        /// </summary>
        Enum_049B = 0x049B,

        /// <summary>
        /// You wandered too far to link with the lifestone!
        /// </summary>
        Enum_049C = 0x049C,

        /// <summary>
        /// You successfully link with the lifestone!
        /// </summary>
        Enum_049D = 0x049D,

        /// <summary>
        /// You must have linked with a lifestone in order to recall to it!
        /// </summary>
        Enum_049E = 0x049E,

        /// <summary>
        /// You fail to recall to the lifestone!
        /// </summary>
        Enum_049F = 0x049F,

        /// <summary>
        /// You fail to link with the portal!
        /// </summary>
        Enum_04A0 = 0x04A0,

        /// <summary>
        /// You successfully link with the portal!
        /// </summary>
        Enum_04A1 = 0x04A1,

        /// <summary>
        /// You fail to recall to the portal!
        /// </summary>
        Enum_04A2 = 0x04A2,

        /// <summary>
        /// You must have linked with a portal in order to recall to it!
        /// </summary>
        Enum_04A3 = 0x04A3,

        /// <summary>
        /// You fail to summon the portal!
        /// </summary>
        Enum_04A4 = 0x04A4,

        /// <summary>
        /// You must have linked with a portal in order to summon it!
        /// </summary>
        Enum_04A5 = 0x04A5,

        /// <summary>
        /// You fail to teleport!
        /// </summary>
        Enum_04A6 = 0x04A6,

        /// <summary>
        /// You have been teleported too recently!
        /// </summary>
        Enum_04A7 = 0x04A7,

        /// <summary>
        /// You must be an Advocate to interact with that portal.
        /// </summary>
        Enum_04A8 = 0x04A8,

        /// <summary>
        /// Players may not interact with that portal.
        /// </summary>
        Enum_04AA = 0x04AA,

        /// <summary>
        /// You are not powerful enough to interact with that portal!
        /// </summary>
        Enum_04AB = 0x04AB,

        /// <summary>
        /// You are too powerful to interact with that portal!
        /// </summary>
        Enum_04AC = 0x04AC,

        /// <summary>
        /// You cannot recall to that portal!
        /// </summary>
        Enum_04AD = 0x04AD,

        /// <summary>
        /// You cannot summon that portal!
        /// </summary>
        Enum_04AE = 0x04AE,

        /// <summary>
        /// The key doesn't fit this lock.
        /// </summary>
        Enum_04B2 = 0x04B2,

        /// <summary>
        /// You do not own that salvage tool!
        /// </summary>
        Enum_04BD = 0x04BD,

        /// <summary>
        /// You do not own that item!
        /// </summary>
        Enum_04BE = 0x04BE,

        /// <summary>
        /// The material cannot be created.
        /// </summary>
        Enum_04C1 = 0x04C1,

        /// <summary>
        /// The list of items you are attempting to salvage is invalid.
        /// </summary>
        Enum_04C2 = 0x04C2,

        /// <summary>
        /// You cannot salvage items that you are trading!
        /// </summary>
        Enum_04C3 = 0x04C3,

        /// <summary>
        /// You must be a guest in this house to interact with that portal.
        /// </summary>
        Enum_04C4 = 0x04C4,

        /// <summary>
        /// Your Allegiance Rank is too low to use that item's magic.
        /// </summary>
        Enum_04C5 = 0x04C5,

        /// <summary>
        /// Your Arcane Lore skill is too low to use that item's magic.
        /// </summary>
        Enum_04C7 = 0x04C7,

        /// <summary>
        /// That item doesn't have enough Mana.
        /// </summary>
        Enum_04C8 = 0x04C8,

        /// <summary>
        /// You have been involved in a player killer battle too recently to do that!
        /// </summary>
        Enum_04CC = 0x04CC,

        Enum_04DC = 0x04DC,

        /// <summary>
        /// You have failed to alter your attributes.
        /// </summary>
        Enum_04DD = 0x04DD,

        /// <summary>
        /// You are currently wielding items which require a certain level of skill. Your attributes cannot be transferred while you are wielding these items. Please remove these items and try again.
        /// </summary>
        Enum_04E0 = 0x04E0,

        /// <summary>
        /// You have succeeded in transferring your attributes!
        /// </summary>
        Enum_04E1 = 0x04E1,

        /// <summary>
        /// This hook is a duplicated housing object. You may not add items to a duplicated housing object. Please empty the hook and allow it to reset.
        /// </summary>
        Enum_04E2 = 0x04E2,

        /// <summary>
        /// That item is of the wrong type to be placed on this hook.
        /// </summary>
        Enum_04E3 = 0x04E3,

        /// <summary>
        /// This chest is a duplicated housing object. You may not add items to a duplicated housing object. Please empty everything -- including backpacks -- out of the chest and allow the chest to reset.
        /// </summary>
        Enum_04E4 = 0x04E4,

        /// <summary>
        /// This hook was a duplicated housing object. Since it is now empty, it will be deleted momentarily. Once it is gone, it is safe to use the other, non-duplicated hook that is here.
        /// </summary>
        Enum_04E5 = 0x04E5,

        /// <summary>
        /// This chest was a duplicated housing object. Since it is now empty, it will be deleted momentarily. Once it is gone, it is safe to use the other, non-duplicated chest that is here.
        /// </summary>
        Enum_04E6 = 0x04E6,

        /// <summary>
        /// You cannot swear allegiance to anyone because you own a monarch-only house. Please abandon your house and try again.
        /// </summary>
        Enum_04E7 = 0x04E7,

        /// <summary>
        /// You cannot modify your player killer status while you are recovering from a PK death.
        /// </summary>
        Enum_04EC = 0x04EC,

        /// <summary>
        /// Advocates may not change their player killer status!
        /// </summary>
        Enum_04ED = 0x04ED,

        /// <summary>
        /// Your level is too low to change your player killer status with this object.
        /// </summary>
        Enum_04EE = 0x04EE,

        /// <summary>
        /// Your level is too high to change your player killer status with this object.
        /// </summary>
        Enum_04EF = 0x04EF,

        /// <summary>
        /// You feel a harsh dissonance, and you sense that an act of killing you have committed recently is interfering with the conversion.
        /// </summary>
        Enum_04F0 = 0x04F0,

        /// <summary>
        /// Bael'Zharon's power flows through you again. You are once more a player killer.
        /// </summary>
        Enum_04F1 = 0x04F1,

        /// <summary>
        /// Bael'Zharon has granted you respite after your moment of weakness. You are temporarily no longer a player killer.
        /// </summary>
        Enum_04F2 = 0x04F2,

        /// <summary>
        /// Lite Player Killers may not interact with that portal!
        /// </summary>
        Enum_04F3 = 0x04F3,

        /// <summary>
        /// You aren't trained in healing!
        /// </summary>
        Enum_04FC = 0x04FC,

        /// <summary>
        /// You aren't ready to heal!
        /// </summary>
        Enum_0500 = 0x0500,

        /// <summary>
        /// You can only use Healing Kits on player characters.
        /// </summary>
        Enum_0501 = 0x0501,

        /// <summary>
        /// The Lifestone's magic protects you from the attack!
        /// </summary>
        Enum_0502 = 0x0502,

        /// <summary>
        /// The portal's residual energy protects you from the attack!
        /// </summary>
        Enum_0503 = 0x0503,

        /// <summary>
        /// You are enveloped in a feeling of warmth as you are brought back into the protection of the Light. You are once again a Non-Player Killer.
        /// </summary>
        Enum_0504 = 0x0504,

        /// <summary>
        /// You're too close to your sanctuary!
        /// </summary>
        Enum_0505 = 0x0505,

        /// <summary>
        /// Only Non-Player Killers may enter PK Lite. Please see @help pklite for more details about this command.
        /// </summary>
        Enum_0507 = 0x0507,

        /// <summary>
        /// A cold wind touches your heart. You are now a Player Killer Lite.
        /// </summary>
        Enum_0508 = 0x0508,

        /// <summary>
        /// You do not belong to a Fellowship.
        /// </summary>
        YouDoNotBelongToAFellowship = 0x050F,

        /// <summary>
        /// You are now using the maximum number of hooks.  You cannot use another hook until you take an item off one of your hooks.
        /// </summary>
        Enum_0512 = 0x0512,

        /// <summary>
        /// You are no longer using the maximum number of hooks.  You may again add items to your hooks.
        /// </summary>
        Enum_0513 = 0x0513,

        /// <summary>
        /// You are not permitted to use that hook.
        /// </summary>
        Enum_0516 = 0x0516,

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
        Enum_0520 = 0x0520,

        /// <summary>
        /// You are now deaf to player's screams.
        /// </summary>
        Enum_0523 = 0x0523,

        /// <summary>
        /// You can hear all players once again.
        /// </summary>
        Enum_0524 = 0x0524,

        /// <summary>
        /// You chicken out.
        /// </summary>
        Enum_0526 = 0x0526,

        /// <summary>
        /// The fellowship is locked; you cannot open locked fellowships.
        /// </summary>
        Enum_0528 = 0x0528,

        /// <summary>
        /// Trade Complete!
        /// </summary>
        Enum_0529 = 0x0529,

        /// <summary>
        /// Character not available.
        /// </summary>
        CharacterNotAvailable = 0x052B,

        /// <summary>
        /// You must wait 30 days after purchasing a house before you may purchase another with any character on the same account. This applies to all housing except apartments.
        /// </summary>
        Enum_0532 = 0x0532,

        /// <summary>
        /// You have been booted from your allegiance chat room. Use "@allegiance chat on" to rejoin. ().
        /// </summary>
        Enum_0533 = 0x0533,

        /// <summary>
        /// You do not have the authority within your allegiance to do that.
        /// </summary>
        Enum_0535 = 0x0535,

        /// <summary>
        /// You have the maximum number of accounts banned.!
        /// </summary>
        Enum_0540 = 0x0540,

        /// <summary>
        /// You already have the maximum number of allegiance officers. You must remove some before you add any more.
        /// </summary>
        Enum_0545 = 0x0545,

        /// <summary>
        /// Your allegiance officers have been cleared.
        /// </summary>
        Enum_0546 = 0x0546,

        /// <summary>
        /// You cannot join any chat channels while gagged.
        /// </summary>
        Enum_0548 = 0x0548,

        /// <summary>
        /// You are no longer an allegiance officer.
        /// </summary>
        Enum_054A = 0x054A,

        /// <summary>
        /// Your allegiance does not have a hometown.
        /// </summary>
        Enum_054C = 0x054C,

        /// <summary>
        /// The hook does not contain a usable item. You cannot open the hook because you do not own the house to which it belongs.
        /// </summary>
        Enum_054E = 0x054E,

        /// <summary>
        /// The hook does not contain a usable item. Use the '@house hooks on'command to make the hook openable.
        /// </summary>
        Enum_054F = 0x054F,

        /// <summary>
        /// You have failed to complete the augmentation.
        /// </summary>
        Enum_0556 = 0x0556,

        /// <summary>
        /// You have used this augmentation too many times already.
        /// </summary>
        Enum_0557 = 0x0557,

        /// <summary>
        /// You have used augmentations of this type too many times already.
        /// </summary>
        Enum_0558 = 0x0558,

        /// <summary>
        /// You do not have enough unspent experience available to purchase this augmentation.
        /// </summary>
        Enum_0559 = 0x0559,

        /// <summary>
        /// You must exit the Training Academy before that command will be available to you.
        /// </summary>
        Enum_055D = 0x055D,

        /// <summary>
        /// Only Player Killer characters may use this command!
        /// </summary>
        Enum_055F = 0x055F,

        /// <summary>
        /// Only Player Killer Lite characters may use this command!
        /// </summary>
        Enum_0560 = 0x0560,

        /// <summary>
        /// That is an invalid officer level.
        /// </summary>
        Enum_056F = 0x056F,

        /// <summary>
        /// That allegiance officer title is not appropriate.
        /// </summary>
        Enum_0570 = 0x0570,

        /// <summary>
        /// That allegiance name is too long. Please choose another name.
        /// </summary>
        Enum_0571 = 0x0571,

        /// <summary>
        /// All of your allegiance officer titles have been cleared.
        /// </summary>
        Enum_0572 = 0x0572,

        /// <summary>
        /// That allegiance title contains illegal characters. Please choose another name using only letters, spaces, - and '.
        /// </summary>
        Enum_0573 = 0x0573,

        /// <summary>
        /// You have not pre-approved any vassals to join your allegiance.
        /// </summary>
        Enum_0579 = 0x0579,

        /// <summary>
        /// You have cleared the pre-approved vassal for your allegiance.
        /// </summary>
        Enum_057C = 0x057C,

        /// <summary>
        /// That character is already gagged!
        /// </summary>
        Enum_057D = 0x057D,

        /// <summary>
        /// That character is not currently gagged!
        /// </summary>
        Enum_057E = 0x057E,

        /// <summary>
        /// Your allegiance chat privileges have been restored.
        /// </summary>
        Enum_0581 = 0x0581,

        /// <summary>
        /// Olthoi cannot interact with that!
        /// </summary>
        Enum_0587 = 0x0587,

        /// <summary>
        /// Olthoi cannot use regular lifestones! Asheron would not allow it!
        /// </summary>
        Enum_0588 = 0x0588,

        /// <summary>
        /// The vendor looks at you in horror!
        /// </summary>
        Enum_0589 = 0x0589,

        /// <summary>
        /// As a mindless engine of destruction an Olthoi cannot join a fellowship!
        /// </summary>
        Enum_058B = 0x058B,

        /// <summary>
        /// The Olthoi only have an allegiance to the Olthoi Queen!
        /// </summary>
        Enum_058C = 0x058C,

        /// <summary>
        /// You cannot use that item!
        /// </summary>
        Enum_058D = 0x058D,

        /// <summary>
        /// This person will not interact with you!
        /// </summary>
        Enum_058E = 0x058E,

        /// <summary>
        /// Only Olthoi may pass through this portal!
        /// </summary>
        Enum_058F = 0x058F,

        /// <summary>
        /// Olthoi may not pass through this portal!
        /// </summary>
        Enum_0590 = 0x0590,

        /// <summary>
        /// You may not pass through this portal while Vitae weakens you!
        /// </summary>
        Enum_0591 = 0x0591,

        /// <summary>
        /// This character must be two weeks old or have been created on an account at least two weeks old to use this portal!
        /// </summary>
        Enum_0592 = 0x0592,

        /// <summary>
        /// Olthoi characters can only use Lifestone and PK Arena recalls!
        /// </summary>
        Enum_0593 = 0x0593,
    }
}
