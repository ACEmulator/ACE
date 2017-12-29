using ACE.Network;
using System.Threading.Tasks;

namespace ACE.Command
{
    public delegate Task CommandHandler(Session session, params string[] parameters);
}
