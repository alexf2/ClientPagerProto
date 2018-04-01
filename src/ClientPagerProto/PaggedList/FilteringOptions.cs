using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ClientPagerProto.PaggedList
{
    [DataContract(Namespace = "http://www.com/")]
    public struct FilteringOptions
    {
        [DataMember(Order = 1)]
        [DefaultValue(false)]
        public bool FilterByValue { get; set; } // value/description

        [DataMember(Order = 2)]
        [DefaultValue(typeof(PagedListFilteringMode), "Contains")]
        public PagedListFilteringMode FilteringMode { get; set; }

        [DataMember(Order = 4)]
        [DefaultValue(false)]
        public bool FilterCaseSensitive { get; set; }

        [DataMember(Order = 5)]
        public object FilterValue { get; set; }

        [DataMember(Order = 6)]
        [DefaultValue(1)]
        public int MinFilterSize { get; set; }        

        [ScriptIgnore]
        [JsonIgnore]
        public bool HasFiltering { get { return FilteringMode != PagedListFilteringMode.None; } }        
    }
}