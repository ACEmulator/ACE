using System;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PersistedPropertyAttribute : Attribute
    {
        public bool Persisted { get; set; }

        public Type AppliesTo { get; set; }

        public object DefaultValue { get; set; }

        public PersistedPropertyAttribute(bool persisted, Type appliesTo, object defaultValue)
        {
            if (!appliesTo.IsSubclassOf(typeof(DbObject)))
            {
                throw new ArgumentException("PersistedProperty can only be applied to types that derive from DbObject.");
            }

            this.Persisted = persisted;
            this.AppliesTo = appliesTo;
            this.DefaultValue = defaultValue;
        }
    }
}
