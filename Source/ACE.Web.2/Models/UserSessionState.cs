using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACE.Entity;

namespace ACE.Web.Models
{
    public class UserSessionState
    {
        private Account _account = null;

        public Account Account
        {
            get { return _account; }
            set { _account = value; }
        }
    }
}