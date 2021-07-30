using System;

using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects
{
    public class Key : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Key(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Key(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // These shoudl come from the weenie. After confirmation, remove these
            //KeyCode = AceObject.KeyCode ?? "";
            //Structure = AceObject.Structure ?? AceObject.MaxStructure;
        }

        public string KeyCode
        {
            get => GetProperty(PropertyString.KeyCode);
            set { if (value == null) RemoveProperty(PropertyString.KeyCode); else SetProperty(PropertyString.KeyCode, value); }
        }

        public bool OpensAnyLock
        {
            get => GetProperty(PropertyBool.OpensAnyLock) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.OpensAnyLock); else SetProperty(PropertyBool.OpensAnyLock, value); }
        }

        public override void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            // check activation requirements

            // verify player level
            if (UseRequiresLevel != null)
            {
                var playerLevel = player.Level ?? 1;
                if (playerLevel < UseRequiresLevel.Value)
                //return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouMustBe_ToUseItemMagic, $"level {UseRequiresLevel.Value}"));
                {
                    //player.SendWeenieError(ACE.Entity.Enum.WeenieError.LevelTooLow);
                    //player.SendUseDoneEvent(ACE.Entity.Enum.WeenieError.LevelTooLow);
                    //player.SendTransientError($"You are not high enough level to use {Name} on {target.Name}!"); // not retail message almost certainly, but unable to locate specific instance in pcaps of using a key on chest below the level required at this time
                    player.SendTransientError("You are not high enough level to use that!");
                    player.SendUseDoneEvent();
                    return;
                }
            }

            UnlockerHelper.UseUnlocker(player, this, target);
        }
    }
}
