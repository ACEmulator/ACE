
namespace ACE.Entity.Enum
{
    /// <summary>
    /// The StatusMessageType1 identifies the specific message to be displayed in the chat window.<para />
    /// Used with F7B0 028A: Game Event -> Display a status message in the chat window.
    /// </summary>
    public enum StatusMessageType1
    {
        /// <summary>
        /// You're too busy!
        /// </summary>
        YoureTooBusy                        = 0x001D,

        /// <summary>
        /// You are too fatigued to attack!
        /// </summary>
        YouAreTooFatiguedToAttack           = 0x03F7,

        /// <summary>
        /// You are out of ammunition!
        /// </summary>
        YouAreOutOfAmmunition               = 0x03F8,

        /// <summary>
        /// Your missile attack misfired!
        /// </summary>
        YourAttackMisfired                  = 0x03F9,

        /// <summary>
        /// You've attempted an impossible spell path!
        /// </summary>
        YouveAttemptedAnImpossibleSpellPath = 0x03FA,

        /// <summary>
        /// You don't know that spell!
        /// </summary>
        YouDontKnowThatSpell                = 0x03FE,

        /// <summary>
        /// Incorrect target type
        /// </summary>
        IncorrectTargetType                 = 0x03FF,

        /// <summary>
        /// You don't have all the components for this spell.
        /// </summary>
        YouDontHaveAllTheComponents         = 0x0400,

        /// <summary>
        /// You don't have enough Mana to cast this spell.
        /// </summary>
        YouDontHaveEnoughManaToCast         = 0x0401,

        /// <summary>
        /// Your spell fizzled.
        /// </summary>
        YourSpellFizzled                    = 0x0402,

        /// <summary>
        /// Your spell's target is missing!
        /// </summary>
        YourSpellsTargetIsMissing           = 0x0403,

        /// <summary>
        /// Your projectile spell mislaunched!
        /// </summary>
        YourProjectileSpellMislaunched      = 0x0404,

        /// <summary>
        /// You have solved this quest too recently!
        /// </summary>
        YouHaveSolvedThisQuestTooRecently   = 0x043E,

        /// <summary>
        /// You have solved this quest too many times!
        /// </summary>
        YouHaveSolvedThisQuestTooManyTimes  = 0x043F,

        /// <summary>
        /// You have entered your allegiance chat room.
        /// </summary>
        YouHaveEnteredYourAllegianceChat    = 0x051B,

        /// <summary>
        /// You have left an allegiance chat room.
        /// </summary>
        YouHaveLeftAnAllegianceChat         = 0x051C,

        /// <summary>
        /// Turbine Chat is enabled.
        /// </summary>
        TurbineChatIsEnabled                = 0x051D
    }
}
