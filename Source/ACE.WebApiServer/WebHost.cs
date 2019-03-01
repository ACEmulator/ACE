using ACE.Server.Managers;
using ACE.Server.Managers.TransferManager.Responses;
using ACE.WebApiServer.Model;
using ACE.WebApiServer.Model.Character;
using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Net;
using System.Threading;

namespace ACE.WebApiServer
{
    internal static class WebHost
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Thread hostThread = null;
        private static IWebHost host = null;
        public static void Run(IPAddress listenAt, int port)
        {
            if (hostThread != null)
            {
                return;
            }

            //initialize model polymorphism
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BaseAuthenticatedModel, CharacterListModel>();
                cfg.CreateMap<MigrationCheckResponseModel, SignedMigrationCheckResponseModel>();
            });


            hostThread = new Thread(new ThreadStart(() =>
            {
                if (CertificateManager.CertificateWebApi == null)
                {
                    log.Fatal("Key and certificate bundle is unavailable.  Aborting WebAPI hosting.");
                    return;
                }
                host = new WebHostBuilder()
                    .UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "True")
                    .UseKestrel(options =>
                    {
                        options.Listen(listenAt, port, listenOptions =>
                        {
                            listenOptions.UseHttps(CertificateManager.CertificateWebApi);
                        });
                    })
                    .UseStartup<KestrelStartup>()
                    .Build();
                host.Run();
            }));
            hostThread.SetApartmentState(ApartmentState.STA);
            hostThread.Priority = ThreadPriority.BelowNormal;
            hostThread.Start();
        }
        public static void Shutdown()
        {
            host.StopAsync(TimeSpan.FromSeconds(1000)).RunSynchronously();
        }
    }
}
