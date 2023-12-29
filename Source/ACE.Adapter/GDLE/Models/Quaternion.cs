using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Quaternion
    {
        [JsonPropertyName("w")]
        public float W { get; set; } = 1f;


        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("z")]
        public float Z { get; set; }

        [JsonIgnore]
        public string Display
        {
            get
            {
                return $"{W:0.000000} {X:0.000000} {Y:0.000000} {Z:0.000000}";
            }
            set
            {
                int startIndex = 0;
                int num = value?.IndexOf(' ') ?? (-1);
                if (num < 0)
                {
                    W = 1f;
                    X = 0f;
                    Y = 0f;
                    Z = 0f;
                    return;
                }

                float result = 0f;
                float result2 = 0f;
                float result3 = 0f;
                float result4 = 0f;
                float.TryParse(value.Substring(startIndex, num), out result);
                startIndex = num + 1;
                num = value.IndexOf(' ', startIndex);
                float.TryParse(value.Substring(startIndex, num - startIndex), out result2);
                startIndex = num + 1;
                num = value.IndexOf(' ', startIndex);
                float.TryParse(value.Substring(startIndex, num - startIndex), out result3);
                startIndex = num + 1;
                float.TryParse(value.Substring(startIndex), out result4);
                W = result;
                X = result2;
                Y = result3;
                Z = result4;
            }
        }
    }
}
