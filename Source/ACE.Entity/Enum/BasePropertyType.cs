using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// This is technically "PropertyType", but renamed to avoid confusion with ACE.Entity.Enum.Properties.PropertyType
    /// </summary>
    public enum BasePropertyType
    {
        Invalid = 0,
	    Bool = 1,
        Integer = 2,
        LongInteger = 3,
        Float = 4,
        Vector = 5,
        Color = 6,
        String = 7,
        StringInfo = 8,
        Enum = 9,
        DataFile = 10,
        Waveform = 11,
        InstanceID = 12,
        Position = 13,
        TimeStamp = 14,
        Bitfield32 = 15,
        Bitfield64 = 16,
        Array = 17,
        Struct = 18,
        StringToken = 19,
        PropertyName = 20,
        TriState = 21,
    }
}
