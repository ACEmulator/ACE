using System;

namespace ACE.Server.Network
{

    public sealed class NetworkSyntheticTesting
    {
        private static readonly Lazy<NetworkSyntheticTesting> lazy =
            new Lazy<NetworkSyntheticTesting>(() => new NetworkSyntheticTesting());

        public static NetworkSyntheticTesting Instance => lazy.Value;

        private NetworkSyntheticTesting() { }

        public bool _TrashNextPacketC2S = false;
        public static bool TrashNextPacketC2S { get => Instance._TrashNextPacketC2S; set => Instance._TrashNextPacketC2S = value; }

        public bool _JunkyConnectionC2S = false;
        public static bool JunkyConnectionC2S { get => Instance._JunkyConnectionC2S; set => Instance._JunkyConnectionC2S = value; }

        public static byte[] SyntheticCorruption_C2S(byte[] packetBytes)
        {
            if (Instance._TrashNextPacketC2S)
            {
                Instance._TrashNextPacketC2S = false;
            }
            else if (Instance._JunkyConnectionC2S)
            {
                /// <summary>
                /// TO-DO: rate of corruption needs to be adjustable
                /// </summary>
                if (ThreadSafeRandom.Next(0f, 1f) > 0.1)
                {
                    return packetBytes;
                }
            }
            else
            {
                return packetBytes;
            }
            for (int i = 0; i < packetBytes.Length; i++)
            {
                if (i < 20)
                {
                    continue;// keep header intact
                }
                packetBytes[i] = (byte)ThreadSafeRandom.Next(0, 255);
            }
            return packetBytes;
        }

        private bool _TrashNextPacketS2C = false;
        public static bool TrashNextPacketS2C { get => Instance._TrashNextPacketS2C; set => Instance._TrashNextPacketS2C = value; }

        private bool _JunkyConnectionS2C = false;
        public static bool JunkyConnectionS2C { get => Instance._JunkyConnectionS2C; set => Instance._JunkyConnectionS2C = value; }

        public static byte[] SyntheticCorruption_S2C(byte[] packetBytes)
        {
            if (Instance._TrashNextPacketS2C)
            {
                Instance._TrashNextPacketS2C = false;
            }
            /// <summary>
            /// TO-DO: rate of corruption needs to be adjustable
            /// </summary>
            else if (Instance._JunkyConnectionS2C)
            {
                if (ThreadSafeRandom.Next(0f, 1f) > 0.1)
                {
                    return packetBytes;
                }
            }
            else
            {
                return packetBytes;
            }
            for (int i = 0; i < packetBytes.Length; i++)
            {
                if (i < 20)
                {
                    continue;// keep header intact
                }
                packetBytes[i] = (byte)ThreadSafeRandom.Next(0, 255);
            }
            return packetBytes;
        }
    }

}
