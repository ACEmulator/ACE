using System.Collections.Generic;
using System.Linq;

namespace ACE.Entity.Enum.Properties
{
    /// <summary>
    /// Static selection of client enums that are [Ephemeral]<para />
    /// These are properties that aren't saved to the shard.
    /// </summary>
    public static class EphemeralProperties
    {
        /// <summary>
        /// Method to return a list of enums by attribute type - in this case [Ephemeral] using generics to enhance code reuse.
        /// </summary>
        /// <typeparam name="T">Enum to list by [Ephemeral]</typeparam>
        /// <typeparam name="TResult">Type of the results</typeparam>
        private static HashSet<TResult> GetValues<T, TResult>()
        {
            var list = typeof(T).GetFields().Select(x => new
            {
                att = x.GetCustomAttributes(false).OfType<EphemeralAttribute>().FirstOrDefault(),
                member = x
            }).Where(x => x.att != null && x.member.Name != "value__").Select(x => (TResult)x.member.GetValue(null)).ToList();

            return new HashSet<TResult>(list);
        }

        /// <summary>
        /// returns a list of values for PropertyInt that are [Ephemeral]
        /// </summary
        public static HashSet<ushort> PropertiesInt = GetValues<PropertyInt, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt64 that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PropertiesInt64 = GetValues<PropertyInt64, ushort>();

        /// <summary>
        /// returns a list of values for PropertyBool that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PropertiesBool = GetValues<PropertyBool, ushort>();

        /// <summary>
        /// returns a list of values for PropertyString that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PropertiesString = GetValues<PropertyString, ushort>();

        /// <summary>
        /// returns a list of values for PropertyFloat that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PropertiesDouble = GetValues<PropertyFloat, ushort>();

        /// <summary>
        /// returns a list of values for PropertyDataId that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PropertiesDataId = GetValues<PropertyDataId, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInstanceId that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PropertiesInstanceId = GetValues<PropertyInstanceId, ushort>();


        /// <summary>
        /// returns a list of values for PositionType that are [Ephemeral]
        /// </summary>
        public static HashSet<ushort> PositionTypes = GetValues<PositionType, ushort>();
    }
}
