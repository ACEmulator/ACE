using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.PlayerActions
{
    public class DelegateAction : ObjectAction
    {
        private Func<IEnumerator> fcn;
        public DelegateAction(Func<IEnumerator> fcn)
        {
            this.fcn = fcn;
        }

        protected override IEnumerator Act()
        {
            return fcn();
        }
    }
}
