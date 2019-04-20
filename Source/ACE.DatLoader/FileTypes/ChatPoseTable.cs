using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.ChatPoseTable)]
    public class ChatPoseTable : FileType
    {
        internal const uint FILE_ID = 0x0E000007;

        // Key is a emote command, value is the state you are enter into
        public Dictionary<string, string> ChatPoseHash = new Dictionary<string, string>();

        // Key is the state, value are the strings that players see during the emote
        public Dictionary<string, ChatEmoteData> ChatEmoteHash = new Dictionary<string, ChatEmoteData>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            var totalObjects = reader.ReadUInt16();
            reader.ReadUInt16(); // var bucketSize
            for (int i = 0; i < totalObjects; i++)
            {
                string key = reader.ReadPString(); reader.AlignBoundary();
                string value = reader.ReadPString(); reader.AlignBoundary();
                ChatPoseHash.Add(key, value);
            }

            var totalEmoteObjects = reader.ReadUInt16();
            reader.ReadUInt16();// var bucketSize
            for (int i = 0; i < totalEmoteObjects; i++)
            {
                string key = reader.ReadPString(); reader.AlignBoundary();
                ChatEmoteData value = new ChatEmoteData();
                value.Unpack(reader);
                ChatEmoteHash.Add(key, value);
            }
        }
    }
}
