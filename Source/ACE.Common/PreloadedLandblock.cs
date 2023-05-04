
namespace ACE.Common
{
    public class PreloadedLandblocks
    {
        /// <summary>
        /// id of landblock to preload
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// description of landblock
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// whether or not this landblock is permaloaded
        /// </summary>
        public bool Permaload { get; set; } = false;

        /// <summary>
        /// whether or not this landblock should also load adjacents, if Permaload is true, the ajacent landblocks will also be marked permaload
        /// </summary>
        public bool IncludeAdjacents { get; set; } = false;

        /// <summary>
        /// whether or not this landblock is included for preload.
        /// </summary>
        public bool Enabled { get; set; } = false;
    }
}
