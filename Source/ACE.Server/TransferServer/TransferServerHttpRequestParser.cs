using HttpMachine;
using System;

namespace ACE.Server.TransferServer
{
    //TO-DO: verify HttpMachine is compatible with linux
    public class TransferServerHttpRequestParser : IHttpParserHandler
    {
        public TransferServerHttpRequestParser(Action<TransferServerHttpRequest> RequestHandler)
            : base()
        {
            this.RequestHandler = RequestHandler;
        }

        private readonly Action<TransferServerHttpRequest> RequestHandler = null;
        private TransferServerHttpRequest request = new TransferServerHttpRequest();
        public void OnBody(HttpParser parser, ArraySegment<byte> data)
        {
        }
        public void OnHeaderName(HttpParser parser, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                request.Method = name;
                request.Headers[name] = string.Empty;
            }
        }
        public void OnHeadersEnd(HttpParser parser)
        {
        }
        public void OnHeaderValue(HttpParser parser, string value)
        {
            if (!string.IsNullOrWhiteSpace(request.Method) && request.Headers.ContainsKey(request.Method))
            {
                request.Headers[request.Method] = value;
            }
        }
        public void OnMessageBegin(HttpParser parser)
        {
        }
        public void OnMessageEnd(HttpParser parser)
        {
            RequestHandler?.Invoke(request);
        }
        public void OnFragment(HttpParser parser, string fragment)
        {
            request.Fragments.Add(fragment);
        }
        public void OnMethod(HttpParser parser, string method)
        {
            request.Method = method;
        }
        public void OnPath(HttpParser parser, string path)
        {
            request.Path = path;
        }
        public void OnQueryString(HttpParser parser, string queryString)
        {
            request.QueryString = queryString;
        }
        public void OnRequestUri(HttpParser parser, string requestUri)
        {
            request.Uri = requestUri;
        }
    }
}
