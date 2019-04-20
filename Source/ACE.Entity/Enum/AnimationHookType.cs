namespace ACE.Entity.Enum
{
    public enum AnimationHookType
    {
        Unknown                 = -1,
        NoOp                    = 0,
        Sound                   = 1,
        SoundTable              = 2,
        Attack                  = 3,
        AnimationDone           = 4,
        ReplaceObject           = 5,
        Ethereal                = 6,
        TransparentPart         = 7,
        Luminous                = 8,
        LuminousPart            = 9,
        Diffuse                 = 10,
        DiffusePart             = 11,
        Scale                   = 12,
        CreateParticle          = 13,
        DestroyParticle         = 14,
        StopParticle            = 15,
        NoDraw                  = 16,
        DefaultScript           = 17,
        DefaultScriptPart       = 18,
        CallPES                 = 19, // Particle Emitter System
        Transparent             = 20,
        SoundTweaked            = 21,
        SetOmega                = 22,
        TextureVelocity         = 23,
        TextureVelocityPart     = 24,
        SetLight                = 25,
        CreateBlockingParticle  = 26,
        ForceAnimationHook32Bit = 2147483647
    }
}
