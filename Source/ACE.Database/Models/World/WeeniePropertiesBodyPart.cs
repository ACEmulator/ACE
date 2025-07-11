using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Body Part Properties of Weenies
/// </summary>
public partial class WeeniePropertiesBodyPart
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Type of Property the value applies to (PropertySkill.????)
    /// </summary>
    public ushort Key { get; set; }

    public int DType { get; set; }

    public int DVal { get; set; }

    public float DVar { get; set; }

    public int BaseArmor { get; set; }

    public int ArmorVsSlash { get; set; }

    public int ArmorVsPierce { get; set; }

    public int ArmorVsBludgeon { get; set; }

    public int ArmorVsCold { get; set; }

    public int ArmorVsFire { get; set; }

    public int ArmorVsAcid { get; set; }

    public int ArmorVsElectric { get; set; }

    public int ArmorVsNether { get; set; }

    public int BH { get; set; }

    public float HLF { get; set; }

    public float MLF { get; set; }

    public float LLF { get; set; }

    public float HRF { get; set; }

    public float MRF { get; set; }

    public float LRF { get; set; }

    public float HLB { get; set; }

    public float MLB { get; set; }

    public float LLB { get; set; }

    public float HRB { get; set; }

    public float MRB { get; set; }

    public float LRB { get; set; }

    public virtual Weenie Object { get; set; }
}
