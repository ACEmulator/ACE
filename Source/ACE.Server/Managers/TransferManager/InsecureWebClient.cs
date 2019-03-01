using System;
using System.Net;

namespace ACE.Server.Managers.TransferManager
{
    internal class InsecureWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri url)
        {
            HttpWebRequest _req = (HttpWebRequest)base.GetWebRequest(url);
            _req.ServerCertificateValidationCallback = (s, cert, chain, polErr) =>
            {
                return true; // chain-of-trust not implemented
            };
            return _req;
        }
    }
}
