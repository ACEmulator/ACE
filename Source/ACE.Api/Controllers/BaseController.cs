using ACE.Common;
using ACE.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ACE.Api.Controllers
{
    public class BaseController : ApiController
    {
        public static WorldDatabase WorldDb { get; private set; }

        public static AuthenticationDatabase AuthDb { get; private set; }

        static BaseController()
        {
            WorldDb = new WorldDatabase();
            WorldDb.Initialize(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database);

            AuthDb = new AuthenticationDatabase();
            AuthDb.Initialize(ConfigManager.Config.MySql.Authentication.Host,
                          ConfigManager.Config.MySql.Authentication.Port,
                          ConfigManager.Config.MySql.Authentication.Username,
                          ConfigManager.Config.MySql.Authentication.Password,
                          ConfigManager.Config.MySql.Authentication.Database);

        }
    }
}
