
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public uint Viewer
        {
            get => GetProperty(PropertyInstanceId.Viewer) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInstanceId.Viewer); else SetProperty(PropertyInstanceId.Viewer, value); }
        }

        public uint? LastUnlocker
        {
            get => GetProperty(PropertyInstanceId.LastUnlocker);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.LastUnlocker); else SetProperty(PropertyInstanceId.LastUnlocker, value.Value); }
        }

        public double? UseLockTimestamp
        {
            get => GetProperty(PropertyFloat.UseLockTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.UseLockTimestamp); else SetProperty(PropertyFloat.UseLockTimestamp, value.Value); }
        }

        public bool ResetMessagePending
        {
            get => GetProperty(PropertyBool.ResetMessagePending) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.ResetMessagePending); else SetProperty(PropertyBool.ResetMessagePending, value); }
        }
    }
}
