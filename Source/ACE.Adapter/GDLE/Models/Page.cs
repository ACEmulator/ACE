using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Page
    {
        [JsonPropertyName("authorAccount")]
        public string AuthorAccount = "";

        private string pageText;

        [JsonPropertyName("authorID")]
        public uint? AuthorId { get; set; }

        [JsonPropertyName("authorName")]
        public string AuthorName { get; set; }

        [JsonPropertyName("ignoreAuthor")]
        public byte? IgnoreAutor_Binder { get; set; }

        [JsonIgnore]
        public bool? IgnoreAuthor
        {
            get
            {
                return (!IgnoreAutor_Binder.HasValue) ? null : new bool?((IgnoreAutor_Binder.Value == 0) ? true : false);
            }
            set
            {
                IgnoreAutor_Binder = ((!value.HasValue) ? null : new byte?((byte)(value.Value ? 1 : 0)));
            }
        }

        [JsonPropertyName("pageText")]
        public string PageText
        {
            get
            {
                return pageText;
            }
            set
            {
                pageText = value.Replace("\r", "");
            }
        }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
