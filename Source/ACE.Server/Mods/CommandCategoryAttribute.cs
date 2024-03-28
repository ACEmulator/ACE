using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Mods;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandCategoryAttribute : Attribute
{
    public string Category { get; }

    public CommandCategoryAttribute(string category)
    {
        Category = category;
    }
}
