using System;
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;
using log4net;

namespace ACE.Server.Entity
{
    public class Proficiency
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static TimeSpan FullTime = TimeSpan.FromMinutes(15);

        public static void OnSuccessUse(Player player, CreatureSkill skill, uint difficulty)
        {
            //Console.WriteLine($"Proficiency.OnSuccessUse({player.Name}, {skill.Skill}, targetDiff: {difficulty})");

            // TODO: this formula still probably needs some work to match up with retail truly...

            // possible todo: does this only apply to players?
            // ie., can monsters still level up from skill usage, or killing players?
            // it was possible on release, but i think they might have removed that feature?

            // ensure skill is at least trained
            if (skill.AdvancementClass < SkillAdvancementClass.Trained)
                return;

            var last_difficulty = skill.BiotaPropertiesSkill.ResistanceAtLastCheck;
            var last_used_time = skill.BiotaPropertiesSkill.LastUsedTime;

            var currentTime = Time.GetUnixTime();

            var timeDiff = currentTime - last_used_time;

            var difficulty_check = difficulty > last_difficulty;
            var time_check = timeDiff >= FullTime.TotalSeconds;

            if (difficulty_check || time_check)
            {
                // todo: not independent variables?
                // always scale if timeDiff < FullTime?
                var timeScale = 1.0f;
                if (!time_check)
                {
                    // 10 mins elapsed from 15 min FullTime:
                    // 0.66f timeScale
                    timeScale = (float)(timeDiff / FullTime.TotalSeconds);

                    // any rng involved?
                }

                skill.BiotaPropertiesSkill.ResistanceAtLastCheck = difficulty;
                skill.BiotaPropertiesSkill.LastUsedTime = currentTime;

                player.ChangesDetected = true;

                var pp = (uint)Math.Round(difficulty * timeScale);
                var totalXPGranted = (uint)Math.Round(pp * 1.1f);   // give additional 10% of proficiency XP to unassigned XP

                if (totalXPGranted > 10000)
                {
                    log.Warn($"Proficiency.OnSuccessUse({player.Name}, {skill.Skill}, {difficulty})");
                }

                //Console.WriteLine($"Earned {pp} PP ({skill.Skill})");

                // send CP to player as unassigned XP
                player.GrantXP(totalXPGranted, XpType.Proficiency, ShareType.None);

                // send PP to player as skill XP, which gets spent from the CP sent
                player.RaiseSkillGameAction(skill.Skill, pp);
            }
        }

        public static void OnSuccessUse(Player player, CreatureSkill skill, int difficulty)
        {
            if (difficulty < 0)
            {
                log.Error($"Proficiency.OnSuccessUse({player.Name}, {skill.Skill}, {difficulty}) - difficulty cannot be negative");
                return;
            }
            OnSuccessUse(player, skill, (uint)difficulty);
        }
    }
}
