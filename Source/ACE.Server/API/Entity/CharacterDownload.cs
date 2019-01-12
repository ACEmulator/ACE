using System;

namespace ACE.Server.API.Entity
{
    public class CharacterDownload
    {
        public bool Valid { get; set; } = false;
        public string FilePath { get; set; } = null;
        public Action UploadCompleted { get; set; } = null;
    }
}
