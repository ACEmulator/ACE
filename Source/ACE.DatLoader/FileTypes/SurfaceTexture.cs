using ACE.Common;
using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SurfaceTexture)]
    public class SurfaceTexture : FileType
    {
        public int Id { get; private set; }
        public int Unknown { get; private set; }
        public byte UnknownByte { get; private set; }
        public int TextureCount { get; private set; }
        public List<uint> TextureFileIds { get; } = new List<uint>();
        public List<Texture> Textures { get; } = new List<Texture>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Unknown = reader.ReadInt32();
            UnknownByte = reader.ReadByte();
            TextureCount = reader.ReadInt32();
            if (TextureCount > 0)
            {
                TextureFileIds.Add(reader.ReadUInt32());
                if (TextureCount > 1)
                    TextureFileIds.Add(reader.ReadUInt32());
            }

            if (ConfigManager.Config == null)
                ConfigManager.Initialize(Path.GetFullPath(@"..\..\..\..\..\ACE.Server\bin\x64\Debug\netcoreapp2.0\Config.json")); // weak

            var dataFiles = new string[] {
                Path.Combine(ConfigManager.Config.Server.DatFilesDirectory, "client_portal.dat"),
                Path.Combine(ConfigManager.Config.Server.DatFilesDirectory, "client_highres.dat")
            };

            foreach (uint fileId in TextureFileIds)
            {
                foreach (var datFilPth in dataFiles)
                {
                    DatDatabase dat = new DatDatabase(datFilPth); // expensive!
                    var texFil = dat.AllFiles.FirstOrDefault(k => k.Key == fileId);
                    if (texFil.Value != null)
                    {
                        var datReader = new DatReader(datFilPth, texFil.Value.FileOffset, texFil.Value.FileSize, dat.Header.BlockSize);
                        var bytes = datReader.Buffer;
                        using (var ms = new MemoryStream(bytes))
                        {
                            var texReader = new BinaryReader(ms);
                            Texture tex = new Texture();
                            tex.Unpack(texReader);
                            Textures.Add(tex);
                        }
                        break;
                    }
                }
            }
            if (Textures.Count != TextureCount)
                throw new Exception($"Found only {Textures.Count} of {TextureCount} surface textures for SurfaceTexture {Id}");

            //// not using datmanager because highres is still unimplemented
            //if (DatManager.PortalDat == null)
            //    DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory);
            //var allDatabases = new DatDatabase[] { DatManager.PortalDat, DatManager.CellDat };
            //foreach (uint fileId in TextureFileIds)
            //{
            //    foreach (var datDb in allDatabases)
            //    {
            //        //DatDatabase dat = new DatDatabase(datFilPth);
            //        var texFil = datDb.AllFiles.FirstOrDefault(k => k.Key == fileId);
            //        if (texFil.Value != null)
            //        {
            //            var datReader = datDb.GetReaderForFile(fileId);
            //            //var datReader = new DatReader(datFilPth, texFil.Value.FileOffset, texFil.Value.FileSize, dat.Header.BlockSize);
            //            var bytes = datReader.Buffer;
            //            using (var ms = new MemoryStream(bytes))
            //            {
            //                var texReader = new BinaryReader(ms);
            //                Texture tex = new Texture();
            //                tex.Unpack(texReader);
            //                Textures.Add(tex);
            //            }
            //            break;
            //        }
            //    }
            //}
        }
    }
}
