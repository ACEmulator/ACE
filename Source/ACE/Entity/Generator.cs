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
            DescriptionFlags = ObjectDescriptionFlag.None;
            Stuck = true; Attackable = true; HiddenAdmin = true;
            IgnoreCollision = true; Hidden = true; Ethereal = true;
            RadarBehavior = Enum.RadarBehavior.ShowNever;
            RadarColor = Enum.RadarColor.Admin;
            Usable = Enum.Usable.No;
        }

        public bool GeneratorStatus
        {
            get { return AceObject.GeneratorStatus ?? false; }
            set { AceObject.GeneratorStatus = value; }
        }

        public bool GeneratorEnteredWorld
        {
            get { return AceObject.GeneratorEnteredWorld ?? false; }
            set { AceObject.GeneratorEnteredWorld = value; }
        }

        public bool GeneratorDisabled
        {
            get { return AceObject.GeneratorDisabled ?? false; }
            set { AceObject.GeneratorDisabled = value; }
        }

        public bool GeneratedTreasureItem
        {
            get { return AceObject.GeneratedTreasureItem ?? false; }
            set { AceObject.GeneratedTreasureItem = value; }
        }

        public bool GeneratorAutomaticDestruction
        {
            get { return AceObject.GeneratorAutomaticDestruction ?? false; }
            set { AceObject.GeneratorAutomaticDestruction = value; }
        }

        public bool CanGenerateRare
        {
            get { return AceObject.CanGenerateRare ?? false; }
            set { AceObject.CanGenerateRare = value; }
        }

        public bool CorpseGeneratedRare
        {
            get { return AceObject.CorpseGeneratedRare ?? false; }
            set { AceObject.CorpseGeneratedRare = value; }
        }

        public bool SuppressGenerateEffect
        {
            get { return AceObject.SuppressGenerateEffect ?? false; }
            set { AceObject.SuppressGenerateEffect = value; }
        }

        public bool ChestRegenOnClose
        {
            get { return AceObject.ChestRegenOnClose ?? false; }
            set { AceObject.ChestRegenOnClose = value; }
        }

        public bool ChestClearedWhenClosed
        {
            get { return AceObject.ChestClearedWhenClosed ?? false; }
            set { AceObject.ChestClearedWhenClosed = value; }
        }

        public int GeneratorTimeType
        {
            get { return AceObject.GeneratorTimeType ?? 0; }
            set { AceObject.GeneratorTimeType = value; }
        }

        public int GeneratorProbability
        {
            get { return AceObject.GeneratorProbability ?? 0; }
            set { AceObject.GeneratorProbability = value; }
        }

        public int MaxGeneratedObjects
        {
            get { return AceObject.MaxGeneratedObjects ?? 0; }
            set { AceObject.MaxGeneratedObjects = value; }
        }

        public int GeneratorType
        {
            get { return AceObject.GeneratorType ?? 0; }
            set { AceObject.GeneratorType = value; }
        }

        public int ActivationCreateClass
        {
            get { return AceObject.ActivationCreateClass ?? 0; }
            set { AceObject.ActivationCreateClass = value; }
        }
    }
}
