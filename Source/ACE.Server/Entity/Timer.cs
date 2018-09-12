using System.Diagnostics;

namespace ACE.Server.Entity
{
    public class Timer
    {
        private static readonly Stopwatch _timer;

        public static double CurrentTime => _timer.Elapsed.TotalSeconds;

        static Timer()
        {
            _timer = Stopwatch.StartNew();
        }
    }
}
