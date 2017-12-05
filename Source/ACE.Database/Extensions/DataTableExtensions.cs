using System;
using System.Data;
using System.Linq;

namespace ACE.Database.Extensions
{
    public static class DataTableExtensions
    {
        public static void LoadEx(this DataTable  dataTable, IDataReader dataReader)
        {
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0)
           dataTable.Load(dataReader);
#else
            // In .net core, System.Data.SqlClient.SqlDataReader does not implement GetSchemaTable()
            // See: https://github.com/dotnet/corefx/issues/19748
            // So, as a work around, we populate the dataTable manually.
            // Big thanks to Rawaho for helping with this.

            for (int i = 0; i < dataReader.FieldCount; i++)
                dataTable.Columns.Add(dataReader.GetName(i), dataReader.GetFieldType(i));

            while (dataReader.Read())
            {
                DataRow row = dataTable.NewRow();

                for (int i = 0; i < dataReader.FieldCount; i++)
                    row[i] = dataReader.GetValue(i);

                dataTable.Rows.Add(row);
            }
#endif
        }
    }
}
