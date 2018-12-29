using System.Collections.Generic;
using System.Net.Sockets;

namespace ACE.Server.TransferServer
{
    public class TransferServerHttpRequest
    {
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
        public string CurrentHeader = string.Empty;
        public string Method = string.Empty;
        public string Uri = string.Empty;
        public string QueryString = string.Empty;
        public string Path = string.Empty;
        public List<string> Fragments = new List<string>();
        public NetworkStream NetworkStream = null;
        public TcpClient Client = null;
    }
}
