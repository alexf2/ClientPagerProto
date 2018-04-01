using CqsDataFoundation.Query;

namespace ClientPagerProto.PaggedList
{
    public interface IPagedListDataProvider
    {
        DataPage<PagedListItem> FetchPage(PagedListConfigModel model);
    }
}
