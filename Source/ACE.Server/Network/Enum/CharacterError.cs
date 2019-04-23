namespace ACE.Server.Network.Enum
{
    /// <summary>
    /// CharError enum in client<para />
    /// Authoritative list is from https://github.com/Zegeger/ACE.Network/blob/master/NetworkStack/Enums/CharacterErrorType.cs
    /// </summary>
    public enum CharacterError
    {
        /// <summary>
        /// ID_CHAR_ERROR_LOGON<para />
        /// Cannot have two accounts logged on at the same time.
        /// </summary>
        Logon = 0x00000001,

        /// <summary>
        /// ID_CHAR_ERROR_ACCOUNT_LOGON<para />
        /// Server could not access your account information. Please try again in a few minutes.
        /// </summary>
        AccountLogin = 0x00000003,

        /// <summary>
        /// ID_CHAR_ERROR_SERVER_CRASH<para />
        /// The server has disconnected. Please try again in a few minutes.
        /// </summary>
        ServerCrash1 = 0x00000004,

        /// <summary>
        /// ID_CHAR_ERROR_LOGOFF<para />
        /// Server could not log off your character
        /// </summary>
        Logoff = 0x00000005,

        /// <summary>
        /// ID_CHAR_ERROR_DELETE<para />
        /// Server could not delete your character.
        /// </summary>
        Delete = 0x00000006,

        /// <summary>
        /// ID_CHAR_ERROR_SERVER_CRASH<para />
        /// The server has disconnected. Please try again in a few minutes.
        /// </summary>
        ServerCrash2 = 0x00000008,

        /// <summary>
        /// ID_CHAR_ERROR_ACCOUNT_INVALID<para />
        /// The account name you specified was not valid.
        /// </summary>
        AccountInvalid = 0x00000009,

        /// <summary>
        /// ID_CHAR_ERROR_ACCOUNT_DOESNT_EXIST<para />
        /// The account you specified doesn't exist.
        /// </summary>
        AccountDoesntExist = 0x0000000A,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_GENERIC<para />
        /// causes a popup saying: ID_CHAR_ERROR_ENTER_GAME_OLD_CHARACTER with an OK button that does nothing more than dismiss the popup when clicked.  If player is in 3D mode it also forces the player back to the character select screen.<para />
        /// Confirmed that both the "official version" and the "latest version" both behave the same.
        /// </summary>
        EnterGameGeneric = 0x0000000B,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_STRESS_ACCOUNT<para />
        /// You cannot entry the game with a stress creating character.
        /// </summary>
        EnterGameStressAccount = 0x0000000C,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_IN_WORLD<para />
        /// One of your characters is still in the world. Please try again in a few minutes.
        /// </summary>
        EnterGameCharacterInWorld = 0x0000000D,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_PLAYER_ACCOUNT_MISSING<para />
        /// Server unable to find player account. Please try again later.
        /// </summary>
        EnterGamePlayerAccountMissing = 0x0000000E,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_NOT_OWNED<para />
        /// You do not own this character.
        /// </summary>
        EnterGameCharacterNotOwned = 0x0000000F,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_IN_WORLD_SERVER<para />
        /// One of your characters is currently in the world. Please try again later. This is likely an internal server error.
        /// </summary>
        EnterGameCharacterInWorldServer = 0x00000010,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_OLD_CHARACTER<para />
        /// Force the player back to the character select screen if they are in 3D mode and it do nothing if the player is already at the character select screen.
        /// </summary>
        EnterGameOldCharacter = 0x00000011,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CORRUPT_CHARACTER<para />
        /// This character's data has been corrupted. Please delete it and create a new character.
        /// </summary>
        EnterGameCorruptCharacter = 0x00000012,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_START_SERVER_DOWN<para />
        /// This character's starting server is experiencing difficulties. Please try again in a few minutes.
        /// </summary>
        EnterGameStartServerDown = 0x00000013,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_COULDNT_PLACE_CHARACTER<para />
        /// This character couldn't be placed in the world right now. Please try again in a few minutes.
        /// </summary>
        EnterGameCouldntPlaceCharacter = 0x00000014,

        /// <summary>
        /// ID_CHAR_ERROR_LOGON_SERVER_FULL<para />
        /// Sorry, but the Asheron's Call server is full currently. Please try again later.
        /// </summary>
        LogonServerFull = 0x00000015,

        /// <summary>
        /// ID_CHAR_ERROR_ENTER_GAME_CHARACTER_LOCKED<para />
        /// A save of this character is still in progress. Please try again later.
        /// </summary>
        EnterGameCharacterLocked = 0x00000017,

        /// <summary>
        /// ID_CHAR_ERROR_SUBSCRIPTION_EXPIRED<para />
        /// Your subscription to this game has expired.
        /// </summary>
        SubscriptionExpired = 0x00000018
    }
}
