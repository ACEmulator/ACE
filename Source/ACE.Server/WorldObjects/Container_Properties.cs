
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public uint? Viewer
        {
            get => GetProperty(PropertyInstanceId.Viewer);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Viewer); else SetProperty(PropertyInstanceId.Viewer, value.Value); }
        }
    }
}
