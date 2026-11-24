using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ACE.Adapter.Tests.Harness
{
    /// <summary>
    /// Builds an object graph by reflection with every scalar set to a distinctive non-default
    /// value and every collection populated.
    /// </summary>
    /// <remarks>
    /// Navigation properties that point back at a type already being built (EF back references)
    /// are left null, and recursion stops at <see cref="MaxDepth"/>.
    /// Overrides replace the generated value for one property so that values the converters key
    /// on (property types, enum-like ids) can be held to something meaningful.
    /// </remarks>
    internal sealed class Populator
    {
        public const int CollectionSize = 2;
        public const int MaxDepth = 4;

        public delegate object Override(Populator populator, int elementIndex);

        private readonly bool boolValue;
        private readonly Dictionary<(Type, string), Override> overrides = new Dictionary<(Type, string), Override>();

        private int counter = 2;

        public Populator(bool boolValue)
        {
            this.boolValue = boolValue;
        }

        public bool BoolValue => boolValue;

        /// <summary>
        /// Replaces the generated value of <paramref name="property"/> on <typeparamref name="T"/>.
        /// The delegate receives the index of the owning object within its parent collection.
        /// </summary>
        public Populator With<T>(string property, Override value)
        {
            if (typeof(T).GetProperty(property) == null)
                throw new ArgumentException($"{typeof(T).Name} has no property {property}", nameof(property));

            overrides[(typeof(T), property)] = value;
            return this;
        }

        private List<Type> activeAncestors;
        private int activeDepth;

        public T Create<T>()
        {
            return Create<T>(0);
        }

        /// <summary>
        /// Builds a <typeparamref name="T"/>. Called from inside an override, it continues the
        /// graph being built, so back references are still left null.
        /// </summary>
        public T Create<T>(int elementIndex)
        {
            return (T)Create(typeof(T), activeAncestors ?? new List<Type>(), activeAncestors == null ? 0 : activeDepth + 1, elementIndex);
        }

        public List<T> CreateList<T>(int count = CollectionSize)
        {
            var list = new List<T>();
            for (var i = 0; i < count; i++)
                list.Add(Create<T>(i));
            return list;
        }

        private object Create(Type type, List<Type> ancestors, int depth, int elementIndex)
        {
            if (TryCreateScalar(type, out var scalar))
                return scalar;

            if (depth >= MaxDepth || ancestors.Contains(type))
                return null;

            if (TryCreateCollection(type, ancestors, depth, out var collection))
                return collection;

            if (type.IsInterface || type.IsAbstract)
                return null;

            var instance = Activator.CreateInstance(type);

            ancestors.Add(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite || property.SetMethod?.IsPublic != true || property.GetIndexParameters().Length > 0)
                    continue;

                if (overrides.TryGetValue((type, property.Name), out var custom))
                {
                    var (savedAncestors, savedDepth) = (activeAncestors, activeDepth);
                    (activeAncestors, activeDepth) = (ancestors, depth);
                    try
                    {
                        property.SetValue(instance, custom(this, elementIndex));
                    }
                    finally
                    {
                        (activeAncestors, activeDepth) = (savedAncestors, savedDepth);
                    }
                    continue;
                }

                var elementType = GetElementType(property.PropertyType) ?? property.PropertyType;
                if (ancestors.Contains(elementType))
                    continue;

                property.SetValue(instance, Create(property.PropertyType, ancestors, depth + 1, 0));
            }

            ancestors.RemoveAt(ancestors.Count - 1);

            return instance;
        }

        private bool TryCreateScalar(Type type, out object value)
        {
            var underlying = Nullable.GetUnderlyingType(type) ?? type;

            if (underlying == typeof(string))
                value = $"s{counter++}";
            else if (underlying == typeof(bool))
                value = boolValue;
            else if (underlying.IsEnum)
                value = CreateEnum(underlying);
            else if (underlying == typeof(DateTime))
                value = new DateTime(2001, 2, 3, 4, 5, 6).AddMinutes(counter++);
            else if (underlying == typeof(float))
                value = counter++ + 0.5f;
            else if (underlying == typeof(double))
                value = counter++ + 0.1d;
            else if (underlying == typeof(decimal))
                value = counter++ + 0.75m;
            else if (underlying == typeof(byte))
                value = (byte)(counter++ % 250 + 2);
            else if (underlying == typeof(sbyte))
                value = (sbyte)(counter++ % 120 + 2);
            else if (underlying == typeof(short))
                value = (short)(counter++ % 30000 + 2);
            else if (underlying == typeof(ushort))
                value = (ushort)(counter++ % 60000 + 2);
            else if (underlying == typeof(int))
                value = counter++;
            else if (underlying == typeof(uint))
                value = (uint)counter++;
            else if (underlying == typeof(long))
                value = (long)counter++;
            else if (underlying == typeof(ulong))
                value = (ulong)counter++;
            else
            {
                value = null;
                return false;
            }

            return true;
        }

        private object CreateEnum(Type enumType)
        {
            var values = System.Enum.GetValues(enumType).Cast<object>().Where(v => Convert.ToInt64(v) != 0).ToArray();
            if (values.Length == 0)
                return Activator.CreateInstance(enumType);

            return values[counter++ % values.Length];
        }

        private bool TryCreateCollection(Type type, List<Type> ancestors, int depth, out object collection)
        {
            collection = null;

            if (type.IsArray)
            {
                var arrayElement = type.GetElementType();
                var array = Array.CreateInstance(arrayElement, CollectionSize);
                for (var i = 0; i < CollectionSize; i++)
                    array.SetValue(Create(arrayElement, ancestors, depth, i), i);
                collection = array;
                return true;
            }

            if (!type.IsGenericType)
                return false;

            var definition = type.GetGenericTypeDefinition();
            var arguments = type.GetGenericArguments();

            if (definition == typeof(Dictionary<,>) || definition == typeof(IDictionary<,>))
            {
                var dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(arguments));
                for (var i = 0; i < CollectionSize; i++)
                {
                    var key = Create(arguments[0], ancestors, depth, i);
                    if (key != null && !dictionary.Contains(key))
                        dictionary.Add(key, Create(arguments[1], ancestors, depth, i));
                }
                collection = dictionary;
                return true;
            }

            var element = GetElementType(type);
            if (element == null)
                return false;

            var concrete = definition == typeof(HashSet<>) || definition == typeof(ISet<>)
                ? typeof(HashSet<>).MakeGenericType(element)
                : typeof(List<>).MakeGenericType(element);

            var add = concrete.GetMethod("Add");
            collection = Activator.CreateInstance(concrete);
            for (var i = 0; i < CollectionSize; i++)
                add.Invoke(collection, new[] { Create(element, ancestors, depth, i) });
            return true;
        }

        internal static Type GetElementType(Type type)
        {
            if (type == typeof(string))
                return null;

            if (type.IsArray)
                return type.GetElementType();

            if (!type.IsGenericType)
                return null;

            var definition = type.GetGenericTypeDefinition();
            if (definition == typeof(List<>) || definition == typeof(IList<>) || definition == typeof(ICollection<>)
                || definition == typeof(IEnumerable<>) || definition == typeof(HashSet<>) || definition == typeof(ISet<>))
                return type.GetGenericArguments()[0];

            return null;
        }
    }
}
