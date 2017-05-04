namespace ACE.Entity.Enum
{
    using System.Diagnostics.CodeAnalysis;
    using System;

    /// <summary>
    /// this is used as a flag to tell the client what we are sending about the position of the object.
    /// </summary>
    [Flags]
    public enum UpdatePositionFlag
    {
        /// <summary>
        /// The I got nothing for you....
        /// </summary>
        None = 0x00,
        
        /// <summary>
        /// The velocity vector is present
        /// </summary>
        Velocity = 0x01,

        /// <summary>
        /// The placement - I think this refers to the orientation of the placement of the item. - this could be animationframe_id from looking at the pcaps
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        Placement = 0x02,

        /// <summary>
        /// The object is in contact with the ground - this flag is all that is needed there is no corresponding data sent - probably true for any boolean as it would be redundant redundant.
        /// </summary>
        Contact = 0x04,

        /// <summary>
        /// The zero qw - orientation quaternion has 0 w component 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        ZeroQw = 0x08,

        /// <summary>
        /// The zero qx - orientation quaternion has 0 x component 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        ZeroQx = 0x10,

        /// <summary>
        /// The zero qy - orientation quaternion has 0 y component 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        ZeroQy = 0x20,

        /// <summary>
        /// The zero qz - orientation quaternion has 0 z component 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        ZeroQz = 0x40
    }
}
