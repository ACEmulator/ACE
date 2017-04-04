using ACE.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Command
{
    public delegate void CommandHandler(Session session, params string[] parameters);
}