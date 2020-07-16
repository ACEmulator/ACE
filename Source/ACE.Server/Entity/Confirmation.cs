using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public abstract class Confirmation
    {
        public ObjectGuid PlayerGuid;

        public ConfirmationType ConfirmationType;

        public Confirmation(ObjectGuid playerGuid, ConfirmationType confirmationType)
        {
            PlayerGuid = playerGuid;

            ConfirmationType = confirmationType;
        }

        public virtual void ProcessConfirmation(bool response)
        {
            // empty base
        }

        public Player GetPlayerResponse(bool response)
        {
            return response ? PlayerManager.GetOnlinePlayer(PlayerGuid) : null;
        }
    }

    public class Confirmation_AlterAttribute: Confirmation
    {
        public ObjectGuid AttributeTransferDevice;

        public Confirmation_AlterAttribute(ObjectGuid playerGuid, ObjectGuid attributeTransferDevice)
            : base(playerGuid, ConfirmationType.AlterAttribute)
        {
            AttributeTransferDevice = attributeTransferDevice;
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = GetPlayerResponse(response);
            if (player == null) return;

            var attributeTransferDevice = player.FindObject(AttributeTransferDevice.Full, Player.SearchLocations.MyInventory) as AttributeTransferDevice;

            if (attributeTransferDevice != null)
                attributeTransferDevice.ActOnUse(player, true);
        }
    }

    public class Confirmation_AlterSkill : Confirmation
    {
        public ObjectGuid SkillAlterationDevice;

        public Confirmation_AlterSkill(ObjectGuid playerGuid, ObjectGuid skillAlterationDevice)
            : base(playerGuid, ConfirmationType.AlterSkill)
        {
            SkillAlterationDevice = skillAlterationDevice;
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = GetPlayerResponse(response);
            if (player == null) return;

            var skillAlterationDevice = player.FindObject(SkillAlterationDevice.Full, Player.SearchLocations.MyInventory) as SkillAlterationDevice;

            if (skillAlterationDevice != null)
                skillAlterationDevice.ActOnUse(player, true);
        }
    }

    public class Confirmation_Augmentation: Confirmation
    {
        public ObjectGuid AugmentationGuid;

        public Confirmation_Augmentation(ObjectGuid playerGuid, ObjectGuid augmentationGuid)
            : base(playerGuid, ConfirmationType.Augmentation)
        {
            AugmentationGuid = augmentationGuid;
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = GetPlayerResponse(response);
            if (player == null) return;

            var augmentation = player.FindObject(AugmentationGuid.Full, Player.SearchLocations.MyInventory) as AugmentationDevice;

            if (augmentation != null)
                augmentation.ActOnUse(player, true);
        }
    }

    public class Confirmation_CraftInteration: Confirmation
    {
        public ObjectGuid SourceGuid;
        public ObjectGuid TargetGuid;

        public bool Tinkering;

        public Confirmation_CraftInteration(ObjectGuid playerGuid, ObjectGuid sourceGuid, ObjectGuid targetGuid, bool tinkering = false)
            : base (playerGuid, ConfirmationType.CraftInteraction)
        {
            SourceGuid = sourceGuid;
            TargetGuid = targetGuid;

            Tinkering = tinkering;
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = GetPlayerResponse(response);
            if (player == null) return;

            var source = player.FindObject(SourceGuid.Full, Player.SearchLocations.LocationsICanMove);
            var target = player.FindObject(TargetGuid.Full, Player.SearchLocations.LocationsICanMove);

            if (source != null && target != null)
            {
                if (!Tinkering)
                    RecipeManager.UseObjectOnTarget(player, source, target, true);
                else
                    RecipeManager.HandleTinkering(player, source, target, true);
            }
        }
    }

    public class Confirmation_Fellowship : Confirmation
    {
        public ObjectGuid InviterGuid;

        public Confirmation_Fellowship(ObjectGuid inviterGuid, ObjectGuid invitedGuid)
            : base(invitedGuid, ConfirmationType.Fellowship)
        {
            InviterGuid = inviterGuid;
        }

        public override void ProcessConfirmation(bool response)
        {
            var invited = PlayerManager.GetOnlinePlayer(PlayerGuid);
            var inviter = PlayerManager.GetOnlinePlayer(InviterGuid);

            if (invited != null && inviter != null && inviter.Fellowship != null)
                inviter.Fellowship.AddConfirmedMember(inviter, invited, response);
        }
    }

    public class Confirmation_SwearAllegiance : Confirmation
    {
        public ObjectGuid VassalGuid;

        public Confirmation_SwearAllegiance(ObjectGuid patronGuid, ObjectGuid vassalGuid)
            : base(patronGuid, ConfirmationType.SwearAllegiance)
        {
            VassalGuid = vassalGuid;
        }

        public override void ProcessConfirmation(bool response)
        {
            var patron = GetPlayerResponse(response);
            if (patron == null) return;

            var vassal = PlayerManager.GetOnlinePlayer(VassalGuid);

            if (vassal != null)
                vassal.SwearAllegiance(patron.Guid.Full, true, true);
        }
    }

    public class Confirmation_YesNo: Confirmation
    {
        public ObjectGuid SourceGuid;

        public string Quest;

        public Confirmation_YesNo(ObjectGuid sourceGuid, ObjectGuid targetPlayerGuid, string quest)
            : base(targetPlayerGuid, ConfirmationType.Yes_No)
        {
            SourceGuid = sourceGuid;
            Quest = quest;
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = PlayerManager.GetOnlinePlayer(PlayerGuid);
            if (player == null) return;

            var source = player.FindObject(SourceGuid.Full, Player.SearchLocations.Landblock);

            if (source != null)
                source.EmoteManager.ExecuteEmoteSet(response ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, Quest, player);
        }
    }

    public class Confirmation_Custom: Confirmation
    {
        public Action Action;

        public Confirmation_Custom(ObjectGuid playerGuid, Action action)
            : base(playerGuid, ConfirmationType.Yes_No)
        {
            Action = action;
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = GetPlayerResponse(response);
            if (player == null) return;

            Action();
        }
    }
}
