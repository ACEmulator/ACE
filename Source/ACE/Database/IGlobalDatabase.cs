using System.Collections.Generic;

namespace ACE.Database
{
    public interface IGlobalDatabase
    {
        List<DBSchema> GetDBSchemas();
        DBSchema GetDBSchema(string nodeName);
        bool InsertDBSchema(string nodeName, string schemaName);
        void UpdateSchemaRevision(string nodeName, uint schemaRevision);
    }
}
