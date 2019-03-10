using System.Runtime.Serialization;

namespace La2Skolopendra.Export
{
    [DataContract]
    public sealed class Settings
    {
        [DataMember]
        public OcrExcludeInfo ExcludeInfo { get; set; }

        [DataMember]
        public OcrRegionInfo RegionInfo { get; set; }
    }
}
