using ACE.Entity.Enum;
using ACE.Network.Enum;


namespace ACE.Entity
{
    /// <summary>
    /// temporary portals (b/c they can disappear), corpses, items on the ground, etc.
    /// </summary>
    public class ImmutableWorldObject : WorldObject
    {
        public ImmutableWorldObject(ObjectType type, ObjectGuid guid, string name, WeenieClass weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position) : base(type, guid)
        {
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Position = position;
            this.WeenieClassid = weenieClassId;
        }
    }
}
