// UTF-8 BOM removed to ensure consistent encoding
using ACE.Server.Network;

namespace ACE.Server.Command
{
    public delegate void CommandHandler(Session session, params string[] parameters);
}