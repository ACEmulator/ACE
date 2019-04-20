using System.Collections.Generic;
using System.Linq;

namespace ACE.Entity.Enum.Properties
{
    /// <summary>
    /// Static selection of client enums that are [AssessmentProperty]<para />
    /// These are properties sent to the client on id.
    /// </summary>
    public static class AssessmentProperties
    {
        /// <summary>
        /// Method to return a list of enums by attribute type - in this case [AssessmentProperty] using generics to enhance code reuse.
        /// </summary>
        /// <typeparam name="T">Enum to list by [AssessmentProperty]</typeparam>
        /// <typeparam name="TResult">Type of the results</typeparam>
        private static HashSet<TResult> GetValues<T, TResult>()
        {
            var list = typeof(T).GetFields().Select(x => new
            {
                att = x.GetCustomAttributes(false).OfType<AssessmentPropertyAttribute>().FirstOrDefault(),
                member = x
            }).Where(x => x.att != null && x.member.Name != "value__").Select(x => (TResult)x.member.GetValue(null)).ToList();

            return new HashSet<TResult>(list);
        }

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary
        public static HashSet<ushort> PropertiesInt = GetValues<PropertyInt, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesInt64 = GetValues<PropertyInt64, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesBool = GetValues<PropertyBool, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesString = GetValues<PropertyString, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesDouble = GetValues<PropertyFloat, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesDataId = GetValues<PropertyDataId, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesInstanceId = GetValues<PropertyInstanceId, ushort>();
    }
}
