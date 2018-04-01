using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using CqsDataFoundation.Query;
using Newtonsoft.Json;

namespace ClientPagerProto.PaggedList
{
    [DataContract(Namespace = "http://www.com/")]
    public struct PaggingOptions
    {
        [DataMember(Order = 1)]
        public int CurrentPageNumber;

        [DataMember(Order = 2)]
        [DefaultValue(1)]
        public int RequestedPageNumber;

        [DataMember(Order = 3)]
        public int TotalPages;

        [DataMember(Order = 4)]
        [DefaultValue(0)]
        public int PageSize; // 0 - disabled        

        [DataMember(Order = 5)]
        public int TotalRecords;

        [ScriptIgnore]
        [JsonIgnore]
        public bool HasPagging { get { return PageSize != Constants.PageSizeNoPagging; } }        
    }
}