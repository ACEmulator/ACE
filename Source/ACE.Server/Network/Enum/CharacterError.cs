namespace ACE.Server.Network.Enum
{
    /// <summary>
    /// CharError enum in client<para />
    /// copied from https://github.com/Zegeger/ACE.Network/blob/master/NetworkStack/Enums/CharacterErrorType.cs
    /// </summary>
    public enum CharacterError
    {
        /// <summary>
        /// ID_CHAR_ERROR_LOGON
        /// </summary>
        Logon = 0x00000001,

        /// <summary>
        /// ID_CHAR_ERROR_ACCOUNT_LOGON
        /// </summary>
        AccountLogin = 0x00000003,

        /// <summary>
        /// ID_CHAR_ERROR_SERVER_CRASH
        /// </summary>
        ServerCrash1 = 0x00000004,

        /// <summary>
        /// ID_CHAR_ERROR_LOGOFF
        /// </summary>
        Logoff = 0x00000005,

        /// <summary>
        /// ID_CHAR_ERROR_DELETE
        /// </summary>
        Delete = 0x00000006,

        /// <summary>
        /// ID_CHAR_ERROR_SERVER_CRASH
        /// </summary>
        ServerCrash2 = 0x00000008,

        /// <summary>
        /// ID_CHAR_ERROR_ACCOUNT_INVALID
        /// </summary>
        AccountInvalid = 0x00000009,

        /// <summary>
        /// ID_CHAR_ERROR_ACCOUNT_DOESNT_EXIST
        /// </summary>
        AccountDoesntExist = 0x0000000A,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_GENERIC
        /// </summary>
        EnterGameGeneric = 0x0000000B,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_STRESS_ACCOUNT
        /// </summary>
        EnterGameStressAccount = 0x0000000C,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_IN_WORLD
        /// </summary>
        EnterGameCharacterInWorld = 0x0000000D,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_PLAYER_ACCOUNT_MISSING
        /// </summary>
        EnterGamePlayerAccountMissing = 0x0000000E,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_NOT_OWNED
        /// </summary>
        EnterGameCharacterNotOwned = 0x0000000F,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_IN_WORLD_SERVER
        /// </summary>
        EnterGameCharacterInWorldServer = 0x00000010,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_OLD_CHARACTER
        /// </summary>
        EnterGameOldCharacter = 0x00000011,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CORRUPT_CHARACTER
        /// </summary>
        EnterGameCorruptCharacter = 0x00000012,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_START_SERVER_DOWN
        /// </summary>
        EnterGameStartServerDown = 0x00000013,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_COULDNT_PLACE_CHARACTER
        /// </summary>
        EnterGameCouldntPlaceCharacter = 0x00000014,

        /// <summary>
        /// ID_CHAR_ERROR_LOGON_SERVER_FULL
        /// </summary>
        LogonServerFull = 0x00000015,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_LOCKED
        /// </summary>
        EnterGameCharacterLocked = 0x00000017,

        /// <summary>
        /// ID_CHAR_ERROR_SUBSCRIPTION_EXPIRED
        /// </summary>
        SubscriptionExpired = 0x00000018

        //Undefined,
        //Logon,                                  // Cannot have two accounts logged on at the same time.
        //LoggedOn,                               // Server could not access your account information. Please try again in a few minutes.
        //AccountLogon,
        //ServerCrash,                            // The server has disconnected. Please try again in a few minutes.
        //Logoff,                                 // Server could not log off your character
        //Delete,                                 // Server could not delete your character.
        //NoPremade,
        //AccountInUse,                           // The server has disconnected. Please try again in a few minutes.
        //AccountInvalid,                         // The account name you specified was not valid.
        //AccountDoesntExist,                     // The account you specified doesn't exist.
        //EnterGameGeneric,
        //EnterGameStressAccount,                 // You cannot entry the game with a stress creating character.
        //EnterGameCharacterInWorld,              // One of your characters is still in the world. Please try again in a few minutes.
        //EnterGamePlayerAccountMissing,          // Server unable to find player account. Please try again later.
        //EnterGameCharacterNotOwned,             // You do not own this character.
        //EnterGameCharacterInWorldServer,        // One of your characters is currently in the world. Please try again later. This is likely an internal server error.
        //EnterGameOldCharacter,
        //EnterGameCorruptCharacter,              // This character's data has been corrupted. Please delete it and create a new character.
        //EnterGameStartServerDown,               // This character's starting server is experiencing difficulties. Please try again in a few minutes.
        //EnterGameCouldntPlaceCharacter,         // This character couldn't be placed in the world right now. Please try again in a few minutes.
        //LogonServerFull,                        // Sorry, but the Asheron's Call server is full currently. Please try again later.
        //CharacterIsBooted,
        //EnterGameCharacterLocked,               // A save of this character is still in progress. Please try again later.
        //SubscriptionExpired                     // Your subscription to this game has expired.
    }
}
