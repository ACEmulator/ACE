using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    using global::ACE.Entity.Enum.Properties;
    using System;
    using System.Diagnostics;

    public class Clothing : WorldObject
    {
        public override int? Value
        {
            get { return AceObject.Value; }
            set { AceObject.Value = value; }
        }
        public override int? ArmorLevel
        {
            get { return AceObject.ArmorLevel; }
            set { AceObject.ArmorLevel = value; }
        }
        public Clothing(AceObject aceObject)
            : base(aceObject)
        {
        }
    }
}
