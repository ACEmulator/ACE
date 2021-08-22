using log4net;

using ACE.Entity.Enum;

namespace ACE.Server.Entity.Mutations
{
    public partial class EffectArgument
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static EffectArgument operator +(EffectArgument a, EffectArgument b)
        {
            switch (a.Type)
            {
                case EffectArgumentType.Double:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument(a.DoubleVal + b.DoubleVal);
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.DoubleVal + b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument(a.DoubleVal + b.LongVal);
                    }
                    break;

                case EffectArgumentType.Int:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument((int)(a.IntVal + b.DoubleVal));
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.IntVal + b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument((int)(a.IntVal + b.LongVal));
                    }
                    break;

                case EffectArgumentType.Int64:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument((long)(a.LongVal + b.DoubleVal));
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.LongVal + b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument(a.LongVal + b.LongVal);
                    }
                    break;
            }

            log.Error($"EffectArgument.Add() - invalid type {a.Type}, {b.Type}");

            return null;
        }

        public static EffectArgument operator -(EffectArgument a, EffectArgument b)
        {
            switch (a.Type)
            {
                case EffectArgumentType.Double:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument(a.DoubleVal - b.DoubleVal);
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.DoubleVal - b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument(a.DoubleVal - b.LongVal);
                    }
                    break;

                case EffectArgumentType.Int:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument((int)(a.IntVal - b.DoubleVal));
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.IntVal - b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument((int)(a.IntVal - b.LongVal));
                    }
                    break;

                case EffectArgumentType.Int64:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument((long)(a.LongVal - b.DoubleVal));
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.LongVal - b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument(a.LongVal - b.LongVal);
                    }
                    break;
            }

            log.Error($"EffectArgument.Subtract() - invalid type {a.Type}, {b.Type}");

            return null;
        }

        public static EffectArgument operator *(EffectArgument a, EffectArgument b)
        {
            switch (a.Type)
            {
                case EffectArgumentType.Double:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument(a.DoubleVal * b.DoubleVal);
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.DoubleVal * b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument(a.DoubleVal * b.LongVal);
                    }
                    break;

                case EffectArgumentType.Int:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument((int)(a.IntVal * b.DoubleVal));
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.IntVal * b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument((int)(a.IntVal * b.LongVal));
                    }
                    break;

                case EffectArgumentType.Int64:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return new EffectArgument((long)(a.LongVal * b.DoubleVal));
                        case EffectArgumentType.Int:
                            return new EffectArgument(a.LongVal * b.IntVal);
                        case EffectArgumentType.Int64:
                            return new EffectArgument(a.LongVal * b.LongVal);
                    }
                    break;
            }

            log.Error($"EffectArgument.Multiply() - invalid type {a.Type}, {b.Type}");

            return null;
        }

        public static EffectArgument operator /(EffectArgument a, EffectArgument b)
        {
            switch (a.Type)
            {
                case EffectArgumentType.Double:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return b.DoubleVal != 0 ? new EffectArgument(a.DoubleVal / b.DoubleVal) : a;
                        case EffectArgumentType.Int:
                            return b.IntVal != 0 ? new EffectArgument(a.DoubleVal / b.IntVal) : a;
                        case EffectArgumentType.Int64:
                            return b.LongVal != 0 ? new EffectArgument(a.DoubleVal / b.LongVal) : a;
                    }
                    break;

                case EffectArgumentType.Int:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return b.DoubleVal != 0 ? new EffectArgument((int)(a.IntVal / b.DoubleVal)) : a;
                        case EffectArgumentType.Int:
                            return b.IntVal != 0 ? new EffectArgument(a.IntVal / b.IntVal) : a;
                        case EffectArgumentType.Int64:
                            return b.LongVal != 0 ? new EffectArgument((int)(a.IntVal / b.LongVal)) : a;
                    }
                    break;

                case EffectArgumentType.Int64:

                    switch (b.Type)
                    {
                        case EffectArgumentType.Double:
                            return b.DoubleVal != 0 ? new EffectArgument((long)(a.LongVal / b.DoubleVal)): a;
                        case EffectArgumentType.Int:
                            return b.IntVal != 0 ? new EffectArgument(a.LongVal / b.IntVal) : a;
                        case EffectArgumentType.Int64:
                            return b.LongVal != 0 ? new EffectArgument(a.LongVal / b.LongVal) : a;
                    }
                    break;
            }

            log.Error($"EffectArgument.Divide() - invalid type {a.Type}, {b.Type}");

            return null;
        }

        public static bool operator <(EffectArgument a, EffectArgument b)
        {
            if (a.Type != b.Type)
            {
                log.Error($"EffectArgument.LessThan() - type mismatch {a.Type} {b.Type}");
                return false;
            }

            switch (a.Type)
            {
                case EffectArgumentType.Double:
                    return a.DoubleVal < b.DoubleVal;

                case EffectArgumentType.Int:
                    return a.IntVal < b.IntVal;

                case EffectArgumentType.Int64:
                    return a.LongVal < b.LongVal;
            }
            return false;
        }

        public static bool operator >(EffectArgument a, EffectArgument b)
        {
            if (a.Type != b.Type)
            {
                log.Error($"EffectArgument.GreaterThan() - type mismatch {a.Type} {b.Type}");
                return false;
            }

            switch (a.Type)
            {
                case EffectArgumentType.Double:
                    return a.DoubleVal > b.DoubleVal;

                case EffectArgumentType.Int:
                    return a.IntVal > b.IntVal;

                case EffectArgumentType.Int64:
                    return a.LongVal > b.LongVal;
            }
            return false;
        }
    }
}
