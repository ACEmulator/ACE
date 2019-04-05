using System;
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Entity
{
    public class Proficiency
    {
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
                var cp = (uint)Math.Round(pp * 0.1f);   // cp = 10% PP

                //Console.WriteLine($"Earned {pp} PP ({skill.Skill})");

                // send PP to player as skill XP
                player.RaiseSkillGameAction(skill.Skill, pp, true);

                // send CP to player as unassigned XP
                player.GrantXP(cp, XpType.Proficiency, false);
            }
        }
    }
}
