using System.Drawing;
using System.Runtime.Serialization;

namespace La2Skolopendra.Export
{
    [DataContract]
    public sealed class OcrRegionInfo
    {
        [DataMember]
        public Rectangle MyHp { get; set; }

        [DataMember]
        public Rectangle TargetHp { get; set; }
    }
}
