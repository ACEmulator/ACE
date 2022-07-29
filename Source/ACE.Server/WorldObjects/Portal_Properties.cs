using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Portal
    {
        public PortalBitmask PortalRestrictions
        {
            get => (PortalBitmask)(GetProperty(PropertyInt.PortalBitmask) ?? (int)PortalBitmask.Unrestricted);
            set { if (value == PortalBitmask.Undef) RemoveProperty(PropertyInt.PortalBitmask); else SetProperty(PropertyInt.PortalBitmask, (int)value); }
        }

        public bool NoRecall => (PortalRestrictions & PortalBitmask.NoRecall) != 0;

        public bool NoTie => NoRecall;

        public bool NoSummon => (PortalRestrictions & PortalBitmask.NoSummon) != 0;

        /// <summary>
        /// For summoned portals, the DID of the original portal
        /// </summary>
        public uint? OriginalPortal
        {
            get => GetProperty(PropertyDataId.OriginalPortal);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.OriginalPortal); else SetProperty(PropertyDataId.OriginalPortal, value.Value); }
        }

        public bool? PortalShowDestination
        {
            get => GetProperty(PropertyBool.PortalShowDestination);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.PortalShowDestination); else SetProperty(PropertyBool.PortalShowDestination, value.Value); }
        }

        public string AppraisalPortalDestination
        {
            get => GetProperty(PropertyString.AppraisalPortalDestination);
            set { if (value == null) RemoveProperty(PropertyString.AppraisalPortalDestination); else SetProperty(PropertyString.AppraisalPortalDestination, value); }
        }

        public int SocietyId => 0;

        public bool PortalIgnoresPkAttackTimer
        {
            get => GetProperty(PropertyBool.PortalIgnoresPkAttackTimer) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.PortalIgnoresPkAttackTimer); else SetProperty(PropertyBool.PortalIgnoresPkAttackTimer, value); }
        }

        public SubscriptionStatus? AccountRequirements
        {
            get => (SubscriptionStatus?)GetProperty(PropertyInt.AccountRequirements);
            set { if (value == null) RemoveProperty(PropertyInt.AccountRequirements); else SetProperty(PropertyInt.AccountRequirements, (int)value); }
        }

        public bool? AdvocateQuest
        {
            get => GetProperty(PropertyBool.AdvocateQuest);
            set { if (value == null) RemoveProperty(PropertyBool.AdvocateQuest); else SetProperty(PropertyBool.AdvocateQuest, value.Value); }
        }
    }
}
