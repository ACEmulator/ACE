using System.Collections.Generic;

using log4net;

using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class AttackQueue
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Player Player;

        public Queue<float> PowerAccuracy;

        public AttackQueue(Player player)
        {
            Player = player;

            PowerAccuracy = new Queue<float>();
        }

        public void Add(float powerAccuracy)
        {
            PowerAccuracy.Enqueue(powerAccuracy);
        }

        public float Fetch()
        {
            if (PowerAccuracy.Count > 1)
                PowerAccuracy.Dequeue();

            if (!PowerAccuracy.TryPeek(out var powerAccuracy))
            {
                //log.Error($"{Player.Name}.AttackQueue.Fetch() - empty queue");
                return 0.5f;
            }
            return powerAccuracy;
        }

        public void Clear()
        {
            PowerAccuracy.Clear();
        }
    }
}
