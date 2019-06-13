using System;
using System.Security.Cryptography;
using System.Text;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public abstract class Confirmation
    {
        public ConfirmationType ConfirmationType;

        public uint ConfirmationID;

        public virtual void ProcessConfirmation(bool response) { }

        protected uint GenerateContextId()
        {
            // this seems to be a much smaller # in retail... the highest i saw was between ~400-500 in a brief search
            // these #s also seem to always increase, not sure if these are just sequence #s in a particular context?
            // sending a context id for a lower # might have the client reject the message...

            char[] chars = new char[] {'1','2','3','4','5','6','7','8','9','0' };
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[9];
            crypto.GetNonZeroBytes(data);
            StringBuilder sb = new StringBuilder(9);
            foreach (byte b in data)
            {
                sb.Append(chars[b % (chars.Length)]);
            }
            return Convert.ToUInt32(sb.ToString());
        }
    }

    public class Confirmation_AlterAttribute: Confirmation
    {
        public ObjectGuid PlayerGuid;
        public ObjectGuid AttributeTransferDevice;

        public Confirmation_AlterAttribute(ObjectGuid playerGuid, ObjectGuid attributeTransferDevice)
        {
            ConfirmationType = ConfirmationType.AlterAttribute;

            PlayerGuid = playerGuid;
            AttributeTransferDevice = attributeTransferDevice;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = PlayerManager.GetOnlinePlayer(PlayerGuid);
            if (player == null)
                return;

            player.CompleteConfirmation(ConfirmationType, ConfirmationID);

            if (!response)
                return;

            var attributeTransferDevice = player.FindObject(AttributeTransferDevice.Full, Player.SearchLocations.MyInventory) as AttributeTransferDevice;
            if (attributeTransferDevice == null)
                return;

            attributeTransferDevice.ActOnUse(player, true);
        }
    }

    public class Confirmation_AlterSkill : Confirmation
    {
        public ObjectGuid PlayerGuid;
        public ObjectGuid SkillAlterationDevice;

        public Confirmation_AlterSkill(ObjectGuid playerGuid, ObjectGuid skillAlterationDevice)
        {
            ConfirmationType = ConfirmationType.AlterSkill;

            PlayerGuid = playerGuid;
            SkillAlterationDevice = skillAlterationDevice;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = PlayerManager.GetOnlinePlayer(PlayerGuid);
            if (player == null)
                return;

            player.CompleteConfirmation(ConfirmationType, ConfirmationID);

            if (!response)
                return;

            var skillAlterationDevice = player.FindObject(SkillAlterationDevice.Full, Player.SearchLocations.MyInventory) as SkillAlterationDevice;
            if (skillAlterationDevice == null)
                return;

            skillAlterationDevice.ActOnUse(player, true);
        }
    }

    public class Confirmation_Augmentation: Confirmation
    {
        public ObjectGuid PlayerGuid;
        public ObjectGuid AugmentationGuid;

        public Confirmation_Augmentation(ObjectGuid playerGuid, ObjectGuid augmentationGuid)
        {
            ConfirmationType = ConfirmationType.Augmentation;

            PlayerGuid = playerGuid;
            AugmentationGuid = augmentationGuid;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = PlayerManager.GetOnlinePlayer(PlayerGuid);
            if (player == null)
                return;

            player.CompleteConfirmation(ConfirmationType, ConfirmationID);

            if (!response)
                return;

            var augmentation = player.FindObject(AugmentationGuid.Full, Player.SearchLocations.MyInventory) as AugmentationDevice;
            if (augmentation == null)
                return;

            augmentation.DoAugmentation(player);
        }
    }

    public class Confirmation_CraftInteration: Confirmation
    {
        public ObjectGuid PlayerGuid;

        public ObjectGuid SourceGuid;
        public ObjectGuid TargetGuid;

        public Confirmation_CraftInteration(ObjectGuid playerGuid, ObjectGuid sourceGuid, ObjectGuid targetGuid)
        {
            ConfirmationType = ConfirmationType.CraftInteraction;

            PlayerGuid = playerGuid;

            SourceGuid = sourceGuid;
            TargetGuid = targetGuid;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = PlayerManager.GetOnlinePlayer(PlayerGuid);
            if (player == null)
                return;

            player.CompleteConfirmation(ConfirmationType, ConfirmationID);

            if (!response)
                return;

            var source = player.FindObject(SourceGuid.Full, Player.SearchLocations.MyInventory | Player.SearchLocations.MyEquippedItems);
            var target = player.FindObject(TargetGuid.Full, Player.SearchLocations.MyInventory | Player.SearchLocations.MyEquippedItems);

            if (source == null || target == null)
                return;

            RecipeManager.HandleTinkering(player, source, target, true);
        }
    }

    public class Confirmation_Fellowship : Confirmation
    {
        public ObjectGuid InviterGuid;
        public ObjectGuid InvitedGuid;

        public Confirmation_Fellowship(ObjectGuid inviterGuid, ObjectGuid invitedGuid)
        {
            ConfirmationType = ConfirmationType.Fellowship;

            InviterGuid = inviterGuid;
            InvitedGuid = invitedGuid;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var inviter = PlayerManager.GetOnlinePlayer(InviterGuid);
            var invited = PlayerManager.GetOnlinePlayer(InvitedGuid);

            if (inviter == null)
                return;

            inviter.CompleteConfirmation(ConfirmationType, ConfirmationID);

            if (response && inviter.Fellowship != null)
                inviter.Fellowship.AddConfirmedMember(inviter, invited, response);
        }
    }

    public class Confirmation_SwearAllegiance : Confirmation
    {
        public ObjectGuid PatronGuid;
        public ObjectGuid VassalGuid;

        public Confirmation_SwearAllegiance(ObjectGuid patronGuid, ObjectGuid vassalGuid)
        {
            ConfirmationType = ConfirmationType.SwearAllegiance;

            PatronGuid = patronGuid;
            VassalGuid = vassalGuid;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var patron = PlayerManager.GetOnlinePlayer(PatronGuid);
            var vassal = PlayerManager.GetOnlinePlayer(VassalGuid);

            if (vassal == null)
                return;

            vassal.CompleteConfirmation(ConfirmationType, ConfirmationID);

            if (response && patron != null)
                vassal.HandleActionSwearAllegiance(patron.Guid.Full, true);
        }
    }

    public class Confirmation_YesNo: Confirmation
    {
        public ObjectGuid SourceGuid;
        public ObjectGuid TargetPlayerGuid;

        public string Quest;

        public Confirmation_YesNo(ObjectGuid sourceGuid, ObjectGuid targetPlayerGuid, string quest)
        {
            ConfirmationType = ConfirmationType.Yes_No;

            SourceGuid = sourceGuid;
            TargetPlayerGuid = targetPlayerGuid;

            Quest = quest;

            GenerateContextId();
        }

        public override void ProcessConfirmation(bool response)
        {
            var player = PlayerManager.GetOnlinePlayer(TargetPlayerGuid);

            if (player == null)
                return;

            player.CompleteConfirmation(ConfirmationType, ConfirmationID);

            var source = player.FindObject(SourceGuid.Full, Player.SearchLocations.Landblock);
            if (source == null)
                return;

            source.EmoteManager.ExecuteEmoteSet(response ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, Quest, player);
        }
    }
}
