using ACE.DatLoader.Entity.AnimationHooks;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class AnimationHook
    {
        public AnimationHookType HookType { get; set; }
        public uint Direction { get; set; }
        private IHook _hook = null;
        public IHook Hook { get { return _hook; } set { _hook = value; } }

        public static AnimationHook Read(DatReader datReader)
        {
            AnimationHook h = new AnimationHook();

            h.HookType = (AnimationHookType)datReader.ReadUInt32();
            h.Direction = datReader.ReadUInt32();

            // The following HookTypes have no additional properties:
            // AnimationHookType.AnimationDone
            // AnimationHookType.DefaultScript
            // CreateBlockingParticle

            switch (h.HookType)
            {
                case AnimationHookType.Sound:
                    h._hook = SoundHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.SoundTable:
                    h._hook = SoundTableHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.Attack:
                    h._hook = AttackHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.ReplaceObject:
                    h._hook = ReplaceObjectHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.Ethereal:
                    h._hook = EtherealHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.TransparentPart:
                    h._hook = TransparentPartHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.Luminous:
                    h._hook = LuminousHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.LuminousPart:
                    h._hook = LuminousPartHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.Diffuse:
                    h._hook = DiffuseHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.DiffusePart:
                    h._hook = DiffusePartHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.Scale:
                    h._hook = ScaleHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.CreateParticle:
                    h._hook = CreateParticleHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.DestroyParticle:
                    h._hook = DestroyParticleHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.StopParticle:
                    h._hook = StopParticleHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.NoDraw:
                    h._hook = NoDrawHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.DefaultScriptPart:
                    h._hook = DefaultScriptPartHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.CallPES:
                    h._hook = CallPESHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.Transparent:
                    h._hook = TransparentHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.SoundTweaked:
                    h._hook = SoundTweakedHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.SetOmega:
                    h._hook = SetOmegaHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.TextureVelocity:
                    h._hook = TextureVelocityHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.TextureVelocityPart:
                    h._hook = TextureVelocityPartHook.ReadHookType(datReader);
                    break;
                case AnimationHookType.SetLight:
                    h._hook = SetLightHook.ReadHookType(datReader);
                    break;
            }

            return h;
        }
    }
}
