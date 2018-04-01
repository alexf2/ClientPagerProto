using System.ComponentModel;
using System.Runtime.Serialization;

namespace ClientPagerProto.PaggedList
{
    [DataContract(Namespace = "http://www.com/")]
    public struct PagedListItem
    {
        [DataMember(Order = 1)]
        public string Value;

        [DataMember(Order = 2)]
        public string Description;

        [DataMember(Order = 3)]
        [DefaultValue(false)]
        public bool Selected;
    }
}