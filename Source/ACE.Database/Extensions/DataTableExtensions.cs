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

            var columns = Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToList();

            var dataColummn = new DataColumn();

            foreach (var column in columns)
            {
                if (column == "MAX(`aceObjectId`)")
                {
                    dataColummn.ColumnName = column;
                    dataColummn.DataType = typeof(UInt32);
                }
                else
                    throw new NotImplementedException();
            }

            dataTable.Columns.Add(dataColummn);

            while (dataReader.Read())
            {
                if (columns.Count == 1)
                    dataTable.Rows.Add(dataReader[0]);
                else
                    throw new NotImplementedException();
            }
#endif
        }
    }
}
