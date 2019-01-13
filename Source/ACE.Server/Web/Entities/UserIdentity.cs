using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Web.Entities
{
    public class UserIdentity
    {
        /// <summary>
        /// Gets or sets the name of the current user.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or set the claims of the current user.
        /// </summary>
        IEnumerable<string> Claims { get; set; }
    }
}
