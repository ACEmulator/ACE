using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;

using Biota = ACE.Database.Models.Shard.Biota;

namespace ACE.Server.WorldObjects
{
    public class Cow : Creature
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Cow(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Cow(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void ActOnUse(WorldObject activator)
        {
            // Cow class handled Cow Tipping action prior to Come What Follows patch.
            // in patch Come What Follows, cow class was either depreciated to be nothing, or updated to blanket proccess cow tipping quest addition to tipping action.

            // could an cow in combat be tipped?

            //if (activator is Player player)
            //    TipCow(player);
        }

        private static readonly List<MotionCommand> motionTippedRight = new List<MotionCommand> { MotionCommand.TippedRight };

        private void TipCow(Player player)
        {
            if (CreatureType == ACE.Entity.Enum.CreatureType.Auroch)
                player.SendMessage($"The {Name} ignores you.");
            else
            {
                Active = false;
                var actionChain = new ActionChain();
                EnqueueMotionAction(actionChain, motionTippedRight);
                actionChain.AddAction(this, () => HandleCowTipQuest(player));
                actionChain.AddAction(this, () => Active = true);
                actionChain.EnqueueChain();
            }
        }

        private void HandleCowTipQuest(Player player)
        {
            if (!player.QuestManager.HasQuest("OnCowTipQuest")) return;

            player.QuestManager.Stamp("CowTipCounter");

            var currentNumberOfCowTips = player.QuestManager.GetCurrentSolves("CowTipCounter");
            var thinkText = currentNumberOfCowTips switch
            {
                300 => "It seems to you like you have done enough tipping for now, maybe Dwennon in Holtburg has an update for you...",
                250 => "Unless you lost track someplace along the way, which you think is very possible at this point, you believe that's your 250th tip. What, no fireworks? Maybe you have a few more tips to go still.",
                200 => "Your arms are getting tired, you think to yourself \"I must have tipped at least 200 times by now..\"",
                150 => "You think to yourself \"I must be at least half done, and if not then Dwennon can just finish tipping this cow himself.\"",
                100 => "A thought pops into your head \"CowLogic\" and you think... \"What's that?\" Maybe you can ask Dwennon about it some day.",
                50 => "The cows lips do not move but you would almost swear you just heard \"Moooo, Mooo Mooooooo!\"",
                1 => "You think to yourself \"One tip down, I wonder how many more this guy could possibly expect...\"",
                _ => null,
            };
            if (!string.IsNullOrEmpty(thinkText))
                player.SendMessage(thinkText);
        }
    }
}
