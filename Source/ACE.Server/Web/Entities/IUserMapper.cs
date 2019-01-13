using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Web.Entities
{
    public interface IUserMapper
    {
        /// <summary>
        /// Get the real username from an identifier
        /// </summary>
        /// <param name="identifier">User identifier</param>
        /// <param name="context">The current NancyFx context</param>
        /// <returns>Matching populated IUserIdentity object, or empty</returns>
        IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context);
    }
}
