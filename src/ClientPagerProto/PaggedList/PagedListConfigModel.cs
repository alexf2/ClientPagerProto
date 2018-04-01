using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using CqsDataFoundation.Query;
using Newtonsoft.Json;


namespace ClientPagerProto.PaggedList
{
    [DataContract(Namespace = "http://www.com/")]
    public sealed class PagedListConfigModel : IPagedListConfigModel
    {
        public const string AllValue = "__all__";
        public const string AllText = "[All]";

        #region Constructors
        //solely for JSON serialization suppport (SFJ)
        public PagedListConfigModel()
        {
            Filtering.MinFilterSize = 1;
            Filtering.FilteringMode = PagedListFilteringMode.Contains;

            Pagging.PageSize = Constants.PageSizeNoPagging;
            Pagging.CurrentPageNumber = Pagging.RequestedPageNumber = 1;
            Pagging.TotalPages = 1;
        }

        public PagedListConfigModel(string modelId)
            : this()
        {
            ModelId = modelId;
            _items = new List<PagedListItem>();
        }
        #endregion Constructors


        [DataMember(Order = 1)]
        public string ModelId { get; private set; }

        #region List presentation and behaviour Options
        [DataMember(Order = 2)]
        public SortingOptions Sorting;

        [DataMember(Order = 3)]
        public FilteringOptions Filtering;

        [DataMember(Order = 4)]
        public PaggingOptions Pagging;
        #endregion List presentation and behaviour Options

        /* Pagged data requesting */
        [DataMember(Order = 5)]
        public string DataServiceUrl { get; set; }

        #region Data
        IList<PagedListItem> _items;
        [DataMember(Order = 6)]
        public IList<PagedListItem> Items
        {
            get { return _items; }
            private set { _items = value; }
        }

        [DataMember(Order = 7)]
        public string Tag { get; set; }

        [ScriptIgnore]
        [JsonIgnore]
        public bool HasData { get { return _items != null && _items.Count > 0; } }

        public void SetItems(IList<PagedListItem> items, int page = 1, int totalPages = 1, int totalRecords = 0, bool addAllElement = true)
        {
            Items = items;
            Pagging.CurrentPageNumber = page;
            Pagging.TotalPages = totalPages;
            Pagging.TotalRecords = totalRecords == 0 ? items.Count : totalRecords;

            if (addAllElement && items.Count > 1 && items[0].Value != AllValue && page == 1)
                items.Insert(0, new PagedListItem() { Value = AllValue, Description = AllText });
        }

        public void MergePage(DataPage<PagedListItem> page, bool addAllElement)
        {
            SetItems(page.Data, page.PageNumber, page.TotalPages, page.TotalRecordsCount, addAllElement);
        }

        public void SetEmptyPageOfVariables()
        {
            SetItems(new List<PagedListItem>(), 1, 0, 0, false);
        }

        public IEnumerable<string> GetSelected()
        {
            if (_items == null || _items.Count == 0)
                yield break;

            foreach (var item in _items.Where(item => item.Selected))
                yield return item.Value;
        }

        public void Select(IEnumerable<string> ids)
        {
            if (_items == null || _items.Count == 0)
                return;

            if (ids == null)
                ids = new List<string>();

            var tmp = new HashSet<string>(ids, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < _items.Count; ++i)
            {
                var t = _items[i];
                t.Selected = tmp.Contains(t.Value);
                _items[i] = t;
            }
        }

        public void ClearData()
        {
            _items = null;
        }

        public DataPage<PagedListItem> ToDataPage()
        {
            return new DataPage<PagedListItem>()
            {
                Data = _items,
                TotalPages = Pagging.TotalPages,
                PageNumber = Pagging.CurrentPageNumber,
                TotalRecordsCount = Pagging.TotalRecords
            };
        }
        #endregion Data
    }
    
}

