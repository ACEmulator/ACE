using ACE.Server.Network;

namespace ACE.Server.Command
{
    public delegate void CommandHandler(Session session, params string[] parameters);
}