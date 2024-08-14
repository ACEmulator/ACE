using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    public class BasePropertyDesc : IUnpackable
    {
        public uint m_propertyName;
        public BasePropertyType m_propertyType;
        public PropertyGroupName m_propertyGroup;
        public uint m_propertyProvider;
        public uint m_data;
        public int unknown_int_1; // Is this `m_ePatchFlags`?

        public bool has_default_value; // Guessed Var Name based on usage
        public bool hasMax; // Guessed Var Name based on usage
        public bool hasMin; // Guessed Var Name based on usage
        public float minFloat;
        public float maxFloat;
        public int min;
        public int max;

        // These are technically all extended BasePropertyValue types, e.g. BoolPropertyValue
        public bool defaultValueBool;
        public uint defaultValueColor;
        public Vector3 defaultValueVector;
        public int defaultValueInt;
        public float defaultValueFloat;
        public uint defaultValueEnum;
        public uint defaultValueDataFile;

        float m_fPredictionTimeout;
        PropertyInheritanceType m_inheritanceType;
        PropertyDatFileType m_datFileType;
        PropertyPropagationType m_propagationType;
        //PropertyCachingType m_cachingType;

        public bool m_bRequired;
        public bool m_bReadOnly;
        public bool m_bPropagateToChildren;
        public bool m_bNoCheckpoint;
        public bool m_bAbsoluteTimeStamp;
        public bool m_bGroupable;
        public bool m_bAllAvailable;
        public bool m_bDoNotReplay;
        public bool m_bRecorded;
        public bool m_bToolOnly;

        public Dictionary<uint, uint> m_availableProperties = new Dictionary<uint, uint>();

        public void Unpack(BinaryReader reader)
        {
            // This is a reference to the m_emapper of the MasterProperty. It also matches the SmartArray key
            m_propertyName = reader.ReadUInt32();
            m_propertyType = (BasePropertyType)reader.ReadUInt32();
            m_propertyGroup = (PropertyGroupName)reader.ReadUInt32();
            m_propertyProvider = reader.ReadUInt32();
            m_data = reader.ReadUInt32();

            unknown_int_1 = reader.ReadInt32(); // Always 0

            has_default_value = reader.ReadBoolean();
            if (has_default_value)
            {
                switch (m_propertyType)
                {
                    // Note that these are the only default value types in the end-of-retail MasterProperty file.
                    // There are additional extended "BasePropertyValue" vars and handlers in the client
                    case BasePropertyType.Bool:
                        defaultValueBool = reader.ReadBoolean();
                        break;
                    case BasePropertyType.Color:
                        defaultValueColor = reader.ReadUInt32();
                        break;
                    case BasePropertyType.DataFile:
                        defaultValueDataFile = reader.ReadUInt32();
                        break;
                    case BasePropertyType.Enum:
                        defaultValueEnum = reader.ReadUInt32();
                        break;
                    case BasePropertyType.Float:
                        defaultValueFloat = reader.ReadSingle();
                        break;
                    case BasePropertyType.Integer:
                        defaultValueInt = reader.ReadInt32();
                        break;
                    case BasePropertyType.Vector:
                        defaultValueVector = reader.ReadVector3();
                        break;
                    default:
                        // Should never hit this!
                        throw new NotImplementedException();
                }
            }
            hasMax = reader.ReadBoolean();
            if (hasMax)
            {
                switch (m_propertyType)
                {
                    case BasePropertyType.Float:
                        maxFloat = reader.ReadSingle();
                        break;
                    default:
                        max = reader.ReadInt32();
                        break;
                }
            }
            hasMin = reader.ReadBoolean();
            if (hasMax)
            {
                switch (m_propertyType)
                {
                    case BasePropertyType.Float:
                        minFloat = reader.ReadSingle();
                        break;
                    default:
                        min = reader.ReadInt32();
                        break;
                }
            }

            /* Confident Var Names */
            m_fPredictionTimeout = reader.ReadSingle(); // const float Const_DefaultPredictedExpirationTime =  1.5; // idb
            m_inheritanceType = (PropertyInheritanceType)reader.ReadByte();
            m_datFileType = (PropertyDatFileType)reader.ReadByte();
            m_propagationType = (PropertyPropagationType)reader.ReadByte();

            // The order of these is a guess 
            m_bRequired = reader.ReadBoolean(); // Always false
            m_bReadOnly = reader.ReadBoolean(); // 3 are true
            m_bPropagateToChildren = reader.ReadBoolean(); // Always false
            m_bNoCheckpoint = reader.ReadBoolean(); // 5 are true
            m_bAbsoluteTimeStamp = reader.ReadBoolean(); // Always false
            m_bGroupable = reader.ReadBoolean(); // Always false
            m_bAllAvailable = reader.ReadBoolean(); // Always false
            m_bDoNotReplay = reader.ReadBoolean(); // Always false
            m_bRecorded = reader.ReadBoolean(); // Always false
            m_bToolOnly = reader.ReadBoolean(); // Always true

            var numItems = reader.ReadByte();
            for (var i = 0; i < numItems; i++)
            {
                m_availableProperties.Add(reader.ReadUInt32(), reader.ReadUInt32());
            }
        }

    }
}
