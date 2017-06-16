using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public enum ConstructedStatementType
    {
        Insert,
        Update,
        Delete,
        Get,
        GetList,
        DeleteList,
        InsertList,
        GetAggregate
    }
}
