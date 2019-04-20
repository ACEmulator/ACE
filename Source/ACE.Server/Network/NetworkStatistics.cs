using ACE.Server.WorldObjects;
using ACE.Common.Extensions;
using System;
using System.Threading;


namespace ACE.Server.Network
{
    public sealed class NetworkStatistics
    {
        private static readonly Lazy<NetworkStatistics> lazy =
            new Lazy<NetworkStatistics>(() => new NetworkStatistics());

        public static NetworkStatistics Instance => lazy.Value;

        private NetworkStatistics() { }


        private long _C2S_RequestsForRetransmit_Aggregate = 0;
        /// <summary>
        /// aggregate client to server requests for retransmit
        /// </summary>
        public static long C2S_RequestsForRetransmit_Aggregate => Instance._C2S_RequestsForRetransmit_Aggregate;
        /// <summary>
        /// increment the aggregate client to server requests for retransmit
        /// </summary>
        /// <returns>the aggregate client to server requests for retransmit after incrementation</returns>
        public static long C2S_RequestsForRetransmit_Aggregate_Increment() { return Interlocked.Increment(ref Instance._C2S_RequestsForRetransmit_Aggregate); }
        // server to client
        private long _S2C_RequestsForRetransmit_Aggregate = 0;
        /// <summary>
        /// aggregate server to client requests for retransmit
        /// </summary>
        public static long S2C_RequestsForRetransmit_Aggregate => Instance._S2C_RequestsForRetransmit_Aggregate;
        /// <summary>
        /// increment the aggregate server to client requests for retransmit
        /// </summary>
        /// <returns>the aggregate server to client requests for retransmit after incrementation</returns>
        public static long S2C_RequestsForRetransmit_Aggregate_Increment() { return Interlocked.Increment(ref Instance._S2C_RequestsForRetransmit_Aggregate); }
        private long _C2S_CRCErrors_Aggregate = 0;
        /// <summary>
        /// aggregate client to server CRC errors
        /// </summary>
        public static long C2S_CRCErrors_Aggregate => Instance._C2S_CRCErrors_Aggregate;
        /// <summary>
        /// increment the aggregate client to server CRC errors
        /// </summary>
        /// <returns>the aggregate client to server CRC errors after incrementation</returns>
        public static long C2S_CRCErrors_Aggregate_Increment() { return Interlocked.Increment(ref Instance._C2S_CRCErrors_Aggregate); }
        private long _C2S_Packets_Aggregate = 0;
        /// <summary>
        /// aggregate client to server packets
        /// </summary>
        public static long C2S_Packets_Aggregate => Instance._C2S_Packets_Aggregate;
        /// <summary>
        /// increment the aggregate client to server packets
        /// </summary>
        /// <returns>the aggregate client to server packets after incrementation</returns>
        public static long C2S_Packets_Aggregate_Increment() { return Interlocked.Increment(ref Instance._C2S_Packets_Aggregate); }
        private long _S2C_Packets_Aggregate = 0;
        /// <summary>
        /// aggregate server to client packets
        /// </summary>
        public static long S2C_Packets_Aggregate => Instance._S2C_Packets_Aggregate;
        /// <summary>
        /// increment the aggregate server to client packets
        /// </summary>
        /// <returns>the aggregate server to client packets after incrementation</returns>
        public static long S2C_Packets_Aggregate_Increment() { return Interlocked.Increment(ref Instance._S2C_Packets_Aggregate); }

        public static string Summary()
        {
            double rfr_s2c_Proportion = (Instance._S2C_Packets_Aggregate < 1) ? 0 : (double)Instance._S2C_RequestsForRetransmit_Aggregate / Instance._S2C_Packets_Aggregate;
            double rfr_c2s_Proportion = (Instance._C2S_Packets_Aggregate < 1) ? 0 : (double)Instance._C2S_RequestsForRetransmit_Aggregate / Instance._C2S_Packets_Aggregate;
            double crce_c2s_Proportion = (Instance._C2S_Packets_Aggregate < 1) ? 0 : (double)Instance._C2S_CRCErrors_Aggregate / Instance._C2S_Packets_Aggregate;

            return $@"
network statistics
packets
client=>server: {Instance._C2S_Packets_Aggregate.ToString("N0")}
server=>client: {Instance._S2C_Packets_Aggregate.ToString("N0")}
requests for retransmit
client=>server: {Instance._C2S_RequestsForRetransmit_Aggregate.ToString("N0")} {BlankZeroProportion(rfr_c2s_Proportion)}
Server=>client: {Instance._S2C_RequestsForRetransmit_Aggregate.ToString("N0")} {BlankZeroProportion(rfr_s2c_Proportion)}
CRC errors
client=>server: {Instance._C2S_CRCErrors_Aggregate.ToString("N0")} {BlankZeroProportion(crce_c2s_Proportion)}
".Replace("\r\n", "\n");
        }

        private static string BlankZeroProportion(double r)
        {
            return (r == 0) ? string.Empty : r.FormatChance();
        }
    }
}
