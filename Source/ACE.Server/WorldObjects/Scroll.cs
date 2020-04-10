using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Scroll : WorldObject
    {
        public Server.Entity.Spell Spell;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Scroll(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Scroll(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            if (SpellDID != null)
                Spell = new Server.Entity.Spell(SpellDID.Value, false);

            if (Spell != null)
                LongDesc = $"Inscribed spell: {Spell.Name}\n{Spell.Description}";
            Use = "Use this item to attempt to learn its spell.";
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// </summary>
        public override void ActOnUse(WorldObject activator)
        {
            // Research: http://asheron.wikia.com/wiki/Announcements_-_2002/06_-_Castling

            if (!(activator is Player player))
                return;

            if (Spell == null)
            {
                Console.WriteLine($"{Name}.ActOnUse({activator.Name}) - SpellDID not found for {WeenieClassId}");
                return;
            }

            if (player.IsBusy || player.Teleporting || player.suicideInProgress)
            {
                player.SendWeenieError(WeenieError.YoureTooBusy);
                return;
            }

            player.IsBusy = true;

            var actionChain = new ActionChain();

            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);

                player.LastUseTime += stanceTime;
            }

            var animTime = player.EnqueueMotion(actionChain, MotionCommand.Reading);
            player.LastUseTime += animTime;

            var readTime = 1.0f;

            actionChain.AddDelaySeconds(readTime);
            player.LastUseTime += readTime;

            actionChain.AddAction(player, () =>
            {
                if (player.SpellIsKnown(Spell.Id))
                {
                    // verify unknown spell
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("You already know that spell!", ChatMessageType.Broadcast));
                    return;
                }

                var skill = Spell.GetMagicSkill();
                var playerSkill = player.GetCreatureSkill(skill);

                if (!player.CanReadScroll(this))
                {
                    var msg = "";
                    if (playerSkill.AdvancementClass < SkillAdvancementClass.Trained)
                        msg = $"You are not trained in {playerSkill.Skill.ToSentence()}!";
                    else
                        msg = $"You are not skilled enough in {playerSkill.Skill.ToSentence()} to learn this spell.";

                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                    return;
                }

                player.LearnSpellWithNetworking(Spell.Id);

                player.TryConsumeFromInventoryWithNetworking(this);

                player.Session.Network.EnqueueSend(new GameMessageSystemChat("The scroll is destroyed.", ChatMessageType.Broadcast));
            });


            // FIXME: return stance time
            player.EnqueueMotion(actionChain, MotionCommand.Ready);

            player.LastUseTime += animTime;     // return stance

            actionChain.AddDelaySeconds(animTime);

            actionChain.AddAction(player, () => player.IsBusy = false);

            actionChain.EnqueueChain();
        }
    }
}
