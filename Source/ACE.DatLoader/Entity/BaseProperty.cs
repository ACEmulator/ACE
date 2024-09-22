using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    public class BaseProperty : IUnpackable
    {
        // If this is in a collection/hashtable, this will actually match the key in that table.
        // The enum value of this can be looked up in MasterProperty
        public uint Id; 

        public BasePropertyType PropertyType; // This is read from the MasterProperty table

        // Depending on the PropertyType, one of these values will be populated
        public uint ValueEnum;
        public bool ValueBool;
        public uint ValueDataFile;
        public float ValueFloat;
        public StringInfo ValueString;
        public uint ValueColor;
        public List<BaseProperty> ValueArray;
        public int ValueInt;
        public Dictionary<uint, BaseProperty> ValueStruct;
        public Vector3 ValueVector;
        public uint ValueBitfield32;
        public ulong ValueBitfield64;
        public uint ValueInstanceId;

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            if (!DatManager.PortalDat.MasterProperty.m_properties.ContainsKey(Id))
                throw (new Exception("Unable to locate BaseProperty type in the MasterProperty table"));

            PropertyType = DatManager.PortalDat.MasterProperty.m_properties[Id].m_propertyType;
            switch(PropertyType)
            {
                case BasePropertyType.Enum:
                    ValueEnum = reader.ReadUInt32();
                    break;
                case BasePropertyType.Bool:
                    ValueBool = reader.ReadBoolean();
                    break;
                case BasePropertyType.DataFile:
                    ValueDataFile = reader.ReadUInt32();
                    break;
                case BasePropertyType.Float:
                    ValueFloat = reader.ReadSingle();
                    break;
                case BasePropertyType.StringInfo:
                    ValueString = new StringInfo();
                    ValueString.Unpack(reader);
                    break;
                case BasePropertyType.Color:
                    ValueColor = reader.ReadUInt32();
                    break;
                case BasePropertyType.Array:
                    ValueArray = new List<BaseProperty>();
                    ValueArray.Unpack(reader);
                    break;
                case BasePropertyType.Integer:
                    ValueInt = reader.ReadInt32();
                    break;
                case BasePropertyType.Struct:
                    ValueStruct = new Dictionary<uint, BaseProperty>();

                    // This packed list uses a little different format not handled in the unpackable extensions
                    reader.ReadByte(); // bucketSize?
                    var totalObjects = reader.ReadByte();
                    for (int i = 0; i < totalObjects; i++)
                    {
                        var key = reader.ReadUInt32();

                        var item = new BaseProperty();
                        item.Unpack(reader);
                        ValueStruct.Add(key, item);
                    }
                    break;
                case BasePropertyType.String:
                    // No use cases in end-of-retail Dat files.
                    // While there are a couple of MasterProperty entries of this type, there are no BaseProperty entries that use them.
                    break;
                case BasePropertyType.Vector:
                    ValueVector = reader.ReadVector3();
                    break;
                case BasePropertyType.Bitfield32:
                    ValueBitfield32 = reader.ReadUInt32();
                    break;
                case BasePropertyType.Bitfield64:
                    ValueBitfield64 = reader.ReadUInt64();
                    break;
                case BasePropertyType.InstanceID:
                    ValueInstanceId = reader.ReadUInt32();
                    break;
                default:
                    // Should never hit this!
                    throw new NotImplementedException();
                    //break;
            }


        }
    }
}
