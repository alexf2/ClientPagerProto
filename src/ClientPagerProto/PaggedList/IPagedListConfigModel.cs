using System.Collections.Generic;
using CqsDataFoundation.Query;

namespace ClientPagerProto.PaggedList
{
    public interface IPagedListConfigModel
    {
        string ModelId { get; }

        IList<PagedListItem> Items { get; }
        IEnumerable<string> GetSelected();
        void Select(IEnumerable<string> ids);

        void ClearData();

        void MergePage(DataPage<PagedListItem> page, bool addAllElement);

        bool HasData { get; }
    }
}
