using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;
using System.Diagnostics;

namespace ACE.Entity
{
    public class Generator : WorldObject
    {
        public Generator(ObjectGuid guid, AceObject baseAceObject)
            : base(guid, baseAceObject)
        {
            DescriptionFlags = ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable | ObjectDescriptionFlag.HiddenAdmin;
            PhysicsState = PhysicsState.IgnoreCollision | PhysicsState.Hidden | PhysicsState.Ethereal;
            RadarBehavior = Network.Enum.RadarBehavior.ShowNever;
            RadarColor = Network.Enum.RadarColor.Admin;
            Usable = Network.Enum.Usable.No;

            SetPhysicsDescriptionFlag(this);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();
        }

        ////public Generator(AceObject aceO)
        ////    : this(new ObjectGuid(aceO.AceObjectId), aceO)
        ////{
        ////    // FIXME(ddevec): These should be inhereted from aceO, not copied
        ////    Location = aceO.Location;
        ////    Debug.Assert(aceO.Location != null, "Trying to create DebugObject with null location");
        ////    WeenieClassId = aceO.WeenieClassId;
        ////    GameDataType = aceO.Type;
        ////}
    }
}
