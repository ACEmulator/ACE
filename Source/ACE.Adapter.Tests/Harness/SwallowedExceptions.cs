using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ACE.Adapter.Tests.Harness
{
    /// <summary>
    /// Records every exception thrown on the current thread while in scope, including ones the
    /// code under test catches and discards.
    /// </summary>
    /// <remarks>
    /// The converters end in a bare <c>catch { return false; }</c>, so a failed conversion reports
    /// only <c>false</c>. Watching first-chance exceptions is the only way to see why.
    /// </remarks>
    internal sealed class SwallowedExceptions : IDisposable
    {
        [ThreadStatic]
        private static List<Exception> current;

        static SwallowedExceptions()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) => current?.Add(e.Exception);
        }

        private readonly List<Exception> previous;

        public SwallowedExceptions()
        {
            previous = current;
            current = Exceptions;
        }

        public List<Exception> Exceptions { get; } = new List<Exception>();

        public void Dispose()
        {
            current = previous;
        }

        /// <summary>
        /// Exception type, message and the innermost frame that has source information.
        /// </summary>
        public static string Describe(Exception exception)
        {
            var frame = new StackTrace(exception, true).GetFrames()
                .FirstOrDefault(f => f.GetFileName() != null);

            var location = frame == null
                ? exception.TargetSite?.ToString()
                : $"{System.IO.Path.GetFileName(frame.GetFileName())}:{frame.GetFileLineNumber()} in {frame.GetMethod()?.Name}";

            return $"{exception.GetType().Name} at {location}: {exception.Message}";
        }
    }
}
