using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
            RadarBehavior = Enum.RadarBehavior.ShowNever;
            RadarColor = Enum.RadarColor.Admin;
            Usable = Enum.Usable.No;

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

        ////public bool GeneratorStatus
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.GeneratorStatus) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.GeneratorStatus, value); }
        ////}

        ////public bool GeneratorEnteredWorld
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.GeneratorEnteredWorld) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.GeneratorEnteredWorld, value); }
        ////}

        ////public bool GeneratorDisabled
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.GeneratorDisabled) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.GeneratorDisabled, value); }
        ////}

        ////public bool GeneratedTreasureItem
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.GeneratedTreasureItem) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.GeneratedTreasureItem, value); }
        ////}

        ////public bool GeneratorAutomaticDestruction
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.GeneratorAutomaticDestruction) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.GeneratorAutomaticDestruction, value); }
        ////}

        ////public bool CanGenerateRare
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.CanGenerateRare) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.CanGenerateRare, value); }
        ////}

        ////public bool CorpseGeneratedRare
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.CorpseGeneratedRare) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.CorpseGeneratedRare, value); }
        ////}

        ////public bool SuppressGenerateEffect
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.SuppressGenerateEffect) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.SuppressGenerateEffect, value); }
        ////}

        ////public bool ChestRegenOnClose
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.ChestRegenOnClose) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.ChestRegenOnClose, value); }
        ////}

        ////public bool ChestClearedWhenClosed
        ////{
        ////    get { return AceObject.GetBoolProperty(PropertyBool.ChestClearedWhenClosed) ?? false; }
        ////    set { AceObject.SetBoolProperty(PropertyBool.ChestClearedWhenClosed, value); }
        ////}

        ////public uint GeneratorTimeType
        ////{
        ////    get { return AceObject.GetIntProperty(PropertyInt.GeneratorTimeType) ?? 0; }
        ////    set { AceObject.SetIntProperty(PropertyInt.GeneratorTimeType, value); }
        ////}

        ////public uint GeneratorProbability
        ////{
        ////    get { return AceObject.GetIntProperty(PropertyInt.GeneratorProbability) ?? 0; }
        ////    set { AceObject.SetIntProperty(PropertyInt.GeneratorProbability, value); }
        ////}

        ////public uint MaxGeneratedObjects
        ////{
        ////    get { return AceObject.GetIntProperty(PropertyInt.MaxGeneratedObjects) ?? 0; }
        ////    set { AceObject.SetIntProperty(PropertyInt.MaxGeneratedObjects, value); }
        ////}

        ////public uint GeneratorType
        ////{
        ////    get { return AceObject.GetIntProperty(PropertyInt.GeneratorType) ?? 0; }
        ////    set { AceObject.SetIntProperty(PropertyInt.GeneratorType, value); }
        ////}

        ////public uint ActivationCreateClass
        ////{
        ////    get { return AceObject.GetIntProperty(PropertyInt.ActivationCreateClass) ?? 0; }
        ////    set { AceObject.SetIntProperty(PropertyInt.ActivationCreateClass, value); }
        ////}
    }
}
