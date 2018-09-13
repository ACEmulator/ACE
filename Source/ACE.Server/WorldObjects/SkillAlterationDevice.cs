using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using System;

namespace ACE.Server.WorldObjects
{
    public class SkillAlterationDevice : WorldObject
    {
        public SkillAlterationType TypeOfAlteration { get; set; }
        public Skill SkillToBeAltered { get; set; }

        public enum SkillAlterationType
        {
            Undef = 0,
            Specialize = 1,
            Lower = 2,
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SkillAlterationDevice(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SkillAlterationDevice(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            //Copy values to properties
            TypeOfAlteration = (SkillAlterationType)(GetProperty(PropertyInt.TypeOfAlteration) ?? 1);
            SkillToBeAltered = (Skill)(GetProperty(PropertyInt.SkillToBeAltered) ?? 0);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// </summary>
        public override void UseItem(Player player)
        {
            Console.WriteLine("In SkillAlterationDevice.UseItem()");

            switch (TypeOfAlteration)
            {
                case SkillAlterationType.Specialize:
                    var currentSkill = player.GetCreatureSkill(SkillToBeAltered);
                    if (currentSkill.AdvancementClass == SkillAdvancementClass.Specialized)
                    {

                    }
                    break;
                case SkillAlterationType.Lower:

                    break;
                default:
#if DEBUG
                    Console.WriteLine("Undefined or unspecified TypeOfAlteration reached");
#endif

                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouFailToAlterSkill));
                    break;
            }

        }
    }
}
