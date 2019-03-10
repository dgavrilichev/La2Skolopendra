using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace La2Skolopendra.Export
{
    [DataContract]
    public sealed class Settings
    {
        private const string FileName = "Settings.sts";

        [DataMember]
        public OcrExcludeInfo ExcludeInfo { get; set; }

        [DataMember]
        public OcrRegionInfo RegionInfo { get; set; }

        internal void Save()
        {
            var serializer = new NetDataContractSerializer();

            using (var file = File.Create(FileName))
            using (var stream = new GZipStream(file, CompressionMode.Compress))
            {
                serializer.Serialize(stream, this);
            }
        }

        internal void Load(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var serializer = new NetDataContractSerializer();

            using (var file = File.OpenRead(path))
            using (var stream = new GZipStream(file, CompressionMode.Decompress))
            {
                var settings = (Settings)serializer.Deserialize(stream);
                ExcludeInfo = settings.ExcludeInfo;
                RegionInfo = settings.RegionInfo;
            }
        }
    }
}
