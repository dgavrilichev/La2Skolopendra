using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace La2Skolopendra.Export
{
    [DataContract]
    public sealed class SkSettings
    {
        private const string FileName = "SkSettings.sts";

        [DataMember]
        public OcrExcludeInfo ExcludeInfo { get; set; } = new OcrExcludeInfo();

        [DataMember]
        public OcrRegionInfo RegionInfo { get; set; } = new OcrRegionInfo();

        internal void Save()
        {
            var serializer = new NetDataContractSerializer();

            using (var file = File.Create(FileName))
            using (var stream = new GZipStream(file, CompressionMode.Compress))
            {
                serializer.Serialize(stream, this);
            }
        }

        internal void Load()
        {
            try
            {
                var serializer = new NetDataContractSerializer();
                using (var file = File.OpenRead(FileName))
                using (var stream = new GZipStream(file, CompressionMode.Decompress))
                {
                    var settings = (SkSettings)serializer.Deserialize(stream);
                    ExcludeInfo = settings.ExcludeInfo;
                    RegionInfo = settings.RegionInfo;
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
