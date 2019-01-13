using Nancy;
using System;

namespace ACE.Server.Web.Entities
{
    public interface IUserIdentity
    {
        IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context);
    }
}
