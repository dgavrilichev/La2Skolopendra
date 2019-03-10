using System.Drawing;
using System.Runtime.Serialization;

namespace La2Skolopendra.Export
{
    [DataContract]
    public sealed class OcrRegionInfo
    {
        [DataMember]
        public Rectangle MyHp { get; set; } = new Rectangle(10, 100, 200, 50);

        [DataMember]
        public Rectangle TargetHp { get; set; } = new Rectangle(10, 200, 200, 50);
    }
}
