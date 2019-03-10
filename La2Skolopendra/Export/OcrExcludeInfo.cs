using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace La2Skolopendra.Export
{
    [DataContract]
    public sealed class OcrExcludeInfo
    {
        [DataMember]
        public List<Rectangle> Data { get; set; } = new List<Rectangle>();
    }
}
