using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace ACE.Entity.PlayerActions
{
    public abstract class ObjectAction
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Done { get; private set; }

        private IEnumerator enumerator = null;
        private Stack<IEnumerator> callStack = new Stack<IEnumerator>();

        public ObjectAction()
        {
            Done = false;
        }

        protected abstract IEnumerator Act();

        public void RunNext()
        {
            if (enumerator == null)
            {
                enumerator = Act();
            }

            bool canContinue = enumerator.MoveNext();

            // Will return false when the routine is done
            // The remainder of the code creates a call-stack to handle nested co-routines
            while (!canContinue && callStack.Count != 0)
            {
                log.Warn("Done with frame, popping stack: " + canContinue);
                enumerator = callStack.Pop();
                canContinue = enumerator.MoveNext();
            }

            while (canContinue && enumerator.Current != null)
            {
                ObjectAction a = enumerator.Current as ObjectAction;
                if (a != null)
                {
                    log.Warn("Have action frame, pushing stack: " + canContinue);
                    IEnumerator nextEnum = a.Act();
                    callStack.Push(enumerator);
                    enumerator = nextEnum;
                    canContinue = enumerator.MoveNext();
                }
                else
                {
                    log.Warn("Have new frame, pushing stack: " + canContinue);
                    IEnumerator nextEnum = (IEnumerator)enumerator.Current;
                    callStack.Push(enumerator);
                    enumerator = nextEnum;
                    canContinue = enumerator.MoveNext();
                }
            }

            Done = !canContinue && callStack.Count == 0;
        }

        public static IEnumerator CallCoroutine(IEnumerator actor)
        {
            yield return actor;
        }

        public static IEnumerator CallCoroutine(ObjectAction actor)
        {
            yield return actor;
        }
    }
}
