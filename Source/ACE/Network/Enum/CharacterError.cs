
namespace ACE.Network.Enum
{
    // CharError enum in client
    public enum CharacterError
    {
        Undefined,
        Logon,                                  // Cannot have two accounts logged on at the same time.
        LoggedOn,                               // Server could not access your account information. Please try again in a few minutes.
        AccountLogon,
        ServerCrash,                            // The server has disconnected. Please try again in a few minutes.
        Logoff,                                 // Server could not log off your character
        Delete,                                 // Server could not delete your character.
        NoPremade,
        AccountInUse,                           // The server has disconnected. Please try again in a few minutes.
        AccountInvalid,                         // The account name you specified was not valid.
        AccountDoesntExist,                     // The account you specified doesn't exist.
        EnterGameGeneric,
        EnterGameStressAccount,                 // You cannot entry the game with a stress creating character.
        EnterGameCharacterInWorld,              // One of your characters is still in the world. Please try again in a few minutes.
        EnterGamePlayerAccountMissing,          // Server unable to find player account. Please try again later.
        EnterGameCharacterNotOwned,             // You do not own this character.
        EnterGameCharacterInWorldServer,        // One of your characters is currently in the world. Please try again later. This is likely an internal server error.
        EnterGameOldCharacter,
        EnterGameCorruptCharacter,              // This character's data has been corrupted. Please delete it and create a new character.
        EnterGameStartServerDown,               // This character's starting server is experiencing difficulties. Please try again in a few minutes.
        EnterGameCouldntPlaceCharacter,         // This character couldn't be placed in the world right now. Please try again in a few minutes.
        LogonServerFull,                        // Sorry, but the Asheron's Call server is full currently. Please try again later.
        CharacterIsBooted,
        EnterGameCharacterLocked,               // A save of this character is still in progress. Please try again later.
        SubscriptionExpired                     // Your subscription to this game has expired.
    }
}
