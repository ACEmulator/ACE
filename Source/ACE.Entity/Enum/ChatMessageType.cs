namespace ACE.Entity.Enum
{
    /// <summary>
    /// The ChatMessageType categorizes chat window messages to control color and filtering.<para />
    /// Used with 02BB: Creature Message<para />
    ///     0x02, 0x0C, 0x11
    /// Used with 02BC: Creature Message (Ranged)<para />
    ///     0x0C
    /// Used with F7B0 02BD: Game Event -> Someone has sent you a @tell.<para />
    ///     0x03,
    /// Used with F7E0: Server Message
    ///     0x00, 0x03, 0x04, 0x05, 0x06, 0x07, 0x0D, 0x10, 0x11, 0x17, 0x18
    /// </summary>
    public enum ChatMessageType: uint
    {
        /// <summary>
        /// allegiance MOTD
        /// 
        /// Failed to leave chat room.
        /// 
        /// You give Mr Muscles 10 Gems of Knowledge.
        /// 
        /// You roast the cacao beans.
        /// 
        /// Your chat privileges have been restored.
        /// 
        /// Buff Dude is online
        /// 
        /// The Mana Stone drains 3,502 points of mana from the Staff.
        /// The Staff is destroyed.
        /// 
        /// The Mana Stone gives 26,693 points of mana to the following items: Sparring Pants, Chainmail Tassets, Sollerets, Platemail Gauntlets, Sparring Shirt, Celdon Breastplate, Bracelet, Chainmail Bracers, Veil of Darkness
        /// Your items are fully charged.
        /// 
        /// The Mana Stone gives 3,123 points of mana to the following items: Frigid Bracelet, Silifi of Crimson Night, Tunic, Sleeves of Inexhaustibility, Breeches
        /// You need 3,640 more mana to fully charge your items.
        /// 
        /// LogTextTypeEnumMapper: Default
        /// </summary>
        Broadcast           = 0x00,

        /// <summary>
        /// The client sends this for message HandleActionModifyCharacterSquelch
        /// 
        /// LogTextTypeEnumMapper: AllChannels
        /// </summary>
        AllChannels         = 0x01,

        /// <summary>
        /// LogTextTypeEnumMapper: Speech
        /// </summary>
        Speech              = 0x02,

        /// <summary>
        /// 
        /// Via F7E0:
        /// Buff Dude has added you to their home's guest list.  You now have access to their home.,
        /// Buff Dude has granted you access to their home's storage.,
        /// Buff DudeRipley has removed all house guests, including yourself.,
        /// 
        /// LogTextTypeEnumMapper: Tell
        /// </summary>
        Tell                = 0x03,

        /// <summary>
        /// You tell ...
        /// 
        /// LogTextTypeEnumMapper: Speech_Direct_Send
        /// </summary>
        OutgoingTell        = 0x04,

        /// <summary>
        /// Warning!  You have not paid your maintenance costs for the last 30 day maintenance period.  Please pay these costs by this deadline or you will lose your house, and all your items within it.
        /// Some Guy has discovered the Wayfarer's Pearl!
        /// 
        /// LogTextTypeEnumMapper: System
        /// </summary>
        System              = 0x05,

        /// <summary>
        /// You receive 18 points of periodic nether damage.
        /// You suffer 4 points of minor impact damage.
        /// Dirty Fighting! Big Guy delivers a Unbalancing Blow to Armored Tusker!,
        /// 
        /// LogTextTypeEnumMapper: Combat
        /// </summary>
        Combat              = 0x06,

        /// <summary>
        /// Prismatic Amuli Leggings cast Epic Quickness on you
        /// The spell Brogard's Defiance on Baggy Pants has expired.
        /// You resist the spell cast by Someone
        /// Some Guy tried and failed to cast a spell at you!
        /// You cast Incantation of Revitalize Self and restore 172 points of your stamina.
        /// 
        /// LogTextTypeEnumMapper: Magic
        /// </summary>
        Magic                = 0x07,

        /// <summary>
        /// Light Pink Text - Both of the following two were associated with the following channels: admin, audit, av1, av2, av3, sentinel
        /// output: You say on the [channel name] channel, "message here"
        /// 
        /// LogTextTypeEnumMapper: Channel
        /// </summary>
        Channel             = 0x08,
                
        /// <summary>
        /// LogTextTypeEnumMapper: Channel_Send
        /// </summary>
        ChannelSend         = 0x09,

        /// <summary>
        /// Bright Yellow Text
        /// LogTextTypeEnumMapper: Social
        /// </summary>
        Social              = 0x0A,

        /// <summary>
        /// Light Yellow Text
        /// LogTextTypeEnumMapper: Social_Send
        /// </summary>
        SocialSend          = 0x0B,

        /// <summary>
        /// Via 0x02BB:
        /// All's quiet on this front!Enum_000C = 0x000C 
        /// Lemme alone.  And keep my door closed!
        /// Haha!
        /// Au virinaa! Au... baeaa?
        /// Suvani hasode, Metresa, tamaa Dar Hallae.
        /// Arena One is now available for new warriors!
        /// 
        /// Via 0x02BC:
        /// Did you know there are other people who collect other items, like gromnie teeth?  You might find them in some towns.  I'll take them, but I won't pay you for them!
        /// I collect only phyntos wasp wings.  I'll reward you well for any you happen to have.
        /// Avoid the great crater, I say; the caves there are none too pleasant, even for me!  Oh, the fumes!
        /// If we cannot think of anything quieter and tidier than that... We are not any better than these humans...
        /// There are other beings that will exchange items and currency with you for the remains of creatures such as husks and skeletal pieces.
        /// Very rarely, you can get a perfect red phyntos wasp wing.  If you can give one to me, I'll pay you for it.  I will also pay you for the tails of rats.
        /// I can take the hides of certain creatures and turn them into items of value.
        /// Some say that two brews based on two yeasts of varying qualities are indistinguishable from one another. To those people I say, "You have the taste of a Drudge and the brains of a Banderling!" Heathens, I say. Heathens!
        /// Phyntos wasps are not my favorite creature, but I do admire the wings.
        /// Have you a drudge charm, swamp stone, rat tail, or such? I'll pay you good money or items if you give them to me. They're hard to come by.
        /// Have you the skins of armoredillos, gromnies, or reedsharks?  I can use them in my craft.
        /// Damnable Mukkir...  They get everywhere...
        /// 
        /// LogTextTypeEnumMapper: Emote
        /// </summary>
        Emote               = 0x0C,

        /// <summary>
        /// You are now level 5!
        /// Your base Heavy Weapons skill is now 70!
        /// 
        /// You are now level 68!
        /// You have 100,647,873 experience points and 3 skill credits available to raise skills and attributes.
        /// You will earn another skill credit at level 70.,
        /// 
        /// LogTextTypeEnumMapper: Advancement
        /// </summary>
        Advancement         = 0x0D,

        /// <summary>
        /// Light Cyan (skyblue?) Text - Would seem to be associated with the following channel: Abuse
        /// output: You say on the Abuse channel, "message here"
        /// 
        /// LogTextTypeEnumMapper: Abuse
        /// </summary>
        Abuse               = 0x0E,

        /// <summary>
        /// Red Text - Possibly OutgoingHelpSay, not even sure if that showed up on the client when you sent out an urgent help command
        /// 
        /// LogTextTypeEnumMapper: Help
        /// </summary>
        Help                = 0x0F,

        /// <summary>
        /// Mr Sneaky tried and failed to assess you!
        /// 
        /// LogTextTypeEnumMapper: Appraisal
        /// </summary>
        Appraisal           = 0x10,

        /// <summary>
        /// Via 02BB:
        /// Malar Quaril
        /// Puish Zharil
        /// 
        /// Via F7E0:
        /// Aetheria surges on Pyreal Target Drudge with the power of Surge of Affliction!
        /// The cloak of Some Guy weaves the magic of Cloaked in Skill!
        /// 
        /// LogTextTypeEnumMapper: Spellcasting
        /// </summary>
        Spellcasting        = 0x11,

        /// <summary>
        /// Fellow warriors, aid me!
        /// 
        /// LogTextTypeEnumMapper: Allegiance
        /// </summary>
        Allegiance           = 0x12,

        /// <summary>
        /// Bright Yellow Text
        /// 
        /// LogTextTypeEnumMapper: Fellowship
        /// </summary>
        Fellowship           = 0x13,

        /// <summary>
        /// Green Text
        /// 
        /// LogTextTypeEnumMapper: World_Broadcast
        /// </summary>
        WorldBroadcast      = 0x14,

        /// <summary>
        /// Red Text
        /// 
        /// LogTextTypeEnumMapper: Combat_Enemy
        /// </summary>
        CombatEnemy         = 0x15,

        /// <summary>
        /// Pink Text
        /// 
        /// LogTextTypeEnumMapper: Combat_Self
        /// </summary>
        CombatSelf          = 0x16,

        /// <summary>
        /// Player is recalling home.
        /// 
        /// LogTextTypeEnumMapper: Recall
        /// </summary>
        Recall              = 0x17,

        /// <summary>
        /// Super Tink fails to apply the Fire Opal Salvage (workmanship 10.00) to the White Sapphire Fire Baton. The target is destroyed.
        /// Super Tink successfully applies the Steel Salvage (workmanship 10.00) to the Silver Signet Crown.,
        /// 
        /// LogTextTypeEnumMapper: Craft
        /// </summary>
        Craft               = 0x18,

        /// <summary>
        /// Green Text
        /// LogTextTypeEnumMapper: Salvaging
        /// </summary>
        Salvaging           = 0x19,

        /// <summary>
        /// Does Nothing - Unknown purpose/filter?
        /// Client doesn't display it. Commenting out because why offer it as an option?
        /// </summary>
        // x1A                 = 0x1A,

        /// <summary>
        /// Light cyan(sky blue) - Unknown purpose/filter?
        /// </summary>
        x1B                 = 0x1B,
        x1C                 = 0x1C,
        x1D                 = 0x1D,

        /// <summary>
        /// Light Cyan (skyblue?) Text - Unknown purpose/filter?
        /// </summary>
        x1E                 = 0x1E, 

        /// <summary>
        /// Bright Yellow Text
        /// 
        /// LogTextTypeEnumMapper: Admin_Tell
        /// </summary>
        AdminTell           = 0x1F,
    }

    public static class ChatMessageTypeExtensions
    {
        public static SquelchMask ToMask(this ChatMessageType type)
        {
            if (type == ChatMessageType.AllChannels)
                return SquelchMask.AllChannels;
            else
                return (SquelchMask)(1 << (int)type);
        }
    }
}
