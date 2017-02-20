
namespace ACE.Entity.Enum
{
    /// <summary>
    /// The StatusMessageType2 identifies the specific message to be displayed in the chat window.<para />
    /// Used with F7B0 028B: Game Event -> Display a parameterized status message in the chat window.
    /// </summary>
    public enum StatusMessageType2
    {
        /// <summary>
        /// &lt;text&gt; is too busy to accept gifts right now.
        /// </summary>
        TargetIsTooBusyToAcceptGifts        = 0x001E,

        /// <summary>
        /// &lt;text&gt; cannot carry anymore.
        /// </summary>
        TargetCannotCarryAnymore            = 0x002B,

        /// <summary>
        /// "&lt;text&gt; is not accepting gifts right now.
        /// </summary>
        TargetIsNotAcceptingGifts           = 0x03EF,

        /// <summary>
        /// &lt;text&gt; doesn't know what to do with that.
        /// </summary>
        TargetDoesNotKnowWhatToDoWithThat   = 0x046A,

        /// <summary>
        /// You have succeeded in specializing your &lt;text&gt; skill!
        /// </summary>
        YouHaveSucceededInSpecializing      = 0x04D6,

        /// <summary>
        /// You have succeeded in lowering your &lt;text&gt; skill from specialized to trained!
        /// </summary>
        YouHaveSucceededInLowering          = 0x04D7,

        /// <summary>
        /// You have succeeded in untraining your &lt;text&gt; skill!
        /// </summary>
        YouHaveSucceededInUntraining        = 0x04D8,

        /// <summary>
        /// Although you cannot untrain your &lt;text&gt; skill, you have succeeded in recovering all the experience you had invested in it.
        /// </summary>
        YouHaveSucceededInRecoveringXP      = 0x04D9
    }
}
