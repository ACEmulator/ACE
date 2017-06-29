using ACE.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class CommonTasks
    {
        public static void WaitForPlayer(Session session)
        {
            if (session.Player != null)
                return;

            var t = new Task(() =>
            {
                while (session.Player == null)
                {
                    Task.Delay(25).Wait();
                }
            });

            t.Start();
            t.Wait();
        }
    }
}
