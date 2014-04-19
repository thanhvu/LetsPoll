using System;
using System.Runtime.Serialization;

namespace LetsPoll
{
    [DataContract]
    public class IPResult
    {
        [DataMember(Name = "ip")]
        public string IP
        {
            get;
            set;
        }
    }
}
