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
            if (activator is Player player)
                TipCow(player);
        }

        private void TipCow(Player player)
        {
            if (CreatureType == ACE.Entity.Enum.CreatureType.Auroch)
                player.SendMessage($"The {Name} ignores you.");
            else
            {
                Active = false;
                var actionChain = new ActionChain();
                //EnqueueMotion(actionChain, MotionCommand.TippedRight);
                EnqueueMotionAction(actionChain, new List<MotionCommand> { MotionCommand.TippedRight });
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
            string thinkText;
            switch (currentNumberOfCowTips)
            {
                case 300:
                    thinkText = "It seems to you like you have done enough tipping for now, maybe Dwennon in Holtburg has an update for you...";
                    break;
                case 250:
                    thinkText = "Unless you lost track someplace along the way, which you think is very possible at this point, you believe that's your 250th tip. What, no fireworks? Maybe you have a few more tips to go still.";
                    break;
                case 200:
                    thinkText = "Your arms are getting tired, you think to yourself \"I must have tipped at least 200 times by now..\"";
                    break;
                case 150:
                    thinkText = "You think to yourself \"I must be at least half done, and if not then Dwennon can just finish tipping this cow himself.\"";
                    break;
                case 100:
                    thinkText = "A thought pops into your head \"CowLogic\" and you think... \"What's that?\" Maybe you can ask Dwennon about it some day.";
                    break;
                case 50:
                    thinkText = "The cows lips do not move but you would almost swear you just heard \"Moooo, Mooo Mooooooo!\"";
                    break;
                case 1:
                    thinkText = "You think to yourself \"One tip down, I wonder how many more this guy could possibly expect...\"";
                    break;
                default:
                    thinkText = null;
                    break;
            }

            if (!string.IsNullOrEmpty(thinkText))
                player.SendMessage(thinkText);
        }
    }
}
