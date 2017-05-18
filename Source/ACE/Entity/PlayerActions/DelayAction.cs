using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.PlayerActions
{
    public class DelayAction : ObjectAction
    {
        private DateTime end;

        public DelayAction(TimeSpan ts)
        {
            end = DateTime.UtcNow + ts;
        }

        protected override IEnumerator Act()
        {
            while (DateTime.UtcNow < end)
            {
                yield return null;
            }
            yield return null;
        }
    }
}
