using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ACE.Adapter.Tests.Harness
{
    /// <summary>
    /// One member whose value differs between the start and end of a round trip.
    /// </summary>
    internal sealed class Difference
    {
        public Difference(string path, object before, object after)
        {
            Path = path;
            Before = before;
            After = after;
        }

        /// <summary>
        /// Member path with collection indexes, e.g. <c>Weenie.WeeniePropertiesInt[1].Value</c>.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// <see cref="Path"/> with every collection index replaced by <c>[*]</c>.
        /// </summary>
        public string Pattern => Differ.NormalizeIndexes(Path);

        public object Before { get; }

        public object After { get; }

        public override string ToString()
        {
            return $"{Path}: {Format(Before)} -> {Format(After)}";
        }

        private static string Format(object value)
        {
            return value switch
            {
                null => "<null>",
                string s => $"\"{s}\"",
                DateTime d => d.ToString("O"),
                _ => value.ToString(),
            };
        }
    }

    /// <summary>
    /// Structural comparison of two object graphs by reflection over public properties.
    /// </summary>
    /// <remarks>
    /// Collections are compared by position. Properties that point back at a type already on
    /// the path (EF back references) are skipped, matching what <see cref="Populator"/> fills.
    /// </remarks>
    internal static class Differ
    {
        public static List<Difference> Diff<T>(T before, T after, string rootName = null)
        {
            var differences = new List<Difference>();
            Compare(rootName ?? typeof(T).Name, typeof(T), before, after, new List<Type>(), differences);
            return differences;
        }

        public static string NormalizeIndexes(string path)
        {
            var chars = new System.Text.StringBuilder(path.Length);
            var inIndex = false;
            foreach (var c in path)
            {
                if (c == '[')
                {
                    inIndex = true;
                    chars.Append("[*]");
                }
                else if (c == ']')
                    inIndex = false;
                else if (!inIndex)
                    chars.Append(c);
            }
            return chars.ToString();
        }

        private static void Compare(string path, Type declaredType, object before, object after, List<Type> ancestors, List<Difference> differences)
        {
            if (before == null && after == null)
                return;

            if (IsNullOrEmptyCollection(before) && IsNullOrEmptyCollection(after))
                return;

            if (before == null || after == null)
            {
                differences.Add(new Difference(path, Summarize(before), Summarize(after)));
                return;
            }

            var type = Nullable.GetUnderlyingType(declaredType) ?? declaredType;
            if (type == typeof(object))
                type = before.GetType();

            if (IsScalar(type))
            {
                if (!Equals(before, after))
                    differences.Add(new Difference(path, before, after));
                return;
            }

            if (before is IDictionary beforeDictionary && after is IDictionary afterDictionary)
            {
                var valueType = type.IsGenericType ? type.GetGenericArguments().Last() : typeof(object);
                foreach (var key in beforeDictionary.Keys.Cast<object>().Union(afterDictionary.Keys.Cast<object>()))
                {
                    Compare($"{path}[{key}]", valueType,
                        beforeDictionary.Contains(key) ? beforeDictionary[key] : null,
                        afterDictionary.Contains(key) ? afterDictionary[key] : null,
                        ancestors, differences);
                }
                return;
            }

            if (before is IEnumerable beforeItems && after is IEnumerable afterItems)
            {
                var elementType = Populator.GetElementType(type) ?? typeof(object);
                var left = beforeItems.Cast<object>().ToList();
                var right = afterItems.Cast<object>().ToList();

                if (left.Count != right.Count)
                    differences.Add(new Difference($"{path}.Count", left.Count, right.Count));

                for (var i = 0; i < Math.Min(left.Count, right.Count); i++)
                    Compare($"{path}[{i}]", elementType, left[i], right[i], ancestors, differences);
                return;
            }

            if (ancestors.Contains(type))
                return;

            ancestors.Add(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                    continue;

                var elementType = Populator.GetElementType(property.PropertyType) ?? property.PropertyType;
                if (ancestors.Contains(elementType))
                    continue;

                Compare($"{path}.{property.Name}", property.PropertyType, property.GetValue(before), property.GetValue(after), ancestors, differences);
            }

            ancestors.RemoveAt(ancestors.Count - 1);
        }

        private static bool IsScalar(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(Guid);
        }

        private static bool IsNullOrEmptyCollection(object value)
        {
            return value == null || (value is IEnumerable items && value is not string && !items.Cast<object>().Any());
        }

        private static object Summarize(object value)
        {
            if (value == null || IsScalar(value.GetType()))
                return value;

            if (value is IEnumerable items)
                return $"{value.GetType().Name}(Count = {items.Cast<object>().Count()})";

            return value.GetType().Name;
        }
    }
}
