using ACE.Server.Factories.Enum;

using MaterialType = ACE.Entity.Enum.MaterialType;

namespace ACE.Server.Factories.Entity
{
    public class GemResult
    {
        public WeenieClassName ClassName;
        public MaterialType MaterialType;

        public GemResult(WeenieClassName className, MaterialType materialType)
        {
            ClassName = className;
            MaterialType = materialType;
        }
    }
}
