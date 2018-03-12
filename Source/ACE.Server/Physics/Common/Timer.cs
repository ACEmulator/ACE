using System.Diagnostics;

namespace ACE.Server.Physics.Common
{
    public class Timer
    {
        private static Stopwatch _timer;

        public static double CurrentTime
        {
            get
            {
                return _timer.Elapsed.TotalSeconds;
            }
        }

        static Timer()
        {
            _timer = Stopwatch.StartNew();
        }
    }
}
