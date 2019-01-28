namespace ACE.WebApiServer.Model
{
    public class NetworkStatsResponseModel
    {
        public long C2S_RequestsForRetransmit_Aggregate { get; set; }
        public long S2C_RequestsForRetransmit_Aggregate { get; set; }
        public long C2S_CRCErrors_Aggregate { get; set; }
        public long C2S_Packets_Aggregate { get; set; }
        public long S2C_Packets_Aggregate { get; set; }
        public string Summary { get; set; }
    }
}
