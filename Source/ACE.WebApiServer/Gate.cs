using log4net;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ACE.WebApiServer
{
    /// <summary>
    ///  Allows a governor to encapsulate a queue of requests to help prevent requests from adversely affecting the game server.
    /// </summary>
    public class Gate
    {
        public const int SlumberSpeed = 500;
        public const int ActionSpeed = 100; // maximum 10 actions per second
        private static readonly Lazy<Gate> lazy = new Lazy<Gate>(() => new Gate());
        public static Gate Instance => lazy.Value;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Thread GateThread { get; set; } = null;
        public Thread GateThread2 { get; set; } = null;
        public bool GateThreadsAlive = false;
        private ConcurrentQueue<GatedAction> GateQueue;
        private ConcurrentQueue<GatedAction> GateQueue2;

        public Gate()
        {
            GateThreadsAlive = true;
            GateQueue = new ConcurrentQueue<GatedAction>();
            GateQueue2 = new ConcurrentQueue<GatedAction>();
            GateThread = new Thread(new ThreadStart(GateThreadLoop));
            GateThread.SetApartmentState(ApartmentState.STA);
            GateThread.Name = "WebApiGate1";
            GateThread.Start();
            GateThread2 = new Thread(new ThreadStart(GateThreadLoop2));
            GateThread2.SetApartmentState(ApartmentState.STA);
            GateThread2.Name = "WebApiGate2";
            GateThread2.Start();
        }
        public static void Shutdown()
        {
            Instance.GateThreadsAlive = false;
        }

        /// <summary>
        /// Enqueue and run an action against the game server.  Blocks until finished.
        /// </summary>
        /// <param name="act">the action to synchronously perform</param>
        public static void RunGatedAction(Action act, int queue = 0)
        {
            switch (queue)
            {
                case 0:
                    GatedAction ba = new GatedAction() { Action = act, CompletionToken = new ManualResetEvent(false) };
                    Instance.GateQueue.Enqueue(ba);
                    while (!ba.CompletionToken.WaitOne(500) && Instance.GateThreadsAlive) { }
                    break;
                case 1:
                    GatedAction ba2 = new GatedAction() { Action = act, CompletionToken = new ManualResetEvent(false) };
                    Instance.GateQueue2.Enqueue(ba2);
                    while (!ba2.CompletionToken.WaitOne(500) && Instance.GateThreadsAlive) { }
                    break;
                default:
                    throw new Exception("unknown queue number");
            }
            if (!Instance.GateThreadsAlive)
            {
                throw new Exception("Gate was shut down.");
            }
        }
        private static void GateThreadLoop()
        {
            while (Instance.GateThreadsAlive)
            {
                Thread.Sleep(SlumberSpeed);
                GatedAction act = null;
                while (Instance.GateQueue.TryDequeue(out act) && Instance.GateThreadsAlive)
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
                    Thread.Sleep(ActionSpeed);
                }
            }
        }
        private static void GateThreadLoop2()
        {
            while (Instance.GateThreadsAlive)
            {
                Thread.Sleep(SlumberSpeed);
                GatedAction act = null;
                while (Instance.GateQueue2.TryDequeue(out act) && Instance.GateThreadsAlive)
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
                    Thread.Sleep(ActionSpeed);
                }
            }
        }
    }
}
