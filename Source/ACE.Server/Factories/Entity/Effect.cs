using log4net;

using ACE.Server.Factories.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Entity
{
    public class Effect
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EffectArgument Quality;
        public MutationEffectType Type;
        public EffectArgument Arg1;
        public EffectArgument Arg2;

        public bool TryMutate(WorldObject wo)
        {
            // type:enum - invalid, double, int32, quality (2 int32s: type and quality), float range (min, max), variable index (int32)
            // a=b,a+=b,a-=b,a*=b,a/=b,a=a<b?b:a+c,a=a>b?b:a-c,a+=b*c,a+=b/c,a-=b*c,a-=b/c,a=b+c,a=b-c,a=b*c,a=b/c

            // do not make changes to the members since this object will be reused

            var result = new EffectArgument(Quality);
            var arg1 = new EffectArgument(Arg1);
            var arg2 = new EffectArgument(Arg2);

            result.ResolveValue(wo);
            arg1.ResolveValue(wo);
            arg2.ResolveValue(wo);

            if (!Validate(wo, result, arg1, arg2, Type))
                return false;

            switch (Type)
            {
                case MutationEffectType.Assign:

                    result = arg1;
                    break;

                case MutationEffectType.Add:

                    result = result + arg1;
                    break;

                case MutationEffectType.Subtract:

                    result = result - arg1;
                    break;

                case MutationEffectType.Multiply:

                    result = result * arg1;
                    break;

                case MutationEffectType.Divide:

                    result = result / arg1;
                    break;

                case MutationEffectType.AtLeastAdd:

                    result = (!result.IsValid || result < arg1) ? arg1 : result + arg2;
                    break;

                case MutationEffectType.AtMostSubtract:

                    result = (!result.IsValid || result > arg1) ? arg1 : result - arg2;
                    break;

                case MutationEffectType.AddMultiply:

                    result = result + (arg1 * arg2);
                    break;

                case MutationEffectType.AddDivide:

                    result = result + (arg1 / arg2);
                    break;

                case MutationEffectType.SubtractMultiply:

                    result = result - (arg1 * arg2);
                    break;

                case MutationEffectType.SubtractDivide:

                    result = result - (arg1 / arg2);
                    break;

                case MutationEffectType.AssignAdd:

                    result = arg1 + arg2;
                    break;

                case MutationEffectType.AssignSubtract:

                    result = arg1 - arg2;
                    break;

                case MutationEffectType.AssignMultiply:

                    result = arg1 * arg2;
                    break;

                case MutationEffectType.AssignDivide:

                    result = arg1 * arg2;
                    break;
            }

            Quality.StoreValue(wo, result);

            return true;
        }

        public bool Validate(WorldObject wo, EffectArgument result, EffectArgument arg1, EffectArgument arg2, MutationEffectType type)
        {
            /*if (!result.IsValid)
            {
                log.Error($"{wo.Name} ({wo.Guid}).TryMutate({type}) - result invalid");
                return false;
            }*/

            if (!arg1.IsValid)
            {
                log.Error($"{wo.Name} ({wo.Guid}).TryMutate({type}) - argument 1 invalid");
                return false;
            }

            switch (type)
            {
                case MutationEffectType.AtLeastAdd:
                case MutationEffectType.AtMostSubtract:
                case MutationEffectType.AddMultiply:
                case MutationEffectType.AddDivide:
                case MutationEffectType.SubtractMultiply:
                case MutationEffectType.SubtractDivide:
                case MutationEffectType.AssignAdd:
                case MutationEffectType.AssignSubtract:
                case MutationEffectType.AssignMultiply:
                case MutationEffectType.AssignDivide:

                    if (!arg2.IsValid)
                    {
                        log.Error($"{wo.Name} ({wo.Guid}).TryMutate({type}) - argument 2 invalid");
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
