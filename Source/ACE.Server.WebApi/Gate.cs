using log4net;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ACE.Server.WebApi
{
    /// <summary>
    ///  Allows a governor to encapsulate a queue of requests to help prevent requests from adversely affecting the game server.
    /// </summary>
    public class Gate
    {
        private static readonly Lazy<Gate> lazy = new Lazy<Gate>(() => new Gate());
        public static Gate Instance => lazy.Value;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Thread GateThread { get; set; } = null;
        public bool GateThreadAlive = false;
        private ConcurrentQueue<GatedAction> GateQueue;

        public Gate()
        {
            GateQueue = new ConcurrentQueue<GatedAction>();
            GateThread = new Thread(new ThreadStart(GateThreadLoop));
            GateThread.SetApartmentState(ApartmentState.STA);
            GateThread.Name = "WebApiGate";
            GateThreadAlive = true;
            GateThread.Start();
        }
        /// <summary>
        /// Enqueue and run an action against the game server.  Blocks until finished.
        /// </summary>
        /// <param name="act">the action to synchronously perform</param>
        public static void RunGatedAction(Action act)
        {
            GatedAction ba = new GatedAction() { Action = act, CompletionToken = new ManualResetEvent(false) };
            Instance.GateQueue.Enqueue(ba);
            while (!ba.CompletionToken.WaitOne(500) && Instance.GateThreadAlive) { }
        }
        private static void GateThreadLoop()
        {
            while (Instance.GateThreadAlive)
            {
                Thread.Sleep(500);
                GatedAction act = null;
                while (Instance.GateQueue.TryDequeue(out act) && Instance.GateThreadAlive)
                {
                    try
                    {
                        act.Action();
                    }
                    catch (Exception ex) { log.Error("Gated action threw an error", ex); }
                    finally
                    {
                        act.CompletionToken.Set();
                    }
                    Thread.Sleep(100); // maximum 10 actions per second
                }
            }
        }
    }
}
