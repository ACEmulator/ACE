using System;
using System.IO;
using System.Linq;

namespace ACE.Server.Web.Util
{
    /// <summary>
    /// adaptation of http://mel-green.com/Uploads/ProgressStream.cs
    /// </summary>
    public class ReportingFileStream : Stream
    {
        #region Private Data Members
        private FileStream innerStream;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new ReportingFileStream supplying the FileStream for it to report on.<para />
        /// Not thread-safe.
        /// </summary>
        /// <param name="streamToReportOn">The underlying FileStream that will be reported on when it is closed the first time.</param>
        public ReportingFileStream(FileStream streamToReportOn)
        {
            if (streamToReportOn != null)
            {
                innerStream = streamToReportOn;
            }
            else
            {
                throw new ArgumentNullException("streamToReportOn");
            }
        }
        #endregion

        #region Events
        public event EventHandler OnFileStreamClosed;
        #endregion

        #region Stream Members

        public override bool CanRead => innerStream.CanRead;

        public override bool CanSeek => innerStream.CanSeek;

        public override bool CanWrite => innerStream.CanWrite;

        public override void Flush()
        {
            innerStream.Flush();
        }

        public override long Length => innerStream.Length;

        public override long Position
        {
            get => innerStream.Position;
            set => innerStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = innerStream.Read(buffer, offset, count);

            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            innerStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            innerStream.Close();
            base.Close();
            OnFileStreamClosed?.Invoke(this, null);
            OnFileStreamClosed?.GetInvocationList().ToList().ForEach(k => OnFileStreamClosed -= (EventHandler)k); // allow only the first invocation
        }
        #endregion
    }
}
