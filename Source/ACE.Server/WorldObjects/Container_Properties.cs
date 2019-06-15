
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

        /// <summary>
        /// This is the default setup for resetting chests
        /// </summary>
        public double ContainerResetInterval
        {
            get
            {
                var containerResetInterval = ResetInterval ?? Default_ContainerResetInterval;

                if (containerResetInterval < 15)
                    containerResetInterval = Default_ContainerResetInterval;

                return containerResetInterval;
            }
        }

        public double Default_ContainerResetInterval = 120;

        public bool ResetMessagePending
        {
            get => GetProperty(PropertyBool.ResetMessagePending) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.ResetMessagePending); else SetProperty(PropertyBool.ResetMessagePending, value); }
        }
    }
}
