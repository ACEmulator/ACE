using System;
using System.Data;
using System.Reflection;

namespace ACE.Database
{
    public class MySqlResult : DataTable
    {
        public uint Count { get; set; }

        public T Read<T>(uint row, string columnName, int number = 0)
        {
            checked
            {
                var val = Rows[(int)row][columnName + (number != 0 ? (1 + number).ToString() : "")];

                if (typeof(T).GetTypeInfo().IsEnum)
                    return (T)Enum.ToObject(typeof(T), val);

                if (val.GetType() == typeof(DBNull))
                    return default(T);

                return (T)Convert.ChangeType(val, typeof(T));
            }
        }

        public object[] ReadAllValuesFromField(string columnName)
        {
            object[] obj = new object[Count];

            for (int i = 0; i < Count; i++)
                obj[i] = Rows[i][columnName];

            return obj;
        }
    }
}
