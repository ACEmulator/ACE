using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class XYZ
    {
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
                return $"{X:0.000000} {Y:0.000000} {Z:0.000000}";
            }
            set
            {
                int num = 0;
                int num2 = value?.IndexOf(' ') ?? (-1);
                if (num2 < 0)
                {
                    X = 0f;
                    Y = 0f;
                    Z = 0f;
                    return;
                }

                float result = 0f;
                float result2 = 0f;
                float result3 = 0f;
                float.TryParse(value.Substring(num, num2 - num), out result);
                num = num2 + 1;
                num2 = value.IndexOf(' ', num);
                float.TryParse(value.Substring(num, num2 - num), out result2);
                num = num2 + 1;
                float.TryParse(value.Substring(num), out result3);
                X = result;
                Y = result2;
                Z = result3;
            }
        }
    }
}
