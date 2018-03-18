
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool IsAdmin
        {
            get => GetProperty(PropertyBool.IsAdmin) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAdmin); else SetProperty(PropertyBool.IsAdmin, value); }
        }

        public bool IsSentinel
        {
            get => GetProperty(PropertyBool.IsSentinel) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsSentinel); else SetProperty(PropertyBool.IsSentinel, value); }
        }

        public bool IsEnvoy
        {
            get => GetProperty(PropertyBool.IsSentinel) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsSentinel); else SetProperty(PropertyBool.IsSentinel, value); }
        }

        public bool IsArch
        {
            get => GetProperty(PropertyBool.IsArch) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsArch); else SetProperty(PropertyBool.IsArch, value); }
        }

        public bool IsPsr
        {
            get => GetProperty(PropertyBool.IsPsr) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsPsr); else SetProperty(PropertyBool.IsPsr, value); }
        }

        public bool IsAdvocate
        {
            get => GetProperty(PropertyBool.IsAdvocate) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAdvocate); else SetProperty(PropertyBool.IsAdvocate, value); }
        }

        public bool Account15Days
        {
            get => GetProperty(PropertyBool.Account15Days) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Account15Days); else SetProperty(PropertyBool.Account15Days, value); }
        }

        public bool? AdvocateQuest
        {
            get => GetProperty(PropertyBool.AdvocateQuest);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.AdvocateQuest); else SetProperty(PropertyBool.AdvocateQuest, value.Value); }
        }

        public bool? AdvocateState
        {
            get => GetProperty(PropertyBool.AdvocateState);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.AdvocateState); else SetProperty(PropertyBool.AdvocateState, value.Value); }
        }

        public int? AdvocateLevel
        {
            get => GetProperty(PropertyInt.AdvocateLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AdvocateLevel); else SetProperty(PropertyInt.AdvocateLevel, value.Value); }
        }

        public Channel? ChannelsActive
        {
            get => (Channel?)GetProperty(PropertyInt.ChannelsActive);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChannelsActive); else SetProperty(PropertyInt.ChannelsActive, (int)value.Value); }
        }

        public Channel? ChannelsAllowed
        {
            get => (Channel?)GetProperty(PropertyInt.ChannelsAllowed);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChannelsAllowed); else SetProperty(PropertyInt.ChannelsAllowed, (int)value.Value); }
        }

        /*private int coinValue;
        public override int? CoinValue
        {
            get => coinValue;
            set
            {
                if (value != coinValue)
                {
                    base.CoinValue = value;
                    coinValue = (int)value;
                    if (FirstEnterWorldDone) // We want to get rid of this. Updating a property shouldn't fire a network event
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.CoinValue, coinValue));
                }
            }
        }*/
    }
}
