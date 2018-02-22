using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public Dictionary<PropertyBool, bool?> EphemeralPropertyBools = new Dictionary<PropertyBool, bool?>();
        public Dictionary<PropertyDataId, uint?> EphemeralPropertyDataIds = new Dictionary<PropertyDataId, uint?>();
        public Dictionary<PropertyFloat, double?> EphemeralPropertyFloats = new Dictionary<PropertyFloat, double?>();
        public Dictionary<PropertyInstanceId, int?> EphemeralPropertyInstanceIds = new Dictionary<PropertyInstanceId, int?>();
        public Dictionary<PropertyInt, int?> EphemeralPropertyInts = new Dictionary<PropertyInt, int?>();
        public Dictionary<PropertyInt64, long?> EphemeralPropertyInt64s = new Dictionary<PropertyInt64, long?>();
        public Dictionary<PropertyString, string> EphemeralPropertyStrings = new Dictionary<PropertyString, string>();

        public bool? GetProperty(PropertyBool property) { if (EphemeralPropertyBools.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public uint? GetProperty(PropertyDataId property) { if (EphemeralPropertyDataIds.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public double? GetProperty(PropertyFloat property) { if (EphemeralPropertyFloats.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public int? GetProperty(PropertyInstanceId property) { if (EphemeralPropertyInstanceIds.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public int? GetProperty(PropertyInt property) { if (EphemeralPropertyInts.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public long? GetProperty(PropertyInt64 property) { if (EphemeralPropertyInt64s.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public string GetProperty(PropertyString property) { if (EphemeralPropertyStrings.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }

        public void SetProperty(PropertyBool property, bool value) { if (EphemeralPropertyBools.ContainsKey(property)) EphemeralPropertyBools[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyDataId property, uint value) { if (EphemeralPropertyDataIds.ContainsKey(property)) EphemeralPropertyDataIds[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyFloat property, double value) { if (EphemeralPropertyFloats.ContainsKey(property)) EphemeralPropertyFloats[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInstanceId property, int value) { if (EphemeralPropertyInstanceIds.ContainsKey(property)) EphemeralPropertyInstanceIds[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInt property, int value) { if (EphemeralPropertyInts.ContainsKey(property)) EphemeralPropertyInts[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInt64 property, long value) { if (EphemeralPropertyInt64s.ContainsKey(property)) EphemeralPropertyInt64s[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyString property, string value) { if (EphemeralPropertyStrings.ContainsKey(property)) EphemeralPropertyStrings[property] = value; else Biota.SetProperty(property, value); }

        public void RemoveProperty(PropertyBool property) { if (EphemeralPropertyBools.ContainsKey(property)) EphemeralPropertyBools[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyDataId property) { if (EphemeralPropertyDataIds.ContainsKey(property)) EphemeralPropertyDataIds[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyFloat property) { if (EphemeralPropertyFloats.ContainsKey(property)) EphemeralPropertyFloats[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInstanceId property) { if (EphemeralPropertyInstanceIds.ContainsKey(property)) EphemeralPropertyInstanceIds[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInt property) { if (EphemeralPropertyInts.ContainsKey(property)) EphemeralPropertyInts[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInt64 property) { if (EphemeralPropertyInt64s.ContainsKey(property)) EphemeralPropertyInt64s[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyString property) { if (EphemeralPropertyStrings.ContainsKey(property)) EphemeralPropertyStrings[property] = null; else Biota.RemoveProperty(property); }

        public Dictionary<PropertyBool, bool> GetAllPropertyBools()
        {
            var results = new Dictionary<PropertyBool, bool>();

            foreach (var property in Biota.BiotaPropertiesBool)
                results[(PropertyBool)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyBools)
                if (property.Value.HasValue)
                    results[property.Key] = (bool)property.Value;

            return results;
        }

        public Dictionary<PropertyDataId, uint> GetAllPropertyDataId()
        {
            var results = new Dictionary<PropertyDataId, uint>();

            foreach (var property in Biota.BiotaPropertiesDID)
                results[(PropertyDataId)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyDataIds)
                if (property.Value.HasValue)
                    results[property.Key] = (uint)property.Value;

            return results;
        }

        public Dictionary<PropertyFloat, double> GetAllPropertyFloat()
        {
            var results = new Dictionary<PropertyFloat, double>();

            foreach (var property in Biota.BiotaPropertiesFloat)
                results[(PropertyFloat)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyFloats)
                if (property.Value.HasValue)
                    results[property.Key] = (double)property.Value;

            return results;
        }

        public Dictionary<PropertyInstanceId, int> GetAllPropertyInstanceId()
        {
            var results = new Dictionary<PropertyInstanceId, int>();

            foreach (var property in Biota.BiotaPropertiesIID)
                results[(PropertyInstanceId)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInstanceIds)
                if (property.Value.HasValue)
                    results[property.Key] = (int)property.Value;

            return results;
        }

        public Dictionary<PropertyInt, int> GetAllPropertyInt()
        {
            var results = new Dictionary<PropertyInt, int>();

            foreach (var property in Biota.BiotaPropertiesInt)
                results[(PropertyInt)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInts)
                if (property.Value.HasValue)
                    results[property.Key] = (int)property.Value;

            return results;
        }

        public Dictionary<PropertyInt64, long> GetAllPropertyInt64()
        {
            var results = new Dictionary<PropertyInt64, long>();

            foreach (var property in Biota.BiotaPropertiesInt64)
                results[(PropertyInt64)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInt64s)
                if (property.Value.HasValue)
                    results[property.Key] = (long)property.Value;

            return results;
        }

        public Dictionary<PropertyString, string> GetAllPropertyString()
        {
            var results = new Dictionary<PropertyString, string>();

            foreach (var property in Biota.BiotaPropertiesString)
                results[(PropertyString)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyStrings)
                if (property.Value != null)
                    results[property.Key] = property.Value;

            return results;
        }


        //public Dictionary<PositionType, Position> Positions { get; set; } = new Dictionary<PositionType, Position>();

        public Position GetPosition(PositionType positionType) // { return Biota.GetPosition(positionType); }
        {
            bool success = Positions.TryGetValue(positionType, out var ret);

            if (!success)
            {
                var result = Biota.GetPosition(positionType);

                if (result != null)
                    Positions.TryAdd(positionType, result);

                return result;
            }

            //if (!success)
            //{
            //    return null;
            //}
            return ret;
        }

        public void SetPosition(PositionType positionType, Position position) // { Biota.SetPosition(positionType, position); }
        {
            if (position == null)
            {
                Positions.Remove(positionType);
                Biota.RemovePosition(positionType);
            }
            else
            {
                if (!Positions.ContainsKey(positionType))
                    Positions.TryAdd(positionType, position);
                else
                    Positions[positionType] = position;
                Biota.SetPosition(positionType, position);
            }
        }

        // todo: We also need to manually remove the property from the shard db.
        // todo: Using these fn's will only remove the property for this session, but the property will return next session since the record isn't removed from the db.
        // todo: fix this in BiotaExcentions. Add code there that removes the entry from teh shard
        public void RemovePosition(PositionType positionType) { Biota.RemovePosition(positionType); }
    }
}
