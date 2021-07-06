using System;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Fast chugging state variables
    /// </summary>
    public class FoodState
    {
        /// <summary>
        /// A reference to the Player for this FoodState
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// This is set to true when a FastTick player is consuming food / drink
        /// and allow_fast_chug = true
        /// </summary>
        public bool IsChugging { get; set; }

        /// <summary>
        /// The eat/drink motion to wait for AnimationDone
        /// </summary>
        public MotionCommand UseMotion { get; set; }

        /// <summary>
        /// The action to perform when UseMotion has completed
        /// </summary>
        public Action Callback { get; set; }

        /// <summary>
        /// Passing this along for some weird gem animations
        /// </summary>
        public float AnimMod { get; set; }

        /// <summary>
        /// Similar requirements as previous var
        /// </summary>
        public float UseAnimTime { get; set; }

        /// <summary>
        /// For returning to combat state after consuming
        /// </summary>
        public MotionStance PrevStance { get; set; }

        public FoodState(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Called when a player starts performing the motion to apply a consumable
        /// </summary>
        public void StartChugging(MotionCommand useMotion, Action callback, float animMod, float useAnimTime, MotionStance prevStance)
        {
            IsChugging = true;

            UseMotion = useMotion;

            Callback = callback;

            AnimMod = animMod;

            UseAnimTime = useAnimTime;

            PrevStance = prevStance;
        }

        /// <summary>
        /// Called when a player completes the UseMotion
        /// </summary>
        public void FinishChugging()
        {
            IsChugging = false;

            UseMotion = MotionCommand.Invalid;

            Callback = null;

            AnimMod = 1.0f;

            UseAnimTime = 0.0f;

            PrevStance = MotionStance.Invalid;
        }
    }
}
