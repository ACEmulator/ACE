using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum LifestoneType : uint
    {
        Original    = 0x020002EEu,  // Weenie Class ID 509
        New         = 0x02000EADu,  // Weenie Class ID 23618
        CandethKeep = 0x020002EEu,  // Weenie Class ID 24572 :: model ID needs to be fixed
        Lugian      = 0x0200107Du,  // Weenie Class ID 27096
        Broken      = 0x020002EEu   // Weenie Class ID 27457 :: model ID needs to be fixed
    }

    public enum LifestoneWeenieClasses : uint
    {
        Original    = 509,  // Weenie Class ID 509
        New         = 23618,  // Weenie Class ID 23618
        CandethKeep = 24572,  // Weenie Class ID 24572 :: model ID needs to be fixed
        Lugian      = 27096,  // Weenie Class ID 27096
        Broken      = 27457   // Weenie Class ID 27457 :: model ID needs to be fixed
    }
}
