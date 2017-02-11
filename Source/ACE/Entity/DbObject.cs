using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ACE.Entity
{
    public class DbObject
    {
        protected Dictionary<PropertyBool, bool> propertiesBool = new Dictionary<PropertyBool, bool>();

        protected Dictionary<PropertyDouble, double> propertiesDouble = new Dictionary<PropertyDouble, double>();

        protected Dictionary<PropertyInt, uint> propertiesInt = new Dictionary<PropertyInt, uint>();

        protected Dictionary<PropertyInt64, ulong> propertiesInt64 = new Dictionary<PropertyInt64, ulong>();

        protected Dictionary<PropertyString, string> propertiesString = new Dictionary<PropertyString, string>();

        public ReadOnlyDictionary<PropertyBool, bool> PropertiesBool => new ReadOnlyDictionary<PropertyBool, bool>(propertiesBool);

        public ReadOnlyDictionary<PropertyDouble, double> PropertiesDouble => new ReadOnlyDictionary<PropertyDouble, double>(propertiesDouble);

        public ReadOnlyDictionary<PropertyInt, uint> PropertiesInt => new ReadOnlyDictionary<PropertyInt, uint>(propertiesInt);

        public ReadOnlyDictionary<PropertyInt64, ulong> PropertiesInt64 => new ReadOnlyDictionary<PropertyInt64, ulong>(propertiesInt64);

        public ReadOnlyDictionary<PropertyString, string> PropertiesString => new ReadOnlyDictionary<PropertyString, string>(propertiesString);

        public uint CreationTimeStamp
        {
            get { return propertiesInt[PropertyInt.CreationTimestamp]; }
            set { propertiesInt[PropertyInt.CreationTimestamp] = value; }
        }

        public uint Id { get; set; }

        public string Name
        {
            get { return propertiesString[PropertyString.Name]; }
            set { SetPropertyString(PropertyString.Name, value); }
        }

        public DbObject()
        {
            CreationTimeStamp = (uint)WorldManager.GetUnixTime();
        }

        public void SetPropertyBool(PropertyBool property, bool value)
        {
            Debug.Assert(property < PropertyBool.Count);
            propertiesBool[property] = value;
        }

        public void SetPropertyDouble(PropertyDouble property, double value)
        {
            Debug.Assert(property < PropertyDouble.Count);
            propertiesDouble[property] = value;
        }

        public void SetPropertyInt(PropertyInt property, uint value)
        {
            Debug.Assert(property < PropertyInt.Count);
            propertiesInt[property] = value;
        }

        public void SetPropertyInt64(PropertyInt64 property, ulong value)
        {
            Debug.Assert(property < PropertyInt64.Count);
            propertiesInt64[property] = value;
        }

        public void SetPropertyString(PropertyString property, string value)
        {
            Debug.Assert(property < PropertyString.Count);
            propertiesString[property] = value;
        }

        protected void InitializeProperties(Type type)
        {
            foreach (PropertyInt prop in System.Enum.GetValues(typeof(PropertyInt)))
            {
                var property = prop.GetPersistedPropertyAttribute();
                if (property?.AppliesTo == type && property?.DefaultValue != null)
                    SetPropertyInt(prop, (uint)property.DefaultValue);
            }

            foreach (PropertyBool prop in System.Enum.GetValues(typeof(PropertyBool)))
            {
                var property = prop.GetPersistedPropertyAttribute();
                if (property?.AppliesTo == type && property?.DefaultValue != null)
                    SetPropertyBool(prop, (bool)property.DefaultValue);
            }

            foreach (PropertyInt64 prop in System.Enum.GetValues(typeof(PropertyInt64)))
            {
                var property = prop.GetPersistedPropertyAttribute();
                if (property?.AppliesTo == type && property?.DefaultValue != null)
                    SetPropertyInt64(prop, (ulong)property.DefaultValue);
            }

            foreach (PropertyDouble prop in System.Enum.GetValues(typeof(PropertyDouble)))
            {
                var property = prop.GetPersistedPropertyAttribute();
                if (property?.AppliesTo == type && property?.DefaultValue != null)
                    SetPropertyDouble(prop, (double)property.DefaultValue);
            }

            foreach (PropertyString prop in System.Enum.GetValues(typeof(PropertyString)))
            {
                var property = prop.GetPersistedPropertyAttribute();
                if (property?.AppliesTo == type && property?.DefaultValue != null)
                    SetPropertyString(prop, (string)property.DefaultValue);
            }
        }
    }
}
