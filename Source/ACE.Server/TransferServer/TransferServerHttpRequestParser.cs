using ACE.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ACE.Server.TransferServer
{
    public interface IHttpParserHandler
    {
        void OnBody(HttpParser parser, ArraySegment<byte> data);
        void OnFragment(HttpParser parser, string fragment);
        void OnHeaderName(HttpParser parser, string name);
        void OnHeadersEnd(HttpParser parser);
        void OnHeaderValue(HttpParser parser, string value);
        void OnMessageBegin(HttpParser parser);
        void OnMessageEnd(HttpParser parser);
        void OnMethod(HttpParser parser, string method);
        void OnQueryString(HttpParser parser, string queryString);
        void OnRequestUri(HttpParser parser, string requestUri);
    }

    public class HttpParser
    {
        private List<ArraySegment<byte>> segmentPile = new List<ArraySegment<byte>>();
        private readonly IHttpParserHandler handler;
        private const int MaxHeaderLength = 8192;

        public HttpParser(IHttpParserHandler parser)
        {
            handler = parser;
        }

        public bool Execute(ArraySegment<byte> buf)
        {
            if (buf.Count > MaxHeaderLength)
            {
                return false; //too big, toss a fit
            }
            if (segmentPile.Sum(k => k.Count) >= MaxHeaderLength)
            {
                return false; //too big, toss a fit
            }
            segmentPile.Add(buf);
            byte[] subject = ConvertToByteArray(segmentPile);
            if (subject.Length > 4 && subject.TakeLast(4).SequenceEqual(new byte[] { 13, 10, 13, 10 }))
            {
                string strSubject = Encoding.UTF8.GetString(subject);
                strSubject = strSubject.Replace("\r\n", "\n");
                string[] subjectSplit = strSubject.Split('\n');
                if (subjectSplit[0].Length < 1)
                {
                    return false;
                }
                string op = $@"^GET \/\?(get={TransferManager.CookieRegex}) HTTP\/1.[0|1]$";
                Match match = Regex.Match(subjectSplit[0], op);
                if (!match.Success)
                {
                    return false;
                }
                handler.OnQueryString(this, match.Groups[1].Value);
                handler.OnMessageEnd(this);
            }
            return true;
        }

        /// <summary>
        /// https://stackoverflow.com/a/5062498/6620171
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public byte[] ConvertToByteArray(IList<ArraySegment<byte>> list)
        {
            byte[] bytes = new byte[list.Sum(asb => asb.Count)];
            int pos = 0;

            foreach (ArraySegment<byte> asb in list)
            {
                Buffer.BlockCopy(asb.Array, asb.Offset, bytes, pos, asb.Count);
                pos += asb.Count;
            }

            return bytes;
        }
    }


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
