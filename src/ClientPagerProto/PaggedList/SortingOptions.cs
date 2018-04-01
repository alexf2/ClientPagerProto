using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ClientPagerProto.PaggedList
{
    [DataContract(Namespace = "http://www.com/")]
    public struct SortingOptions
    {
        [DataMember(Order = 1)]
        [DefaultValue(false)]
        public bool SortByValue { get; set; } // value/description

        [DataMember(Order = 2)]
        [DefaultValue(typeof(PagedListSortOrder), "None")]
        public PagedListSortOrder SortingOrder { get; set; }

        [ScriptIgnore]
        [JsonIgnore]
        public bool HasSorting { get { return SortingOrder != PagedListSortOrder.None; } }        
    }
}