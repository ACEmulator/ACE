using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction
{
    public abstract class GameAction
    {
        protected ClientMessage clientMessage;
        protected Session session;

        public virtual void Handle(ClientMessage message, Session session)
        {
            clientMessage = message;
            this.session = session;
        }

        public virtual void HandleLandBlock(Landblock block)
        {
        }

    }
}