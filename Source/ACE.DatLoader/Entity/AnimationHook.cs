using System;
using System.IO;

using ACE.DatLoader.Entity.AnimationHooks;
using ACE.Entity.Enum;
using log4net;

namespace ACE.DatLoader.Entity
{
    public class AnimationHook : IUnpackable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnimationHookType HookType { get; private set; }
        public AnimationHookDir Direction { get; private set; }

        public static AnimationHook AnimDoneHook;

        static AnimationHook()
        {
            AnimDoneHook = new AnimationHook();
            AnimDoneHook.HookType = AnimationHookType.AnimationDone;
        }

        /// <summary>
        /// WARNING: If you're reading a hook from the dat, you should use AnimationHook.ReadHook(reader).
        /// If you read a hook from the dat using this function, it is likely you will not read all the data correctly.
        /// </summary>
        public virtual void Unpack(BinaryReader reader)
        {
            HookType = (AnimationHookType)reader.ReadUInt32();
            Direction = (AnimationHookDir)reader.ReadInt32();
        }

        public static AnimationHook ReadHook(BinaryReader reader)
        {
            // We peek forward to get the hook type, then revert our position.
            var hookType = (AnimationHookType)reader.ReadUInt32();
            reader.BaseStream.Position -= 4;

            AnimationHook hook;

            switch (hookType)
            {
                case AnimationHookType.Sound:
                    hook = new SoundHook();
                    break;

                case AnimationHookType.SoundTable:
                    hook = new SoundTableHook();
                    break;

                case AnimationHookType.Attack:
                    hook = new AttackHook();
                    break;

                case AnimationHookType.ReplaceObject:
                    hook = new ReplaceObjectHook();
                    break;

                case AnimationHookType.Ethereal:
                    hook = new EtherealHook();
                    break;

                case AnimationHookType.TransparentPart:
                    hook = new TransparentPartHook();
                    break;

                case AnimationHookType.Luminous:
                    hook = new LuminousHook();
                    break;

                case AnimationHookType.LuminousPart:
                    hook = new LuminousPartHook();
                    break;

                case AnimationHookType.Diffuse:
                    hook = new DiffuseHook();
                    break;

                case AnimationHookType.DiffusePart:
                    hook = new DiffusePartHook();
                    break;

                case AnimationHookType.Scale:
                    hook = new ScaleHook();
                    break;

                case AnimationHookType.CreateParticle:
                    hook = new CreateParticleHook();
                    break;

                case AnimationHookType.DestroyParticle:
                    hook = new DestroyParticleHook();
                    break;

                case AnimationHookType.StopParticle:
                    hook = new StopParticleHook();
                    break;

                case AnimationHookType.NoDraw:
                    hook = new NoDrawHook();
                    break;

                case AnimationHookType.DefaultScriptPart:
                    hook = new DefaultScriptPartHook();
                    break;

                case AnimationHookType.CallPES:
                    hook = new CallPESHook();
                    break;

                case AnimationHookType.Transparent:
                    hook = new TransparentHook();
                    break;

                case AnimationHookType.SoundTweaked:
                    hook = new SoundTweakedHook();
                    break;

                case AnimationHookType.SetOmega:
                    hook = new SetOmegaHook();
                    break;

                case AnimationHookType.TextureVelocity:
                    hook = new TextureVelocityHook();
                    break;

                case AnimationHookType.TextureVelocityPart:
                    hook = new TextureVelocityPartHook();
                    break;

                case AnimationHookType.SetLight:
                    hook = new SetLightHook();
                    break;

                case AnimationHookType.CreateBlockingParticle:
                    hook = new CreateBlockingParticle();
                    break;

                // The following HookTypes have no additional properties:
                // AnimationHookType.AnimationDone
                // AnimationHookType.DefaultScript
                case AnimationHookType.AnimationDone:
                case AnimationHookType.DefaultScript:
                    hook = new AnimationHook();
                    break;

                default:
                    log.Warn($"Not Implemented Hook type encountered: {hookType}");
                    hook = null;
                    break;
            }

            if (hook != null)
                hook.Unpack(reader);

            return hook;
        }
    }
}
