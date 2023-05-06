using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class BodyPart
    {
        [JsonPropertyName("dtype")]
        public int DType { get; set; }


        [JsonPropertyName("dval")]
        public int DVal { get; set; }

        [JsonPropertyName("dvar")]
        public float DVar { get; set; }

        [JsonPropertyName("acache")]
        public ArmorValues ArmorValues { get; set; }

        [JsonPropertyName("bh")]
        public int BH { get; set; }

        [JsonPropertyName("bpsd")]
        public Zones SD { get; set; }


        public BodyPart()
        {
            ArmorValues = new ArmorValues();
            SD = new Zones();
        }
    }
}
